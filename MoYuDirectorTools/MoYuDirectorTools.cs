using System;
using Modding;
using UnityEngine;

namespace MoYuDirectorTools
{
	public class MoYuDirectorTools : ModEntryPoint
	{
        public static GameObject mod;
        //public static ModAssetBundle nijigenShaders;
        public override void OnLoad()
        {

            mod = new GameObject("Moyu Director Tools");
            UnityEngine.Object.DontDestroyOnLoad(mod);
            mod.AddComponent<CustomBlockController>();
            mod.AddComponent<ResourceController>();
            mod.AddComponent<RGBController>();
            mod.AddComponent<TimeScaleRendererController>();
            mod.AddComponent<SmoothCameraController>();
            mod.AddComponent<LanguageManager>();
            //nijigenShaders = ModResource.GetAssetBundle("3to2_shaders");
            //UnityEngine.Object.DontDestroyOnLoad(nijigenShaders);
            // Called when the mod is loaded.
        }
    }
}
