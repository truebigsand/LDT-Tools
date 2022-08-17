using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace LDT_Tools.Tools
{
    public class _ProcessForbider
    {
        private string? processName;
        private double? countTime; //单位分钟
        public _ProcessForbider(string? processName, double? countTime)
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
            int _countTime = 0;
            while (true)
            {
                if (_countTime * 60 >= countTime)
                {
                    Console.WriteLine("End");
                    break;
                }
                if (isAlive())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("What are you motherfukcing opening!");
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

        }
        [DisplayName("防沉迷系统")]
        public class ProcessForbider : ITool
        {
            public static void Interactive()
            {
                //这怎么写？
            }
        }
    }
}


