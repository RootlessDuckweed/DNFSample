using RenderLayer;
using ZM.AssetFrameWork;

namespace SkillSystem.Runtime.Render
{
    public class SkillEffectRender : RenderObject
    {
        protected override void Update()
        {
            base.Update();
        }


        public override void OnRelease()
        {
            base.OnRelease();
            //Destroy(gameObject);
            ZMAssetsFrame.Release(gameObject);
        }
    }
}