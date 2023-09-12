using Rocket.API;
using SeniorS.HomePerms.Models;
using System.Collections.Generic;

namespace SeniorS.HomePerms
{
    public class Configuration : IRocketPluginConfiguration
    {
        public void LoadDefaults()
        {
            hexDefaultMessagesColor = "#2BC415";

            Permissions = new()
            {
                new Permission
                {
                    Name = "Vip Upgrade",
                    PermissionName = "vip",
                    Price = 5000,
                }
            };
        }

        public string hexDefaultMessagesColor;

        public List<Permission> Permissions;
    }
}