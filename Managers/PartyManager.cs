using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.Actions;

namespace Bannerlord.DarkNightInvasion.Managers
{
    public class PartyManager
    {
        public static void AddTroopsToParty(string partyName, CharacterObject troopObject, int numberOfTroopsToAdd, MobileParty? partyObject = null, bool free = false)
        {
            if(partyObject == null)
            {
                partyObject = GetPartyByName(partyName);
            }
            if(partyObject != null)
            {
                
                int price = CalculateValueOfTroops(troopObject, numberOfTroopsToAdd);
                if (free)
                {
                    price = 0;
                }
                int goldBalance = 0;

                switch(DarkNightInvasionSetting.Instance.payer.SelectedIndex)
                {
                    case 0: goldBalance = Campaign.Current.MainParty.Owner.Gold; break;
                    case 1: goldBalance = partyObject.Owner.Gold; break;
                    case 2: price = 0; break;
                }

                if(goldBalance >= price)
                {
                    switch (DarkNightInvasionSetting.Instance.payer.SelectedIndex)
                    {
                        case 0: 
                            Campaign.Current.MainParty.Owner.ChangeHeroGold(-1 * price);
                            InformationManager.DisplayMessage(new InformationMessage(Campaign.Current.MainParty.Owner.Name.ToString() + " payed " + price + " for army"));
                            break;

                        case 1: 
                            partyObject.Owner.ChangeHeroGold(-1 * price);
                            InformationManager.DisplayMessage(new InformationMessage(partyObject.Owner.Name.ToString() + " payed " + price + " for army"));
                            break;
                    }

                    int result = partyObject.AddElementToMemberRoster(troopObject, numberOfTroopsToAdd);
                }
                else
                {
                    InformationManager.DisplayMessage(new InformationMessage("not enough money"));
                }
            }
        }

        public static int CalculateValueOfTroops(CharacterObject troopObject, int numberOfTroopsToAdd)
        {
            int value = 0;

            value = troopObject.TroopWage * 1500 * ((troopObject.Tier / 10) + 1) * numberOfTroopsToAdd;

            return value;
        }

        public static MobileParty CreateParty(CharacterObject characterObject, Vec2 position)
        {
            return characterObject.HeroObject.Clan.CreateNewMobilePartyAtPosition(characterObject.HeroObject, position);
        }

        public static MobileParty CreateParty(CharacterObject characterObject)
        {
            return characterObject.HeroObject.Clan.CreateNewMobileParty(characterObject.HeroObject);
        }

        public static void GiveFoodToParty(MobileParty party, string itemID, int amount)
        {
            party.ItemRoster.AddToCounts(new ItemObject(itemID), amount);
        }

        public static MobileParty? GetPartyByName(string PartyName)
        {
            MobileParty? resultParty = null;
            if (PartyName.Length > 0)
            {
                resultParty = MobileParty.All.Find(party => String.Equals(party.ArmyName.ToString(), PartyName, StringComparison.OrdinalIgnoreCase));
            }

            return resultParty;
        }

        public static List<MobileParty> GetPartiesByKingdom(IFaction faction)
        {
            List<MobileParty> parties = new List<MobileParty>();
            MobileParty.All.ForEach(party =>
            {
                if (party.MapFaction == faction)
                {
                    parties.Add(party);
                }
            });

            return parties;
        }


    }
}
