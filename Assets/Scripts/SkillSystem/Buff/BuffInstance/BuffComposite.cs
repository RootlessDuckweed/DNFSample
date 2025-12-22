namespace SkillSystem.Buff.BuffInstance
{
    public abstract class BuffComposite
    {
        protected Buff Buff;

        public BuffComposite(Buff buff)
        {
            Buff = buff;
        }

        /// <summary>
        /// buff延迟触发
        /// </summary>
        public abstract void BuffDelay();
        /// <summary>
        /// buff开始流程
        /// </summary>
        public abstract void BuffStart();
        /// <summary>
        /// buff逻辑触发，可以执行晕眩逻辑或者属性修改等
        /// </summary>
        public abstract void BuffTrigger();
        /// <summary>
        /// buff结束
        /// </summary>
        public abstract void BuffEnd();
    }
}