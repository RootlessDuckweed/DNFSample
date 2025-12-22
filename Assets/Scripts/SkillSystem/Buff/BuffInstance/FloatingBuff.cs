using FixMath;

namespace SkillSystem.Buff.BuffInstance
{
    public class FloatingBuff : BuffComposite
    {
        public FloatingBuff(Buff buff) : base(buff)
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
            if (Buff.BuffCfg.buffParamsList is { Count : > 0 })
            {
                FixInt floatingValue = Buff.BuffCfg.buffParamsList[0].value;
                Buff.AttachTarget.AddRisingForce(floatingValue,Buff.BuffCfg.buffDurationMs);
            }
            
        }

        public override void BuffEnd()
        {
        }
    }
}