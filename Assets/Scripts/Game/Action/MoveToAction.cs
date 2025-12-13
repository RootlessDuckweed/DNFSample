using FixMath;
using LogicLayer;
using SkillSystem.Config;
using UnityEngine;

namespace Game.Action
{
    public enum MoveType
    {
        Target,
        X,
        Y,
        Z,
    }
    public class MoveToAction : ActionBehaviour
    {
        private LogicObject _actionObject;
        private FixIntVector3 _startPos;
        private FixInt _moveTime;
        private MoveType _moveType;
        
        /// <summary>
        /// 移动的向量
        /// </summary>
        private FixIntVector3 _moveDistance;

        /// <summary>
        /// 当前累积运行的时间
        /// </summary>
        private FixInt _accRunTime;
        /// <summary>
        /// 当前移动的时间缩放
        /// </summary>
        private FixInt _timeScale;
        
        public MoveToAction(LogicObject actionObj,FixIntVector3 startPos, FixIntVector3 targetPos,
            FixInt time,System.Action moveFinishCallback, System.Action moveUpdateCallback,MoveType moveType)
        {
            _actionObject = actionObj;
            _startPos = startPos;
            _moveTime = time;
            _moveType = moveType;
            ActionFinishCallback = moveFinishCallback;
            ActionUpdateCallback = moveUpdateCallback;
            
            _moveDistance = targetPos -  startPos;
            
        }
        public override void OnLogicFrameUpdate()
        {
            if(_actionObject == null) return;
            _accRunTime += LogicFrameConfig.LogicFrameIntervalMs;
            _timeScale = _accRunTime / _moveTime;

            if (_timeScale >= 1)
            {
                _timeScale = 1;
                ActionFinished = true;
            }
            ActionUpdateCallback?.Invoke();
            /*FixIntVector3 addDistance = FixIntVector3.zero;
            if (_moveType == MoveType.Target)
            {
                addDistance = _startPos + _moveDistance * _timeScale;
            }
            else if(_moveType == MoveType.X)
            {
                addDistance.x = _actionObject.LogicPos.x * _timeScale;
            }
            else if(_moveType == MoveType.Y)
            {
                addDistance.y = _actionObject.LogicPos.y * _timeScale;
            }
            else if(_moveType == MoveType.Z)
            {
                addDistance.z = _actionObject.LogicPos.z * _timeScale;
            }
            _actionObject.LogicPos = addDistance;*/
            _actionObject.LogicPos = _startPos + _moveDistance * _timeScale;
        }

        /// <summary>
        /// 行动完成
        /// </summary>
        public override void OnActionFinish()
        {
            if (ActionFinished)
            {
                ActionFinishCallback?.Invoke();
            }
        }
    }
}