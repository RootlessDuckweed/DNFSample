using System;
using System.Collections.Generic;
using LogicLayer;
using Unity.VisualScripting;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public class SkillSystem
    {
        private LogicActor _skillCreator;
        private List<Skill> _skillArr = new List<Skill>();
        private Skill _curReleasingSkill;
        private List<int> _combinationSkillIdList = new List<int>();

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
            // 技能在前摇状态下，是无法释放其他技能的
            if (_curReleasingSkill != null && _curReleasingSkill.SkillId != skillID)
            {
                return;
            }
            // 如果这个技能有组合技能 ，是无法释放其他技能的
            // 判断当前是否有组合技能正在释放中，如果有，是不允许其他技能释放的
            if (_combinationSkillIdList.Count > 0 && !_combinationSkillIdList.Contains(skillID))
            {
                return;
            }
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
            // 技能在前摇状态下，是无法释放其他技能的
            if (_curReleasingSkill != null && _curReleasingSkill.State == SkillState.Before)
            {
                return null;
            }

            // 判断当前是否有组合技能正在释放中，如果有，是不允许其他技能释放的
            if (_combinationSkillIdList.Count > 0 && !_combinationSkillIdList.Contains(skillId))
            {
                return null;
            }
            foreach (var skill in _skillArr)
            {
                if (skill.SkillId == skillId)
                {
                    // 如果技能还在释放中 就不能再次释放 
                    if(skill.State!=SkillState.None && skill.State != SkillState.End) return null;
                    // 释放技能
                    if (skill.SkillCfg.comboBinationSkillID != 0)
                    {
                        CalculateCombinationSkillIdList(skillId);
                    }
                    skill.ReleaseSkill(releaseAfterCallback, (ski, comboSkill) =>
                    {
                        // 释放完成技能的回调
                        releaseEndCallback?.Invoke(ski);
                        if (!comboSkill)
                        {
                            _curReleasingSkill = null;
                            if (ski.SkillCfg.comboBinationSkillID == 0 && _combinationSkillIdList.Count>0)
                            {
                                _combinationSkillIdList.Clear();
                            }
                        }
                        // 如果是组合技能，处理技能组逻辑
                    });
                    _curReleasingSkill = skill;
                    return skill;
                }
            }
            Debug.LogError($"Skill {skillId} not found");
            return null;
        }

        public void CalculateCombinationSkillIdList(int skillId)
        {
            if (skillId != 0)
            {
                int combinationSkillId = skillId;
                while (combinationSkillId != 0)
                {
                    _combinationSkillIdList.Add(combinationSkillId);
                    combinationSkillId = GetSkill(combinationSkillId).SkillCfg.comboBinationSkillID;
                }
            }
            else
            {
                Debug.Log("无效的技能组合ID");
            }
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