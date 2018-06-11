using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Messaging;

namespace PPM.Commands.Report
{
    public class GenerateDailyReportCommand : Command
    {
        public DateTime GenerateStartTime { get; set; }
    }
}
