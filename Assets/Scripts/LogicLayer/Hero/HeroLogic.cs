using System.Collections.Generic;
using FixMath;
using RenderLayer;
using SkillSystem.Runtime.Logic;

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
        

        public override void OnLogicFrameUpdate()
        {
            base.OnLogicFrameUpdate();
            
        }
    }
}