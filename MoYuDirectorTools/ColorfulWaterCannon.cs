using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class ColorfulWaterCannon : MonoBehaviour
    {
        public BlockBehaviour BB { get; internal set; }

        public GameObject SteamParticle;
        public GameObject PartSystem;
        public GameObject SubEmitter;
        public GameObject PartVis;

        public MColourSlider waterColor;
        public MSlider waterAlpha;
        private void Awake()
        {

            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        public virtual void SafeAwake()
        {
            waterColor = BB.AddColourSlider("液体颜色", "watercolor", new Color(1f, 1f, 1f, 1f), false);
            waterAlpha = BB.AddSlider("液体不透明度", "wateralpha", 1f, 0f, 1f);
        }
        public void Start()
        {
            SteamParticle = BB.gameObject.transform.FindChild("SteamParticle").gameObject;
            PartSystem = BB.gameObject.transform.FindChild("PartSystem").gameObject;
            SubEmitter = PartSystem.transform.FindChild("SubEmitter").gameObject;
            PartVis = BB.gameObject.transform.FindChild("PartVis").gameObject;

            Color thisColor = waterColor.Value;
            thisColor.a = Mathf.Max(0f, Mathf.Min(1f, waterAlpha.Value));
            SteamParticle.GetComponent<ParticleSystem>().startColor = thisColor;
            PartSystem.GetComponent<ParticleSystem>().startColor = thisColor;
            SubEmitter.GetComponent<ParticleSystem>().startColor = thisColor;
            PartVis.GetComponent<ParticleSystem>().startColor = thisColor;
        }
    }
}
