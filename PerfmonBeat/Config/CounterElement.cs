﻿using System.Configuration;

namespace PerfmonBeat.Config
{
	public class CounterElement : ConfigurationElement
	{
		[ConfigurationProperty("category", DefaultValue = "", IsKey = false, IsRequired = true)]
		public string Category
		{
			get
			{
				return (string)base["category"];
			}
			set
			{
				base["category"] = value;
			}
		}

		[ConfigurationProperty("counter", DefaultValue = "", IsKey = false, IsRequired = true)]
		public string Counter
		{
			get
			{
				return (string)base["counter"];
			}
			set
			{
				base["counter"] = value;
			}
		}

		[ConfigurationProperty("instance", DefaultValue = "", IsKey = false, IsRequired = false)]
		public string Instance
		{
			get
			{
				return (string)base["instance"];
			}
			set
			{
				base["instance"] = value;
			}
		}
	}
}
