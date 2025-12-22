using System;
using LogicLayer;
using SkillSystem.Buff.BuffCfg;
using SkillSystem.Config;
using SkillSystem.Runtime;
using Tools;
using ZM.AssetFrameWork;

namespace SkillSystem.Buff.BuffInstance
{
    public enum BuffState
    {
        None,
        Delay, // 延迟中
        Start, // 开始触发
        Update, // 更新中
        End // buff结束
    }

    public class Buff
    {
        /// <summary>
        /// buff配置
        /// </summary>
        public BuffConfig BuffCfg { get; private set; }

        /// <summary>
        /// buff当前的状态
        /// </summary>
        public BuffState State;

        /// <summary>
        /// buff唯一id
        /// </summary>
        public readonly int BuffID;

        /// <summary>
        /// buff释放者
        /// </summary>
        public LogicActor Releaser;

        /// <summary>
        /// buff附加攻击对象
        /// </summary>
        public LogicActor AttachTarget;

        /// <summary>
        /// 隶属于的技能
        /// </summary>
        public Skill Skill;

        /// <summary>
        /// buff所需的一些参数
        /// </summary>
        public object[] ParamObjs;

        private int _curDeyTime;
        
        /// <summary>
        /// buff逻辑组合对象
        /// </summary>
        private BuffComposite _buffLogic;

        /// <summary>
        /// 当前真实运行时间
        /// </summary>
        private int _curRealRuntime;

        /// <summary>
        /// 当前累积运行时间
        /// </summary>
        private int _accRuntime;

        public Buff(int buffId, LogicActor releaser, LogicActor target, Skill skill, object[] paramsObjs)
        {
            this.BuffID = buffId;
            this.Releaser = releaser;
            this.AttachTarget = target;
            this.Skill = skill;
            this.ParamObjs = paramsObjs;
        }

        public void OnCreate()
        {
            // 加载buff配置文件
            BuffCfg = ZMAssetsFrame.LoadScriptableObject<BuffConfig>(AssetPathConfig.BUFF_DATA+BuffID+".asset");

            if (BuffCfg.buffType == BuffTypeEnum.Repel)
            {
                _buffLogic = new RepelBuff(this);
            }
            else if(BuffCfg.buffType == BuffTypeEnum.Floating)
            {
                _buffLogic = new FloatingBuff(this);
            }
            else if (BuffCfg.buffType == BuffTypeEnum.Stiff)
            {
                _buffLogic = new StiffBuff(this);
            }
            
            
            State = BuffCfg.buffDelay == 0 ? BuffState.Start : BuffState.Delay;
            _curDeyTime = BuffCfg.buffDelay;
        }

        public void OnLogicFrameUpdate()
        {
            switch (State)
            {
                case BuffState.None:
                    break;
                case BuffState.Delay:

                    if (_curDeyTime == BuffCfg.buffDelay)
                    {
                        //处理buff延迟逻辑
                        _buffLogic.BuffDelay();
                    }
                    _curDeyTime -= LogicFrameConfig.LogicFrameIntervalMs;
                    if (_curDeyTime <= 0)
                    {
                        State = BuffState.Start;
                    }
                    
                    break;
                case BuffState.Start:
                    
                    // 1.调用buffStart接口
                    BuffStart();
                    // 2.调用buff触发逻辑接口
                    BuffTrigger();
                    // 判断buff是否需要切换位更新状态，如果buff持续时间为有限或无限，才进入更新状态
                    State = (BuffCfg.buffDurationMs==-1||BuffCfg.buffDurationMs>0) ? BuffState.Update : BuffState.End;
                    
                    break;
                case BuffState.Update:
                    UpdateBuffLogic();
                    break;
                case BuffState.End:
                    OnDestroy();
                    break;
            }
        }

        public void BuffStart()
        {
            _buffLogic.BuffStart();
            AttachTarget.AddBuff(this);
        }

        public void BuffTrigger()
        {
            _buffLogic.BuffTrigger();
            switch (BuffCfg.buffTriggerAnim)
            {
                case ObjectAnimationState.None:
                    break;
                case ObjectAnimationState.BeHit:
                    AttachTarget.PlayAnim(AnimationName.Anim_Beiji_01);
                    break;
                case ObjectAnimationState.Stiff:
                    AttachTarget.PlayAnim(AnimationName.Anim_Beiji_02);
                    break;
            }
            // 处理buff需要播放的音效
            if (BuffCfg.buffAudio != null)
            {
                AudioController.GetInstance().PlaySoundByAudioClip(BuffCfg.buffAudio,false,2);
            }
        }

        private void UpdateBuffLogic()
        {
            //处理buff间隔逻辑
            int logicFrameIntervalMs = LogicFrameConfig.LogicFrameIntervalMs;
            if (BuffCfg.buffIntervalMs > 0)
            {
                // 当前累积运行时间是否大于buff触发间隔，如果大于触发buff效果
                _curRealRuntime += logicFrameIntervalMs;
                if (_curRealRuntime >= BuffCfg.buffIntervalMs)
                {
                    _buffLogic.BuffTrigger();
                    _curRealRuntime -= logicFrameIntervalMs;
                }
               
            }
          
            UpdateBuffDurationTime();
            
        }

        private void UpdateBuffDurationTime()
        {
            _accRuntime += LogicFrameConfig.LogicFrameIntervalMs;
            if (_accRuntime >= BuffCfg.buffDurationMs)
            {
                State = BuffState.End;
            }
        }

        public void OnDestroy()
        {
            _buffLogic.BuffEnd();
            AttachTarget.RemoveBuff(this);
            SkillSystem.Buff.BuffSystem.Instance.RemoveBuff(this);
        }
    }
}