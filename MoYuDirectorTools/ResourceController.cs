﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Modding;
using InternalModding;
using Modding.Common;
using UnityEngine;

namespace MoYuDirectorTools
{
    class ResourceController: SingleInstance<ResourceController>
    {
        public override string Name { get; } = "Resource Controller";

        public GameObject skybox;
        public GameObject orgskybox;
        public UnityEngine.Light mainLight;
        public Material[] matArray = new Material[2];
        public bool showgui = true;

        public bool nofog = false;
        public bool nofloor = false;
        public bool noenv = false;
        public bool nobound = false;
        public string fogDensity = "0.004";
        public string fogStartDis = "140";
        public string[] lightEular = new string[3];

        public float scrollY = 45f;
        public float scrollX = 320f;
        public float evnY = 20f;
        public string skytexName = "";
        public string skyBK = "0,0,0,0";
        public string skyCol = "1,1,1,1";
        public string fogCol = "1,1,1,1";
        public string lightCol = "";
        public string ambientCol = "";
        public bool isFirstFrame = true;
        ModAudioClip MAC;
        public float ButtonHeight = 20f;
        public Vector2 scrollpos = Vector2.zero;
        public List<Texture2D> texs = new List<Texture2D>();
        public Dictionary<string, List<Mesh>> meshes = new Dictionary<string, List<Mesh>>();
        public List<ModAudioClip> audios = new List<ModAudioClip>();
        //List<List<int>> vtest = new List<List<int>>();
        private string ResourcePath = "Resource";
        private bool isdata = true;
        private Rect windowRect = new Rect(15f, 400f, 630f, 365f);
        private readonly int windowID = ModUtility.GetWindowId();

