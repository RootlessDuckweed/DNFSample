using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using LogicLayer;
using SkillSystem.Config;
using SkillSystem.Runtime;
using ZMGC.Battle;

namespace SkillSystem.Buff.BuffCfg
{
    /// <summary>
    /// 只负责buff碰撞体的生成和目标检测
    /// </summary>
    public class BuffCollider
    {
        /// <summary>
        /// 当前碰撞体实例
        /// </summary>
        private ColliderBehaviour _buffCollider;
        /// <summary>
        /// buff配置
        /// </summary>
        private BuffConfig _buffConfig;
        /// <summary>
        /// buff伤害配置
        /// </summary>
        private SkillDamageConfig _damageCfg;
        /// <summary>
        /// buff释放者
        /// </summary>
        private LogicActor _releaser;
        /// <summary>
        /// buff附加目标
        /// </summary>
        private LogicActor _target;

        private Skill _skill;

        public BuffCollider(BuffInstance.Buff buff)
        {
            _buffConfig = buff.BuffCfg;
            _damageCfg = buff.BuffCfg.targetConfig.damageCfg;
            _releaser = buff.Releaser;
            _skill = buff.Skill;
            _target = buff.AttachTarget;
        }
        
        //0.初始化碰撞体数据
        //1.生成对应碰撞体
        //2.更新碰撞体
        //3.释放碰撞体
        public ColliderBehaviour CreateOrUpdateCollider()
        {
            
            if (_damageCfg.damageDetectionMode == DamageDetectionMode.Box3D)
            {
                FixIntVector3 boxOffset = new FixIntVector3(_damageCfg.boxOffset);
                FixIntVector3 boxSize = new FixIntVector3(_damageCfg.boxSize);
                
                if(_buffCollider == null)
                    _buffCollider = new FixIntBoxCollider(boxSize, boxOffset);
                
                _buffCollider.SetBoxData(boxOffset, boxSize);
                _buffCollider.UpdateColliderInfo(GetBuffPos(), boxSize);
            }
            else if (_damageCfg.damageDetectionMode == DamageDetectionMode.Sphere3D)
            {
                FixIntVector3 sphereOffset = new FixIntVector3(_damageCfg.sphereOffset);
                
                if(_buffCollider == null)
                    _buffCollider = new FixIntSphereCollider(_damageCfg.radius, sphereOffset);
                
                _buffCollider.SetBoxData(_damageCfg.radius, sphereOffset);
                _buffCollider.UpdateColliderInfo(GetBuffPos(), FixIntVector3.zero, _damageCfg.radius);
            }

            return _buffCollider;
        }

        public List<LogicActor> CalculateTargetObjects()
        {
            // 获取敌人目标列表 英雄 敌人
            List<LogicActor> enemyList =
                BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(_releaser.ObjectType);
            // 通过碰撞检测 去检测碰到的敌人
            var damageTargetList = new List<LogicActor>();
            foreach (var item in enemyList)
            {
                if (_buffCollider.ColliderType == ColliderType.Box)
                {
                    if (PhysicsManager.IsCollision(_buffCollider as FixIntBoxCollider, item.Collider))
                    {
                        damageTargetList.Add(item);
                    }
                }
                else if (_buffCollider.ColliderType == ColliderType.Sphere)
                {
                    if (PhysicsManager.IsCollision(item.Collider, _buffCollider as FixIntSphereCollider))
                    {
                        damageTargetList.Add(item);
                    }
                }
            }
            return damageTargetList;
        }

        /// <summary>
        /// 获取buff附加位置
        /// </summary>
        /// <returns></returns>
        public FixIntVector3 GetBuffPos()
        {
            if (_buffConfig.attachType == BuffAttachType.Guide_Pos)
            {
                return _skill.SkillGuidePos;
            }
            else if(_buffConfig.attachType == BuffAttachType.Creator)
            {
                return _releaser.LogicPos;
            }
            else if (_buffConfig.attachType == BuffAttachType.Target)
            {
                return  _target.LogicPos;
            }

            return _releaser.LogicPos;
        }

        public void OnRelease()
        {
            _buffCollider?.OnRelease();
            _buffCollider = null;
        }
    }
}