using System;
using System.Collections.Generic;
using LogicLayer;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillSystem
    {
        private LogicActor _skillCreator;
        private List<Skill> _skillArr = new List<Skill>();

        public SkillSystem(LogicActor skillCreator)
        {
            _skillCreator = skillCreator;
        }

        public Skill GetSkill(int skillId)
        {
            foreach (var skill in _skillArr)
            {
                if (skill.SkillId == skillId)
                {
                    return skill;
                }
            }
            return null;
        }

        public void TriggerStockPileSkill(int skillID)
        {
            Skill skill = GetSkill(skillID);
            if (skill != null)
            {
                skill.TriggerStockPileSkill();
            }
        }

        public void InitSkills(int[] skillIdArr)
        {
            foreach (var skillId in skillIdArr)
            {
                Skill skill = new Skill(skillId, _skillCreator);
                _skillArr.Add(skill);
                // 初始化相关联的组合技能
                if (skill.SkillCfg.comboBinationSkillID != 0)
                {
                    InitSkills(new []{skill.SkillCfg.comboBinationSkillID});
                }
                // 初始化相关联的蓄力阶段技能
                if (skill.SkillCfg.stockPileStageData.Count > 0)
                {
                    foreach (var item in skill.SkillCfg.stockPileStageData)
                    {
                        InitSkills(new []{item.skillID});
                    }
                }
            }
        }

        public Skill ReleaseSkill(int skillId, Action<Skill> releaseAfterCallback, Action<Skill> releaseEndCallback)
        {
            foreach (var skill in _skillArr)
            {
                if (skill.SkillId == skillId)
                {
                    // 如果技能还在释放中 就不能再次释放 
                    if(skill.State!=SkillState.None && skill.State != SkillState.End) return null;
                    // 释放技能
                    skill.ReleaseSkill(releaseAfterCallback, (ski, comboSkill) =>
                    {
                        // 释放完成技能的回调
                        releaseEndCallback?.Invoke(ski);
                        // 如果是组合技能，处理技能组逻辑
                        if (comboSkill)
                        {
                        }
                    });
                    return skill;
                }
            }
            Debug.LogError($"Skill {skillId} not found");
            return null;
        }

        public void OnLogicFrameUpdate()
        {
            foreach (var skill in _skillArr)
            {
                skill.OnLogicFrameUpdate();
            }
        }
    }
}