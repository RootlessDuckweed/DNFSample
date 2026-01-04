using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using Game;
using LogicLayer;
using SkillSystem.Config;
using SkillSystem.Buff;
using ZMGC.Battle;

namespace SkillSystem.Runtime
{
    /// <summary>
    /// 伤害来源枚举
    /// </summary>
    public enum DamageSource
    {
        None,
        Skill, // 技能伤害
        Buff,  // buff伤害
        Bullet,// 子弹伤害
    }
    public partial class Skill
    {
        private Dictionary<int, ColliderBehaviour> _colliderDic = new Dictionary<int, ColliderBehaviour>();
        private List<int> _curCreateDamageAccTimeList = new List<int>(); // 当前伤害累加时间

        private void OnDamageInit()
        {
            if (_skillDataConfig.damageCfgList is { Count: > 0 })
            {
                for (int i = 0; i < _skillDataConfig.damageCfgList.Count; i++)
                {
                    _curCreateDamageAccTimeList.Add(0);
                }
            }
        }
        public void OnLogicFrameUpdateDamage()
        {
            if (_skillDataConfig.damageCfgList != null && _skillDataConfig.damageCfgList.Count > 0)
            {
                for (int i = 0; i < _skillDataConfig.damageCfgList.Count; i++)
                {
                    var item = _skillDataConfig.damageCfgList[i];
                    int hashCode = item.GetHashCode();
                    
                    // 更新碰撞体位置，如果是跟随玩家位置
                    if (item.colliderPosType == ColliderPosType.FollowPos)
                    {
                        if (_colliderDic.TryGetValue(item.GetHashCode(), out var damageCollider) &&
                            damageCollider != null)
                        {
                            CreateOrUpdateCollider(item, damageCollider);
                        }
                    }

                    // 正常创建碰撞体，如果到了触发帧
                    if (_curLogicFrame == item.triggerFrame)
                    {
                        DestroyCollider(item);
                        ColliderBehaviour collider = CreateOrUpdateCollider(item, null);
                        _colliderDic.Add(hashCode, collider);
                        if (item.triggerIntervalMS == 0)
                        {
                            // 触发一次伤害
                            if (_colliderDic.ContainsKey(hashCode))
                            {
                                TriggerColliderDamage(_colliderDic[hashCode], item);
                            }
                        }
                    }

                    if (item.triggerIntervalMS != 0)
                    {
                        _curCreateDamageAccTimeList[i] += LogicFrameConfig.LogicFrameIntervalMs;
                        if (_curCreateDamageAccTimeList[i] >= item.triggerIntervalMS)
                        {
                            // 触发一次伤害
                            _curCreateDamageAccTimeList[i] = 0;
                            if (_colliderDic.ContainsKey(hashCode))
                            {
                                TriggerColliderDamage(_colliderDic[hashCode], item);
                            }
                        }
                    }

                    if (_curLogicFrame == item.endFrame)
                    {
                        DestroyCollider(item);
                    }
                }
            }
        }

        public ColliderBehaviour CreateOrUpdateCollider(SkillDamageConfig item,ColliderBehaviour damageCollider,LogicObject followObj = null)
        {
            ColliderBehaviour collider = damageCollider;
            
            LogicObject followTargetObj = followObj ?? _skillCreator;
            
            if (item.damageDetectionMode == DamageDetectionMode.Box3D)
            {
                FixIntVector3 boxOffset = new FixIntVector3(item.boxOffset);
                FixIntVector3 boxSize = new FixIntVector3(item.boxSize);
                boxOffset.x = boxOffset.x * followTargetObj.LogicXAxis;
                
                if(collider == null)
                    collider = new FixIntBoxCollider(boxSize, boxOffset);
                
                collider.SetBoxData(boxOffset, boxSize);
                collider.UpdateColliderInfo(followTargetObj.LogicPos, boxSize);
            }
            else if (item.damageDetectionMode == DamageDetectionMode.Sphere3D)
            {
                FixIntVector3 sphereOffset = new FixIntVector3(item.sphereOffset);
                sphereOffset.x = sphereOffset.x * followTargetObj.LogicXAxis;
                
                if(collider == null)
                    collider = new FixIntSphereCollider(item.radius, sphereOffset);
                
                collider.SetBoxData(item.radius, sphereOffset);
                collider.UpdateColliderInfo(followTargetObj.LogicPos, FixIntVector3.zero, item.radius);
            }

            return collider;
        }

        public void TriggerColliderDamage(ColliderBehaviour collider, SkillDamageConfig skillDamageCfg)
        {
            // 获取敌人目标列表 英雄 敌人
            List<LogicActor> enemyList =
                BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>().GetEnemyList(_skillCreator.ObjectType);
            // 通过碰撞检测 去检测碰到的敌人
            var damageTargetList = new List<LogicActor>();
            foreach (var item in enemyList)
            {
                if (collider.ColliderType == ColliderType.Box)
                {
                    if (PhysicsManager.IsCollision(collider as FixIntBoxCollider, item.Collider))
                    {
                        damageTargetList.Add(item);
                    }
                }
                else if (collider.ColliderType == ColliderType.Sphere)
                {
                    if (PhysicsManager.IsCollision(item.Collider, collider as FixIntSphereCollider))
                    {
                        damageTargetList.Add(item);
                    }
                }
            }

            // 获取攻击目标后 对敌人造成伤害
            enemyList.Clear();
            foreach (var target in damageTargetList)
            {
                // 造成伤害
                // TODO : 需要有统一的计算公式，先随便给个数值测试
                target.SkillDamage(DamageCalculateCenter.CalculateDamage(skillDamageCfg,_skillCreator,target), skillDamageCfg,_skillCreator);
                
                if (skillDamageCfg.addBuffs != null && skillDamageCfg.addBuffs.Length > 0)
                {
                    foreach (var buffId in skillDamageCfg.addBuffs)
                    {
                        BuffSystem.Instance.AttachBuff(buffId, _skillCreator, target, this, null);
                    }
                }
                
                // 添加伤害特效
                AddHitEffect(target,_skillCreator);
                
                // 播放击中音效
                PlayHitAudio();
            }
            //处理造成伤害后触发的技能
            if (damageTargetList.Count > 0) // 说明打中了敌人造成了伤害
            {
                // 触发对应技能
                if (skillDamageCfg.triggerSkillID != 0)
                {
                    _comboBinationSkillId = skillDamageCfg.triggerSkillID;
                    
                }
            }
        }

        private void AddHitEffect(LogicActor targetObj,LogicActor sourceObj)
        {
            targetObj.OnHit(_skillDataConfig.skillCfg.skillHitEffectPath,_skillDataConfig.skillCfg.hitEffectSurvivalTimeMs,sourceObj,targetObj);
        }

        private void DestroyCollider(SkillDamageConfig item)
        {
            int hashCode = item.GetHashCode();
            _colliderDic.TryGetValue(hashCode, out var collider);
            if (collider != null)
            {
                _colliderDic.Remove(hashCode);
                collider.OnRelease();
            }
        }
        
        public void OnDamageRelease()
        {
            _curCreateDamageAccTimeList.Clear();
        }
    }
}