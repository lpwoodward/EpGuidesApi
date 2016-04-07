using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using EpGuidesApi.Domain.DataObjects;

namespace EpGuidesApi.Domain
{
	public class EpGuidesApi : IServiceDataService
	{
		#region Properties and Fields

		internal static EpGuidesApi MethodObject = new EpGuidesApi();

		protected internal const string EpisodeRegex = "^(\\d+)\\.\\s+(\\d+)-(\\d+)\\s+([0-9A-Za-zA-Z ]+)\\s+<a[^>]*>([^<]+)<\\/a>.*$";

		#endregion

		public Series GetSeriesInformation(string seriesName)
		{
			var epGuidesSeriesHtml = WebHelpers.GetHtmlPageAsString("http://epguides.com/" + seriesName);
			var episodeMatches = Regex.Matches(epGuidesSeriesHtml, EpisodeRegex, RegexOptions.Multiline);
			var episodes = new List<Episode>();

			foreach (Match match in episodeMatches)
			{
				if (match.Groups.Count != 6) throw new Exception("Incorrect number of match groups");

				var episodeId = int.Parse(match.Groups[1].Value);
				var seasonNumber = int.Parse(match.Groups[2].Value);
				var episodeNumber = int.Parse(match.Groups[3].Value);
				var airDate = DateTime.Parse(match.Groups[4].Value);
				var episodeName = match.Groups[5].Value;

				episodes.Add(new Episode
				{
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

