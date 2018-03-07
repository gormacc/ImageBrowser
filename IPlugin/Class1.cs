using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IPlugin
{
    public interface IPlugin
    {
        string Name { get; }
        BitmapImage Do(BitmapImage image);
    }
}
