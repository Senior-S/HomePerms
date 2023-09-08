using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SeniorS.HomePerms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace SeniorS.HomePerms.Commands
{
    public class PermBuy : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "PermBuy";

        public string Help => "Buy a new perm!";

        public string Syntax => "/permbuy <Permission Name>";

        private string SyntaxError => $"Wrong syntax! Correct usage: {Syntax}";
        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "ss.command.PermBuy" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1)
            {
                UnturnedChat.Say(caller, SyntaxError, Color.red, false);
                return;
            }

            UnturnedPlayer user = (UnturnedPlayer)caller;
            HomePerms Instance = HomePerms.Instance;

            string permName = command[0];

            Permission perm = Instance.Configuration.Instance.Permissions.FirstOrDefault(c => c.Name.Equals(permName, StringComparison.OrdinalIgnoreCase));

            if(perm == null)
            {
                Instance._messageHelper.Say(user, "no_perm", true, permName);
                return;
            }

            var group = R.Permissions.GetGroup(perm.GroupID);
            if (group == null)
            {
                Instance._messageHelper.Say(user, "no_perm", true, permName);
                Logger.Log($"ERROR! Permission ({perm.Name}) have a invalid group id!");
                return;
            }

            if (group.Members.Contains(user.CSteamID.m_SteamID.ToString()))
            {
                Instance._messageHelper.Say(user, "already_own_perm", true);
                return;
            }

            if (user.Experience < perm.Price)
            {
                Instance._messageHelper.Say(user, "no_balance", true, perm.Price);
                return;
            }

            user.Experience -= perm.Price;
            R.Permissions.AddPlayerToGroup(perm.GroupID, user);

            Instance._messageHelper.Say(user, "perm_bought", false, perm.Name);

        }
    }
}
