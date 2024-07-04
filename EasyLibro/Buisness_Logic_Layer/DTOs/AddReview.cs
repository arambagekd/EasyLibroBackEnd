using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness_Logic_Layer.DTOs
{
    public class AddReview
    {
        public string Description { get; set; }
        public string ISBN { get; set; }

        public int Stars {  get; set; }
    }
}
