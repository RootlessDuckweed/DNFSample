using System.Collections.Generic;
using SkillSystem.Runtime.Logic;

namespace LogicLayer
{
    public partial class LogicActor
    {
        private List<SkillBulletLogic> _bulletLogicsList = new List<SkillBulletLogic>();
        
        public  void AddBullet(SkillBulletLogic bullet)
        {
            _bulletLogicsList.Add(bullet);
        }

        public void RemoveBullet(SkillBulletLogic bullet)
        {
            _bulletLogicsList.Remove(bullet);
        }

        public void OnLogicFrameUpdateBullet()
        {
            for (int i =_bulletLogicsList.Count - 1; i >= 0; i--)
            {
                if (_bulletLogicsList[i].IsFailure)
                {
                    RemoveBullet(_bulletLogicsList[i]);   
                }
            }
            
            
            for (int i =_bulletLogicsList.Count - 1; i >= 0; i--)
            {
                _bulletLogicsList[i].OnLogicFrameUpdate();
            }
        }
    }
}