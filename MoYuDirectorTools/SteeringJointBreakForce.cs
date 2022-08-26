using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class SteeringJointBreakForce : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }
        public MSlider breakForceSlider;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            breakForceSlider = BB.AddSlider(LanguageManager.Instance.outLang.Break_Force, "breakforce", BB.blockJoint.breakForce, 0f, 50000f);
            try
            {
                breakForceSlider.ValueChanged += (float value) =>
                {
                    BB.blockJoint.breakForce = Mathf.Max(0f, breakForceSlider.Value);
                    BB.blockJoint.breakTorque = Mathf.Max(0f, breakForceSlider.Value);
                };
            }
            catch { }
        }
    }
}
