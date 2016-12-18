using System.Configuration;

namespace PerfmonBeat.Config
{
	public class PerfmonConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("interval", DefaultValue = "5000", IsKey = false, IsRequired = false)]
		public int Interval
		{
			get
			{
				return (int)base["interval"];
			}
			set
			{
				base["interval"] = value;
			}
		}

		[ConfigurationProperty("elastic", DefaultValue = "http://localhost:9200", IsKey = false, IsRequired = false)]
		public string Elastic
		{
			get
			{
				return (string)base["elastic"];
			}
			set
			{
				base["elastic"] = value;
			}
		}

		[ConfigurationProperty("counters")]
		public CountersCollection Counters
		{
			get
			{
				return ((CountersCollection)(base["counters"]));
			}
		}
	}
}
