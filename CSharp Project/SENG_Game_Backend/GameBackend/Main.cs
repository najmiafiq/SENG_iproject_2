using SENG_Game_Backend.app;
using SENG_Game_Backend.src;
using System;
using System.Linq; // Included implicitly in newer C# projects, but good practice

public class Program
{
    private static readonly CharacterCatalog _catalog = new CharacterCatalog();

    public static void Main(string[] args)
    {
        _catalog.GenerateSampleData(10); // Start with some sample data
        RunMainMenu();
    }

    private static void RunMainMenu()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=============================================");
            Console.WriteLine("           ⚔️ TEMMU DATA MANAGER ⚔️         ");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. Character Catalog (CRUD)");
            Console.WriteLine("2. Data Analysis & Reports");
            Console.WriteLine("3. Generate More Sample Data");
            Console.WriteLine("4. Exit Application");
            Console.WriteLine("---------------------------------------------");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": RunCrudMenu(); break;
                case "2": RunAnalysisMenu(); break;
                case "3":
                    _catalog.GenerateSampleData(5);
                    Console.WriteLine("\n✅ 5 more characters generated! Press Enter to continue...");
                    Console.ReadLine();
                    break;
                case "4": running = false; break;
                default:
                    Console.WriteLine("\nInvalid option. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RunCrudMenu()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== 📋 Character Catalog Menu ===");
            Console.WriteLine("1. Create New Character (C)");
            Console.WriteLine("2. View All Characters (R)");
            Console.WriteLine("3. Update Character (U)");
            Console.WriteLine("4. Delete Character (D)");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine("----------------------------------");
            Console.Write("Enter choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": CreateCharacter(); break;
                case "2": ViewAllCharacters(); break;
                case "3": UpdateCharacter(); break;
                case "4": DeleteCharacter(); break;
                case "5": running = false; break;
                default:
                    Console.WriteLine("\nInvalid option. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    private static void RunAnalysisMenu()
    {
        Console.Clear();
        DataAnalyzer.ShowSummary(_catalog);
        Console.WriteLine("\nPress Enter to return to the Main Menu...");
        Console.ReadLine();
    }

    // --- CRUD Implementation Methods ---

    private static void ViewAllCharacters()
    {
        Console.Clear();
        Console.WriteLine("=== All Fighters ===");
        var characters = _catalog.GetAll();
        if (!characters.Any())
        {
            Console.WriteLine("No characters in the catalog.");
        }
        else
        {
            foreach (var c in characters)
            {
                Console.WriteLine(c);
            }
        }
        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }

    // (Other CRUD methods like CreateCharacter, UpdateCharacter, DeleteCharacter would go here, 
    // involving Console.ReadLine() to get user input for the character properties.)

    private static void CreateCharacter()
    {
        Console.Clear();
        Console.WriteLine("=== Create New Character ===");
        Console.Write("Enter Character Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Fighting Style (e.g., Boxing, Karate): ");
        string style = Console.ReadLine();

        // Simple validation for required fields
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(style))
        {
            Console.WriteLine("\nName and Style are required. Creation cancelled.");
        }
        else
        {
            var newChar = new FighterCharacter
            {
                Name = name,
                Style = style,
                healthBase = 1000,
                attackMultiplier = 1.0,
                defenseMultiplier = 1.0,
                speed = 5,
                matchesPlayed = 0,
                wins = 0
            };
            _catalog.Add(newChar);
            Console.WriteLine($"\n✅ Character '{newChar.Name}' created with ID: {newChar.id}");
        }
        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }

    private static void UpdateCharacter()
    {
        Console.Clear();
        Console.WriteLine("=== Update Character ===");
        Console.Write("Enter ID of character to update: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        var characterToUpdate = _catalog.GetById(id);
        if (characterToUpdate == null)
        {
            Console.WriteLine($"Character with ID {id} not found.");
            return;
        }

        // Get new values
        Console.WriteLine($"\n--- Current Data for ID {id} ({characterToUpdate.Name}) ---");
        Console.Write($"Enter new Name (Current: {characterToUpdate.Name}): ");
        string newName = Console.ReadLine();

        // Use a ternary operator to keep the old value if the new input is empty
        string finalName = string.IsNullOrWhiteSpace(newName) ? characterToUpdate.Name : newName;

        // Create an object to pass to the update method
        var updatedModel = new FighterCharacter
        {
            id = id,
            Name = finalName,
            Style = characterToUpdate.Style, // Keep old style for simplicity
            healthBase = characterToUpdate.healthBase,
            attackMultiplier = characterToUpdate.attackMultiplier,
            defenseMultiplier = characterToUpdate.defenseMultiplier,
            speed = characterToUpdate.speed,
        };

        if (_catalog.Update(updatedModel))
        {
            Console.WriteLine($"\n✅ Character ID {id} updated successfully!");
        }
        else
        {
            Console.WriteLine("\n❌ Update failed (Character not found).");
        }
        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }

    private static void DeleteCharacter()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Character ===");
        Console.Write("Enter ID of character to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID format.");
            return;
        }

        if (_catalog.Delete(id))
        {
            Console.WriteLine($"\n✅ Character with ID {id} successfully deleted.");
        }
        else
        {
            Console.WriteLine($"\n❌ Deletion failed. Character with ID {id} not found.");
        }
        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }

}