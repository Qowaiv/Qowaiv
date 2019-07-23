using NUnit.Framework;
using Qowaiv.DomainModel.EventSourcing;
using Qowaiv.TestTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace Qowaiv.DomainModel.UnitTests.EventSourcing
{
    public class EventMessageTest
    {
        [Test]
        public void DebuggerDisplay()
        {
            var info = new EventInfo(1, Guid.Parse("6E1D3455-D1E2-484E-8C54-27F9B0BFE8BA"), new DateTime(2017, 06, 11, 6, 15, 0));
            var @event = new ExampleEvent { Number = 17, Label = "Hello" };
            var message = new EventMessage(info, @event);

            DebuggerDisplayAssert.HasResult("Example, Version: 1, Props[2] { Number: 17, Label: Hello }, 2017-06-11 06:15:00, Aggregate: {6e1d3455-d1e2-484e-8c54-27f9b0bfe8ba}", message);
        }
    }

    internal class ExampleEvent
    {
        public int Number { get; set; }
        public string Label { get; set; }
    }

}
