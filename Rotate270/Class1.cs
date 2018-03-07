using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rotate270
{
    

    public class Rotate270Plugin : IPlugin.IPlugin
    {
        public string Name
        {
            get
            {
                return "Rotate 270 Plugin";
            }
        }

        public BitmapImage Do(BitmapImage bmpOriginal)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = bmpOriginal.UriSource;
            bmp.Rotation = Rotation.Rotate270;
            bmp.EndInit();

            return bmp;
        }
    }
}
