using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Lopushok.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ImagesWindow.xaml
    /// </summary>
    public partial class ImagesWindow : Window
    {
        public string ImgUri { get; private set; }

        /// <summary>
        /// Конструктор окна
        /// </summary>
        public ImagesWindow()
        {
            InitializeComponent();
            ShowImages();
        }

        /// <summary>
        /// Получает изображения из папки products
        /// </summary>
        /// <returns>Массив ссылок на изображения</returns>
        private string[] GetImages()
        {
            string[] files = Directory.GetFiles("../../products");
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Remove(0, 5);
            }
            return files;
        }

        /// <summary>
        /// Выводит StackPanel с изображениями
        /// </summary>
        private void ShowImages()
        {
            const int imagesInRow = 5;

            var files = GetImages();

            var column = new StackPanel
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            for (int i = 0; i <= files.Length / imagesInRow; i++)
            {
                var row = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(0, 5, 0, 5)
                };
                for (int j = imagesInRow * i; j < (imagesInRow * i) + imagesInRow; j++)
                {
                    if (j == files.Length)
                    {
                        break;
                    }
                    var image = new Image
                    {
                        Width = 60,
                        Height = 60,
                        Margin = new Thickness(5, 0, 5, 0),
                        Source = new BitmapImage(new Uri(files[j], UriKind.Relative)),
                        Cursor = Cursors.Hand,
                        Tag = files[j]
                    };
                    image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                    row.Children.Add(image);
                }
                column.Children.Add(row);
            }
            gridImages.Children.Add(column);
        }

        /// <summary>
        /// Выбор картинки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var image = (Image)sender;
            ImgUri = image.Tag.ToString();
            DialogResult = true;
            Close();
        }
    }
}
