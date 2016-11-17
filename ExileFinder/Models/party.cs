using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExileFinder.Models
{
    public class Party
    {
        public List<Character> PartyMemberList { get; protected set; }
        public string Description;
        public PartyType Type;

        public Party(string desc, PartyType type)
        {
            this.Description = desc;
            this.Type = type;
        }

        public void AddPartyMember(Character character)
        {
            this.PartyMemberList.Add(character);
        }

        public void RemovePartyMember(Character character)
        {
            PartyMemberList.Remove(character);
        }

        public void ClearPartyMembers()
        {
            PartyMemberList.Clear();
        }
    }
}