        public ResourceController()
        {
            if (!Modding.ModIO.ExistsDirectory(ResourcePath, isdata))
                Modding.ModIO.CreateDirectory(ResourcePath, isdata);
            lightEular[0] = "";
            lightEular[1] = "";
            lightEular[2] = "";
        }
        public void Update()
        {
            if (!isFirstFrame)
            {
                if (StatMaster.isMainMenu)
                {
                    isFirstFrame = true;
                    Destroy(skybox);
                }
            }
            else
            {
                if (!StatMaster.isMainMenu) 
                {
                    isFirstFrame = false;
                    //orgskybox = (GameObject.Find("MULTIPLAYER LEVEL").transform.FindChild("Environments").FindChild("Barren").FindChild("AviamisAtmosphere").FindChild("STAR SPHERE").gameObject);

                    skybox= new GameObject("MoYu Sky Box");

                    matArray[0]= new Material(Shader.Find("Instanced/Block Shader (GPUI off)"));
                    matArray[0].SetTexture("_EmissMap", Texture2D.whiteTexture);
                    matArray[0].SetTexture("_MainTex", Texture2D.blackTexture);
                    matArray[0].SetColor("_Color", Color.black);
                    matArray[0].SetColor("_EmissCol", Color.white);
                    matArray[1] = new Material(Shader.Find("Particles/Additive"));
                    matArray[1].mainTexture = Texture2D.whiteTexture;
                    matArray[1].SetColor("_TintColor", Color.white);
                    skybox.AddComponent<MeshFilter>().mesh = ModResource.GetMesh("SkyBall").Mesh;
                    skybox.AddComponent<MeshRenderer>().materials = matArray;
                    skybox.GetComponent<MeshRenderer>().sortingOrder = -32768;


                    //skybox.GetComponent<MeshRenderer>().material.SetColor("_Emission", Color.white);
                    //skybox.GetComponent<MeshRenderer>().material.renderQueue = 4000;
                    skybox.transform.localPosition = Vector3.zero;
                    skybox.transform.localScale = Vector3.one * 800f;

                    skybox.SetActive(false);

                    getlight();
                }
            }
            if(!isFirstFrame)
            {
                if (Input.GetKey("left ctrl") && Input.GetKeyDown("d")) 
                    showgui = !showgui;
                if (skybox.activeSelf)
                    skybox.transform.position = Camera.main.transform.position;
            }
        }
        public Color str2color(string tgt, Color orgColor)
        {
            Color output = orgColor;
            try
            {
                string[] colors = tgt.Split(',');
                if (colors.Length != 4)
                    return output;

                output.r = Convert.ToSingle(colors[0]);
                output.g = Convert.ToSingle(colors[1]);
                output.b = Convert.ToSingle(colors[2]);
                output.a = Convert.ToSingle(colors[3]);
            }
            catch { }
            return output;
        }
        private System.Collections.IEnumerator updateSkybox()
        {
            if (skybox == null)  
                yield break;
            
            addTex(skytexName);
            Texture2D loadtex = getTex(skytexName);
            if (loadtex == null)
                yield break;


            matArray[0].SetColor("_EmissCol", str2color(skyBK, matArray[0].GetColor("_EmissCol")));
            matArray[1].SetColor("_TintColor", str2color(skyCol, matArray[1].GetColor("_TintColor")));
            matArray[1].mainTexture = loadtex;
            skybox.SetActive(true);
            skybox.GetComponent<MeshRenderer>().materials = matArray;
            //skybox.GetComponent<MeshRenderer>().material.SetTexture("_EmissMap", loadtex);
            //skybox.GetComponent<MeshRenderer>().material.renderQueue = 4000;

            yield break;
        }
        private System.Collections.IEnumerator setColorFog()
        {
            foreach (var a in Camera.main.GetComponents<ColorfulFog>())
            {
                a.solidColor = str2color(fogCol, a.solidColor);
                a.fogDensity = Convert.ToSingle(fogDensity);
                a.startDistance = Convert.ToSingle(fogStartDis);
                a.coloringMode = ColorfulFog.ColoringMode.Solid;
            }
            yield break;
        }
        private System.Collections.IEnumerator setfog()
        {
            nofog = !nofog;
            try
            {
                GameObject.Find("FOG SPHERE").GetComponent<MeshRenderer>().enabled = !nofog;
            }
            catch { }

            try
            {
                GameObject.Find("Fog Volume").GetComponent<MeshRenderer>().enabled = !nofog;
            }
            catch { }
            try
            {
                Camera.main.transform.FindChild("Fog Volume Dark (1)").GetComponent<MeshRenderer>().enabled = !nofog;
            }
            catch { }

            yield break;
        }
        private System.Collections.IEnumerator setfloor()
        {
            nofloor = !nofloor;
            if (StatMaster.isMP)
            {
                try
                {
                    GameObject.Find("MULTIPLAYER LEVEL").transform.FindChild("FloorBig").gameObject.SetActive(!nofloor);
                }
                catch { }
            }
            else
            {
                try
                {
                    GameObject.Find("LEVEL BARREN EXPANSE").transform.FindChild("FloorBig").gameObject.SetActive(!nofloor);
                }
                catch { }
                try
                {
                    GameObject.Find("LEVEL SANDBOX").transform.FindChild("FloorBig").gameObject.SetActive(!nofloor);
                }
                catch { }
                try
                {
                    GameObject.Find("LEVEL MISTY MOUNTAIN").transform.FindChild("FloorBig").gameObject.SetActive(!nofloor);
                }
                catch { }
            }
            yield break;
        }
        private System.Collections.IEnumerator setenv()
        {
            noenv = !noenv;
            if (StatMaster.isMP)
            {
                try
                {
                    GameObject.Find("MULTIPLAYER LEVEL").transform.FindChild("Environments").FindChild("Barren").FindChild("BarrenEnv").gameObject.SetActive(!noenv);
                }
                catch { }
                try
                {
                    GameObject.Find("MULTIPLAYER LEVEL").transform.FindChild("Environments").FindChild("MountainTop").FindChild("STATIC").gameObject.SetActive(!noenv);
                    GameObject.Find("MULTIPLAYER LEVEL").transform.FindChild("Environments").FindChild("MountainTop").FindChild("PHYSICS GOAL").gameObject.SetActive(!noenv);
                }
                catch { }
            }
            else
            {
                try
                {
                    GameObject.Find("LEVEL BARREN EXPANSE").transform.FindChild("STATIC").gameObject.SetActive(!noenv);
                }
                catch { }
                try
                {
                    GameObject.Find("LEVEL SANDBOX").transform.FindChild("STATIC").gameObject.SetActive(!noenv);
                }
                catch { }
                try
                {
                    GameObject.Find("LEVEL MISTY MOUNTAIN").transform.FindChild("STATIC").gameObject.SetActive(!noenv);
                }
                catch { }
            }
            yield break;
        }
        private System.Collections.IEnumerator setbounds()
        {
            nobound = !nobound;
            try
            {
                GameObject wbound = GameObject.Find("WORLD BOUNDARIES").gameObject;
                for(int i=0;i<wbound.transform.childCount;i++)
                {
                    wbound.transform.GetChild(i).gameObject.SetActive(!nobound);
                }
            }
            catch { }
            yield break;
        }
        public void getlight()
        {
            if (StatMaster.isMP)
            {
                try
                {
                    mainLight = GameObject.Find("Environments").transform.FindChild("Directional light").gameObject.GetComponent<UnityEngine.Light>();
                }
                catch { }
            }
            else
            {
                try
                {
                    mainLight = GameObject.Find("AviamisAtmosphere").transform.FindChild("Directional light").gameObject.GetComponent<UnityEngine.Light>();
                }
                catch { }
                try
                {
                    mainLight = GameObject.Find("ATMOSPHERE").transform.FindChild("Directional light").gameObject.GetComponent<UnityEngine.Light>();
                }
                catch { }

            }
            if(mainLight!=null)
            {
                lightEular[0] = mainLight.transform.rotation.eulerAngles.x.ToString();
                lightEular[1] = mainLight.transform.rotation.eulerAngles.y.ToString();
                lightEular[2] = mainLight.transform.rotation.eulerAngles.z.ToString();
                lightCol = mainLight.color.ToString().Replace("(", "").Replace(")", "").Replace("RGBA", "").Replace(" ", "");
                ambientCol = RenderSettings.ambientLight.ToString().Replace("(", "").Replace(")", "").Replace("RGBA", "").Replace(" ", "");
            }
        }
        public System.Collections.IEnumerator setlight()
        {
            if (mainLight != null) 
            {
                mainLight.transform.rotation = Quaternion.Euler(Convert.ToSingle(lightEular[0]), Convert.ToSingle(lightEular[1]), Convert.ToSingle(lightEular[2]));
                mainLight.color = str2color(lightCol, mainLight.color);
                RenderSettings.ambientLight = str2color(ambientCol, RenderSettings.ambientLight);
            }
            yield break;
        }
        public System.Collections.IEnumerator setfixedRGB()
        {
            RGBController.Instance.fixedRGB = !RGBController.Instance.fixedRGB;
            yield break;
        }
        public void Start()
        {
            //cameraController = FindObjectOfType<FixedCameraController>();
        }
        private void OnGUI()
        {
            if (StatMaster.isMainMenu)
                return;
            if (!StatMaster.hudHidden && showgui) 
            {
                windowRect = GUILayout.Window(windowID, windowRect, new GUI.WindowFunction(ResourceWindow), "MoYu Director Tools (LCtrl+D to hide)");
            }
        }
        private void ResourceWindow(int windowID)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            {
                GUILayout.BeginVertical();
                if (GUI.Button(new Rect(scrollX, 20f, 200f, 20f), "Open resource folder")) 
                {
                    Modding.ModIO.OpenFolderInFileBrowser(ResourcePath, isdata);
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                if (GUI.Button(new Rect(scrollX + 210f, 20f, 90f, 20f), "Clear")) 
                {
                    meshes.Clear();
                    texs.Clear();
                    //audios.Clear();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                GUI.Label(new Rect(10f, evnY, 140f, 20f), "Environment Controll");
                if (GUI.Button(new Rect(150f, evnY, 160f, 20f), "Apply Sky Box"))
                {
                    StartCoroutine(updateSkybox());
                }
                GUI.Label(new Rect(10f, evnY + 25f, 170f, 20f), "Sky Box Texture");
                skytexName = GUI.TextField(new Rect(190f, evnY + 25f, 120f, 20f), skytexName);
                GUI.Label(new Rect(10f, evnY + 50f, 170f, 20f), "Sky Texture Color(RGBA)");
                skyCol = GUI.TextField(new Rect(190f, evnY + 50f, 120f, 20f), skyCol);
                GUI.Label(new Rect(10f, evnY + 75f, 170f, 20f), "Background Color(RGBA)");
                skyBK = GUI.TextField(new Rect(190f, evnY + 75f, 120f, 20f), skyBK);
                if (GUI.Button(new Rect(10f, evnY + 100f, 145f, 20f), "No Basic Fog")) 
                {
                    StartCoroutine(setfog());
                }
                if (GUI.Button(new Rect(165f, evnY + 100f, 145f, 20f), "Apply Fog Color")) 
                {
                    StartCoroutine(setColorFog());
                }
                GUI.Label(new Rect(10f, evnY + 125f, 140f, 20f), "Fog Start Distance");
                fogStartDis = GUI.TextField(new Rect(150f, evnY + 125f, 160f, 20f), fogStartDis);
                GUI.Label(new Rect(10f, evnY + 150f, 140f, 20f), "Fog Density");
                fogDensity = GUI.TextField(new Rect(150f, evnY + 150f, 160f, 20f), fogDensity);
                GUI.Label(new Rect(10f, evnY + 175f, 140f, 20f), "Fog Color(RGBA)");
                fogCol = GUI.TextField(new Rect(150f, evnY + 175f, 160f, 20f), fogCol);
                //if (StatMaster.isMP)
                {
                    if (GUI.Button(new Rect(10f, evnY + 200f, 93f, 20f), "No Floor"))
                    {
                        StartCoroutine(setfloor());
                    }
                    if (GUI.Button(new Rect(113f, evnY + 200f, 93f, 20f), "No Env"))
                    {
                        StartCoroutine(setenv());
                    }
                    if (GUI.Button(new Rect(216f, evnY + 200f, 93f, 20f), "No Bound"))
                    {
                        StartCoroutine(setbounds());
                    }
                }
                GUI.Label(new Rect(10f, evnY + 225f, 140f, 20f), "Main Light Controll");
                if (GUI.Button(new Rect(165f, evnY + 225f, 145f, 20f), "Apply Main Light"))
                {
                    StartCoroutine(setlight());
                }
                GUI.Label(new Rect(10f, evnY + 245f, 140f, 20f), "Main Light Rotation");
                RGBController.Instance.fixedRGB = GUI.Toggle(new Rect(165f, evnY + 245f, 145f, 20f), RGBController.Instance.fixedRGB, "Fixed RGB");
                GUI.Label(new Rect(10f, evnY + 265f, 20f, 20f), "x:");
                lightEular[0] = GUI.TextField(new Rect(30f, evnY + 265f, 70f, 20f), lightEular[0]);
                GUI.Label(new Rect(110f, evnY + 265f, 20f, 20f), "y:");
                lightEular[1] = GUI.TextField(new Rect(130f, evnY + 265f, 70f, 20f), lightEular[1]);
                GUI.Label(new Rect(210f, evnY + 265f, 20f, 20f), "z:");
                lightEular[2] = GUI.TextField(new Rect(230f, evnY + 265f, 70f, 20f), lightEular[2]);
                
                GUI.Label(new Rect(10f, evnY + 290f, 170f, 20f), "Light Color(RGBA)");
                lightCol = GUI.TextField(new Rect(190f, evnY + 290f, 120f, 20f), lightCol);
                GUI.Label(new Rect(10f, evnY + 315f, 170f, 20f), "Ambient Color(RGBA)");
                ambientCol = GUI.TextField(new Rect(190f, evnY + 315f, 120f, 20f), ambientCol);
                
                GUILayout.EndVertical();
                GUILayout.BeginVertical();

                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            {
                //StatMaster.collapseSkinMapper
                GUI.Label(new Rect(scrollX, scrollY, 300f, 20f), "Resources Controll");
                GUI.Label(new Rect(scrollX, scrollY + 15f, 300f, 20f), "(click to remove, except audios)");
                //GUI.Label(new Rect(10f, 50f, 300f, 20f), "Resources (click to remove, except audios):");
                ButtonHeight = (meshes.Count + texs.Count + audios.Count) * 25f;
                GUI.Box(new Rect(scrollX, scrollY + 35f, 300f, 365f - scrollY - 45f), "");
                scrollpos = GUI.BeginScrollView(new Rect(scrollX+5f, scrollY+35f, 290f, 365f - scrollY - 55f), scrollpos, new Rect(0f, 0f, 250f, ButtonHeight));
                GUILayout.BeginArea(new Rect(0f, 0f, 300f, ButtonHeight));
                try
                {
                    foreach (var a in meshes)
                    {
                        //GUILayout.Label(a.name);
                        if (GUILayout.Button(a.Key, new GUILayoutOption[]{GUILayout.Width(260f),GUILayout.Height(20f)}))
                        {
                            meshes.Remove(a.Key);
                        }
                    }
                }
                catch { }
                try
                {
                    foreach (var a in texs)
                    {
                        if (GUILayout.Button(a.name, new GUILayoutOption[] { GUILayout.Width(260f), GUILayout.Height(20f) }))
                        {
                            texs.Remove(a);
                        }
                    }
                }
                catch { }
                try
                {
                    foreach (var a in audios)
                    {
                        if (GUILayout.Button(a.Name, new GUILayoutOption[] { GUILayout.Width(260f), GUILayout.Height(20f) }))
                        {
                            //audios.Remove(a);
                            
                        }
                    }
                }
                catch { }
                GUILayout.EndArea();
                GUI.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
        public void addTex(string filename)
        {
            
            if (!Modding.ModIO.ExistsFile(ResourcePath + "/" + filename, isdata))
                return;
            
            if (texs.Count > 0)
            {
                foreach (var a in texs)
                {
                    if (a.name == filename)
                        return;
                }
            }
            
            //texbytes = 
            texs.Add(new Texture2D(32, 32, TextureFormat.ARGB32, false, false));
            texs[texs.Count - 1].LoadImage(Modding.ModIO.ReadAllBytes(ResourcePath + "/" + filename, isdata));
            texs[texs.Count - 1].name = filename;

        }
        public Texture2D getTex(string filename)
        {
            if (texs.Count == 0)
                return null;
            foreach (var a in texs)
            {
                if (a.name == filename)
                    return a;
            }
            return null;
        }
        public List<Mesh> loadMesh(string MeshFile)
        {
            string[] lines = Modding.ModIO.ReadAllLines(MeshFile, isdata);
            List<Vector3> meshVertices = new List<Vector3>();
            List<Vector2> meshUVs = new List<Vector2>();
            List<Vector3> meshNormals = new List<Vector3>();
            List<List<int>> meshTriangles = new List<List<int>>();
            
            List<Mesh> output = new List<Mesh>();

            foreach (var line in lines) 
            {
                if (line.Length < 4)
                    continue;
                else if (line[0] == 'v' && line[1] == ' ')
                {
                    string[] vector = line.Split(' ');
                    meshVertices.Add(new Vector3(float.Parse(vector[1]), float.Parse(vector[2]), float.Parse(vector[3])));
                }
                else if (line[0] == 'v' && line[1] == 't' && line[2] == ' ')
                {
                    string[] UV = line.Split(' ');
                    meshUVs.Add(new Vector2(float.Parse(UV[1]), float.Parse(UV[2])));
                }
                else if (line[0] == 'v' && line[1] == 'n' && line[2] == ' ')
                {
                    string[] normal = line.Split(' ');
                    meshNormals.Add(new Vector3(float.Parse(normal[1]), float.Parse(normal[2]), float.Parse(normal[3])));
                }
                else if (line[0] == 'f' && line[1] == ' ')
                {
                    string[] triangles= line.Split(' ');
                    meshTriangles.Add(new List<int>());
                    foreach (var triangle in triangles)
                    {
                        if (triangle[0] == 'f')
                            continue;
                        string[] indexes = triangle.Split('/');
                        foreach(string index in indexes)
                        {
                            meshTriangles[meshTriangles.Count - 1].Add(int.Parse(index));
                        }
                    }
                }
            }
            
            List<Vector3> varray = new List<Vector3>();
            List<Vector2> uvarray = new List<Vector2>();
            List<Vector3> narray = new List<Vector3>();
            List<int> trarray = new List<int>();

            int vcount = 0;
            foreach(var f in meshTriangles)
            {
                int times = f.Count / 3;
                if (times + vcount >= 60000 || (trarray.Count / 3) + times - 2 >= 20000) 
                {
                    Mesh thisoutput = new Mesh();
                    thisoutput.vertices = varray.ToArray();
                    thisoutput.uv = uvarray.ToArray();
                    thisoutput.normals = narray.ToArray();
                    thisoutput.triangles = trarray.ToArray();
                    thisoutput.RecalculateBounds();
                    output.Add(thisoutput);

                    vcount = 0;
                    varray.Clear();
                    uvarray.Clear();
                    narray.Clear();
                    trarray.Clear();
                }
                for (int i = 0; i < times; i++) 
                {
                    varray.Add(meshVertices[f[0 + i * 3] - 1]);
                    uvarray.Add(meshUVs[f[1 + i * 3] - 1]);
                    narray.Add(meshNormals[f[2 + i * 3] - 1]);
                }
                for (int i = 0; i < times - 2; i++) 
                {
                    trarray.Add(vcount);
                    trarray.Add(vcount + i + 1);
                    trarray.Add(vcount + i + 2);
                }
                vcount += times;
            }
            
            //vtest = meshTriangles;
            Mesh lastoutput = new Mesh();
            lastoutput.vertices = varray.ToArray();
            lastoutput.uv = uvarray.ToArray();
            lastoutput.normals = narray.ToArray();
            lastoutput.triangles = trarray.ToArray();
            lastoutput.RecalculateBounds();
            output.Add(lastoutput);
            return output;
        }
        public void addMesh(string filename)
        {
            if (meshes.ContainsKey(filename))
                return;
            if (!Modding.ModIO.ExistsFile(ResourcePath + "/" + filename, isdata))
                return;

            meshes.Add(filename, loadMesh(ResourcePath + "/" + filename));
            
            //meshes[meshes.Count - 1].mesh = loadMesh(ResourcePath + "/" + filename);
            //meshes[meshes.Count - 1].name = filename;
        }
        public List<Mesh> getMesh(string filename)
        {
            if (meshes.Count == 0)
                return null;
            foreach (var a in meshes)
            {
                if (a.Key == filename)
                    return a.Value;
            }
            return null;
        }
        public void addAudio(string filename)
        {

            if (!Modding.ModIO.ExistsFile(ResourcePath + "/" + filename, isdata))
                return;

            if (audios.Count > 0)
            {
                foreach (var a in audios)
                {
                    if (a.Name == filename)
                        return;
                }
            }

            //texbytes = 
            try
            {
                audios.Add(ModResource.CreateAudioClipResource(filename, ResourcePath + "/" + filename, isdata));
                
            }
            catch { }
        }
        public AudioClip getAudio(string filename)
        {
            if (audios.Count == 0)
                return null;
            foreach (var a in audios)
            {
                if (a.Name == filename)
                {
                    return a.AudioClip;
                    //return a;
                }
            }
            return null;
        }
    }
}
