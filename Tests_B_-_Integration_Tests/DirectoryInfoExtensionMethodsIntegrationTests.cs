using System;
using NUnit.Framework;
using System.IO;
using EpGuidesApi.Domain.DirectoryStuff;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EpGuidesApi.Tests.Integration
{
	public class DirectoryInfoExtensionMethodsIntegrationTests : LocalTestBase
	{
		private string TestingDirectoryPath = ConfigurationSettings.AppSettings["TestingDirectoryPath"];

		private DirectoryInfo _instance;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_instance = Directory.CreateDirectory(TestingDirectoryPath);
		}

		[TearDown]
		public void TearDown()
		{
			_instance.ClearDirectory();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_instance.Delete(true);
		}

		#region GetFileSystemInfosAsList

		[Test]
		public void GetFileSystemInfosAsList_WHERE_empty_directory_SHOULD_return_empty_list()
		{
			//act
			var actual = _instance.GetFileSystemInfosAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetFileSystemInfosAsList_WHERE_two_files_and_two_directories_exist_only_one_of_each_match_regex_SHOULD_return_list_of_file_and_directory()
		{
			//arrange
			File.Create(TestingDirectoryPath + "/foo.txt");
			var file = File.Create(TestingDirectoryPath + "/bar.txt");

			_instance.CreateSubdirectory("foo");
			var directory = _instance.CreateSubdirectory("bar");

			//act
			var actual = _instance.GetFileSystemInfosAsList(new Regex("bar"));

			//assert
			Assert.That(actual, Has.Count.EqualTo(2));
			Assert.That(actual.Exists(x => x.FullName == file.Name));
			Assert.That(actual.Exists(x => x.FullName == directory.FullName));
		}

		[Test]
		public void GetFileSystemInfosAsList_WHERE_file_and_directory_exists_and_regex_matches_both_in_sub_directory_but_only_searching_top_level_directory_SHOULD_return_empty_list()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("foo");
			File.Create(subDirectory.FullName + "/bar.txt");
			subDirectory.CreateSubdirectory("bar");

			//act
			var actual = _instance.GetFileSystemInfosAsList(new Regex("bar"));

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetFileSystemInfosAsList_WHERE_file_and_directory_exists_in_sub_directory_and_regex_matches_and_searching_recursively_SHOULD_return_list_with_file_and_directory()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("foo");
			var file = File.Create(subDirectory.FullName + "/bar.txt");
			var directory = subDirectory.CreateSubdirectory("bar");

			//act
			var actual = _instance.GetFileSystemInfosAsList(new Regex("bar"), searchOption: SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(2));
			Assert.That(actual.Exists(x => x.FullName == file.Name));
			Assert.That(actual.Exists(x => x.FullName == directory.FullName));
		}

		[Test]
		public void GetFileSystemInfosAsList()
		{
			//arrange
			var file1 = File.Create(TestingDirectoryPath + "/bar1.txt");
			var file2 = File.Create(TestingDirectoryPath + "/bar2.txt");
			var hiddenFile = File.Create(TestingDirectoryPath + "/.bar.txt");
			File.Create(TestingDirectoryPath + "/foo.txt");

			var subDirectory = _instance.CreateSubdirectory("bar");
			var nonMatchingDirectory = _instance.CreateSubdirectory("foo");
			var hiddenDirectory = _instance.CreateSubdirectory(".bar");

			var file1InSubDir1 = File.Create(subDirectory.FullName + "/bar.txt");
			File.Create(subDirectory.FullName + "/foo.csv");

			var fileInSubDir2 = File.Create(nonMatchingDirectory.FullName + "/bar.txt");

			var subSubDirectory = subDirectory.CreateSubdirectory("bar");
			var fileInSubSubDirectory = File.Create(subSubDirectory.FullName + "/bar.txt");

			//act
			var actual = _instance.GetFileSystemInfosAsList(new Regex("bar"), SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(9));
			Assert.That(actual.Exists(x => x.FullName == file1.Name));
			Assert.That(actual.Exists(x => x.FullName == file2.Name));
			Assert.That(actual.Exists(x => x.FullName == hiddenFile.Name));
			Assert.That(actual.Exists(x => x.FullName == subDirectory.FullName));
			Assert.That(actual.Exists(x => x.FullName == hiddenDirectory.FullName));
			Assert.That(actual.Exists(x => x.FullName == file1InSubDir1.Name));
			Assert.That(actual.Exists(x => x.FullName == fileInSubDir2.Name));
			Assert.That(actual.Exists(x => x.FullName == subSubDirectory.FullName));
			Assert.That(actual.Exists(x => x.FullName == fileInSubSubDirectory.Name));
		}

		#endregion

		#region GetDirectoriesAsList

		[Test]
		public void GetDirectoriesAsList_WHERE_empty_directory_SHOULD_return_empty_list()
		{
			//act
			var actual = _instance.GetDirectoriesAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetDirectoriesAsList_WHERE_directory_only_has_files_no_directories_SHOULD_return_empty_list()
		{
			//arrange
			File.Create(TestingDirectoryPath + "/file.txt");

			//act
			var actual = _instance.GetDirectoriesAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetDirectoriesAsList_WHERE_directory_exists_but_search_regex_doesnt_match_SHOULD_return_empty_list()
		{
			//arrange
			_instance.CreateSubdirectory("foo");

			//act
			var actual = _instance.GetDirectoriesAsList(new Regex("bar"));

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetDirectoriesAsList_WHERE_directory_exists_and_regex_matches_sub_sub_directory_but_only_searching_top_level_directory_SHOULD_return_empty_list()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("foo");
			subDirectory.CreateSubdirectory("bar");

			//act
			var actual = _instance.GetDirectoriesAsList(new Regex("bar"));

			//assert
			Assert.That(actual, Is.Empty);
		}

		[TestCase("normal")]
		[TestCase(".hidden")]
		public void GetDirectoriesAsList_WHERE_directory_exists_and_regex_matches_SHOULD_return_list_with_directory(string directoryName)
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory(directoryName);

			//act
			var actual = _instance.GetDirectoriesAsList();

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0].FullName, Is.EqualTo(subDirectory.FullName));
		}

		[Test]
		public void GetDirectoriesAsList_WHERE_sub_sub_directory_exists_in_sub_directory_and_regex_matches_and_searching_recursively_SHOULD_return_list_with_directory()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("foo");
			var subSubDirectory = subDirectory.CreateSubdirectory("bar");

			//act
			var actual = _instance.GetDirectoriesAsList(new Regex("bar"), searchOption: SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0].FullName, Is.EqualTo(subSubDirectory.FullName));
		}

		[Test]
		public void GetDirectoriesAsList()
		{
			//arrange
			var directory1 = _instance.CreateSubdirectory("foo1");
			var directory2 = _instance.CreateSubdirectory("foo2");
			var nonMatchingDirectory = _instance.CreateSubdirectory("bar");

			File.Create(TestingDirectoryPath + "/foo.txt");

			var subDirectory = directory1.CreateSubdirectory("sub-foo1");
			var directoryInNonMatchingDirectory = nonMatchingDirectory.CreateSubdirectory("fooInNonMatchingDir");

			//act
			var actual = _instance.GetDirectoriesAsList(new Regex("foo"), SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(4));
			Assert.That(actual.Exists(x => x.FullName == directory1.FullName));
			Assert.That(actual.Exists(x => x.FullName == directory2.FullName));
			Assert.That(actual.Exists(x => x.FullName == subDirectory.FullName));
			Assert.That(actual.Exists(x => x.FullName == directoryInNonMatchingDirectory.FullName));
		}

		#endregion

		#region GetFilesAsList

		[Test]
		public void GetFilesAsList_WHERE_empty_directory_SHOULD_return_empty_list()
		{
			//act
			var actual = _instance.GetFilesAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetFilesAsList_WHERE_directory_only_has_directories_no_files_SHOULD_return_empty_list()
		{
			//arrange
			_instance.CreateSubdirectory("subDirectory");

			//act
			var actual = _instance.GetFilesAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetFilesAsList_WHERE_file_exists_but_search_regex_doesnt_match_SHOULD_return_empty_list()
		{
			//arrange
			File.Create(TestingDirectoryPath + "/foo.txt");

			//act
			var actual = _instance.GetFilesAsList(new Regex("bar\\.txt"));

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void GetFilesAsList_WHERE_file_exists_and_regex_matches_file_in_sub_directory_but_only_searching_top_level_directory_SHOULD_return_empty_list()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("subDir");
			File.Create(subDirectory.FullName + "/foo.txt");

			//act
			var actual = _instance.GetFilesAsList();

			//assert
			Assert.That(actual, Is.Empty);
		}

		[TestCase("normal")]
		[TestCase(".hidden")]
		public void GetFilesAsList_WHERE_file_exists_and_regex_matches_SHOULD_return_list_with_file(string fileName)
		{
			//arrange
			var file = File.Create(TestingDirectoryPath + "/" + fileName);

			//act
			var actual = _instance.GetFilesAsList();

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0].FullName, Is.EqualTo(file.Name));
		}

		[Test]
		public void GetFilesAsList_WHERE_file_exists_in_sub_directory_and_regex_matches_and_searching_recursively_SHOULD_return_list_with_file()
		{
			//arrange
			var subDirectory = _instance.CreateSubdirectory("subDir");
			var file = File.Create(subDirectory.FullName + "/bar.txt");

			//act
			var actual = _instance.GetFilesAsList(searchOption: SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(1));
			Assert.That(actual[0].FullName, Is.EqualTo(file.Name));
		}

		[Test]
		public void GetFilesAsList()
		{
			//arrange
			var file1 = File.Create(TestingDirectoryPath + "/foo.txt");
			var file2 = File.Create(TestingDirectoryPath + "/bar.txt");
			File.Create(TestingDirectoryPath + "/foo.csv");

			var subDirectory = _instance.CreateSubdirectory("subDir");
			var file1InSubDir1 = File.Create(subDirectory.FullName + "/test.txt");
			var file2InSubDir1 = File.Create(subDirectory.FullName + "/tmp.txt");
			File.Create(subDirectory.FullName + "/tmp.csv");

			var directoryWithMatchingName = _instance.CreateSubdirectory("directory.txt");
			var fileInSubDir2 = File.Create(directoryWithMatchingName.FullName + "/Sara.txt");

			var subSubDirectory = subDirectory.CreateSubdirectory("subSubDirectory");
			var fileInSubSubDirectory = File.Create(subSubDirectory.FullName + "/Laurence.txt");

			//act
			var actual = _instance.GetFilesAsList(new Regex("\\.txt"), SearchOption.AllDirectories);

			//assert
			Assert.That(actual, Has.Count.EqualTo(6));
			Assert.That(actual.Exists(x => x.FullName == file1.Name));
			Assert.That(actual.Exists(x => x.FullName == file2.Name));
			Assert.That(actual.Exists(x => x.FullName == file1InSubDir1.Name));
			Assert.That(actual.Exists(x => x.FullName == file2InSubDir1.Name));
			Assert.That(actual.Exists(x => x.FullName == fileInSubDir2.Name));
			Assert.That(actual.Exists(x => x.FullName == fileInSubSubDirectory.Name));
		}

		#endregion

		#region CleanDirectory

		[Test]
		public void ClearDirectory_WHERE_directory_is_already_empty_SHOULD_leave_directory_empty()
		{
			//act
			_instance.ClearDirectory();

			//assert
			Assert.That(_instance.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_files_in_it_SHOULD_delete_all_files()
		{
			//arrange
			File.Create(TestingDirectoryPath + "/file1.txt");
			File.Create(TestingDirectoryPath + "/file2.txt");

			if (_instance.EnumerateFiles().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_instance.ClearDirectory();

			//assert
			Assert.That(_instance.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_empty_subdirectories_in_it_SHOULD_delete_all_subdirectories()
		{
			//arrange
			_instance.CreateSubdirectory("subDirectory_1");
			_instance.CreateSubdirectory("subDirectory_2");

			if (_instance.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_instance.ClearDirectory();

			//assert
			Assert.That(_instance.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		public void ClearDirectory_WHERE_directory_has_non_empty_subdirectories_in_it_SHOULD_delete_all_subdirectories()
		{
			//arrange
			var subDirectory1DirectoryInfo = _instance.CreateSubdirectory("subDirectory_1");
			var subDirectory2DirectoryInfo = _instance.CreateSubdirectory("subDirectory_2");

			File.Create(subDirectory1DirectoryInfo.FullName + "/subDirectory1_File.txt");
			File.Create(subDirectory2DirectoryInfo.FullName + "/subDirectory2_File.txt");

			if (_instance.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_instance.ClearDirectory();

			//assert
			Assert.That(_instance.EnumerateFileSystemInfos(), Is.Empty);
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "Cannot clear directory, some sub-directories are not empty")]
		public void ClearDirectory_WHERE_directory_has_non_empty_subdirectories_in_it_and_opting_to_not_clear_subdirectories_SHOULD_throw_error()
		{
			//arrange
			var subDirectory1DirectoryInfo = _instance.CreateSubdirectory("subDirectory_1");
			var subDirectory2DirectoryInfo = _instance.CreateSubdirectory("subDirectory_2");

			File.Create(subDirectory1DirectoryInfo.FullName + "/subDirectory1_File.txt");
			File.Create(subDirectory2DirectoryInfo.FullName + "/subDirectory2_File.txt");

			if (_instance.EnumerateDirectories().Count() != 2) Assert.Fail("Directory not setup correctly");

			//act
			_instance.ClearDirectory(true);
		}

		[Test]
		public void ClearDirectory()
		{
			//arrange
			var emptySubDirectory = _instance.CreateSubdirectory("emptySubDirectory");
			var subDirectory = _instance.CreateSubdirectory("subDirectory");
			File.Create(subDirectory.FullName + "/subDirectoryFile.txt");

			File.Create(TestingDirectoryPath + "/file1.txt");
			File.Create(TestingDirectoryPath + "/file2.txt");

			if (_instance.EnumerateFileSystemInfos().Count() != 4) Assert.Fail("Directory not setup correctly");

			//act
			_instance.ClearDirectory();

			//assert
			Assert.That(_instance.EnumerateFileSystemInfos(), Is.Empty);
		}

		#endregion

		#region IsEmpty

		[Test]
		public void IsEmpty_WHERE_directory_is_empty_SHOULD_return_true()
		{
			//act
			var actual = _instance.IsEmpty();

			//assert
			Assert.That(actual);
		}

		[TestCase("normal")]
		[TestCase(".hidden")]
		public void IsEmpty_WHERE_directory_contains_file_SHOULD_return_false(string fileName)
		{
			//arrange
			File.Create(TestingDirectoryPath + "/" + fileName);

			//act
			var actual = _instance.IsEmpty();

			//assert
			Assert.That(actual, Is.False);
		}

		[TestCase("normal")]
		[TestCase(".hidden")]
		public void IsEmpty_WHERE_directory_contains_directory_SHOULD_return_false(string directoryName)
		{
			//arrange
			File.Create(TestingDirectoryPath + "/" + directoryName);

			//act
			var actual = _instance.IsEmpty();

			//assert
			Assert.That(actual, Is.False);
		}

		#endregion
	}
}

