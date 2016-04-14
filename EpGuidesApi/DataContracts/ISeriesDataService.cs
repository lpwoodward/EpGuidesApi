using System;
using EpGuidesApi.Domain.DataObjects;

namespace EpGuidesApi.Domain
{
	public interface IServiceDataService
	{
		Series GetSeriesInformation(string seriesName);
	}
}

