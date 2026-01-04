namespace SkillSystem.Buff.BuffInstance
{
    public class IgnoreGravityBuff :BuffComposite
    {
        public IgnoreGravityBuff(Buff buff) : base(buff)
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
            Buff.AttachTarget.IsIgnoreGravity = true;
        }

        public override void BuffEnd()
        {
            Buff.AttachTarget.IsIgnoreGravity = false;
        }
    }
}