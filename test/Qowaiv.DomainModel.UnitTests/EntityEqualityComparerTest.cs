﻿using NUnit.Framework;
using Qowaiv.DomainModel.UnitTests.Models;

namespace Qowaiv.DomainModel.UnitTests
{
    public class EntityEqualityComparerTest
    {
        [Test]
        public void Equals_NullAndNull_True()
        {
            var comparer = new EntityEqualityComparer<SimpleEntity>();
            Assert.IsTrue(comparer.Equals(null, null));
        }
    }
}
