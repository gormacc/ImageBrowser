using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Rotate90
{

    public class Rotate90Plugin : IPlugin.IPlugin
    {
        public string Name
        {
            get
            {
                return "Rotate 90 Plugin";
            }
        }

        public BitmapImage Do(BitmapImage bmpOriginal)
        {
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = bmpOriginal.UriSource;
            bmp.Rotation = Rotation.Rotate90;
            bmp.EndInit();

            return bmp;
        }
    }
}
