//处理逻辑对象技能

using System;
using System.Collections.Generic;
using SkillSystem.Buff.BuffInstance;
using SkillSystem.Config;
using SkillSystem.Runtime;
using Tools;
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
        /// 显示正在生效的buff,可以用来显示buff状态栏等,仅用于记录没有逻辑运算，逻辑运算在BuffSystem中
        /// </summary>
        private List<Buff> _buffList = new List<Buff>();
        
        /// <summary>
        /// 初始化技能
        /// </summary>
        public void InitActorSkill()
        {
            HeroDataMgr heroData = BattleWorld.GetExitsDataMgr<HeroDataMgr>();
            _normalSkillIdArr = heroData.GetHeroNormalSkillId(1001);
            _skillIdArr = heroData.GetHeroSkillId(1001);
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
        public void ReleaseSkill(int skillId,Action<bool> releaseSkillCallback = null)
        {
            Skill releaseSkill = _skillSystem.ReleaseSkill(skillId, OnSkillReleaseAfter, (releaseFinishedSkill) =>
            {
                if (releaseFinishedSkill.SkillCfg.skillType == SkillType.StockPile)
                {
                    releaseSkillCallback?.Invoke(true);
                }
                OnSkillReleaseEnd(releaseFinishedSkill);
            });
            if (releaseSkill != null)
            {
                releasingSkills.Add(releaseSkill);
                if (!IsNormalAttackSkill(skillId))
                {
                    _curNormalComboIndex = 0;
                }
                ActionState = LogicObjectActionState.SkillReleasing;
                if (releaseSkill.SkillCfg.skillType != SkillType.StockPile)
                {
                    releaseSkillCallback?.Invoke(true);
                }
                else
                {
                    releaseSkillCallback?.Invoke(false);
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
                if (_curNormalComboIndex >= _normalSkillIdArr.Length || skill.SkillId == _normalSkillIdArr[^1])
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
            ActionState = LogicObjectActionState.Idle;
        }


        public void OnLogicFrameUpdateSkill()
        {
            _skillSystem.OnLogicFrameUpdate();
        }

        /// <summary>
        /// 添加一个buff到记录
        /// </summary>
        /// <param name="buff"></param>
        public void AddBuff(Buff buff)
        {
            _buffList.Add(buff);
        }

        /// <summary>
        /// 删除buff记录
        /// </summary>
        /// <param name="buff"></param>
        public void RemoveBuff(Buff buff)
        {
            if(_buffList.Contains(buff))
                _buffList.Remove(buff);
            if (ObjectState == LogicObjectState.Death)
            {
                return;
            }

            // 没有buff，并且不是在起身的状态，则恢复idle动画
            if (_buffList.Count == 0 && RenderObj.GetCurAnimName() != AnimationName.Anim_Getup)
            {
                PlayAnim(AnimationName.Anim_Idle);
            }
        }
    }
}