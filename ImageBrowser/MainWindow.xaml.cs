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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace ImageBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        private System.Collections.ObjectModel.ObservableCollection<Picture> _pictures = new System.Collections.ObjectModel.ObservableCollection<Picture>();
        private object dummyNode = null;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void AboutClickEvent(object sender, RoutedEventArgs e) // About
        {
            System.Windows.MessageBox.Show("Simple Image Browser", "About", MessageBoxButton.OK);
        }

        private void ExitClickEvent(object sender, RoutedEventArgs e) // Exit
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = _pictures;

            foreach (string s in Directory.GetLogicalDrives())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = s;
                item.Tag = s;
                item.FontWeight = FontWeights.Normal;
                item.Items.Add(dummyNode);
                item.Expanded += new RoutedEventHandler(folder_Expanded);
                drzewko.Items.Add(item);
            }

        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItem subitem = new TreeViewItem();
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                }
                catch (Exception) { }
            }
        }

        public class Picture
        {
            public BitmapImage image { get; set; }
            public string name { get; set; }
            public string creationDate { get; set; }
            public double height { get; set; }
            public double width { get; set; }
        }

        private void OpenImageEvent(object sender, RoutedEventArgs e)
        {
            Stream myStream = null;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.Filter = "Image files (*.gif;*.jpg; *.png)|*.gif;*.jpg;*.png | All files (*.*) | *.*";
            openFileDialog1.Title = "Open an image file";

            openFileDialog1.ShowDialog();
            try
            {
                string ext = System.IO.Path.GetExtension(openFileDialog1.FileName);
                if (ext != ".jpg" && ext != ".gif" && ext != ".png")
                {
                    System.Windows.MessageBox.Show("Error: Could not open file.");
                    return;
                }
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(openFileDialog1.FileName));
                    Picture picture = new Picture
                    {
                        image = bitmap,
                        name = openFileDialog1.SafeFileName,
                        creationDate = File.GetCreationTime(openFileDialog1.FileName).ToString() ,
                        height = bitmap.PixelHeight,
                        width = bitmap.PixelWidth
                    };
                    ExtraWindow window = new ExtraWindow(picture , PluginsManager.Instance._plugins);
                    window.Show();
                }

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: Could not read file. Original error: " + ex.Message);
            }
        }

        private void OpenFolderEvent(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = folderDialog.ShowDialog();

            try
            {
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    if (folderDialog.SelectedPath != null)
                    {
                        _pictures.Clear();
                        foreach (string file in Directory.GetFiles(folderDialog.SelectedPath))
                        {
                            string ext = System.IO.Path.GetExtension(file);
                            if (ext == ".jpg" || ext == ".gif" || ext == ".png")
                            {
                                BitmapImage bitmap = new BitmapImage(new Uri(file));

                                this._pictures.Add(new Picture
                                {
                                    image = bitmap,
                                    name = System.IO.Path.GetFileName(file),
                                    creationDate = File.GetCreationTime(file).ToString(),
                                    height = bitmap.PixelHeight,
                                    width = bitmap.PixelWidth
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: Could not read folder. Original error: " + ex.Message);
            }
            
        }

        private void DoubleClick(object sender, MouseButtonEventArgs e) 
        {
            System.Windows.Controls.Button sp = sender as System.Windows.Controls.Button;
            Picture pic = sp.DataContext as Picture;
            ExtraWindow window = new ExtraWindow(pic, PluginsManager.Instance._plugins);
            window.Show();
        }

        private void drzewko_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem temp = (TreeViewItem)e.NewValue;
            if (temp == null)
                return;

            string path;
            path = "";
            string temp1 = "";
            string temp2 = "";
            while (true)
            {
                temp1 = temp.Header.ToString();
                if (temp1.Contains(@"\"))
                {
                    temp2 = "";
                }
                path = temp1 + temp2 + path;
                if (temp.Parent.GetType().ToString() == "System.Windows.Controls.TreeView")
                {
                    break;
                }
                temp = ((TreeViewItem)temp.Parent);
                temp2 = @"\";
            }

            try
            {
                if (path != null)
                {
                    _pictures.Clear();
                    foreach (string file in Directory.GetFiles(path))
                    {
                        string ext = System.IO.Path.GetExtension(file);
                        if (ext == ".jpg" || ext == ".gif" || ext == ".png")
                        {
                            BitmapImage bitmap = new BitmapImage(new Uri(file));

                            this._pictures.Add(new Picture
                            {
                                image = bitmap,
                                name = System.IO.Path.GetFileName(file),
                                creationDate = File.GetCreationTime(file).ToString(),
                                height = bitmap.PixelHeight,
                                width = bitmap.PixelWidth
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
