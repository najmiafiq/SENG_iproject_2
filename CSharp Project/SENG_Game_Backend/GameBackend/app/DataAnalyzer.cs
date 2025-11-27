using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SENG_Game_Backend.app
{
    public static class DataAnalyzer
    {
        public static void ShowSummary(CharacterCatalog catalog)
        {
            var characters = catalog.GetAll();
            if (!characters.Any())
            {
                Console.WriteLine("No data available for analysis.");
                return;
            }

            Console.WriteLine("\n=== 📊 Catalog Analysis & Summary ===");

            // 1. LINQ Query: Total and Average Stats
            var totalMatches = characters.Sum(c => c.matchesPlayed);
            var averageWinRate = characters.Average(c => c.winRate);
            var avgSpeed = characters.Average(c => c.speed);
            Console.WriteLine($"Total Characters: {characters.Count}");
            Console.WriteLine($"Total Matches Recorded: {totalMatches:N0}");
            Console.WriteLine($"Average Character Win Rate: {averageWinRate:P2}");
            Console.WriteLine($"Average Character Speed: {avgSpeed:F1}");

            Console.WriteLine("\n--- Top Performers ---");

            // 2. LINQ Query: Top 3 Characters by Win Rate
            var topFighters = characters
                .OrderByDescending(c => c.winRate)
                .Take(3)
                .Select((c, index) => new { Rank = index + 1, c.Name, c.winRate, c.Style });

            foreach (var f in topFighters)
            {
                Console.WriteLine($"#{f.Rank}: {f.Name} ({f.Style}) - Win Rate: {f.winRate:P2}");
            }

            Console.WriteLine("\n--- Style Distribution ---");

            // 3. LINQ Query: Grouping by Style and counting
            var styleCounts = characters
                .GroupBy(c => c.Style)
                .Select(g => new { Style = g.Key, Count = g.Count(), AverageAttack = g.Average(c => c.attackMultiplier) })
                .OrderByDescending(x => x.Count);

            foreach (var s in styleCounts)
            {
                Console.WriteLine($"Style: {s.Style,-15} | Count: {s.Count} | Avg ATK: {s.AverageAttack:F2}");
            }
        }
    }
}
