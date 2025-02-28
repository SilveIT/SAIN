using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using SAIN.Components;
using System.Reflection;
using Aki.Reflection.Patching;
using UnityEngine;
using static SAIN.Helpers.Shoot;
using WeaponAIPresetManager = GClass401; // Contains property WeaponAIPreset

namespace SAIN.Patches.Shoot.RateOfFire
{
    public class FullAutoPatch : ModulePatch
    {
        private static PropertyInfo _ShootData;

        protected override MethodBase GetTargetMethod()
        {
            _ShootData = AccessTools.Property(typeof(BotOwner), "ShootData");
            //public void method_6()
            //{
            //    float num = this._owner.WeaponManager.WeaponAIPreset.TriggerDownTime();
            //    this.nextFingerUpTime = Time.time + num;
            //}
            return AccessTools.Method(_ShootData.PropertyType, "method_6");
        }

        [PatchPrefix]
        public static bool PatchPrefix(BotOwner ____owner, ref float ___nextFingerUpTime)
        {
            if (SAINPlugin.IsBotExluded(____owner))
            {
                return true;
            }
            if (____owner.AimingData == null)
            {
                return true;
            }

            Weapon weapon = ____owner.WeaponManager.CurrentWeapon;

            if (weapon.SelectedFireMode == Weapon.EFireMode.fullauto)
            {
                float distance = ____owner.AimingData.LastDist2Target;
                float scaledDistance = FullAutoBurstLength(____owner, distance);

                ___nextFingerUpTime = scaledDistance + Time.time;

                return false;
            }

            ___nextFingerUpTime = 0.001f + Time.time;

            return true;
        }
    }

    public class SemiAutoPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //public float method_1()
            //{
            //    return this.botGlobalShootData_0.WAIT_NEXT_SINGLE_SHOT;
            //}
            return AccessTools.Method(typeof(WeaponAIPresetManager), "method_1");
        }

        [PatchPostfix]
        public static void PatchPostfix(BotOwner ___botOwner_0, ref float __result)
        {
            if (SAINPlugin.IsBotExluded(___botOwner_0))
            {
                return;
            }
            if (SAINBotController.Instance.GetSAIN(___botOwner_0, out var component))
            {
                __result = component.Info.WeaponInfo.Firerate.SemiAutoROF();
            }
        }
    }

    public class SemiAutoPatch2 : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //public float method_6()
            //{
            //    return this.gclass535_0.CurrentHoldDownAutoFire * GClass766.Random(0.7f, 1.3f);
            //}
            return AccessTools.Method(typeof(WeaponAIPresetManager), "method_6");
        }

        [PatchPostfix]
        public static void PatchPostfix(BotOwner ___botOwner_0, ref float __result)
        {
            if (SAINPlugin.IsBotExluded(___botOwner_0))
            {
                return;
            }
            if (SAINBotController.Instance.GetSAIN(___botOwner_0, out var component))
            {
                __result = component.Info.WeaponInfo.Firerate.SemiAutoROF();
            }
        }
    }

    public class SemiAutoPatch3 : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            //public float method_0()
            //{
            //    return this.gclass535_0.CurrentHoldDownSingleShot;
            //}
            return AccessTools.Method(typeof(WeaponAIPresetManager), "method_0");
        }

        [PatchPostfix]
        public static void PatchPostfix(BotOwner ___botOwner_0, ref float __result)
        {
            if (SAINPlugin.IsBotExluded(___botOwner_0))
            {
                return;
            }
            if (SAINBotController.Instance.GetSAIN(___botOwner_0, out var component))
            {
                __result = component.Info.WeaponInfo.Firerate.SemiAutoROF();
            }
        }
    }
}