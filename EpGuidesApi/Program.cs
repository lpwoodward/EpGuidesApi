using System;
using System.Net;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EpGuidesApi
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			EpGuidesApi.GetSeriesInformation("Friends");
		}
	}
}
