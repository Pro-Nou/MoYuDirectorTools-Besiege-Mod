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
    class VisBlock: BlockScript
    {
        public MMenu shaderType;
        public MToggle Apply;
        public MToggle RGB;
        public MToggle hasCol;
        public MSlider massSlider;
        public MSlider healthSlider;

        public MMenu cullMode;
        public MMenu zwriteMode;
        public MMenu blendOp;
        public MMenu srcBlend;
        public MMenu dstBlend;
        public MSlider alphaSlider;
        public MSlider difPower;
        public MSlider ambientPower;
        public MSlider shadowThreshold;
        public MSlider shadowReceiveThreshold;
        public MSlider shadowBrightness;
        public MSlider shadowSmoothness;
        public MSlider rimThreshold;
        public MSlider specularScale;
        public MColourSlider colourSlider;
        public MColourSlider colourSliderDif;
        public MColourSlider rimColor;
        public MColourSlider specular;


        public MText MeshName;
        public MText TexName;
        public MText RSName;
        public MText LightName;
        private Rect UIrect = new Rect(0, 100, 512, 600);

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

        //public MeshFilter mesh2;
        public MeshRenderer renderer;
        public List<MeshRenderer> renderer2;
        public cakeslice.Outline orgOutLine;
        public List<cakeslice.Outline> outlines;
        public Material visMat;
        public bool myClusterCode = false;
        public bool mySkinCode = false;

        MaterialPropertyBlock VisPropertyBlock = new MaterialPropertyBlock();

        public GameObject colliders;
        public GameObject Vis;
        public GameObject listVis;

        public void initNijigen()
        {
            visMat.SetFloat("_CullMode", cullMode.Value);
            visMat.SetFloat("_ZWriteMode", zwriteMode.Value);
            visMat.SetFloat("_BlendOp", blendOp.Value);
            visMat.SetFloat("_SrcBlend", srcBlend.Value);
            visMat.SetFloat("_DstBlend", dstBlend.Value);
            Color thiscolor = colourSliderDif.Value;
            thiscolor.a = Mathf.Max(0f, Mathf.Min(1f, alphaSlider.Value));
            visMat.SetColor("_Color", thiscolor);
            visMat.SetFloat("_DiffusePower", difPower.Value);
            visMat.SetFloat("_AmbientPower", ambientPower.Value);
            visMat.SetFloat("_ShadowThreshold", shadowThreshold.Value);
            visMat.SetFloat("_ShadowReceiveThreshold", shadowReceiveThreshold.Value);
            visMat.SetFloat("_ShadowBrightness", shadowBrightness.Value);
            visMat.SetFloat("_ShadowSmoothness", shadowSmoothness.Value);
            visMat.SetColor("_RimColor", rimColor.Value);
            visMat.SetFloat("_RimThreshold", rimThreshold.Value);
            visMat.SetColor("_Specular", specular.Value);
            visMat.SetFloat("_SpecularScale", specularScale.Value);
            visMat.SetColor("_EmisColor", colourSlider.Value);
        }
        public void initTransformSliders()
        {
            posx = AddSlider("Pos.x", "posx", 0f, -10f, 10f);
            posx.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posy = AddSlider("Pos.y", "posy", 0f, -10f, 10f);
            posy.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posz = AddSlider("Pos.z", "posz", 0f, -10f, 10f);
            posz.ValueChanged += (float value) =>
            {
                listVis.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            rotx = AddSlider("Rot.x", "rotx", 0f, 0f, 360f);
            rotx.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            roty = AddSlider("Rot.y", "roty", 0f, 0f, 360f);
            roty.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));

            };
            rotz = AddSlider("Rot.z", "rotz", 0f, 0f, 360f);
            rotz.ValueChanged += (float value) =>
            {
                listVis.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));

            };
            scalex = AddSlider("Scale.x", "scalex", 1f, 0f, 10f);
            scalex.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;

            };
            scaley = AddSlider("Scale.y", "scaley", 1f, 0f, 10f);
            scaley.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;
            };
            scalez = AddSlider("Scale.z", "scalez", 1f, 0f, 10f);
            scalez.ValueChanged += (float value) =>
            {
                listVis.transform.localScale = new Vector3(scalex.Value, scaley.Value, scalez.Value);
                colliders.transform.localScale = Vis.transform.localScale;
            };

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
        public void displayRenderToggles(bool active)
        {
            Apply.DisplayInMapper = active;
            RGB.DisplayInMapper = active;
            hasCol.DisplayInMapper = active;
            massSlider.DisplayInMapper = active;
            healthSlider.DisplayInMapper = active;
            shaderType.DisplayInMapper = active;

            cullMode.DisplayInMapper = active;
            zwriteMode.DisplayInMapper = active;
            blendOp.DisplayInMapper = active;
            srcBlend.DisplayInMapper = active;
            dstBlend.DisplayInMapper = active;
            alphaSlider.DisplayInMapper = active;
            colourSlider.DisplayInMapper = active;
            colourSliderDif.DisplayInMapper = active;
            rimColor.DisplayInMapper = active;
            specular.DisplayInMapper = active;
            MeshName.DisplayInMapper = active;
            TexName.DisplayInMapper = active;
            LightName.DisplayInMapper = active;
            RSName.DisplayInMapper = active;
            difPower.DisplayInMapper = active;
            ambientPower.DisplayInMapper = active;
            shadowThreshold.DisplayInMapper = active;
            shadowReceiveThreshold.DisplayInMapper = active;
            shadowBrightness.DisplayInMapper = active;
            shadowSmoothness.DisplayInMapper = active;
            rimThreshold.DisplayInMapper = active;
            specularScale.DisplayInMapper = active;
        }
        public void initMat()
        {
            visMat = new Material(Shader.Find("Instanced/Block Shader (GPUI off)"));
            visMat.SetTexture("_DamageMap", renderer.material.GetTexture("_DamageMap"));
            //renderer2.materials = matArray;
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
        public void updateVis(List<Mesh> meshesInput)
        {
            listVis.SetActive(false);
            for (int i = 0; i < listVis.transform.childCount; i++)
            {
                try
                {
                    Destroy(listVis.transform.GetChild(i).gameObject);
                }
                catch { }
            }
            renderer2.Clear();
            outlines.Clear();
            for (int i = 0; i < meshesInput.Count; i++) 
            {
                GameObject thisVisBody = new GameObject("VisBody" + i.ToString());
                thisVisBody.transform.SetParent(listVis.transform);
                thisVisBody.transform.localPosition = Vector3.zero;
                thisVisBody.transform.localRotation = Quaternion.identity;
                thisVisBody.transform.localScale = Vector3.one;

                thisVisBody.AddComponent<MeshFilter>().sharedMesh = meshesInput[i];
                renderer2.Add(thisVisBody.GetComponent<MeshRenderer>() ?? thisVisBody.AddComponent<MeshRenderer>());
                renderer2[i].sharedMaterial = visMat;


                outlines.Add(thisVisBody.AddComponent<cakeslice.Outline>());
            }
            listVis.SetActive(true);
        }
        public override void SafeAwake()
        {
            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;

            Apply = AddToggle(LanguageManager.Instance.outLang.Apply, "apply", false);
            RGB = AddToggle("R G B!", "rgb", false);
            hasCol = AddToggle(LanguageManager.Instance.outLang.Collider, "hasCol", false);
            massSlider = AddSlider(LanguageManager.Instance.outLang.Mass, "mass", 2f, 0f, 10f);
            healthSlider = AddSlider(LanguageManager.Instance.outLang.Health, "health", 10f, 0f, 100f);


            alphaSlider = AddSlider(LanguageManager.Instance.outLang.Alpha, "alpha", 1.0f, 0f, 1.0f);
            difPower = AddSlider(LanguageManager.Instance.outLang.Diffuse_Strength, "difpower", 0.8f, 0f, 1.0f);
            ambientPower = AddSlider(LanguageManager.Instance.outLang.Ambient_Strength, "ambientPower", 0.2f, 0f, 1.0f);
            shadowThreshold = AddSlider(LanguageManager.Instance.outLang.Shadow_Threshold, "shadowThreshold", 0.2f, -1f, 1f);
            shadowReceiveThreshold = AddSlider(LanguageManager.Instance.outLang.Shadow_Receive_Threshold, "shadowReceiveThreshold", 0.5f, 0f, 1f);
            shadowBrightness = AddSlider(LanguageManager.Instance.outLang.Shadow_Brightness, "shadowBrightness", 0.6f, 0f, 1f);
            shadowSmoothness = AddSlider(LanguageManager.Instance.outLang.Shadow_Smoothness, "shadowSmoothness", 1.5f, 0f, 10f);
            rimThreshold = AddSlider(LanguageManager.Instance.outLang.Rim_Threshold, "rimThreshold", 0.8f, 0f, 1f);
            specularScale = AddSlider(LanguageManager.Instance.outLang.Specular_Scale, "specularScale", 0.02f, 0f, 1f);

            colourSliderDif = AddColourSlider(LanguageManager.Instance.outLang.Main_Color, "dif", new Color(1, 1, 1, 1), false);
            colourSlider = AddColourSlider(LanguageManager.Instance.outLang.Emis_Color, "hilight", new Color(1, 1, 1, 1), false);
            rimColor = AddColourSlider(LanguageManager.Instance.outLang.Rim_Color, "rimColor", new Color(1, 1, 1, 1), false);
            specular = AddColourSlider(LanguageManager.Instance.outLang.Specular_Color, "specular", new Color(1, 1, 1, 1), false);
            MeshName = AddText(LanguageManager.Instance.outLang.Meshname, "meshname", "MeshName");
            TexName = AddText(LanguageManager.Instance.outLang.Main_Tex, "texname", "TexName");
            RSName = AddText(LanguageManager.Instance.outLang.RS_Tex, "RSname", "RSName");
            LightName = AddText(LanguageManager.Instance.outLang.Emis_Tex, "lightname", "LightName");

            root = AddMenu("rootmenu", 0, new List<string> { "renderer", "transform" });
            shaderType = AddMenu("shadermenu", 0, new List<string> { "block shader", "alpha blend", "3to2", "3to2 transparent" });
            cullMode = AddMenu("CullMode", 0, new List<string> { "Cull Off", "Cull Front", "Cull Back" });
            zwriteMode = AddMenu("ZWriteMode", 1, new List<string> { "ZWrite Off", "Zwrite On" });
            blendOp = AddMenu("BlendOp", 0, Enum.GetNames(typeof(UnityEngine.Rendering.BlendOp)).ToList());
            srcBlend = AddMenu("SrcBlend", 5, Enum.GetNames(typeof(UnityEngine.Rendering.BlendMode)).ToList());
            dstBlend = AddMenu("DstBlend", 10, Enum.GetNames(typeof(UnityEngine.Rendering.BlendMode)).ToList());

            //mesh2 = Vis.GetComponent<MeshFilter>();
            renderer = Vis.GetComponent<MeshRenderer>();
            orgOutLine = Vis.GetComponent<cakeslice.Outline>();

            initMat();
            initTransformSliders();
            InitVis();

            root.ValueChanged += (int value) =>
            {
                if (root.Value == 1)
                {
                    displayRenderToggles(false);
                    displayTransformSliders(true);
                }
                else if (root.Value == 0)
                {
                    displayTransformSliders(false);
                    displayRenderToggles(true);
                }
            };
            shaderType.ValueChanged += (int value) =>
            {
                if(shaderType.Value==0)
                {
                    visMat.shader = Shader.Find("Instanced/Block Shader (GPUI off)");
                }
                else if(shaderType.Value==1)
                {
                    visMat.shader = Shader.Find("Particles/Alpha Blended");
                    Color thiscolor = colourSliderDif.Value;
                    thiscolor.a = Mathf.Max(0f, Mathf.Min(1f, alphaSlider.Value));
                    visMat.SetColor("_TintColor", thiscolor);
                }
                else if(shaderType.Value ==2)
                {
                    ModAssetBundle nijigenShaders = ModResource.GetAssetBundle("3to2_shaders");
                    //Shader nijigenOpaque = nijigenShaders.LoadAsset<Shader>("3to2_Opaque");
                    //visMat.shader = Instantiate(nijigenOpaque);
                    visMat.shader = nijigenShaders.LoadAsset<Shader>("3to2_Opaque");
                    //visMat.shader = Shader.Find("Unlit/3to2_Opaque");
                    //visMat.shader = Shader.Find("Unlit/3to2_Opaque");
                    //Debug.Log(visMat.shader);

                    initNijigen();
                }
                /*
                else if(shaderType.Value == 3)
                {
                    ModAssetBundle nijigenShaders = ModResource.GetAssetBundle("3to2_shaders");
                    visMat.shader = nijigenShaders.LoadAsset<Shader>("3to2_AlphaTest");
                    //visMat.shader = Shader.Find("Unlit/3to2_AlphaTest");

                    initNijigen();
                }
                else if (shaderType.Value == 4)
                {
                    ModAssetBundle nijigenShaders = ModResource.GetAssetBundle("3to2_shaders");
                    visMat.shader = nijigenShaders.LoadAsset<Shader>("3to2_AlphaTest+1");
                    //visMat.shader = Shader.Find("Unlit/3to2_AlphaTest+1");

                    initNijigen();
                }
                */
                else if (shaderType.Value == 3)
                {
                    ModAssetBundle nijigenShaders = ModResource.GetAssetBundle("3to2_shaders");
                    visMat.shader = nijigenShaders.LoadAsset<Shader>("3to2_Transparent");
                    //visMat.shader = Shader.Find("Unlit/3to2_Transparent");

                    initNijigen();
                }
            };
            try
            {
                colourSlider.ValueChanged += (Color color) =>
                {
                    try
                    {
                        visMat.SetColor("_EmissCol", colourSlider.Value);
                        visMat.SetColor("_EmisColor", colourSlider.Value);
                    }
                    catch { }
                };
                colourSliderDif.ValueChanged += (Color color) =>
                {
                    try
                    {
                        Color thiscolor = colourSliderDif.Value;
                        thiscolor.a = Mathf.Max(0f, Mathf.Min(1f, alphaSlider.Value));
                        visMat.SetColor("_TintColor", thiscolor);
                    }
                    catch { }
                    try
                    {
                        visMat.SetColor("_Color", colourSliderDif.Value);
                    }
                    catch { }
                };
                rimColor.ValueChanged += (Color color) =>
                {
                    visMat.SetColor("_RimColor", rimColor.Value);
                };
                specular.ValueChanged += (Color color) =>
                {
                    visMat.SetColor("_Specular", specular.Value);
                };
                alphaSlider.ValueChanged += (float value) =>
                {
                    Color thiscolor = colourSliderDif.Value;
                    thiscolor.a = Mathf.Max(0f, Mathf.Min(1f, alphaSlider.Value));
                    visMat.SetColor("_Color", thiscolor);
                    visMat.SetColor("_TintColor", thiscolor);
                };
                difPower.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_DiffusePower", difPower.Value);
                };
                ambientPower.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_AmbientPower", ambientPower.Value);
                };
                cullMode.ValueChanged += (int value) =>
                {
                    visMat.SetFloat("_CullMode", cullMode.Value);
                };
                zwriteMode.ValueChanged += (int value) =>
                {
                    visMat.SetFloat("_ZWriteMode", zwriteMode.Value);
                };
                blendOp.ValueChanged += (int value) =>
                {
                    visMat.SetFloat("_BlendOp", blendOp.Value);
                };
                srcBlend.ValueChanged += (int value) =>
                {
                    visMat.SetFloat("_SrcBlend", srcBlend.Value);
                };
                dstBlend.ValueChanged += (int value) =>
                {
                    visMat.SetFloat("_DstBlend", dstBlend.Value);
                };
                shadowThreshold.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_ShadowThreshold", shadowThreshold.Value);
                };
                shadowReceiveThreshold.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_ShadowReceiveThreshold", shadowReceiveThreshold.Value);
                };
                shadowBrightness.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_ShadowBrightness", shadowBrightness.Value);
                };
                shadowSmoothness.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_ShadowSmoothness", shadowSmoothness.Value);
                };
                rimThreshold.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_RimThreshold", rimThreshold.Value);
                };
                specularScale.ValueChanged += (float value) =>
                {
                    visMat.SetFloat("_SpecularScale", specularScale.Value);
                };
            }
            catch { }

            BlockBehaviour.name = "VisBlock";
        }
        public override void SimulateFixedUpdateHost()
        {
            if (!BlockBehaviour.noRigidbody && !hasCol.IsActive) 
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

        }
        public void Update()
        {
            if (BlockBehaviour.isSimulating)
            {
                if (RGB.IsActive)
                {
                    if (shaderType.Value == 0)
                        visMat.SetColor("_EmissCol", RGBController.Instance.outputColor);
                    else if (shaderType.Value == 1)
                    {
                        Color thiscolor = RGBController.Instance.outputColor;
                        thiscolor.a = Mathf.Max(0f, Mathf.Min(1f, alphaSlider.Value));
                        visMat.SetColor("_TintColor", thiscolor);
                    }
                    else if(shaderType.Value >= 2 && shaderType.Value <= 5)
                        visMat.SetColor("_EmisColor", RGBController.Instance.outputColor);
                    //HiLightColor = RGBController.Instance.outputColor;
                }
                if (shaderType.Value == 0)
                {
                    renderer.GetPropertyBlock(VisPropertyBlock);
                    //float damageAmout=renderer.sharedMaterial.GetFloat("_DamageAmount");
                    visMat.SetFloat("_DamageAmount", VisPropertyBlock.GetFloat("_DamageAmount"));
                    //renderer2[0].GetPropertyBlock(VisPropertyBlock);
                    //VisPropertyBlock.SetFloat("_DamageAmount", damageAmout);
                    //renderer2[0].SetPropertyBlock(VisPropertyBlock);
                }
            }
            if (OptionsMaster.skinsEnabled != mySkinCode) 
            {
                mySkinCode = OptionsMaster.skinsEnabled;
                listVis.SetActive(mySkinCode);
                Vis.SetActive(!mySkinCode);
            }
            if (StatMaster.clusterCoded != myClusterCode)
            {
                myClusterCode = StatMaster.clusterCoded;
                if(myClusterCode)
                {
                    foreach (var a in renderer2)
                        a.sharedMaterial = renderer.material;
                }
                else
                {
                    foreach (var a in renderer2)
                        a.sharedMaterial = visMat;
                }
            }

        }
        public void Start()
        {

            BlockBehaviour.Rigidbody.drag = 0.0f;
            BlockBehaviour.Rigidbody.angularDrag = 0.0f;
            BlockBehaviour.blockJoint.breakForce = 100000f;
            BlockBehaviour.blockJoint.breakTorque = 100000f;
            BlockBehaviour.noRigidbody = false;

            //Vis.SetActive(false);

            //if (listVis.transform.childCount == 0)
            applyVis();
            //HiLightColor = colourSlider.Value;
            //renderer2.materials = matArray;

            BlockBehaviour.BlockHealth.health = Mathf.Max(1f, healthSlider.Value);
            BlockBehaviour.BlockHealth.Init(BlockBehaviour.ParentMachine, BlockBehaviour);
            BlockBehaviour.Rigidbody.mass = massSlider.Value;

            //applyVis();
        }
        public override void OnSimulateStart()
        {
            if (!hasCol.IsActive)
                colliders.SetActive(false);
        }
        public override void OnSimulateStop()
        {
        }
        public override void BuildingUpdate()
        {
            //if (renderer2.materials.Length < 2)
            //    renderer2.materials = matArray;
            if (Apply.IsActive)
            {
                Apply.IsActive = false;
                applyVis();
            }
            try
            {
                foreach (var a in outlines)
                {
                    a.enabled = orgOutLine.enabled;
                    a.color = orgOutLine.color;
                }
            }
            catch { }
        }
        public void applyVis()
        {
            ResourceController.Instance.addTex(TexName.Value);
            Texture2D loadtex= ResourceController.Instance.getTex(TexName.Value);
            if (loadtex != null)
            {
                visMat.SetTexture("_MainTex", loadtex);
            }
            ResourceController.Instance.addTex(RSName.Value);
            Texture2D rstex = ResourceController.Instance.getTex(RSName.Value);
            if (loadtex != null)
            {
                visMat.SetTexture("_RSTex", rstex);
            }
            ResourceController.Instance.addTex(LightName.Value);
            Texture2D lighttex = ResourceController.Instance.getTex(LightName.Value);
            if (lighttex != null)
            {
                visMat.SetTexture("_EmissMap", lighttex);
                visMat.SetTexture("_EmisTex", lighttex);
            }
            ResourceController.Instance.addMesh(MeshName.Value);
            List<Mesh> loadmesh= ResourceController.Instance.getMesh(MeshName.Value);
            if (loadmesh != null)
                if (loadmesh.Count > 0)
                    updateVis(loadmesh);
        }
        /*
        readonly GUIStyle vec3Style = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            normal = { textColor = new Color(1, 1, 1, 1) },
            alignment = TextAnchor.MiddleCenter,
        };


        void OnGUI()
        {
            //string showmsg = "";

            //GUI.Box(UIrect, showmsg);

        }
        */
    }
}
