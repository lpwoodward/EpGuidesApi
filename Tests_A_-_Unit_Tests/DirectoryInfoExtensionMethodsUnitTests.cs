using System;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;
using EpGuidesApi.Domain.DirectoryStuff;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EpGuidesApi.Tests.Unit
{
	[TestFixture]
	public class DirectoryInfoExtensionMethodsUnitTests
	{
		private DirectoryInfo _instance;
		private DirectoryInfoExtensionMethodsConcreteObject _concreteObject;

		[SetUp]
		public void Setup()
		{
			_instance = MockRepository.GenerateStrictMock<DirectoryInfo>();
			_concreteObject = MockRepository.GeneratePartialMock<DirectoryInfoExtensionMethodsConcreteObject>();

			DirectoryInfoExtensionMethods.MethodObject = MockRepository.GenerateStrictMock<DirectoryInfoExtensionMethodsConcreteObject>();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			DirectoryInfoExtensionMethods.MethodObject = new DirectoryInfoExtensionMethodsConcreteObject();
		}

		#region GetFileSystemInfosAsList

		[Test]
		public void GetFileSystemInfosAsList_WHERE_not_providing_parameters_SHOULD_call_slave_with_default_parameters()
		{
			//arrange
			var expected = new List<FileSystemInfo> { MockRepository.GenerateStrictMock<FileSystemInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetFileSystemInfosAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(expected);

			//act
			var actual = _instance.GetFileSystemInfosAsList();

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(SearchOption.AllDirectories)]
		[TestCase(SearchOption.TopDirectoryOnly)]
		public void GetFileSystemInfosAsList_calls_slave(SearchOption searchOption)
		{
			//arrange
			var regex = MockRepository.GenerateStrictMock<Regex>();

			var expected = new List<FileSystemInfo> { MockRepository.GenerateStrictMock<FileSystemInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetFileSystemInfosAsListSlave(_instance, regex, searchOption))
				.Return(expected);

			//act
			var actual = _instance.GetFileSystemInfosAsList(regex, searchOption);

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region GetDirectoriesAsList

		[Test]
		public void GetDirectoriesAsList_WHERE_not_providing_parameters_SHOULD_call_slave_with_default_parameters()
		{
			//arrange
			var expected = new List<DirectoryInfo> { MockRepository.GenerateStrictMock<DirectoryInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(expected);

			//act
			var actual = _instance.GetDirectoriesAsList();

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(SearchOption.AllDirectories)]
		[TestCase(SearchOption.TopDirectoryOnly)]
		public void GetDirectoriesAsList_calls_slave(SearchOption searchOption)
		{
			//arrange
			var regex = MockRepository.GenerateStrictMock<Regex>();

			var expected = new List<DirectoryInfo> { MockRepository.GenerateStrictMock<DirectoryInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetDirectoriesAsListSlave(_instance, regex, searchOption))
				.Return(expected);

			//act
			var actual = _instance.GetDirectoriesAsList(regex, searchOption);

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region GetFilesAsList

		[Test]
		public void GetFilesAsList_WHERE_not_providing_parameters_SHOULD_call_slave_with_default_parameters()
		{
			//arrange
			var expected = new List<FileInfo> { MockRepository.GenerateStrictMock<FileInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(expected);

			//act
			var actual = _instance.GetFilesAsList();

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[TestCase(SearchOption.AllDirectories)]
		[TestCase(SearchOption.TopDirectoryOnly)]
		public void GetFilesAsList_calls_slave(SearchOption searchOption)
		{
			//arrange
			var regex = MockRepository.GenerateStrictMock<Regex>();

			var expected = new List<FileInfo> { MockRepository.GenerateStrictMock<FileInfo>() };
			DirectoryInfoExtensionMethods.MethodObject.Expect(x =>
					x.GetFilesAsListSlave(_instance, regex, searchOption))
				.Return(expected);

			//act
			var actual = _instance.GetFilesAsList(regex, searchOption);

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		#endregion

		#region IsEmpty

		[TestCase(true)]
		[TestCase(false)]
		public void IsEmpty_calls_slave(bool expected)
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Stub(x => x.IsEmptySlave(_instance)).Return(expected);

			//act
			var actual = _instance.IsEmpty();

			//assert
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		public void IsEmptySlave_WHERE_directory_has_fileSystemInfo_entires_SHOULD_return_false()
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFileSystemInfosAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileSystemInfo>{ MockRepository.GenerateStrictMock<FileSystemInfo>() });

			//act
			var actual = _concreteObject.IsEmptySlave(_instance);

			//assert
			Assert.That(actual, Is.False);
		}

		[Test]
		public void IsEmptySlave_WHERE_directory_has_no_fileSystemInfo_entires_SHOULD_return_true()
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFileSystemInfosAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileSystemInfo>());

			//act
			var actual = _concreteObject.IsEmptySlave(_instance);

			//assert
			Assert.That(actual);
		}

		#endregion

		#region ClearDirectory

		[Test]
		public void ClearDirectory_WHERE_not_providing_parameters_SHOULD_call_slave_with_defaults()
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Expect(x => x.ClearDirectorySlave(_instance, true));

			//act
			_instance.ClearDirectory(true);

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
		}

		[TestCase(true)]
		[TestCase(false)]
		public void ClearDirectory_calls_slave(bool throwErrors)
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Expect(x => x.ClearDirectorySlave(_instance, throwErrors));

			//act
			_instance.ClearDirectory(throwErrors);

			//assert
			DirectoryInfoExtensionMethods.MethodObject.VerifyAllExpectations();
		}

		[Test]
		public void ClearDirectorySlave_WHERE_throwing_errors_and_directory_has_no_directories_or_files_SHOULD_do_nothing()
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo>());

			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileInfo>());

			//act
			_concreteObject.ClearDirectorySlave(_instance, true);
		}

		[Test]
		public void ClearDirectorySlave_WHERE_throwing_errors_and_directory_has_single_directory_which_is_empty_SHOULD_delete_directory()
		{
			//arrange
			var directoryInfo = MockRepository.GenerateStrictMock<DirectoryInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo> { directoryInfo });

			DirectoryInfoExtensionMethods.MethodObject.Stub(x => x.IsEmptySlave(directoryInfo)).Return(true);

			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileInfo>());

			directoryInfo.Expect(x => x.Delete(true));

			//act
			_concreteObject.ClearDirectorySlave(_instance, true);

			//assert
			directoryInfo.VerifyAllExpectations();
		}

		[Test]
		[ExpectedException(typeof(Exception), ExpectedMessage = "Cannot clear directory, some sub-directories are not empty")]
		public void ClearDirectorySlave_WHERE_throwing_errors_and_directory_has_single_directory_which_is_not_empty_SHOULD_throw_error()
		{
			//arrange
			var directoryInfo = MockRepository.GenerateStrictMock<DirectoryInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo> { directoryInfo });

			DirectoryInfoExtensionMethods.MethodObject.Stub(x => x.IsEmptySlave(directoryInfo)).Return(false);

			//act
			_concreteObject.ClearDirectorySlave(_instance, true);
		}

		[Test]
		public void ClearDirectorySlave_WHERE_not_throwing_errors_and_directory_has_single_directory_SHOULD_delete_directory()
		{
			//arrange
			var directoryInfo = MockRepository.GenerateStrictMock<DirectoryInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo> { directoryInfo });

			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileInfo>());

			directoryInfo.Expect(x => x.Delete(true));

			//act
			_concreteObject.ClearDirectorySlave(_instance, false);

			//assert
			directoryInfo.VerifyAllExpectations();
		}

		[Test]
		public void ClearDirectorySlave_WHERE_directory_has_single_file_SHOULD_delete_file()
		{
			//arrange
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo>());

			var fileInfo = MockRepository.GenerateStrictMock<FileInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileInfo> { fileInfo });

			fileInfo.Expect(x => x.Delete());

			//act
			_concreteObject.ClearDirectorySlave(_instance, false);

			//assert
			fileInfo.VerifyAllExpectations();
		}

		[Test]
		public void ClearDirectorySlave_WHERE_directory_has_multiple_directories_and_files_SHOULD_delete_all_directories_and_files()
		{
			//arrange
			var directoryInfo1 = MockRepository.GenerateStrictMock<DirectoryInfo>();
			var directoryInfo2 = MockRepository.GenerateStrictMock<DirectoryInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetDirectoriesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<DirectoryInfo> { directoryInfo1, directoryInfo2 });

			var fileInfo1 = MockRepository.GenerateStrictMock<FileInfo>();
			var fileInfo2 = MockRepository.GenerateStrictMock<FileInfo>();
			DirectoryInfoExtensionMethods.MethodObject.Stub(x =>
					x.GetFilesAsListSlave(_instance, null, SearchOption.TopDirectoryOnly))
				.Return(new List<FileInfo> { fileInfo1, fileInfo2 });

			fileInfo1.Expect(x => x.Delete());
			fileInfo2.Expect(x => x.Delete());

			directoryInfo1.Expect(x => x.Delete(true));
			directoryInfo2.Expect(x => x.Delete(true));

			//act
			_concreteObject.ClearDirectorySlave(_instance, false);

			//assert
			fileInfo1.VerifyAllExpectations();
			fileInfo2.VerifyAllExpectations();
			directoryInfo1.VerifyAllExpectations();
			directoryInfo2.VerifyAllExpectations();
		}

		#endregion
	}
}

