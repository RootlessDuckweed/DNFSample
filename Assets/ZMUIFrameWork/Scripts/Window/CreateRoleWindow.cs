/*---------------------------------
 *Title:UI表现层脚本自动化生成工具
 *Author:ZM 铸梦
 *Date:2025/11/18 0:47:33
 *Description:UI 表现层，该层只负责界面的交互、表现相关的更新，不允许编写任何业务逻辑代码
 *注意:以下文件是自动生成的，再次生成不会覆盖原有的代码，会在原有的代码上进行新增，可放心使用
---------------------------------*/

using System.Collections.Generic;
using DG.Tweening;
using Hall;
using Tools;
using UnityEngine.UI;
using UnityEngine;
using ZM.AssetFrameWork;
using ZMGC.Battle;
using ZMGC.Hall;
using ZMUIFrameWork;

	public class CreateRoleWindow:WindowBase
	{
	
		 public CreateRoleWindowDataComponent dataCompt;
		 private UserDataMgr _userDataMgr;
		 private List<RoleSelectItem> _roleSelectItems = new List<RoleSelectItem>();
		 private int _curSelectRoleId = 0;
		 private DOTweenAnimation _doTweenAnimation;
		 private GameObject _rolePortraitObj;
		 #region 声明周期函数
		 //调用机制与Mono Awake一致
		 public override void OnAwake()
		 {
			 dataCompt=gameObject.GetComponent<CreateRoleWindowDataComponent>();
			 mDisableAnim = true;
			 dataCompt.InitComponent(this);
			 base.OnAwake();
			 _userDataMgr = HallWorld.GetExitsDataMgr<UserDataMgr>();
			 _doTweenAnimation = dataCompt.RolePortraitRootTransform.GetComponent<DOTweenAnimation>();
			 for (int i = 0; i < _userDataMgr.CreateRoleIdList.Count; i++)
			 {
				 var obj = ZMAssetsFrame.Instantiate(AssetPathConfig.HALL_PREFABS_ITEM + "RoleSelectItem",
					 dataCompt.ContentTransform,Vector3.zero,Vector3.one, Quaternion.identity);
				 RoleSelectItem itemScript = obj.GetComponent<RoleSelectItem>();
				 itemScript.SetItemData(_userDataMgr.CreateRoleIdList[i]);
				 _roleSelectItems.Add(itemScript);
			 }
			 SelectRoleUpdate(_userDataMgr.CreateRoleIdList[0]);
		 }
		 //物体显示时执行
		 public override void OnShow()
		 {
			 base.OnShow();
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
		 
		 public void SelectRoleUpdate(int roleId)
		 {
			 _curSelectRoleId = roleId;
			 _userDataMgr.RoleId = roleId;
			 _userDataMgr.UserName = dataCompt.NameInputField.text;
			 Debug.Log(_userDataMgr.UserName);
			 HideAllItemSelect();
			 ShowItemSelect();

			 if (_rolePortraitObj != null)
			 {
				 ZMAssetsFrame.Release(_rolePortraitObj);
				 _rolePortraitObj = null;
			 }
			 
			 _rolePortraitObj = ZMAssetsFrame.Instantiate(AssetPathConfig.HALL_EFFECTS + "RulePortrait/" + roleId,dataCompt.RolePortraitRootTransform,
				 Vector3.zero, Vector3.one, Quaternion.identity);
			 dataCompt.RolePortraitRootTransform.localPosition = new Vector3(-1500, 0, 0);
			 _doTweenAnimation.DORestart();
		 }

		 private void HideAllItemSelect()
		 {
			 foreach (var item in  _roleSelectItems)
			 {
				 item.SetSelectHideState(true);
			 }
		 }
		 private void ShowItemSelect()
		 {
			 foreach (var item in  _roleSelectItems)
			 {
				 if (_curSelectRoleId == item.roleId)
				 {
					item.SetSelectHideState(false);
					break;
				 }
			 }
		 }
		 #endregion
		 #region UI组件事件
		 public void OnEnterGameButtonClick()
		 {
			 dataCompt.EnterGameButton.interactable=false;
			 HallWorld.EnterBattleWord();
		 }
		 public void OnNameInputEnd(string text)
		 {
			 HallWorld.GetExitsDataMgr<UserDataMgr>().UserName = text;
		 }
		 public void OnNameInputChange(string text)
		 {
		
		 }
		 public void OnCloseButtonClick()
		 {
			HideWindow();
		 }
		 #endregion
	}
