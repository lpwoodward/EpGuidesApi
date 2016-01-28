using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EpGuidesApi
{
	public static class DirectoryInfoExtensionMethods
	{
		public static List<DirectoryInfo> GetDirectories(this DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return directoryInfo.EnumerateDirectories("*", searchOption)
								.Where(x => regex.IsMatch(x.Name))
								.ToList();
		}

		public static List<FileInfo> GetFiles(this DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return directoryInfo.EnumerateFiles("*", searchOption)
								.Where(x => regex.IsMatch(x.Name))
								.ToList();
		}
	}
}

