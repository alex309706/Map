using NUnit.Framework;
using Infrastructure.Data;
using Domain.Core;
using Interfaces;
using System.Collections.Generic;

namespace NUnitTestProject
{
    public class Tests
    {
        ISubdivisionRepository repo;
        IEnumerable<Subdivision> subdivisions;
        [SetUp]
        public void Setup()
        {
            repo = new ExcelSubdivisionRepository(@"E:\Саша\Начало\Наполнение\Книга_с_группировкой_(2).xlsx");
            subdivisions = repo.GetSubdivisions();
        }
        [Test]
        public void Count_Test()
        {
            IEnumerator<Subdivision> ie = subdivisions.GetEnumerator();
            int count = 0;
             while(ie.MoveNext())
            {
                count++;
            }
            Assert.IsNotNull(count);
        }
        [Test]
        public void DefaultConnection_Test()
        {
            ISubdivisionRepository newRepo = new ExcelSubdivisionRepository(@"E:\Саша\Начало\Наполнение\Книга_с_группировкой_(2).xlsx");
            IEnumerable<Subdivision> newSubdivisions = newRepo.GetSubdivisions();
            Assert.IsNotNull(newRepo);
        }
        [Test]
        public void GetSubdivision_Test()
        {
            Subdivision returnedSubdivision = repo.GetSubdivision(1);
            Assert.IsNotNull(returnedSubdivision);
        }

    }
}