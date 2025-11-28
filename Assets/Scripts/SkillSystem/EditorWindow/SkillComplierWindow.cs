using System;
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
        
        [MenuItem("Skill/技能编辑器")]
        public static SkillComplierWindow ShowWindow()
        {
            return GetWindowWithRect<SkillComplierWindow>(new Rect(0,0,1000,600));
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EditorApplication.update += OnEditorUpdate;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EditorApplication.update -= OnEditorUpdate;
        }

        public void OnEditorUpdate()
        {
            try
            {
                character.OnUpdate(Focus);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}