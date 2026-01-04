using System.IO;
using SkillSystem.Buff.BuffCfg;
using SkillSystem.Config;
using UnityEditor;

namespace SkillSystem.Editor
{
    public class SkillEffectPathEditor
    {
        public static string[] SkillDataCfgPathArr = 
            new string[]{"Assets/GameData/Game/SkillSystem/SkillData/","Assets/GameData/Game/SkillSystem/BuffData/"};
        
        [MenuItem("Tool/SyncCFGPrefabsPaths")]
        public static void SyncCfgPrefabPath()
        {
            for (int i = 0; i < SkillDataCfgPathArr.Length; i++)
            {
                string path = SkillDataCfgPathArr[i];
                string[] filePathArr = Directory.GetFiles(path, "*");
                foreach (var filePath in filePathArr)
                {
                    if (filePath.EndsWith(".asset"))
                    {
                        if (i == 0)//技能配置
                        {
                            SkillDataConfig  skillData = AssetDatabase.LoadAssetAtPath<SkillDataConfig>(filePath);
                            skillData.skillCfg.GetObjectPath(skillData.skillCfg.skillHitEffect);
                            foreach (var effectCfg in skillData.effectCfgList)
                            {
                                effectCfg.GetObjectPath(effectCfg.skillEffect);
                            }
                            foreach (var bulletCfg in skillData.bulletList)
                            {
                                bulletCfg.GetBulletHitObjectPath(bulletCfg.hitEffect);
                                bulletCfg.GetBulletObjectPath(bulletCfg.bulletPrefab);
                            }
                            skillData.SaveAsset();
                        }
                        else //buff配置
                        {
                            BuffConfig  buffData = AssetDatabase.LoadAssetAtPath<BuffConfig>(filePath);
                            buffData.GetObjectPath(buffData.buffHitEffectObj);
                            if (buffData.effectConfig != null)
                            {
                                buffData.effectConfig.GetObjectPath(buffData.effectConfig.effect);
                            }
                            buffData.SaveAsset();
                        }
                        AssetDatabase.SaveAssets();
                    }
                }
            }
        }
    }
}