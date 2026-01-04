using FixIntPhysics;
using FixMath;
using LogicLayer;
using RenderLayer;
using SkillSystem.Config;
using UnityEngine;

namespace SkillSystem.Runtime.Logic
{
    public class SkillEffectLogic : LogicObject
    {
        private LogicActor _skillCreator;
        private SkillEffectConfig _skillEffectConfig;
        private ColliderBehaviour _collider;
        private Skill _skill;
        private int _accRunTime;

        public SkillEffectLogic(LogicObjectType objType, SkillEffectConfig effectCfg, RenderObject renderObject,
            LogicActor skillCreator,Skill skill)
        {
            this.ObjectType = objType;
            this._skillEffectConfig = effectCfg;
            this._skillCreator = skillCreator;
            this.RenderObj = renderObject;
            this.LogicXAxis = skillCreator.LogicXAxis;
            this._skill = skill;
            ObjectType = LogicObjectType.Bullet;
            if (effectCfg.effectPosType == EffectPosType.FollowDir ||
                effectCfg.effectPosType == EffectPosType.FollowPosDir)
            {
                var offset = new FixIntVector3(effectCfg.effectOffsetPosition);
                offset.x = offset.x * LogicXAxis;
                LogicPos = skillCreator.LogicPos + offset;
            }
            else if (effectCfg.effectPosType == EffectPosType.Zero)
            {
                LogicPos = FixIntVector3.zero;
            }
            else if (effectCfg.effectPosType == EffectPosType.GuidePos)
            {
                FixIntVector3 offsetFixIntVector3 = new FixIntVector3(effectCfg.effectOffsetPosition);
                offsetFixIntVector3.x *= skillCreator.LogicXAxis;
                LogicPos = _skill.SkillGuidePos + offsetFixIntVector3;
            }
        }

        public void OnLogicFrameEffectUpdate(Skill skill, int logicFrame)
        {
            // 1.处理特效位置配置，比如 如果是跟随对象，做每个逻辑帧跟随
            ResolveEffectPosType();

            // 2.处理特效行动配置，让特效能够随着配置移动
            ResolveEffectAction(skill, logicFrame);

            // 3.处理伤害配置，让伤害碰撞体跟随动效移动
            ResolveEffectDamage(skill, logicFrame);
        }

        private void ResolveEffectDamage(Skill skill, int logicFrame)
        {
            if (_skillEffectConfig.isAttachDamage)
            {
                if (_skillEffectConfig.damageConfig.triggerFrame == logicFrame)
                {
                    _collider = skill.CreateOrUpdateCollider(_skillEffectConfig.damageConfig, null, this);
                    if (_skillEffectConfig.damageConfig.triggerIntervalMS == 0)
                    {
                        skill.TriggerColliderDamage(_collider, _skillEffectConfig.damageConfig);
                    }
                }

                // 处理间隔性伤害
                if (_skillEffectConfig.damageConfig.triggerIntervalMS != 0 && _collider != null)
                {
                    _accRunTime += LogicFrameConfig.LogicFrameIntervalMs;
                    if (_accRunTime >= _skillEffectConfig.damageConfig.triggerIntervalMS)
                    {
                        skill.TriggerColliderDamage(_collider, _skillEffectConfig.damageConfig);
                        _accRunTime -= LogicFrameConfig.LogicFrameIntervalMs;
                    }
                }

                // 更新碰撞体位置
                if (_skillEffectConfig.damageConfig.isFollowEffect)
                {
                    skill.CreateOrUpdateCollider(_skillEffectConfig.damageConfig, _collider, this);
                }
            }
        }

        private void ResolveEffectAction(Skill skill, int logicFrame)
        {
            if (_skillEffectConfig.isAttachAction && logicFrame == _skillEffectConfig.actionConfig.triggerFrame)
            {
                skill.AddMoveAction(_skillEffectConfig.actionConfig,this, _skillEffectConfig.effectOffsetPosition,finishedCallback:() =>
                {
                    _collider?.OnRelease();
                    skill.DestroyEffect(_skillEffectConfig);
                    _collider = null;
                    
                },updateCallback:()=>
                {
                    // 如果 技能状态已经结束，但是技能的移动特效操作未完成 则继续更新直到技能的移动等操作完成
                    if (skill.State == SkillState.End)
                    {
                        // 更新碰撞体位置
                        if (_skillEffectConfig.damageConfig.isFollowEffect)
                        {
                            skill.CreateOrUpdateCollider(_skillEffectConfig.damageConfig, _collider, this);
                        }

                        if (_skillEffectConfig.isAttachDamage)
                        {
                            // 处理间隔性伤害
                            if (_skillEffectConfig.damageConfig.triggerIntervalMS != 0 && _collider != null)
                            {
                                _accRunTime += LogicFrameConfig.LogicFrameIntervalMs;
                                if (_accRunTime >= _skillEffectConfig.damageConfig.triggerIntervalMS)
                                {
                                    skill.TriggerColliderDamage(_collider, _skillEffectConfig.damageConfig);
                                    _accRunTime -= LogicFrameConfig.LogicFrameIntervalMs;
                                }
                            }
                        }
                    }
                       
                });
            }
        }

        private void ResolveEffectPosType()
        {
            if (_skillEffectConfig.effectPosType == EffectPosType.FollowPosDir)
            {
                var offset = new FixIntVector3(_skillEffectConfig.effectOffsetPosition);
                offset.x = offset.x * LogicXAxis;
                offset.z = offset.z * LogicXAxis;
                LogicPos = _skillCreator.LogicPos + offset;
                LogicXAxis =  _skillCreator.LogicXAxis;
            }
        }
        

        public override void OnDestroy()
        {
            base.OnDestroy();
            _collider?.OnRelease();
            _collider = null;
            RenderObj?.OnRelease();
        }
    }
}