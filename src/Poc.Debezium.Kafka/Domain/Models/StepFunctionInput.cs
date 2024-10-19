using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class StepFunctionInput
    {
        public string TipoOperacao { get; set; }
        public int? GroupId { get; set; }
        public string StateMachineArn { get; set; }
    }
}
