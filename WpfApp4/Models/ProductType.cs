using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Models
{
    public class ProductType
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }
}
