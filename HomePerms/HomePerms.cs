using HarmonyLib;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using SeniorS.HomePerms.Helpers;
using SeniorS.HomePerms.Models;
using System.Collections.Generic;
using System.Linq;
using Logger = Rocket.Core.Logging.Logger;

namespace SeniorS.HomePerms
{
    public class HomePerms : RocketPlugin<Configuration>
    {
        public static HomePerms Instance;
        public MessageHelper _messageHelper;
        public List<PlayerHome> Homes;

        internal Harmony harmony;

        protected override void Load()
        {
            Instance = this;
            _messageHelper = new(this);
            Homes = new();

            harmony = new("com.seniors.homeperms");
            harmony.PatchAll(this.Assembly);

            R.Commands.OnExecuteCommand += OnExecuteCommand;

            Logger.Log($"HomePerms v{this.Assembly.GetName().Version}");
            Logger.Log("<<SSPlugins>>");
        }

        private void OnExecuteCommand(IRocketPlayer player, IRocketCommand command, ref bool cancel)
        {
            Permission perm = Configuration.Instance.Permissions.FirstOrDefault(c => command.Permissions.Contains(c.PermissionName));
            if (perm == null || cancel || player is not UnturnedPlayer) return;

            UnturnedPlayer user = (UnturnedPlayer)player;

            if (user.Experience < perm.Price)
            {
                Instance._messageHelper.Say(user, "no_balance", true, perm.Price);
                return;
            }

            user.Experience -= perm.Price;
            string message = _messageHelper.FormatMessage("perm_pay", perm.Price);
            user.Player.ServerShowHint(message, 2f);
        }

        public BarricadeDrop GetBedByInstanceID(uint instanceid, out InteractableBed interactableBed)
        {
            BarricadeDrop drop = null;
            interactableBed = null;

            List<BarricadeRegion> regions = BarricadeManager.BarricadeRegions.OfType<BarricadeRegion>().ToList();

            foreach (BarricadeRegion region in regions)
            {
                List<BarricadeDrop> drops = region.drops;

                foreach (BarricadeDrop d in drops)
                {
                    if (d.instanceID == instanceid)
                    {
                        drop = d;
                        interactableBed = d.interactable as InteractableBed;
                        break;
                    }
                }

                if (drop != null) break;
            }

            return drop;
        }

        public override TranslationList DefaultTranslations
        {
            get
            {
                return new TranslationList
                {
                    { "no_home", "You don't have a home." },
                    { "teleport_home", "Home -=color=blue=-sweet-=/color=- home!" },
                    { "available_pages", "Sorry, there's only {0} available pages." },
                    { "available_perms", "The available perms to buy are:" },
                    { "display_perm", "-=color=#10c2a7=-Name:-=/color=- {0} -=color=#e39434=-Price:-=/color=- {1}" },
                    { "display_page", "Page: {0}/{1}" },
                    { "no_perm", "A permission with name {0} doesn't exists! To see a list of available permissions do -=color=blue=-/permlist 1-=/color=-" },
                    { "no_balance", "You need at least {0} to buy this permission!" },
                    { "already_own_perm", "You already have this permission, so you can't bought it again!" },
                    { "perm_pay", "You have pay {0} for using this command!" }
                };
            }
        }

        protected override void Unload()
        {
            Instance = null;
            Homes.Clear();

            harmony.UnpatchAll(this.harmony.Id);

            R.Commands.OnExecuteCommand -= OnExecuteCommand;

            Logger.Log("<<SSPlugins>>");
        }
    }
}