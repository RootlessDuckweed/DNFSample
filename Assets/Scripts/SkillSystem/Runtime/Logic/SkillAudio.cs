namespace SkillSystem.Runtime
{
    public partial class Skill
    {
        /// <summary>
        /// 音效逻辑帧更新
        /// </summary>
        public void OnLogicFrameUpdateAudio()
        {
            if (_skillDataConfig.audioList is { Count: > 0 })
            {
                foreach (var item in _skillDataConfig.audioList)
                {
                    if (item.triggerFrame == _curLogicFrame)
                    {
                        // 播放音效
                        AudioController.GetInstance().PlaySoundByAudioClip(item.skillAudio,item.isLoop,100);
                    }
                    
                    // 如果当前是循环音效并且已经到了结束帧
                    if (item.endFrame == _curLogicFrame && item.isLoop)
                    {
                        // 停止循环音效
                        AudioController.GetInstance().StopSound(item.skillAudio);
                    }
                }
            }
        }

        public void PlayHitAudio()
        {
            AudioController.GetInstance().PlaySoundByAudioClip(_skillDataConfig.skillCfg.skillHitAudio, false, 100);
        }
    }
}