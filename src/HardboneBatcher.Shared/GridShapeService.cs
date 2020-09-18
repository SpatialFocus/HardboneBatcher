// <copyright file="GridShapeService.cs" company="Spatial Focus GmbH">
// Copyright (c) Spatial Focus GmbH. All rights reserved.
// </copyright>

namespace HardboneBatcher.Shared
{
	using System.Collections.Generic;
	using MaxRev.Gdal.Core;
	using OSGeo.OGR;

	public class GridShapeService
	{
		public GridShapeService(string filePath)
		{
			FilePath = filePath;
		}

		protected string FilePath { get; }

		public List<string> GetCellCodes()
		{
			GdalBase.ConfigureAll();

			List<string> cells = new List<string>();

			Driver shapefileDriver = Ogr.GetDriverByName("ESRI Shapefile");
			DataSource dataSource = shapefileDriver.Open(FilePath, 0);

			Layer layer = dataSource.GetLayerByIndex(0);
			Feature feature = layer.GetNextFeature();

			while (feature != null)
			{
				string cellCode = feature.GetFieldAsString("CellCode");

				string split = feature.GetFieldAsString("split");

				// TODO: Skip cells with Status != 0
				cells.Add(!string.IsNullOrEmpty(split) ? $"{cellCode}_{split}" : cellCode);

				feature = layer.GetNextFeature();
			}

			return cells;
		}
	}
}