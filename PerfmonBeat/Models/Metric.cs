using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace PerfmonBeat.Models
{
	public class Metric
	{
		[JsonProperty("@timestamp")]
		public DateTime Timestamp { get; set; } = DateTime.Now;
		public string Type { get; set; } = "metricsets";
		public Metricset Metricset { get; set; } = new Metricset();
		public Beat Beat { get; set; } = new Beat();
		public Dictionary<string, Dictionary<string, float>> Perfmon { get; set; } = new Dictionary<string, Dictionary<string, float>>();
	}
}
