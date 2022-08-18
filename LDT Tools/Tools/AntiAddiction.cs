
using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using LDT_Tools.Tools;

using System.Reflection;


namespace LDT_Tools.AntiAddiction
{
    public class ProcessForbider
    {
        private string? processName;
        private double? countTime;
        public ProcessForbider(string? processName, double? countTime)
        {


            this.processName = processName;
            this.countTime = countTime;
        }
        private bool isAlive()
        {
            int countProcess = 0;
            foreach (var process in Process.GetProcessesByName(processName))
            {
                countProcess += 1;

            }
            return countProcess > 0;
        }
        public void KillProcess()
        {

            using (StreamWriter streamwritew = new StreamWriter("config.txt"))
            {
                streamwritew.WriteLine(this.processName + "*" + this.countTime.ToString());
            }

            int _countTime = 0;
            while (true)
            {
                if (_countTime >= this.countTime * 60)
                {
                    Console.WriteLine("End");
                    break;
                }
                if (this.isAlive())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("What are you motherfukcing opening!");
                    this.countTime += 1;
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (var process in Process.GetProcessesByName(processName))
                    {
                        try
                        {
                            process.Kill();
                            process.WaitForExit();
                        }
                        catch (Win32Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        catch (Exception)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("ERROR");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                }
                Thread.Sleep(1000);
                _countTime++;
            }
            File.Delete("config.txt");
        }

    }
    public class AntiAddiction
    {     
        private static bool ConsoleEventCallback(int eventType)
        {
            switch (eventType)
            {
                case 2:
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("What are you motherfucking closing!");
                    Console.ForegroundColor = ConsoleColor.White;

                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = Environment.ProcessPath;
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.CreateNoWindow = false;
                        process.Start();
                    }
                    Thread.Sleep(1000);
                    break;


            }
            return false;
        }
        private static ConsoleEventDelegate? handler;
        private delegate bool ConsoleEventDelegate(int eventType);
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        
        public static void SetUnableClose() //关闭时，会创建config.txt并新建窗口
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
        }
        public static void ProcessKill(string name, double time)
        {
            ProcessForbider process = new ProcessForbider(name, time);
            process.KillProcess();
        } //锁定一个线程
        public static string[] prase()
        {
            string processName = new string("");
            string countTime = new string("");

            using (StreamReader streamreader = new StreamReader("config.txt"))
            {

                string? bePrased = streamreader.ReadLine();
                bool is_process_name = true;
#pragma warning disable CS8602 // 解引用可能出现空引用。
                int length = bePrased.Length;
#pragma warning restore CS8602 // 解引用可能出现空引用。
                for (int i = 0; i < length; i++)
                {

                    if (bePrased[i] == '*')
                    {
                        is_process_name = !(is_process_name);
                        continue;
                    }
                    if (is_process_name)
                    {
                        processName += bePrased[i];
                    }
                    else
                    {
                        countTime += bePrased[i];
                    }
                }

            }
            return new string[] { processName, countTime };
        } //解析config.txt,返回第一个为process name,第二个为count time 的string[]

    }
    /* 这是一个示例项目
      class execute
    {
        [STAThread]
        static void Main(string[] args) 
        {
            //设置流氓属性，需要放到STAThread中
            AntiAddiction.SetUnableClose();

            if (!(File.Exists("config.txt"))) //如果不存在config.txt说明没有closing控制台，正常模式
            {
                Console.WriteLine("Please input a file name,then you mustn't start it.");
                Console.WriteLine("First,input the exe you want to stop , then input the stop time");
                AntiAddiction.ProcessKill(Console.ReadLine(), Convert.ToDouble(Console.ReadLine()));
 
                Console.WriteLine("The program has started");
                Thread.Sleep(1000);
            }
            else //反之，是关闭了
            {
                Console.WriteLine("Because you close the console, so the time improve.");
                string processName = AntiAddiction.prase()[0];
                string countTime = AntiAddiction.prase()[1];
                Console.WriteLine("process:{0}\ntime:{1}", processName, countTime);
                AntiAddiction.ProcessKill(processName, Convert.ToDouble(countTime) + 10.0);
                Console.ReadKey();
            }
        }
    }
    */
    [Tools.DisplayName("防沉迷")]
    public class Kouyu100AutoFinisher : ITool
    {
        public static void Interactive()
        {
            //这怎么写
            //你来写吧，用法在注释里很明确了
        }
    }

}


