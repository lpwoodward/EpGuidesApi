using System;
using System.Collections.Generic;
using System.Globalization;

namespace EpGuidesApi
{
	public class Season
	{
		public virtual int Number { get; set; }
		public virtual List<Episode> Episodes { get; set; }

		public virtual Episode GetMostRecentEpisode()
		{
			throw new NotImplementedException();
		}

		public static Season Create(int seasonNumber, List<Episode> episodes)
		{
			return new Season {
				Number = seasonNumber,
				Episodes = episodes
			};
		}
	}
}

