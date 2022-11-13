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
using LDT_Tools.Utility;

namespace LDT_Tools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            if(App.Current.MainWindow is not MainWindow mainWindow)
            {
                throw new DevelopmentException("mainWindow is null");
            }
            mainWindow.MainBorder.CornerRadius = new CornerRadius(App.GlobalSettings.IsRoundCornerEnabled ? 15 : 0);
            Wpf.Ui.Appearance.Background.Apply(this, App.GlobalSettings.BackgroundType);
            Wpf.Ui.Appearance.Theme.Apply(
                App.GlobalSettings.ThemeType,
                App.GlobalSettings.BackgroundType,
                true
            );
        }

        private void NavigationButtonTheme_OnClick(object sender, RoutedEventArgs e)
        {
            Wpf.Ui.Appearance.ThemeType themeType;
            if(Wpf.Ui.Appearance.Theme.GetAppTheme() == Wpf.Ui.Appearance.ThemeType.Light)
            {
                themeType = Wpf.Ui.Appearance.ThemeType.Dark;
            }
            else
            {
                themeType = Wpf.Ui.Appearance.ThemeType.Light;
            }
            Wpf.Ui.Appearance.Theme.Apply(
                themeType,
                App.GlobalSettings.BackgroundType,
                true
            );
            App.GlobalSettings.ThemeType = themeType;

        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
