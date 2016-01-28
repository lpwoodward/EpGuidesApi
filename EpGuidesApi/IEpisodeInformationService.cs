using System;

namespace EpGuidesApi
{
	public interface IEpisodeInformationService
	{
		Series GetSeriesInformation(string seriesName);
	}
}

