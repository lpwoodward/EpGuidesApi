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
		//Refactor this whole class, it's horrible!

		internal static FileSystemEpisodeApi MethodObject = new FileSystemEpisodeApi();

		public static Series GetSeriesInformation(string seriesName, string pathToSearch) { return MethodObject.GetSeriesInformationSlave(seriesName, pathToSearch); }
		protected internal virtual Series GetSeriesInformationSlave(string seriesName, string pathToSearch)
		{
			var seriesDirectoryPath = string.Format("{0}/{1}", pathToSearch, seriesName);
			var directory = new DirectoryInfo(seriesDirectoryPath);
			var seasonFolders = directory.GetDirectories(new Regex("Season [\\d]+", RegexOptions.IgnoreCase));				

			var episodes = new List<Episode>();
			seasonFolders.ForEach(x => AddEpisodesForSeason(x, seriesName, episodes));
			return Series.Create(seriesName, episodes);
		}

		protected internal virtual void AddEpisodesForSeason(DirectoryInfo seasonFolder, string seriesName, List<Episode> episodes)
		{
			var episodeFiles = seasonFolder.GetFiles(new Regex("[\\d]+"));
			var seasonNumber = int.Parse(Regex.Match(seasonFolder.Name, "Season (\\d+)", RegexOptions.IgnoreCase).Groups[1].Value);
			episodeFiles.ForEach(x => episodes.Add(GetEpisode(x, seasonNumber, seriesName)));
		}

		protected internal virtual Episode GetEpisode(FileInfo episodeFile, int seasonNumber, string seriesName)
		{
			//Maybe make a Create on Episode that takes into a FileSystemInfo
			var episodeNumber = int.Parse(Regex.Match(episodeFile.Name, "^(\\d+).*").Groups[1].Value);
			var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(episodeFile.FullName);
			var episodeName = Regex.Match(fileNameWithoutExtension, "^[\\d]+(.*)$").Groups[1].Value;
			return new Episode
			{
				Number = episodeNumber,
				Name = episodeName,
				SeasonNumber = seasonNumber,
				SeriesName = seriesName
			};
		}
	}
}

