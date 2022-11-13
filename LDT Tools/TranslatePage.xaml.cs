using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;
using System.Diagnostics;
using YY.Screenshot;
using Wpf.Ui.Controls;

namespace LDT_Tools
{
    /// <summary>
    /// HomePage.xaml 的交互逻辑
    /// </summary>
    public partial class TranslatePage : Page
    {
        public TranslatePage()
        {
            InitializeComponent();
            accessToken = GetAccessToken();
        }

        private void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("图片", ".jpg;.png;.bmp;.jpeg"));
            var result = dialog.ShowDialog();
            if(result == CommonFileDialogResult.Ok)
            {
                PreviewImage.Source = new BitmapImage(new Uri(dialog.FileName));
                Thread thread = new Thread(new ThreadStart(() =>
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    using var bitmap = new Bitmap(dialog.FileName);
                    string text = OCR(bitmap);
                    TimeTextBlock.Dispatcher.InvokeAsync(() =>
                    {
                        TimeTextBlock.Text = $"用时：{sw.Elapsed.TotalSeconds}s";
                    });
                    TranslateResultTextBox.Dispatcher.InvokeAsync(() =>
                    {
                        TranslateResultTextBox.Text = text;
                    });
                    GC.Collect();
                }));
                thread.Start();
            }
        }

        private static HttpClient client = new HttpClient();
        private static string accessTokenUrl = "https://aip.baidubce.com/oauth/2.0/token?grant_type=client_credentials&client_id=bvpsaHE9t8cY6OEq1kWGX76n&client_secret=4FGvrcgdr9AFhWoWFsmz7c4OKm2aXcZi";
        private static string ocrUrl = "https://aip.baidubce.com/rest/2.0/ocr/v1/general_basic";
        private static string accessToken = string.Empty;
        static string OCR(Bitmap bitmap)
        {
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);
            //return Convert.ToBase64String(ms.ToArray());
            string image = Convert.ToBase64String(ms.ToArray());
            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("language_type", "JAP"),
                new KeyValuePair<string, string>("access_token", accessToken),
                new KeyValuePair<string, string>("image", image)
            };
            FormUrlEncodedContent content = new FormUrlEncodedContent(keyValuePairs);
            string json = client.PostAsync(ocrUrl, content).Result.Content.ReadAsStringAsync().Result;
            var words_result = JsonNode.Parse(json)?["words_result"];
            if (words_result == null)
            {
                return "[File too large]";
            }
            var words = words_result.AsArray().Reverse().Select(x => x?["words"]?.GetValue<string>());
            return string.Join('\n', words);
        }
        static string GetAccessToken()
        {
            string response = client.GetStringAsync(accessTokenUrl).Result;
            var result = JsonNode.Parse(response)?["access_token"]?.GetValue<string>();
            return result ?? throw new InvalidDataException($"Invalid AccessToken Result: {result}");
        }

        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            Screenshot screenshot = new Screenshot();
            using var image = screenshot.start();
            var bitmap = new Bitmap(image);
            PreviewImage.Source = BitmapToImageSource(bitmap);
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string text = OCR(bitmap);
                bitmap.Dispose();
                TimeTextBlock.Dispatcher.InvokeAsync(() =>
                {
                    TimeTextBlock.Text = $"用时：{sw.Elapsed.TotalSeconds}s";
                });
                TranslateResultTextBox.Dispatcher.InvokeAsync(() =>
                {
                    TranslateResultTextBox.Text = text;
                });
                GC.Collect();
            }));
            thread.Start();
        }

        public static ImageSource BitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new System.ComponentModel.Win32Exception();
            }
            return wpfBitmap;
        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
