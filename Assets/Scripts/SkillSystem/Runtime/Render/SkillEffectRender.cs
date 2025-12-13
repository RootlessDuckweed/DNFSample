using RenderLayer;

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
            Destroy(gameObject);
        }
    }
}