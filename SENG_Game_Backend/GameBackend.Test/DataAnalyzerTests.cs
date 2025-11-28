using NUnit.Framework;
using SENG_Game_Backend.app;
using SENG_Game_Backend.src;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GameBackend.Tests
{
    [TestFixture]
    public class DataAnalyzerTests
    {
        private CharacterCatalog _catalog;

        // Use a StringWriter to capture Console output for verification
        private StringWriter _consoleOutput;

        [SetUp]
        public void Setup()
        {
            // 1. Setup predictable data
            _catalog = new CharacterCatalog();

            // Character A: Top Fighter
            _catalog.Add(new FighterCharacter { Name = "A-TOP", Style = "Karate", healthBase = 1000, attackMultiplier = 1.30, speed = 10, matchesPlayed = 100, wins = 90 }); // 90.0% Win Rate

            // Character B: Mid Fighter
            _catalog.Add(new FighterCharacter { Name = "B-MID", Style = "Judo", healthBase = 950, attackMultiplier = 0.90, speed = 5, matchesPlayed = 200, wins = 100 }); // 50.0% Win Rate

            // Character C: Low Fighter (Same Style as A)
            _catalog.Add(new FighterCharacter { Name = "C-LOW", Style = "Karate", healthBase = 1050, attackMultiplier = 1.10, speed = 3, matchesPlayed = 100, wins = 10 }); // 10.0% Win Rate

            // 2. Setup console capture for testing output (since DataAnalyzer prints directly)
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        [TearDown]
        public void TearDown()
        {
            // Restore standard console output after each test
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }

        // --- 1. Summary Statistics Tests ---

        [Test]
        public void ShowSummary_CalculatesCorrectTotals()
        {
            // Act
            DataAnalyzer.ShowSummary(_catalog);
            string output = _consoleOutput.ToString();

            // Assert
            // Total Characters: 3
            Assert.That(output, Contains.Substring("Total Characters: 3"));

            // Total Matches: 100 + 200 + 100 = 400
            Assert.That(output, Contains.Substring("Total Matches Recorded: 400"));

            // Average Speed: (10 + 5 + 3) / 3 = 6.0
            Assert.That(output, Contains.Substring("Average Character Speed: 6.0"));
        }

        [Test]
        public void ShowSummary_CalculatesCorrectAverageWinRate()
        {
            // Arrange
            // Win Rates: 0.90, 0.50, 0.10
            // Average Win Rate: (0.9 + 0.5 + 0.1) / 3 = 0.5 (50.00%)

            // Act
            DataAnalyzer.ShowSummary(_catalog);
            string output = _consoleOutput.ToString();

            // Assert
            Assert.That(output, Contains.Substring("Average Character Win Rate: 50.00%"));
        }

        // --- 2. LINQ Query Tests (Top Performers) ---

        [Test]
        public void ShowSummary_IdentifiesTopFightersByWinRate()
        {
            // Act
            DataAnalyzer.ShowSummary(_catalog);
            string output = _consoleOutput.ToString();

            // Assert
            // Check for the top ranked fighter
            Assert.That(output, Contains.Substring("#1: A-TOP (Karate) - Win Rate: 90.00%"));
            // Check for the second ranked fighter
            Assert.That(output, Contains.Substring("#2: B-MID (Judo) - Win Rate: 50.00%"));
            // Ensure the third fighter is listed
            Assert.That(output, Contains.Substring("#3: C-LOW (Karate) - Win Rate: 10.00%"));
        }

        // --- 3. LINQ Query Tests (Grouping/Distribution) ---

        [Test]
        public void ShowSummary_GroupsCharactersByStyleCorrectly()
        {
            // Arrange
            // Karate: A-TOP (ATK 1.30) and C-LOW (ATK 1.10). Count: 2. Avg ATK: 1.20
            // Judo: B-MID (ATK 0.90). Count: 1. Avg ATK: 0.90

            // Act
            DataAnalyzer.ShowSummary(_catalog);
            string output = _consoleOutput.ToString();

            // Assert
            // Check the dominant style (Karate, Count 2)
            Assert.That(output, Contains.Substring("Style: Karate          | Count: 2 | Avg ATK: 1.20"));

            // Check the other style (Judo, Count 1)
            Assert.That(output, Contains.Substring("Style: Judo            | Count: 1 | Avg ATK: 0.90"));
        }

        // --- 4. Edge Case Test ---

        [Test]
        public void ShowSummary_HandlesEmptyCatalogGracefully()
        {
            // Arrange
            var emptyCatalog = new CharacterCatalog();

            // Act
            DataAnalyzer.ShowSummary(emptyCatalog);
            string output = _consoleOutput.ToString();

            // Assert
            Assert.That(output, Contains.Substring("No data available for analysis."));
        }
    }
}