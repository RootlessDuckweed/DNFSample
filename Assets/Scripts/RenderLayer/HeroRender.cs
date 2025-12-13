using FixMath;
using LogicLayer;
using LogicLayer.Hero;
using SkillSystem.Config;
using UnityEngine;

namespace RenderLayer
{
    public class HeroRender : RenderObject
    {
        private HeroLogic _heroLogic;
        public Vector3 inputDirection;
        protected Animation anim;
        /// <summary>
        /// 左手节点
        /// </summary>
        public Transform leftRoot;
        /// <summary>
        /// 右手节点
        /// </summary>
        public Transform rightRoot;
        public override void OnCreate()
        {
            base.OnCreate();
            _heroLogic = LogicObj as HeroLogic;
            JoystickUGUI.OnMoveCallBack += OnJoyStickMove;
            anim = GetComponent<Animation>();
        }

        protected override void Update()
        {
            base.Update();
            // 判断有没有技能在释放， 有则无法播放移动和待机动画
            //Debug.Log(_heroLogic.releasingSkills.Count);
            if (_heroLogic.releasingSkills.Count <= 0)
            {
                if (inputDirection.x == 0 && inputDirection.z == 0)
                {
                    PlayAnim("Anim_Idle02");
                }
                else
                {
                    PlayAnim("Anim_Run");
                }
            }
            //判断摇杆有无输入
        }

        public override void OnRelease()
        {
            base.OnRelease();
            JoystickUGUI.OnMoveCallBack -= OnJoyStickMove;
        }
        
        // 模拟直接向服务端发送 移动指令
        private void OnJoyStickMove(Vector3 inputDir)
        {
            //TODO: 客户端没有服务端情况下的代码，后续有服务端更改
            inputDirection = inputDir ;
            FixIntVector3 logicDir = FixIntVector3.zero;
            if (inputDir != Vector3.zero)
            {
                logicDir.x = inputDir.x;
                logicDir.y = inputDir.y;
                logicDir.z = inputDir.z;
            }
            _heroLogic.InputLogicFrameEvent(logicDir);
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animName"></param>
        public void PlayAnim(string animName)
        {
            anim.CrossFade(animName,0.2f);
        }

        public override void PlayAnim(AnimationClip clip)
        {
            base.PlayAnim(clip);
            if (anim.GetClip(clip.name) == null)
            {
                anim.AddClip(clip, clip.name);
            }
            anim.clip = clip;
            PlayAnim(clip.name);
        }

        public override Transform GetEffectParent(TransformParentType type)
        {
            if(type==TransformParentType.LeftHand) return leftRoot;
            if(type==TransformParentType.RightHand) return rightRoot;
            
            return null;
        }
    }
}