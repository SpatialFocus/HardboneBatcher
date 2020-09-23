// <copyright file="Program.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using CliWrap;
	using CliWrap.EventStream;
	using Dasync.Collections;
	using HardboneBatcher.Shared;

	public class Program
	{
		// Maximum number of parallel processes; 0 to use processor count
		private const int Parallelism = 0;

		// Path to the ArcPy executable
		private const string PythonPath = @"C:\Python27\ArcGIS10.8\python.exe";

		// Root directory to the geodatabases
		private const string RootPath = @"C:\temp\CLCplus\Testsite_Sweden\01_input_data\SE";

		// Location of the ArcPy script
		private const string ScriptPath = @"C:\temp\CLCplus\HardboneIntegration_v1_0_withParams.py";

		// Path to the batch shapefile
		private const string ShapePath =
			@"C:\temp\CLCplus\Testsite_Sweden\02_grid_cells\europe_25km_10km_Sweden_testsite_v2\europe_25km_10km_Sweden_testsite_v2.shp";

		private static async Task Main()
		{
			List<Command> commands = new List<Command>();
			CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

			Console.CancelKeyPress += (sender, eventArgs) =>
			{
				Console.WriteLine("Cancelling, please wait...");

				// Cancel the cancellation to allow the program to shutdown cleanly
				eventArgs.Cancel = true;
				cancellationTokenSource.Cancel();
			};

			DateTime startTime = DateTime.Now;
			Console.WriteLine("Starting... " + startTime.ToLongTimeString());

			GridShapeService gridShapeService = new GridShapeService(Program.ShapePath);

			List<string> cells = gridShapeService.GetCellCodes();

			foreach (string cell in cells)
			{
				commands.Add(Cli.Wrap(Program.PythonPath)
					.WithArguments(a => a.Add(Program.ScriptPath).Add($"-r:{Program.RootPath}").Add($"-c:{cell}")));
			}

			Random random = new Random();

			try
			{
				await commands.ParallelForEachAsync(async cmd =>
					{
						ConsoleColor randomColor = Helpers.GetRandomColor(random);

						await foreach (CommandEvent cmdEvent in cmd.ListenAsync(cancellationTokenSource.Token))
						{
							CommandEventHandler.Handle(cmd.Arguments.Split(':').Last(), cmdEvent, randomColor);
						}
					}, Program.Parallelism, cancellationTokenSource.Token)
					.ConfigureAwait(false);
			}
			catch (OperationCanceledException)
			{
				Console.WriteLine("Cancelled operation.");

				// Wait for the remaining tasks to finish
				Thread.Sleep(5000);
				return;
			}

			DateTime endTime = DateTime.Now;
			Console.WriteLine("Finished. " + endTime.ToLongTimeString());
			Console.WriteLine("Processing time: " + (endTime - startTime).ToString("g", CultureInfo.InvariantCulture));
			Console.WriteLine("--- Press a key to close ---");
			Console.ReadKey();
		}
	}
}