namespace Game.Timer
{
    public abstract class TimerBehaviour
    {
        /// <summary>
        /// 是否行动完成
        /// </summary>
        public bool TimerFinished = false;
        /// <summary>
        /// 行动完成回调
        /// </summary>
        protected System.Action TimerFinishCallback;
        /// <summary>
        /// 行动更新回调
        /// </summary>
        protected System.Action TimerUpdateCallback;

        public abstract void OnLogicFrameUpdate();
        
        public abstract void OnTimerFinish();
    }
}