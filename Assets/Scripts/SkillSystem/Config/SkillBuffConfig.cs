using Sirenix.OdinInspector;

namespace SkillSystem.Config
{
    [System.Serializable]
    [HideMonoScript]
    public class SkillBuffConfig
    {
        [LabelText("附加buff的ID")]
        public int buffId;
        [LabelText("触发帧")]
        public int triggerFrame;
        
    }
}