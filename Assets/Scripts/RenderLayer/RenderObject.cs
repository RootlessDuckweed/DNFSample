using System;
using FixMath;
using Game.Timer;
using LogicLayer;
using SkillSystem.Config;
using SkillSystem.Runtime;
using UnityEngine;
using ZM.AssetFrameWork;

namespace RenderLayer
{
    public class RenderObject : MonoBehaviour
    {
        /// <summary>
        /// 逻辑对象
        /// </summary>
        public LogicObject LogicObj;
        /// <summary>
        /// 平滑插值速度
        /// </summary>
        protected float SmoothPosSpeed = 10;

        protected Vector2 RenderDir;

        private bool _isUpdatePosAndRota;
        
        /// <summary>
        /// 渲染层脚本创建
        /// </summary>
        public virtual void OnCreate()
        {
            
        }

        /// <summary>
        /// 渲染层脚本释放
        /// </summary>
        public virtual void OnRelease()
        {
        }
        protected virtual void Update()
        {
            UpdatePosition();
            UpdateDir();
        }

        /// <summary>
        /// 通用的位置更新逻辑
        /// </summary>
        protected virtual void UpdatePosition()
        {
            if(_isUpdatePosAndRota)
                transform.position = Vector3.Lerp(transform.position, LogicObj.LogicPos.ToVector3(), SmoothPosSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 通用的对象方向更新逻辑
        /// </summary>
        protected virtual void UpdateDir()
        {
            if (!_isUpdatePosAndRota) return;
            //RenderDir.x = LogicObj.LogicXAxis >= 0 ? 0 : -20;
            RenderDir.y = LogicObj.LogicXAxis >= 0 ? 0 : 180;
            transform.localEulerAngles = RenderDir;
        }

        public virtual void SetLogicObject(LogicObject logicObj,bool isUpdatePositionAndRotation = true)
        {
            LogicObj = logicObj;
            _isUpdatePosAndRota = isUpdatePositionAndRotation;
            transform.position = logicObj.LogicPos.ToVector3();
            if (_isUpdatePosAndRota == false)
            {
                transform.localPosition = Vector3.zero;
            }
            UpdateDir();
        }


        public virtual void PlayAnim(AnimationClip clip)
        {
            
        }
        
        public virtual void PlayAnim(string clipName)
        {
            
        }

        public virtual string GetCurAnimName()
        {
            return string.Empty;
        }

        /// <summary>
        /// 造成伤害扣血的渲染表现
        /// </summary>
        /// <param name="damageValue"></param>
        /// <param name="damageSource"></param>
        /// <param name="source"></param>
        public virtual void Damage(int damageValue,DamageSource damageSource,LogicObject source)
        {
            GameObject damageText = ZMAssetsFrame.Instantiate(AssetPathConfig.DAMAGE_TEXT, null);
            var item = damageText.GetComponent<DamageTextItem>();
            item.ShowDamageText(damageValue, this);
           
        }
        /// <summary>
        /// 受到伤害的渲染表现
        /// </summary>
        /// <param name="effectPath">特效</param>
        /// <param name="survivalTimeMs">特效存活时间</param>
        /// <param name="source">伤害源</param>
        /// <param name="effectPoint">受击特效生成附着对象</param>
        public virtual void OnHit(string effectPath, int survivalTimeMs, LogicObject source,LogicObject effectPoint)
        {
            if (!string.IsNullOrEmpty(effectPath))
            {
                var hitEffectObj = ZMAssetsFrame.Instantiate(effectPath, null);
                hitEffectObj.transform.position = effectPoint.RenderObj.transform.position;
                Destroy(hitEffectObj,survivalTimeMs*1.0f/1000f);
                LogicTimerManager.Instance.DelayCall(new FixInt(survivalTimeMs) / new FixInt(1000), () =>
                {
                    ZMAssetsFrame.Release(hitEffectObj);
                });
            }
        }
        
        public virtual Transform GetEffectParent(TransformParentType type)
        {
            return null;
        }

        public virtual void ShowSkillPortrait(GameObject portrait)
        {
            if (portrait != null)
            {
                GameObject por = GameObject.Instantiate(portrait);
                Destroy(por,3);
            }
        }

        public virtual void OnDeath()
        {
            LogicObj.ObjectState = LogicObjectState.Death;
        }
    }
}