//处理逻辑对象技能

using System.Collections.Generic;
using SkillSystem.Runtime;
using ZMGC.Battle;

namespace LogicLayer
{
    public partial class LogicActor
    {
        /// <summary>
        /// 技能系统
        /// </summary>
        private SkillSystem.Runtime.SkillSystem _skillSystem;

        /// <summary>
        /// 普通攻击技能id数组
        /// </summary>
        private int[] _normalSkillIdArr;

        private int[] _skillIdArr;
        /// <summary>
        /// 普通攻击连击索引
        /// </summary>
        private int _curNormalComboIndex = 0;

        /// <summary>
        /// 正在释放技能的列表
        /// </summary>
        public List<Skill> releasingSkills = new List<Skill>();
        
        /// <summary>
        /// 初始化技能
        /// </summary>
        public void InitActorSkill()
        {
            HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
            _normalSkillIdArr = heroData.GetHeroNormalSkillId(1000);
            _skillIdArr = heroData.GetHeroSkillId(1000);
            _skillSystem = new SkillSystem.Runtime.SkillSystem(this);
            _skillSystem.InitSkills(_normalSkillIdArr);
            _skillSystem.InitSkills(_skillIdArr);
        }

        public Skill GetSkill(int skillId)
        {
            return _skillSystem.GetSkill(skillId);
        }

        public void ReleaseNormalAttack()
        {
            ReleaseSkill(_normalSkillIdArr[_curNormalComboIndex]);
        }

        public void TriggerStockPileSkill(int skillId)
        {
            _skillSystem.TriggerStockPileSkill(skillId);
        }

        /// <summary>
        /// 是否为普通攻击技能
        /// </summary>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public bool IsNormalAttackSkill(int skillId)
        {
            foreach (var item in _normalSkillIdArr)
            {
                if (skillId == item)
                {
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// 释放技能
        /// </summary>
        /// <param name="skillId"></param>
        public void ReleaseSkill(int skillId)
        {
            Skill releaseSkill = _skillSystem.ReleaseSkill(skillId, OnSkillReleaseAfter, OnSkillReleaseEnd);
            if (releaseSkill != null)
            {
                releasingSkills.Add(releaseSkill);
                if (!IsNormalAttackSkill(skillId))
                {
                    _curNormalComboIndex = 0;
                }
            }
        }

        private void OnSkillReleaseAfter(Skill skill)
        {
            // 如果不是普通攻击，连击索引直接归0
            // 如果是普通技能，则进入后摇之后，连击索引+1
            if (!IsNormalAttackSkill(skill.SkillId))
            {
                _curNormalComboIndex = 0;
            }
            else
            {
                _curNormalComboIndex++;
                if (_curNormalComboIndex >= _normalSkillIdArr.Length)
                {
                    _curNormalComboIndex = 0;
                }
            }
        }

        /// <summary>
        /// 技能释放完成
        /// </summary>
        /// <param name="skill"></param>
        private void OnSkillReleaseEnd(Skill skill)
        {
            releasingSkills.Remove(skill);
            //如果没有正在释放的技能，连击索引归0
            if (releasingSkills.Count == 0)
            {
                _curNormalComboIndex = 0;
            }
        }


        public void OnLogicFrameUpdateSkill()
        {
            _skillSystem.OnLogicFrameUpdate();
        }
    }
}