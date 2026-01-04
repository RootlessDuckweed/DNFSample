using System;
using Config;
using UnityEngine;
using UnityEngine.UI;
using ZM.AssetFrameWork;

namespace Game.Item
{
    public class MonsterBloodItem : MonoBehaviour
    {
        public Image headImage;
        public Image monsterTypeImage;
        public MultipleBloodBars bloodBars;
        public Text nameText;
        
        private MonsterCfg _monsterCfg;
        private int _monsterId;
        public int curShowMonsterInsId;//对象ID

        public void InitBloodData(MonsterCfg monsterCfg,int  curHp,int insId)
        {
            curShowMonsterInsId = insId;
            bloodBars.InitBlood(curHp);
            _monsterCfg = monsterCfg;
            _monsterId = monsterCfg.id;
            headImage.sprite =
                ZMAssetsFrame.LoadSprite(AssetPathConfig.GAME_TEXTURES_PATH + "HeadIcon/" + monsterCfg.id);
            monsterTypeImage.sprite =
                ZMAssetsFrame.LoadPNGAtlasSprite(AssetPathConfig.GAME_TEXTURES_PATH + "BattlePEV/p_UI_Battle_Pve",GetMonsterTypeName(monsterCfg));
            nameText.text = monsterCfg.name;
        }

        public void Damage(int damageHp)
        {
            bloodBars.ChangeBlood(damageHp);
        }

        public string GetMonsterTypeName(MonsterCfg monsterCfg)
        {
            switch (monsterCfg.type)
            {
                case MonsterType.Normal:
                    return "UI_Battle_Pve_Tubiao_Putong";
                    break;
                case MonsterType.Elite:
                    return "UI_Battle_Pve_Tubiao_Jingying";
                    break;
                case MonsterType.Boss:
                    return "UI_Battle_Pve_Tubiao_Lingzhu";
                    break; ;
            }

            return null;
        }
    }
}