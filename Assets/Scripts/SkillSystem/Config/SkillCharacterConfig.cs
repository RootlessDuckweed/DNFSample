using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace SkillSystem.Config
{
    [HideMonoScript]
    [System.Serializable]
    public class SkillCharacterConfig
    {
        [AssetList]
        [LabelText("角色模型")]
        [PreviewField(70,ObjectFieldAlignment.Center)]
        [OnValueChanged("OnSelectCharacterChanged")]
        public GameObject character;

        [TitleGroup("技能渲染","所有英雄渲染数据都会在技能开始释放时触发")]
        [LabelText("技能动画")]
        public AnimationClip skillAnimClip;

        [BoxGroup("动画数据")]
        [ProgressBar(0,100,Height = 30)]
        [HideLabel]
        [OnValueChanged("OnAnimProgressChanged")]
        public short animProgress = 0;
        
        [LabelText("是否循环动画")]
        [BoxGroup("动画数据")]
        public bool isLoopAnim = false;
        
        [LabelText("动画循环次数")]
        [BoxGroup("动画数据")]
        [ShowIf("isLoopAnim")]
        public int loopCount = 0;
        
        [LabelText("逻辑帧数")]
        [BoxGroup("动画数据")]
        public int logicFrame = 0;
        
        [LabelText("动画长度")]
        [BoxGroup("动画数据")]
        public float animLength = 0;

        [FormerlySerializedAs("skillDuration")]
        [LabelText("技能推荐时长(ms)")]
        [BoxGroup("动画数据")]
        public int skillDurationMS = 0;
        
        private GameObject _tempCharacter;
        private bool _isPlayAnim = false;
        private double _lastRunTime = 0;
        private Animation _animationCache;
        private bool _isPaused = false;

        [Button("播放",ButtonSizes.Large)]
        [ButtonGroup("按钮数组")]
        public void Play()
        {
            if (character != null)
            {
                //先从场景中查找技能对象，如果查找不到，主动克隆一个
                string characterName = character.name;
                _tempCharacter = GameObject.Find(characterName);
                if (_tempCharacter == null)
                {
                    _tempCharacter = GameObject.Instantiate(character);
                }
                
                Animation animation = _tempCharacter.GetComponent<Animation>();
                _animationCache = animation;
                if (!animation?.GetClip(skillAnimClip.name))
                {
                    animation.AddClip(skillAnimClip, skillAnimClip.name);
                }

                float clipLength = skillAnimClip.length;
                animation.clip = skillAnimClip;
                //计算长度
                animLength = isLoopAnim ? clipLength * loopCount : clipLength;
                //计算所需的逻辑帧数
                logicFrame = (int)(isLoopAnim ? clipLength/0.066f * loopCount:  clipLength/0.066f);
                //计算时长ms
                skillDurationMS =(int)(1000 * animLength);

                _lastRunTime = 0;
                _isPlayAnim = true;
            }
        }
        
        [Button("暂停",ButtonSizes.Large)]
        [ButtonGroup("按钮数组")]
        public void Pause()
        {
            _isPlayAnim = false;
        }

        [Button("保存",ButtonSizes.Large)]
        [ButtonGroup("按钮数组")]
        public void SaveAssets()
        {
            
        }

        public void OnUpdate(Action OnProgressUpdateCallback)
        {
            if (_isPlayAnim)
            {
                if (_lastRunTime <= 0)
                {
                    _lastRunTime = EditorApplication.timeSinceStartup;
                }
                var curRuntime = EditorApplication.timeSinceStartup - _lastRunTime;
                float curAnimNormalizationValue = (float) curRuntime / animLength;
                animProgress = (short)Mathf.Clamp(curAnimNormalizationValue*100, 0, 100);
                logicFrame = (int)(curRuntime / LogicFrameConfig.LogicFrameInterval);
                _animationCache.clip.SampleAnimation(_tempCharacter, (float)curRuntime);
                
                if (animProgress >= 100)
                {
                    PlaySkillEnd();
                }
                
                
                
                OnProgressUpdateCallback?.Invoke();
            }
        }

        public void PlaySkillEnd()
        {
            _isPlayAnim = false;
        }

        public void OnAnimProgressChanged(float value)
        {
            //先从场景中查找技能对象，如果查找不到，主动克隆一个
            string characterName = character.name;
            if (characterName != _tempCharacter.name || _tempCharacter == null)
            {
                _tempCharacter = GameObject.Find(characterName);
                if (_tempCharacter == null)
                {
                    _tempCharacter = GameObject.Instantiate(character);
                }
                _animationCache = _tempCharacter.GetComponent<Animation>();
            }
            float progressValue = value / 100 * skillAnimClip.length;
            logicFrame = (int)(progressValue / LogicFrameConfig.LogicFrameInterval);
            _animationCache.clip.SampleAnimation(_tempCharacter,progressValue);
        }

        public void OnSelectCharacterChanged(GameObject ch)
        {
            character = ch;
        }
    }
}