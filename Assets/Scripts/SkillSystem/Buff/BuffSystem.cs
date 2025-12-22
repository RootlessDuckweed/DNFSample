using System.Collections.Generic;
using LogicLayer;
using SkillSystem.Runtime;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Buff
{
    /// <summary>
    /// 管理所有的buff释放，移除，更新逻辑
    /// </summary>
    public class BuffSystem : Singleton<BuffSystem>
    {
        private List<BuffInstance.Buff> _buffList = new List<BuffInstance.Buff>();

        public void OnCreate()
        {
            
        }

        public BuffInstance.Buff AttachBuff(int buffId, LogicActor releaser,LogicActor target,Skill skill, object[] paramObjs=null)
        {
            if (buffId == 0)
            {
                Debug.LogError("BUFF的ID不能为0");
                return null;
            }
            
            BuffInstance.Buff buff = new BuffInstance.Buff(buffId, releaser, target, skill, paramObjs);
            buff.OnCreate();
            _buffList.Add(buff);
            return buff;
        }

        /// <summary>
        /// 逻辑帧更新接口
        /// </summary>
        public void OnLogicFrameUpdate()
        {
            for (int i = _buffList.Count-1; i >= 0; --i)
            {
                _buffList[i].OnLogicFrameUpdate();
            }
        }

        /// <summary>
        /// 移除buff
        /// </summary>
        /// <param name="buff">指定的buff</param>
        public void RemoveBuff(BuffInstance.Buff buff)
        {   
            if (_buffList.Contains(buff))
            {
                _buffList.Remove(buff);
            }
        }
        
        public void OnDestroy()
        {
            for (int i = _buffList.Count-1; i >= 0; --i)
            {
                _buffList[i].OnDestroy();
            }
        }
    }
}
