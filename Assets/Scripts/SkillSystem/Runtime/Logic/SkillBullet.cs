using System.Collections.Generic;
using FixMath;
using SkillSystem.Config;
using SkillSystem.Runtime.Logic;
using SkillSystem.Runtime.Render;
using UnityEngine;
using ZM.AssetFrameWork;

namespace SkillSystem.Runtime
{
    public partial class Skill
    {

        private List<int> _curCreateBulletAccTimeList = new List<int>();
        private LogicRandom _logicRandom;
        /// <summary>
        /// 初始化子弹相关的数据
        /// </summary>
        private void OnBulletInit()
        {
            _logicRandom =  new LogicRandom(10);
            if (_skillDataConfig.bulletList is { Count: > 0 })
            {
                for (int i = 0; i < _skillDataConfig.bulletList.Count; i++)
                {
                    _curCreateBulletAccTimeList.Add(0);
                }
            }
        }
        
        public void OnLogicFrameUpdateBullet()
        {
            if (_skillDataConfig.bulletList is { Count: > 0 })
            {

                for (int i = 0; i < _skillDataConfig.bulletList.Count; i++)
                {
                   _curCreateBulletAccTimeList[i] += LogicFrameConfig.LogicFrameIntervalMs;
                    SkillBulletConfig item = _skillDataConfig.bulletList[i];
                    if (item.triggerFrame == _curLogicFrame)
                    {
                        CreateBullet(item);
                    }

                    if (item.isLoopCreate)
                    {
                        if (item.loopIntervalMs <= 0)
                        {
                            Debug.LogError("子弹的生成间隔<=0!");
                            continue;
                        }
                        while (_curCreateBulletAccTimeList[i] >= item.loopIntervalMs)
                        {
                            CreateBullet(item);
                            _curCreateBulletAccTimeList[i] -= item.loopIntervalMs;
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// 创建子弹
        /// </summary>
        /// <param name="config">子弹配置</param>
        private void CreateBullet(SkillBulletConfig config)
        {
            // 简单创建，减少对框架依赖，目的是为移植简单
            //GameObject bulletObj = GameObject.Instantiate(config.bulletPrefab);
            GameObject bulletObj = ZMAssetsFrame.Instantiate(config.bulletPrefabPath, null);
            
            //绑定逻辑层和渲染层
            SkillBulletRender bulletRender = bulletObj.GetComponent<SkillBulletRender>();
            if (bulletRender == null)
            {
                bulletRender = bulletObj.AddComponent<SkillBulletRender>();
            }
            
            // 初始化对象位置
            FixIntVector3 rangePos = FixIntVector3.zero;
            if (config.isLoopCreate)
            {
                FixInt x = _logicRandom.Range(config.minRandomRangeVect3.x, config.maxRandomRangeVect3.x);
                FixInt y = _logicRandom.Range(config.minRandomRangeVect3.y, config.maxRandomRangeVect3.y);
                FixInt z = _logicRandom.Range(config.minRandomRangeVect3.z, config.maxRandomRangeVect3.z);
                rangePos = new FixIntVector3(x, y, z);
            }
            
            SkillBulletLogic bulletLogic =  new SkillBulletLogic(this,_skillCreator,bulletRender, config,rangePos);
            bulletRender.SetRenderData(bulletLogic, config);
            _skillCreator.AddBullet(bulletLogic);
        }
        
        public void OnBulletRelease()
        {
            _curCreateBulletAccTimeList.Clear();
            _logicRandom = null;
        }
    }
}