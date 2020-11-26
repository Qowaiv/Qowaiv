﻿using NUnit.Framework;
using Qowaiv.Text;
using System.Globalization;

namespace CharBuffer_specs
{
    public class Add
    {
        private CharBuffer WithCapacity() => CharBuffer.Empty(16);

        [Test]
        public void A_char_can_be_added()
        {
            var buffer = WithCapacity().Add('c');
            Assert.AreEqual("c", buffer);
        }

        [Test]
        public void A_char_can_be_added_as_lowercase()
        {
            var buffer = WithCapacity().AddLower('C');
            Assert.AreEqual("c", buffer);
        }

        [Test]
        public void A_string_can_be_added()
        {
            var buffer = WithCapacity().Add("string");
            Assert.AreEqual("string", buffer);
        }

        [Test]
        public void A_CharBuffer_can_be_added()
        {
            var other = " string ".Buffer().Trim();
            var buffer = WithCapacity().Add(other);
            Assert.AreEqual("string", buffer);
        }
    }

    public class Remove
    {
        [TestCase("Test", "this is a Test!", "this is a !")]
        [TestCase("x", "What a fxckin' xwxsxmx feature.", "What a fckin' wsm feature.")]
        [TestCase("%", "-3%", "-3")]
        public void string_values_from_buffer(string remove, string input, string expected)
        {
            var buffer = input.Buffer();
            Assert.AreEqual(expected, buffer.Remove(remove).ToString());
        }

        [Test]
        public void FromStart_removes_characters_from_the_start()
        {
            var buffer = " test ".Buffer().Trim().RemoveFromStart(2);
            Assert.AreEqual("st", buffer);
        }
        
        [Test]
        public void FromEnd_removes_characters_from_the_end()
        {
            var buffer = " test ".Buffer().Trim().RemoveFromEnd(2);
            Assert.AreEqual("te", buffer);
        }

        [Test]
        public void Range_removes_specified_range()
        {
            var buffer = " test ".Buffer().Trim().RemoveRange(1, 2);
            Assert.AreEqual("tt", buffer);
        }

        [TestCase('\t')]
        [TestCase(' ')]
        [TestCase('\r')]
        [TestCase('\n')]
        [TestCase((char)0160)]
        public void WhiteSpace_removes_all_white_space_chars(char ch)
        {
            var buffer = $"{ch} Hello.{ch}world!  ".Buffer().RemoveWhiteSpace();
            Assert.AreEqual("Hello.world!", buffer);
        }

        [TestCase('.')]
        [TestCase('-')]
        [TestCase('_')]
        [TestCase((char)0x00B7)] // middle dot
        [TestCase((char)0x22C5)] // dot operator
        [TestCase((char)0x2202)] // bullet
        [TestCase((char)0x2012)] // figure dash / minus
        [TestCase((char)0x2013)] // en dash
        [TestCase((char)0x2014)] // em dash
        [TestCase((char)0x2015)] // horizontal bar
        public void Markup_removes_all_markup_chars(char ch)
        {
            var buffer = $"{ch} Hello,{ch}world!  ".Buffer().RemoveMarkup();
            Assert.AreEqual("Hello,world!", buffer);
        }
    }

    public class Clear
    {
        [Test]
        public void Empties_the_buffer()
        {
            Assert.IsTrue("test".Buffer().Clear().IsEmpty());
        }
    }

    public class Trim
    {
        [Test]
        public void Trims_left_and_right_spaces()
        {
            var buffer = "  content ".Buffer().Trim();
            Assert.AreEqual("content", buffer);
        }

        [Test]
        public void Left_trims_left_spaces()
        {
            var buffer = "  content ".Buffer().TrimLeft();
            Assert.AreEqual("content ", buffer);
        }

        [Test]
        public void Right_trims_right_spaces()
        {
            var buffer = "  content ".Buffer().TrimRight();
            Assert.AreEqual("  content", buffer);
        }
    }

    public class Uppercase
    {
        [Test]
        public void Transforms_all_characters_to__uppercase_alternatives()
        {
            var buffer = "abcDeéf".Buffer().Uppercase();
            Assert.AreEqual("ABCDEÉF", buffer);
        }
    }

    public class Count
    {
        [Test]
        public void Returns_the_total_of_specified_char()
        {
            var buffer = " let us count this   ".Buffer().Trim();
            Assert.AreEqual(3, buffer.Count(' '));
        }
    }

    public class IndexOf
    {
        [Test]
        public void Returns_index_of_char()
        {
            var buffer = " 0123456777   ".Buffer().Trim();
            Assert.AreEqual(7, buffer.IndexOf('7'));
        }

        [Test]
        public void Last_returns_last_index_of_char()
        {
            var buffer = " 7123456789   ".Buffer().Trim();
            Assert.AreEqual(7, buffer.LastIndexOf('7'));
        }
    }

    public class StartsWith_String
    {
        [Test]
        public void Longer_then_buffer_is_false()
        {
            var buffer = " test".Buffer().RemoveFromEnd(1);
            Assert.IsFalse(buffer.StartsWith("testx"));
        }

        [Test]
        public void That_matches_buffer_but_not_al_visible_is_false()
        {
            var buffer = " test".Buffer().Trim().RemoveFromEnd(1);
            Assert.IsFalse(buffer.StartsWith("test"));
        }

        [Test]
        public void Not_matching_buffer_is_false()
        {
            var buffer = " test".Buffer().Trim();
            Assert.IsFalse(buffer.StartsWith("xe"));
        }

        [Test]
        public void Matching_buffer_is_true()
        {
            var buffer = " test".Buffer().Trim();
            Assert.IsTrue(buffer.StartsWith("tes"));
        }
    }

    public class Substring
    {
        [Test]
        public void With_start_index_returns_substring()
        {
            var buffer = " test ".Buffer().Trim();
            Assert.AreEqual("st", buffer.Substring(2));
        }

        [Test]
        public void With_start_index_and_length_returns_substring()
        {
            var buffer = " test ".Buffer().Trim();
            Assert.AreEqual("es", buffer.Substring(1, 2));
        }
    }

    public class IsEmpty
    {
        [Test]
        public void Is_true_for_buffer_without_visable_content()
        {
            var buffer = "   ".Buffer().Trim();
            Assert.IsTrue(buffer.IsEmpty());
        }
    }

    public class IsUnknown
    {
        [Test]
        public void Is_true_when_the_ToString_represents_an_unknown_value()
        {
            var buffer = "?".Buffer();
            Assert.IsTrue(buffer.IsUnknown(CultureInfo.InvariantCulture));
        }
        
        [Test]
        public void Is_false_when_the_ToString_does_not_represent_an_unknown_value()
        {
            var buffer = "some value".Buffer();
            Assert.IsFalse(buffer.IsUnknown(CultureInfo.InvariantCulture));
        }
    }
}
