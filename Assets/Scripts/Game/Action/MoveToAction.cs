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
            FixIntVector3 addDistance = FixIntVector3.zero;
            if (_moveType == MoveType.Target)
            {
                addDistance = _moveDistance * _timeScale;
                _actionObject.LogicPos = _startPos + addDistance;
            }
            else if(_moveType == MoveType.X)
            {
                addDistance.x = _moveDistance.x * _timeScale;
                _actionObject.LogicPos = new FixIntVector3(_startPos.x + addDistance.x,_actionObject.LogicPos.y,_actionObject.LogicPos.z);
            }
            else if(_moveType == MoveType.Y)
            {
                addDistance.y = _moveDistance.y * _timeScale;
                _actionObject.LogicPos = new FixIntVector3(_actionObject.LogicPos.x,_startPos.y + addDistance.y,_actionObject.LogicPos.z);
            }
            else if(_moveType == MoveType.Z)
            {
                addDistance.z = _moveDistance.z * _timeScale;
                _actionObject.LogicPos = new FixIntVector3(_actionObject.LogicPos.x,_actionObject.LogicPos.y,_startPos.z + addDistance.z);
            }
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