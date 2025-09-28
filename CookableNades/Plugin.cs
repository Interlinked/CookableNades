using BepInEx;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Reflection;
using RoR2.UI;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets.ResourceLocators;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using R2API;


namespace CookableNadesMain {
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class CookableNadesClass : BaseUnityPlugin {
        public static GameObject grenadeGhost;
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "ModAuthorName";
        public const string PluginName = "CookableNades";
        public const string PluginVersion = "1.0.0";

        public static BepInEx.Logging.ManualLogSource ModLogger;

        public void Awake()
        {
            // set logger
            ModLogger = Logger;
            Paths.SkillDef.ThrowGrenade.activationState = new SerializableEntityStateType(typeof(CookableNades.entityState.CookableNade));

            grenadeGhost = PrefabAPI.InstantiateClone(Paths.GameObject.CommandoGrenadeGhost, "CookableNadeGhost", false);
            Destroy(grenadeGhost.GetComponent<ProjectileGhostController>());
            EntityStateConfiguration config = GameObject.Instantiate(Paths.EntityStateConfiguration.EntityStatesCommandoCommandoWeaponThrowGrenade);
            config.name = "ThrowGrenade";
            config.targetType = (HG.SerializableSystemType)typeof(CookableNades.entityState.CookableNade);
            ContentAddition.AddEntityStateConfiguration(config);
            GameObject commandoGrenadeProjectile = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoGrenadeProjectile.prefab").WaitForCompletion();
            GameObject commandoBodyPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion();
            
            ContentAddition.AddEntityState<CookableNades.entityState.CookableNade>(out _);
        }
    }
}