using System;
using System.IO;
using System.Reflection;
using Mono.Unix;

namespace EpGuidesApi.Domain.DirectoryStuff
{
	public static class FileInfoExtensionMethods
	{
		internal static FileInfoExtensionMethodsConcreteObject MethodObject = new FileInfoExtensionMethodsConcreteObject();

		public static FileSystemInfo CreateHardLink(this FileInfo fileInfo, string linkNameFullPath) { return MethodObject.CreateHardLinkSlave(fileInfo, linkNameFullPath); }
	}

	public class FileInfoExtensionMethodsConcreteObject
	{
		/// <summary>
		/// Creates a hard link.
		/// </summary>
		/// <returns>The hard link slave.</returns>
		/// <param name="fileInfo">File info.</param>
		/// <param name="linkNameFullPath">Link name full path.</param>
		protected internal virtual FileSystemInfo CreateHardLinkSlave(FileInfo fileInfo, string linkNameFullPath)
		{
			if (linkNameFullPath == fileInfo.FullName)
				throw new ArgumentException("Cannot create hardlink with same path as original file");
			
			var unixFileInfo = new UnixFileInfo(fileInfo.FullName);
			var hardLinkUnixFileInfo = unixFileInfo.CreateLink(linkNameFullPath);
			return new FileInfo(hardLinkUnixFileInfo.FullName);
		}
	}
}

