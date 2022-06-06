using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class buildSurfaceMass : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }
        public MToggle changeMass;
        public MSlider massSlider;
        private void Awake()
        {

            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            changeMass = BB.AddToggle("改变质量", "changemass", false);
            massSlider = BB.AddSlider("质量", "mass", 2f, 0f, 10f);

        }
        public void Start()
        {
            if (BB.isSimulating && changeMass.IsActive) 
            {
                try
                {
                    BB.Rigidbody.mass = Mathf.Max(0f, massSlider.Value);
                }
                catch { }
            }
        }
    }
}
