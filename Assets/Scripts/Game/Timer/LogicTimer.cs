using FixMath;
using Game.Action;
using SkillSystem.Config;

namespace Game.Timer
{
   
    public class LogicTimer : TimerBehaviour
    {
        private FixInt _delayTime;
        private int _loopCount;
        private FixInt _curLogicFrameAccTime;
        private FixInt _totalTime;
        public LogicTimer(FixInt delayTime,System.Action timerCallback,int loopCount=1)
        {
            this._delayTime = delayTime;
            this._loopCount = loopCount;
            this._totalTime = loopCount * delayTime;
            TimerFinishCallback = timerCallback;
        }
        
        public override void OnLogicFrameUpdate()
        {
            _curLogicFrameAccTime += LogicFrameConfig.LogicFrameIntervalFix;
            if (_curLogicFrameAccTime >= _delayTime)
            {
                TimerFinishCallback?.Invoke();
                _curLogicFrameAccTime -= _delayTime;
                _totalTime -= _delayTime;
                if (_loopCount <= 1 || _totalTime<=0)
                {
                    TimerFinished = true;
                    TimerFinishCallback = null;
                }
               
            }
        }

        public override void OnTimerFinish()
        {
            
        }
    }
}