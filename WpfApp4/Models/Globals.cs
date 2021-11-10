using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Models
{
    class Globals
    {
        public static IDataProvider DataProvider;
        public static IEnumerable<ProductType> ProductTypeList { get; set; }
    }
}
