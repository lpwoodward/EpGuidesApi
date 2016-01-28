using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EpGuidesApi
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			var seriesInformationFromEpGuides = EpGuidesApi.GetSeriesInformation("Friends");
			var seriesInformationFromDisk = FileSystemEpisodeApi.GetSeriesInformation("Friends", "/Users/Laurence/tmpDir");
			var latestEpisodeFromDisk = seriesInformationFromDisk.GetLatestEpisode();
			var episodesToGet = seriesInformationFromEpGuides.GetEpisodesAfterEpisode(latestEpisodeFromDisk);

			Console.WriteLine("Need to get the following episodes");
			foreach (var episodeToGet in episodesToGet)
			{
				Console.WriteLine("{0} - Season {1} - Episode {2} - {3}", episodeToGet.SeriesName, episodeToGet.SeasonNumber, episodeToGet.Number, episodeToGet.AirDate.ToString("d"));
			}
		}
	}
}
