/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/12/9 0:52:13
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using LogicLayer;

namespace ZMGC.Battle
{
	/// <summary>
	/// 处理战斗逻辑
	/// </summary>
	public  class BattleLogicCtrl : ILogicBehaviour
	{
		private HeroLogicCtrl _heroLogicCtrl;
		private MonsterLogicCtrl _monsterLogicCtrl;
		 public  void OnCreate()
		 {
			_heroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
			_monsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
		 }
			
		 /// <summary>
		 /// 更具当前对象类型获取攻击的目标
		 /// </summary>
		 /// <param name="logicObjectType"></param>
		 /// <returns></returns>
		 public List<LogicActor> GetEnemyList(LogicObjectType logicObjectType)
		 {
			 List<LogicActor> enemyList = new List<LogicActor>();
			 if (logicObjectType == LogicObjectType.Hero)
			 {
				 foreach (var item in _monsterLogicCtrl.MonsterList)
				 {
					 enemyList.Add(item);
				 }
			 }
			 else if(logicObjectType == LogicObjectType.Monster)
			 {
				 enemyList.Add(_heroLogicCtrl.HeroLgc);
			 }

			 return enemyList;
		 }
		
		 public  void OnDestroy()
		 {
		
		 }
	
	}
}
