using PerfmonBeat.Config;
using System;
using System.Configuration;
using System.Linq;
using Topshelf;

namespace PerfmonBeat
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = (PerfmonConfiguration)ConfigurationManager.GetSection("perfmon");

			Console.WriteLine($"[Config] Interval: {config.Interval:N0}");
			Console.WriteLine($"[Config] Elastic: {config.Elastic}");
			Console.WriteLine($"[Config] Counters: {config.Counters.Count()}");

			if (config.InvalidCounters.Count() > 0)
			{
				Console.WriteLine("[Warning] Invalid counters");
				foreach (var item in config.InvalidCounters)
				{
					Console.WriteLine($"[Warning] {item.Key}");
				}
			}

			HostFactory.Run(x =>
			{
				x.Service<PerfmonBeatService>(hostSettings => new PerfmonBeatService(config));
				x.RunAsNetworkService();
				//x.RunAsLocalSystem();
				x.StartAutomaticallyDelayed();

				x.EnableServiceRecovery(rc => rc.RestartService(1));

				x.SetDescription(config.AssemblyDescription);
				x.SetDisplayName(config.AssemblyName);
				x.SetServiceName(config.AssemblyName);
			});
		}
	}
}
