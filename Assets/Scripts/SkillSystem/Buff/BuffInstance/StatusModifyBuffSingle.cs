using FixMath;
using SkillSystem.Buff.BuffCfg;

namespace SkillSystem.Buff.BuffInstance
{
    public class StatusModifyBuffSingle: BuffComposite
    {
        /// <summary>
        /// 单体角色状态修改buff
        /// </summary>
        /// <param name="buff"></param>
        public StatusModifyBuffSingle(Buff buff) : base(buff)
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
            ModifyAttribute(true);
        }

        public override void BuffEnd()
        {
            ModifyAttribute(false);
        }
        
        public void ModifyAttribute(bool value)
        {
            switch (Buff.BuffCfg.buffType)
            {
                case BuffType.AllowMove:
                    Buff.AttachTarget.IsForceAllowMove = value;
                    break;
                case BuffType.NotAllowDir:
                    Buff.AttachTarget.IsForceAllowModifyDir = value;
                    break;
            }
        }
    }
}
    
        
  