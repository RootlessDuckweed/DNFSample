using FixMath;
using Game.Action;

namespace SkillSystem.Buff.BuffInstance
{
    public class RepelBuff : BuffComposite
    {
        public RepelBuff(Buff buff) : base(buff)
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
            if (Buff.BuffCfg.buffParamsList.Count > 0)
            {
                FixInt repelValue = Buff.BuffCfg.buffParamsList[0].value;
                FixInt releaserXAxis = Buff.Releaser.LogicXAxis;
                FixIntVector3 endPos =
                    new FixIntVector3(
                        Buff.AttachTarget.LogicPos.x + releaserXAxis * repelValue, 
                        Buff.AttachTarget.LogicPos.y,
                        Buff.AttachTarget.LogicPos.z);
                MoveToAction moveTo = new MoveToAction(Buff.AttachTarget, Buff.AttachTarget.LogicPos, endPos,
                    Buff.BuffCfg.buffDurationMs, null, null, MoveType.X);
                LogicActionController.Instance.RunAction(moveTo);
            }
        }

        public override void BuffEnd()
        {
        }
    }
}