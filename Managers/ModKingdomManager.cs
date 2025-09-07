using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.DarkNightInvasion.Managers
{
    class ModKingdomManager
    {
        public static Kingdom? CreateKingdom(string name, string description, CultureObject cultureObject, Clan rulerClan)
        {
            Kingdom? kingdom = ModKingdomManager.FindKingdom(name);
            
            if (kingdom == null)
            {
                Campaign.Current.KingdomManager.CreateKingdom(new TextObject(name), new TextObject(description), cultureObject, rulerClan);
                kingdom = ModKingdomManager.FindKingdom(name);
            }

            return kingdom;
        }

        public static Kingdom? FindKingdom(string name)
        {
            Kingdom? result = null;

            Campaign.Current.Kingdoms.ForEach(kingdom =>
            {
                if(String.Equals(kingdom.Name.ToString(), name, StringComparison.OrdinalIgnoreCase))
                {
                    result = kingdom;
                }
            });

            return result;
        }

        public static List<string> GetKingdomNames()
        {
            List<string> kingdomNames = new List<string>();
            Campaign.Current.Kingdoms.ForEach(kingdom =>
            {
                kingdomNames.Add(kingdom.Name.ToString());
            });
            
            return kingdomNames;
        }

        public static List<MobileParty> GetLordPartiesOfKingdom(string name) 
        { 
            Kingdom? kingdom = FindKingdom(name);
            List <MobileParty> lords = new List<MobileParty>();
            if (kingdom != null)
            {
                foreach(var party in kingdom.AllParties)
                {
                    if(party.IsActive && party.IsLordParty) 
                    {
                        lords.Add(party);
                    }
                }
            }

            return lords;
        }
    }
}
