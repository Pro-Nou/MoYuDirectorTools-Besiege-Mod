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
    class KinematicAnchor : BlockScript
    {
        public GameObject colliders;
        public GameObject Vis;
        public Rigidbody myRigidbody;

        public MKey Launch;
        public MMenu rootMenu;
        public MToggle KeepHeld;
        public MToggle freezeX;
        public MToggle freezeY;
        public MToggle freezeZ;
        public MToggle freezeXA;
        public MToggle freezeYA;
        public MToggle freezeZA;

        public MSlider dampenX;
        public MSlider dampenY;
        public MSlider dampenZ;
        public MSlider limitX;
        public MSlider limitY;
        public MSlider limitZ;
        public MSlider massSlider;

        public MKey picker;
        public MKey resetPicker;
        public MToggle trackX;
        public MToggle trackY;
        public MToggle trackZ;
        public MSlider rotSpeed;
        public Collider pickedCollider;
        public bool isTracking = false;

        public Quaternion lastRot;
        public Quaternion tgtRot;
        public float RotLifetime;
        public Vector3 lastRotPos;
        public Vector3 tgtRotPos;
        public bool canRotUpdate = false;

        public bool isActivating = false;
        public bool isActivating2 = false;
        public void displayMenu0(bool active)
        {
            freezeX.DisplayInMapper = active;
            freezeY.DisplayInMapper = active;
            freezeZ.DisplayInMapper = active;
            freezeXA.DisplayInMapper = active;
            freezeYA.DisplayInMapper = active;
            freezeZA.DisplayInMapper = active;
        }
        public void displayMenu1(bool active)
        {
            dampenX.DisplayInMapper = active;
            dampenY.DisplayInMapper = active;
            dampenZ.DisplayInMapper = active;
            massSlider.DisplayInMapper = active;
            limitX.DisplayInMapper = active;
            limitY.DisplayInMapper = active;
            limitZ.DisplayInMapper = active;
        }
        public void displayMenu2(bool active)
        {
            picker.DisplayInMapper = active;
            resetPicker.DisplayInMapper = active;
            trackX.DisplayInMapper = active;
            trackY.DisplayInMapper = active;
            trackZ.DisplayInMapper = active;
            rotSpeed.DisplayInMapper = active;
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
        public override void SafeAwake()
        {
            colliders = BlockBehaviour.gameObject.transform.FindChild("Colliders").gameObject;
            Vis = BlockBehaviour.gameObject.transform.FindChild("Vis").gameObject;

            Launch = AddKey(LanguageManager.Instance.outLang.Launch, "launch", KeyCode.B);
            rootMenu = AddMenu("activemode", 0, new List<string> { LanguageManager.Instance.outLang.Constraints, LanguageManager.Instance.outLang.Damper, LanguageManager.Instance.outLang.Tracking });
            KeepHeld = AddToggle(LanguageManager.Instance.outLang.Toggle_Mode, "keepheld", false);
            freezeX = AddToggle(LanguageManager.Instance.outLang.FreezeX, "freezeX", false);
            freezeY = AddToggle(LanguageManager.Instance.outLang.FreezeY, "freezeY", false);
            freezeZ = AddToggle(LanguageManager.Instance.outLang.FreezeZ, "freezeZ", false);
            freezeXA = AddToggle(LanguageManager.Instance.outLang.Freeze_Angular_X, "freezeXA", false);
            freezeYA = AddToggle(LanguageManager.Instance.outLang.Freeze_Angular_Y, "freezeYA", false);
            freezeZA = AddToggle(LanguageManager.Instance.outLang.Freeze_Angular_Z, "freezeZA", false);
            //rotFreezeType = AddMenu("freezetype", 0, new List<string> { "Freeze None", "Freeze X", "Freeze Y", "Freeze Z", "Freeze All" });

            dampenX = AddSlider(LanguageManager.Instance.outLang.DamperX, "dampenX", 0f, 0f, 10f);
            dampenY = AddSlider(LanguageManager.Instance.outLang.DamperY, "dampenY", 0f, 0f, 10f);
            dampenZ = AddSlider(LanguageManager.Instance.outLang.DamperZ, "dampenZ", 0f, 0f, 10f);
            limitX = AddSlider(LanguageManager.Instance.outLang.Max_Speed_X, "limitX", 0f, 0f, 10f);
            limitY = AddSlider(LanguageManager.Instance.outLang.Max_Speed_Y, "limitY", 0f, 0f, 10f);
            limitZ = AddSlider(LanguageManager.Instance.outLang.Max_Speed_Z, "limitZ", 0f, 0f, 10f);
            massSlider = AddSlider(LanguageManager.Instance.outLang.Mass, "mass", 2f, 0f, 10f);

            picker = AddKey(LanguageManager.Instance.outLang.Lock_On, "picker", KeyCode.None);
            resetPicker = AddKey(LanguageManager.Instance.outLang.Reset_Lock, "reset", KeyCode.None);
            trackX = AddToggle(LanguageManager.Instance.outLang.TrackingX, "trackx", true);
            trackY = AddToggle(LanguageManager.Instance.outLang.TrackingY, "tracky", true);
            trackZ = AddToggle(LanguageManager.Instance.outLang.TrackingZ, "trackz", true);
            rotSpeed = AddSlider(LanguageManager.Instance.outLang.Rotate_Speed, "rotSpeed", 1f, 0.1f, 2f);
            try
            {
                rootMenu.ValueChanged += (int value) =>
                {
                    switch(rootMenu.Value)
                    {
                        case 0:
                            {
                                displayMenu0(true);
                                displayMenu1(false);
                                displayMenu2(false);
                                break;
                            }
                        case 1:
                            {
                                displayMenu0(false);
                                displayMenu1(true);
                                displayMenu2(false);
                                break;
                            }
                        case 2:
                            {
                                displayMenu0(false);
                                displayMenu1(false);
                                displayMenu2(true);
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                };
                massSlider.ValueChanged += (float value) =>
                {
                    try
                    {
                        BlockBehaviour.Rigidbody.mass = massSlider.Value;
                    }
                    catch { }
                };
            }
            catch { }
            BlockBehaviour.name = "Kinematic Anchor";
        }
        public override void OnSimulateStart()
        {
            colliders.SetActive(false);
            Vis.SetActive(false);
        }
        public void Start()
        {
            BlockBehaviour.Rigidbody.drag = 0.0f;
            BlockBehaviour.Rigidbody.angularDrag = 0.0f;
            BlockBehaviour.blockJoint.breakForce = Mathf.Infinity;
            BlockBehaviour.blockJoint.breakTorque = Mathf.Infinity;
            myRigidbody = BlockBehaviour.Rigidbody;

            BlockBehaviour.noRigidbody = false;

            isActivating = false;
            isActivating2 = false;
        }
        public void Update()
        {
            if (BlockBehaviour.isSimulating)
            {
                if (!KeepHeld.IsActive)
                {
                    if (Launch.IsHeld || Launch.EmulationHeld())
                        isActivating = true;
                    else
                        isActivating = false;
                }
                else
                {
                    if (Launch.IsPressed)
                        isActivating = !isActivating;
                }
                if (rootMenu.Value == 2)
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
                    if (canRotUpdate)
                    {
                        RotLifetime += Time.deltaTime;
                        //Debug.Log(RotLifetime.ToString() + "," + Time.fixedDeltaTime.ToString());
                        float rotOffset =  RotLifetime / Time.fixedDeltaTime;
                        myRigidbody.transform.rotation = Quaternion.LerpUnclamped(lastRot, tgtRot, rotOffset);
                        myRigidbody.transform.position = Vector3.LerpUnclamped(lastRotPos, tgtRotPos, rotOffset);
                        myRigidbody.angularVelocity = Vector3.zero;
                    }
                }
                if (isActivating) 
                {
                    if (rootMenu.Value == 1)
                    {
                        try
                        {
                            Vector3 thisVelocity = myRigidbody.velocity;
                            thisVelocity.x = Mathf.Sign(thisVelocity.x) * Mathf.Min(Mathf.Abs(limitX.Value), Mathf.Max(0f, Mathf.Abs(thisVelocity.x) - dampenX.Value * 100f * Time.deltaTime));
                            thisVelocity.y = Mathf.Sign(thisVelocity.y) * Mathf.Min(Mathf.Abs(limitY.Value), Mathf.Max(0f, Mathf.Abs(thisVelocity.y) - dampenY.Value * 100f * Time.deltaTime));
                            thisVelocity.z = Mathf.Sign(thisVelocity.z) * Mathf.Min(Mathf.Abs(limitZ.Value), Mathf.Max(0f, Mathf.Abs(thisVelocity.z) - dampenZ.Value * 100f * Time.deltaTime));
                            myRigidbody.velocity = thisVelocity;
                        }
                        catch { }
                    }
                }
            }
        }
        public override void SimulateFixedUpdateHost()
        {
            if (!BlockBehaviour.noRigidbody && rootMenu.Value != 1) 
            {
                try
                {
                    myRigidbody = BlockBehaviour.blockJoint.connectedBody;
                    BlockBehaviour.transform.SetParent(BlockBehaviour.blockJoint.connectedBody.transform);
                    BlockBehaviour.transform.localPosition = Vector3.zero;
                    BlockBehaviour.transform.localRotation = Quaternion.identity;
                    BlockBehaviour.transform.localScale = Vector3.one;
                    Destroy(BlockBehaviour.blockJoint);
                    Destroy(BlockBehaviour.Rigidbody);
                    BlockBehaviour.noRigidbody = true;
                }
                catch { }
            }
            if (KeepHeld.IsActive)
                if (Launch.EmulationPressed())
                    isActivating = !isActivating;
            
            if (isActivating != isActivating2)
            {
                isActivating2 = isActivating;
                if (rootMenu.Value != 0)
                    return;
                if (isActivating)
                {
                    if (freezeX.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
                    if (freezeY.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezePositionY;
                    if (freezeZ.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
                    if (freezeXA.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezeRotationX;
                    if (freezeYA.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezeRotationY;
                    if (freezeZA.IsActive)
                        myRigidbody.constraints |= RigidbodyConstraints.FreezeRotationZ;
                }
                else
                    myRigidbody.constraints = RigidbodyConstraints.None;
            }

            canRotUpdate = false;
            if (rootMenu.Value == 2 && isActivating)
            {
                if (isTracking)
                {
                    /*
                    BlockBehaviour.transform.LookAt(pickedCollider.transform.position);
                    //BlockBehaviour.transform.forward = (pickedCollider.transform.position - BlockBehaviour.transform.position).normalized;
                    Vector3 thisRot = BlockBehaviour.transform.localEulerAngles;
                    thisRot.z = 0f;
                    //BlockBehaviour.transform.localEulerAngles = thisRot;
                    //thisRot = BlockBehaviour.transform.localEulerAngles;
                    if (thisRot.x > 180f)
                        thisRot.x -= 360f;
                    if (thisRot.y > 180f)
                        thisRot.y -= 360f;
                    if (thisRot.magnitude < Mathf.Max(0.1f, rotEndLimit.Value))
                    {
                        float speedShouldBe = Mathf.Lerp(0f, Mathf.Max(rotLimit.Value, 0f), thisRot.magnitude / Mathf.Max(0.1f, rotEndLimit.Value));
                        myRigidbody.angularVelocity = myRigidbody.angularVelocity.normalized * speedShouldBe;
                    }

                    thisRot /= 30f;
                    if (thisRot.magnitude > 1f)
                        thisRot = thisRot.normalized;
                    //if (myRigidbody.angularVelocity.magnitude > Mathf.Max(rotLimit.Value, 0f))
                    //    myRigidbody.angularVelocity = myRigidbody.angularVelocity.normalized * Mathf.Max(rotLimit.Value, 0f);
                    myRigidbody.AddRelativeTorque(thisRot * Mathf.Max(0.1f, rotSpeed.Value));
                    */
                    BlockBehaviour.transform.LookAt(pickedCollider.transform.position);
                    Vector3 localRot = BlockBehaviour.transform.localEulerAngles;
                    if ((myRigidbody.constraints & RigidbodyConstraints.FreezeRotationX) == RigidbodyConstraints.FreezeRotationX || !trackX.IsActive) 
                        localRot.x = 0f;
                    if ((myRigidbody.constraints & RigidbodyConstraints.FreezeRotationY) == RigidbodyConstraints.FreezeRotationY || !trackY.IsActive)
                        localRot.y = 0f;
                    if ((myRigidbody.constraints & RigidbodyConstraints.FreezeRotationZ) == RigidbodyConstraints.FreezeRotationZ || !trackZ.IsActive)
                        localRot.z = 0f;
                    BlockBehaviour.transform.localEulerAngles = localRot;
                    float cosAngle = Vector3.Dot(BlockBehaviour.transform.forward, myRigidbody.transform.forward);
                    float angleToBe = Mathf.Acos(Mathf.Max(-1, Mathf.Min(1f, cosAngle))) * 180f / Mathf.PI;

                    if (angleToBe > Mathf.Max(0f, rotSpeed.Value))
                    {
                        tgtRot = Quaternion.Lerp(myRigidbody.transform.rotation, BlockBehaviour.transform.rotation, Mathf.Max(0f, rotSpeed.Value) / angleToBe);
                    }
                    else
                    {
                        tgtRot = BlockBehaviour.transform.rotation;
                    }
                    myRigidbody.transform.rotation = tgtRot;
                    myRigidbody.angularVelocity = Vector3.zero;
                    /*
                    lastRot = myRigidbody.transform.rotation;
                    lastRotPos = myRigidbody.transform.position;
                    tgtRotPos = myRigidbody.velocity * Time.fixedDeltaTime + lastRotPos;
                    RotLifetime = 0f;
                    canRotUpdate = true;
                    */


                }
            }
        }
    }
}
