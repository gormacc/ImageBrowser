using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using IPlugin;

namespace ImageBrowser
{
    public class PluginsManager
    {
        public static PluginsManager Instance { get; }= new PluginsManager();

        public List<IPlugin.IPlugin> _plugins = new List<IPlugin.IPlugin>();
        public IEnumerable<IPlugin.IPlugin> Plugins => _plugins;

        public PluginsManager()
        {
            String path = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().Location);
            DirectoryInfo di = new DirectoryInfo(path);
            FileInfo[] fls = di.GetFiles("*.dll");
            foreach(FileInfo fis in fls)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(fis.FullName);
                    AddInterfaces(assembly);
                }
                catch(Exception)
                {
                    MessageBox.Show("Error loading assembly " + fis.Name);
                }
            }
        }

        private void AddInterfaces(Assembly assembly)
        {
            Type[] types = assembly.GetExportedTypes();
            foreach(var type in types)
            {
                if(type.GetInterfaces().Contains(typeof(IPlugin.IPlugin)))
                    _plugins.Add((IPlugin.IPlugin)Activator.CreateInstance(type));
            }
        }
    }
}
