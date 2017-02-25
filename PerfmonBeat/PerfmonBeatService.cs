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
		private readonly Dictionary<string, PerformanceCounter> counters = new Dictionary<string, PerformanceCounter>();
		private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		private readonly ElasticClient client;
		private readonly PerfmonConfiguration config;

		public PerfmonBeatService(PerfmonConfiguration config)
		{
			this.config = config;
			this.client = new ElasticClient(new ConnectionSettings(new Uri(config.Elastic)));
		}

		private void Collect(CancellationToken cancellationToken)
		{
			while (true)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}
				
				foreach (var category in config.Counters.GroupBy(counter => counter.Category))
				{
					var metric = new Metric();
					metric.Metricset.Name = Normalize(category.Key);
					metric.Perfmon[Normalize(category.Key)] = new Dictionary<string, Dictionary<string, float>>();

					foreach (var counter in category.GroupBy(counter => counter.Counter))
					{
						metric.Perfmon[Normalize(category.Key)][Normalize(counter.Key)] = new Dictionary<string, float>();

						foreach (var item in counter)
						{
							metric.Perfmon[Normalize(category.Key)][Normalize(counter.Key)][Normalize(string.IsNullOrEmpty(item.Instance) ? "total" : item.Instance)] = counters[item.Key].NextValue();
						}
					}

					Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(metric.Perfmon, Newtonsoft.Json.Formatting.None, new Newtonsoft.Json.JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }));
					client.Index(metric, request => request.Index($"metricbeat-{DateTime.Now.ToString("yyyy.MM.dd")}"));
				}

				Thread.Sleep(config.Interval);
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
				counters.Add(counter.Key, new PerformanceCounter(counter.Category, counter.Counter, counter.Instance));
				counters[counter.Key].NextValue();
			}

			Task.Factory.StartNew(() => Collect(cancellationTokenSource.Token), cancellationTokenSource.Token);
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			cancellationTokenSource.Cancel();
			return true;
		}
	}
}
