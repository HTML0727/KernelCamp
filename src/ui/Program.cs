using System;
using Gtk;

namespace KernelCamp
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();
            
            // 设置应用程序信息
            GLib.Application.Init("kernel-camp", GLib.ApplicationFlags.None);
            
            // 创建主窗口
            var win = new MainWindow();
            win.ShowAll();
            
            Application.Run();
        }
    }
}