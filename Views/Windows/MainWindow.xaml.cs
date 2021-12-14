using System;
using System.Windows;

namespace Lopushok
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Конструктор окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Возврат на предыдущее окно
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.NavigationService.GoBack();
        }

        /// <summary>
        /// По загрузке данных во фрейме определяет, возможен ли возврат назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            btnBack.Visibility = mainFrame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
