using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SeniorS.HomePerms.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeniorS.HomePerms.Commands
{
    public class Home : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "Home";

        public string Help => "Teleport to your home; If you have one.";

        public string Syntax => "/Home";

        private string SyntaxError => $"Wrong syntax! Correct usage: {Syntax}";
        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "ss.command.Home" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length > 0)
            {
                UnturnedChat.Say(caller, SyntaxError, Color.red, false);
                return;
            }

            UnturnedPlayer user = (UnturnedPlayer)caller;
            HomePerms Instance = HomePerms.Instance;

            PlayerHome home = Instance.Homes.FirstOrDefault(c => c.SteamID == user.CSteamID.m_SteamID);

            if(home == null || !home.HomeExists())
            {
                Instance._messageHelper.Say(user, "no_home", true);
                return;
            }

            Vector3 position = home.GetBedPosition();

            user.Player.teleportToLocationUnsafe(position, user.Rotation);
            Instance._messageHelper.Say(user, "teleport_home", false);
        }
    }
}
