using FixMath;
using RenderLayer;
using SkillSystem.Config;
using SkillSystem.Runtime;
using SkillSystem.Runtime.Logic;
using UnityEngine;

namespace LogicLayer
{
    public partial class LogicActor : LogicObject
    {
        public override void OnCreate()
        {
            base.OnCreate();
            InitActorSkill();
        }

        public override void OnLogicFrameUpdate()
        {
            base.OnLogicFrameUpdate();
            //处理 移动帧
            OnLogicFrameUpdateMove();
            //处理 技能帧
            OnLogicFrameUpdateSkill();
            //处理 重力帧
            OnLogicFrameUpdateGravity();
            //处理 子弹帧
            OnLogicFrameUpdateBullet();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


        public void PlayAnim(AnimationClip clip)
        {
            RenderObj.PlayAnim(clip);
        }
        
        public void PlayAnim(string clipName)
        {
            RenderObj.PlayAnim(clipName);
        }

        public virtual void SkillDamage(FixInt damage,SkillDamageConfig skillDamageCfg)
        {
            Debug.Log(RenderObj.name + " - SkillDamage: "+damage);
            CalculateDamage(damage,DamageSource.Skill);
        }

        public void CalculateDamage(FixInt damage,DamageSource damageSource)
        {
            // 判断对象是否死亡
            if (ObjectState == LogicObjectState.Survival)
            {
                // 对象逻辑层血量减少
                // 进行伤害数值飘字渲染
                RenderObj.Damage(damage.RawInt,damageSource);
                
            }
        }

        public void BulletDamage(FixInt damage,SkillDamageConfig damageCfg)
        {
            Debug.Log(RenderObj.name + " - BulletDamage: "+damage);
            CalculateDamage(damage,DamageSource.Bullet);
        }
        
        public virtual void OnHit(GameObject effect,int survivalTimeMs,LogicObject source)
        {
            RenderObj.OnHit(effect,survivalTimeMs,source);
        }
        
        public virtual void OnHitByBullet(GameObject effect,int survivalTimeMs,LogicObject source)
        {
            RenderObj.OnHitByBullet(effect,survivalTimeMs,source);
        }

        public virtual void Floating(bool isUpFloating) { }

        public virtual void TriggerGround() { }
    }
}