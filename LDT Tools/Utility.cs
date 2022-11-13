using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LDT_Tools.Utility
{
	[Serializable]
	public class DevelopmentException : Exception
	{
		public DevelopmentException() { }
		public DevelopmentException(string message) : base(message) { }
		public DevelopmentException(string message, Exception inner) : base(message, inner) { }
		protected DevelopmentException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
