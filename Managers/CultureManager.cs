using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.ObjectSystem;

namespace Bannerlord.DarkNightInvasion.Managers
{
    public class CultureManager
    {
        public static List<CultureObject> GetAllCultures()
        {
            List<CultureObject> cultures = new List<CultureObject>();
            MBObjectManager.Instance.GetObjectTypeList<CultureObject>().ForEach(culture => { cultures.Add(culture); });
            return cultures;
        }

        public static CultureObject FindCulture(string name)
        {
            return MBObjectManager.Instance.GetObjectTypeList<CultureObject>().Find( culture => String.Equals(culture.Name.ToString(), name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
