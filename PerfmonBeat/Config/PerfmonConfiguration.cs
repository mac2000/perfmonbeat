using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace PerfmonBeat.Config
{
	public class PerfmonConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("counters")]
		private CountersCollection _counters => (CountersCollection)base["counters"];
		public IEnumerable<CounterElement> Counters => _counters.Cast<CounterElement>().Where(c => c.IsValid);
		public IEnumerable<CounterElement> InvalidCounters => _counters.Cast<CounterElement>().Where(c => !c.IsValid);

		public string Elastic => ConfigurationManager.AppSettings[nameof(Elastic)];
		public int Interval => int.Parse(ConfigurationManager.AppSettings[nameof(Interval)]);
		public string AssemblyName => Assembly.GetExecutingAssembly().GetName().Name;
		public string AssemblyDescription => Assembly.GetExecutingAssembly()
			.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
			.OfType<AssemblyDescriptionAttribute>()
			.FirstOrDefault().Description;
		public string AssemblyVersion => Assembly.GetEntryAssembly().GetName().Version.ToString();
	}
}
