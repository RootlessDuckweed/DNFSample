using System.Collections.Generic;
using FixMath;
using Game.Action;
using ZM.AssetFrameWork;

namespace Game.Timer
{
    public class LogicTimerManager : Singleton<LogicTimerManager>
    {
        /// <summary>
        /// 行动列表
        /// </summary>
        private List<LogicTimer> _timerList = new List<LogicTimer>();

        public void DelayCall(FixInt delayTime,System.Action timerCallback,int loopCount = 1)
        {
            LogicTimer timer = new LogicTimer(delayTime, timerCallback, loopCount); 
            _timerList.Add(timer);
        }

        public void OnLogicFrameUpdate()
        {
            for (int i = _timerList.Count - 1; i >= 0; i--)
            {
                LogicTimer timer = _timerList[i];
                if (timer.TimerFinished)
                {
                    timer.OnTimerFinish();
                    RemoveTimer(timer);
                }
            }

            foreach (var actionBehaviour in _timerList)
            {
                actionBehaviour.OnLogicFrameUpdate();
            }
            
        }
        
        public void RemoveTimer(LogicTimer timer)
        {
            _timerList.Remove(timer);
        }

        public void OnDestroy()
        {
            _timerList.Clear();
        }
    }
}