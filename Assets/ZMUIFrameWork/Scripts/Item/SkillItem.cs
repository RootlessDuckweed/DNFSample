using System;
using FixMath;
using Game.Timer;
using LogicLayer;
using RenderLayer;
using SkillSystem.Config;
using SkillSystem.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace ZMUIFrameWork.Scripts.Item
{
    public class SkillItem : MonoBehaviour
    {
        public Text cdText;
        public Image iconImage;
        public Image cdMaskImage;
        public SKillItem_JoyStick skillJoyStick;

        private Skill _skillData;
        private LogicActor _skillCreator;
        //是否进入技能CD
        private bool _isEnterSkillCd;
        //已经冷却的时间
        private float _alreadyCdTime;
        //技能冷却时间
        private float _skillCdTime;
        private HeroRender _heroRender;
        /// <summary>
        /// 设置技能设置
        /// </summary>
        /// <param name="skillData">技能数据</param>
        /// <param name="logicActor">释放者</param>
        public void SetItemSkillData(Skill skillData,LogicActor logicActor)
        {
            if (skillData == null)
            {
                Debug.LogError("有传入的技能为空，可能是技能系统没有初始化或者不存在这个技能。");
                return;
            }
            _skillData = skillData;
            _skillCreator = logicActor;
            _heroRender = logicActor.RenderObj as HeroRender;
            skillJoyStick.InitSkillData(GetSkillGuideType(skillData.SkillCfg.skillType),skillData.SkillId,skillData.SkillCfg.skillGuideRange);
            skillJoyStick.OnReleaseSkill += OnTriggerSkill;
            skillJoyStick.OnSkillGuide += OnUpdateSkillGuide;
            iconImage.sprite = skillData.SkillCfg.skillIcon;
            cdText.gameObject.SetActive(false);
            cdMaskImage.gameObject.SetActive(false);
        }

        /// <summary>
        /// 更新技能引导回调
        /// </summary>
        /// <param name="sKillGuide">技能引导类型</param>
        /// <param name="isCancel">是否取消</param>
        /// <param name="skillPos">技能释放位置</param>
        /// <param name="skillId">技能ID</param>
        /// <param name="skillDirDis">技能半径距离</param>
        private void OnUpdateSkillGuide(SKillGuideType sKillGuide, bool isCancel, Vector3 skillPos, int skillId, float skillDirDis)
        {
            if (sKillGuide == SKillGuideType.LongPress)
            {
                if (_isEnterSkillCd) return;
                _skillCreator.ReleaseSkill(skillId,releaseSkillCallback:OnReleaseSkill);
            }
            else if (sKillGuide == SKillGuideType.Position)
            {
                _heroRender.UpdateSkillGuide(sKillGuide,skillId,isCancel,skillPos,skillDirDis);
            }
        }

        /// <summary>
        /// 触发对应的技能 (释放摇杆时触发)
        /// </summary>
        /// <param name="sKillGuide">技能引导类型</param>
        /// <param name="skillPos">触发位置</param>
        /// <param name="skillId">技能触发ID</param>
        private void OnTriggerSkill(SKillGuideType sKillGuide, Vector3 skillPos, int skillId)
        {
            if (_isEnterSkillCd) return;
            if (sKillGuide == SKillGuideType.Click)
            {
                _skillCreator.ReleaseSkill(skillId,releaseSkillCallback:OnReleaseSkill);
            }
            else if (sKillGuide == SKillGuideType.LongPress)
            {
                _skillCreator.TriggerStockPileSkill(skillId);
            }
            else if (sKillGuide == SKillGuideType.Position)
            {
                // TODO:指定位置技能
                skillPos.y = 0;
                _skillCreator.ReleaseSkill(skillId,_skillCreator.LogicPos + new FixIntVector3(skillPos),releaseSkillCallback:OnReleaseSkill);
                _heroRender.OnGuideRelease();
            }
        }

        /// <summary>
        /// 技能释放回调
        /// </summary>
        /// <param name="isReleaseSuccess"></param>
        public void OnReleaseSkill(bool isReleaseSuccess)
        {
            if (isReleaseSuccess)
            {
                EnterSkillCd();
            }
        }

        /// <summary>
        /// 进入技能CD
        /// </summary>
        public void EnterSkillCd()
        {
            cdText.gameObject.SetActive(true);
            cdMaskImage.gameObject.SetActive(true);
            _isEnterSkillCd = true;
            _skillCdTime = _alreadyCdTime = _skillData.SkillCfg.skillCDTimeMS * 1.0f / 1000;
            cdText.text = _skillCdTime.ToString();
            int cdTime = _skillData.SkillCfg.skillCDTimeMS / 1000;
            LogicTimerManager.Instance.DelayCall(1, () =>
            {
                cdTime--;
                if (cdTime <= 0)
                {
                    cdText.gameObject.SetActive(false);
                    cdMaskImage.gameObject.SetActive(false);
                    _isEnterSkillCd = false;
                }
                else
                {
                    cdText.text = cdTime.ToString();
                }
            },cdTime);
        }

        public SKillGuideType GetSkillGuideType(SkillType type)
        {
            SKillGuideType skillGuideType = SKillGuideType.Click;
            if (type == SkillType.StockPile)
            {
                skillGuideType = SKillGuideType.LongPress;
            }
            else if (type == SkillType.Ballistic || type == SkillType.Chant || type == SkillType.None)
            {
                skillGuideType = SKillGuideType.Click;
            }
            else if (type == SkillType.PosGuide)
            {
                skillGuideType = SKillGuideType.Position;
            }
            return skillGuideType;
        }

        private void Update()
        {
            if (_isEnterSkillCd)
            {
                cdMaskImage.fillAmount = (_alreadyCdTime -= Time.deltaTime) /_skillCdTime;
            }
        }

        private void OnDestroy()
        {
            skillJoyStick.OnReleaseSkill -= OnTriggerSkill;
            skillJoyStick.OnSkillGuide -= OnUpdateSkillGuide;
        }
    }
}