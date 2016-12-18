using System.Configuration;

namespace PerfmonBeat.Config
{
	[ConfigurationCollection(typeof(CounterElement), AddItemName = "counter")]
	public class CountersCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new CounterElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			var item = (CounterElement)element;
			return $"{item.Category}\\{item.Counter}\\{item.Instance}";
		}

		public CounterElement this[int idx]
		{
			get
			{
				return (CounterElement)BaseGet(idx);
			}
		}
	}
}
