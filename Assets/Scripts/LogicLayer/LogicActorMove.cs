//处理逻辑对象移动

using FixMath;
using SkillSystem.Config;

namespace LogicLayer
{
    public partial class LogicActor
    {
        private FixIntVector3 _inputMoveDir;
        public void OnLogicFrameUpdateMove()
        {
            Collider?.UpdateColliderInfo(LogicPos,Collider.Size);
            if (ActionState != LogicObjectActionState.Idle && ActionState != LogicObjectActionState.Move && !IsForceAllowMove )
            {
                return;
            }

            LogicPos += _inputMoveDir * LogicMoveSpeed * LogicFrameConfig.LogicFrameIntervalFix;

            // 对象的移动方向
            if (LogicDir != _inputMoveDir)
            {
                LogicDir = _inputMoveDir;
            }
            
            //对象的朝向轴向
            if (LogicDir.x != FixInt.Zero && !IsForceAllowModifyDir)
            {
                LogicXAxis = _inputMoveDir.x > 0 ? 1 : -1;
            }
        }
        
        public void InputLogicFrameEvent(FixIntVector3 inputDir)
        {
            _inputMoveDir = inputDir;
        }
    }
}