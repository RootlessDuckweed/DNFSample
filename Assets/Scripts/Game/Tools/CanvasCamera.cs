using System;
using UnityEngine;

namespace Game.Tools
{
    public class CanvasCamera : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Canvas>().worldCamera = UIModule.Instance.mUICamera;
        }
    }
}