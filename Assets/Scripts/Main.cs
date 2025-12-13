using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZM.AssetFrameWork;
using ZMGC.Battle;
using ZMGC.Hall;

public class Main : MonoBehaviour
{
    public static Main Instance;

    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ZMAssetsFrame.Instance.InitFrameWork();
        UIModule.Instance.Initialize();
        WorldManager.CreateWorld<HallWorld>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        WorldManager.OnUpdate();
    }


    public void StartGame()
    {
       
    }
    
    
}