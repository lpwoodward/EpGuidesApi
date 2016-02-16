using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Mono.Unix;
using System.Threading.Tasks;

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

	public static class UnixDirectoryInfoExtensionMethods
	{
		public static List<UnixDirectoryInfo> GetDirectories(this UnixDirectoryInfo directoryInfo, Regex regex)
		{
			return directoryInfo.GetFileSystemEntries()
								.Where(x => x.IsDirectory)
								.Where(x => regex.IsMatch(x.Name))
								.Cast<UnixDirectoryInfo>()
								.ToList();
		}

		public static List<UnixFileInfo> GetFiles(this UnixDirectoryInfo directoryInfo, Regex regex)
		{
			//not sure how IsRegularFile will work with links
			return directoryInfo.GetFileSystemEntries()
								.Where(x => x.IsRegularFile)
								.Where(x => regex.IsMatch(x.Name))
								.Cast<UnixFileInfo>()
								.ToList();
		}

		public static UnixFileInfo MoveTo(this UnixFileInfo unixFileInfo, string newFullPathName)
		{
			var fileInfo = new FileInfo(unixFileInfo.FullName);
			fileInfo.MoveTo(newFullPathName);
			return new UnixFileInfo(newFullPathName);
		}

		public static UnixDirectoryInfo MoveTo(this UnixDirectoryInfo unixDirectoryInfo, string newFullPathName)
		{
			throw new NotImplementedException();
		}

		//doing generic <T> where T : UnixFileSystemInfo doesn't seem right, as I can't create a instance of FileSystemInfo
	}

	internal class DirectoryInfoExtensionMethodsConcreteObject
	{
		protected internal virtual List<DirectoryInfo> GetDirectoriesSlave(DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption)
		{
			throw new NotImplementedException();
		}

		protected internal virtual List<FileInfo> GetFilesSlave(DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption)
		{
			throw new NotImplementedException();
		}
	}
}

