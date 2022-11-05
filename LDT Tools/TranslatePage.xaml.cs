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
                    string text = OCR(dialog.FileName);
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
        static string OCR(string path)
        {
            using Bitmap bitmap = new Bitmap(path);
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
            var words_result = JsonNode.Parse(json)["words_result"];
            if (words_result == null)
            {
                return "[File too large!]";
            }
            var words = words_result.AsArray().Reverse().Select(x => x["words"].GetValue<string>());
            return string.Join('\n', words);
        }
        static string GetAccessToken()
        {
            return JsonNode.Parse(client.GetStringAsync(accessTokenUrl).Result)["access_token"].GetValue<string>();
        }
    }
}
