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
    class AnimeBlock : BlockScript
    {
        public MToggle staticAnime;

        public MMenu root;
        public MSlider posx;
        public MSlider posy;
        public MSlider posz;
        public MSlider rotx;
        public MSlider roty;
        public MSlider rotz;
        public MSlider scalex;
        public MSlider scaley;
        public MSlider scalez;

        public MToggle Apply;
        public MText AssetName;
        public MText ObjectName;

        public MKey[] AnimeKeys = new MKey[8];
        public MText[] AnimeStateName = new MText[8];
        public MSlider[] AnimeStartTime = new MSlider[8];

        public GameObject listVis;
        public GameObject colliders;
        public GameObject Vis;
        public GameObject Adding_point;
        public Animator animator;
        public bool mySkinCode = false;

        public void initKeys()
        {
            for (int i = 0; i < 8; i++)
            {
                AnimeKeys[i] = AddKey("PlayState" + i.ToString(), "AnimeKey"+i.ToString(), (KeyCode)((int)KeyCode.None));
                AnimeStateName[i] = AddText("State" + i.ToString(), "State" + i.ToString(), "undefined");
                AnimeStartTime[i] = AddSlider(LanguageManager.Instance.outLang.StartTime + i.ToString(), "StartTime" + i.ToString(), 0f, 0f, 100f);
            }
        }
        public void initTransformSliders()
        {
            posx = AddSlider("Pos.x", "posx", 0f, -10f, 10f);
            posx.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posy = AddSlider("Pos.y", "posy", 0f, -10f, 10f);
            posy.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posz = AddSlider("Pos.z", "posz", 0f, -10f, 10f);
            posz.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            rotx = AddSlider("Rot.x", "rotx", 0f, 0f, 360f);
            rotx.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            roty = AddSlider("Rot.y", "roty", 0f, 0f, 360f);
            roty.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            rotz = AddSlider("Rot.z", "rotz", 0f, 0f, 360f);
            rotz.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            scalex = AddSlider("Scale.x", "scalex", 1f, 0f, 10f);
            scalex.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;

            };
            scaley = AddSlider("Scale.y", "scaley", 1f, 0f, 10f);
            scaley.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;
            };
            scalez = AddSlider("Scale.z", "scalez", 1f, 0f, 10f);
            scalez.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;
            };

        }
        public void displayAssetToggles(bool active)
        {
            Apply.DisplayInMapper = active;
            AssetName.DisplayInMapper = active;
            ObjectName.DisplayInMapper = active;
            staticAnime.DisplayInMapper = active;
            foreach (var a in AnimeKeys)
                a.DisplayInMapper = active;
            foreach (var a in AnimeStateName)
                a.DisplayInMapper = active;
            foreach (var a in AnimeStartTime)
                a.DisplayInMapper = active;
        }
        public void displayTransformSliders(bool active)
        {
            posx.DisplayInMapper = active;
            posy.DisplayInMapper = active;
            posz.DisplayInMapper = active;
            rotx.DisplayInMapper = active;
            roty.DisplayInMapper = active;
            rotz.DisplayInMapper = active;
            scalex.DisplayInMapper = active;
            scaley.DisplayInMapper = active;
            scalez.DisplayInMapper = active;
        }
        public void InitVis()
        {
            if (BlockBehaviour.transform.FindChild("list Vis") == null)
            {
                listVis = new GameObject("list Vis");
                listVis.SetActive(true);
                listVis.transform.SetParent(BlockBehaviour.transform);
                listVis.transform.localPosition = Vector3.zero;
                listVis.transform.localRotation = Quaternion.identity;
                listVis.transform.localScale = Vector3.one;
            }
        }
        public override void SafeAwake()
        {
            initTransformSliders();
            root = AddMenu("rootmenu", 0, new List<string> { "animator", "transform" });

            AssetName = AddText("Asset", "assetname", "AssetBundle");
            ObjectName = AddText("Object", "objname", "Object");
            Apply = AddToggle(LanguageManager.Instance.outLang.Apply, "apply", false);
            staticAnime = AddToggle(LanguageManager.Instance.outLang.Static, "isStatic", false);

            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;
            Adding_point = BlockBehaviour.gameObject.transform.FindChild("Adding Point").gameObject;

            initKeys();

            InitVis();
            root.ValueChanged += (int value) =>
            {
                if (root.Value == 1)
                {
                    displayAssetToggles(false);
                    displayTransformSliders(true);
                }
                else if (root.Value == 0)
                {
                    displayTransformSliders(false);
                    displayAssetToggles(true);
                }
            };
            BlockBehaviour.name = "AnimeBlock";
        }
        public void applyAsset()
        {
            ResourceController.Instance.addAsset(AssetName.Value);
            ModAssetBundle modAsset = ResourceController.Instance.getAsset(AssetName.Value);
            if (modAsset == null)
            {
                return;
            }
            try
            {
                GameObject getObject = Instantiate(modAsset.LoadAsset<GameObject>(ObjectName.Value));
                if (getObject == null)
                    return;
                try
                {
                    Destroy(listVis.transform.GetChild(0).gameObject);
                }
                catch { }
                getObject.transform.SetParent(listVis.transform);
                getObject.transform.localPosition = Vector3.zero;
                getObject.transform.localRotation = Quaternion.identity;
                getObject.transform.localScale = Vector3.one;
                try
                {
                    animator = getObject.GetComponent<Animator>();
                }
                catch { }

            }
            catch { }
        }
        public override void BuildingUpdate()
        {
            if (Apply.IsActive)
            {
                Apply.IsActive = false;
                applyAsset();
            }
        }
        public void Update()
        {
            if (BlockBehaviour.isSimulating)
            {
                for (int i = 0; i < 8; i++) 
                    if (AnimeKeys[i].IsPressed)
                        animator.CrossFadeInFixedTime(AnimeStateName[i].Value, 0.2f, 0, AnimeStartTime[i].Value);
            }
            if (OptionsMaster.skinsEnabled != mySkinCode)
            {
                mySkinCode = OptionsMaster.skinsEnabled;
                Vis.SetActive(!mySkinCode);
            }
        }
        public override void OnSimulateStart()
        {
            colliders.SetActive(false);
            
        }
        public void Start()
        {
            Physics.IgnoreLayerCollision(1, 1, true);
            applyAsset();
        }
        public override void SimulateFixedUpdateHost()
        {
            if (!BlockBehaviour.noRigidbody)
            {
                try
                {
                    if(!staticAnime.IsActive)
                        BlockBehaviour.transform.SetParent(BlockBehaviour.blockJoint.connectedBody.transform);
                    Destroy(BlockBehaviour.blockJoint);
                    Destroy(BlockBehaviour.Rigidbody);
                    BlockBehaviour.noRigidbody = true;
                    
                    try
                    {
                        BlockBehaviour.transform.FindChild("CameraBlock").SetParent(BlockBehaviour.transform.FindChild("list Vis").transform.GetChild(0));
                    }
                    catch { }
                    //mainCam = Camera.main;
                }
                catch { }
            }
            if (BlockBehaviour.isSimulating)
            {
                if (BlockBehaviour.isSimulating)
                {
                    for (int i = 0; i < 8; i++)
                        if (AnimeKeys[i].EmulationPressed())
                            animator.CrossFadeInFixedTime(AnimeStateName[i].Value, 0.2f, 0, AnimeStartTime[i].Value);
                }
            }
        }
    }
}
