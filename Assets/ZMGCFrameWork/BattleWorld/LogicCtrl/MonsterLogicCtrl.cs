/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/12/2 23:41:12
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;
using FixIntPhysics;
using FixMath;
using LogicLayer.Monster;
using RenderLayer;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
	public  class MonsterLogicCtrl : ILogicBehaviour
	{
		 public List<MonsterLogic> MonsterList = new List<MonsterLogic>();
		 /// <summary>
		 /// 怪物位置列表
		 /// </summary>
		 public List<Vector3> monsterPosList = new List<Vector3>()
		 {
			new Vector3( 0, 0, 0),
			new Vector3( 1, 0, 0),
		 };

		 public int[] monsterIDArr = new[]
		 {
			 20001,
			 20004,
		 };
	
		 public  void OnCreate()
		 {
		
		 }

		 public void InitMonster()
		 {
			 int index = 0;
			 foreach (var id in monsterIDArr)
			 {
				 var initPos = new FixIntVector3(monsterPosList[index]);
				 var monsterObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_MONSTER + id,null);
				
				 //处理怪物碰撞数据
				 var boxInfo = monsterObj.GetComponent<BoxColliderGizmo>();
				 boxInfo.enabled = false;
				 var monsterBox = new FixIntBoxCollider(boxInfo.mSize, boxInfo.mSize);
				 monsterBox.SetBoxData(boxInfo.mConter,boxInfo.mSize);
				 
				 //TODO:后续需要修改
				 monsterBox.UpdateColliderInfo(initPos,new FixIntVector3(boxInfo.mSize));
				 
				 // 创建怪物逻辑层与渲染层
				 MonsterRender monsterRender = monsterObj.GetComponent<MonsterRender>();
				 MonsterLogic monsterLogic = new MonsterLogic(id, monsterRender,monsterBox,initPos);
				 monsterRender.SetLogicObject(monsterLogic);
				 
				 monsterLogic.OnCreate();
				 monsterRender.OnCreate();
				 
				 MonsterList.Add(monsterLogic);
				 
				 index++;
			 }
		 }
		 
		 public void OnLogicFrameUpdate()
		 {
			 foreach (var monsterLogic in MonsterList)
			 {
				 monsterLogic.OnLogicFrameUpdate();
			 }
		 }
		
		 public  void OnDestroy()
		 {
		
		 }
		 
		 
	
	}
}
