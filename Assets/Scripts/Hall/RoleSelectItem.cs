using UnityEngine;
using UnityEngine.UI;
using ZM.AssetFrameWork;

namespace Hall
{
    public class RoleSelectItem : MonoBehaviour
    {
        public Transform noSelectMask;
        public Image roleIconImage;
        public Image roleNameImage;
        public int roleId;
        public void SetItemData(int id)
        {
            roleId = id;
            roleIconImage.sprite =
                ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.HALL_TEXTURES_PATH + "CreateRole/p_UI_Creat",
                    GetHeroIconName());
            roleNameImage.sprite = 
                ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.HALL_TEXTURES_PATH + "CreateRole/p_UI_Creat",
                    GetHeroNameText());
        }

        public void SetSelectHideState(bool active)
        {
            noSelectMask.gameObject.SetActive(active);
        }
        
        public void OnRoleSelectButtonClick()
        {
            UIModule.Instance.GetWindow<CreateRoleWindow>()?.SelectRoleUpdate(roleId);
        }

        public string GetHeroIconName()
        {
            if (roleId == 1000)
            {
                return "UI_Chuangjiao_Guijianshi_Di";
            }
            else if (roleId == 1001)
            {
                return "UI_Chuangjiao_Shenqiangshou_Di";
            }
            return string.Empty;
        }
        
        public string GetHeroNameText()
        {
            if (roleId == 1000)
            {
                return "UI_Chuangjiao_Liemozhe_Zi";
            }
            else if (roleId == 1001)
            {
                return "UI_Chuangjiao_Shenqiangshou_Zi";
            }
            return string.Empty;
        }
    }
}