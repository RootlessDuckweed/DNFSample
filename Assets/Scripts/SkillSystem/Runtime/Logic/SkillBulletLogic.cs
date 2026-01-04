using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using Game;
using LogicLayer;
using RenderLayer;
using SkillSystem.Buff;
using SkillSystem.Config;
using ZMGC.Battle;

namespace SkillSystem.Runtime.Logic
{
    public class SkillBulletLogic : LogicObject
    {
        private readonly Skill _skill;
        private readonly LogicActor _fireLogicActor;
        private readonly SkillBulletConfig _bulletCfg;
        private ColliderBehaviour _bulletCollider;

        // 当前逻辑帧
        private int _curLogicFrame = 0;
        // 当前逻辑帧累加时间
        private int _curLogicFrameAccTime;
        // 子弹是否击中目标
        private bool _isBulletHit = false;
        
        private List<LogicActor> _hitTargetsList = new List<LogicActor>();
        // 子弹是否失效
        public bool IsFailure; 

        public SkillBulletLogic(Skill skill,LogicActor fireLogicActor,RenderObject renderObject,SkillBulletConfig bulletCfg, FixIntVector3 rangePos)
        {
            _skill = skill;
            _fireLogicActor = fireLogicActor;
            _bulletCfg = bulletCfg;
            RenderObj = renderObject;
            
            // 更新轴向
            LogicXAxis = _fireLogicActor.LogicXAxis;
            // 初始化对象偏移位置
            FixIntVector3 pos = new FixIntVector3(_bulletCfg.offset)+rangePos;
            pos.x *= LogicXAxis;
            pos.y = FixIntMath.Abs(pos.y);
            LogicPos = _fireLogicActor.LogicPos + pos;
            // 更新角度
            var angle = new FixIntVector3(_bulletCfg.angle)*LogicXAxis;
            if (LogicXAxis < 0)
            {
                angle += new FixIntVector3(0, 0, 180);
            }
            LogicAngle = angle;
            LogicDir = new FixIntVector3(LogicXAxis, 0, 0) + new FixIntVector3(_bulletCfg.dirOffset);
            if (_bulletCfg.isAttachDamage)
            {
                SkillDamageConfig damageConfig = _bulletCfg.damageCfg;
                if (damageConfig.damageDetectionMode == DamageDetectionMode.Box3D)
                {
                    _bulletCollider = new FixIntBoxCollider(FixIntVector3.zero, FixIntVector3.zero);
                }
                else if (damageConfig.damageDetectionMode == DamageDetectionMode.Sphere3D)
                {
                    _bulletCollider = new FixIntSphereCollider(damageConfig.radius, FixIntVector3.zero);
                }
            }
        }
        
        
        public override void OnLogicFrameUpdate()
        {
            base.OnLogicFrameUpdate();
            _curLogicFrameAccTime = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMs;
            _curLogicFrame++;
            
            //子弹击中逻辑
            foreach (var target in _hitTargetsList)
            {
                target.BulletDamage(DamageCalculateCenter.CalculateDamage(_bulletCfg.damageCfg,_fireLogicActor,target),_bulletCfg.damageCfg,_fireLogicActor); // 造成伤害
                target.OnHit(_bulletCfg.hitEffectPath,_bulletCfg.hitEffectSurvivalTimeMs,_fireLogicActor,this); // 播放特效
                if (_bulletCfg.hitAudio != null) // 播放音效
                {
                    AudioController.GetInstance().PlaySoundByAudioClip(_bulletCfg.hitAudio,false,1);
                }
                //处理子弹附加的buff
                AttachBuff(target);
                if (_bulletCfg.isHitDestroy)
                {
                    Release();
                    break;
                }
            }
            
            // 击中目标处理完成，清理缓存
            if (_hitTargetsList.Count > 0)
            {
                _hitTargetsList.Clear();
            }
            
            //子弹碰撞体位置更新
            if (_bulletCollider != null)
            {
                if (_bulletCfg.damageCfg.colliderPosType == ColliderPosType.FollowPos)
                {
                    // 更新子弹碰撞体位置
                    if (_bulletCfg.damageCfg.damageDetectionMode == DamageDetectionMode.Box3D)
                    {
                        FixIntVector3 offset = LogicXAxis*new FixIntVector3(_bulletCfg.damageCfg.boxOffset);
                        _bulletCollider.SetBoxData(offset,new FixIntVector3(_bulletCfg.damageCfg.boxSize));
                        _bulletCollider.UpdateColliderInfo(LogicPos,new FixIntVector3(_bulletCfg.damageCfg.boxSize));
                    }
                    else if(_bulletCfg.damageCfg.damageDetectionMode == DamageDetectionMode.Sphere3D)
                    {
                        FixIntVector3 offset = LogicXAxis*new FixIntVector3(_bulletCfg.damageCfg.sphereOffset);
                        _bulletCollider.SetBoxData(_bulletCfg.damageCfg.radius,offset);
                        _bulletCollider.UpdateColliderInfo(LogicPos,FixIntVector3.zero,_bulletCfg.damageCfg.radius);
                    }
                }

                List<LogicActor> enemyList = BattleWorld.GetExitsLogicCtrl<BattleLogicCtrl>()
                    .GetEnemyList(_fireLogicActor.ObjectType);
                foreach (var target in enemyList)
                {
                    if (_bulletCfg.damageCfg.damageDetectionMode == DamageDetectionMode.Box3D)
                    {
                        _isBulletHit = PhysicsManager.IsCollision(_bulletCollider as FixIntBoxCollider, target.Collider);
                    }
                    else if (_bulletCfg.damageCfg.damageDetectionMode == DamageDetectionMode.Sphere3D)
                    {
                        _isBulletHit = PhysicsManager.IsCollision(target.Collider,_bulletCollider as FixIntSphereCollider);
                    }
                    // 收集击中的目标
                    if (_isBulletHit)
                    {
                        _hitTargetsList.Add(target);
                    }
                }
                
            }
            
            //子弹位置更新
            LogicPos += LogicDir * _bulletCfg.moveSpeed * LogicFrameConfig.LogicFrameIntervalFix;

            //如果当前运行时间大于子弹存活时间，销毁子弹
            if (_curLogicFrameAccTime >= _bulletCfg.survivalTimeMs)
            {
                Release();
            }
        }

        public void AttachBuff(LogicActor target)
        {
            if (_bulletCfg.damageCfg.addBuffs is { Length: > 0 })
            {
                foreach (var buffId in _bulletCfg.damageCfg.addBuffs)
                {
                    BuffSystem.Instance.AttachBuff(buffId,_fireLogicActor,target,_skill);
                }
            }
        }

        private void Release()
        {
            RenderObj.OnRelease();
            _bulletCollider?.OnRelease();
            IsFailure = true;
            _bulletCollider = null;
        }
    }
}