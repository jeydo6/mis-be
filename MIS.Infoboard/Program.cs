using Avalonia;
using System;

namespace MIS.Infoboard;

internal static class Program
{
	[STAThread]
	public static void Main(string[] args)
	{
		try
		{
			BuildAvaloniaApp()
				.StartWithClassicDesktopLifetime(args);
		}
		catch (Exception e)
		{
			Serilog.Log.Fatal(e, "Something very bad happened");
		}
		finally
		{
			Serilog.Log.CloseAndFlush();
		}
	}

	private static AppBuilder BuildAvaloniaApp() =>
		AppBuilder.Configure<App>()
			.UsePlatformDetect()
			.LogToTrace();
}
