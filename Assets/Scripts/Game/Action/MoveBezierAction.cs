using FixMath;
using Game.Tools;
using LogicLayer;
using SkillSystem.Config;

namespace Game.Action
{
    public class MoveBezierAction : ActionBehaviour
    {
        private LogicObject _actionObject;
        private FixIntVector3 _startPos;
        private FixInt _moveTime;
        private FixIntVector3 _heightPos;
        private FixIntVector3 _endPos;
        
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
        
        public MoveBezierAction(LogicObject actionObj,FixIntVector3 startPos,FixIntVector3 heightPos, FixIntVector3 endPos,
            FixInt time,System.Action moveFinishCallback, System.Action moveUpdateCallback)
        {
            _actionObject = actionObj;
            _startPos = startPos;
            _moveTime = time;
            _heightPos = heightPos;
            _endPos = endPos;
            ActionFinishCallback = moveFinishCallback;
            ActionUpdateCallback = moveUpdateCallback;
            
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
            _actionObject.LogicPos = BezierUtils.BezierCurve(_startPos, _heightPos, _endPos, _timeScale);

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