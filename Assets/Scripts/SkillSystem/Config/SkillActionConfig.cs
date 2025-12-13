using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace SkillSystem.Config
{

    public enum MoveActionType
    {
        [LabelText("指定目标位置")]
        TargetPos,
        [LabelText("引导位置")]
        GuidePos,
        [LabelText("贝塞尔移动")]
        BezierPos,
    }

    public enum MoveActonFinishOption
    {
        None,
        Skill,
        Buff,
    }
    
    [System.Serializable]
    [HideMonoScript]
    public class SkillActionConfig
    {
        // 是否显示移动位置
        private bool _isShowMovePos;
        // 是否显示移动完成参数
        private bool _isShowFinishParam;
        // 是否显示贝塞尔参数
        private bool _isShowBezierPos;
        [LabelText("触发帧")]
        public int triggerFrame;
        [LabelText("移动方式"),OnValueChanged("OnMoveActionTypeChanged")]
        public MoveActionType moveActionType;
        [LabelText("最高位置"),ShowIf("_isShowBezierPos")]
        public Vector3 heightPos;
        [LabelText("移动位置"),ShowIf("_isShowMovePos")]
        public Vector3 movePos;
        [LabelText("移动完成毫秒")]
        public int durationMs;
        [LabelText("移动完成操作"),OnValueChanged("OnMoveActionFinishOptionChanged")]
        public MoveActonFinishOption actonFinishOption;
        [LabelText("触发参数"),ShowIf("_isShowFinishParam")]
        public List<int> actionFinishedList;

        public void OnMoveActionTypeChanged(MoveActionType type)
        {
            _isShowMovePos = type == MoveActionType.TargetPos||type == MoveActionType.BezierPos;
            _isShowBezierPos = type == MoveActionType.BezierPos;
        }

        public void OnMoveActionFinishOptionChanged(MoveActonFinishOption option)
        {
            _isShowFinishParam = option != MoveActonFinishOption.None;
        }
    }
}