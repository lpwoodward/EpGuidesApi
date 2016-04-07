using System;
using System.Collections.Generic;
using System.Linq;

namespace EpGuidesApi.Domain.DataObjects
{
	public class Series
	{
		public virtual string Name { get; set; }
		public virtual string EpGuidesName { get; set; }
		public virtual List<Season> Seasons { get; set; }

		public virtual Episode GetLatestEpisode()
		{
			var latestSeason = Seasons.OrderBy(x => x.Number).LastOrDefault();
			if (latestSeason == null || latestSeason.Episodes == null) return null;
			return latestSeason.Episodes.OrderBy(x => x.Number).LastOrDefault();
		}

		public virtual List<Episode> GetEpisodesAfterEpisode(Episode episode)
		{
			var episodes = new List<Episode>();
			var episodesAfterEpisodeFromSameSeason = Seasons.FirstOrDefault(x => x.Number == episode.SeasonNumber)
															.Episodes.Where(x => x.Number > episode.Number);
															
			var episodesAfterEpisodeFromLaterSeasons = Seasons.OrderBy(x => x.Number)
															  .Where(x => x.Number > episode.SeasonNumber)
															  .Select(x => x)
															  .SelectMany(x => x.Episodes.OrderBy(y => y.Number));
			
			episodes.AddRange(episodesAfterEpisodeFromSameSeason);
			episodes.AddRange(episodesAfterEpisodeFromLaterSeasons);

			return episodes;
		}

		public static Series Create(string seriesName, List<Episode> episodes, string epGuidesName = null)
		{
			var series = new Series
			{
				Name = seriesName,
				EpGuidesName = string.IsNullOrWhiteSpace(epGuidesName) ? seriesName : epGuidesName,
				Seasons = new List<Season>()
			};

			var episodeSeasonDictionary = episodes.GroupBy(x => x.SeasonNumber)
												  .ToDictionary(x => x.Key, x => x.ToList());

			foreach (var episodeSeasonKeyPairValue in episodeSeasonDictionary)
			{
				var seasonNumber = episodeSeasonKeyPairValue.Key;
				var seasonEpisodes = episodeSeasonKeyPairValue.Value;
				series.Seasons.Add(Season.Create(seasonNumber, seasonEpisodes));
			}

			return series;
		}
	}
}

