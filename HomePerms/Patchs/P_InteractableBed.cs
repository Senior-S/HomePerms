using HarmonyLib;
using SDG.Unturned;
using SeniorS.HomePerms.Models;
using System.Linq;

namespace SeniorS.HomePerms.Patchs
{
    [HarmonyPatch]
    public class P_InteractableBed
    {
        [HarmonyPatch(typeof(InteractableBed), "ReceiveClaimRequest")]
        static void Postfix(InteractableBed __instance, in ServerInvocationContext context)
        {
            BarricadeDrop drop = BarricadeManager.FindBarricadeByRootTransform(__instance.transform);

            if (drop == null) return;

            Player player = context.GetPlayer();

            PlayerHome home = HomePerms.Instance.Homes.FirstOrDefault(c => c.SteamID == player.channel.owner.playerID.steamID.m_SteamID);

            if (home == null)
            {
                home = new PlayerHome(player.channel.owner.playerID.steamID, drop.instanceID);
                HomePerms.Instance.Homes.Add(home);
                return;
            }
            
            int index = HomePerms.Instance.Homes.IndexOf(home);

            HomePerms.Instance.Homes[index].BedInstanceID = drop.instanceID;
            

        }
    }
}