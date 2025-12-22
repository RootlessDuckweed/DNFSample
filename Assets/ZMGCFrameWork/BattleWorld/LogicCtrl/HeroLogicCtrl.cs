/*--------------------------------------------------------------------------------------
* Title: 业务逻辑脚本自动生成工具
* Author: 铸梦xy
* Date:2025/12/2 23:36:35
* Description:业务逻辑层,主要负责游戏的业务逻辑处理
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using LogicLayer.Hero;
using RenderLayer;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
	public  class HeroLogicCtrl : ILogicBehaviour
	{
		public HeroLogic HeroLgc { get; private set; }

		public  void OnCreate()
		{
			
		}

		public void InitHero()
		{
			GameObject heroObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS_HERO + "1001",null);
			HeroRender heroRender = heroObj.GetComponent<HeroRender>();
			HeroLogic heroLogic = new HeroLogic(1001, heroRender);
			HeroLgc = heroLogic;
			heroRender.SetLogicObject(heroLogic);
			heroLogic.OnCreate();
			heroRender.OnCreate();
		}

		public void OnLogicFrameUpdate()
		{
			HeroLgc.OnLogicFrameUpdate();
		}
		
		public  void OnDestroy()
		{
		
		}
	
	}
}
