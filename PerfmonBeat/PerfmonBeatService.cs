using Nest;
using PerfmonBeat.Config;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace PerfmonBeat
{
	public class PerfmonBeatService : ServiceControl
	{
		private readonly ConcurrentDictionary<string, float> data = new ConcurrentDictionary<string, float>();
		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private readonly ElasticClient client;
		private readonly PerfmonConfiguration config;

		public PerfmonBeatService(PerfmonConfiguration config)
		{
			this.config = config;
			this.client = new ElasticClient(new ConnectionSettings(new Uri(config.Elastic)));
		}

		private void Collect(CounterElement counter, CancellationToken cancellationToken)
		{
			using (var performanceCounter = new PerformanceCounter(counter.Category, counter.Counter, counter.Instance))
			{
				performanceCounter.NextValue();

				while (true)
				{
					if (cancellationToken.IsCancellationRequested)
					{
						break;
					}

					Thread.Sleep(config.Interval);
					data[counter.Key] = performanceCounter.NextValue();
				}
			}
		}

		private void Send(CancellationToken cancellationToken) {
			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}

				Thread.Sleep(config.Interval);

				//data[counter.Key] = performanceCounter.NextValue();
				//client.Index(m, request => request.Index($"metricbeat-{DateTime.Now.ToString("yyyy.MM.dd")}"));
			}
		}

		public bool Start(HostControl hostControl)
		{
			foreach (var counter in config.Counters)
			{
				Task.Factory.StartNew(() => Collect(counter, cancellationTokenSource.Token), cancellationTokenSource.Token);
			}

			Task.Factory.StartNew(() => Send(cancellationTokenSource.Token), cancellationTokenSource.Token);


			Console.WriteLine("got " + config.Counters.Count());
			//if (PerfmonCounterSelector.IsCounterAvailable("Processor", ""))

			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			return true;
		}
	}
}
