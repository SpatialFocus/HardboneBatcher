// <copyright file="CommandEventHandler.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher.Shared
{
	using System;
	using CliWrap.EventStream;

	public static class CommandEventHandler
	{
		public static void Handle(string tile, CommandEvent cmdEvent, ConsoleColor color)
		{
			switch (cmdEvent)
			{
				case StartedCommandEvent started:
					Helpers.Output(tile, $"Started process '{started.ProcessId}'", color);
					break;
				case StandardOutputCommandEvent stdOut:
					Helpers.OutputConditional(tile, stdOut.Text, color);
					break;
				case StandardErrorCommandEvent stdErr:
					Helpers.OutputError(tile, stdErr.Text, color);
					break;
				case ExitedCommandEvent exited:
					Helpers.Output(tile, $"Exit '{exited.ExitCode}'", color);
					break;
			}
		}
	}
}