using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Models
{
    public class SettingsModel
    {
        public string Theme { get; set; }
        public bool CancelInListView { get; set; }
        public bool AlwaysSaveLogin { get; set; }
        public bool AskBeforeLogout { get; set; }
    }

}
