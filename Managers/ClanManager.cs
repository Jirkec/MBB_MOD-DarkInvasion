using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.Party;

namespace Bannerlord.DarkNightInvasion.Managers
{
    class ClanManager
    {
        public static Clan CreateClan(string name, CultureObject culture, Hero leader, Kingdom kingdom, string desc = "", Banner? banner = null)
        {
            if(banner == null)
            {
                banner = Banner.CreateRandomClanBanner(-1);
            }

            Clan clan = Clan.CreateClan("test_clan_" + Clan.All.Count);
            clan.InitializeClan(new TextObject(name), new TextObject(desc), culture, banner);
            clan.AddRenown(1000000);
            clan.SetLeader(leader);
            
            ChangeKingdomAction.ApplyByJoinToKingdom(clan, kingdom, false);

            return clan;
        }

        public static List<MobileParty> GetLordPartiesOfClan(Clan clan)
        {
            List<MobileParty> lords = new List<MobileParty>();
            foreach( var party in Campaign.Current.LordParties)
            {
                if(party.ActualClan == clan)
                {
                    lords.Add(party);    
                }
            }

            return lords;
        }
    }
}
