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

namespace LDT_Tools
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        private List<ComboBoxItem> ComboBoxItems;
        public SettingsPage()
        {
            InitializeComponent();
            for (int i = 0; i < WindowBackgroundType_ComboBox.Items.Count; i++)
            {
                if ((WindowBackgroundType_ComboBox.Items[i] as ComboBoxItem).Tag.ToString()
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
            App.GlobalSettings.BackgroundType = Enum.Parse<Wpf.Ui.Appearance.BackgroundType>((sender as ComboBoxItem).Tag.ToString());
            //MessageBox.Show(App.GlobalSettings.BackgroundType.ToString());
            Wpf.Ui.Appearance.Background.Apply(App.Current.MainWindow, App.GlobalSettings.BackgroundType);
            //MessageBox.Show(backgroundType.ToString());
        }

        private void EnableWindowCornerSwitch_CheckStateChanged(object sender, RoutedEventArgs e)
        {
            App.GlobalSettings.IsRoundCornerEnabled = (bool)EnableWindowCornerSwitch.IsChecked;
            (App.Current.MainWindow as MainWindow).MainBorder.CornerRadius = new CornerRadius(App.GlobalSettings.IsRoundCornerEnabled ? 15 : 0);
        }
    }
}
