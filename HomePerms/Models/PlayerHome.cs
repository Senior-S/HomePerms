using Newtonsoft.Json;
using SDG.Unturned;
using Steamworks;
using UnityEngine;

namespace SeniorS.HomePerms.Models
{
    public class PlayerHome
    {
        public ulong SteamID { get; set; }

        public uint BedInstanceID { get; set; }

        [JsonConstructor]
        public PlayerHome(ulong steamid, uint bedInstanceID)
        {
            SteamID = steamid;
            BedInstanceID = bedInstanceID;
        }

        public PlayerHome(CSteamID csteamID, uint bedInstanceID)
        {
            SteamID = csteamID.m_SteamID;
            BedInstanceID = bedInstanceID;
        }

        public bool HomeExists()
        {
            BarricadeDrop drop = HomePerms.Instance.GetBedByInstanceID(this.BedInstanceID, out InteractableBed bed);

            if (drop == null || !bed.isClaimed || !bed.checkClaim(new CSteamID(SteamID))) return false;

            return true;
        }

        public Vector3 GetBedPosition()
        {
            BarricadeDrop drop = HomePerms.Instance.GetBedByInstanceID(this.BedInstanceID, out InteractableBed bed);

            var position = bed.transform.position;

            return new Vector3(position.x, position.y +1f, position.z);
        }
    }
}
