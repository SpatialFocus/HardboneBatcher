// <copyright file="CommandEventHandler.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher.Shared
{
	using System;
	using CliWrap.EventStream;
	using Microsoft.Extensions.Logging;

	public class CommandEventHandler
	{
		private readonly ILogger logger;

		public CommandEventHandler(ILogger logger)
		{
			this.logger = logger;
		}

		public void Handle(string tile, CommandEvent cmdEvent)
		{
			switch (cmdEvent)
			{
				case StartedCommandEvent started:
					this.logger.LogInformation("{Tile} || Started process '" + started.ProcessId + "'", tile);
					break;
				case StandardOutputCommandEvent stdOut:
					var message = stdOut.Text;
					//if (!message.StartsWith("Started Pre-Processing of tile", StringComparison.InvariantCultureIgnoreCase) &&
					//	!message.StartsWith("Tile-ID:", StringComparison.InvariantCultureIgnoreCase) &&
					//	!message.StartsWith("Time Elapsed:", StringComparison.InvariantCultureIgnoreCase) &&
					//	!message.StartsWith("Path to Results:", StringComparison.InvariantCultureIgnoreCase))
					//{
					//	return;
					//}

					this.logger.LogDebug("{Tile} || " + stdOut.Text, tile);
					break;
				case StandardErrorCommandEvent stdErr:
					this.logger.LogError("{Tile} || Error: " + stdErr.Text, tile);
					break;
				case ExitedCommandEvent exited:
					this.logger.LogInformation("{Tile} || Exit: " + exited.ExitCode, tile);
					break;
			}
		}
	}
}