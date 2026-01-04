using Config;
using FixIntPhysics;
using FixMath;
using Game.Timer;
using RenderLayer;
using SkillSystem.Config;
using SkillSystem.Runtime;
using Tools;
using UnityEngine;
using ZMGC.Battle;

namespace LogicLayer.Monster
{
    public class MonsterLogic : LogicActor
    {
        public int MonsterId { get; private set; }
        private FixInt _attackRange = 1;
        private FixInt _chaseDistance = 4;
        private LogicActor _chaseTarget;
        public MonsterLogic(int monsterID,RenderObject renderObj,FixIntBoxCollider boxCollider,FixIntVector3 initPos)
        {
            MonsterId = monsterID;
            RenderObj = renderObj;
            Collider = boxCollider;
            LogicPos = initPos;
            ObjectType = LogicObjectType.Monster;
            _chaseTarget = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLgc;
            LogicMoveSpeed = 1;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            InitMonsterAttribute();
        }

        public override void OnLogicFrameUpdate()
        {
            base.OnLogicFrameUpdate();
            UpdateAIMove();
        }

        public void UpdateAIMove()
        {
            if(ObjectState == LogicObjectState.Death) return;
            FixIntVector3 targetPos = _chaseTarget.LogicPos;
            FixIntVector3 directionToPlayer = (targetPos - LogicPos).normalized;
            FixInt dis = FixIntVector3.Distance(LogicPos, targetPos);
            if (dis <= _attackRange)
            {
                // 可以进行攻击
                if (ActionState == LogicObjectActionState.Idle)
                {
                    PlayAnim(AnimationName.Anim_Gongji_01);
                }
            }
            else if (dis <= _chaseDistance)
            {
                if (ActionState is LogicObjectActionState.Idle or LogicObjectActionState.Move)
                {
                    LogicPos += directionToPlayer * LogicMoveSpeed * LogicFrameConfig.LogicFrameIntervalFix;
                    LogicXAxis = directionToPlayer.x;
                    PlayAnim(AnimationName.Anim_Walk);
                }
            }
            else
            {
                if (ActionState == LogicObjectActionState.Idle)
                {
                    PlayAnim(AnimationName.Anim_Idle);
                }
            }

        }

        private void InitMonsterAttribute()
        {
            MonsterCfg data = ConfigCenter.Instance.GetMonsterCfgById(MonsterId);
            if (data == null)
            {
                Debug.LogError("MonsterId not found or MonsterCfg not found");
                return;
            }
            hp = data.hp;
            mp = data.mp;
            ap = data.ap;
            ad = data.ad;
            adDef = data.adDef;
            apDef = data.apDef;
            pct = data.pct;
            mct = data.mct;
            adPctRate = data.adPctRate;
            apMctRate = data.apMctRate;
            str = data.str;
            sta = data.sta;
            Int = data.Int;
            spi = data.spi;
            agl = data.agl;
            
            atkRange = data.atkRange;
            searchDisRange = data.searchDisRange;
        } 
        

        public override void CalculateDamage(FixInt damage, DamageSource damageSource, LogicObject source)
        {
            base.CalculateDamage(damage, damageSource, source);
            // 怪物受到伤害的时候 会触发转向伤害源的逻辑
            if (source.ObjectType == LogicObjectType.Hero)
            {
                LogicXAxis = source.LogicPos.x > LogicPos.x ? 1 : -1;
            }
            else
            {
                LogicXAxis = - source.LogicXAxis;
            }
        }


        public override void Floating(bool isUpFloating)
        {
            base.Floating(isUpFloating);
            string clipName = isUpFloating ? AnimationName.Anim_Float_up :  AnimationName.Anim_Float_down;
            PlayAnim(clipName);
            ActionState = LogicObjectActionState.Floating;
        }

        public override void TriggerGround()
        {
            base.TriggerGround();
            //处理怪物落地逻辑
            if (ObjectState != LogicObjectState.Death)
            {
                PlayAnim(AnimationName.Anim_Getup);
                
                LogicTimerManager.Instance.DelayCall(new FixInt(500L), () =>
                {
                    PlayAnim(AnimationName.Anim_Idle);
                    ActionState = LogicObjectActionState.Idle;
                });
            }
            else
            {
                PlayAnim(AnimationName.Anim_Dead);
            }
        }
    }
}