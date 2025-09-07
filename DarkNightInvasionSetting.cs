using MCM.Abstractions.Attributes.v1;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Base.PerSave;
using MCM.Common;
using System;
using TaleWorlds.CampaignSystem;
using Bannerlord.DarkNightInvasion.Managers;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Party;
using System.Collections.Generic;
using TaleWorlds.DotNet;

namespace Bannerlord.DarkNightInvasion
{
    public class DarkNightInvasionSetting : AttributePerSaveSettings<DarkNightInvasionSetting>
    {
        public override string Id { get; } = "Bannerlord.DarkNightInvasion";
        public override string DisplayName { get; } = "DarkNightInvasion";
        public override string FolderName { get; } = "DarkNightInvasion.setting";

        // Dark Knight Management

        [SettingPropertyButton("", Content = "Spawn dark knights", RequireRestart = false, HintText = "Setting explanation.", Order = 1)]
        [SettingPropertyGroup("Dark Knight management")]
        public Action StartInvation { get; set; } = (() => { StartInvationAction(); });

        public static void StartInvationAction()
        {
            DarkNightKingdom darkNightKingdom = new DarkNightKingdom();
        }

        //Dark Knight management/Debug
        [SettingPropertyText("Kingdom name", RequireRestart = false, HintText = "")]
        [SettingPropertyGroup("Dark Knight management/Debug")]
        public string kingdomName { get; set; } = "";

        [SettingPropertyButton("Find kingdom", Content = "Press Me", RequireRestart = false, HintText = "Setting explanation.")]
        [SettingPropertyGroup("Dark Knight management/Debug")]
        public Action FindKingdom { get; set; } = (() => { FindKingdomAction(); });

        [SettingPropertyButton("", Content = "PrintMoraleDetail", RequireRestart = false, HintText = "Setting explanation.")]
        [SettingPropertyGroup("Dark Knight management/Debug")]
        public Action PrintMoraleDetail { get; set; } = (() => { PrintMoraleDetailAction(); });


        private static void PrintMoraleDetailAction()
        {
            try
            {

                Kingdom? darkknigts = ModKingdomManager.FindKingdom("Dark knights");
                if (darkknigts != null)
                {
                    foreach (var party in darkknigts.AllParties)
                    {
                        if (!party.IsLordParty) continue;

                        var morale = party.MoraleExplained;
                        var limit = party.LimitedPartySize;
                        var food = party.ItemRoster.TotalFood;
                        var money = party.Owner.Gold;
                        var clantier = party.ActualClan.Tier;

                        InformationManager.DisplayMessage(new InformationMessage("party:" + party.Name.ToString() + " morale:" + party.MoraleExplained.ResultNumber));
                        InformationManager.DisplayMessage(new InformationMessage("party:" + party.Name.ToString() + " limit:" + party.LimitedPartySize));
                        InformationManager.DisplayMessage(new InformationMessage("party:" + party.Name.ToString() + " food:" + party.ItemRoster.TotalFood));
                        InformationManager.DisplayMessage(new InformationMessage("party:" + party.Name.ToString() + " gold:" + money));
                        InformationManager.DisplayMessage(new InformationMessage("party:" + party.Name.ToString() + " caln tier:" + clantier));

                    }
                }
            }
            catch(Exception e) 
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }

        private static void FindKingdomAction()
        {
            if (Instance == null)
            {
                Debug.PrintError("Error setting instance is null");
                return;
            }

            Kingdom? kingdom = ModKingdomManager.FindKingdom(Instance.kingdomName);
            if (kingdom == null)
            {
                Debug.PrintError("Error setting instance is null");
            }
            else
            {
                Debug.Print("kingdom found!");
            }
        }


        //Troop adding
        [SettingPropertyText("Party owner name", Order = 1, RequireRestart = false, HintText = "Enter whole name of party (Lord's party | Garrison of Castle | Militia of castle )")]
        [SettingPropertyGroup("Troop adding")]
        public string PartyName { get; set; } = "";

