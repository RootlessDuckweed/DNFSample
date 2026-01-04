using System;
using FixMath;
using SkillSystem.Buff.BuffCfg;

namespace SkillSystem.Buff.BuffInstance
{
    /// <summary>
    /// 单体属性修改buff
    /// </summary>
    public class AttributeModifyBuffSingle : BuffComposite
    {
        private FixInt _configValue;

        public AttributeModifyBuffSingle(Buff buff) : base(buff)
        {
        }

        public override void BuffDelay()
        {
            
        }

        public override void BuffStart()
        {
            //获取配置参数
            if (Buff.BuffCfg.buffParamsList.Count > 0)
            {
                _configValue = Buff.BuffCfg.buffParamsList[0].value;
            }
        }

        public override void BuffTrigger()
        {
            ModifyAttribute(_configValue);
        }

        public override void BuffEnd()
        {
            ModifyAttribute( - _configValue);
        }
        
        public void ModifyAttribute(FixInt value)
        {
            switch (Buff.BuffCfg.buffType)
            {
                case BuffType.MoveSpeedModifySingle:
                    Buff.AttachTarget.LogicMoveSpeed += value;
                    break;
            }
        }
    }
}