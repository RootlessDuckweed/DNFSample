using RenderLayer;
using Sirenix.OdinInspector;
using SkillSystem.Agent;
using SkillSystem.EditorWindow;
using UnityEngine;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillEffectConfig
    {
        [LabelText("技能特效")]
        [AssetList]
        [PreviewField(70, ObjectFieldAlignment.Left)]
        public GameObject skillEffect;

        [LabelText("触发帧")]
        public int triggerFrame;

        [LabelText("结束帧")]
        public int endFrame;
        
        [LabelText("特效偏移位置")]
        public Vector3 effectOffsetPosition;
        
        [LabelText("特效位置类型")]
        public EffectPosType effectPosType;
        
        [ToggleGroup("isSetTransformParent","是否设置特效父节点")]
        public bool isSetTransformParent;
        
        [ToggleGroup("isSetTransformParent")]
        [LabelText("节点类型")]
        public TransformParentType transformParentType;

        [ToggleGroup("isAttachDamage","是否附加伤害")]
        public bool isAttachDamage;
        [ToggleGroup("isAttachDamage","是否附加伤害")]
        public SkillDamageConfig damageConfig;

        [ToggleGroup("isAttachAction", "是否附加行动(当特效位置类型为 跟随角色方向 起作用)"),ShowIf("EnableAttachAction")]
        public bool isAttachAction;

        [ToggleGroup("isAttachAction","是否附加行动"),ShowIf("EnableAttachAction")]
        public SkillActionConfig actionConfig;
        
        private bool EnableAttachAction
        {
            get
            {
                if (effectPosType != EffectPosType.FollowDir)
                {
                    isAttachAction = false;
                }
                return effectPosType == EffectPosType.FollowDir;
            }
        }

#if UNITY_EDITOR
        
        private GameObject _cloneGameObject;
        private int _curLogicFrame;
        private AnimationAgent _animationAgent;
        private ParticleAgent _particleAgent;
        
            
        public void StartPlaySkill()
        {
            _curLogicFrame = 0;
            DestroyEffect();
            
        }

        public void PlaySkillEnd()
        {
            DestroyEffect();
        }

        public void OnLogicFrameUpdate()
        {
            if (_curLogicFrame == triggerFrame)
            {
                CreateEffect();
            }
            else if (_curLogicFrame == endFrame)
            {
                DestroyEffect();
            }
            _curLogicFrame++;
        }
        
        public void SkillPause()
        {
            DestroyEffect();
        }

        /// <summary>
        /// 创建特效
        /// </summary>
        public void CreateEffect()
        {
            if (skillEffect != null)
            {
                var renderObject = SkillComplierWindow.GetWindow().character.tempCharacter.GetComponentInChildren<RenderObject>();
                Transform parent = null;
                if (isSetTransformParent)
                {
                    parent = renderObject.GetEffectParent(transformParentType);
                }
                _cloneGameObject = GameObject.Instantiate(skillEffect,parent);
                if (isSetTransformParent)
                {
                    _cloneGameObject.transform.localPosition = effectPosType == EffectPosType.Zero ? Vector3.zero : effectOffsetPosition;
                }
                else
                {
                    _cloneGameObject.transform.position = SkillComplierWindow.GetCharacterPos()+effectOffsetPosition;
                }
                _animationAgent = new AnimationAgent();
                _particleAgent = new ParticleAgent();
                
                _animationAgent.InitPlayAnim(_cloneGameObject.transform);
                _particleAgent.InitPlayAnim(_cloneGameObject.transform);
            }
        }
        
        
        /// <summary>
        /// 销毁特效
        /// </summary>
        public void DestroyEffect()
        {
            if (_cloneGameObject != null)
            {
                GameObject.DestroyImmediate(_cloneGameObject);
            }
            if (_animationAgent != null)
            {
                _animationAgent.OnDestroy();
            }

            if (_particleAgent != null)
            {
                _particleAgent.OnDestroy();
            }
        }
#endif
    }

    public enum EffectPosType
    {
        [LabelText("跟随角色位置和方向")]
        FollowPosDir,
        [LabelText("跟随角色方向")]
        FollowDir,
        [LabelText("屏幕中心位置")]
        CenterPos,
        [LabelText("引导位置")]
        GuidePos,
        [LabelText("跟随特效移动位置")]
        FollowEffectMovePos,
        [LabelText("位置旋转归零不动")]
        Zero,
    }

    public enum TransformParentType
    {
        [LabelText("无配置")]
        None,
        [LabelText("左手")]
        LeftHand,
        [LabelText("右手")]
        RightHand,
    }
}