using FixIntPhysics;
using FixMath;
using Game.Timer;
using RenderLayer;
using Tools;
using UnityEngine;

namespace LogicLayer.Monster
{
    public class MonsterLogic : LogicActor
    {
        public int MonsterId { get; private set; }
        public MonsterLogic(int monsterID,RenderObject renderObj,FixIntBoxCollider boxCollider,FixIntVector3 initPos)
        {
            MonsterId = monsterID;
            RenderObj = renderObj;
            Collider = boxCollider;
            LogicPos = initPos;
            ObjectType = LogicObjectType.Monster;
        }

        public override void OnHit(GameObject effect, int survivalTimeMs, LogicObject source)
        {
            base.OnHit(effect, survivalTimeMs, source);
            LogicXAxis = - source.LogicXAxis;
        }

        public override void OnHitByBullet(GameObject effect, int survivalTimeMs, LogicObject source)
        {
            base.OnHitByBullet(effect, survivalTimeMs, source);
            LogicXAxis = - source.LogicXAxis;
            Debug.Log("onhit");
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
                    PlayAnim( AnimationName.Anim_Idle);
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