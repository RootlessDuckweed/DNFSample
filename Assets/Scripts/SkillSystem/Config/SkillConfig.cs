using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillConfig
    {
        [LabelText("技能图标"),SuffixLabel("技能图标")]
        [LabelWidth(0.1f)]
        [PreviewField(70,ObjectFieldAlignment.Left)]
        public Sprite skillIcon;

        [LabelText("技能ID")]
        public int skillId;
        
        [LabelText("技能名称")]
        public string skillName;
        
        [LabelText("技能所需蓝量")]
        public int needMagicValue;

        [LabelText("技能前摇时间MS")]
        public int skillShakeBeforeTimeMS;

        [LabelText("技能攻击持续时间MS")]
        public int skillAttackDurationMS;
        
        [LabelText("技能后摇时间MS")]
        public int skillShakeAfterTimeMS;

        [LabelText("技能CD时间MS")]
        public int skillCDTimeMS;
        
        [LabelText("技能类型")]
        [OnValueChanged("OnSkillTypeChanged")]
        public SkillType skillType;
        
        [LabelText("蓄力阶段配置(若第一阶段触发时间不为0，则空档时间为动画表现时间)")]
        [ShowIf("_showStockPileData")]
        public List<StockPileStageData> stockPileStageData;

        [LabelText("技能引导特效")]
        [ShowIf("_showPosGuideData")]
        public GameObject skillGuideObj;
        
        [LabelText("技能引导范围")]
        [ShowIf("_showPosGuideData")]
        public float skillGuideRange;
        
        [LabelText("组合技能Id(衔接下一个技能的ID)"),Tooltip("比如:技能A由 C B D技能组合而成")]
        public int comboBinationSkillID;
        
        // 技能渲染相关
        [TitleGroup("技能渲染","技能释放时产生的特效音效等相关")]
        [LabelText("技能命中特效"),OnValueChanged("GetObjectPath")]
        public GameObject skillHitEffect;

        [ReadOnly] public string skillHitEffectPath;
        
        [TitleGroup("技能渲染")]
        [LabelText("技能命中特效存活时间")]
        public int hitEffectSurvivalTimeMs = 100;
        
        [TitleGroup("技能渲染")]
        [LabelText("技能命中音效")]
        public AudioClip skillHitAudio;
        
        [TitleGroup("技能渲染")]
        [LabelText("是否显示技能立绘")]
        public bool showSkillPortrait;
        
        [TitleGroup("技能渲染")]
        [LabelText("技能立绘显示对象"),ShowIf("showSkillPortrait")]
        public GameObject skillPortraitObj;
        
        [TitleGroup("技能渲染")]
        [LabelText("技能描述")]
        [TextArea(5,5)]
        public string skillDes;
        
#region 控制编辑面板相关操作
        
        // 控制变量
        private bool _showStockPileData;
        private bool _showPosGuideData;
        
        // 控制方法
        public void OnSkillTypeChanged(SkillType type)
        {
           _showStockPileData = type == SkillType.StockPile;
           _showPosGuideData = type == SkillType.PosGuide;
        }
        
        #if UNITY_EDITOR        
        public void GetObjectPath(GameObject obj)
        {
            skillHitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        }
        #endif
        
#endregion
    }

    public enum SkillType
    {
        [LabelText("无配置(默认瞬发型技能)")]
        None,
        [LabelText("吟唱型技能")]
        Chant,
        [LabelText("弹道型技能")]
        Ballistic,
        [LabelText("蓄力型技能")]
        StockPile,
        [LabelText("位置引导型技能")]
        PosGuide,
    }

    [System.Serializable]
    public sealed class StockPileStageData
    {
        [LabelText("蓄力阶段ID")]
        public int stage;

        [LabelText("当前蓄力阶段触发的技能ID")]
        public int skillID;

        [LabelText("当前阶段触发时间")]
        public int startTimeMS;
        
        [LabelText("当前阶段结束时间")]
        public int endTimeMS;
    }
}