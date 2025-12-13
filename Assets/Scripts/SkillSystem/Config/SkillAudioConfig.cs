using Sirenix.OdinInspector;
using UnityEngine;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillAudioConfig
    {
        [AssetList] 
        [BoxGroup("音效文件"), PreviewField(70, ObjectFieldAlignment.Left),OnValueChanged("OnAudioChanged")]
        public AudioClip skillAudio;

        [LabelText("音效文件名称")]
        [BoxGroup("音效文件"),GUIColor("green")]
        public string audioName;

        [BoxGroup("参数配置")]
        [LabelText("触发帧"),GUIColor("green")]
        public int triggerFrame;
        
        [ToggleGroup("isLoop","是否循环")]
        public bool isLoop;
        
        [ToggleGroup("isLoop","结束帧")]
        public int endFrame;

        public void OnAudioChanged()
        {
            if (skillAudio != null)
            {
                audioName = skillAudio.name;
            }
        }
    }
}