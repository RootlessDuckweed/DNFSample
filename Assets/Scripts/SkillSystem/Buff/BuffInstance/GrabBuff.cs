using FixMath;
using Game.Action;
using LogicLayer;

namespace SkillSystem.Buff.BuffInstance
{
    public class GrabBuff : BuffComposite
    {
        public GrabBuff(Buff buff) : base(buff)
        {
        }

        public override void BuffDelay()
        {
            
        }

        public override void BuffStart()
        {
            
        }

        public override void BuffTrigger()
        {
            LogicObject attachTarget = Buff.AttachTarget;
            LogicObject releaser = Buff.Releaser;
            // 抓取怪物到角色所在的位置
            attachTarget.LogicPos = releaser.LogicPos;
            // 把怪物抓取到指定的目标点
            FixIntVector3 grabPos = new FixIntVector3(Buff.BuffCfg.targetGrabData.garbMoveTargetPos);
            grabPos.x *= releaser.LogicXAxis;
            //抓取目标位置
            FixIntVector3 targetGrabPos = releaser.LogicPos + grabPos;
            MoveToAction moveToAction = new MoveToAction(attachTarget,attachTarget.LogicPos, targetGrabPos,
                Buff.BuffCfg.targetGrabData.moveTimeMs ,null,null,MoveType.Target);
            LogicActionController.Instance.RunAction(moveToAction);
        }

        public override void BuffEnd()
        {
            
        }
    }
}