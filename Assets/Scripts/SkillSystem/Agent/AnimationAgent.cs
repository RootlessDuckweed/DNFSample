#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace SkillSystem.Agent
{
    public class AnimationAgent
    {
#if UNITY_EDITOR
        private Animation _anim;
        private double _lastRunTime = 0;
        
        public void InitPlayAnim(Transform transform)
        {
            _anim = transform.GetComponentInChildren<Animation>();
            EditorApplication.update += OnUpdate;
        }
        
        public void OnUpdate()
        {
            if(_anim == null) return;
            if (_lastRunTime <= 0)
            {
                _lastRunTime = EditorApplication.timeSinceStartup;
            }
            var curRuntime = EditorApplication.timeSinceStartup - _lastRunTime;
            
            if (_anim is {clip: null}) return;
            
            float curAnimNormalizationValue = (float) curRuntime / _anim.clip.length;
                
            _anim.clip.SampleAnimation(_anim.gameObject, (float)curRuntime);
        }
        
        public void OnDestroy()
        {
            EditorApplication.update -= OnUpdate;
        }
#endif
    }
}