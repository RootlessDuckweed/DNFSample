using System;
using System.Collections;
using UnityEngine.SceneManagement;
using ZM.AssetFrameWork;
using ZMGC.Battle;

namespace Tools
{
    public class LoadSceneManager : MonoSingleton<LoadSceneManager>
    {
        public void LoadSceneAsync(string sceneName, Action OnLoadComplete = null)
        {
            UIModule.Instance.PopUpWindow<LoadingWindow>();
            StartCoroutine(AsyncLoadScene(sceneName, OnLoadComplete));
        }

        IEnumerator AsyncLoadScene(string sceneName, Action OnLoadComplete = null)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            float currentProgress = 0f;
            float maxProgress = 100f;
            while (currentProgress < 90f)
            {
                currentProgress = operation.progress * 100f;
                UIEventControl.DispensEvent(UIEventEnum.SceneLoadProgressUpdate, currentProgress );
                yield return null;
            }

            while (currentProgress < maxProgress)
            {
                currentProgress++;
                UIEventControl.DispensEvent(UIEventEnum.SceneLoadProgressUpdate, currentProgress );
                yield return null;
            }
        
            operation.allowSceneActivation = true;
            yield return null;
            OnLoadComplete?.Invoke();
        }
    }
}