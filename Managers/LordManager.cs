using System;
using System.Linq;
using System.Text.RegularExpressions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.DarkNightInvasion.Managers
{
    class LordManager
    {
        public static CharacterObject CreateLord(string name, CultureObject culture, Settlement? bornPlace = null, Clan? clan = null, Settlement? enterSettlement = null, int gold = 50000) 
        {
            CharacterObject characterObject = FindLord(name);
            if (characterObject == null)
            {
                characterObject = culture.LordTemplates.FirstOrDefault((CharacterObject x) => x.Occupation == Occupation.Lord);

                Hero hero = HeroCreator.CreateSpecialHero(characterObject, bornPlace, clan, null, MBRandom.RandomInt(18, 36));
                hero.HeroDeveloper.DeriveSkillsFromTraits(false, null);
                hero.ChangeState(Hero.CharacterStates.Active);
                hero.SetName(new TextObject(name), new TextObject(name));

                if (enterSettlement != null)
                {
                    EnterSettlementAction.ApplyForCharacterOnly(hero, enterSettlement);
                }

                GiveGoldAction.ApplyBetweenCharacters(null, hero, gold, false);

                characterObject = hero.CharacterObject;
            }

            return characterObject;
        }

        public static CharacterObject FindLord(string name)
        {
            CharacterObject result = null;

            result = MBObjectManager.Instance.GetObjectTypeList<CharacterObject>().Find( characterObject => characterObject.IsHero && String.Equals(characterObject.Name.ToString(), name, StringComparison.OrdinalIgnoreCase));

            return result;
        }
    }
}
