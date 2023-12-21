namespace Qowaiv.UnitTests.Statistics
{
    /// <summary>Tests the Elo SVO.</summary>
    public class EloTest
    {
        /// <summary>The test instance for most tests.</summary>
        public static readonly Elo TestStruct = 1732.4;

        #region Elo const tests

        /// <summary>Elo.Zero should be equal to the default of Elo.</summary>
        [Test]
        public void Zero_None_EqualsDefault()
        {
            Elo.Zero.Should().Be(default(Elo));
        }
        [Test]
        public void MinValue_None_DoubleMinValue()
        {
            Elo.MinValue.Should().Be((Elo)double.MinValue);
        }
        [Test]
        public void MaxValue_None_DoubleMaxValue()
        {
            Elo.MaxValue.Should().Be((Elo)double.MaxValue);
        }

        #endregion

        #region Methods

        [Test]
        public void GetZScore_Delta100_0Dot64()
        {
            var act = Elo.GetZScore(1600, 1500);
            var exp = 0.64;

            Assert.AreEqual(exp, act, 0.001);
        }

        #endregion

        #region TryParse tests

        /// <summary>TryParse with specified string value should be valid.</summary>
        [Test]
        public void TryParse_StringValue_IsValid()
        {
            string str = "1400";
            Elo.TryParse(str, out Elo val).Should().BeTrue();
            Assert.AreEqual(str, val.ToString(), "Value");
        }

        [Test]
        public void Parse_InvalidInput_ThrowsFormatException()
        {
            using (TestCultures.En_GB.Scoped())
            {
                Assert.Catch<FormatException>
                (() =>
                {
                    Elo.Parse("InvalidInput");
                },
                "Not a valid Elo");
            }
        }

        [Test]
        public void TryParse_TestStructInput_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var exp = TestStruct;
                var act = Elo.TryParse(exp.ToString());

                act.Should().Be(exp);
            }
        }

        #endregion

        #region (XML) (De)serialization tests

#if NET8_0_OR_GREATER
#else
        [Test]
        public void GetObjectData_SerializationInfo_AreEqual()
        {
            ISerializable obj = TestStruct;
            var info = new SerializationInfo(typeof(Elo), new System.Runtime.Serialization.FormatterConverter());
            obj.GetObjectData(info, default);

            info.GetDouble("Value").Should().Be(1732.4000000000001);
        }

        [Test]
        [Obsolete("Usage of the binary formatter is considered harmful.")]
        public void SerializeDeserialize_TestStruct_AreEqual()
        {
            var input = EloTest.TestStruct;
            var exp = EloTest.TestStruct;
            var act = SerializeDeserialize.Binary(input);
            act.Should().Be(exp);
        }
#endif

        [Test]
        public void DataContractSerializeDeserialize_TestStruct_AreEqual()
        {
            var input = EloTest.TestStruct;
            var exp = EloTest.TestStruct;
            var act = SerializeDeserialize.DataContract(input);
            act.Should().Be(exp);
        }

        [Test]
        public void XmlSerialize_TestStruct_AreEqual()
        {
            var act = Serialize.Xml(TestStruct);
            var exp = "1732.4";
            act.Should().Be(exp);
        }

        [Test]
        public void XmlDeserialize_XmlString_AreEqual()
        {
            var act = Deserialize.Xml<Elo>("1732.4");
            act.Should().Be(TestStruct);
        }

#if NET8_0_OR_GREATER
#else
        [Test]
        [Obsolete("Usage of the binary formatter is considered harmful.")]
        public void SerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
