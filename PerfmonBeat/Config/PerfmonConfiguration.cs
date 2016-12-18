using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PerfmonBeat.Config
{
	public class PerfmonConfiguration : ConfigurationSection
	{
		public string Elastic => ConfigurationManager.AppSettings[nameof(Elastic)];
		public int Interval => int.Parse(ConfigurationManager.AppSettings[nameof(Interval)]);

		[ConfigurationProperty("counters")]
		private CountersCollection _counters => (CountersCollection)base["counters"];
		public IEnumerable<CounterElement> Counters => _counters.Cast<CounterElement>().Where(c => c.IsValid);
	}
}
