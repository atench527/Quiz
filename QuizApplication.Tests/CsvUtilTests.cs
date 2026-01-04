using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using QuizApplication;

namespace QuizApplication.Tests
{
    [TestClass]
    public class CsvUtilTests
    {
        // Escape: null becomes empty string
        [TestMethod]
        public void Escape_Null_ShouldReturnEmpty()
        {
            Assert.AreEqual("", CsvUtil.Escape(null));
        }

        // Escape: no special chars returns same
        [TestMethod]
        public void Escape_NoSpecialChars_ShouldReturnSame()
        {
            Assert.AreEqual("hello", CsvUtil.Escape("hello"));
        }

        // Escape: comma forces quotes
        [TestMethod]
        public void Escape_WithComma_ShouldQuote()
        {
            Assert.AreEqual("\"a,b\"", CsvUtil.Escape("a,b"));
        }

        // Escape: quote is doubled and string is quoted
        [TestMethod]
        public void Escape_WithQuote_ShouldDoubleQuotesAndQuote()
        {
            Assert.AreEqual("\"a\"\"b\"", CsvUtil.Escape("a\"b"));
        }

        // Escape: newline forces quotes
        [TestMethod]
        public void Escape_WithNewline_ShouldQuote()
        {
            Assert.AreEqual("\"a\nb\"", CsvUtil.Escape("a\nb"));
        }

        // SplitLine: null returns empty list
        [TestMethod]
        public void SplitLine_Null_ShouldReturnEmptyList()
        {
            List<string> parts = CsvUtil.SplitLine(null);
            Assert.AreEqual(0, parts.Count);
        }

        // SplitLine: basic commas split into fields
        [TestMethod]
        public void SplitLine_BasicCommaSeparated_ShouldSplit()
        {
            List<string> parts = CsvUtil.SplitLine("a,b,c");
            Assert.AreEqual(3, parts.Count);
            Assert.AreEqual("a", parts[0]);
            Assert.AreEqual("b", parts[1]);
            Assert.AreEqual("c", parts[2]);
        }

        // SplitLine: quoted commas stay in one field
        [TestMethod]
        public void SplitLine_QuotedComma_ShouldKeepTogether()
        {
            List<string> parts = CsvUtil.SplitLine("\"a,b\",c");
            Assert.AreEqual(2, parts.Count);
            Assert.AreEqual("a,b", parts[0]);
            Assert.AreEqual("c", parts[1]);
        }

        // SplitLine: escaped quotes inside quotes
        [TestMethod]
        public void SplitLine_EscapedQuotes_ShouldUnescape()
        {
            List<string> parts = CsvUtil.SplitLine("\"a\"\"b\",c");
            Assert.AreEqual(2, parts.Count);
            Assert.AreEqual("a\"b", parts[0]);
            Assert.AreEqual("c", parts[1]);
        }

        // JoinPipe: null/empty returns empty string
        [TestMethod]
        public void JoinPipe_NullOrEmpty_ShouldReturnEmpty()
        {
            Assert.AreEqual("", CsvUtil.JoinPipe(null));
            Assert.AreEqual("", CsvUtil.JoinPipe(new List<string>()));
        }

        // JoinPipe: escapes | and \
        [TestMethod]
        public void JoinPipe_ShouldEscapePipeAndBackslash()
        {
            string joined = CsvUtil.JoinPipe(new List<string> { "a|b", "c\\d" });
            Assert.AreEqual("a\\|b|c\\\\d", joined);
        }

        // SplitPipe: empty returns empty list
        [TestMethod]
        public void SplitPipe_Empty_ShouldReturnEmptyList()
        {
            List<string> parts = CsvUtil.SplitPipe("");
            Assert.AreEqual(0, parts.Count);
        }

        // SplitPipe: reverses JoinPipe behaviour
        [TestMethod]
        public void SplitPipe_ShouldUnescapePipeAndBackslash()
        {
            List<string> parts = CsvUtil.SplitPipe("a\\|b|c\\\\d");
            Assert.AreEqual(2, parts.Count);
            Assert.AreEqual("a|b", parts[0]);
            Assert.AreEqual("c\\d", parts[1]);
        }
    }
}
