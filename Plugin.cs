using BepInEx;
using BepInEx.Configuration;
using Aki.Reflection.Patching;
using Aki.Reflection.Utils;
using System.Reflection;
using UnityEngine;
using EFT.UI;
using HarmonyLib;

namespace AdjustableTraderRows
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        // create config ent
        public static ConfigEntry<int> configNumberInARow;

        private void Awake()
        {
            // set up config
            configNumberInARow = Config.Bind("General", "Traders In a Row", 6, new ConfigDescription("Number of traders in a single row", new AcceptableValueRange<int>(1, 16)));


            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            new TraderRowsPatch().Enable();
        }

        public class TraderRowsPatch : ModulePatch
        {
            protected override MethodBase GetTargetMethod()
            {
                return AccessTools.Method(typeof(MerchantsList), nameof(MerchantsList.method_4));
            }

            [PatchPostfix]
            private static void AdjustTraderRows()
            {

                var TradersContainer = GameObject.Find("Menu UI/UI/Merchants List/Traders Container");
                var TradeRectTrans = TradersContainer.GetComponent<RectTransform>();

                float DefaultSize = 177.25f;
                float ResultRows = DefaultSize * (float)Plugin.configNumberInARow.Value;


                TradeRectTrans.sizeDelta = new Vector2(ResultRows, 481f);



            }
        }
    }
}