        [SettingPropertyText("TroopID", RequireRestart = false, HintText = "Enter troop id")]
        [SettingPropertyGroup("Troop adding")]
        public string troopID { get; set; } = "";

        [SettingProperty("Num of troops to add", minValue: 0, maxValue: 10000, RequireRestart = false, HintText = "Setting explanation.")]
        [SettingPropertyGroup("Troop adding")]
        public int NumOfTropsToAdd { get; set; } = 0;

        [SettingPropertyDropdown("Culture", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding")]
        public Dropdown<CultureObject> Culture { get; set; } = new Dropdown<CultureObject>(CultureManager.GetAllCultures().ToArray(), 0);

        [SettingPropertyDropdown("Troop type", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding")]
        public Dropdown<ETroopCategory> TroopCategory { get; set; } = new Dropdown<ETroopCategory>(TroopManager.GetAllCategories(), 0);

        [SettingProperty("Tier", minValue: 1, maxValue: 10, RequireRestart = false, HintText = "Setting explanation.")]
        [SettingPropertyGroup("Troop adding")]
        public int tier { get; set; } = 0;
        
        private static string[] payerList = { "player", "owner", "nobody" };
        [SettingPropertyDropdown("payer", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding")]
        public Dropdown<string> payer { get; set; } = new Dropdown<string>(payerList, 0);

        [SettingPropertyButton("", Content = "Add troops", RequireRestart = false, HintText = "Setting explanation.")]
        [SettingPropertyGroup("Troop adding")]
        public Action AddTroopsButton { get; set; } = (() => { AddTroopsAction(); });

        private static void AddTroopsAction()
        {
            try { 
            if(Instance == null) 
            {
                Debug.PrintError("Error setting instance is null");
                return;
            }

            string partyName = Instance.PartyName;
            string troopID = Instance.troopID;
            int NumOfTropsToAdd = Instance.NumOfTropsToAdd;
            CultureObject Culture = Instance.Culture.SelectedValue;
            ETroopCategory TroopCategory = Instance.TroopCategory.SelectedValue;
            int tier = Instance.tier;

            CharacterObject? troopObject = TroopManager.GetTroop(troopID, Culture, tier, TroopCategory);
            if(troopObject != null ) 
            {
                PartyManager.AddTroopsToParty(partyName, troopObject, NumOfTropsToAdd);
            }
            else
            {
                Debug.PrintError("Error troop object was not found");
            }
        }
            catch(Exception e) 
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
}
        /*
        [SettingPropertyDropdown("kingdomToAddTroops", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding kingdom")]
        public Dropdown<string> kingdomToAddTroops { get; set; } = new Dropdown<string>( ModKingdomManager.GetKingdomNames(), 0);
        */
        [SettingPropertyButton("", Content = "full party sturgia knights", RequireRestart = false, HintText = "Add troops to full party size to selected kingdom")]
        [SettingPropertyGroup("Troop adding sturgia kingdom")]
        public Action AddTroopsKingdomSturgiaButton { get; set; } = (() => { AddTroopsKingdomSturgiaAction(); });

        private static void AddTroopsKingdomSturgiaAction()
        {
            try { 
                if(Instance == null) 
                {
                    Debug.PrintError("Error setting instance is null");
                    return;
                }

                string kingdomName = "Sturgia";// Instance.kingdomToAddTroops.SelectedValue;
                List<MobileParty> parties = new List<MobileParty>();
                Kingdom? kingdom = ModKingdomManager.FindKingdom(kingdomName);
                if(kingdom != null)
                {
                    parties = ModKingdomManager.GetLordPartiesOfKingdom(kingdomName);

                    foreach(MobileParty part in parties)
                    {
                        part.LeaderHero.ChangeHeroGold(100000);
                    }

                    CharacterObject? infantry = TroopManager.GetTroop("sturgian_spearman");
                    CharacterObject? ranged = TroopManager.GetTroop("sturgian_archer");
                    CharacterObject? mounted = TroopManager.GetTroop("sturgian_hardened_brigand");
                    AddTroopsToParties(parties, infantry, ranged, mounted);
                }
            }
            catch(Exception e) 
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }

        /*
        [SettingPropertyDropdown("kingdomToAddTroops", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding kingdom")]
        public Dropdown<string> kingdomToAddTroops { get; set; } = new Dropdown<string>( ModKingdomManager.GetKingdomNames(), 0);
        */
        [SettingPropertyButton("", Content = "full party dark knights", RequireRestart = false, HintText = "Add troops to full party size to selected kingdom")]
        [SettingPropertyGroup("Troop adding dark kingdom")]
        public Action AddTroopsKingdomButton { get; set; } = (() => { AddTroopsKingdomAction(); });

        private static void AddTroopsKingdomAction()
        {
            try { 
                if(Instance == null) 
                {
                    Debug.PrintError("Error setting instance is null");
                    return;
                }

                string kingdomName = "Dark Knights";// Instance.kingdomToAddTroops.SelectedValue;
                List<MobileParty> parties = new List<MobileParty>();
                Kingdom? kingdom = ModKingdomManager.FindKingdom(kingdomName);
                if(kingdom != null)
                {
                    parties = ModKingdomManager.GetLordPartiesOfKingdom(kingdomName);

                    CharacterObject? infantry = TroopManager.GetTroop("darknight_spearman");
                    CharacterObject? ranged = TroopManager.GetTroop("darknight_light_crossbowman");
                    CharacterObject? mounted = TroopManager.GetTroop("darknight_light_cavalry");
                    AddTroopsToParties(parties, infantry, ranged, mounted);
                }
            }
            catch(Exception e) 
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }
        /*
        [SettingPropertyDropdown("kingdomToAddTroops", RequireRestart = false)]
        [SettingPropertyGroup("Troop adding kingdom")]
        public Dropdown<string> kingdomToAddTroops { get; set; } = new Dropdown<string>( ModKingdomManager.GetKingdomNames(), 0);
        */
        [SettingPropertyButton("", Content = "full party clan", RequireRestart = false, HintText = "Add troops to full party size to selected kingdom")]
        [SettingPropertyGroup("Troop adding clan")]
        public Action AddTroopsMainClanButton { get; set; } = (() => { AddTroopsMainClanAction(); });

        private static void AddTroopsMainClanAction()
        {
            try { 
            List<MobileParty> parties = new List<MobileParty>();
            parties = ClanManager.GetLordPartiesOfClan(Campaign.Current.MainParty.ActualClan);

            CharacterObject? infantry = TroopManager.GetTroop("vlandian_spearman");
            CharacterObject? ranged = TroopManager.GetTroop("vlandian_crossbowman");
            CharacterObject? mounted = TroopManager.GetTroop("vlandian_cavalry");
            AddTroopsToParties(parties, infantry, ranged, mounted);
            }
            catch (Exception e)
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }

        private static void AddTroopsToParties(List<MobileParty> parties, CharacterObject? infantry, CharacterObject? ranged, CharacterObject? mounted)
        {
            try { 
            if (infantry != null && ranged != null && mounted != null)
            {
                foreach (var party in parties)
                {
                    int NumOfTropsToAdd = party.LimitedPartySize - party.MemberRoster.TotalManCount;

                    int numOfInfantry = (int)Math.Round(NumOfTropsToAdd * 0.5);
                    int numOfRanged = (int)(NumOfTropsToAdd * 0.3);
                    int numOfmount = (int)(NumOfTropsToAdd * 0.2);

                    PartyManager.AddTroopsToParty(party.Name.ToString(), infantry, numOfInfantry, party);
                    PartyManager.AddTroopsToParty(party.Name.ToString(), ranged, numOfRanged, party);
                    PartyManager.AddTroopsToParty(party.Name.ToString(), mounted, numOfmount, party);
                }
            }
            else
            {
                Debug.PrintError("Error troop object was not found");
                }
            }
            catch (Exception e)
            {
                InformationManager.DisplayMessage(new InformationMessage(e.Message));
            }
        }
    }
}
