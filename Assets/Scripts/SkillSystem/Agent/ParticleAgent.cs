#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#endif
namespace SkillSystem.Agent
{
    public class ParticleAgent
    {
#if UNITY_EDITOR
        private ParticleSystem[]  _particleSystems;
        private double _lastRunTime = 0;
        
        public void InitPlayAnim(Transform transform)
        {
            _particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
            EditorApplication.update += OnUpdate;
        }
        
        public void OnUpdate()
        {
            if (_lastRunTime <= 0)
            {
                _lastRunTime = EditorApplication.timeSinceStartup;
            }
            var curRuntime = EditorApplication.timeSinceStartup - _lastRunTime;
            if (_particleSystems != null)
            {
                foreach (var item in _particleSystems)
                {
                    if (item != null)
                    {
                        // 停止所以粒子动效
                        item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                        // 关闭由随机种子播放的动效
                        item.useAutoRandomSeed = false;
                        item.Simulate((float)curRuntime);
                    }
                }
            }
        }
        
        public void OnDestroy()
        {
            EditorApplication.update -= OnUpdate;
        }
#endif
    }
}