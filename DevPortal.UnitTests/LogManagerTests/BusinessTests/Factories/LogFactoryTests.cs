using AB.Framework.UnitTests;
using DevPortal.Log.Business;
using DevPortal.Model;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;

namespace DevPortal.UnitTests.LogManagerTests.BusinessTests.Factories
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class LogFactoryTests : LooseBaseTestFixture
    {
        #region members & setup

        LogFactory factory;

        [SetUp]
        public void Initialize()
        {
            factory = new LogFactory();
        }

        #endregion

        [Test]
        public void CreateLogFile_NoCondition_ReturnLogFile()
        {
            //Arrange
            string path = "path";
            DateTime dateModified = DateTime.Now;
            string expectedDateModified = string.Format("{0:g}", DateTime.Now);
            string size = "12";
            DateTime creationTime = DateTime.Now;

            //Act
            var result = factory.CreateLogFile(path, dateModified, size, creationTime);

            //Assert
            result.CreationTime.Should().Be(creationTime);
            result.Size.Should().Be(size);
            result.DateModified.Should().Be(expectedDateModified);
            result.Path.Should().Be(path);
        }

        [Test]
        public void CreateListLogFile_NoCondition_ReturnLogFile()
        {
            //Arrange
            Collection<LogFileModel> logFiles = new Collection<LogFileModel>();

            //Act
            var result = factory.CreateListLogFile();

            //Assert
            result.Count.Should().Be(logFiles.Count);
            result.GetType().Should().Be(logFiles.GetType());
        }
    }
}