using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp4.Models
{
     class MySqlDataProvider: IDataProvider
    {
        private MySqlConnection Connection;

        public MySqlDataProvider()
        {
            try
            {
                Connection = new MySqlConnection(
                    "Server=kolei.ru;Database=aegoshin;port=3306;UserId=aegoshin;password=090903;");
            }
            catch (Exception)
            {
            }
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> ProductList = new List<Product>();
            string Query = @"SELECT 
            p.*,
            pt.Title AS ProductTypeTitle,
            pp.MaterialList, pp.Total
        FROM
            Product p
        LEFT JOIN
            ProductType pt ON p.ProductTypeID = pt.ID
        LEFT JOIN
            (
            SELECT
                pm.ProductID,
                GROUP_CONCAT(m.Title SEPARATOR ', ') as MaterialList, 
                SUM(pm.Count * m.Cost / m.CountInPack) as Total
            FROM
                Material m,
	            ProductMaterial pm
            WHERE m.ID = pm.MaterialID
            GROUP BY ProductID
            ) pp ON pp.ProductID = p.ID";

            try
            {
                // открываем соединение с сервером
                Connection.Open();
                try
                {
                    // создаем команду
                    MySqlCommand Command = new MySqlCommand(Query, Connection);
                    // получаем результат команды (массив строк)
                    MySqlDataReader Reader = Command.ExecuteReader();

                    // перебираем стоки
                    while (Reader.Read())
                    {
                        // создаем экземпляр класса 
                        Product NewProduct = new Product();
                        // и заполняем его поля
                        NewProduct.ID = Reader.GetInt32("ID");
                        NewProduct.Title = Reader.GetString("Title");
                        NewProduct.ProductTypeID = Reader.GetInt32("ProductTypeID");
                        NewProduct.ArticleNumber = Reader.GetString("ArticleNumber");
                        NewProduct.ProductionPersonCount = Reader.GetInt32("ProductionPersonCount");
                        NewProduct.ProductionWorkshopNumber = Reader.GetString("ProductionWorkshopNumber");
                        NewProduct.MinCostForAgent = Reader.GetInt32("MinCostForAgent");

                        // Методы Get<T> не поддерживают работу с NULL
                        // для полей, в которых может встретиться NULL (а лучше для всех)
                        // используйте следующий синтаксис
                        NewProduct.Description = Reader["Description"].ToString();
                        NewProduct.Image = Reader["Image"].ToString();

                        NewProduct.ProductTypeTitle = Reader["ProductTypeTitle"].ToString();
                        NewProduct.MaterialString = Reader["MaterialList"].ToString();
                        NewProduct.Total = Reader["Total"].ToString();

                        // добавляем экземпляр класса в список продуктов
                        ProductList.Add(NewProduct);
                    }
                }
                finally
                {
                    // обязательно закрываем соединение
                    // ресурсы сервера конечны
                    Connection.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return ProductList;
        }
        public IEnumerable<ProductType> GetProductTypes()
        {
            List<ProductType> productTypeList = new List<ProductType>();
            string Query = "SELECT * FROM ProductType";

            try
            {
                Connection.Open();
                try
                {
                    MySqlCommand Command = new MySqlCommand(Query, Connection);
                    MySqlDataReader Reader = Command.ExecuteReader();

                    while (Reader.Read())
                    {
                        ProductType NewProductType = new ProductType();
                        NewProductType.ID = Reader.GetInt32("ID");
                        NewProductType.Title = Reader.GetString("Title");

                        productTypeList.Add(NewProductType);
                    }
                }
                finally
                {
                    Connection.Close();
                }
            }
            catch (Exception)
            {
            }

            return productTypeList;

        }
    }
}
