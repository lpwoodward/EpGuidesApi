using System;

namespace EpGuidesApi
{
	public class Episode
	{
		public virtual int EpisodeId { get; set; }
		public virtual string FullEpisodeName { get; set; }
		public virtual int Number { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime AirDate { get; set; }
		public virtual int SeasonNumber { get; set; }
		public virtual string SeriesName { get; set; }
	}
}

