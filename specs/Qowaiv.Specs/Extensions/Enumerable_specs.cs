using FluentAssertions;
using NUnit.Framework;
using Qowaiv.Statistics;
using System.Linq;

namespace Extensions.Enumerable_specs
{
    public class Avarage
    {
        [Test]
        public void Elo()
        {
            var elos = new Elo[] { 1400, 1600 };
            elos.Avarage().Should().Be(1500.Elo());
        }
    }
}
