/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/12/4 20:44:35
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using LogicLayer.Hero;
using SkillSystem.Runtime;
using UnityEngine.UI;
using UnityEngine;
using ZM.AssetFrameWork;
using ZMGC.Battle;
using ZMUIFrameWork;
using ZMUIFrameWork.Scripts.Item;

public class BattleWindow:WindowBase
	{
	
		 public BattleWindowDataComponent dataCompt;
		 private HeroLogic _heroLogic;
		 //技能按钮父节点节点列表
		 private readonly List<Transform> _skillItemRootList = new List<Transform>();
		 //技能按钮列表
		 private readonly List<SkillItem>  _skillItemList = new List<SkillItem>();
		 
		 #region 声明周期函数
		 //调用机制与Mono Awake一致
		 public override void OnAwake()
		 {
			 dataCompt=gameObject.GetComponent<BattleWindowDataComponent>();
			 dataCompt.InitComponent(this);
			 base.OnAwake();
			 for (int i = 0; i < dataCompt.SkillRootTransform.childCount; i++)
			 {
				 _skillItemRootList.Add(dataCompt.SkillRootTransform.GetChild(i));
			 }
		 }
		 //物体显示时执行
		 public override void OnShow()
		 {
			 base.OnShow();
			 _heroLogic = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>().HeroLgc;
			 // 遍历角色技能数组，生成对应的技能按钮
			 int[] heroSkillIDArr = BattleWorld.GetExitsDataMgr<HeroDataMgr>().GetHeroSkillId(_heroLogic.HeroID);
			 
			 for (var index = 0; index < heroSkillIDArr.Length; index++)
			 {
				 var id = heroSkillIDArr[index];
				 GameObject skillItemObj = ZMAssetsFrame.Instantiate(AssetPathConfig.GAME_PREFABS + "Item/SkillItem",_skillItemRootList[index]);
				 SkillItem  skillItem = skillItemObj.GetComponent<SkillItem>();
				 _skillItemList.Add(skillItem);
				 skillItem.SetItemSkillData(_heroLogic.GetSkill(id),_heroLogic);
				 skillItem.transform.localPosition = Vector3.zero;
				 skillItem.transform.localRotation = Quaternion.identity;
				 skillItem.transform.localScale = Vector3.one;
			 }
		 }
		 //物体隐藏时执行
		 public override void OnHide()
		 {
			 base.OnHide();
		 }
		 //物体销毁时执行
		 public override void OnDestroy()
		 {
			 base.OnDestroy();
		 }
		 #endregion
		 #region API Function
		    
		 #endregion
		 #region UI组件事件
		 public void OnNormalAttackButtonClick()
		 {
			_heroLogic.ReleaseNormalAttack();
		 }
		 public void OnCloseButtonClick()
		 {
		
			HideWindow();
		 }
		 #endregion
	}
