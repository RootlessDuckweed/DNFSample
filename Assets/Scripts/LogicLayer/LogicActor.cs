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

        public virtual void SkillDamage(FixInt damage,SkillDamageConfig skillDamageCfg,LogicActor source)
        {
            Debug.Log(RenderObj.name + " - SkillDamage: "+damage);
            CalculateDamage(damage,DamageSource.Skill,source);
        }

        public virtual void CalculateDamage(FixInt damage,DamageSource damageSource,LogicObject source)
        {
            // 判断对象是否死亡
            if (ObjectState == LogicObjectState.Survival)
            {
                // 对象逻辑层血量减少
                ReduceHP(damage);
                if (this.HP <= FixInt.Zero)
                {
                    Collider.Active = false;
                    ObjectState = LogicObjectState.Death;
                    RenderObj.OnDeath();
                }
                // 进行伤害数值飘字渲染
                RenderObj.Damage(damage.RawInt,damageSource,source);
            }
        }

        public void BulletDamage(FixInt damage,SkillDamageConfig damageCfg,LogicObject source)
        {
            Debug.Log(RenderObj.name + " - BulletDamage: "+damage);
            CalculateDamage(damage,DamageSource.Bullet,source);
        }
        
        public void BuffDamage(FixInt damage,SkillDamageConfig damageCfg,LogicObject source)
        {
            Debug.Log(RenderObj.name + " - BuffDamage: "+damage);
            CalculateDamage(damage,DamageSource.Buff,source);
        }
        
        public virtual void OnHit(string effectPath,int survivalTimeMs,LogicObject source,LogicObject effectPoint)
        {
            RenderObj.OnHit(effectPath,survivalTimeMs,source,effectPoint);
        }
        

        public virtual void Floating(bool isUpFloating) { }

        public virtual void TriggerGround() { }
    }
}