using System;
using System.Collections.Generic;
using FixMath;
using LogicLayer;
using SkillSystem.Config;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Runtime
{
    public enum SkillState
    {
        None,
        Before,
        After,
        End
    }

    public partial class Skill
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public int SkillId;

        /// <summary>
        /// 技能创建者
        /// </summary>
        private LogicActor _skillCreator;

        /// <summary>
        /// 技能数据
        /// </summary>
        private SkillDataConfig _skillDataConfig;

        public SkillConfig SkillCfg => _skillDataConfig.skillCfg;

        /// <summary>
        /// 释放技能后摇
        /// </summary>
        public Action<Skill> OnReleaseSkillAfter;

        /// <summary>
        /// 释放技能结束回调
        /// </summary>
        public Action<Skill, bool> OnReleaseSkillEnd;

        public SkillState State { get; protected set; } = SkillState.None;

        public List<SkillDamageConfig> DamageCfgList => _skillDataConfig.damageCfgList; 
        
        private int _curLogicFrame = 0;
        private int _curLogicFrameAccTime = 0;
        /// <summary>
        /// 是否自动匹配蓄力阶段
        /// </summary>
        private bool _autoMatchStockStage;
        /// <summary>
        /// 组合技能ID
        /// </summary>
        private int _comboBinationSkillId = 0;

        public FixIntVector3 SkillGuidePos;

        public Skill(int skillId, LogicActor skillCreator)
        {
            this.SkillId = skillId;
            _skillCreator = skillCreator;
            _skillDataConfig =
                ZMAssetsFrame.LoadScriptableObject<SkillDataConfig>(AssetPathConfig.SKILL_DATA_CONFIG + skillId +
                                                                    ".asset");
        }

        /// <summary>
        /// 释放技能
        /// </summary>
        public void ReleaseSkill(Action<Skill> releaseSkillAfter, FixIntVector3 guidePos,Action<Skill, bool> releaseSkillEnd)
        {
            OnReleaseSkillAfter = releaseSkillAfter;
            OnReleaseSkillEnd = releaseSkillEnd;
            SkillGuidePos = guidePos;
            SkillStart();
            State = SkillState.Before;
            PlayAnim();
        }

        public void PlayAnim()
        {
            // 播放动画
            _skillCreator.PlayAnim(_skillDataConfig.character.skillAnimClip);
        }

        /// <summary>
        /// 技能前摇
        /// </summary>
        public void SkillStart()
        {
            // 开始释放技能时，初始化技能数据
            _curLogicFrame = 0;
            _curLogicFrameAccTime = 0;
            _autoMatchStockStage = false;
            _comboBinationSkillId = _skillDataConfig.skillCfg.comboBinationSkillID;
            if (_skillDataConfig.character.isSetCustomLogicFrame)
            {
                _skillDataConfig.character.logicFrame = _skillDataConfig.character.customLogicFrame;
            }
            OnBulletInit();
            OnDamageInit();
        }

        /// <summary>
        /// 技能后摇
        /// </summary>
        public void SkillAfter()
        {
            State = SkillState.After;
            OnReleaseSkillAfter?.Invoke(this);
        }

        public void SkillEnd()
        {
            State = SkillState.End;
            OnReleaseSkillEnd?.Invoke(this, false);
            ReleaseAllEffect();
            OnBulletRelease();
            OnDamageRelease();
            if (_comboBinationSkillId != 0)
            {
                _skillCreator.ReleaseSkill(_comboBinationSkillId);
            }
            
        }
        

        public void OnLogicFrameUpdate()
        {
            if (State == SkillState.None || State == SkillState.End)
            {
                return;
            }

            _curLogicFrameAccTime = _curLogicFrame * LogicFrameConfig.LogicFrameIntervalMs;

            if (State == SkillState.Before && _curLogicFrameAccTime >= _skillDataConfig.skillCfg.skillShakeAfterTimeMS && _skillDataConfig.skillCfg.skillType!=SkillType.StockPile)
            {
                SkillAfter();
            }

            //更新不同配置的逻辑帧，处理不同配置的逻辑
            // 伤害逻辑帧
            OnLogicFrameUpdateDamage();
            // 特效逻辑帧
            OnLogicFrameUpdateEffect();
            // 音效逻辑帧
            OnLogicFrameUpdateAudio();
            // 技能行动逻辑帧
            OnLogicFrameUpdateAction();
            // 子弹射击逻辑帧
            OnLogicFrameUpdateBullet();
            // 附加给自己的buff逻辑帧
            OnLogicFrameUpdateBuff();
            
            // 蓄力技能通过蓄力时间进行触发，与结束帧无关
            if (_skillDataConfig.skillCfg.skillType == SkillType.StockPile)
            {
                int stockDataCount = _skillDataConfig.skillCfg.stockPileStageData.Count;
                if (stockDataCount > 0)
                {
                    // 处理手指按下立马抬起的情况
                    if (_autoMatchStockStage)
                    {
                        // 自动匹配第一阶段蓄力技能
                        StockPileStageData stageData = _skillDataConfig.skillCfg.stockPileStageData[0];
                        if (_curLogicFrameAccTime >= stageData.startTimeMS)
                        {
                            StockPileFinish(stageData);
                        }
                    }
                    else
                    {
                        // 处理超时蓄力逻辑
                        StockPileStageData stageData = _skillDataConfig.skillCfg.stockPileStageData[stockDataCount - 1];
                        if (_curLogicFrameAccTime >= stageData.endTimeMS)
                        {
                            StockPileFinish(stageData);
                        }
                    }
                }
            }
            else
            {
                if (_curLogicFrame == _skillDataConfig.character.logicFrame)
                {
                    SkillEnd();
                }
            }

            if (_skillDataConfig.skillCfg.showSkillPortrait && _curLogicFrame == 0)
            {
                _skillCreator.RenderObj.ShowSkillPortrait(_skillDataConfig.skillCfg.skillPortraitObj);
            }
            
            _curLogicFrame++;
        }
        

        public void StockPileFinish(StockPileStageData stageData)
        {
            SkillEnd();
            State = SkillState.End;
            if (stageData.skillID == 0)
            {
                Debug.LogError("蓄力技能配置的蓄力阶段技能id为0");
            }
            else
            {
                _skillCreator.ReleaseSkill(stageData.skillID);
            }
        }

        public void TriggerStockPileSkill()
        {
            if (_skillDataConfig.skillCfg.skillType != SkillType.StockPile)
            {
                Debug.LogError("该技能不是蓄力技能："+_skillDataConfig.skillCfg.skillId);
            };
            foreach (var item in _skillDataConfig.skillCfg.stockPileStageData)
            {
                if (_curLogicFrameAccTime >= item.startTimeMS && _curLogicFrameAccTime <= item.endTimeMS)
                {
                    StockPileFinish(item);
                    return;
                }
            }
            _autoMatchStockStage = true;
        }
    }
}