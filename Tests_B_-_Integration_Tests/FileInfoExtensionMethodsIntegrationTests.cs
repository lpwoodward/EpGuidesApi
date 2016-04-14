using System;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using EpGuidesApi.Domain.DirectoryStuff;
using Mono.Posix;
using Mono.Unix;
using NUnit.Framework.Constraints;
using System.Text.RegularExpressions;
using System.Linq;
using System.Reflection;

namespace EpGuidesApi.Tests.Integration
{
	[TestFixture]
	public class FileInfoExtensionMethodsIntegrationTests : LocalTestBase
	{
		private string TestingDirectoryPath = ConfigurationSettings.AppSettings["TestingDirectoryPath"];

		private FileInfo _instance;
		private DirectoryInfo _directoryInfo;

		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			_directoryInfo = Directory.CreateDirectory(TestingDirectoryPath);

			var file = File.Create(TestingDirectoryPath + "/file.txt");
			_instance = new FileInfo(file.Name);
			file.Close();
		}

		[TearDown]
		public void TearDown()
		{
			_directoryInfo.ClearDirectory();
		}

		[TestFixtureTearDown]
		public void FixtureTearDown()
		{
			_directoryInfo.Delete(true);
		}

		#region CreateHardLink

		[Test]
		public void CreateHardLink_WHERE_not_on_unix_based_system_where_cannot_create_hard_links_SHOULD_do_something_sensible()
		{
			//act
//			_instance.CreateHardLink(TestingDirectoryPath + "/file_hardlink.txt");
			Assert.Fail("Work out what should happen here");
		}
			
		[Test]
		public void CreateHardLink()
		{
			//arrange
			File.WriteAllText(TestingDirectoryPath + "/file.txt", "asdkfjahsd fkajsldhf askdjfha fhjkgasd fjlkadsfjas fkjahdsfk jlaskfjdhskfa sd");
			const string linkFileName = "link.txt";

			//arrange
			var actual = _instance.CreateHardLink(TestingDirectoryPath + "/" + linkFileName);

			//assert
			Assert.That(_directoryInfo.GetFilesAsList(new Regex(linkFileName)), Has.Count.EqualTo(1));
			Assert.That(actual.FullName, Is.EqualTo(TestingDirectoryPath + "/" + linkFileName));

			var fileContents = File.ReadAllText(TestingDirectoryPath + "/" + "file.txt");
			var linkFileContents = File.ReadAllText(TestingDirectoryPath + "/" + linkFileName);
			Assert.That(linkFileContents, Is.EqualTo(fileContents));
		}

		#endregion
	}
}

