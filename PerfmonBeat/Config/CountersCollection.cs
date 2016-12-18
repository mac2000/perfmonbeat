using System.Configuration;

namespace PerfmonBeat.Config
{
	[ConfigurationCollection(typeof(CounterElement), AddItemName = "counter")]
	public class CountersCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement() => new CounterElement();
		protected override object GetElementKey(ConfigurationElement element) => ((CounterElement)element).Key;
		public CounterElement this[int idx] => (CounterElement)BaseGet(idx);
	}
}
