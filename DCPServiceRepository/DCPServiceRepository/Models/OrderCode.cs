using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCPServiceRepository.Models
{
    public class OrderCode
    {
        [Key]
        public string Key { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public System.DateTime FinishTime { get; set; }
    }

}
