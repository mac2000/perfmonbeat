using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace PerfmonBeat
{
	public class PerfmonBeatService : ServiceControl
	{
		private readonly int Interval = int.Parse(ConfigurationManager.AppSettings["Interval"]);
		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

		public bool Start(HostControl hostControl)
		{
			Console.WriteLine("Collect available counters");

			//if (PerfmonCounterSelector.IsCounterAvailable("Processor", ""))

			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			return true;
		}
	}
}
