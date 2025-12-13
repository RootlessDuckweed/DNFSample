using System;
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
        private void UpdatePosition()
        {
            // TODO:后续需要更改 不能使用Time.deltaTime
            if(_isUpdatePosAndRota)
                transform.position = Vector3.Lerp(transform.position, LogicObj.LogicPos.ToVector3(), SmoothPosSpeed * Time.deltaTime);
        }

        /// <summary>
        /// 通用的对象方向更新逻辑
        /// </summary>
        private void UpdateDir()
        {
            if (!_isUpdatePosAndRota) return;
            //RenderDir.x = LogicObj.LogicXAxis >= 0 ? 0 : -20;
            RenderDir.y = LogicObj.LogicXAxis >= 0 ? 0 : 180;
            transform.localEulerAngles = RenderDir;
        }

        public void SetLogicObject(LogicObject logicObj,bool isUpdatePositionAndRotation = true)
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

        public virtual void Damage(int damageValue,DamageSource damageSource)
        {
            GameObject damageText = ZMAssetsFrame.Instantiate(AssetPathConfig.DAMAGE_TEXT, null);
            var item = damageText.GetComponent<DamageTextItem>();
            item.ShowDamageText(damageValue, this);
        }

        public void OnHit(GameObject effect, int survivalTimeMs, LogicActor source)
        {
            if (effect != null)
            {
                GameObject hitEffectObj = GameObject.Instantiate(effect);
                hitEffectObj.transform.position = transform.position;
                hitEffectObj.transform.localScale = source.LogicXAxis > 0 ? Vector3.one : new Vector3(-1, 1, 1);
                Destroy(hitEffectObj,survivalTimeMs*1.0f/1000f);
            }
        }
        
        public virtual Transform GetEffectParent(TransformParentType type)
        {
            return null;
        }
    }
}