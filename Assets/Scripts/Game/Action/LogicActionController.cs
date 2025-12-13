using System.Collections.Generic;
using ZM.AssetFrameWork;

namespace Game.Action
{
    public class LogicActionController : Singleton<LogicActionController>
    {
        /// <summary>
        /// 行动列表
        /// </summary>
        private List<ActionBehaviour> _actionList = new List<ActionBehaviour>();

        public void RunAction(ActionBehaviour action)
        {
            action.ActionFinished = false;
            _actionList.Add(action);
        }

        public void OnLogicFrameUpdate()
        {
            for (int i = _actionList.Count - 1; i >= 0; i--)
            {
                ActionBehaviour action = _actionList[i];
                if (action.ActionFinished)
                {
                    action.OnActionFinish();
                    RemoveAction(action);
                }
            }

            foreach (var actionBehaviour in _actionList)
            {
                actionBehaviour.OnLogicFrameUpdate();
            }
            
        }
        
        public void RemoveAction(ActionBehaviour action)
        {
            _actionList.Remove(action);
        }

        public void OnDestroy()
        {
            _actionList.Clear();
        }
    }
}