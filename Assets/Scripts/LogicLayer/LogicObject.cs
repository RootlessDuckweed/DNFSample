using FixIntPhysics;
using FixMath;
using RenderLayer;

namespace LogicLayer
{
    // 代表 怪物和英雄的逻辑对象同时具有的属性
    public abstract class LogicObject
    {
        private FixIntVector3 _logicPos;
        private FixIntVector3 _logicDir;
        private FixIntVector3 _logicAngle;
        private FixInt _logicMoveSpeed = 3;
        private FixInt _logicXAxis = 1;
        private FixIntVector3 _isActive; // 当前对象是否激活
        private bool _isForceAllowMove = false;
        private bool _isNotForceAllowModifyDir = false;
        /// <summary>
        /// 逻辑位置
        /// </summary>
        public FixIntVector3 LogicPos
        {
            get => _logicPos;
            set { _logicPos = value; }
        }
        
        /// <summary>
        /// 逻辑朝向
        /// </summary>
        public FixIntVector3 LogicDir
        {
            get => _logicDir;
            set { _logicDir = value; }
        }

        /// <summary>
        /// 逻辑角度
        /// </summary>
        public FixIntVector3 LogicAngle
        {
            get => _logicAngle;
            set { _logicAngle = value; }
        }
 
        /// <summary>
        /// 逻辑对象移动速度
        /// </summary>
        public FixInt LogicMoveSpeed
        {
            get => _logicMoveSpeed;
            set { _logicMoveSpeed = value; }
        }
        
        /// <summary>
        /// 逻辑轴向
        /// </summary>
        public FixInt LogicXAxis
        {
            get => _logicXAxis;
            set { _logicXAxis = value; }
        }

        /// <summary>
        /// 当前对象是否激活
        /// </summary>
        public FixIntVector3 IsActive
        {
            get => _isActive;
            set { _isActive = value; }
        } 

        /// <summary>
        /// 渲染对象
        /// </summary>
        public RenderObject RenderObj
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 定点数碰撞体
        /// </summary>
        public FixIntBoxCollider Collider
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 逻辑对象状态
        /// </summary>
        public LogicObjectState ObjectState
        {
            get;
            set;
        }
        
        /// <summary>
        /// 逻辑对象类型
        /// </summary>
        public LogicObjectType ObjectType
        {
            get;
            set;
        }
        
        /// <summary>
        /// 逻辑对象行动状态
        /// </summary>
        public LogicObjectActionState ActionState
        {
            get;
            set;
        }
        /// <summary>
        /// 是否强制允许移动
        /// </summary>
        public bool IsForceAllowMove
        {
            get => _isForceAllowMove;
            set => _isForceAllowMove = value;
        }

        /// <summary>
        /// 是否不允许改变朝向
        /// </summary>
        public bool IsForceAllowModifyDir
        {
            get => _isNotForceAllowModifyDir;
            set => _isNotForceAllowModifyDir = value;
        }

        /// <summary>
        /// 初始化接口
        /// </summary>
        public virtual void OnCreate()
        {
            
        }

        /// <summary>
        /// 逻辑帧更新接口
        /// </summary>
        public virtual void OnLogicFrameUpdate()
        {
            
        }
        
        /// <summary>
        /// 逻辑对象销毁接口 
        /// </summary>
        public virtual void OnDestroy()
        {
            
        }
        
    }

    public enum LogicObjectState
    {
        Survival,
        Death,
    }

    public enum LogicObjectType
    {
        Hero,
        Monster,
        Effect,
        Bullet
    }

    public enum LogicObjectActionState
    {
        Idle, // 待机中
        Move, // 移动中
        SkillReleasing, // 释放技能中
        Floating, // 浮空中
        Hitting, // 受击中 
        StockPiling, // 蓄力中
    }
}