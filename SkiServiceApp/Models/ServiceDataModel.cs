using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Models
{
    public class ServiceDataModel
    {
        public int Id { get; set; }
        public string Priority { get; set; }
        public string Service { get; set; }
        public string Status { get; set; }
        public string RemainingDays { get; set; }
        public bool isAssigned { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime SubmissionDate { get; set; }

        public string PriorityService => $"{Priority} - {Service}";
        public string RemainingDaysAssignment => $"{RemainingDays} - {(isAssigned == false ? "Nicht zugewiesen" : "Zugewiesen")}";
        public string SubmissionDateFormatted => $"Gestellt am. {SubmissionDate:dd.MM.yyyy}";
    }
}