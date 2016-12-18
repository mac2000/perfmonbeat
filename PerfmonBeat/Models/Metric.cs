using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PerfmonBeat.Models
{
	public class Metric
	{
		[JsonProperty("@timestamp")]
		public DateTime Timestamp { get; set; }
		public string Type { get; set; }
		public Metricset Metricset { get; set; } = new Metricset();
		public Beat Beat { get; set; } = new Beat();
		public Dictionary<string, object> Perfmon { get; set; } = new Dictionary<string, object>();
	}
}
