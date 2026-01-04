using LogicLayer;
using RenderLayer;
using SkillSystem.Config;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Runtime.Render
{
    public class SkillBulletRender : RenderObject
    {
        private SkillBulletConfig _bulletCfg;
        public void SetRenderData(LogicObject logicObject,SkillBulletConfig bulletCfg)
        {
            SetLogicObject(logicObject);
            _bulletCfg = bulletCfg;
        }

        protected override void UpdatePosition()
        {
            base.UpdatePosition();
        }

        protected override void UpdateDir()
        {
            transform.rotation = Quaternion.Euler(LogicObj.LogicAngle.ToVector3());
        }
        
        public override void OnRelease()
        {
            base.OnRelease();
            //Destroy(gameObject);
            ZMAssetsFrame.Release(gameObject);
        }
    }
}