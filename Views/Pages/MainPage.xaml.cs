using Lopushok.Model;
using Lopushok.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Lopushok.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private const int productsPerPage = 20;
        private int selectedPage = 1;
        private const int pageNumbers = 4;
        private int productCount;

        /// <summary>
        /// Конструктор страницы
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Читает типы продуктов в БД
        /// </summary>
        /// <returns>List типов</returns>
        private List<ProductType> GetTypes()
        {
            var typesList = DB.entities.ProductTypes.ToList();
            return typesList;
        }

        /// <summary>
        /// Читает продукцию в БД
        /// </summary>
        /// <param name="search">Строка поиска</param>
        /// <param name="sort">Сортировка</param>
        /// <param name="filter">Фильрация</param>
        /// <returns>Список продукции с учетом фильтров, сортировки и поиска</returns>
        private List<Product> GetProducts(string search = "", string sort = "", string filter = "Все типы")
        {
            var productsList = DB.entities.Products.ToList();
            switch (sort)
            {
                case "По наименованию, от А до Я":
                    {
                        productsList = productsList.OrderBy(x => x.Title).ToList();
                        break;
                    }
                case "По наименованию, от Я до А":
                    {
                        productsList = productsList.OrderByDescending(x => x.Title).ToList();
                        break;
                    }
                case "По возрастанию номера производственного цеха":
                    {
                        productsList = productsList.OrderBy(x => x.ProductionWorkshopNumber).ToList();
                        break;
                    }
                case "По убыванию номера производственного цеха":
                    {
                        productsList = productsList.OrderByDescending(x => x.ProductionWorkshopNumber).ToList();
                        break;
                    }
                case "По возрастанию минимальной стоимости для агента":
                    {
                        productsList = productsList.OrderBy(x => x.MinCostForAgent).ToList();
                        break;
                    }
                case "По убыванию минимальной стоимости для агента":
                    {
                        productsList = productsList.OrderByDescending(x => x.MinCostForAgent).ToList();
                        break;
                    }

                default:
                    break;
            }
            if (filter != "Все типы" && !string.IsNullOrEmpty(filter))
            {
                productsList = productsList.Where(x => x.ProductType.Title == filter).ToList();
            }
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // TODO: description
                productsList = productsList.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
            }
            productCount = productsList.Count;
            if (productCount == 0)
            {
                return null;
            }
            CreatePages();
            productsList = CreatePaginatedList(productsList)[selectedPage - 1];
            return productsList;
        }

        /// <summary>
        /// Создает список продукции, разбивая ее по страницам
        /// </summary>
        /// <param name="products">Список продукции</param>
        /// <returns>Разбитый на страницы список продукции</returns>
        private Dictionary<int, List<Product>> CreatePaginatedList(List<Product> products)
        {
            Dictionary<int, List<Product>> paginatedList = new Dictionary<int, List<Product>>();
            int productCount = 0;
            for (int i = 0; i <= products.Count / productsPerPage; i++)
            {
                var productsOnPage = new List<Product>();
                for (int j = 0; j < productsPerPage; j++)
                {
                    if (productCount >= products.Count)
                    {
                        break;
                    }
                    productsOnPage.Add(products[productCount]);
                    productCount++;
                }
                if (productsOnPage.Count == 0)
                {
                    break;
                }
                paginatedList.Add(i, productsOnPage);
            }
            return paginatedList;
        }

        /// <summary>
        /// Создает номера страниц
        /// </summary>
        private void CreatePages()
        {
            spPages.Children.Clear();
            var tbPrev = new TextBlock
            {
                Text = " < ",
                FontSize = 30,
                Margin = new Thickness(5)
            };
            tbPrev.MouseLeftButtonDown += TbPrev_MouseLeftButtonDown;
            spPages.Children.Add(tbPrev);
            var pagesIterator = selectedPage;
            for (int i = 0; i < pageNumbers; i++)
            {
                if (pagesIterator > productCount / productsPerPage)
                {
                    break;
                }
                var tbPage = new TextBlock
                {
                    Text = pagesIterator.ToString(),
                    FontSize = 30,
                    Margin = new Thickness(5),
                };
                if (pagesIterator == selectedPage)
                {
                    tbPage.TextDecorations = TextDecorations.Underline;
                }
                tbPage.MouseLeftButtonDown += TbPage_MouseLeftButtonDown;
                spPages.Children.Add(tbPage);
                pagesIterator++;
            }
            var tbNext = new TextBlock
            {
                Text = " > ",
                FontSize = 30,
                Margin = new Thickness(5)
            };
            tbNext.MouseLeftButtonDown += TbNext_MouseLeftButtonDown;
            spPages.Children.Add(tbNext);
        }

        /// <summary>
        /// По клику увеличивает номер страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbNext_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedPage + 1 < productCount / productsPerPage)
            {
                selectedPage++;
                dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
            }
        }

        /// <summary>
        /// По клику меняет номер страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedPage = Convert.ToInt32((sender as TextBlock).Text);
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
        }

        /// <summary>
        /// По клику уменьшает номер страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPrev_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedPage > 1)
            {
                selectedPage--;
                dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
            }
        }

        /// <summary>
        /// Загружает данные в элементы страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbSort.SelectedIndex = 0;
            cbFilter.Items.Add("Все типы");
            foreach (var item in GetTypes())
            {
                cbFilter.Items.Add(item.Title);
            }
            cbFilter.SelectedIndex = 0;
            DB.entities.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            dgProducts.ItemsSource = GetProducts();
        }

        /// <summary>
        /// При изменении строки поиска выводит подходящую продукцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, cbFilter.Text);
        }

        /// <summary>
        /// Сортирует продукцию по выьранному параметру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, ((ComboBoxItem)cbSort.SelectedValue).Content.ToString(), cbFilter.Text);
        }

        /// <summary>
        /// При изменении фильтра выводит подходящую продукцию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedPage = 1;
            dgProducts.ItemsSource = GetProducts(tbSearch.Text, cbSort.Text, (cbFilter.SelectedValue).ToString());
        }

        /// <summary>
        /// По двойному клику на продукт переходит на форму изменения этого продукта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgProducts.SelectedItem is Product)
            {
                NavigationService.Navigate(new AddEditPage(dgProducts.SelectedItem as Product));
            }
        }

        /// <summary>
        /// По нажатию кнопки "Добавить" переходит на форму добавления продукта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage(new Product()));
        }

        /// <summary>
        /// Выделение нескольких элементов в списке продукции, после чего появляется кнопка “Изменить стоимость на...”
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnMinEdit.Visibility = dgProducts.SelectedItems.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Изменение минимальной стоимости продукции для агента сразу для нескольких выбранных продуктов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMinEdit_Click(object sender, RoutedEventArgs e)
        {
            // а как вывести в интерфейс
            var products = new List<Product>();
            foreach (var item in dgProducts.SelectedItems)
            {
                products.Add(item as Product);
            }
            var window = new EditCostWindow(products);
            window.ShowDialog();
        }
    }
}
