/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2025/12/11 16:36:18
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace ZMGC.Battle
{
	/// <summary>
	/// 储存英雄相关的数据
	/// </summary>
	public  class HeroDataMgr : IDataBehaviour
	{
		 /// <summary>
		 /// 普通技能ID
		 /// </summary>
		 private Dictionary<int,int[]> _heroNormalSkillIdDict = new Dictionary<int,int[]>()
		 {
			 {1000,new []{1001,1002,1003}}, // 鬼剑士
		 };
		 
		 /// <summary>
		 /// 特殊技能ID
		 /// </summary>
		 private Dictionary<int,int[]> _heroSkillIdDict = new Dictionary<int,int[]>()
		 {
			 {1000,new []{1004,1005,1007,1008,1010}}, // 鬼剑士
		 };
		 
		 public  void OnCreate()
		 {
		
		 }

		 public int[] GetHeroNormalSkillId(int heroId)
		 {
			 _heroNormalSkillIdDict.TryGetValue(heroId, out int[] skillIdArr);
			 return skillIdArr;
		 }

		 public int[] GetHeroSkillId(int heroId)
		 {
			 _heroSkillIdDict.TryGetValue(heroId, out int[] skillIdArr);
			 return skillIdArr;
		 }
		
		 public  void OnDestroy()
		 {
		
		 }
	
	}
}
