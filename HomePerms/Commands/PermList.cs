using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SeniorS.HomePerms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeniorS.HomePerms.Commands
{
    public class PermList : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "PermList";

        public string Help => "See a list of perms";

        public string Syntax => "/permlist <Page>";

        private string SyntaxError => $"Wrong syntax! Correct usage: {Syntax}";
        public List<string> Aliases => new List<string>();

        public List<string> Permissions => new List<string> { "ss.command.PermList" };

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Length != 1 || !int.TryParse(command[0], out int page) || page < 1)
            {
                UnturnedChat.Say(caller, SyntaxError, Color.red, false);
                return;
            }

            UnturnedPlayer user = (UnturnedPlayer)caller;
            HomePerms Instance = HomePerms.Instance;

            List<List<Permission>> splitPerms = SplitList(Instance.Configuration.Instance.Permissions, 4);

            if (page > splitPerms.Count)
            {
                Instance._messageHelper.Say(user, "available_pages", true, splitPerms.Count);
                return;
            }

            var perms = splitPerms[page - 1];

            Instance._messageHelper.Say(user, "available_perms", false);

            perms.ForEach(c => 
            {
                Instance._messageHelper.Say(user, "display_perm", false, c.Name, c.Price);
            });

            Instance._messageHelper.Say(user, "display_page", false, page, splitPerms.Count);

        }

        List<List<T>> SplitList<T>(List<T> list, int pageSize)
        {
            return Enumerable.Range(0, (int)Math.Ceiling((double)list.Count / pageSize))
                .Select(i => list.Skip(i * pageSize).Take(pageSize).ToList())
                .ToList();
        }
    }
}
