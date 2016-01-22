using System;
using System.Net;
using System.IO;
using System.Text;

namespace EpGuidesApi
{
	public class WebHelpers
	{
		
			public string GetHtmlPageAsString(string url)
			{
			var request = HttpWebRequest.Create(url);
				var response = request.GetResponse();

				string responseAsString;
				using (var stream = response.GetResponseStream())
				{
					StreamReader reader = new StreamReader(stream, Encoding.UTF8);
					responseAsString = reader.ReadToEnd();
				}

				return responseAsString;
			}
	}
}

