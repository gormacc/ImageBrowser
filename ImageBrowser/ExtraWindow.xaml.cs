using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace ImageBrowser
{
    /// <summary>
    /// Interaction logic for ExtraWindow.xaml
    /// </summary>
    public partial class ExtraWindow : Window
    {
        private List<IPlugin.IPlugin> _plugins = new List<IPlugin.IPlugin>();


        private BitmapImage bmpImage;
        public ExtraWindow(ImageBrowser.MainWindow.Picture picture , List<IPlugin.IPlugin> plugins)
        {
            InitializeComponent();
            insertName.Content = picture.name;
            insertHeight.Content = string.Format("{0}px", picture.height);
            insertWidth.Content = string.Format("{0}px", picture.width);
            insertCreationDate.Content = picture.creationDate;
            myImage.Source = picture.image;
            bmpImage = picture.image;

            _plugins = new List<IPlugin.IPlugin>(plugins);

            foreach (var plug in _plugins)
            {
                myComboBox.Items.Add(plug.Name);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Width = bmpImage.Width + 200;
            this.Height = bmpImage.Height;
            if (Width + 100 > SystemParameters.PrimaryScreenWidth)
                Width = SystemParameters.PrimaryScreenWidth - 100;
            if (Height + 100 > SystemParameters.PrimaryScreenHeight)
                Height = SystemParameters.PrimaryScreenHeight - 100;
        }


        private void saveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Image files (*.gif;*.jpg; *.png)|*.gif;*.jpg;*.png | All files (*.*) | *.*";
            saveFileDialog1.Title = "Save an image file";

            try
            {
                if(saveFileDialog1.ShowDialog() == true)
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmpImage));

                    using (var fileStream = new System.IO.FileStream(saveFileDialog1.FileName, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
            }
        }

        private void exitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            myImage.Source = bmpImage;
        }

        private void rotateClick(object sender, RoutedEventArgs e)
        {
            string name = "";
            try
            {
                name = (string)myComboBox.SelectedItem;
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show("Nie mozna wczytac pluginu , szczegoly : " + ex.Message);
            }

            foreach(var plug in _plugins)
            {
                if(plug.Name == name)
                {
                    myImage.Source = plug.Do(bmpImage);
                    break;
                }
            }
        }
    }
}
