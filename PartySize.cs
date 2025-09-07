using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem;
using HarmonyLib;
using Bannerlord.DarkNightInvasion.Managers;
using System;
using TaleWorlds.CampaignSystem.Settlements;
using SandBox.View.Menu;
using TaleWorlds.CampaignSystem.Settlements.Buildings;

namespace Bannerlord.DarkNightInvasion
{
    [HarmonyPatch(typeof(DefaultPartySizeLimitModel))]
    [HarmonyPatch("GetPartyMemberSizeLimit")]
    class BiggerArmySize_Override
    {
        public static void Postfix(PartyBase party, bool includeDescriptions, ref ExplainedNumber __result)
        {
            if(party == null) return;
            if(!party.IsMobile) return;

            //increase garrion limit
            try
            {
                if (party.MobileParty.IsGarrison)
                {
                    Settlement currentSettlement = party.MobileParty.CurrentSettlement;
                    if (currentSettlement != null && (currentSettlement.IsTown || currentSettlement.IsCastle))
                    {
                        __result.Add(300 + __result.ResultNumber * 0.54f);

                        foreach (Building building in currentSettlement.Town.Buildings)
                        {
                            float buildingEffectAmount = building.GetBuildingEffectAmount(BuildingEffectEnum.GarrisonCapacity);
                            if (buildingEffectAmount > 0f)
                            {
                                __result.Add(buildingEffectAmount * 3.6f);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }

            //dark knights has bigger party size
            try
            {
                string darkKnightKingdom = "Dark Knights";
                if (party.MobileParty.IsLordParty)
                {
                    Kingdom? tmpKingdom = ModKingdomManager.FindKingdom(darkKnightKingdom);
                    if (tmpKingdom != null && party.MobileParty.ActualClan != null)
                    {
                        if (party.MobileParty.ActualClan.Kingdom != null)
                        {
                            if (String.Equals(party.MobileParty.ActualClan.Kingdom.Name.ToString(), darkKnightKingdom, System.StringComparison.OrdinalIgnoreCase))
                            {
                                if (!String.Equals(party.Owner.Name.ToString(), "Charles von Blackbournee", StringComparison.OrdinalIgnoreCase))
                                {
                                    __result.Add(300f);
                                }
                                    
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }


            try
            {
                if (party.MobileParty.IsMainParty)
                {
                    __result.Add(100 + __result.ResultNumber * 0.54f);
                }
                else if(party.MobileParty.IsLordParty)
                {
                    __result.Add(100f);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
            }
        }
    }
}
