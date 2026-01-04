using System;
using FixMath;
using LogicLayer;
using RenderLayer;
using SkillSystem.Buff.BuffCfg;
using SkillSystem.Config;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Buff
{
    public class BuffRender : RenderObject
    {
        private BuffConfig _buffCfg;
        private FixIntVector3 _inputPos;
        private HeroRender _heroRender;
        private LogicObject _attachTarget;
        public void InitBuffRender(LogicObject logicObj,LogicObject attachTarget,BuffConfig buffCfg,FixIntVector3 targetPos)
        {
            base.SetLogicObject(logicObj,false);
            _buffCfg = buffCfg;
            _inputPos = targetPos;
            _heroRender =logicObj.RenderObj as HeroRender;
            _attachTarget = attachTarget;
            // 处理音效的播放
            if (buffCfg.buffAudio != null)
            {
                AudioController.GetInstance().PlaySoundByAudioClip(buffCfg.buffAudio,false,2);
            }
            // 处理特效位置以及附加节点
            if (buffCfg.effectConfig.effectAttachType == EffectAttachType.Hand)
            {
                transform.SetParent(_heroRender?.GetEffectParent(TransformParentType.LeftHand));
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.identity;
            }
            else 
            {
                switch (buffCfg.buffPosType)
                {
                    case BuffPosType.None:
                        break;
                    case BuffPosType.FollowTarget:
                        break;
                    case BuffPosType.HitTargetPos:
                        transform.position = attachTarget.LogicPos.ToVector3();
                        break;
                    case BuffPosType.ReleaserPos:
                        transform.position = logicObj.LogicPos.ToVector3();
                        break;
                    case BuffPosType.UIInputPos:
                        transform.position = _inputPos.ToVector3();
                        break;
                }
            }
            PlayParticle();
        }

        protected override void Update()
        {
            if (_buffCfg != null)
            {
                if (_buffCfg.buffPosType == BuffPosType.FollowTarget)
                {
                    transform.position = _attachTarget.RenderObj.transform.position;
                }
            }
        }

        public void PlayParticle()
        {
            ParticleSystem[] particleSystems = transform.GetComponents<ParticleSystem>();
            foreach (var item in particleSystems)
            {
                item.Play();
            }
        }

        public override void OnRelease()
        {
            base.OnRelease();
            _buffCfg = null;
            _attachTarget = null;
            //Destroy(gameObject);
            ZMAssetsFrame.Release(gameObject);
        }
    }
}