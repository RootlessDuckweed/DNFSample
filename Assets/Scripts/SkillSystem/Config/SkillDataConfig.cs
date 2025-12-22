using System.Collections.Generic;
using Sirenix.OdinInspector;
using SkillSystem.EditorWindow;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Config
{
    [CreateAssetMenu(fileName = "SkillDataConfig", menuName = "Config/SkillDataConfig", order = 0)]
    public class SkillDataConfig : ScriptableObject
    {
        public SkillCharacterConfig character;
        public SkillConfig skillCfg;
        public List<SkillDamageConfig> damageCfgList;
        public List<SkillEffectConfig>  effectCfgList;
        public List<SkillAudioConfig> audioList;
        public List<SkillActionConfig> actionList;
        public List<SkillBulletConfig> bulletList;
#if UNITY_EDITOR
        
        public static void SaveSkillData(SkillCharacterConfig character, SkillConfig skillCfg,
            List<SkillDamageConfig> damageCfgList, List<SkillEffectConfig> effectCfgList,
            List<SkillAudioConfig> audioCfgList, List<SkillActionConfig> actionCfgList,List<SkillBulletConfig> bulletCfgList)
        {
            var skillDataConfig = ScriptableObject.CreateInstance<SkillDataConfig>();
            skillDataConfig.character = character;
            skillDataConfig.skillCfg = skillCfg;
            skillDataConfig.damageCfgList = damageCfgList;
            skillDataConfig.effectCfgList = effectCfgList;
            skillDataConfig.audioList = audioCfgList;
            skillDataConfig.actionList = actionCfgList;
            skillDataConfig.bulletList = bulletCfgList;
            
            string assetPath = "Assets/GameData/Game/SkillSystem/SkillData/"+skillCfg.skillId+".asset";
            AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(skillDataConfig, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        [Button("配置技能",ButtonSizes.Large),GUIColor("green")]
        public  void ShowSkillWindow()
        {
            var window = SkillComplierWindow.ShowWindow();
            window.LoadSkillData(this);
        }
    }
#endif
}