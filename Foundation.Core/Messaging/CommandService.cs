using System;
using System.Transactions;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Foundation.Windsor;

namespace Foundation.Messaging
{
    public class CommandService : ICommandService
    {
        private readonly IWindsorContainer _container;

        public CommandService(IWindsorContainer container)
        {
            _container = container;
        }

        public virtual void Execute<TCommand>(TCommand command) where TCommand : ICommand
        {
            using (var scope = _container.BeginScope())
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        ExecuteSingleCommand(command);
                        transaction.Complete();
                    }
                    catch (Exception exc)
                    {
                        throw;
                    }
                }
            }
        }

        public virtual TCommandResult ExecuteFoResult<TCommandResult>(ICommand<TCommandResult> command)
        {
            TCommandResult commandResult;
            using (var scope = _container.BeginScope())
            {
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        commandResult = ExecuteSingleCommand(command);
                        transaction.Complete();
                    }
                    catch (Exception exc)
                    {
                        throw;
                    }
                }
            }
            return commandResult;
        }

        public void ExecuteSingleCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = _container.Resolve<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }

        public TCommandResult ExecuteSingleCommand<TCommandResult>(ICommand<TCommandResult> command)
        {
            var commandType = command.GetType();
            var resultType = typeof(TCommandResult);
            var handlerInterfaceType = typeof(ICommandHandler<,>).MakeGenericType(commandType, resultType);

            var handler = _container.Resolve(handlerInterfaceType);
            return ((dynamic)handler).Handle((dynamic)command);
        }

        public void ExecuteBatchCommands(params ICommand[] commands)
        {
            using (var scope = _container.BeginScope())
            {
                using (var transaction = Transaction.Current == null ? new TransactionScope() : new TransactionScope(Transaction.Current))
                {
                    try
                    {
                        foreach (var command in commands)
                        {
                            ExecuteSingleCommand((dynamic)command);
                        }
                    }
                    catch (Exception exc)
                    {
                        // TODO: Write log
                        throw;
                    }

                    transaction.Complete();
                }
            }
        }
    }
}