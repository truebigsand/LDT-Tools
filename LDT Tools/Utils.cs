using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using LDT_Tools.Tools;

namespace LDT_Tools
{
    public static class Utils
    {
        public static void ConsoleWriteErrorLine(string Message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static string GetCustomOrDefaultName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<DisplayNameAttribute>();
            if (nameAttribute == null)
            {
                return type.Name;
            }
            return nameAttribute.Name;
        }
    }
}