#endif

        [Test]
        public void XmlSerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var act = SerializeDeserialize.Xml(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
        [Test]
        public void DataContractSerializeDeserialize_EloSerializeObject_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = EloTest.TestStruct,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var act = SerializeDeserialize.DataContract(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }

#if NET8_0_OR_GREATER
#else
        [Test]
        [Obsolete("Usage of the binary formatter is considered harmful.")]
        public void SerializeDeserialize_Zero_AreEqual()
        {
            var input = new EloSerializeObject
            {
                Id = 17,
                Obj = Elo.Zero,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var exp = new EloSerializeObject
            {
                Id = 17,
                Obj = Elo.Zero,
                Date = new DateTime(1970, 02, 14, 00, 00, 000, DateTimeKind.Local),
            };
            var act = SerializeDeserialize.Binary(input);
            Assert.AreEqual(exp.Id, act.Id, "Id");
            Assert.AreEqual(exp.Obj, act.Obj, "Obj");
            Assert.AreEqual(exp.Date, act.Date, "Date");
        }
#endif

        [Test]
        public void GetSchema_None_IsNull()
        {
            IXmlSerializable obj = TestStruct;
            obj.GetSchema().Should().BeNull();
        }

        #endregion

        #region IFormattable / ToString tests

        [Test]
        public void ToString_CustomFormatter_SupportsCustomFormatting()
        {
            var act = TestStruct.ToString("00000", FormatProvider.CustomFormatter);
            var exp = "Unit Test Formatter, value: '01732', format: '00000'";

            act.Should().Be(exp);
        }
        [Test]
        public void ToString_TestStruct_ComplexPattern()
        {
            using (new CultureInfoScope(CultureInfo.InvariantCulture))
            {
                var act = TestStruct.ToString(string.Empty);
                var exp = "1732.4";
                act.Should().Be(exp);
            }
        }

        [Test]
        public void ToString_ValueDutchBelgium_AreEqual()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                var act = Elo.Parse("1600,1").ToString();
                var exp = "1600,1";
                act.Should().Be(exp);
            }
        }

        [Test]
        public void ToString_ValueEnglishGreatBritain_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Elo.Parse("1600.1").ToString();
                var exp = "1600.1";
                act.Should().Be(exp);
            }
        }

        [Test]
        public void ToString_FormatValueDutchBelgium_AreEqual()
        {
            using (TestCultures.Nl_BE.Scoped())
            {
                var act = Elo.Parse("800").ToString("0000");
                var exp = "0800";
                act.Should().Be(exp);
            }
        }

        [Test]
        public void ToString_FormatValueEnglishGreatBritain_AreEqual()
        {
            using (TestCultures.En_GB.Scoped())
            {
                var act = Elo.Parse("800").ToString("0000");
                var exp = "0800";
                act.Should().Be(exp);
            }
        }

        [Test]
        public void ToString_FormatValueSpanishEcuador_AreEqual()
        {
            var act = Elo.Parse("1700").ToString("00000.0", new CultureInfo("es-EC"));
            var exp = "01700,0";
            act.Should().Be(exp);
        }

        #endregion

        #region IEquatable tests

        [Test]
        public void Equals_FormattedAndUnformatted_IsTrue()
        {
            var l = Elo.Parse("1600", CultureInfo.InvariantCulture);
            var r = Elo.Parse("1,600.00*", CultureInfo.InvariantCulture);

            l.Equals(r).Should().BeTrue();
        }

        #endregion

        #region IComparable tests

        /// <summary>Orders a list of Elos ascending.</summary>
        [Test]
        public void OrderBy_Elo_AreEqual()
        {
            Elo item0 = 1601;
            Elo item1 = 2371;
            Elo item2 = 2416;
            Elo item3 = 2601;

            var inp = new List<Elo> { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
            var exp = new List<Elo> { Elo.Zero, Elo.Zero, item0, item1, item2, item3 };
            var act = inp.OrderBy(item => item).ToList();

            act.Should().BeEquivalentTo(exp);
        }

        /// <summary>Orders a list of Elos descending.</summary>
        [Test]
        public void OrderByDescending_Elo_AreEqual()
        {
            Elo item0 = 1601;
            Elo item1 = 2371;
            Elo item2 = 2416;
            Elo item3 = 2601;

            var inp = new List<Elo> { Elo.Zero, item3, item2, item0, item1, Elo.Zero };
            var exp = new List<Elo> { item3, item2, item1, item0, Elo.Zero, Elo.Zero };
            var act = inp.OrderByDescending(item => item).ToList();

            act.Should().BeEquivalentTo(exp);
        }

        /// <summary>Compare with a to object casted instance should be fine.</summary>
        [Test]
        public void CompareTo_ObjectTestStruct_0()
        {
            object other = TestStruct;

            var exp = 0;
            var act = TestStruct.CompareTo(other);

            act.Should().Be(exp);
        }

        /// <summary>Compare with a random object should throw an exception.</summary>
        [Test]
        public void CompareTo_newObject_ThrowsArgumentException()
        {
            Func<int> compare = () => TestStruct.CompareTo(new object());
            compare.Should().Throw<ArgumentException>();
        }

        [Test]
        public void LessThan_17LT19_IsTrue()
        {
            Elo l = 17;
            Elo r = 19;

            (l < r).Should().BeTrue();
        }
        [Test]
        public void GreaterThan_21LT19_IsTrue()
        {
            Elo l = 21;
            Elo r = 19;

            (l > r).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqual_17LT19_IsTrue()
        {
            Elo l = 17;
            Elo r = 19;

            (l <= r).Should().BeTrue();
        }
        [Test]
        public void GreaterThanOrEqual_21LT19_IsTrue()
        {
            Elo l = 21;
            Elo r = 19;

            (l >= r).Should().BeTrue();
        }

        [Test]
        public void LessThanOrEqual_17LT17_IsTrue()
        {
            Elo l = 17;
            Elo r = 17;

            (l <= r).Should().BeTrue();
        }
        [Test]
        public void GreaterThanOrEqual_21LT21_IsTrue()
        {
            Elo l = 21;
            Elo r = 21;

            (l >= r).Should().BeTrue();
        }
        #endregion

        #region Casting tests

        [Test]
        public void Implicit_DoubleToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600.1);
            Elo act = 1600.1;

            act.Should().Be(exp);
        }
        [Test]
        public void Explicit_EloToDouble_AreEqual()
        {
            var exp = 1600.1;
            var act = (double)Elo.Create(1600.1);

            act.Should().Be(exp);
        }

        [Test]
        public void Implicit_DecimalToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600.1);
            Elo act = 1600.1m;

            act.Should().Be(exp);
        }
        [Test]
        public void Explicit_EloToDecimal_AreEqual()
        {
            var exp = 1600.1m;
            var act = (decimal)Elo.Create(1600.1);

            act.Should().Be(exp);
        }


        [Test]
        public void Implicit_Int32ToElo_AreEqual()
        {
            Elo exp = Elo.Create(1600);
            Elo act = 1600;

            act.Should().Be(exp);
        }
        [Test]
        public void Explicit_EloToInt32_AreEqual()
        {
            var exp = 1600;
            var act = (int)Elo.Create(1600);

            act.Should().Be(exp);
        }

        #endregion

        #region Properties
        #endregion

        #region Operators

        [Test]
        public void Add_1600Add100_1700()
        {
            Elo l = 1600;
            Elo r = 100;

            Elo act = l + r;
            Elo exp = 1700;

            act.Should().Be(exp);
        }

        [Test]
        public void Subtract_1600Subtract100_1500()
        {
            Elo l = 1600;
            Elo r = 100;

            Elo act = l - r;
            Elo exp = 1500;

            act.Should().Be(exp);
        }

        [Test]
        public void Divide_1600Subtract100_800()
        {
            Elo act = 1600m;

            act /= 2.0;

            Elo exp = 800;

            act.Should().Be(exp);
        }

        [Test]
        public void Multiply_3200()
        {
            Elo act = 1600m;

            act *= 2.0;

            Elo exp = 3200;

            act.Should().Be(exp);
        }

        [Test]
        public void Increment_1600_1601()
        {
            Elo act = 1600;
            act++;

            Elo exp = 1601;

            act.Should().Be(exp);
        }

        [Test]
        public void Decrement_1600_1599()
        {
            Elo act = 1600;
            act--;

            Elo exp = 1599;

            act.Should().Be(exp);
        }

        [Test]
        public void Negate_Min1600_1600()
        {
            Elo act = -1600;

            act = -act;

            Elo exp = 1600;

            act.Should().Be(exp);
        }

        [Test]
        public void Plus_1600_1600()
        {
            Elo act = 1600;

            act = +act;

            Elo exp = 1600;

            act.Should().Be(exp);
        }
        #endregion
    }

    [Serializable]
    public class EloSerializeObject
    {
        public int Id { get; set; }
        public Elo Obj { get; set; }
        public DateTime Date { get; set; }
    }
}
