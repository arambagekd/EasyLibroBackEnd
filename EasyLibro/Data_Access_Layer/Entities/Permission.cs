using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Entities
{
    public class Permission
    {
        [Key]
        public string userName {  get; set; }
        public bool permission { get; set; }
    }
}
