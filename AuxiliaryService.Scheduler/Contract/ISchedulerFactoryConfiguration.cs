using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Contract
{
    public interface ISchedulerFactoryConfiguration
    {
        NameValueCollection GetConfiguration();
    }
}
