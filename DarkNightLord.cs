using Bannerlord.DarkNightInvasion.Managers;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace Bannerlord.DarkNightInvasion.ModuleData
{
    class DarkNightLord
    {

        public CharacterObject Character;
        public MobileParty? party;

        public DarkNightLord(CharacterObject Character) 
        { 
            this.Character = Character;
            Character.HeroObject.AddInfluenceWithKingdom(100000);
        }

        public DarkNightLord(string name, CultureObject cultureObject, Clan clan, Dictionary<EquipmentIndex, EquipmentElement> equipmentElements = null) 
        { 
            Character = LordManager.CreateLord(name, cultureObject, null, clan, null, 500000000);

            //set level and skills
            Character.HeroObject.Level = 50;
            foreach (SkillObject skill in Skills.All)
            {
                Character.HeroObject.HeroDeveloper.AddFocus(skill, 100, false);
                Character.HeroObject.HeroDeveloper.SetInitialSkillLevel(skill, 300);
                Character.HeroObject.HeroDeveloper.InitializeSkillXp(skill);
            }

            ChangeClanInfluenceAction.Apply(clan, 100000);

            if (equipmentElements != null) 
            {
                SetEquipment(equipmentElements);
            }
        }

        //public void CreateParty(Dictionary<CharacterObject, int> troops, Vec2 positon)
        //{
        //    party = PartyManager.CreateParty(Character, positon);

        //    foreach(var troop in troops)
        //    {
        //        PartyManager.AddTroopsToParty(party.Name.ToString(), troop.Key, troop.Value, party);
        //    }
        //}

        public void SetEquipment(Dictionary<EquipmentIndex, EquipmentElement> equipmentElements)
        {
            foreach(var equipmentElement in equipmentElements)
            {
                Character.HeroObject.BattleEquipment[equipmentElement.Key] = equipmentElement.Value;
            }
        }
    }
}
