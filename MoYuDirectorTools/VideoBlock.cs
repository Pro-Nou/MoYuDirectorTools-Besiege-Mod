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
    class VideoBlock : BlockScript
    {
        public MovieTexture movTex;

        public MKey playKey;
        public MToggle Apply;
        public MText AssetName;
        public MText VideoName;
        public MToggle staticVideo;
        public MToggle isloop;
        public MSlider sortingOrder;

        public GameObject listVis;
        public GameObject Vis;
        public GameObject colliders;
        public MeshRenderer renderer;

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

        public bool mySkinCode = false;

        public void initTransformSliders()
        {
            posx = AddSlider("Pos.x", "posx", 0f, -10f, 10f);
            posx.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posy = AddSlider("Pos.y", "posy", 0f, -10f, 10f);
            posy.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posz = AddSlider("Pos.z", "posz", 0f, -10f, 10f);
            posz.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                Vis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //Adding_point.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                //colliders.transform.localPosition = Vis.transform.localPosition;
            };
            rotx = AddSlider("Rot.x", "rotx", 0f, 0f, 360f);
            rotx.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                //Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            roty = AddSlider("Rot.y", "roty", 0f, 0f, 360f);
            roty.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                //Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            rotz = AddSlider("Rot.z", "rotz", 0f, 0f, 360f);
            rotz.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                Vis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
                //Adding_point.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            scalex = AddSlider("Scale.x", "scalex", 1f, 0f, 10f);
            scalex.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //colliders.transform.localScale = Vis.transform.localScale;

            };
            scaley = AddSlider("Scale.y", "scaley", 1f, 0f, 10f);
            scaley.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //colliders.transform.localScale = Vis.transform.localScale;
            };
            scalez = AddSlider("Scale.z", "scalez", 1f, 0f, 10f);
            scalez.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                Vis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //Adding_point.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                //colliders.transform.localScale = Vis.transform.localScale;
            };

        }
        public void displayAssetToggles(bool active)
        {
            playKey.DisplayInMapper = active;
            Apply.DisplayInMapper = active;
            AssetName.DisplayInMapper = active;
            VideoName.DisplayInMapper = active;
            staticVideo.DisplayInMapper = active;
            sortingOrder.DisplayInMapper = active;
            isloop.DisplayInMapper = active;
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
                listVis.transform.localScale = Vector3.one * 4f;

                listVis.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("Plane").Mesh;
                renderer = listVis.AddComponent<MeshRenderer>();
                renderer.material.shader = Shader.Find("Particles/Alpha Blended");
                renderer.material.mainTexture = Texture2D.blackTexture;
            }
        }
        public override void SafeAwake()
        {
            root = AddMenu("rootmenu", 0, new List<string> { "video", "transform" });
            playKey = AddKey(LanguageManager.Instance.outLang.Play, "play", KeyCode.Alpha1);
            AssetName = AddText("Asset", "assetname", "AssetBundle");
            VideoName = AddText("Movie Texture", "movtex", "MovieTexture");
            Apply = AddToggle(LanguageManager.Instance.outLang.Apply, "apply", false);
            staticVideo = AddToggle(LanguageManager.Instance.outLang.Static, "isStatic", false);
            isloop = AddToggle(LanguageManager.Instance.outLang.Loop, "isloop", false);
            sortingOrder = AddSlider("sortingOrder", "sortingOrder", 0f, -32768f, 32768f);

            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;
            initTransformSliders();
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
            BlockBehaviour.name = "VideoBlock";
        }
        public void Start()
        {
            applyAsset();
            renderer = listVis.GetComponent<MeshRenderer>();
            renderer.material.mainTexture = movTex;
            renderer.sortingOrder = (int)sortingOrder.Value;
            movTex.loop = isloop.IsActive;
        }
        public override void OnSimulateStart()
        {
            colliders.SetActive(false);

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
                if(playKey.IsPressed)
                {
                    if (!movTex.isPlaying)
                        movTex.Play();
                    else
                        movTex.Stop();
                }
            }
            if (OptionsMaster.skinsEnabled != mySkinCode)
            {
                mySkinCode = OptionsMaster.skinsEnabled;
                Vis.SetActive(!mySkinCode);
            }
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
                movTex = Instantiate(modAsset.LoadAsset<MovieTexture>(VideoName.Value));
                if (movTex == null)
                    return;
            }
            catch { }
        }
        public override void SimulateFixedUpdateHost()
        {
            if (!BlockBehaviour.noRigidbody)
            {
                try
                {
                    if (!staticVideo.IsActive)
                        BlockBehaviour.transform.SetParent(BlockBehaviour.blockJoint.connectedBody.transform);
                    Destroy(BlockBehaviour.blockJoint);
                    Destroy(BlockBehaviour.Rigidbody);
                    BlockBehaviour.noRigidbody = true;
                    //mainCam = Camera.main;
                }
                catch { }
            }
            if (BlockBehaviour.isSimulating)
            {
                if (playKey.EmulationPressed()) 
                { 
                    if (!movTex.isPlaying)
                        movTex.Play();
                    else
                        movTex.Stop();
                }
            }
        }
    }
}
