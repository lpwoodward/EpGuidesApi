using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EpGuidesApi
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var series = new MainClass().GetSeriesInformationFromEpGuides("Friends");
		}



		public virtual Series GetSeriesInformationFromEpGuides(string seriesName)
		{
			const string episodeRegex = "^(\\d+)\\.\\s+(\\d+)-(\\d+)\\s+([0-9A-Za-zA-Z ]+)\\s+<a[^>]*>([^<]+)<\\/a>.*$";

			var epGuidesSeriesHtml = new WebHelpers().GetHtmlPageAsString("http://epguides.com/" + seriesName);
			var episodeMatches = Regex.Matches(epGuidesSeriesHtml, episodeRegex, RegexOptions.Multiline);
			var episodes = new List<Episode>();
			foreach (Match match in episodeMatches)
			{
				if (match.Groups.Count != 6) throw new Exception("Incorrect number of match groups");

				var episodeId = int.Parse(match.Groups[1].Value);
				var seasonNumber = int.Parse(match.Groups[2].Value);
				var episodeNumber = int.Parse(match.Groups[3].Value);
				var airDate = DateTime.Parse(match.Groups[4].Value);
				var episodeName = match.Groups[5].Value;

				episodes.Add(new Episode {
					AirDate = airDate,
					EpisodeId = episodeId,
					Name = episodeName,
					Number = episodeNumber,
					SeasonNumber = seasonNumber,
					SeriesName = seriesName
				});
			}

			return Series.Create(seriesName, episodes);
		}
	}






}
