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
	// It might be possible to make IFileInfo, IDirectoryInfo, IFileSystemInfo interfaces that expose common functionality
	// and then service locate either UnixFileInfo or WindowsFileInfo implementations dependent upon run-time OperatingSystem
	// this is probably wasted effort, I probably won't be running this on a Windows box

	public static class DirectoryInfoExtensionMethods
	{
		internal static DirectoryInfoExtensionMethodsConcreteObject MethodObject = new DirectoryInfoExtensionMethodsConcreteObject();

		public static List<FileSystemInfo> GetFileSystemInfosAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly) { return MethodObject.GetFileSystemInfosAsListSlave(directoryInfo, regex, searchOption); }
		public static List<DirectoryInfo> GetDirectoriesAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly) { return MethodObject.GetDirectoriesAsListSlave(directoryInfo, regex, searchOption); }
		public static List<FileInfo> GetFilesAsList(this DirectoryInfo directoryInfo, Regex regex = null, SearchOption searchOption = SearchOption.TopDirectoryOnly) { return MethodObject.GetFilesAsListSlave(directoryInfo, regex, searchOption); }
		public static bool IsEmpty(this DirectoryInfo directoryInfo) { return MethodObject.IsEmptySlave(directoryInfo); }
		public static void ClearDirectory(this DirectoryInfo directoryInfo, bool errorIfSubdirectoriesNotEmpty = false) { MethodObject.ClearDirectorySlave(directoryInfo, errorIfSubdirectoriesNotEmpty); }
	}

	public class DirectoryInfoExtensionMethodsConcreteObject
	{
		#region GetFileSystemInfosAsList

		/// <summary>
		/// Gets the FileSystemInfos as List.
		/// </summary>
		/// <returns>The file system infos as list slave.</returns>
		/// <param name="directoryInfo">Directory info.</param>
		/// <param name="regex">Regex.</param>
		/// <param name="searchOption">Search option.</param>
		protected internal virtual List<FileSystemInfo> GetFileSystemInfosAsListSlave(DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption)
		{
			return directoryInfo.EnumerateFileSystemInfos("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}

		#endregion

		#region GetDirectoriesAsList

		/// <summary>
		/// Gets the Directories as List.
		/// </summary>
		/// <returns>The directories as list slave.</returns>
		/// <param name="directoryInfo">Directory info.</param>
		/// <param name="regex">Regex.</param>
		/// <param name="searchOption">Search option.</param>
		protected internal virtual List<DirectoryInfo> GetDirectoriesAsListSlave(DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption)
		{
			return directoryInfo.EnumerateDirectories("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}

		#endregion

		#region GetFilesAsList

		/// <summary>
		/// Gets the Files as List.
		/// </summary>
		/// <returns>The files as list slave.</returns>
		/// <param name="directoryInfo">Directory info.</param>
		/// <param name="regex">Regex.</param>
		/// <param name="searchOption">Search option.</param>
		protected internal virtual List<FileInfo> GetFilesAsListSlave(DirectoryInfo directoryInfo, Regex regex, SearchOption searchOption)
		{
			return directoryInfo.EnumerateFiles("*", searchOption)
								.Where(x => (regex ?? new Regex(".*")).IsMatch(x.Name))
								.ToList();
		}

		#endregion

		#region IsEmpty

		/// <summary>
		/// Determines whether directory is empty.
		/// </summary>
		/// <returns><c>true</c> if this instance is empty the specified directoryInfo; otherwise, <c>false</c>.</returns>
		/// <param name="directoryInfo">Directory info.</param>
		protected internal virtual bool IsEmptySlave(DirectoryInfo directoryInfo)
		{
			return directoryInfo.GetFileSystemInfosAsList().Any() == false;
		}

		#endregion

		#region ClearDirectory

		/// <summary>
		/// Clears the directory.
		/// </summary>
		/// <param name="directoryInfo">Directory info.</param>
		/// <param name="errorIfSubdirectoriesNotEmpty">If set to <c>true</c> error if subdirectories not empty.</param>
		protected internal virtual void ClearDirectorySlave(DirectoryInfo directoryInfo, bool errorIfSubdirectoriesNotEmpty)
		{
			var directories = directoryInfo.GetDirectoriesAsList();
			if (errorIfSubdirectoriesNotEmpty && directories.Any(x => x.IsEmpty() == false))
				throw new Exception("Cannot clear directory, some sub-directories are not empty");

			directoryInfo.GetFilesAsList().ForEach(x => x.Delete());
			directories.ForEach(x => x.Delete(true));
		}

		#endregion
	}
}

