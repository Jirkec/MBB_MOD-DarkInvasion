using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.DarkNightInvasion.Managers
{
    public enum ETroopCategory
    {
        INFANTRY,
        RANGED,
        MOUNTED,
        NUM_OF_ELEMENTS
    }

    public class TroopManager
    {
        public static CharacterObject? GetTroop(string troopID = "", CultureObject? culture = null, int tier = -1, ETroopCategory troopCategory = ETroopCategory.INFANTRY)
        {
            CharacterObject? resultTroop = null;

            foreach(var troop in MBObjectManager.Instance.GetObjectTypeList<CharacterObject>())
            {
                if(troopID.Length > 0)
                {
                    if(String.Equals(troop.StringId, troopID, StringComparison.OrdinalIgnoreCase))
                    {
                        resultTroop = troop; break;
                    }
                }
                else if
                    (
                            (culture == null || troop.Culture == culture)
                        && (tier == -1 || troop.Tier == tier)
                        && (
                                (troopCategory == ETroopCategory.RANGED && troop.IsRanged)
                            || (troopCategory == ETroopCategory.MOUNTED && troop.IsMounted)
                            || (troopCategory == ETroopCategory.INFANTRY && troop.IsInfantry)
                            )
                        && troop.IsBasicTroop == true
                    )
                {
                    resultTroop = troop; break;
                }
            }
            
            return resultTroop;
        }

        public static ETroopCategory[] GetAllCategories()
        {
            ETroopCategory[] categories = new ETroopCategory[(int)ETroopCategory.NUM_OF_ELEMENTS];
            for(int i = 0; i < categories.Length; i++)
            {
                categories[i] = (ETroopCategory)i;
            }

            return categories;
        }
    }
}
