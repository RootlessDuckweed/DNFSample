using System;
using FixMath;
using Game.Action;
using LogicLayer;
using SkillSystem.Buff;
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
        /// <param name="offset">引导位置偏移</param>
        /// <param name="finishedCallback">完成移动的回调</param>
        /// <param name="updateCallback">移动更新回调</param>
        public void AddMoveAction(SkillActionConfig item,LogicObject logicObject,Vector3 offset = default(Vector3)
            ,Action finishedCallback = null,Action updateCallback = null)
        {
            FixIntVector3 movePos = new FixIntVector3(item.movePos);
            movePos.x *= logicObject.LogicXAxis;
            FixIntVector3 targetPos = logicObject.LogicPos + movePos;
            FixIntVector3 startPos = logicObject.LogicPos;
            FixIntVector3 offsetFixIntVector3 = new FixIntVector3(offset);
            
            MoveType moveType = MoveType.Target;
            if (item.moveActionType == MoveActionType.TargetPos)
            {
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
            }
            else if (item.moveActionType == MoveActionType.GuidePos)
            {
                // 目标位置
                targetPos = SkillGuidePos;
                offsetFixIntVector3.x *= logicObject.LogicXAxis;
                startPos = targetPos +  offsetFixIntVector3;
            }
            else if (item.moveActionType == MoveActionType.BezierPos)
            {
                // 计算起始位置
                offsetFixIntVector3.x *= _skillCreator.LogicXAxis;
                startPos = _skillCreator.LogicPos + offsetFixIntVector3;
                // 计算最高点
                FixIntVector3 heightPosOffset = new FixIntVector3(item.heightPos);
                heightPosOffset.x *= _skillCreator.LogicXAxis;
                FixIntVector3 heightPos = _skillCreator.LogicPos + heightPosOffset;
                // 结束位置
                targetPos = _skillCreator.LogicPos + movePos;
                //执行贝塞尔移动
                MoveBezierAction bezierAction = new MoveBezierAction(logicObject, startPos, heightPos, targetPos,
                    item.durationMs, moveFinishCallback: () =>
                    {
                        OnActionFinish(item, logicObject,finishedCallback);
                    }, moveUpdateCallback: updateCallback);
                LogicActionController.Instance.RunAction(bezierAction);
                return;
            }
            MoveToAction action = new MoveToAction(logicObject,startPos,targetPos, item.durationMs, moveFinishCallback : () =>
            {
                OnActionFinish(item, logicObject,finishedCallback);
            },moveUpdateCallback: updateCallback,moveType);
            
            LogicActionController.Instance.RunAction(action);
        }

        private void OnActionFinish(SkillActionConfig item,LogicObject logicObject, Action finishedCallback)
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
                        SkillGuidePos = logicObject.LogicPos;
                        foreach (var buffId in item.actionFinishedList)
                        {
                            BuffSystem.Instance.AttachBuff(buffId,_skillCreator,_skillCreator,this);
                        }
                        break;
                }
            }
            finishedCallback?.Invoke();
        }
    }
}