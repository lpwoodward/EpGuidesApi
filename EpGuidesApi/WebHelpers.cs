using System;
using System.Net;
using System.IO;
using System.Text;

namespace EpGuidesApi
{
	public class WebHelpers
	{
		#region Properties and Fields

		internal static WebHelpers MethodObject = new WebHelpers();

		#endregion

		#region Static and Factory Methods

		public static string GetHtmlPageAsString(string url) { return MethodObject.GetHtmlPageAsStringSlave(url); }
		protected internal virtual string GetHtmlPageAsStringSlave(string url)
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

		#endregion
	}
}

