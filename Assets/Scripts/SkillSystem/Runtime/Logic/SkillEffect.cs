using System.Collections.Generic;
using LogicLayer;
using SkillSystem.Config;
using SkillSystem.Runtime.Logic;
using SkillSystem.Runtime.Render;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Runtime
{
    public partial class Skill
    {
        private Dictionary<int,SkillEffectLogic> _gameEffectObjDic = new Dictionary<int,SkillEffectLogic>();
        public void OnLogicFrameUpdateEffect()
        {
            if (_skillDataConfig.effectCfgList != null && _skillDataConfig.effectCfgList.Count > 0)
            {
                foreach (var item in _skillDataConfig.effectCfgList)
                {
                    if (item.skillEffect != null && _curLogicFrame == item.triggerFrame)
                    {
                        DestroyEffect(item);
                        Transform parent = null;
                        if (item.isSetTransformParent)
                        {
                            parent = _skillCreator.RenderObj.GetEffectParent(item.transformParentType);
                        }
                        //TODO : 特效生成后续修改成其他方式 现在用这个方法只是为了测试
                        /*var gameEffectObj = GameObject.Instantiate(item.skillEffect,parent);
                        gameEffectObj.transform.localPosition = Vector3.zero;
                        gameEffectObj.transform.localScale = Vector3.one;*/
                        var gameEffectObj = ZMAssetsFrame.Instantiate(item.skillEffectPath, parent, Vector3.zero, Vector3.one,
                            Quaternion.identity);
                        gameEffectObj.transform.localEulerAngles = Vector3.zero;
                        SkillEffectRender effectRender = gameEffectObj.GetComponent<SkillEffectRender>();
                        if (effectRender == null)
                        {
                            effectRender = gameEffectObj.AddComponent<SkillEffectRender>();
                        }
                        SkillEffectLogic effectLogic =
                            new SkillEffectLogic(LogicObjectType.Effect, item, effectRender, _skillCreator,this);
                        effectRender.SetLogicObject(effectLogic,item.effectPosType!=EffectPosType.Zero);
                        _gameEffectObjDic.Add(item.GetHashCode(), effectLogic);
                    }

                    if (_curLogicFrame == item.endFrame)
                    {
                       DestroyEffect(item);
                       continue;
                    }
                    if (_gameEffectObjDic.TryGetValue(item.GetHashCode(), out var effect) && effect != null)
                    {
                        effect.OnLogicFrameEffectUpdate(this,_curLogicFrame);
                    }

                }
            }
            
        }

        public void DestroyEffect(SkillEffectConfig item)
        {
            int hashCode = item.GetHashCode();
            _gameEffectObjDic.TryGetValue(hashCode, out var effect);
            if (effect != null)
            {
                _gameEffectObjDic.Remove(hashCode);
                effect.OnDestroy();
            }
            
        }

        public void ReleaseAllEffect()
        {
            foreach (var item in _skillDataConfig.effectCfgList)
            {
                if (!item.isAttachAction)
                {
                    DestroyEffect(item);
                }
            }
        }
    }
}