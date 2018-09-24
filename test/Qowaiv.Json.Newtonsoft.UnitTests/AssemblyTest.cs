using NUnit.Framework;
using Qowaiv.TestTools;
using System;

namespace Qowaiv.Json.UnitTests
{
    public class AssemblyTest : AssemblyTestBase
    {
        protected override Type AssemblyType => typeof(QowaivJsonConverter);

        protected override Type[] Svos => new Type[0];
    }
}

