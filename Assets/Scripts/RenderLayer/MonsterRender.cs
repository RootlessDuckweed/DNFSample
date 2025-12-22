using LogicLayer;
using UnityEngine;

namespace RenderLayer
{
    public class MonsterRender : RenderObject
    {
        private Animation _anim;
        private string _curAnimName;
        public override void OnCreate()
        {
            base.OnCreate();
            _anim = GetComponentInChildren<Animation>();
        }

        public override void PlayAnim(string clipName)
        {
            base.PlayAnim(clipName);
            if (_anim == null)
            {
                return;
            }

            //怪物死亡只能播放死亡动画
            if (LogicObj.ObjectState == LogicObjectState.Death && !string.Equals(name, "Anim_Dead"))
            {
                return;
            }
            _curAnimName = clipName;
            _anim.Play(clipName);   
        }

        public override string GetCurAnimName()
        {
            return _curAnimName;
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}