using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class screenSort : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }
        public GameObject ScreenObject;
        public bool isFirstFrame = false;

        public MSlider sortingOrder;
        private void Awake()
        {

            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            sortingOrder = BB.AddSlider("sortingOrder", "sortingOrder", 0f, -32768f, 32768f);
            ScreenObject = null;

        }
        public void Start()
        {
            isFirstFrame = true;
        }
        public void Update()
        {
            if (BB.isSimulating)
            {
                if (isFirstFrame)
                {
                    try
                    {
                        ScreenObject = BB.transform.FindChild("Screen").gameObject;
                        ScreenObject.GetComponent<MeshRenderer>().sortingOrder = (int)Mathf.Clamp(sortingOrder.Value, -32768f, 32768f);
                    }
                    catch { }
                    isFirstFrame = false;
                }
            }
        }
    }
}
