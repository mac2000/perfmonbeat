using System;
using System.Reflection;

namespace PerfmonBeat.Models
{
	public class Beat
	{
		public string Hostname { get; set; } = Environment.MachineName;
		public string Name { get; set; } = Environment.MachineName;
		public string Version { get; set; } = Assembly.GetEntryAssembly().GetName().Version.ToString();
	}
}
