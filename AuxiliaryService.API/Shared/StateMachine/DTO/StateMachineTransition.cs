using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine.DTO
{
    public class StateMachineTransition
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Description { get; set; }
        public bool IsInitial { get; set; }
        public bool SavePoint { get; set; }

        public StateMachineTransition(string from, string to, string description = null, bool isInitial = false)
        {
            From = from;
            To = to;
            Description = description;
            IsInitial = isInitial;
            SavePoint = false;
        }
    }
}
