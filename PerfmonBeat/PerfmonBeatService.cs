using Nest;
using PerfmonBeat.Config;
using PerfmonBeat.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
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
					Console.WriteLine($"{data[counter.Key]:N0} - {counter.Key}");
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

				foreach (var category in config.Counters.GroupBy(counter => counter.Category))
				{
					var tick = 0;
					var metric = new Metric();
					metric.Metricset.Name = Normalize(category.Key);

					foreach (var counter in category.GroupBy(counter => counter.Counter))
					{
						metric.Perfmon[Normalize(counter.Key)] = new Dictionary<string, float>();

						foreach (var item in counter) {
							metric.Perfmon[Normalize(counter.Key)][Normalize(string.IsNullOrEmpty(item.Instance) ? "total" : item.Instance)] = data[item.Key];
							tick += 1;
						}
					}
					
					//Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(metric, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
					client.Index(metric, request => request.Index($"metricbeat-{DateTime.Now.ToString("yyyy.MM.dd")}"));
					Console.WriteLine($"Sent {tick} metrics for {metric.Metricset.Name}");
				}
			}
		}

		private string Normalize(string name)
		{
			name = name.ToLower();
			name = Regex.Replace(name, "[^a-z0-9]+", "_");
			name = Regex.Replace(name, "_+", "_");
			name = name.Trim('_');
			return name;
		}

		public bool Start(HostControl hostControl)
		{
			foreach (var counter in config.Counters)
			{
				Task.Factory.StartNew(() => Collect(counter, cancellationTokenSource.Token), cancellationTokenSource.Token);
			}

			Task.Factory.StartNew(() => Send(cancellationTokenSource.Token), cancellationTokenSource.Token);
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			cancellationTokenSource.Cancel();
			return true;
		}
	}
}
