using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class LimitedSteeringBlock : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }
        public SteeringWheel steeringWheel;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            steeringWheel = BB.GetComponent<SteeringWheel>();
            steeringWheel.allowLimits = true;
            FauxTransform iconInfo = new FauxTransform(new Vector3(0f, -0.342f, 0f), Quaternion.Euler(90f, 0f, 0f), Vector3.one * 0.5f);
            steeringWheel.AddLimits("角度限制", "limits", 40f, 40f, 180f, iconInfo, steeringWheel);
        }
    }
}
