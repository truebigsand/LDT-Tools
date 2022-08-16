using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDT_Tools.Tools
{
    public interface ITool
    {
        public static abstract void Interactive(); // 交互式主过程
    }
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class DisplayNameAttribute : Attribute
    {
        public string Name;
        public DisplayNameAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
