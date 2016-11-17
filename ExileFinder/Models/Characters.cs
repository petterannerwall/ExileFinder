using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileFinder.Models
{
    public class Character
    {
        public string Name;
        public int Level;
        public CharacterClass CharClass;

        public Character(string name, int level, CharacterClass charClass)
        {
            this.Name = name;
            this.Level = level;
            this.CharClass = charClass;
        }
    }
}
