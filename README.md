# CLC+ Backbone - Hardbone Batcher

Batch script to run the hardbone extraction in parallel.

## Adjust the variables in appsettings

1) Open `appsettings.json` for editing
2) Modify top variables according to your environment:

```json
{
   // Maximum number of parallel processes; 0 to use processor count
   "Parallelism": 0,

   // Path to the ArcPy executable
   "PythonPath": "C:/Python27/ArcGIS10.8/python.exe",

   // Root directory to the geo-databases
   "RootPath": "C:/temp/CLCplus/hardbones/Testsite_Sweden/01_input_data/SE",

   // Location of the ArcPy script
   "ScriptPath": "C:/temp/CLCplus/hardbones/HardboneIntegration_v1_0.py",

   // Path to the batch shapefile
   "ShapePath": "C:/temp/CLCplus/hardbones/Testsite_Sweden/02_grid_cells/europe_25km_10km_Sweden_testsite_v2/europe_25km_10km_Sweden_testsite_v2.shp",

   // Do not close the console window after completed workflow
   "WaitForUserInputAfterCompletion": true,

...
```

## Known issues

Sometimes when cancelling the run, some Python processes are stuck and keep running. Kill them in Task Manager.

----

Made with :heart: by [Spatial Focus](https://spatial-focus.net/)