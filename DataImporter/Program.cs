using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Castle.Windsor;
using Castle.Windsor.Installer;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.SqlServer;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Type;
using PPM.Entities.ContainerInstallers;


namespace DataImporter
{
    class Program
    {
        private static IWindsorContainer container;

        private static string ConnectionString
            => ConfigurationManager.ConnectionStrings["PPM"].ConnectionString;
        public class ConsoleInterceptor : EmptyInterceptor
        {
            public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
            {
                //                Console.WriteLine($"Saving {id}, {entity.GetType().FullName}");
                return base.OnSave(entity, id, state, propertyNames, types);
            }
        }

  

        static void Info(string format, params object[] values)
        {
            Write(ConsoleColor.White, format, values);
        }

        static void Warning(string format, params object[] values)
        {
            Write(ConsoleColor.Yellow, format, values);
        }

        static void Error(string format = null, params object[] values)
        {
            Write(ConsoleColor.Red, format ?? "Error", values);
        }

        static void Success(string format = null, params object[] values)
        {
            Write(ConsoleColor.Green, format ?? "   Success\r\n", values);
        }

        static void Write(ConsoleColor color, string format, params object[] values)
        {
            Console.ForegroundColor = color;
            Console.Write(format, values);
        }

        static void WriteLine(ConsoleColor color, string format, params object[] values)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(format, values);
        }

        static void Main(string[] args)
        {
            try
            {
                var container = new WindsorContainer();
                container.Install(FromAssembly.Containing<NHibernateConfigurationInstaller>());


                Info("Creating tables...");
                var cfg = container.Resolve<NHibernate.Cfg.Configuration>();
                cfg.SetInterceptor(new ConsoleInterceptor());
                var se = new SchemaUpdate(cfg);
                se.Execute(sql =>
                {
                    File.WriteAllText("update-database.sql", sql);
                }, doUpdate: true);

                RunMigration();

                Success(" Success\r\n");
                ExecuteSqlScripts();
            }
            catch (Exception exc)
            {
                Error(exc.ToString());
            }
            Console.Read();
        }

        private static void RunMigration()
        {

            var announcer = new TextWriterAnnouncer(Console.WriteLine) {ShowSql = true};

            var assembly = Assembly.GetAssembly(typeof(Program));
            var migrationContext = new RunnerContext(announcer);

            var options = new ProcessorOptions
            {
                PreviewOnly = false,  // set to true to see the SQL
                Timeout = 60
            };
            var factory = new SqlServer2008ProcessorFactory();
            IMigrationProcessor processor = factory.Create(ConnectionString, announcer, options);
            var runner = new MigrationRunner(assembly, migrationContext, processor);
            runner.MigrateUp(true);

            // Or go back down
            //runner.MigrateDown(0);
        }

        private static void ExecuteSqlScripts()
        {
            var builder = new SqlConnectionStringBuilder(ConnectionString);

            foreach (var sqlFile in Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scripts"), "*.sql"))
            {
                Info( $"Executing {Path.GetFileName(sqlFile)}");

                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    FileName = "SQLCMD.EXE",
                    Arguments = $"-S \"{builder.DataSource}\" -d {builder.InitialCatalog} -U {builder.UserID} -P {builder.Password} -i {sqlFile}"
                };

                var process = Process.Start(startInfo);
                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                output += process.StandardOutput.ReadToEnd();
                var lines = output.Split(new string[] {"\r\n"},StringSplitOptions.RemoveEmptyEntries);
                if (lines.Any())
                {
                Console.WriteLine();
                Warning(lines.Last());    
                }
                Success();
            }
        }
    }

    public static class DataReaderExtensions
    {
        public static string ReadString(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName];
            return (value == null || value == DBNull.Value) ? null : value.ToString();
        }

        public static int ReadInt32(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName];
            return (value == null || value == DBNull.Value) ? 0 : (int)value;
        }

        public static long ReadInt64(this IDataReader reader, string fieldName)
        {
            var value = reader[fieldName];
            return (value == null || value == DBNull.Value) ? 0 : (long)value;
        }
    }

}
