using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Threading;

namespace EpGuidesApi
{
	public class FileSystemEpisodeApi
	{
		internal static FileSystemEpisodeApi MethodObject = new FileSystemEpisodeApi();

		public static Series GetSeriesInformation(string seriesName, string pathToSearch) { return MethodObject.GetSeriesInformationSlave(seriesName, pathToSearch); }
		protected internal virtual Series GetSeriesInformationSlave(string seriesName, string pathToSearch)
		{
			var seriesDirectoryPath = string.Format("{0}/{1}", pathToSearch, seriesName);
			var directory = new DirectoryInfo(seriesDirectoryPath);
			var seasonFolders = directory.GetDirectories(new Regex("Season [\\d]+", RegexOptions.IgnoreCase));				

			var episodes = new List<Episode>();
			foreach (var seasonFolder in seasonFolders)
			{
				var episodeFiles = seasonFolder.GetFiles(new Regex("[\\d]+"));

				var seasonNumber = int.Parse(Regex.Match(seasonFolder.Name, "Season (\\d+)", RegexOptions.IgnoreCase).Groups[1].Value);

				foreach (var episodeFile in episodeFiles)
				{
					var episodeNumber = int.Parse(Regex.Match(episodeFile.Name, "^(\\d+).*").Groups[1].Value);
					var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(episodeFile.FullName);
					var episodeName = Regex.Match(fileNameWithoutExtension, "^[\\d]+(.*)$").Groups[1].Value;
					episodes.Add(new Episode
					{
						Number = episodeNumber,
						Name = episodeName,
						SeasonNumber = seasonNumber,
						SeriesName = seriesName
					});
				}
			}

			return Series.Create(seriesName, episodes);
		}
	}
}

