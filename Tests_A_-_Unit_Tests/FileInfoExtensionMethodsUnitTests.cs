using System;
using NUnit.Framework;
using System.IO;
using EpGuidesApi.Domain.DirectoryStuff;
using Rhino.Mocks;

namespace EpGuidesApi.Tests.Unit
{
	[TestFixture]
	public class FileInfoExtensionMethodsUnitTests
	{
		private FileInfo _fileInfo;
		private FileInfoExtensionMethodsConcreteObject _concreteObject;

		[SetUp]
		public void Setup()
		{
			_fileInfo = MockRepository.GenerateStrictMock<FileInfo>();
			_concreteObject = MockRepository.GeneratePartialMock<FileInfoExtensionMethodsConcreteObject>();

			FileInfoExtensionMethods.MethodObject = MockRepository.GenerateStrictMock<FileInfoExtensionMethodsConcreteObject>();
		}

		[TestFixtureTearDown]
		public void TearDown()
		{
			FileInfoExtensionMethods.MethodObject = new FileInfoExtensionMethodsConcreteObject();
		}

		#region CreateHardLink

		[Test]
		public void CreateHardLinkSlave_calls_slave()
		{
			//arrange
			const string linkFileName = "khjhjhagsdf";
			var expected = MockRepository.GenerateStrictMock<FileInfo>();

			FileInfoExtensionMethods.MethodObject.Expect(x => x.CreateHardLinkSlave(_fileInfo, linkFileName)).Return(expected);

			//act
			var actual = _fileInfo.CreateHardLink(linkFileName);

			//assert
			FileInfoExtensionMethods.MethodObject.VerifyAllExpectations();
			Assert.That(actual, Is.EqualTo(expected));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException), ExpectedMessage = "Cannot create hardlink with same path as original file")]
		public void CreateHardLinkSlave_WHERE_hard_link_path_is_same_as_original_file_SHOULD_throw_error()
		{
			//arrange
			const string linkFullName = "kakjhasgdsf";
			_fileInfo.Stub(x => x.FullName).Return(linkFullName);

			_concreteObject.CreateHardLinkSlave(_fileInfo, linkFullName);
		}

		#endregion
	}
}

