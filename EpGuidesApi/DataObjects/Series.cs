using System;
using System.Collections.Generic;
using System.Linq;

namespace EpGuidesApi
{
	public class Series
	{
		public virtual string Name { get; set; }
		public virtual List<Season> Seasons { get; set; }

		public virtual Episode GetMostRecentEpisode()
		{
			throw new NotImplementedException();
		}

		public static Series Create(string seriesName, List<Episode> episodes)
		{
			var series = new Series { Name = seriesName, Seasons = new List<Season>() };
			var episodeSeasonDictionary = episodes.GroupBy(x => x.SeasonNumber)
												  .ToDictionary(x => x.Key, x => x.ToList());

			foreach (var episodeSeasonKeyPairValue in episodeSeasonDictionary)
			{
				series.Seasons.Add(Season.Create(episodeSeasonKeyPairValue.Key, episodeSeasonKeyPairValue.Value));
			}

			return series;
		}
	}
}

