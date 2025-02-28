using Comfort.Common;
using EFT;
using HarmonyLib;
using SAIN.Components;
using SAIN.SAINComponent.Classes;
using System;
using System.Reflection;
using Aki.Reflection.Patching;
using UnityEngine;
using static EFT.Player;
//using GrenadeFinishResult = GInterface145;
using ThrowWeapItemClass = GrenadeClass;

namespace SAIN.Patches.Shoot.Grenades
{
    public class SetGrenadePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //public void method_3(GrenadeClass potentialGrenade)
            //{
            //    this.grenade = potentialGrenade;
            //    this.Mass = this.grenade.Weight;
            //    this.Mass = 0.6f;
            //}
            return AccessTools.Method(typeof(BotGrenadeController), "method_3");
        }

        [PatchPostfix]
        public static void Patch(BotOwner ___botOwner_0, ThrowWeapItemClass potentialGrenade, BotGrenadeController __instance)
        {
            if (potentialGrenade == null) {
                return;
            }
            if (!SAINBotController.Instance.GetSAIN(___botOwner_0, out var botComponent)) {
                return;
            }
            //__instance.Mass = potentialGrenade.Weight;
            botComponent.Grenade.MyGrenade = potentialGrenade;
        }
    }

    public class DisableSpreadPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //float grenadePrecision = this.botOwner_0.Settings.FileSettings.Grenade.GrenadePrecision;
            //if (grenadePrecision > 0f)
            return AccessTools.Method(typeof(BotGrenadeController), "method_0");
        }

        [PatchPrefix]
        public static bool Patch(BotOwner ___botOwner_0, ref Vector3 ____precisionOffset)
        {
            if (!SAINBotController.Instance.GetSAIN(___botOwner_0, out var botComponent)) {
                return true;
            }
            ____precisionOffset = Vector3.zero;
            return false;
        }
    }

    public class ResetGrenadePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //public void method_2()
            //{
            //    List<GrenadeClass> list = this.botOwner_0.GetPlayer.GClass2772_0.Inventory.GetPlayerItems(EPlayerItems.Equipment).OfType<GrenadeClass>().ToList<GrenadeClass>();
            //    foreach (GrenadeClass grenadeClass in list)
            //    {
            //        if (grenadeClass.ThrowType == ThrowWeapType.frag_grenade)
            //        {
            //            this.method_3(grenadeClass);
            //            return;
            //        }
            //    }
            return AccessTools.Method(typeof(BotGrenadeController), "method_2");
        }

        [PatchPostfix]
        public static void Patch(BotOwner ___botOwner_0, ThrowWeapItemClass ___grenade)
        {
            if (!SAINBotController.Instance.GetSAIN(___botOwner_0, out var botComponent)) {
                return;
            }
            botComponent.Grenade.MyGrenade = ___grenade;
        }
    }

    public class DoThrowPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotGrenadeController), "DoThrow");
        }

        [PatchPrefix]
        public static bool Patch(BotOwner ___botOwner_0, ref bool __result, BotGrenadeController __instance, ref GrenadeActionType ___GrenadeActionType, ref bool ____checkStop, ref float ____clearTime, ThrowWeapItemClass ___grenade)
        {
            if (SAINPlugin.IsBotExluded(___botOwner_0)) {
                return true;
            }
            if (__instance.AIGreanageThrowData == null) {
                return false;
            }
            if (__instance.CheckPeriodTime()) {
                return false;
            }
            if (__instance.ThrowindNow == true) {
                return false;
            }
            //public bool method_4()
            //{
            //if (this.AIGreanageThrowData != null)
            //{
            //    if (this.AIGreanageThrowData.Force < 0.01f)
            //    {
            //        return true;
            //    }
            __instance.method_4();
            switch (___GrenadeActionType) {
                case GrenadeActionType.ready: {
						____checkStop = true;
						____clearTime = Time.time + 4f;
						___GrenadeActionType = GrenadeActionType.change2grenade;
						if (___grenade == null)
						{
                            //public void method_6(GrenadeClass grenade = null)
                            //{
                            //    this.ThrowindNow = false;
                            //    this.method_0();
                            //    this._clearTime = 0f;
                            //    this._checkStop = false;
                            //    this.GrenadeActionType = GrenadeActionType.ready;
                            __instance.method_6(null);
							return false;
						}
						if (__instance.AIGreanageThrowData.GrenadeType != null)
						{
                            //public void method_1(ThrowWeapType grenadeType)
                            //{
                            //    using (IEnumerator<Item> enumerator = this.botOwner_0.GetPlayer.GClass2772_0.Inventory.GetPlayerItems(EPlayerItems.Equipment).Where(new Func<Item, bool>(BotGrenadeController.Class165.class165_0.method_1)).GetEnumerator())
                            //    {
                            __instance.method_1(__instance.AIGreanageThrowData.GrenadeType.Value);
						}
						BotPersonalStats botPersonalStats = ___botOwner_0.BotPersonalStats;
						if (botPersonalStats != null)
						{
							botPersonalStats.GrendateThrow(null);
						}
						__instance.ThrowindNow = true;
						___botOwner_0.GetPlayer.SetInHandsForQuickUse(___grenade, __instance.method_9);
						break;
					}
            }
            return false;
        }
    }
}