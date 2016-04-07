using System;
using NUnit.Framework;
using System.IO;
using EpGuidesApi.Domain.DirectoryStuff;
using System.Linq;

namespace EpGuidesApi.Tests.Integration
{
	public class DirectoryInfoExtensionMethodsIntegrationTests : LocalTestBase
	{
		private const string TestingDirectoryPath = "/Users/Laurence/Desktop/TestingDirectory";

		private DirectoryInfo _directoryInfo;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_directoryInfo = Directory.CreateDirectory(TestingDirectoryPath);
		}

		[TearDown]
		public void TearDown()
		{
			_directoryInfo.ClearDirectory();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_directoryInfo.Delete();
		}

		#region CleanDirectory

		[Test]
		public void ClearDirectory_WHERE_directory_is_already_empty_SHOULD_leave_directory_empty()
		{
			//act
			_directoryInfo.ClearDirectory();

			//assert
			Assert.That(_directoryInfo.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_files_in_it_SHOULD_delete_all_files()
		{
			//arrange
			File.Create(TestingDirectoryPath + "/file1.txt");
			File.Create(TestingDirectoryPath + "/file2.txt");

			if (_directoryInfo.EnumerateFiles().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_directoryInfo.ClearDirectory();

			//assert
			Assert.That(_directoryInfo.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_empty_subdirectories_in_it_SHOULD_delete_all_subdirectories()
		{
			//arrange
			_directoryInfo.CreateSubdirectory("subDirectory_1");
			_directoryInfo.CreateSubdirectory("subDirectory_2");

			if (_directoryInfo.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_directoryInfo.ClearDirectory();

			//assert
			Assert.That(_directoryInfo.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_non_empty_subdirectories_in_it_SHOULD_delete_all_subdirectories()
		{
			//arrange
			var subDirectory1DirectoryInfo = _directoryInfo.CreateSubdirectory("subDirectory_1");
			var subDirectory2DirectoryInfo = _directoryInfo.CreateSubdirectory("subDirectory_2");

			File.Create(subDirectory1DirectoryInfo.FullName + "/subDirectory1_File.txt");
			File.Create(subDirectory2DirectoryInfo.FullName + "/subDirectory2_File.txt");

			if (_directoryInfo.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_directoryInfo.ClearDirectory();

			//assert
			Assert.That(_directoryInfo.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "Cannot clear directory, some sub-directories are not empty")]
		public void ClearDirectory_WHERE_directory_has_non_empty_subdirectories_in_it_and_opting_to_not_clear_subdirectories_SHOULD_throw_error()
		{
			//arrange
			var subDirectory1DirectoryInfo = _directoryInfo.CreateSubdirectory("subDirectory_1");
			var subDirectory2DirectoryInfo = _directoryInfo.CreateSubdirectory("subDirectory_2");

			File.Create(subDirectory1DirectoryInfo.FullName + "/subDirectory1_File.txt");
			File.Create(subDirectory2DirectoryInfo.FullName + "/subDirectory2_File.txt");

			if (_directoryInfo.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_directoryInfo.ClearDirectory(true);
		}

		[Test]
		public void ClearDirectory()
		{
			//arrange
			var emptySubDirectory = _directoryInfo.CreateSubdirectory("emptySubDirectory");
			var subDirectory = _directoryInfo.CreateSubdirectory("subDirectory");
			File.Create(subDirectory.FullName + "/subDirectoryFile.txt");

			File.Create(TestingDirectoryPath + "/file1.txt");
			File.Create(TestingDirectoryPath + "/file2.txt");

			if (_directoryInfo.EnumerateFileSystemInfos().Count() != 4) Assert.Fail("Directory not setup correctly");

			//act
			_directoryInfo.ClearDirectory();

			//assert
			Assert.That(_directoryInfo.EnumerateFileSystemInfos(), Is.Empty);
		}

		#endregion
	}
}

