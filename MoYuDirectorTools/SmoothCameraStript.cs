using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using Modding;
using Modding.Blocks;

namespace MoYuDirectorTools
{
    class SmoothCameraStript : MonoBehaviour
    {
        private Rect UIrect = new Rect(256, 100, 256, 256);
        public BlockBehaviour BB { get; internal set; }

        //public MText CameraNum;
        public MToggle smoothActive;
        public MSlider lifeTimeSlider;
        public MMenu smoothMode;
        public MToggle trackActive;
        public MKey picker;
        public MKey resetPicker;

        public Collider pickedCollider;
        public bool isTracking = false;
        public GameObject Tracker;
        private bool activating = false;
        private bool activating2 = false;

        public float lifetime = 0f;
        public float lifetimeLimit = 1f;
        public Vector3 camPos = Vector3.zero;
        public Quaternion camRot = Quaternion.identity;
        public float camFov = 41f;

        public FixedCameraBlock cameraBlock;
        
        private void Awake()
        {

            BB = GetComponent<BlockBehaviour>();

            SafeAwake();

            if (BB.isSimulating) { return; }
        }
        private IEnumerator GetAim()
        {
            RaycastHit hitCol;
            try
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitCol, 1000f, Game.BlockEntityLayerMask, QueryTriggerInteraction.Ignore))
                {
                    pickedCollider = hitCol.collider;
                    isTracking = true;
                }
            }
            catch { }
            yield break;
        }
        public virtual void SafeAwake()
        {
            //CameraNum = BB.AddText("相机编号", "cameraNum", "");
            picker = BB.AddKey(LanguageManager.Instance.outLang.Lock_On, "picker", KeyCode.None);
            resetPicker = BB.AddKey(LanguageManager.Instance.outLang.Reset_Lock, "reset", KeyCode.None);
            smoothActive = BB.AddToggle(LanguageManager.Instance.outLang.Smooth_Active, "smoothActive", false);
            trackActive = BB.AddToggle(LanguageManager.Instance.outLang.Tracking_Active, "trackActive", false);
            lifeTimeSlider = BB.AddSlider(LanguageManager.Instance.outLang.Life_Time, "lifetime", 1f, 0f, 2f);
            smoothMode = BB.AddMenu("smoothmode", 0, new List<string> { "t", "t^2", "t^0.5", "cos(t)", "arccos(t)" });
        }
        public void Start()
        {
            try
            {
                Tracker = BB.transform.FindChild("CompositeTracker").GetChild(0).GetChild(0).gameObject;
            }
            catch { }
            try
            {
                cameraBlock = BB.GetComponent<FixedCameraBlock>();
            }
            catch { }
            lifetimeLimit = Mathf.Max(0.05f, lifeTimeSlider.Value);
        }
        public void Update()
        {
            if (BB.isSimulating)
            {
                if (SmoothCameraController.Instance.fixedCamera.activeCamera == null)
                {
                    activating = false;
                }
                else
                {
                    if (SmoothCameraController.Instance.fixedCamera.activeCamera.gameObject.GetHashCode() != BB.gameObject.GetHashCode())
                    {
                        activating = false;
                    }
                    else
                    {
                        activating = true;
                    }
                }
                if(trackActive.IsActive)
                {
                    if (picker.IsPressed)
                    {
                        StartCoroutine(GetAim());
                    }
                    if (resetPicker.IsPressed)
                    {
                        pickedCollider = null;
                        isTracking = false;
                    }
                }
                if (smoothActive.IsActive)
                {
                    if (activating2 != activating)
                    {
                        activating2 = activating;
                        if (activating)
                        {
                            lifetime = 0;
                            camPos = Tracker.transform.localPosition;
                            camRot = Tracker.transform.localRotation;

                        }
                    }
                    if (!activating)
                    {
                        camFov = Camera.main.fieldOfView;
                        Tracker.transform.position = Camera.main.transform.position;
                        Tracker.transform.rotation = Camera.main.transform.rotation;
                    }
                    else
                    {
                        if (lifetime < lifetimeLimit)
                        {
                            lifetime += Time.deltaTime;
                            float offset = Mathf.Min(1f, lifetime / lifetimeLimit);
                            switch (smoothMode.Value)
                            {
                                case 0:
                                    {
                                        break;
                                    }
                                case 1:
                                    {
                                        offset = Mathf.Pow(offset, 2f);
                                        break;
                                    }
                                case 2:
                                    {
                                        offset = Mathf.Pow(offset, 0.5f);
                                        break;
                                    }
                                case 3:
                                    {
                                        offset = -Mathf.Cos(offset * Mathf.PI) / 2 + 0.5f;
                                        break;
                                    }
                                case 4:
                                    {
                                        offset = Mathf.Acos(-2 * (offset - 0.5f)) / Mathf.PI;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                            if (cameraBlock.CamMode == FixedCameraBlock.Mode.FirstPerson || cameraBlock.CamMode == FixedCameraBlock.Mode.Custom)
                            {
                                Camera.main.fieldOfView = Mathf.Lerp(camFov, cameraBlock.fovSlider.Value, offset);
                                //Debug.Log(cameraBlock.fovSlider.Value.ToString());
                                //Debug.Log(Camera.main.fieldOfView.ToString());
                            }
                            Tracker.transform.localPosition = Vector3.Lerp(camPos, Vector3.zero, offset);
                            if (trackActive.IsActive && isTracking)
                            {
                                Tracker.transform.LookAt(pickedCollider.transform.position);
                                Vector3 tgtAngle = Tracker.transform.localEulerAngles;
                                tgtAngle.z = 0f;
                                Tracker.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.Euler(tgtAngle), offset);
                            }
                            else
                                Tracker.transform.localRotation = Quaternion.Lerp(camRot, Quaternion.identity, offset);
                        }
                        else
                        {
                            if (trackActive.IsActive && isTracking)
                            {
                                Tracker.transform.LookAt(pickedCollider.transform.position);
                                Vector3 tgtAngle = Tracker.transform.localEulerAngles;
                                tgtAngle.z = 0f;
                                Tracker.transform.localEulerAngles = tgtAngle;
                            }
                            else
                            {
                                Tracker.transform.localRotation = Quaternion.identity;
                            }
                        }
                    }
                }
                else
                {
                    if (trackActive.IsActive && isTracking)
                    {
                        Tracker.transform.LookAt(pickedCollider.transform.position);
                        Vector3 tgtAngle = Tracker.transform.localEulerAngles;
                        tgtAngle.z = 0f;
                        Tracker.transform.localEulerAngles = tgtAngle;
                    }
                    else
                    {
                        Tracker.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }
        /*
        readonly GUIStyle vec3Style = new GUIStyle()
        {
            fontStyle = FontStyle.Bold,
            fontSize = 16,
            normal = { textColor = new Color(1, 1, 1, 1) },
            alignment = TextAnchor.MiddleCenter,
        };
        private void OnGUI()
        {
            string showmsg = "";
            showmsg += Tracker.name + "\n";
            showmsg += Tracker.transform.position.ToString() + "\n";
            showmsg += Tracker.transform.rotation.ToString() + "\n";
            showmsg += lifetime.ToString() + "\n";
            showmsg += activating.ToString() + "\n";
            showmsg += BB.gameObject.GetHashCode().ToString() + "\n";
            try
            {
                showmsg += SmoothCameraController.Instance.fixedCamera.activeCamera.gameObject.GetHashCode();
            }
            catch { }
            GUI.Box(UIrect, showmsg);
        }
        */
    }
}
