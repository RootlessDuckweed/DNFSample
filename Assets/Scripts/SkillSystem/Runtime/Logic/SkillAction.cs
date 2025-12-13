using System;
using FixMath;
using Game.Action;
using LogicLayer;
using SkillSystem.Config;
using UnityEngine;

namespace SkillSystem.Runtime
{
    public partial class Skill
    {
        public void OnLogicFrameUpdateAction()
        {
            if (_skillDataConfig.actionList is { Count: > 0 })
            {
                foreach (var item in _skillDataConfig.actionList)
                {
                    if (item.triggerFrame == _curLogicFrame)
                    {
                        // 触发行动
                        AddMoveAction(item,_skillCreator);
                    }
                    
                }
            }
        }

        /// <summary>
        /// 添加移动行动
        /// </summary>
        /// <param name="item">行动配置</param>
        /// <param name="logicObject">移动的对象</param>
        /// <param name="finishedCallback">完成移动的回调</param>
        public void AddMoveAction(SkillActionConfig item,LogicObject logicObject,Action finishedCallback = null)
        {
            FixIntVector3 movePos = new FixIntVector3(item.movePos);
            movePos.x *= logicObject.LogicXAxis;
            FixIntVector3 targetPos = logicObject.LogicPos + movePos;
            MoveType moveType = MoveType.Target;
            if (movePos.x != FixInt.Zero && movePos.y == FixInt.Zero && movePos.z == FixInt.Zero)
            {
                moveType = MoveType.X;
            }else if (movePos.x == FixInt.Zero && movePos.y != FixInt.Zero && movePos.z == FixInt.Zero)
            {
                moveType = MoveType.Y;
            }else if (movePos.x == FixInt.Zero && movePos.y == FixInt.Zero && movePos.z != FixInt.Zero)
            {
                moveType = MoveType.Z;
            }
            MoveToAction action = new MoveToAction(logicObject,logicObject.LogicPos,targetPos, item.durationMs,() =>
            {
                if (item.actonFinishOption != MoveActonFinishOption.None)
                {
                    switch (item.actonFinishOption)
                    {
                        case MoveActonFinishOption.Skill:
                            foreach (var skillId in item.actionFinishedList)
                            {
                                _skillCreator.ReleaseSkill(skillId);
                            }
                            break;
                        case MoveActonFinishOption.Buff:
                            //TODO : 添加buff
                            break;
                    }
                    finishedCallback?.Invoke();
                }
            },null,moveType);
            
            LogicActionController.Instance.RunAction(action);
        }
    }
}