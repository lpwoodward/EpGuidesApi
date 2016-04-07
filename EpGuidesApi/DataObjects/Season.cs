using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EpGuidesApi.Domain.DataObjects
{
	public class Season
	{
		public virtual int Number { get; set; }
		public virtual List<Episode> Episodes { get; set; }

		public static Season Create(int seasonNumber, List<Episode> episodes)
		{
			return new Season
			{
				Number = seasonNumber,
				Episodes = episodes
			};
		}
	}
}

