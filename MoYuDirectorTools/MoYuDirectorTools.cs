using System;
using Modding;
using UnityEngine;

namespace MoYuDirectorTools
{
	public class MoYuDirectorTools : ModEntryPoint
	{
        public static GameObject mod;
        public override void OnLoad()
        {

            mod = new GameObject("Moyu Director Tools");
            UnityEngine.Object.DontDestroyOnLoad(mod);
            mod.AddComponent<CustomBlockController>();
            mod.AddComponent<ResourceController>();
            mod.AddComponent<RGBController>();
            mod.AddComponent<TimeScaleRendererController>();
            mod.AddComponent<SmoothCameraController>();

            // Called when the mod is loaded.
        }
    }
}
