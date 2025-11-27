using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Xml.Linq;

namespace SENG_Game_Backend.src
{
    public class FighterCharacter
    {
        public int id {  get; set; }

        //Core Identity
        public string Name { get; set; }
        public string Style { get; set; }

        //Core stats
        public int healthBase { get; set; } //Base HP
        public double attackMultiplier {  get; set; } //Damage Modifier
        public double defenseMultiplier { get; set; } // Resistance Modifier
        public int speed { get; set; } //Movement speed/frame data influence

        public int matchesPlayed { get; set; }
        public int wins {  get; set; }
        public int losses => matchesPlayed - wins;
        public double winRate => matchesPlayed > 0 ? (double)wins / matchesPlayed : 0.0;

        public override string ToString()
        {
            return $"[ID: {id}] {Name} ({Style}) - HP: {healthBase} | ATK: {attackMultiplier:F2} | SPD: {speed} | Win Rate: {winRate:P0}";
        }
    }
}
