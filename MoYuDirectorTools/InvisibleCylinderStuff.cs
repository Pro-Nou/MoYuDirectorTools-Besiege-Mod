using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class InvisibleCylinderStuff : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }
        public GameObject visA;
        public GameObject visB;
        public GameObject visC;

        public MToggle showA;
        public MToggle showB;
        public MToggle showC;
        private void Awake()
        {
            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            visA = BB.transform.FindChild("A").gameObject;
            visB = BB.transform.FindChild("B").gameObject;
            visC = BB.transform.FindChild("Cylinder").gameObject;
            if(BB.BlockID==(int)BlockType.Spring)
            {
                visA = visA.transform.GetChild(0).GetChild(0).gameObject;
                visB = visB.transform.GetChild(0).GetChild(0).gameObject;
            }
            else if(BB.BlockID == (int)BlockType.RopeWinch)
            {
                visA = visA.transform.FindChild("Vis").gameObject;
                visB = visB.transform.FindChild("Vis").gameObject;
                visC = visC.transform.FindChild("Vis").gameObject;
            }
            showA = BB.AddToggle("显示头", "showA", true);
            showB = BB.AddToggle("显示尾", "showB", true);
            showC = BB.AddToggle("显示中间", "showC", true);
        }
        private void Start()
        {
            if(BB.isSimulating)
            {
                try
                {
                    if (!showA.IsActive)
                        visA.GetComponent<MeshRenderer>().enabled = false;
                    if (!showB.IsActive)
                        visB.GetComponent<MeshRenderer>().enabled = false;
                    if (!showC.IsActive)
                        visC.GetComponent<MeshRenderer>().enabled = false;
                }
                catch { }
                //visA.SetActive(showA.IsActive);
                //visB.SetActive(showB.IsActive);
                //visC.SetActive(showC.IsActive);
            }
        }
    }
}
