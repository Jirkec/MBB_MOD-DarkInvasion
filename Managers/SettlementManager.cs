using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Library;

namespace Bannerlord.DarkNightInvasion.Managers
{
    class SettlementManager
    {
        public static Settlement? FindSettlement(string name)
        {
            Settlement? settlement = null;

            settlement = Settlement.All.Find(settlement => String.Equals(settlement.Name.ToString(), name, StringComparison.OrdinalIgnoreCase));

            return settlement;
        }

        public static List<string> GetSettlementNames()
        {
            List<string> names = new List<string>();

            Settlement.All.ForEach(settlemnt => names.Add(settlemnt.Name.ToString()));
            names.Sort();

            return names;
        }

        public static Vec2 GetSettlementPosition(Settlement settlement)
        {
            return settlement.Position2D;
        }
    }
}
