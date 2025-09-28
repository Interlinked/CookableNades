using System.Transactions;
using EntityStates;
using RoR2;
using UnityEngine;


namespace CookableNades.entityState
{
    public class CookableNade : GenericProjectileBaseState
    {
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
            fireProjectileInfo.fuseOverride = 3-Mathf.Clamp(cookTime*2, 0, 3); 
        }
        public override void OnEnter()
        {


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

                isCooking = true;
                cookTime += Time.fixedDeltaTime;
                if (cookTime / duration >= 0.2)
                {
                    nadeAnimator.SetFloat(FireFMJParamHash, 0f);
                }
                
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
