using FixIntPhysics;
using Sirenix.OdinInspector;
using SkillSystem.EditorWindow;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillDamageConfig
    {
        [LabelText("触发帧")]
        public int triggerFrame; // 触发帧

        [LabelText("结束帧")]
        public int endFrame;

        [LabelText("触发间隔")]
        [InfoBox("毫秒,value = 0 默认一次, >0则为间隔,一帧66ms")]
        public int triggerIntervalMS;

        [LabelText("是否跟随特效")]
        public bool isFollowEffect;
        
        [LabelText("伤害配置")]
        public DamageType damageType;

        [LabelText("伤害倍率")]
        public int damageRate;
        
        [LabelText("伤害检测方式")]
        [OnValueChanged("OnDetectionValueChanged")]
        public DamageDetectionMode damageDetectionMode;

        [LabelText("Box碰撞体检测大小")]
        [ShowIf("_showBox3D")]
        [OnValueChanged("OnColliderDataChanged")]
        public Vector3 boxSize = new Vector3(1, 1, 1);
        
        [LabelText("Box碰撞体偏移")]
        [ShowIf("_showBox3D")]
        [OnValueChanged("OnColliderDataChanged")]
        public Vector3 boxOffset = new Vector3(0, 0, 0);
        
        [LabelText("球体碰撞体偏移")]
        [ShowIf("_showSphere3D")]
        [OnValueChanged("OnColliderDataChanged")]
        public Vector3 sphereOffset = new Vector3(0, 0.9f, 0);

        [LabelText("球体碰撞体检测半径")]
        [ShowIf("_showSphere3D")]
        [OnValueChanged("OnColliderDataChanged")]
        public float radius = 1;
        
        [LabelText("球体碰撞体检测半径高度")]
        [ShowIf("_showSphere3D")]
        [OnValueChanged("OnColliderDataChanged")]
        public float radiusHeight = 0;
        
        [LabelText("碰撞体位置类型")]
        public ColliderPosType colliderPosType = ColliderPosType.FollowDir;
        
        [LabelText("伤害触发目标")]
        public TargetType targetType; //伤害触发目标

        [TitleGroup("附加的buff","伤害生效一瞬间，附加的buff")]
        public int[] addBuffs; // 技能附加buff

        [TitleGroup("触发技能ID","完成这个伤害后触发的其他技能ID")]
        public int triggerSkillID; //触发技能ID(完成这个伤害后触发的其他技能ID)
#if UNITY_EDITOR
        
        private bool _showBox3D;
        private bool _showSphere3D;
        private FixIntBoxCollider _boxCollider;
        private FixIntSphereCollider _sphereCollider;
        private int _curLogicFrame = 0;
        public void OnDetectionValueChanged(DamageDetectionMode mode)
        {
            _showBox3D = mode == DamageDetectionMode.Box3D;
            _showSphere3D = mode == DamageDetectionMode.Sphere3D;
            DestroyCollider();
            CreateCollider();
        }

        public void OnColliderDataChanged() 
        {
            if(_boxCollider!=null)
                _boxCollider.SetBoxData(GetColliderOffsetPos(),boxSize,colliderPosType == ColliderPosType.FollowPos);
            if(_sphereCollider != null)
                _sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
        }

        public void CreateCollider()
        {
            if (damageDetectionMode == DamageDetectionMode.Box3D)
            {
                _boxCollider = new FixIntBoxCollider(boxSize,  GetColliderOffsetPos());
                _boxCollider.SetBoxData(GetColliderOffsetPos(),boxSize,colliderPosType == ColliderPosType.FollowPos);
            }
            else if(damageDetectionMode == DamageDetectionMode.Sphere3D)
            {
                _sphereCollider = new FixIntSphereCollider(radius, GetColliderOffsetPos());
                _sphereCollider.SetBoxData(radius, GetColliderOffsetPos(), colliderPosType == ColliderPosType.FollowPos);
            }
        }

        public void DestroyCollider()
        {
            if (_boxCollider != null)
            {
                _boxCollider.OnRelease();
                _boxCollider = null;
            }

            if (_sphereCollider != null)
            {
                _sphereCollider.OnRelease();
                _sphereCollider = null;
            }
        }

        public Vector3 GetColliderOffsetPos()
        {
            var characterPos = SkillComplierWindow.GetCharacterPos();
            if (damageDetectionMode == DamageDetectionMode.Box3D)
            {
                return characterPos + boxOffset;
            }
            
            if(damageDetectionMode == DamageDetectionMode.Sphere3D)
            {
                return characterPos + sphereOffset;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 当窗口初始化
        /// </summary>
        public void OnInit()
        {
            CreateCollider();
        }

        /// <summary>
        /// 当窗口关闭
        /// </summary>
        public void OnRelease()
        {
            DestroyCollider();
        }

        public void PlaySkillStart()
        {
            _curLogicFrame = 0;
            DestroyCollider();
        }

        public void PlaySkillEnd()
        {
            DestroyCollider();
        }

        public void OnLogicFrameUpdate()
        {
            
            if (_curLogicFrame == triggerFrame)
            {
                CreateCollider();
            }
            else if (_curLogicFrame == endFrame)
            {
                DestroyCollider();
            }
            
            _curLogicFrame++;
        }
#endif
    }

    public enum DamageType
    {
        [LabelText("无伤害")]
        None,
        [LabelText("物理伤害")]
        ADDamage,
        [LabelText("魔法伤害")]
        APDamage,
    }

    public enum DamageDetectionMode
    {
        [LabelText("无配置")]
        None,
        [LabelText("3D盒子碰撞检测")]
        Box3D,
        [LabelText("3D球体碰撞检测")]
        Sphere3D,
        [LabelText("通过代码搜索半径的距离")]
        RadiusDistance,
        [LabelText("通过代码搜索所有目标")]
        AllTarget,
        [LabelText("3D圆柱碰撞检测")]
        Cylinder3D,
    }

    public enum ColliderPosType
    {
        [LabelText("跟随角色朝向")]
        FollowDir,
        [LabelText("跟随角色位置")]
        FollowPos,
        [LabelText("屏幕中心坐标")]
        CenterPos,
        [LabelText("目标位置")]
        TargetPos,
    }

    public enum TargetType
    { 
        [LabelText("无配置")]
        None,
        [LabelText("队友")]
        Teammate,
        [LabelText("敌人")]
        Enemy,
        [LabelText("自身")]
        Self,
        [LabelText("所有对象")]
        AllObject,
    }
    
}