using Config;
using FixMath;
using Game.Timer;
using LogicLayer;
using LogicLayer.Monster;
using SkillSystem.Runtime;
using Tools;
using UnityEngine;
using ZM.AssetFrameWork;

namespace RenderLayer
{
    public class MonsterRender : RenderObject
    {
        private Animation _anim;
        private string _curAnimName;
        private int _monsterId;
        private MonsterCfg _monsterCfg;
        private MonsterLogic _monsterLogic;
        public override void OnCreate()
        {
            base.OnCreate();
            _anim = GetComponentInChildren<Animation>();
            _monsterLogic = LogicObj as MonsterLogic;
            _monsterId = _monsterLogic.MonsterId;
            _monsterCfg = ConfigCenter.Instance.GetMonsterCfgById(_monsterId);
        }

        public override void PlayAnim(string clipName)
        {
            base.PlayAnim(clipName);
            if (_anim == null)
            {
                return;
            }

            //怪物死亡只能播放死亡动画
            if (LogicObj.ObjectState == LogicObjectState.Death && !string.Equals(clipName,AnimationName.Anim_Dead))
            {
                return;
            }
            
            
            if (_anim.GetClip(clipName) != null)
            {
                _anim.Play(clipName);
                _curAnimName = clipName;
            }
            else
            {
                Debug.Log(gameObject.name+" 不存在这个动画片段:"+clipName);
            }
        }

        public override string GetCurAnimName()
        {
            return _curAnimName;
        }

        public override void OnHit(string effectPath, int survivalTimeMs, LogicObject source,LogicObject effectPoint)
        {
            base.OnHit(effectPath, survivalTimeMs, source, effectPoint);
            AudioClip audioClip = null;
            if (_monsterId == 20001) // 哥布林
            {
                audioClip = ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUDIO_PATH + "Gebulin/GoblinAttackC.wav");
            }
            else if (_monsterId == 20005) // 蜘蛛
            {
                audioClip =ZMAssetsFrame.LoadAudio(AssetPathConfig.GAME_AUDIO_PATH + "zhizu/NorthrendGhoulWound1.wav");
            }

            if (audioClip != null)
            {
                AudioController.GetInstance().PlaySoundByAudioClip(audioClip,false,2);
            }
        }

        public override void Damage(int damageValue, DamageSource damageSource, LogicObject source)
        {
            base.Damage(damageValue, damageSource, source);
            if (_monsterLogic.HP >= FixInt.Zero)
            { 
                UIModule.Instance.GetWindow<BattleWindow>().
                    ShowMonsterDamage(_monsterCfg,gameObject.GetInstanceID(),_monsterLogic.HP+damageValue,damageValue);
                
            }
        }

        public override void OnDeath()
        {
            base.OnDeath();
            PlayAnim(AnimationName.Anim_Dead);
            LogicTimerManager.Instance.DelayCall(new FixInt(1536L), () =>
            {
                ZMAssetsFrame.Release(gameObject);
            });
        }

        public override void OnRelease()
        {
            base.OnRelease();
        }
    }
}