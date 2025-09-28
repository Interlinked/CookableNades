using System.Transactions;
using EntityStates;
using R2API;
using Rewired.Utils;
using RoR2;
using UnityEngine;


namespace CookableNades.entityState
{
    public class CookableNade : GenericProjectileBaseState
    {
        public static GameObject grenadeGhost = PrefabAPI.InstantiateClone(Paths.GameObject.CommandoGrenadeGhost, "CookableNadeGhost", false);
        public Animator nadeAnimator;
        private float cookTime = 0; 
        private bool isCooking = true;
        
        public float basePlaybackRate;
        private static int ThrowGrenadeStateHash = Animator.StringToHash("ThrowGrenade");

        private static int FireFMJParamHash = Animator.StringToHash("FireFMJ.playbackRate");

        public override void PlayAnimation(float duration)
        {   
            if ((bool)GetModelAnimator())
            {
                Animator nadeAnimator = GetModelAnimator();
                PlayAnimation("Gesture, Additive", ThrowGrenadeStateHash, FireFMJParamHash, duration * 2f);
                PlayAnimation("Gesture, Override", ThrowGrenadeStateHash, FireFMJParamHash, duration * 2f);
                basePlaybackRate = nadeAnimator.GetFloat(FireFMJParamHash);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public override void ModifyProjectileInfo(ref FireProjectileInfo fireProjectileInfo)
        {
            base.ModifyProjectileInfo(ref fireProjectileInfo);
            fireProjectileInfo.damageTypeOverride = DamageTypeCombo.GenericSpecial;
           
            
            fireProjectileInfo.useFuseOverride = true;
            fireProjectileInfo.fuseOverride = 3-Mathf.Clamp(cookTime*1.5f, 0, 3); 
        }
        public override void OnEnter()
        {
            CookableNadesMain.CookableNades.ModLogger.LogInfo("Entered CookableNade State");
            Destroy(grenadeGhost.GetComponent<ProjectileGhostController>());
            GameObject.Instantiate(grenadeGhost, FindModelChild("HandR"));
            nadeAnimator = GetModelAnimator();
            
            isCooking = true;
            cookTime = 0; // reset cookTime on enter
            base.OnEnter();
        }

        public override void OnExit()
        {
            
            
            base.OnExit();
            
        }
        public override void FixedUpdate()
        {
            
            CookableNadesMain.CookableNades.ModLogger.LogInfo(isCooking);
            if (inputBank.skill4.down && isCooking == true)
            {
                float timeUntilFreeze = 0.2f;
                bool isTicking = false;
                isCooking = true;
                cookTime += Time.fixedDeltaTime;
                CookableNadesMain.CookableNades.ModLogger.LogInfo(isTicking);
                CookableNadesMain.CookableNades.ModLogger.LogInfo(cookTime);
                if (isTicking)
                {
                    base.characterBody.SetSpreadBloom(1f, true);
                    isTicking = false;
                } else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5 / 4,1)) )
                    {
                        CookableNadesMain.CookableNades.ModLogger.LogInfo("Tick");
                        base.characterBody.SetSpreadBloom(0.5f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5 / 2,1)) )
                    {
                        CookableNadesMain.CookableNades.ModLogger.LogInfo("Tick");
                        base.characterBody.SetSpreadBloom(0.5f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3*(3/1.5 / 4),1))  )
                    {
                        CookableNadesMain.CookableNades.ModLogger.LogInfo("Tick");
                        base.characterBody.SetSpreadBloom(0.5f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5,1)) )
                    {
                        CookableNadesMain.CookableNades.ModLogger.LogInfo("Tick");
                        base.characterBody.SetSpreadBloom(0.5f, true);
                        isTicking = true;
                    }
                    nadeAnimator.SetFloat(FireFMJParamHash, 0f);
                
                
            }
            else if (isCooking == true || isCooking == false)
            {

                nadeAnimator.SetFloat(FireFMJParamHash, basePlaybackRate);
                CookableNadesMain.CookableNades.ModLogger.LogInfo(basePlaybackRate);
                CookableNadesMain.CookableNades.ModLogger.LogInfo("Done Cooking");
                isCooking = false;
                base.FixedUpdate();
            }
        }
    }
}
