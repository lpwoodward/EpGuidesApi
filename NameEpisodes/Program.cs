using System;
using Mono.Posix;
using Mono.Unix;
using EpGuidesApi;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using EpGuidesApi.Domain.DataObjects;
using EpGuidesApi.Domain;
using EpGuidesApi.Domain.DirectoryStuff;

namespace NameEpisodes
{
	public class MainClass
	{
		private const string OriginalMediaDirectoryName = "originalMedia/";
		private const string TemporaryDirectoryName = "tmpMedia/";

		private static string workingDirectoryPath = UnixDirectoryInfo.GetCurrentDirectory();
		private static string originalMediaDirectoryPath;
		private static string temporaryMediaDirectoryPath;

		internal static MainClass MethodObject = new MainClass();

		public static void Main(string[] args)
		{
			originalMediaDirectoryPath = string.Format("{0}/{1}", workingDirectoryPath, OriginalMediaDirectoryName);
			temporaryMediaDirectoryPath = string.Format("{0}/{1}", workingDirectoryPath, TemporaryDirectoryName);

			MethodObject.CreateOriginalMediaAndTemporaryMediaDirectories();
			var episodeHardLinks = MethodObject.SetupHardLinksForEpisodeFiles();

		}

		protected internal virtual void CreateOriginalMediaAndTemporaryMediaDirectories()
		{
			var originalMediaDirectory = new DirectoryInfo(originalMediaDirectoryPath);
			var temporaryMediaDirectory = new DirectoryInfo(temporaryMediaDirectoryPath);

			originalMediaDirectory.Create();
			temporaryMediaDirectory.Create();
		}

		protected internal virtual List<UnixFileSystemInfo> SetupHardLinksForEpisodeFiles()
		{
			throw new NotImplementedException("This all needs rewriting to use new directoryInfo methods");
//			var workingDirectory = new UnixDirectoryInfo(workingDirectoryPath);
//			var episodeFiles = workingDirectory.GetFiles(new Regex("s\\d+e\\d+", RegexOptions.IgnoreCase));

//			return episodeFiles.Select(x => x.CreateLink(originalMediaDirectoryPath + x.Name)).ToList();
		}

		protected internal virtual Episode GetEpisode(UnixFileSystemInfo episodeFile)
		{
			var episodeRegex = new Regex("^(.*)s(\\d)+e(\\d)+.*$", RegexOptions.IgnoreCase);
			var fileName = episodeFile.Name;
			var file = new FileInfo(episodeFile.FullName);

			var fileNameRegexMatch = episodeRegex.Match(fileName);
			var seriesName = FormatSeriesName(fileNameRegexMatch.Groups[1].Value);
			var seasonNumber = int.Parse(fileNameRegexMatch.Groups[2].Value);
			var episodeNumber = int.Parse(fileNameRegexMatch.Groups[3].Value);
			var extension = file.Extension;

			var newFileName = string.Format("{0}_S{1}E{2}.{3}", seriesName, seasonNumber, episodeNumber, extension);
			file.MoveTo(file.DirectoryName + "/" + newFileName);

			return new Episode
			{
				Number = episodeNumber,
				SeasonNumber = seasonNumber,
				SeriesName = seriesName
			};
		}

		protected internal virtual string FormatSeriesName(string series)
		{
			series = Regex.Replace(series, "^the", "", RegexOptions.IgnoreCase);
			return series.Replace(".", "")
						 .Replace("-", "")
						 .Replace(" ", "");
		}
	}
}
