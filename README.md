# CLC+ Backbone - Hardbone Batcher

Batch script to run the hardbone extraction in parallel.

## Adjust the path variables

1) Open `Program.cs` in `/src/HardboneBatcher` for editing
2) Modify top variables according to your environment:

```csharp
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

...
```

## How to use?

1) Download and install the .NET Core 3.1 SDK [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
2) Download and extract the sourcecode or clone the repo
3) Open a command shell in the `/src/HardboneBatcher` directory
4) Run `dotnet restore` and then `dotnet run`

## Known issues

Sometimes when cancelling the run, some Python processes are stuck and keep running. Kill them in Task Manager.

----

Made with :heart: by [Spatial Focus](https://spatial-focus.net/)