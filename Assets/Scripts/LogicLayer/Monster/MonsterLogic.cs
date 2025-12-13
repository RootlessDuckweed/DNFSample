using FixIntPhysics;
using FixMath;
using RenderLayer;

namespace LogicLayer.Monster
{
    public class MonsterLogic : LogicActor
    {
        public int MonsterId { get; private set; }
        public MonsterLogic(int monsterID,RenderObject renderObj,FixIntBoxCollider boxCollider,FixIntVector3 initPos)
        {
            MonsterId = monsterID;
            RenderObj = renderObj;
            Collider = boxCollider;
            LogicPos = initPos;
            ObjectType = LogicObjectType.Monster;
        }
    }
}