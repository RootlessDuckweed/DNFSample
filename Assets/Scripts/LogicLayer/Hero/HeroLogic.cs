using FixMath;
using RenderLayer;

namespace LogicLayer.Hero
{
    public class HeroLogic : LogicActor
    {
        public int HeroID { get; private set; }

        public HeroLogic(int heroID, RenderObject renderObj)
        {
            HeroID = heroID;
            RenderObj = renderObj;
            ObjectType = LogicObjectType.Hero;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            
        }
    }
}