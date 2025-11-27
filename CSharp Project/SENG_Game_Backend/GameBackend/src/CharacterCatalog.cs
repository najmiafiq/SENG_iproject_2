
using SENG_Game_Backend.src;
using System;
using System.Collections.Generic;
using System.Linq;

public class CharacterCatalog
{
    private readonly List<FighterCharacter> _characters = new List<FighterCharacter>();
    private int _nextId = 1;
    private readonly Random _random = new Random();

    public IReadOnlyList<FighterCharacter> GetAll() => _characters;

    // --- Core CRUD Operations ---

    public void Add(FighterCharacter character)
    {
        character.id = _nextId++;
        _characters.Add(character);
    }

    public FighterCharacter GetById(int id) =>
        _characters.FirstOrDefault(c => c.id == id);

    public bool Update(FighterCharacter updatedCharacter)
    {
        var existing = GetById(updatedCharacter.id);
        if (existing == null) return false;

        // A professional update only touches allowed fields
        existing.Name = updatedCharacter.Name;
        existing.Style = updatedCharacter.Style;
        existing.healthBase = updatedCharacter.healthBase;
        existing.attackMultiplier = updatedCharacter.attackMultiplier;
        existing.defenseMultiplier = updatedCharacter.defenseMultiplier;
        existing.speed = updatedCharacter.speed;
        // MatchesPlayed/Wins/Losses would typically be updated by a separate 'game event' system

        return true;
    }

    public bool Delete(int id)
    {
        var character = GetById(id);
        if (character == null) return false;
        return _characters.Remove(character);
    }

    // --- Random Data Generation (Requirement 3) ---

    public void GenerateSampleData(int count = 10)
    {
        var names = new[] { "KAI", "ZERO", "LUNA", "BLAZE", "VIPER", "TITAN", "STORM", "ECHO" };
        var styles = new[] { "Karate", "Judo", "Boxing", "Capoeira", "Kung Fu", "Taekwondo" };

        for (int i = 0; i < count; i++)
        {
            var character = new FighterCharacter
            {
                id = _nextId++,
                Name = names[_random.Next(names.Length)] + _nextId, // Ensure unique-ish name
                Style = styles[_random.Next(styles.Length)],
                healthBase = 900 + _random.Next(20) * 10, // 900 to 1080
                attackMultiplier = 1.0 + _random.NextDouble() * 0.4 - 0.2, // 0.8 to 1.4
                defenseMultiplier = 1.0 + _random.NextDouble() * 0.2 - 0.1, // 0.9 to 1.1
                speed = _random.Next(4, 11), // 4 to 10
                matchesPlayed = _random.Next(50, 201), // 50 to 200 matches
                wins = _random.Next(0, 101)
            };
            // Ensure Wins <= MatchesPlayed
            character.wins = _random.Next(0, character.matchesPlayed + 1);

            _characters.Add(character);
        }
    }
}