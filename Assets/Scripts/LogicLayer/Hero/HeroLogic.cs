using System.Collections.Generic;
using Config;
using FixMath;
using RenderLayer;
using SkillSystem.Runtime.Logic;
using UnityEngine;

namespace LogicLayer.Hero
{
    public class HeroLogic : LogicActor
    {
        public int HeroID { get; private set; }
        
        public HeroLogic(int heroID, RenderObject renderObj)
        {
            HeroID = heroID;
            RenderObj = renderObj;
            ObjectType = LogicObjectType.Hero;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            InitActorSkill(HeroID);
            InitHeroAttribute();
        }

        private void InitHeroAttribute()
        {
            HeroDataCfg data = ConfigCenter.Instance.GetHeroCfgById(HeroID);
            if (data == null)
            {
                Debug.LogError("heroId not found");
                return;
            }
            hp = data.hp;
            mp = data.mp;
            ap = data.ap;
            ad = data.ad;
            adDef = data.adDef;
            apDef = data.apDef;
            pct = data.pct;
            mct = data.mct;
            adPctRate = data.adPctRate;
            apMctRate = data.apMctRate;
            str = data.str;
            sta = data.sta;
            Int = data.Int;
            spi = data.spi;
            agl = data.agl;
        } 
        

        public override void OnLogicFrameUpdate()
        {
            base.OnLogicFrameUpdate();
            
        }
    }
}