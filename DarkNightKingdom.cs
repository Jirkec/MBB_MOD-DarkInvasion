using Bannerlord.DarkNightInvasion.Managers;
using Bannerlord.DarkNightInvasion.ModuleData;
using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.DarkNightInvasion
{
    class DarkNightKingdom
    {
        private static Random rnd = new Random();

        public const string KINGDOM_NAME = "Dark Knights";
        public const string KINGDOM_DESCRIPTION = "Dark Knights";
        public const string CULTURE_NAME = "Dark Knights";
        public const string CLAN_DESCRIPTION = "";

        public const string RULER_NAME = "Charles von Blackbournee";
        public const string RULER_CLAN_NAME = "Blackbournee";
        public const string RULER_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.161.140.116.608.636.762.763.1.0.0";

        public const string Widonids_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.301.116.116.608.636.762.763.1.0.0";
        public const string Ravenberg_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.419.116.116.608.636.762.763.1.0.0";
        public const string Thorwald_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.124.149.116.608.636.762.763.1.0.0";
        public const string Dyere_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.435.149.116.608.636.762.763.1.0.0";
        public const string Tatee_CLAN_BANNER = "1.0.0.1129.1412.775.754.1.0.-90.133.149.116.608.636.762.763.1.0.0";

        public const string SPAWN_POINT = "Sahel";

        public const int PARTY_SIZE_LIMIT = 5000;
        public const int NUMBER_OF_OTHER_CLANS = 5;

        Dictionary<string, CharacterObject> troops;

        public Clan rulerClan;
        public Clan[] otherClans = new Clan[NUMBER_OF_OTHER_CLANS];
        Dictionary<DarkNightLord, Dictionary<CharacterObject, int>> darkNightLords;

        public Kingdom kingdom;
        public CharacterObject leader;
        public CultureObject culture;

        public DarkNightKingdom()
        {
            culture = CultureManager.FindCulture(CULTURE_NAME);
            LoadTroops();

            Kingdom? tmpKingdom = ModKingdomManager.FindKingdom(KINGDOM_NAME);
            if (tmpKingdom != null )
            {
                kingdom = tmpKingdom;
                leader = LordManager.FindLord(RULER_NAME);
            }
            else 
            {
                try
                {
                    Create();
                }
                catch(Exception e)
                {
                    Debug.Print(e.ToString());
                }
                
            }
        }

        public void Create()
        {
            CreateLeader();
            CreateRulerClan();
            CreateKingdom();

            CreateClans();
            CreateLords();
            CreateArmy();
            CreateParties();

            DeclareWarAction.ApplyByDefault(kingdom, Campaign.Current.MainParty.ActualClan.Kingdom);
            Settlement gatheringPoint = SettlementManager.FindSettlement("Sahel Castle");
            //ChangeOwnerOfSettlementAction.ApplyByDefault(leader.HeroObject, gatheringPoint);

            
            MobileParty rulerParty = null;
            foreach ( var lord in darkNightLords)
            {
                rulerParty = lord.Key.party;
                break;
            }

            kingdom.CreateArmy( leader.HeroObject, gatheringPoint, Army.ArmyTypes.Besieger);

            //if(rulerParty != null)
            //{
            //    GatherArmyAction.Apply(rulerParty, gatheringPoint);
            //}
            JoinArmy(kingdom.Armies[0]);
            kingdom.Armies[0].LeaderParty.Ai.SetDoNotMakeNewDecisions(true);
            SetPartyAiAction.GetActionForBesiegingSettlement(kingdom.Armies[0].LeaderParty, gatheringPoint);
            kingdom.Armies[0].LeaderParty.Ai.SetDoNotMakeNewDecisions(false);


            //SetPartyAiAction.GetActionForBesiegingSettlement(kingdom.Armies[0].LeaderParty, gatheringPoint);
            //kingdom.Armies[0].AIBehavior = Army.AIBehaviorFlags.WaitingForArmyMembers;
        }

        private void CreateClans()
        {
            otherClans[0] = ClanManager.CreateClan("Widonids", culture, LordManager.CreateLord("Guy of Widonids", culture).HeroObject, kingdom, "", new Banner(Widonids_CLAN_BANNER));
            otherClans[1] = ClanManager.CreateClan("Ravenberg", culture, LordManager.CreateLord("Ulrich von Ravenberg", culture).HeroObject, kingdom, "", new Banner(Ravenberg_CLAN_BANNER));
            otherClans[2] = ClanManager.CreateClan("Thorwald", culture, LordManager.CreateLord("Guy of Thorwald", culture).HeroObject, kingdom, "", new Banner(Thorwald_CLAN_BANNER));
            otherClans[3] = ClanManager.CreateClan("Dyere", culture, LordManager.CreateLord("Tatton Dyere", culture).HeroObject, kingdom, "", new Banner(Dyere_CLAN_BANNER));
            otherClans[4] = ClanManager.CreateClan("Tatee", culture, LordManager.CreateLord("Kendall Tatee", culture).HeroObject, kingdom, "", new Banner(Tatee_CLAN_BANNER));
        }

        private void CreateLords()
        {
            darkNightLords = new Dictionary<DarkNightLord, Dictionary<CharacterObject, int>>
            {
                //ruler clan
                { new DarkNightLord(RULER_NAME, culture, rulerClan, RulerEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Wilton Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Winfred  Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Dane Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Gerry Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Garry Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Callias Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Valentinus Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Erebus Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Brutus Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Arminius Blackbournee", culture, rulerClan, GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },

                //Widonids clan
                { new DarkNightLord("Guy of Widonids", culture, otherClans[0], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Charles Widonids", culture, otherClans[0], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Hede Widonids", culture, otherClans[0], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Ulla Widonids", culture, otherClans[0], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Ferdinand Widonids", culture, otherClans[0], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },

                //Ravenberg clan
                { new DarkNightLord("Ulrich von Ravenberg", culture, otherClans[1], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Gebhard Ravenberg", culture, otherClans[1], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Filibert Ravenberg", culture, otherClans[1], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Fulbert Ravenberg", culture, otherClans[1], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Ermenrich Ravenberg", culture, otherClans[1], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },

                //Thorwald clan
                { new DarkNightLord("Guy of Thorwald", culture, otherClans[2], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Arnulf of Thorwald", culture, otherClans[2], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Hartmut of Thorwald", culture, otherClans[2], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Ermenrich of Thorwald", culture, otherClans[2], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Anselm of Thorwald", culture, otherClans[2], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },

                //Dyere clan
                { new DarkNightLord("Tatton Dyere", culture, otherClans[3], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Norma Dyere", culture, otherClans[3], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Eoforhild Dyere", culture, otherClans[3], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Audley Dyere", culture, otherClans[3], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Edwin Dyere", culture, otherClans[3], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },

                //Tatee clan
                { new DarkNightLord("Kendall Tatee", culture, otherClans[4] , GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Riley Tatee", culture, otherClans[4], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Greysen Tatee", culture, otherClans[4], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Deana Tatee", culture, otherClans[4], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() },
                { new DarkNightLord("Ashley Tatee", culture, otherClans[4], GenerateLordEquipmentElements()), new Dictionary<CharacterObject, int>() }
            };
        }

        public Dictionary<EquipmentIndex, EquipmentElement> GenerateLordEquipmentElements()
        {
            Dictionary<EquipmentIndex, EquipmentElement> equipList = new Dictionary<EquipmentIndex, EquipmentElement>();

            List<EquipmentElement> armor = new List<EquipmentElement>();
            armor.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_armor")));
            armor.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_armor2")));

            List<EquipmentElement> helm = new List<EquipmentElement>();
            helm.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_helm")));

            List<EquipmentElement> horse = new List<EquipmentElement>();
            horse.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("noble_horse_southern")));

            List<EquipmentElement> HorseHarness = new List<EquipmentElement>();
            HorseHarness.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("chain_barding")));
            HorseHarness.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("halfchain_barding")));

            List<EquipmentElement> gloves = new List<EquipmentElement>();
            gloves.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_gloves")));

            List<EquipmentElement> boots = new List<EquipmentElement>();
            boots.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_boots")));

            List<EquipmentElement> weapon1 = new List<EquipmentElement>();
            weapon1.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("hrt_bn_swd")));

            List<EquipmentElement> weapon2 = new List<EquipmentElement>();
            weapon2.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("mp_emirs_oval_shield")));
            
            equipList[EquipmentIndex.Body] = armor[rnd.Next(0, 1)];
            equipList[EquipmentIndex.Cape] = new EquipmentElement();
            equipList[EquipmentIndex.Head] = helm[0];
            equipList[EquipmentIndex.Horse] = horse[0];
            equipList[EquipmentIndex.HorseHarness] = HorseHarness[rnd.Next(0, 1)];
            equipList[EquipmentIndex.Gloves] = gloves[0];
            equipList[EquipmentIndex.Leg] = boots[0];
            equipList[EquipmentIndex.Weapon0] = weapon1[0];
            equipList[EquipmentIndex.Weapon1] = weapon2[0];
            equipList[EquipmentIndex.Weapon2] = new EquipmentElement();
            equipList[EquipmentIndex.Weapon3] = new EquipmentElement();

            return equipList;
        }

        public Dictionary<EquipmentIndex, EquipmentElement> RulerEquipmentElements()
        {
            Dictionary<EquipmentIndex, EquipmentElement> equipList = new Dictionary<EquipmentIndex, EquipmentElement>();

            List<EquipmentElement> armor = new List<EquipmentElement>();
            armor.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_ruler_armor")));

            List<EquipmentElement> helm = new List<EquipmentElement>();
            helm.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_ruler_helm")));

            List<EquipmentElement> cape = new List<EquipmentElement>();
            cape.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_ruler_cape")));

            List<EquipmentElement> horse = new List<EquipmentElement>();
            horse.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("noble_horse_southern")));

            List<EquipmentElement> HorseHarness = new List<EquipmentElement>();
            HorseHarness.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("mail_and_plate_barding")));

            List<EquipmentElement> gloves = new List<EquipmentElement>();
            gloves.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_gloves")));

            List<EquipmentElement> boots = new List<EquipmentElement>();
            boots.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("dark_knight_lord_boots")));

            List<EquipmentElement> weapon1 = new List<EquipmentElement>();
            weapon1.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("hrt_bn_swd")));

            List<EquipmentElement> weapon2 = new List<EquipmentElement>();
            weapon2.Add(new EquipmentElement(MBObjectManager.Instance.GetObject<ItemObject>("mp_emirs_oval_shield")));

            equipList[EquipmentIndex.Body] = armor[0];
            equipList[EquipmentIndex.Cape] = cape[0];
            equipList[EquipmentIndex.Head] = helm[0];
            equipList[EquipmentIndex.Horse] = horse[0];
            equipList[EquipmentIndex.HorseHarness] = HorseHarness[0];
            equipList[EquipmentIndex.Gloves] = gloves[0];
            equipList[EquipmentIndex.Leg] = boots[0];
            equipList[EquipmentIndex.Weapon0] = weapon1[0];
            equipList[EquipmentIndex.Weapon1] = weapon2[0];

            return equipList;
        }

        private void LoadTroops()
        {
            Dictionary<string, CharacterObject> troops = new Dictionary<string, CharacterObject>
            {
                { "darknight_peasant", TroopManager.GetTroop("darknight_peasant") },
                { "darknight_light_crossbowman", TroopManager.GetTroop("darknight_light_crossbowman") },
                { "darknight_arbalest", TroopManager.GetTroop("darknight_arbalest") },
                { "darknight_spearman", TroopManager.GetTroop("darknight_spearman") },
                { "darknight_halb", TroopManager.GetTroop("darknight_halb") },
                { "darknight_knight", TroopManager.GetTroop("darknight_knight") },
                { "darknight_light_cavalry", TroopManager.GetTroop("darknight_light_cavalry") },
                { "darknight_paladin", TroopManager.GetTroop("darknight_paladin") }
            };
            foreach (var troop in troops) 
            { 
                if(troop.Value == null)
                {
                    throw new Exception("Troop " + troop.Key + "was not found");
                }
            }

            this.troops = troops; 
        }

        private void CreateArmy()
        {
            foreach (var lord in darkNightLords)
            {
                if (String.Equals(lord.Key.Character.Name.ToString(), RULER_NAME, StringComparison.OrdinalIgnoreCase))
                {
                    lord.Value.Add(troops["darknight_peasant"], 200);

                    lord.Value.Add(troops["darknight_spearman"], 150);
                    lord.Value.Add(troops["darknight_halb"], 120);
                    lord.Value.Add(troops["darknight_knight"], 50);

                    lord.Value.Add(troops["darknight_light_crossbowman"], 100);
                    lord.Value.Add(troops["darknight_arbalest"], 50);

                    lord.Value.Add(troops["darknight_light_cavalry"], 20);
                    lord.Value.Add(troops["darknight_paladin"], 30);
                }
                else
                {
                    lord.Value.Add(troops["darknight_peasant"], 100);

                    lord.Value.Add(troops["darknight_spearman"], 70);
                    lord.Value.Add(troops["darknight_halb"], 20);
                    lord.Value.Add(troops["darknight_knight"], 20);

                    lord.Value.Add(troops["darknight_light_crossbowman"], 60);
                    lord.Value.Add(troops["darknight_arbalest"], 40);

                    lord.Value.Add(troops["darknight_light_cavalry"], 40);
                    lord.Value.Add(troops["darknight_paladin"], 20);
                }
            }
        }

        private void CreateParties()
        {
            foreach (var lord in darkNightLords) 
            {
                MobileParty party = PartyManager.CreateParty(lord.Key.Character, SettlementManager.FindSettlement(SPAWN_POINT).Position2D);
                party.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("grape"), 600);
                party.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("grain"), 600);
                party.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("meat"), 600);
                party.ItemRoster.AddToCounts(MBObjectManager.Instance.GetObject<ItemObject>("mule"), 200);

                party.RecentEventsMorale = 1000;

                lord.Key.party = party;
                
                foreach (var troop in lord.Value) 
                {
                    PartyManager.AddTroopsToParty(party.Name.ToString(), troop.Key, troop.Value, party, true);
                }
            }
        }

        private void JoinArmy(Army army)
        {
            foreach (var party in kingdom.AllParties)
            {
                if (party != null)
                {
                    if (!party.IsLordParty) continue;

                    if (!String.Equals(party.Owner.Name.ToString(), RULER_NAME, StringComparison.OrdinalIgnoreCase))
                    {
                        party.Army = army;
                        army.Parties.Add(party);
                        party.AttachedTo = army.LeaderParty;
                    }
                }
            }
        }

        private void CreateLeader()
        {
            leader = LordManager.CreateLord(RULER_NAME, culture);
        }

        private void CreateRulerClan()
        {
            rulerClan = ClanManager.CreateClan(RULER_CLAN_NAME, culture, leader.HeroObject, kingdom, CLAN_DESCRIPTION, new Banner(RULER_CLAN_BANNER));
        }

        private void CreateKingdom()
        {
            Kingdom? tmKingdom = ModKingdomManager.CreateKingdom(KINGDOM_NAME, KINGDOM_DESCRIPTION, culture, rulerClan);
            if(tmKingdom != null)
            {
                kingdom = tmKingdom;
            }
        }
    }
}
