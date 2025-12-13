namespace Game.Action
{
    public abstract class ActionBehaviour
    {
        /// <summary>
        /// 是否行动完成
        /// </summary>
        public bool ActionFinished = false;
        /// <summary>
        /// 行动完成回调
        /// </summary>
        protected System.Action ActionFinishCallback;
        /// <summary>
        /// 行动更新回调
        /// </summary>
        protected System.Action ActionUpdateCallback;

        public abstract void OnLogicFrameUpdate();
        
        public abstract void OnActionFinish();
    }
}