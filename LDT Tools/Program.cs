using Sharprompt;
using System.Reflection;

using LDT_Tools.Tools;
using static LDT_Tools.Utils;

namespace LDT_Tools
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tools = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetInterface(nameof(ITool)) != null);
            if (tools.Count() == 0)
            {
                ConsoleWriteErrorLine("No tool is available!");
                return;
            }
            var tool = Prompt.Select("Please select a tool", items: tools, textSelector: GetCustomOrDefaultName);
            tool.InvokeMember("Interactive", BindingFlags.InvokeMethod, null, null, null);

            Console.ReadKey();
        }
    }
}