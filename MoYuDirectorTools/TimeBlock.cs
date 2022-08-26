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
    class TimeScaleRendererController : SingleInstance<TimeScaleRendererController>
    {
        public override string Name { get; } = "TimeScaleRendererController";
        public GameObject TimeScaleHandle;
        public DynamicText timeScaleText;
        public void Start()
        {
            TimeScaleHandle = GameObject.Find("HUD").transform.FindChild("TopBar").FindChild("Align (Top Left)").FindChild("TimeScale").gameObject;
            timeScaleText = TimeScaleHandle.transform.FindChild("r_Text").GetComponent<DynamicText>();
            TimeScaleHandle = TimeScaleHandle.transform.FindChild("Handle").FindChild("Knob").gameObject;
        }
        public void FixHUD()
        {
            try
            {
                timeScaleText.SetText(((int)(Time.timeScale * 100f)).ToString() + "%");
            }
            catch { }
            try
            {
                TimeScaleHandle.transform.localPosition = new Vector3(Mathf.Lerp(-0.8f, 0.8f, Time.timeScale / 2f), 0f,0f);
            }
            catch { }
        }
    }
    class TimeBlock : BlockScript
    {
        public MKey changeTime;
        public MToggle isVisible;
        public MToggle dioMode;
        public MSlider tgtTimeSlider;
        public MSlider changeSpeedSlider;
        public MSlider theWorldWidth;
        public MColourSlider theWorldColor;

        public GameObject colliders;
        public GameObject Vis;
        public GameObject shockwave;
        public GlobalFog globalFog;
        public MeshRenderer shockwaveRenderer;
        public AudioSource AS;

        public bool ischanging = false;
        public bool theWorld = false;
        public float tgtTime;
        public float changeSpeed;
        public float timeOffset = 0f;
        public float currentTimeScale = 100f;
        public void InitFX()
        {
            if (BlockBehaviour.transform.FindChild("shockwaveFX") == null)
            {
                shockwave = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                shockwave.name = "shockwaveFX";
                shockwave.SetActive(false);
                shockwave.transform.SetParent(BlockBehaviour.transform);
                shockwave.transform.localScale = Vector3.zero;
                shockwave.transform.localPosition = Vector3.zero;
                shockwave.transform.localRotation = Quaternion.identity;
                Destroy(shockwave.GetComponent<SphereCollider>());
                shockwaveRenderer = shockwave.GetComponent<MeshRenderer>();
                shockwaveRenderer.material.shader = Shader.Find("FX/Shockwave/Distortion");
                shockwaveRenderer.material.SetFloat("_Refraction", 20f);

                AS = shockwave.GetComponent<AudioSource>() ?? shockwave.AddComponent<AudioSource>();

                AS.spatialBlend = 0.9f;
                AS.loop = false;
                AS.clip = ModResource.GetAudioClip("THE WORLD").AudioClip;
                AS.volume = 1.0f;
                AS.SetSpatializerFloat(1, 3f);
                AS.SetSpatializerFloat(2, -96f);
                AS.SetSpatializerFloat(3, 12f);
                AS.SetSpatializerFloat(4, 1f);
                AS.SetSpatializerFloat(5, 1f);
            }
        }
        public override void SafeAwake()
        {
            changeTime = AddKey(LanguageManager.Instance.outLang.Launch, "apply", KeyCode.B);
            isVisible = AddToggle(LanguageManager.Instance.outLang.Visible, "isVisible", true);
            dioMode = AddToggle("Ko no DIO da!", "dio", false);
            tgtTimeSlider = AddSlider(LanguageManager.Instance.outLang.Target_Time_Scale, "tgttime", 1f, 0f, 2f);
            changeSpeedSlider = AddSlider(LanguageManager.Instance.outLang.Change_Speed, "changespeed", 1f, 0f, 2f);
            theWorldWidth = AddSlider(LanguageManager.Instance.outLang.FXwidth, "FXwidth", 1f, 0f, 10f);
            theWorldColor = AddColourSlider(LanguageManager.Instance.outLang.FXcolor, "diocolor", new Color(0, 0, 0, 1), false);

            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;

            InitFX();

            BlockBehaviour.name = "Time Block";
        }
        public override void OnSimulateStart()
        {
            colliders.SetActive(false);
            if (!isVisible.IsActive)
                Vis.SetActive(false);
        }
        public void Start()
        {
            BlockBehaviour.Rigidbody.drag = 0.0f;
            BlockBehaviour.Rigidbody.angularDrag = 0.0f;
            BlockBehaviour.blockJoint.breakForce = Mathf.Infinity;
            BlockBehaviour.blockJoint.breakTorque = Mathf.Infinity;

            BlockBehaviour.noRigidbody = false;

            if (dioMode.IsActive)
            {
                tgtTime = 0f;
                changeSpeed = 0.5f;
            }
            else
            {
                tgtTime = Mathf.Max(0f, Mathf.Min(2f, tgtTimeSlider.Value));
                changeSpeed = Mathf.Max(0f, changeSpeedSlider.Value);
            }

            timeOffset = 0f;
            ischanging = false;
            theWorld = false;
            currentTimeScale = 100f;
            shockwave.SetActive(false);

            globalFog = Camera.main.GetComponent<GlobalFog>();
            if (dioMode.IsActive)
            {
                globalFog.fogMode = GlobalFog.FogMode.Distance;
                globalFog.globalDensity = 0f;
                globalFog.globalFogColor = theWorldColor.Value;
                globalFog.enabled = true;
            }
        }
        public void Update()
        {
            if (BlockBehaviour.isSimulating)
            {
                if(changeTime.IsPressed)
                {
                    if (changeSpeed == 0f)
                        return;
                    if (!ischanging)
                    {
                        timeOffset = 0f;
                        ischanging = true;
                        if (dioMode.IsActive)
                        {
                            if (!theWorld)
                            {
                                currentTimeScale = Time.timeScale;
                                shockwave.SetActive(true);
                            }
                            theWorld = !theWorld;
                        }
                        else
                            currentTimeScale = Time.timeScale;
                    }
                }
                if(ischanging)
                {
                    timeOffset += changeSpeed * Time.unscaledDeltaTime;
                    if (timeOffset >= 1f)
                    {
                        timeOffset = 1f;
                        ischanging = false;
                        if (dioMode.IsActive)
                            shockwave.SetActive(false);
                    }
                    if (dioMode.IsActive)
                    {
                        if (theWorld)
                        {
                            Time.timeScale = Mathf.Lerp(currentTimeScale, tgtTime, timeOffset);
                            globalFog.globalDensity = Mathf.Lerp(0f, 0.01f, timeOffset);
                            float FXoffset = Mathf.Sin(timeOffset * Mathf.PI);
                            shockwave.transform.localScale = Vector3.one * Mathf.Lerp(0f, theWorldWidth.Value * 100f, FXoffset);
                        }
                        else
                        {
                            Time.timeScale = Mathf.Lerp(tgtTime, currentTimeScale, timeOffset);
                            globalFog.globalDensity = Mathf.Lerp(0.01f, 0f, timeOffset);
                        }
                    }
                    else
                        Time.timeScale = Mathf.Lerp(currentTimeScale, tgtTime, timeOffset);
                    TimeScaleRendererController.Instance.FixHUD();
                }
            }
        }
        public override void SimulateFixedUpdateHost()
        {
            if (!BlockBehaviour.noRigidbody)
            {
                try
                {
                    BlockBehaviour.transform.SetParent(BlockBehaviour.blockJoint.connectedBody.transform);
                    Destroy(BlockBehaviour.blockJoint);
                    Destroy(BlockBehaviour.Rigidbody);
                    BlockBehaviour.noRigidbody = true;
                }
                catch { }
            }
            if (BlockBehaviour.isSimulating)
            {
                if (changeTime.EmulationPressed())
                {
                    if (changeSpeed == 0f)
                        return;
                    if (!ischanging)
                    {
                        timeOffset = 0f;
                        ischanging = true;
                        if (dioMode.IsActive)
                        {
                            if (!theWorld)
                            {
                                currentTimeScale = Time.timeScale;
                                shockwave.SetActive(true);
                            }
                            theWorld = !theWorld;
                        }
                        else
                            currentTimeScale = Time.timeScale;
                    }
                }
            }
        }
    }
}
