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

namespace TappersWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static  MainWindow instance;

        private Cache cache;

        public MainWindow()
        {
            Cache.loadCache();

            Cache.Instance.init();

            cache = Cache.Instance;

            cache.attemptCharacterUpdates();
            cache.attemptBackgroundUpdates();


            InitializeComponent();

            instance = this;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Cache.saveCache();
        }

        private void lblTitle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }
            this.DragMove();
        }

        private void recMain_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                return;
            }
            this.DragMove();
        }

        private void cmdExit_MouseEnter(object sender, MouseEventArgs e)
        {
            cmdExit.Background = new SolidColorBrush(Color.FromRgb(232, 17, 35));
            cmdExit.Foreground = new SolidColorBrush(Colors.White);
        }

        private void cmdExit_MouseLeave(object sender, MouseEventArgs e)
        {
            cmdExit.Background = null;
            cmdExit.Foreground = new SolidColorBrush(Color.FromRgb(186, 229, 241));
        }

        private void cmdExit_MouseDown(object sender, MouseButtonEventArgs e)
        {
            cmdExit.Background = new SolidColorBrush(Color.FromRgb(255, 57, 75));
            
        }

        private void cmdExit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
