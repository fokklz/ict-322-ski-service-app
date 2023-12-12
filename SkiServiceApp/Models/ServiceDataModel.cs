using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Models
{
    public class ServiceDataModel
    {
        public string Priority { get; set; }
        public string Service { get; set; }
        public string RemainingDays { get; set; }
        public string IsAssigned { get; set; }

        public string PriorityService => $"{Priority} - {Service}";
        public string RemainingDaysAssignment => $"{RemainingDays} - {IsAssigned}";
    }
}
