using PerfmonBeat.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace PerfmonBeat
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = (PerfmonConfiguration)ConfigurationManager.GetSection("perfmon");
			Console.WriteLine($"Interval: {config.Interval:N0}");
			Console.WriteLine($"Elastic: {config.Elastic}");
			Console.WriteLine($"Counters: {config.Counters.Count()}");


			//var assembly = Assembly.GetExecutingAssembly();
			//var descriptionAttribute = assembly
			//	.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false)
			//	.OfType<AssemblyDescriptionAttribute>()
			//	.FirstOrDefault();

			//HostFactory.Run(x =>
			//{
			//	x.Service<PerfmonBeatService>();
			//	x.RunAsNetworkService();
			//	//x.RunAsLocalSystem();
			//	x.StartAutomaticallyDelayed();

			//	x.EnableServiceRecovery(rc => rc.RestartService(1));

			//	x.SetDescription(descriptionAttribute?.Description);
			//	x.SetDisplayName(assembly.GetName().Name);
			//	x.SetServiceName(assembly.GetName().Name);
			//});
		}
	}
}
