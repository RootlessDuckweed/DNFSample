using FixMath;
using Game;
using LogicLayer;
using SkillSystem.Buff.BuffCfg;

namespace SkillSystem.Buff.BuffInstance
{
    public class AttributeModifyBuffGroup: BuffComposite
    {
        private BuffCollider _buffCollider;
        private FixInt _configValue;
        
        public AttributeModifyBuffGroup(Buff buff) : base(buff)
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
            
            if (Buff.BuffCfg.targetConfig.isOpen)
            {
                _buffCollider = new BuffCollider(Buff);
                _buffCollider.CreateOrUpdateCollider();
            }
        }

        public override void BuffTrigger()
        {
            if (Buff.BuffCfg.targetConfig.isOpen)
            {
                var targetList = _buffCollider.CalculateTargetObjects();
                for (int i = 0; i < targetList.Count; i++)
                {
                    LogicActor target = targetList[i];
                    if (target.ObjectState != LogicObjectState.Death)
                    {
                        target.BuffDamage(DamageCalculateCenter.CalculateDamage(Buff.BuffCfg,Buff.Releaser,Buff.AttachTarget),Buff.BuffCfg.targetConfig.damageCfg,Buff.Releaser);
                        target.OnHit(Buff.BuffCfg.buffHitEffectPath,1000,Buff.Releaser,target);
                        // 处理这个伤害配置附加的后续buff
                        int[] buffIdArr = Buff.BuffCfg.targetConfig.damageCfg.addBuffs;
                        if (buffIdArr is { Length: > 0 })
                        {
                            for (int k = 0; k < buffIdArr.Length; k++)
                            {
                                BuffSystem.Instance.AttachBuff(buffIdArr[k],Buff.Releaser,target,Buff.Skill);
                            }
                        }
                    }
                }
                targetList.Clear();
            }
        }

        public override void BuffEnd()
        {
            _buffCollider?.OnRelease();
            _buffCollider = null;
        }
    }
}