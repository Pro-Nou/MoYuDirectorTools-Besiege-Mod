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
    class TextBlock : BlockScript
    {
        public MToggle isVisible;

        public GameObject colliders;
        public GameObject Vis;
        public GameObject TextBody;

        public MColourSlider textColor;
        public MSlider textAlpha;
        public MToggle RGB;
        public MText textItem;

        public MMenu root;
        public MSlider posx;
        public MSlider posy;
        public MSlider posz;
        public MSlider rotx;
        public MSlider roty;
        public MSlider rotz;
        public MSlider scalex;
        public MSlider scaley;

        public TextMesh textMesh;
        public MeshRenderer textRenderer;
        public void initText()
        {
            if(BlockBehaviour.transform.FindChild("TextBody")==null)
            {
                TextBody = new GameObject("TextBody");
                TextBody.transform.SetParent(BlockBehaviour.transform);
                TextBody.transform.localPosition = Vector3.zero;
                TextBody.transform.localRotation = Quaternion.identity;
                TextBody.transform.localScale = Vector3.one;

                textMesh = TextBody.GetComponent<TextMesh>() ?? TextBody.AddComponent<TextMesh>();
                textMesh.text = "example";
                textMesh.alignment = TextAlignment.Center;
                textMesh.anchor = TextAnchor.MiddleCenter;
                textMesh.color = new Color(1f, 1f, 1f, 1f);

                GameObject floatingText = PrefabMaster.LevelPrefabs[8].GetValue(9002).gameObject;
                floatingText = floatingText.transform.FindChild("OneLine").FindChild("Line1").gameObject;
                //Debug.Log(floatingText.name);
                textMesh.font = floatingText.GetComponent<TextMesh>().font;
                textMesh.characterSize = 0.12f;
                textMesh.fontSize = 80;

                textRenderer = TextBody.GetComponent<MeshRenderer>() ?? TextBody.AddComponent<MeshRenderer>();
                textRenderer.material = new Material(Shader.Find("GUI/3D Text Shader Textured"));
                textRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 1f));
                textRenderer.material.SetTexture("_MainTex", floatingText.GetComponent<MeshRenderer>().material.mainTexture);

                /*
                RenderTexture tmp = RenderTexture.GetTemporary(
                      textRenderer.material.mainTexture.width,
                     textRenderer.material.mainTexture.height,
                     0,
                    RenderTextureFormat.Default,
                      RenderTextureReadWrite.Linear);

                Graphics.Blit(textRenderer.material.mainTexture, tmp);

                RenderTexture previous = RenderTexture.active;

                RenderTexture.active = tmp;

                Texture2D myTexture2D = new Texture2D(textRenderer.material.mainTexture.width, textRenderer.material.mainTexture.height);

                myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
                myTexture2D.Apply();

                RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(tmp);

                Byte[] bytes = myTexture2D.EncodeToPNG();
                Modding.ModIO.WriteAllBytes("Font.png", bytes, true);
                */
            }
        }
        public void initTransformSliders()
        {
            posx = AddSlider("Pos.x", "posx", 0f, -10f, 10f);
            posx.ValueChanged += (float value) =>
            {
                TextBody.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posy = AddSlider("Pos.y", "posy", 0f, -10f, 10f);
            posy.ValueChanged += (float value) =>
            {
                TextBody.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            posz = AddSlider("Pos.z", "posz", 1f, -10f, 10f);
            posz.ValueChanged += (float value) =>
            {
                TextBody.transform.localPosition = new Vector3(posx.Value, posy.Value, posz.Value);
                colliders.transform.localPosition = Vis.transform.localPosition;
            };
            rotx = AddSlider("Rot.x", "rotx", 0f, 0f, 360f);
            rotx.ValueChanged += (float value) =>
            {
                TextBody.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));
            };
            roty = AddSlider("Rot.y", "roty", 180f, 0f, 360f);
            roty.ValueChanged += (float value) =>
            {
                TextBody.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));

            };
            rotz = AddSlider("Rot.z", "rotz", 0f, 0f, 360f);
            rotz.ValueChanged += (float value) =>
            {
                TextBody.transform.localRotation = Quaternion.Euler(new Vector3(rotx.Value, roty.Value, rotz.Value));

            };
            scalex = AddSlider("Scale.x", "scalex", 1f, 0f, 10f);
            scalex.ValueChanged += (float value) =>
            {
                TextBody.transform.localScale = new Vector3(scalex.Value, scaley.Value, 1f);
                colliders.transform.localScale = Vis.transform.localScale;

            };
            scaley = AddSlider("Scale.y", "scaley", 1f, 0f, 10f);
            scaley.ValueChanged += (float value) =>
            {
                TextBody.transform.localScale = new Vector3(scalex.Value, scaley.Value, 1f);
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
        }
        public void displayRenderToggles(bool active)
        {
            isVisible.DisplayInMapper = active;
            textColor.DisplayInMapper = active;
            textAlpha.DisplayInMapper = active;
            RGB.DisplayInMapper = active;
            textItem.DisplayInMapper = active;
        }
        public override void SafeAwake()
        {
            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;

            root = AddMenu("rootmenu", 0, new List<string> { "renderer", "transform" });
            isVisible = AddToggle(LanguageManager.Instance.outLang.Visible, "isVisible", true);
            RGB = AddToggle("R G B!", "rgb", false);
            textColor = AddColourSlider(LanguageManager.Instance.outLang.Text_Color, "textColor", new Color(1f, 1f, 1f, 1f), false);
            textAlpha = AddSlider(LanguageManager.Instance.outLang.Text_Alpha, "textAlpha", 1f, 0f, 1f);
            textItem = AddText(LanguageManager.Instance.outLang.Text_Item, "textItem", "Example");
            
            initText();
            initTransformSliders();
            textItem.TextChanged += (string value) =>
            {
                try
                {
                    textMesh.text = textItem.Value;
                }
                catch { }
            };
            textColor.ValueChanged +=(Color value)=>
            {
                try
                {
                    Color thisColor = textColor.Value;
                    thisColor.a = Mathf.Max(0f, Mathf.Min(1f, textAlpha.Value));
                    textRenderer.material.SetColor("_Color", thisColor);
                }
                catch { }
            };
            textAlpha.ValueChanged += (float value) =>
            {
                try
                {
                    Color thisColor = textColor.Value;
                    thisColor.a = Mathf.Max(0f, Mathf.Min(1f, textAlpha.Value));
                    textRenderer.material.SetColor("_Color", thisColor);
                }
                catch { }
            };
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
            BlockBehaviour.name = "Text Block";
        }
        public override void OnSimulateStart()
        {
            colliders.SetActive(false);
            if (!isVisible.IsActive)
                Vis.SetActive(false);
        }
        public void Start()
        {
            BlockBehaviour.noRigidbody = false;
            textRenderer = TextBody.GetComponent<MeshRenderer>();
        }
        public void Update()
        {
            if(BlockBehaviour.isSimulating)
            {
                if(RGB.IsActive)
                {
                    Color thisColor = RGBController.Instance.outputColor;
                    thisColor.a = Mathf.Max(0f, Mathf.Min(1f, textAlpha.Value));
                    textRenderer.material.SetColor("_Color", thisColor);
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
        }
    }
}
