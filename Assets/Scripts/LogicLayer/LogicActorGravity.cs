//处理逻辑对象重力

using FixMath;
using Sirenix.OdinInspector.Editor.Validation;
using SkillSystem.Config;
using UnityEngine;

namespace LogicLayer
{
    public partial class LogicActor
    {
        protected FixInt Gravity = new FixInt(10035L); // 9.8f的定点数
        protected FixIntVector3 Velocity;
        public bool IsAddForce;
        protected FixInt RisingTime;
        private bool _isIgnoreGravity;
        public bool IsIgnoreGravity
        {
            get => _isIgnoreGravity;
            set => _isIgnoreGravity = value;
        }
        /// <summary>
        /// 初始速度y的速度
        /// </summary>
        private FixInt _vo;
        public void OnLogicFrameUpdateGravity()
        {
            if (IsAddForce)
            {
                FixInt logicFrameIntervalFix = LogicFrameConfig.LogicFrameIntervalFix;
                FixInt gt = Gravity * logicFrameIntervalFix;
                FixInt risingForceTime = (_vo / gt) * logicFrameIntervalFix;
                FixInt timeScale = (risingForceTime * 2) / RisingTime;
                
                Velocity.y -= Gravity * logicFrameIntervalFix * timeScale;
                
                //如果忽略重力，则不进行重力的位置更新
                FixIntVector3 newPos = new FixIntVector3(LogicPos.x, FixIntMath.Clamp(LogicPos.y + Velocity.y * logicFrameIntervalFix,0,FixInt.MaxValue), LogicPos.z);
                if (!_isIgnoreGravity)
                {
                    LogicPos = newPos;
                }
                if (newPos.y <= 0)
                {
                    IsAddForce = false;
                    TriggerGround();
                }
                else
                {
                    if (Velocity.y > 0)
                    {
                        Floating(true);
                    }
                    else
                    {
                        Floating(false);
                    }
                }

            }
        }

        /// <summary>
        /// 添加上升力
        /// </summary>
        /// <param name="risingForceValue"></param>
        /// <param name="risingTime"></param>
        public void AddRisingForce(FixInt risingForceValue,FixInt risingTime)
        {
            _vo = Velocity.y = risingForceValue;
            RisingTime = risingTime*1.0f/1000;
            IsAddForce = true;
            Debug.Log("AddRisingForceStart:" + Time.realtimeSinceStartup);
        }
    }
}