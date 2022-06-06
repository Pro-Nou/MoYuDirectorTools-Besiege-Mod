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
    class AudioBlock: BlockScript
    {
        private Rect UIrect = new Rect(0, 100, 512, 600);
        public MKey PlayAC;
        public MToggle Apply;
        public MText AudioName;
        public MToggle isVisible;
        public MToggle isLoop;
        public MToggle scaleable;
        public MSlider diffuse;
        public MMenu DisplayType;
        public MMenu OptionMenu;
        public MMenu FFTtype;
        public float[] ASData;
        public Color[] colorData;
        public MeshRenderer[] rendererData;
        public float colorOffset = 0;
        public int ASresolution = 6;

        public MToggle ASVApply;
        public MToggle RGBtoggle;
        public MToggle listeningMode;
        public MText listeningBlock;
        public MToggle StreamLight;
        public MSlider StreamSpeed;
        public MSlider channelSlider;
        public MSlider MaxFFTVolum;
        public MSlider ASRSlider;
        public MSlider ASRScaleSlider;
        public MSlider ASVwidth;
        public MSlider ASVheight;
        public MSlider ASVdis;
        public MSlider ASVoffset;
        public MSlider ASVRoffset;
        public MMenu ShapeMenu;
        public MColourSlider firstColor;
        public MColourSlider secondColor;
        //public AudioClip AC;
        public AudioSource AS;
        public AudioSource ASselect;
        public GameObject colliders;
        public GameObject Vis;
        public GameObject ASGO;
        public GameObject ASVGO;

        public int MyGUID;
        public bool isPlaying = false;
        public void initAudio()
        {
            if (BlockBehaviour.gameObject.transform.FindChild("Audio Player") != null)
                return;

            ASGO = new GameObject("Audio Player");
            ASGO.transform.SetParent(BlockBehaviour.gameObject.transform);
            ASGO.transform.localPosition = new Vector3(0, 0, 0);
            AS = ASGO.GetComponent<AudioSource>() ?? ASGO.AddComponent<AudioSource>();

            AS.spatialBlend = 1.0f;
            AS.volume = 1.0f;
            AS.SetSpatializerFloat(1, 1f);
            AS.SetSpatializerFloat(2, 0);
            AS.SetSpatializerFloat(3, 12);
            AS.SetSpatializerFloat(4, 1000f);
            AS.SetSpatializerFloat(5, 1f);

            ASGO.SetActive(false);

            ASVGO = new GameObject("Audio Vis");
            ASVGO.transform.SetParent(BlockBehaviour.gameObject.transform);
            ASVGO.transform.localPosition = Vector3.zero;
            ASVGO.transform.localScale = Vector3.one;
            ASVGO.transform.localRotation = Quaternion.identity;
            ASVGO.SetActive(false);
        }
        public override void SafeAwake()
        {
            OptionMenu = AddMenu("optionmenu", 0, new List<string>() { "音频设置", "可视化设置" });
            FFTtype = AddMenu("FFTtype", 0, Enum.GetNames(typeof(FFTWindow)).ToList());

            PlayAC = AddKey("播放", "play", KeyCode.B);
            Apply = AddToggle("应用", "apply", false);
            isLoop = AddToggle("循环", "isloop", false);
            isVisible = AddToggle("可见", "isVisible", true);
            scaleable = AddToggle("时间缩放", "scaleable", false);
            DisplayType = AddMenu("BlendType", 1, new List<string>() { "2D", "3D" });
            diffuse = AddSlider("扩散强度", "diffuse", 0.5f, 0f, 1f);
            AudioName = AddText("音源(.wav)", "audio", "AudioName");

            ASVApply = AddToggle("启用可视化", "asvapply", false);
            RGBtoggle = AddToggle("R G B!", "rgb", false);
            listeningMode = AddToggle("监听模式", "listeningMode", false);
            listeningBlock = AddText("监听GUID", "listeningBlock", "0");
            StreamLight = AddToggle("流光", "streamLight", false);
            StreamSpeed = AddSlider("流光速度", "streamSpeed", 1f, 0f, 10.0f);
            ASRSlider = AddSlider("采样分辨率", "asresolution", 6.0f, 6.0f, 13.0f);
            MaxFFTVolum = AddSlider("最大采样音量", "maxfftvolum", 1f, 0f, 1f);
            channelSlider = AddSlider("采样频道", "FFTchannel", 0f, 0f, 8f);
            ASRScaleSlider = AddSlider("形变强度", "asrscale", 10.0f, 0.0f, 100.0f);
            ASVheight = AddSlider("单位高度", "asvheight", 1.0f, 0.0f, 100.0f);
            ASVwidth = AddSlider("单位宽度", "asvwidth", 1.0f, 0.0f, 100.0f);
            ASVdis = AddSlider("单位间距", "asvdis", 1.0f, 0.0f, 100.0f);
            ASVoffset = AddSlider("中轴偏移", "asvoffset", 0f, -0.5f, 0.5f);
            ASVRoffset = AddSlider("倾斜偏移", "asvroffset", 0f, -90f, 90f);
            ShapeMenu = AddMenu("shapeType", 0, new List<string>() { "条带", "环" });
            firstColor = AddColourSlider("颜色1", "firstcolor", new Color(1, 0, 0, 1), false);
            secondColor = AddColourSlider("颜色2", "secondcolor", new Color(1, 0, 0, 1), false);

            initAudio();
            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;
            BlockBehaviour.name = "Audio Block";

            OptionMenu.ValueChanged += (int value) =>
            {
                if (OptionMenu.Value == 1)
                {
                    displayASToggles(false);
                    displayASVToggles(true);
                }
                else if (OptionMenu.Value == 0)
                {
                    displayASVToggles(false);
                    displayASToggles(true);
                }
            };
        }
        public void displayASToggles(bool active)
        {
            PlayAC.DisplayInMapper = active;
            Apply.DisplayInMapper = active;
            isLoop.DisplayInMapper = active;
            isVisible.DisplayInMapper = active;
            scaleable.DisplayInMapper = active;
            DisplayType.DisplayInMapper = active;
            diffuse.DisplayInMapper = active;
            AudioName.DisplayInMapper = active;
        }
        public void displayASVToggles(bool active)
        {
            ASVApply.DisplayInMapper = active;
            ASRSlider.DisplayInMapper = active;
            ASRScaleSlider.DisplayInMapper = active;
            ASVheight.DisplayInMapper = active;
            ASVwidth.DisplayInMapper = active;
            ASVdis.DisplayInMapper = active;
            ASVoffset.DisplayInMapper = active;
            ASVRoffset.DisplayInMapper = active;
            FFTtype.DisplayInMapper = active;
            StreamLight.DisplayInMapper = active;
            RGBtoggle.DisplayInMapper = active;
            MaxFFTVolum.DisplayInMapper = active;
            StreamSpeed.DisplayInMapper = active;
            ShapeMenu.DisplayInMapper = active;
            listeningBlock.DisplayInMapper = active;
            listeningMode.DisplayInMapper = active;
            channelSlider.DisplayInMapper = active;
            firstColor.DisplayInMapper = active;
            secondColor.DisplayInMapper = active;
        }
        public void tryGetAS()
        {
            Transform parentTransform = BlockBehaviour.transform.parent;
            //Debug.Log(childCount.ToString());
            for (int i = 0; i < parentTransform.childCount; i++) 
            {
                try
                {
                    if (listeningBlock.Value == parentTransform.GetChild(i).GetComponent<AudioBlock>().MyGUID.ToString())
                    {
                        ASselect = parentTransform.GetChild(i).GetComponent<AudioBlock>().AS;
                        return;
                    }
                }
                catch { }
            }
            ASselect = null;
        }
        public void Update()
        {
            if (BlockBehaviour.isSimulating)
            {
                if (ASselect != null)
                {
                    if (PlayAC.IsPressed && !listeningMode.IsActive)
                    {
                        ASGO.SetActive(!ASGO.activeSelf);
                        if (!ASGO.activeSelf)
                            resetASV();
                    }
                    if (ASselect.isPlaying)
                    {
                        isPlaying = ASselect.isPlaying;
                        try
                        {
                            ASselect.GetSpectrumData(ASData, (int)Mathf.Max(0f, channelSlider.Value), (FFTWindow)FFTtype.Value);
                            UpdateASV();
                        }
                        catch { }
                    }
                    else
                    {
                        if(isPlaying!= ASselect.isPlaying)
                        {
                            isPlaying = false;
                            resetASV();
                        }
                    }
                    if (ASGO.activeSelf)
                    {
                        if (!ASselect.isPlaying)
                        {
                            ASGO.SetActive(false);
                            resetASV();
                        }
                    }
                }
                if(StreamLight.IsActive)
                {
                    colorOffset += Time.deltaTime * StreamSpeed.Value * 10f;
                    if (colorOffset >= ASData.Length)
                        colorOffset -= ASData.Length;
                    //Debug.Log(colorData[(int)colorOffset].ToString());
                    for (int i = 0; i < ASData.Length; i++)
                    {
                        int index = i + (int)colorOffset;
                        if (index >= ASData.Length)
                            index -= ASData.Length;
                        rendererData[i].material.SetColor("_TintColor", colorData[index]);
                    }
                }
                else if (RGBtoggle.IsActive)
                {
                    for (int i = 0; i < ASData.Length; i++)
                    {
                        rendererData[i].material.SetColor("_TintColor", RGBController.Instance.outputColor);
                    }
                }
            }
        }
        public void LateUpdate()
        {
            if (BlockBehaviour.isSimulating)
            {
                if (ASselect != null)
                {
                    if (ASGO.activeSelf)
                    {
                        if (scaleable.IsActive)
                        {
                            ASselect.pitch = Time.timeScale;
                        }
                        if (DisplayType.Value == 1)
                        {
                            ASGO.transform.localPosition = Vector3.Lerp(Vector3.zero, BlockBehaviour.gameObject.transform.InverseTransformPoint(Camera.main.transform.position), diffuse.Value);
                        }
                    }
                }
            }
        }
        public void UpdateASV()
        {
            for (int i = 0; i < ASData.Length; i++)
            {
                ASData[i] = Mathf.Min(MaxFFTVolum.Value, ASData[i]);
                float thisScale = 0f;

                if (i < ASData.Length - 1)
                    thisScale = ASVheight.Value * (1 + (ASData[i] * ASRScaleSlider.Value));
                else
                {
                    if (ShapeMenu.Value == 1 || ShapeMenu.Value == 2)
                    {
                        thisScale = ASVheight.Value * (1 + ((ASData[i] + ASData[0]) / 2f * ASRScaleSlider.Value));
                    }
                    else
                        thisScale = ASVheight.Value * (1 + (ASData[i] * ASRScaleSlider.Value));
                }
                thisScale = (thisScale + ASVGO.transform.GetChild(i).localScale.y) / 2;
                ASVGO.transform.GetChild(i).localScale = new Vector3(ASVwidth.Value, thisScale, 1f);
            }
        }
        public void resetASV()
        {
            for (int i = 0; i < ASData.Length; i++)
                ASVGO.transform.GetChild(i).localScale = new Vector3(ASVwidth.Value, ASVheight.Value, 1f);
        }
        public void Start()
        {
            colorOffset = 0;
            //ASData = new float[(int)Math.Pow(2, ASresolution)];
            //colorData = new Color[ASData.Length];
            BlockBehaviour.Rigidbody.drag = 0.0f;
            BlockBehaviour.Rigidbody.angularDrag = 0.0f;
            BlockBehaviour.blockJoint.breakForce = Mathf.Infinity;
            BlockBehaviour.blockJoint.breakTorque = Mathf.Infinity;

            BlockBehaviour.noRigidbody = false;
            applyAudio();
            isPlaying = false;
            if (!listeningMode.IsActive)
                ASselect = AS;
            else
                tryGetAS();
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
                if (ASselect != null)
                {
                    if (PlayAC.EmulationPressed() && !listeningMode.IsActive)
                    {
                        ASGO.SetActive(!ASGO.activeSelf);
                        if (!ASGO.activeSelf)
                            resetASV();
                    }
                }
            }
        }
        public void createASVis()
        {
            int childCount = ASVGO.transform.childCount;
            float R = (ASVdis.Value * ASData.Length) / (float)(2 * Math.PI);
            for (int i = childCount - 1; i >= 0; i--)
                Destroy(ASVGO.transform.GetChild(i).gameObject);
            for (int i = 0; i < ASData.Length; i++) 
            {
                GameObject asvObject = new GameObject("Audio Vis Object" + i.ToString());
                GameObject asvBody = GameObject.CreatePrimitive(PrimitiveType.Quad);
                asvBody.transform.SetParent(asvObject.transform);
                asvBody.transform.localPosition = new Vector3(0f, ASVoffset.Value, 0f);
                asvBody.transform.localRotation = Quaternion.identity;
                asvBody.GetComponent<MeshCollider>().enabled = false;
                rendererData[i] = asvBody.GetComponent<MeshRenderer>();
                rendererData[i].material = new Material(Shader.Find("Particles/Alpha Blended"));
                asvObject.transform.SetParent(ASVGO.transform);

                if (ShapeMenu.Value == 0)
                {
                    asvObject.transform.localPosition = new Vector3((float)((i - (ASData.Length / 2) + 0.5) * ASVdis.Value), 0, 0);
                    asvObject.transform.localScale = new Vector3(ASVwidth.Value, ASVheight.Value, 1);
                    asvObject.transform.localRotation = Quaternion.Euler(ASVRoffset.Value, 0, 0);
                }
                if (ShapeMenu.Value == 1)
                {
                    float thisCircleOffset = (float)i / (float)ASData.Length;
                    asvObject.transform.localPosition = new Vector3(R * Mathf.Sin(thisCircleOffset * 2 * Mathf.PI), 0, R * Mathf.Cos(thisCircleOffset * 2 * Mathf.PI));
                    asvObject.transform.localScale = new Vector3(ASVwidth.Value, ASVheight.Value, 1);
                    asvObject.transform.localRotation = Quaternion.Euler(ASVRoffset.Value, thisCircleOffset * 360.0f, 0);
                }
                /*
                if (ShapeMenu.Value == 2)
                {
                    float thisCircleOffset = (float)i / (float)ASData.Length;
                    asvObject.transform.localPosition = new Vector3(R * Mathf.Sin(thisCircleOffset * 2 * Mathf.PI), 0, R * Mathf.Cos(thisCircleOffset * 2 * Mathf.PI));
                    asvObject.transform.localScale = new Vector3(ASVwidth.Value, ASVheight.Value, 1);
                    asvObject.transform.localRotation = Quaternion.Euler(90f, thisCircleOffset * 360.0f, 0);
                }
                */
                if (!RGBtoggle.IsActive)
                {
                    colorData[i] = Color.Lerp(firstColor.Value, secondColor.Value, Mathf.Abs(((float)i * 2 / (float)ASData.Length) - 1f));
                    rendererData[i].material.SetColor("_TintColor", colorData[i]);
                }
                else
                {
                    if (StreamLight.IsActive)
                    {
                        float RGBoffset = (float)i / (float)ASData.Length;
                        if (RGBoffset > 1f)
                            RGBoffset -= 1f;
                        colorData[i] = Color.HSVToRGB(RGBoffset, 1f, 1f);
                        rendererData[i].material.SetColor("_TintColor", colorData[i]);
                    }
                    else
                        rendererData[i].material.SetColor("_TintColor", RGBController.Instance.outputColor);
                }
                asvObject.SetActive(true);
            }
            ASVGO.SetActive(true);
        }
        public override void OnSimulateStart()
        {
            ASresolution = (int)ASRSlider.Value;
            if (ASresolution > 13)
                ASresolution = 13;
            if (ASresolution < 6)
                ASresolution = 6;
            ASData = new float[(int)Math.Pow(2, ASresolution)];
            colorData = new Color[ASData.Length];
            rendererData = new MeshRenderer[ASData.Length];
            ASGO.SetActive(false);
            colliders.SetActive(false);
            if (!isVisible.IsActive)
                Vis.SetActive(false);
            if (ASVApply.IsActive)
                createASVis();
            else
                ASVGO.SetActive(false);
            
            AS.spatialBlend = DisplayType.Value;
            AS.loop = isLoop.IsActive;
        }
        public override void OnSimulateStop()
        {
            ASGO.SetActive(false);
            colliders.SetActive(true);
            Vis.SetActive(true);
        }
        public override void BuildingUpdate()
        {
            if (Apply.IsActive)
            {
                Apply.IsActive = false;
                applyAudio();
            }
            if (BlockBehaviour.Guid.GetHashCode() != 0 && BlockBehaviour.Guid.GetHashCode() != MyGUID)
                MyGUID = BlockBehaviour.Guid.GetHashCode();
            if(!listeningMode.IsActive)
            {
                listeningBlock.Value = MyGUID.ToString();
            }
        }
        public void applyAudio()
        {
            ResourceController.Instance.addAudio(AudioName.Value);
            AudioClip ACload = ResourceController.Instance.getAudio(AudioName.Value);
            if (ACload != null)
                AS.clip = ACload;
        }
        /*void OnGUI()
        {
            string showmsg = "";
            foreach(var a in ASData)
            {
                showmsg += a.ToString() + '\n';
            }
            GUI.Box(UIrect, showmsg);

        }*/
    }
}
