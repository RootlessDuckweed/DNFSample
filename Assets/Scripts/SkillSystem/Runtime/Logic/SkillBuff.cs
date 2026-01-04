using SkillSystem.Buff;

namespace SkillSystem.Runtime
{
    public partial class Skill
    {
        public void OnLogicFrameUpdateBuff()
        {
            if (_skillDataConfig.buffList is { Count: > 0 })
            {
                foreach (var buffCfg in _skillDataConfig.buffList)
                {
                    if (buffCfg.triggerFrame == _curLogicFrame)
                    {
                        BuffSystem.Instance.AttachBuff(buffCfg.buffId, _skillCreator, 
                            _skillCreator, this);
                    }
                }
            }
        } 
    }
}