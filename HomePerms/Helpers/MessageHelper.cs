using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using Logger = Rocket.Core.Logging.Logger;

namespace SeniorS.HomePerms.Helpers
{
    public class MessageHelper
    {
        private readonly HomePerms Instance;
        private readonly Color defaultMessageColor;

        public MessageHelper(HomePerms instance)
        {
            Instance = instance;
            defaultMessageColor = HexToColor(Instance.Configuration.Instance.hexDefaultMessagesColor);
        }

        public void Say(IRocketPlayer caller, string translationkey, bool error, params object[] values)
        {
            string message = FormatMessage(translationkey, values);

            UnturnedChat.Say(caller, message, error ? HexToColor("#F82302") : defaultMessageColor, true);
        }

        private Color HexToColor(string hex)
        {
            if (!ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                Logger.LogError($"Could not convert {hex} to a Color.");
                return Color.white;
            }

            return color;
        }

        public string FormatMessage(string translationKey, params object[] values)
        {
            string baseMessage = Instance.Translate(translationKey, values);
            baseMessage = baseMessage.Replace("-=", "<").Replace("=-", ">");

            return baseMessage;

        }
    }
}