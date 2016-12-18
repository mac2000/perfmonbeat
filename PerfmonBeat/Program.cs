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
