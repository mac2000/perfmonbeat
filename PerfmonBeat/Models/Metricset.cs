using System;

namespace PerfmonBeat.Models
{
	public class Metricset
	{
		public string Host { get; set; } = Environment.MachineName;
		public string Module { get; set; } = "perfmon";
		public string Name { get; set; }
		public int Rtt { get; set; }
	}
}
