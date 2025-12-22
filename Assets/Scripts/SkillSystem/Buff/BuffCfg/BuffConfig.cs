using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using SkillSystem.Config;
using UnityEngine;

namespace SkillSystem.Buff.BuffCfg
{
    [CreateAssetMenu(fileName = "buff配置",menuName = "buff配置",order = 0)]
    [Serializable]
    public class BuffConfig: ScriptableObject
    {
        [LabelText("buff图标"),LabelWidth(0.1f),PreviewField(70,ObjectFieldAlignment.Left),SuffixLabel("buff图标")]
        public Sprite buffIcon;
        [LabelText("buffID")]
        public int buffId;
        [LabelText("buff名称")]
        public string buffName;
        [LabelText("延迟触发时间")]
        public int buffDelay;
        [LabelText("触发间隔")]
        public int buffIntervalMs;
        [LabelText("buff持续时间(0表示一次，-1表示时间无限直到战斗结束)")]
        public int buffDurationMs;
        [LabelText("buff类型")]
        public BuffTypeEnum buffType;
        [LabelText("附加目标")]
        public BuffAttachType attachType;
        [LabelText("附加位置")]
        public BuffPosType buffPosType;
        [LabelText("伤害类型")]
        public DamageType damageType;
        [LabelText("伤害倍率")]
        public int damageRate;
        [LabelText("buff数值配置")]
        public List<BuffParams> buffParamsList;
        [LabelText("抓取数据")]
        public TargetGrabData targetGrabData;
        
        [LabelText("buff触发音效"),TitleGroup("buff表现","所有的表现数据会在buff触发释放时触发")]
        public AudioClip buffAudio;
        [LabelText("buff触发特效"),TitleGroup("buff表现","所有的表现数据会在buff触发释放时触发")]
        public BuffEffectConfig effectConfig;
        [LabelText("buff命中特效"),TitleGroup("buff表现","所有的表现数据会在buff触发释放时触发")] 
        public GameObject buffHitEffectObj;
        [LabelText("buff触发动画"),TitleGroup("buff表现","所有的表现数据会在buff触发释放时触发")] 
        public ObjectAnimationState buffTriggerAnim = ObjectAnimationState.None;
        [LabelText("buff描述"),HideLabel,MultiLineProperty(5)]  
        public string buffDes;
    }

    /// <summary>
    /// 表示当前buff触发时所需要播放的动画
    /// </summary>
    public enum ObjectAnimationState
    {
        [LabelText("无配置")]None,
        [LabelText("受击动画")]BeHit,
        [LabelText("僵直动画")]Stiff,
    }

    [Serializable]
    public class BuffEffectConfig
    {
        [LabelText("特效对象")] public GameObject effect;
        [LabelText("特效附加类型")] public EffectAttachType effectAttachType;
        [LabelText("特效位置类型")] public BuffEffectPosType buffEffectPosType;
    }

    [LabelText("特效位置类型")]
    public enum BuffEffectPosType
    {
        [LabelText("无配置")] None,
        [LabelText("跟随目标")] TargetFollow,
        [LabelText("目标位置")] TargetPos,
    }

    [LabelText("特效附着类型")]
    public enum EffectAttachType
    {
        [LabelText("无配置")] None,
        [LabelText("中心")]Center,
        [LabelText("手部")]Hand,
    }

    [Serializable]
    public class TargetGrabData
    {
        [LabelText("抓取到的目标位置")] public Vector3 garbMoveTargetPos;
        [LabelText("移动到抓取为止所需要的时间")] public int moveTimeMs;
    }
    
    [Serializable]
    public class BuffParams
    {
        [LabelText("参数"),PropertyTooltip("例如造成的伤害量，击退距离等")]public float value;
        [LabelText("参数描述")]public string des;
    }
    
    [LabelText("buff位置类型")]
    public enum BuffPosType
    {
        [LabelText("无配置")] None,
        [LabelText("跟随目标位置")] FollowTarget,
        [LabelText("击中目标位置")] HitTargetPos,
        [LabelText("UI摇杆输入位置")] UIInputPos,
    }
    
    [LabelText("buff附加类型")]
    public enum BuffAttachType
    {
        [LabelText("无配置")]None,
        [LabelText("施法者")]Creator,
        [LabelText("施法目标")]Target,
        [LabelText("施法者位置")]Creator_Pos,
        [LabelText("施法目标位置")]Target_Pos,
        [LabelText("引导位置")]Guide_Pos,
    }

    public enum BuffTypeEnum
    {
        [LabelText("无类型")] None = 0,
        [LabelText("击退")] Repel,
        [LabelText("浮空")] Floating,
        [LabelText("僵直")] Stiff, 
    }
}