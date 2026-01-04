using Config;
using Game.Action;
using Game.Timer;
using SkillSystem.Buff;
using SkillSystem.Config;
using UnityEngine;
using ZM.AssetFrameWork;

namespace ZMGC.Battle
{
    public class BattleWorld : World
    {
        /// <summary>
        /// 英雄控制器
        /// </summary>
        public HeroLogicCtrl HeroLogicCtrl { get; private set; }
        /// <summary>
        /// 怪物控制器
        /// </summary>
        public MonsterLogicCtrl MonsterLogicCtrl { get; private set; }

        private float _accLogicRuntime;
        private float _nextLogicFrameTime;
        public float LogicDeltaTime;
        
        public override void OnCreate()
        {
            base.OnCreate();
            ConfigCenter.Instance.InitGameCfg();
            HeroLogicCtrl = BattleWorld.GetExitsLogicCtrl<HeroLogicCtrl>();
            MonsterLogicCtrl = BattleWorld.GetExitsLogicCtrl<MonsterLogicCtrl>();
            HeroLogicCtrl.InitHero();
            MonsterLogicCtrl.InitMonster();
            BuffSystem.Instance.OnCreate();
            UIModule.PopUpWindow<BattleWindow>();
            AudioController.GetInstance().PlayMusicFade(AssetPathConfig.GAME_AUDIO_PATH+"BG/jizhou.mp3",2);
        }

        /// <summary>
        /// unity渲染帧更新，模拟逻辑帧更新
        /// </summary>
        public override void OnUpdate()
        {
            base.OnUpdate();
            _accLogicRuntime += Time.deltaTime;
            
            // 逻辑帧更新 
            // 控制帧数，保证所有设备的逻辑帧帧数一致
            // TODO:模拟逻辑帧更新，后续逻辑帧的更新将由服务端消息推送
            while (_accLogicRuntime > _nextLogicFrameTime)
            {
                OnLogicFrameUpdate();
                // 计算下一帧
                _nextLogicFrameTime += LogicFrameConfig.LogicFrameInterval;
                LogicFrameConfig.LogicFrameId++;
            }

            LogicDeltaTime = (_accLogicRuntime + LogicFrameConfig.LogicFrameInterval - _nextLogicFrameTime) /
                             LogicFrameConfig.LogicFrameInterval;
        }

        /// <summary>
        /// 逻辑帧更新（后期通过服务端进行调用）
        /// </summary>
        private void OnLogicFrameUpdate()
        {
           HeroLogicCtrl.OnLogicFrameUpdate();
           MonsterLogicCtrl.OnLogicFrameUpdate();
           LogicActionController.Instance.OnLogicFrameUpdate();
           BuffSystem.Instance.OnLogicFrameUpdate();
           LogicTimerManager.Instance.OnLogicFrameUpdate();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            HeroLogicCtrl.OnDestroy();
            MonsterLogicCtrl.OnDestroy();
            LogicActionController.Instance.OnDestroy();
            BuffSystem.Instance.OnDestroy();
            LogicTimerManager.Instance.OnDestroy();
        }
        
        public override void OnDestroyPostProcess(object args)
        {
            base.OnDestroyPostProcess(args);
        }
    }
}