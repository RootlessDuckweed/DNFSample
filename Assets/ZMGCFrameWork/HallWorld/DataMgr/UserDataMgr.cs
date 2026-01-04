/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2025/11/18 23:55:14
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/

using System.Collections.Generic;

namespace ZMGC.Hall
{
	public  class UserDataMgr : IDataBehaviour
	{
		 public string UserName;
		 public int RoleId;
		 public List<int> CreateRoleIdList { get; private set; } =  new List<int>(){1000,1001};

		 public  void OnCreate()
		 {
			
		 }
		
		 public  void OnDestroy()
		 {
		
		 }
	
	}
}
