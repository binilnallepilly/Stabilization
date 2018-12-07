using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCPServiceRepository.Models
{
    public class ConfigCallLog
    {
        [Key]
        public Guid CallId { get; set; }
        public string Key { get; set; }
        public DateTime RequestStartTime { get; set; }
        public string Status { get; set; }
        public long ElapsedTime { get; set; }
        public DateTime FinishTime { get; set; }
        public DateTime Expirytime { get; set; }
    }

}
