using System;
using Sirenix.Utilities;
using UnityEngine;

namespace Game.Tools
{
    public class CameraFollow : MonoBehaviour
    {
        /// <summary>
        /// 跟随目标
        /// </summary>
        public Transform target;
        /// <summary>
        /// 最小移动距离
        /// </summary>
        public Vector2 minPosition;
        /// <summary>
        /// 平滑时间
        /// </summary>
        public float smoothTime = 0.3f;
        /// <summary>
        /// 最大移动距离
        /// </summary>
        public Vector2 maxPosition;
        /// <summary>
        /// 人物偏移中心距离后 跟随
        /// </summary>
        public float followDistance;

        private void LateUpdate()
        {
            if (target != null)
            {
                // 计算目标位置
                Vector3 targetPos = new Vector3(target.position.x, transform.position.y, transform.position.z);
                // 计算摄像机和角色之间的距离
                float distance = Vector3.Distance(targetPos, transform.position);
                bool isFollowTarget = distance > followDistance;
                // 限制X位置，不能超过地图范围
                targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x, maxPosition.x);
                if (isFollowTarget)
                {
                    transform.position= Vector3.Lerp(transform.position,targetPos,Time.deltaTime*smoothTime);
                }
            }
        }
    }
}