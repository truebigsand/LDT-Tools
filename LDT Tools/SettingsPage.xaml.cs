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
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            for (int i = 0; i < WindowBackgroundType_ComboBox.Items.Count; i++)
            {
                if ((WindowBackgroundType_ComboBox.Items[i] as ComboBoxItem)?.Tag.ToString()
                    == App.GlobalSettings.BackgroundType.ToString())
                {
                    //WindowBackgroundType_ComboBox.SelectedIndex = i;
                    WindowBackgroundType_ComboBox.SelectedItem = WindowBackgroundType_ComboBox.Items[i];
                }
            }
            EnableWindowCornerSwitch.IsChecked = App.GlobalSettings.IsRoundCornerEnabled;
        }

        private void WindowBackgroundType_Selected(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(WindowBackgroundType_ComboBox.Items.Count.ToString());
            string? backgroundTypeString = (sender as ComboBoxItem)?.Tag?.ToString();
            if (backgroundTypeString == null)
            {
                throw new DevelopmentException($"{nameof(backgroundTypeString)} is null");
            }
            App.GlobalSettings.BackgroundType = Enum.Parse<Wpf.Ui.Appearance.BackgroundType>(backgroundTypeString);
            //MessageBox.Show(App.GlobalSettings.BackgroundType.ToString());
            Wpf.Ui.Appearance.Background.Apply(App.Current.MainWindow, App.GlobalSettings.BackgroundType);
            //MessageBox.Show(backgroundType.ToString());
        }

        private void EnableWindowCornerSwitch_CheckStateChanged(object sender, RoutedEventArgs e)
        {
            App.GlobalSettings.IsRoundCornerEnabled = EnableWindowCornerSwitch.IsChecked
                ?? throw new DevelopmentException($"{nameof(EnableWindowCornerSwitch.IsChecked)} is null");
            if (Application.Current.MainWindow is not MainWindow mainWindow)
            {
                throw new DevelopmentException($"{nameof(mainWindow)} is null");
            }
            // 感谢Visual Studio 2022教我写C#
            mainWindow.MainBorder.CornerRadius = new CornerRadius(App.GlobalSettings.IsRoundCornerEnabled ? 15 : 0);
        }
    }
}
