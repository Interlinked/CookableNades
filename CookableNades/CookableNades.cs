using System.Transactions;
using EntityStates;
using R2API;
using Rewired.Utils;
using RoR2;
using UnityEngine;


namespace CookableNadesMain.CookableNades.entityState
{
    public class CookableNade : GenericProjectileBaseState
    {
        public GameObject nadeGhost;
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
         
            nadeGhost = GameObject.Instantiate(CookableNadesClass.grenadeGhost, FindModelChild("HandR"));
            nadeGhost.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
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
            
            CookableNadesMain.CookableNadesClass.ModLogger.LogInfo(isCooking);
            if (inputBank.skill4.down && isCooking == true)
            {
                float timeUntilFreeze = 0.2f;
                bool isTicking = false;
                isCooking = true;
                cookTime += Time.fixedDeltaTime;
             
                if (isTicking)
                {
                    base.characterBody.SetSpreadBloom(1f, true);
                    isTicking = false;
                } else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5 / 4,1)) )
                    {
                       
                        base.characterBody.SetSpreadBloom(0.6f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5 / 2,1)) )
                    {
                  
                        base.characterBody.SetSpreadBloom(0.6f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3*(3/1.5 / 4),1))  )
                    {
                        
                        base.characterBody.SetSpreadBloom(0.6f, true);
                        isTicking = true;
                    }
                    else if ((Math.Round(cookTime,1)  == Math.Round(3/1.5,1)) )
                    {
                        
                        base.characterBody.SetSpreadBloom(0.9f, true);
                        isTicking = true;
                    }
                    nadeAnimator.SetFloat(FireFMJParamHash, 0f);
                
                
            }
            else if (isCooking == true || isCooking == false)
            {

                nadeAnimator.SetFloat(FireFMJParamHash, basePlaybackRate);
            
                isCooking = false;
                Destroy(nadeGhost);
                base.FixedUpdate();
            }
        }
    }
}
