using AuxiliaryService.Scheduler.Contract;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Infrastructure
{
    public class SchedulerFactoryConfiguration : ISchedulerFactoryConfiguration
    {
        public NameValueCollection GetConfiguration()
        {
            var configValues = new NameValueCollection();
            return configValues;
        }
    }
}
