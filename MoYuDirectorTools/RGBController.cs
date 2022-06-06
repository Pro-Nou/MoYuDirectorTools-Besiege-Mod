using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;

using Modding.Modules;
using Modding;
using Modding.Blocks;
using UnityEngine;

namespace MoYuDirectorTools
{
    class RGBController : SingleInstance<RGBController>
    {
        public override string Name { get; } = "RGB Controller";

        public float AAA = 0f;
        public bool fixedRGB = false;
        public Color outputColor;
        public RGBController()
        {
            AAA = 0f;
            outputColor = Color.HSVToRGB(AAA, 1f, 1f);
        }
        public void Start()
        {
            outputColor = Color.HSVToRGB(AAA, 1f, 1f);
        }
        public void Update()
        {
            if (StatMaster.levelSimulating)
            {
                if (!fixedRGB)
                    AAA += Time.unscaledDeltaTime;
                else
                    AAA += Time.deltaTime;
                while (AAA > 1f)
                    AAA -= 1f;
                outputColor = Color.HSVToRGB(AAA, 1f, 1f);
            }
        }

    }
}
