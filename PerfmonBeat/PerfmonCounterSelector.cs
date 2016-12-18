using System.Diagnostics;
using System.Linq;

namespace PerfmonBeat
{
	public static class PerfmonCounterSelector
	{
		public static bool IsCounterAvailable(string categoryName, string counterName, string instanceName) {
			var hasCategory = PerformanceCounterCategory.GetCategories().Any(category => category.CategoryName == categoryName);
			if (!hasCategory) return false;
			
			var hasCounter = PerformanceCounterCategory.CounterExists(counterName, categoryName);
			if (!hasCounter) return false;

			var hasInstance = PerformanceCounterCategory.InstanceExists(instanceName, categoryName);
			if (!hasInstance) return false;

			return true;
		}
	}
}
