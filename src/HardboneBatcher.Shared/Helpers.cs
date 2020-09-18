// <copyright file="Helpers.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher.Shared
{
	using System;
	using System.Collections.Generic;

	public static class Helpers
	{
		public static readonly List<ConsoleColor> Colors = new List<ConsoleColor>
		{
			ConsoleColor.DarkBlue,
			ConsoleColor.DarkGreen,
			ConsoleColor.DarkCyan,
			ConsoleColor.DarkRed,
			ConsoleColor.DarkMagenta,
			ConsoleColor.DarkYellow,
			ConsoleColor.Gray,
			ConsoleColor.DarkGray,
			ConsoleColor.Blue,
			ConsoleColor.Green,
			ConsoleColor.Cyan,
			ConsoleColor.Magenta,
			ConsoleColor.Yellow,
			ConsoleColor.White,
		};

		public static ConsoleColor GetRandomColor(Random random) => Helpers.Colors[random?.Next(Helpers.Colors.Count) ?? 0];

		public static void Output(string tile, string message, ConsoleColor color)
		{
			ConsoleColor consoleColor = Console.ForegroundColor;

			Console.ForegroundColor = color;
			Console.Write($"{tile}");
			Console.ForegroundColor = consoleColor;
			Console.WriteLine($" || {message}");
		}

		public static void OutputConditional(string tile, string message, ConsoleColor color)
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}

			if (!message.StartsWith("Started Pre-Processing of tile", StringComparison.InvariantCultureIgnoreCase) &&
				!message.StartsWith("Tile-ID:", StringComparison.InvariantCultureIgnoreCase) &&
				!message.StartsWith("Time Elapsed:", StringComparison.InvariantCultureIgnoreCase) &&
				!message.StartsWith("Path to Results:", StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			ConsoleColor consoleColor = Console.ForegroundColor;

			Console.ForegroundColor = color;
			Console.Write($"{tile}");
			Console.ForegroundColor = consoleColor;
			Console.WriteLine($" || {message}");
		}

		public static void OutputError(string tile, string message, ConsoleColor color)
		{
			ConsoleColor consoleColor = Console.ForegroundColor;

			Console.ForegroundColor = color;
			Console.Write($"{tile}");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($" || {message}");
			Console.ForegroundColor = consoleColor;
		}
	}
}