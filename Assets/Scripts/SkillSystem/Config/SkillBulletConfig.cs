using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillBulletConfig
    {
        [AssetList,LabelText("特效"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("GetBulletObjectPath")]
        public GameObject bulletPrefab;
        [ReadOnly] public string bulletPrefabPath;
        [LabelText("智能锁定寻敌")]
        public bool intelligentAttack;
        [LabelText("触发帧")]
        public int triggerFrame;
        [LabelText("是否循环创建"),BoxGroup("循环创建参数")]
        public bool isLoopCreate;
        [LabelText("循环间隔ms"),ShowIf("isLoopCreate"),BoxGroup("循环创建参数")]
        public int loopIntervalMs;
        [LabelText("最小随机波动范围"),ShowIf("isLoopCreate"),BoxGroup("循环创建参数")]
        public Vector3 minRandomRangeVect3;
        [LabelText("最大随机波动范围"),ShowIf("isLoopCreate"),BoxGroup("循环创建参数")]
        public Vector3 maxRandomRangeVect3;
        [LabelText("移动速度")]
        public float moveSpeed;
        [LabelText("存活时间ms")]
        public int survivalTimeMs;
        [LabelText("重力加速度")]
        public Vector2 gravitySpeed;
        [LabelText("发射位置偏移")]
        public Vector3 offset;
        [LabelText("发射方向偏移")]
        public Vector3 dirOffset;
        [LabelText("发射角度")]
        public Vector3 angle;
        [LabelText("是否击中销毁")]
        public bool isHitDestroy;
        [LabelText("击中特效"),PreviewField(70,ObjectFieldAlignment.Left),OnValueChanged("GetBulletHitObjectPath")]
        public GameObject hitEffect;
        [ReadOnly] 
        public string hitEffectPath;
        [LabelText("击中存活时间")]
        public int hitEffectSurvivalTimeMs = 3000;
        [LabelText("击中音效")]
        public AudioClip hitAudio;
        [ToggleGroup("isAttachDamage","是否附加伤害")]
        public bool isAttachDamage = false;
        [ToggleGroup("isAttachDamage","是否附加伤害")]
        public SkillDamageConfig damageCfg;
        
#if UNITY_EDITOR        
        public void GetBulletHitObjectPath(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }
            hitEffectPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        }
#endif
                
#if UNITY_EDITOR        
        public void GetBulletObjectPath(GameObject obj)
        {
            bulletPrefabPath = UnityEditor.AssetDatabase.GetAssetPath(obj);
        }
#endif
    }
}