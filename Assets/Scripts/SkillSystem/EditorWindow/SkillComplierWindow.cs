using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using SkillSystem.Config;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.EditorWindow
{
    public class SkillComplierWindow : OdinEditorWindow
    {
        [TabGroup("Skill","模型动画数据",SdfIconType.PersonFill)]
        public SkillCharacterConfig character = new SkillCharacterConfig();
        
        [TabGroup("SkillComplier","Skill",SdfIconType.Robot,TextColor = "lightmagenta")]
        public SkillConfig skillCfg = new SkillConfig();
        
        [TabGroup("SkillComplier","Effect",SdfIconType.OpticalAudio,TextColor = "blue")]
        public List<SkillEffectConfig> effectList = new List<SkillEffectConfig>();
        
        [TabGroup("SkillComplier","Damage",SdfIconType.Activity,TextColor = "red")]
        public List<SkillDamageConfig> damageList = new List<SkillDamageConfig>();
        
        [TabGroup("SkillComplier","Audio",SdfIconType.OpticalAudio,TextColor = "blue")]
        public List<SkillAudioConfig> audioList = new List<SkillAudioConfig>();
        
        [TabGroup("SkillComplier","Bullet",SdfIconType.OpticalAudio,TextColor = "cyan")]
        public List<SkillBulletConfig> bulletList = new List<SkillBulletConfig>();
        
        [TabGroup("SkillComplier","Action",SdfIconType.Activity,TextColor = "orange")]
        public List<SkillActionConfig> actionList = new List<SkillActionConfig>();
        
#if UNITY_EDITOR
        private bool _isStartPlaySkill = false;
        private float _accLogicRunTime = 0;
        private float _nextLogicFrameTime = 0;
        private double _lastUpdateTime = 0;
        
        [MenuItem("Skill/技能编辑器")]
        public static SkillComplierWindow ShowWindow()
        {
            return GetWindowWithRect<SkillComplierWindow>(new Rect(0,0,1000,600));
        }
        public void SaveSkillData()
        {
            SkillDataConfig.SaveSkillData(character, skillCfg, damageList,
                effectList, audioList,actionList,bulletList);
            Close();
        }

        public void LoadSkillData(SkillDataConfig skillDataConfig)
        {
            character =skillDataConfig.character;
            skillCfg = skillDataConfig.skillCfg;
            damageList = skillDataConfig.damageCfgList;
            effectList =skillDataConfig.effectCfgList;
            audioList = skillDataConfig.audioList;
            actionList = skillDataConfig.actionList;
            bulletList = skillDataConfig.bulletList;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var item in damageList)
            {
                item.OnInit();
            }
            
            EditorApplication.update += OnEditorUpdate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            foreach (var item in damageList)
            {
                item.OnRelease();
            }
            
            EditorApplication.update -= OnEditorUpdate;
        }

        public void OnEditorUpdate()
        {
            try
            {
                character.OnUpdate(Focus);
                if (_isStartPlaySkill)
                {
                    OnLogicUpdate();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }


        public void OnLogicUpdate()
        {
            if (_lastUpdateTime == 0)
            {
                _lastUpdateTime = EditorApplication.timeSinceStartup;
            }
            _accLogicRunTime =(float)(EditorApplication.timeSinceStartup - _lastUpdateTime);
            while (_accLogicRunTime > _nextLogicFrameTime)
            {
                OnLogicFrameUpdate();
                _nextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
            }
            
        }
        

        private void OnLogicFrameUpdate()
        {
            foreach (var item in effectList)
            {
                item.OnLogicFrameUpdate();
            }
            
            foreach (var item in damageList)
            {
                item.OnLogicFrameUpdate();
            }
        }

        public void StartPlaySkill()
        {
            foreach (var item in effectList)
            {
                item.StartPlaySkill();
            }
            
            foreach (var item in damageList)
            {
                item.PlaySkillStart();
            }
            
            _accLogicRunTime = 0;
            _nextLogicFrameTime = 0;
            _lastUpdateTime = 0;
            _isStartPlaySkill = true;
        }

        public void PlaySkillEnd()
        {
            foreach (var item in effectList)
            {
                item.PlaySkillEnd();
            }

            foreach (var item in damageList)
            {
                item.PlaySkillEnd();
            }

            _accLogicRunTime = 0;
            _nextLogicFrameTime = 0;
            _lastUpdateTime = 0;
            _isStartPlaySkill = false;
        }

        public void SkillPause()
        {
            foreach (var item in effectList)
            {
                item.SkillPause();
            }
        }
        
        public static Vector3 GetCharacterPos()
        {
            var window = GetWindow<SkillComplierWindow>();
            if (window.character.skillCharacter != null)
            {
                return window.character.skillCharacter.transform.position;
            }
            return Vector3.zero;
        }

        public static SkillComplierWindow GetWindow()
        {
            return GetWindow<SkillComplierWindow>();
        }
        
#endif
    }
}
