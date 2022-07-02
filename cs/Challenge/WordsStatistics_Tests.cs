using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using static System.String;

namespace Challenge
{
	[TestFixture]
	public class WordsStatistics_Tests
	{
		public virtual IWordsStatistics CreateStatistics()
		{
			// меняется на разные реализации при запуске exe
			return new WordsStatistics();
		}

		private IWordsStatistics wordsStatistics;

		[SetUp]
		public void SetUp()
		{
			wordsStatistics = CreateStatistics();
		}

		[Test]
		public void GetStatistics_IsEmpty_AfterCreation()
		{
			wordsStatistics.GetStatistics().Should().BeEmpty();
		}

		[Test]
		public void GetStatistics_ContainsItem_AfterAddition()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.GetStatistics().Should().Equal(new WordCount("abc", 1));
		}

		[Test]
		public void GetStatistics_ContainsManyItems_AfterAdditionOfDifferentWords()
		{
			wordsStatistics.AddWord("abc");
			wordsStatistics.AddWord("def");
			wordsStatistics.GetStatistics().Should().HaveCount(2);
		}

		[Test]
		public void GetStatistics_ContainsOneItem_AfterAdditionOfSameWords()
		{
			var word = "a";
			wordsStatistics.AddWord(word);
			wordsStatistics.AddWord(word);
			wordsStatistics.AddWord(word);

			wordsStatistics.GetStatistics().Should().HaveCount(1);
		}
		
		[Test]
		public void GetStatistics_ShouldBeZero_WhenEmpty()
		{
			wordsStatistics.AddWord(string.Empty);
			wordsStatistics.GetStatistics().Should().HaveCount(0);
		}

		[Test]
		public void GetStatistics_ShouldBeZero_WhenAddWhitespace()
		{
			wordsStatistics.AddWord(" ");
			wordsStatistics.GetStatistics().Should().HaveCount(0);
		}

		[Test]
		public void AddWord_ShouldThrowException_WhenAddNull()
		{
			wordsStatistics.Invoking(w => w.AddWord(null)).Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void GetStatistics_ShouldBeSameWords_WhenDiffsAfter10Symbols()
		{
			wordsStatistics.AddWord("qwertyuiopabcdX");
			wordsStatistics.AddWord("qwertyuiopabcdY");
			wordsStatistics.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void GetStatistics_ShouldBeInDescendingOrderCount_WhenDifferentWords()
		{
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("a");

			wordsStatistics.GetStatistics().Select(wc => wc.Count).Should().BeInDescendingOrder();
		}

		[Test]
		public void GetStatistics_ShouldBeInWordOrder_WhenSameWordCount()
		{
			wordsStatistics.AddWord("b");
			wordsStatistics.AddWord("a");

			wordsStatistics.GetStatistics().Select(wc => wc.Word).Should().BeInAscendingOrder();
		}

		[Test]
		public void GetStatistics_ShouldBe_CorrectPair()
		{
			wordsStatistics.AddWord("a");

			wordsStatistics.GetStatistics().First().Equals(WordCount.Create(new KeyValuePair<string, int>("a", 1)));
		}

		[Test]
		public void GetStatistics_ShouldBe_UnsensetiveToWordRegister()
		{
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("A");

			wordsStatistics.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void ShouldNotBeStatic()
		{
			var wordStatisticsSecond = CreateStatistics();
			
			wordsStatistics.AddWord("a");
			wordsStatistics.AddWord("b");
			wordStatisticsSecond.AddWord("a");

			wordsStatistics.GetStatistics().Should().HaveCount(2);
			wordStatisticsSecond.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void GetStatistics_ShouldBeSameWords_AfterAdd()
		{
			var word = "a";
			wordsStatistics.AddWord(word);

			wordsStatistics.GetStatistics().Select(wc => wc.Word).Should().Equal(word);
		}

		[Test]
		public void AddWord_ShouldAcceptDifferentLengthsUnder10()
		{
			var word = "";
			for (var i = 0; i < 10; i++)
			{
				word += "a";
				wordsStatistics.AddWord(word);
			}

			wordsStatistics.GetStatistics().Should().HaveCount(10);
		}

		[Test]
		public void AddWord_ShouldCutLongWord()
		{
			var word = "1234567890";
			wordsStatistics.AddWord(word);
			word += "a";
			wordsStatistics.AddWord(word);

			wordsStatistics.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void ShouldCutWordWithLeadingWhitespaces_AndAddWord()
		{
			var elevenWhitespacesAndNumber = "           1";
			wordsStatistics.AddWord(elevenWhitespacesAndNumber);

			wordsStatistics.GetStatistics().Should().HaveCount(1);
		}

		[Test]
		public void AddWord_ShouldToLowerCorrect()
		{
			var word = "Ё";
			wordsStatistics.AddWord(word);
			word = word.ToLower();

			wordsStatistics.GetStatistics().First().Word.Should().Be(word);
		}
		
		// Документация по FluentAssertions с примерами : https://github.com/fluentassertions/fluentassertions/wiki
	}
}