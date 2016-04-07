using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
using Mono.Unix;
using System.Threading.Tasks;

namespace EpGuidesApi.Domain.DirectoryStuff
{
	public static class DirectoryInfoExtensionMethods
	{
		internal static DirectoryInfoExtensionMethodsConcreteObject MethodObject = new DirectoryInfoExtensionMethodsConcreteObject();

		public static List<FileSystemInfo> GetFileSystemInfosAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return directoryInfo.EnumerateFileSystemInfos("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}
		
		public static List<DirectoryInfo> GetDirectoriesAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return directoryInfo.EnumerateDirectories("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}

		public static List<FileInfo> GetFilesAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			return directoryInfo.EnumerateFiles("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}

		public static bool IsEmpty(this DirectoryInfo directoryInfo)
		{
			return directoryInfo.GetFileSystemInfos().Any() == false;
		}

		public static void ClearDirectory(this DirectoryInfo directoryInfo, bool errorIfSubdirectoriesNotEmpty = false)
		{
			if (errorIfSubdirectoriesNotEmpty && directoryInfo.GetDirectoriesAsList().Any(x => x.IsEmpty() == false))
				throw new Exception("Cannot clear directory, some sub-directories are not empty");

			directoryInfo.GetFilesAsList().ForEach(x => x.Delete());
			directoryInfo.GetDirectoriesAsList().ForEach(x => x.Delete(true));
		}
	}

	public static class FileInfoExtensionMethods
	{
		public static FileSystemInfo CreateHardLink(this FileInfo fileInfo, string linkNameFullPath)
		{
			throw new NotImplementedException();
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

