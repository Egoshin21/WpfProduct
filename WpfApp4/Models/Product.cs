using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp4.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ProductTypeID { get; set; }
        public string ArticleNumber { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int ProductionPersonCount { get; set; }
        public string ProductionWorkshopNumber { get; set; }
        public decimal MinCostForAgent { get; set; }
        public string MaterialString { get; set; }
        // данные из сводной таблицы
        public string ProductTypeTitle { get; set; }
        public string MaterialList { get; set; }
        public string Total { get; set; }
        public string LinqTitle
        {
            get
            {
                return Globals.ProductTypeList
                    .Where(t => t.ID == ProductTypeID)
                    .Select(t => t.Title)
                    .FirstOrDefault();
            }
        }
        public Uri ImagePreview
        {
            get
            {
                var imageName = Environment.CurrentDirectory + (Image ?? "");
                return System.IO.File.Exists(imageName) ? new Uri(imageName) : null;
            }
        }
        public string TypeAndName
        {
            get
            {
                return ProductTypeTitle + " | " + Title;
            }
        }

        public object CurrentProductType { get; internal set; }
    }
}
