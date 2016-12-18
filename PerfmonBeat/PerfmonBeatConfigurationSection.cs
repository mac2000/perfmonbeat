using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfmonBeat
{
	public class PerfmonBeatConfigurationSection : ConfigurationSection
	{
		[ConfigurationProperty("interval", DefaultValue = "5000", IsRequired = false)]
		public int Interval
		{
			get
			{
				return (int)this["interval"];
			}
			set
			{
				this["interval"] = value;
			}
		}
	}
}
