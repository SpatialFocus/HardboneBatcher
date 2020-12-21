// <copyright file="Program.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using CliWrap;
	using CliWrap.EventStream;
	using Dasync.Collections;
	using HardboneBatcher.Shared;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
	using Serilog;

	public class Program
	{
		private static async Task Main()
		{
			IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

			IConfigurationRoot config = builder.Build();

			ServiceCollection serviceCollection = new ServiceCollection();
			serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(new LoggerConfiguration().ReadFrom
				.Configuration(config.GetSection("Logging"))
				.WriteTo.Map("Tile", "generic", (name, wt) => wt.File($"./logs/log-{name}.txt"))
				.CreateLogger()));

			ServiceProvider provider = serviceCollection.BuildServiceProvider();
			ILogger<Program> logger = provider.GetRequiredService<ILogger<Program>>();

			List<Command> commands = new List<Command>();

#pragma warning disable CA2000 // Dispose objects before losing scope
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
#pragma warning restore CA2000 // Dispose objects before losing scope

			Console.CancelKeyPress += (sender, eventArgs) =>
			{
				logger.LogInformation("Cancelling, please wait...");

				// Cancel the cancellation to allow the program to shutdown cleanly
				eventArgs.Cancel = true;
				cancellationTokenSource.Cancel();
			};

			Stopwatch stopwatch = Stopwatch.StartNew();
			logger.LogInformation("Starting...");

			GridShapeService gridShapeService = new GridShapeService(config["ShapePath"]);

			List<string> cells = gridShapeService.GetCellCodes();

			foreach (string cell in cells)
			{
				commands.Add(Cli.Wrap(config["PythonPath"])
					.WithArguments(a => a.Add(config["ScriptPath"]).Add($"{config["RootPath"]}").Add("#").Add($"{cell}")));
			}

			try
			{
				CommandEventHandler commandEventHandler = new CommandEventHandler(logger);
				await commands.ParallelForEachAsync(async cmd =>
					{
						await foreach (CommandEvent cmdEvent in cmd.ListenAsync(cancellationTokenSource.Token))
						{
							commandEventHandler.Handle(cmd.Arguments.Split(' ').Last(), cmdEvent);
						}
					}, int.Parse(config["Parallelism"], CultureInfo.InvariantCulture), cancellationTokenSource.Token)
					.ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				logger.LogInformation("Cancelled operation.");

				// Wait for the remaining tasks to finish
				Thread.Sleep(5000);
				return;
			}

			logger.LogInformation("Finished in {Time}ms", stopwatch.ElapsedMilliseconds);

			if (bool.Parse(config["WaitForUserInputAfterCompletion"]))
			{
				logger.LogInformation("=== PRESS A KEY TO PROCEED ===");
				Console.ReadKey();
			}
		}
	}
}