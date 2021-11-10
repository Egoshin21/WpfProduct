using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp4.Models;

namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<Product> _ProductList;

        public List<ProductType> ProductTypeList { get; set; }

        public IEnumerable<Product> ProductList
        {
            get
            {
                var Result = _ProductList;
                if (ProductTypeFilterId > 0)
                    Result = Result.Where(
                        p => p.ProductTypeID == ProductTypeFilterId);
                switch (SortType)
                {
                    case 1:
                        Result = Result.OrderBy(p => p.Title);
                        break;
                    case 2:
                        Result = Result.OrderByDescending(p => p.Title);
                        break;
                    case 3:
                        Result = Result.OrderByDescending(p => p.ProductionWorkshopNumber);
                        break;
                    case 4:
                        Result = Result.OrderBy(p => p.ProductionWorkshopNumber);
                        break;
                    case 5:
                        Result = Result.OrderByDescending(p => p.MinCostForAgent);
                        break;
                    case 6:
                        Result = Result.OrderBy(p => p.MinCostForAgent);
                        break;
                }
                if (SearchFilter != "")
                    Result = Result.Where(
                        p => p.Title.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                                p.Description.IndexOf(SearchFilter, StringComparison.OrdinalIgnoreCase) >= 0
                    );

                Paginator.Children.Clear();

                Paginator.Children.Add(new TextBlock { Text = " < " });
                for (int i = 1; i <= (Result.Count() / 20) + 1; i++)
                    Paginator.Children.Add(new TextBlock { Text = " " + i.ToString() + " " });
                Paginator.Children.Add(new TextBlock { Text = " > " });
                foreach (TextBlock tb in Paginator.Children)
                    tb.PreviewMouseDown += PrevPage_PreviewMouseDown;

                if (CurrentPage > Result.Count() / 20)
                    CurrentPage = Result.Count() / 20;

                return Result.Skip(20 * CurrentPage).Take(20);
            }
            set
            {
                _ProductList = value;
                Invalidate();
            }
        }

        private int _CurrentPage = 0;
        private int CurrentPage
        {
            get
            {
                return _CurrentPage;
            }
            set
            {
                _CurrentPage = value;
                Invalidate();
            }
        }

        private void Invalidate(string ComponentName = "ProductList")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(ComponentName));
        }

        private void PrevPage_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as TextBlock).Text)
            {
                case " < ":
                    if (CurrentPage > 0) CurrentPage--;
                    return;
                case " > ":
                    if (CurrentPage < _ProductList.Count() / 20) CurrentPage++;
                    return;
                default:
                    CurrentPage = Convert.ToInt32(
                        (sender as TextBlock).Text.Trim()) - 1;
                    return;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Globals.DataProvider = new MySqlDataProvider();
            ProductList = Globals.DataProvider.GetProducts();
            ProductTypeList = Globals.DataProvider.GetProductTypes().ToList();
            ProductTypeList.Insert(0, new ProductType { Title = "Все типы" });


        }
        public event PropertyChangedEventHandler PropertyChanged;
        public string[] SortList { get; set; } = {
             "Без сортировки",
             "название по убыванию",
             "название по возрастанию",
             "номер цеха по убыванию",
             "номер цеха по возрастанию",
             "цена по убыванию",
             "цена по возрастанию" };
        private int SortType = 0;

        private void SortTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortType = SortTypeComboBox.SelectedIndex;
            Invalidate();
        }
        private int ProductTypeFilterId = 0;

        private void ProductTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductTypeFilterId = (ProductTypeFilter.SelectedItem as ProductType).ID;
            Invalidate();
        }
        private string SearchFilter = "";
        private void SearchFilterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchFilter = SearchFilterTextBox.Text;
            Invalidate();
        }


    }


}
