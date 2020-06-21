using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine.DTO
{
    public class StateMachineTransitionRunInfo
    {
        public Guid RunId { get; set; }
        public StateMachineTransition Transition { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
        public int OrderNum { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
