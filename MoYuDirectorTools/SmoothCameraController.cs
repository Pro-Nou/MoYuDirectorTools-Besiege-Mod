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
    class SmoothCameraController : SingleInstance<SmoothCameraController>
    {
        public override string Name { get; } = "SmoothCameraController";
        public FixedCameraController fixedCamera;
        
        private bool isFirstFrame = true;
        private Rect UIrect = new Rect(0, 100, 256, 256);
        public void FixedUpdate()
        {
            //if (StatMaster.isMP == StatMaster.IsLevelEditorOnly && StatMaster.levelSimulating)
            if (StatMaster.isMP || StatMaster.IsLevelEditorOnly)
            {
                if (PlayerData.localPlayer.machine != null)
                {
                    if (PlayerData.localPlayer.machine.isSimulating)
                    {
                        if (isFirstFrame)
                        {
                            try
                            {
                                fixedCamera = FindObjectOfType<FixedCameraController>();
                            }
                            catch { }
                            isFirstFrame = false;
                        }
                    }
                    else
                    {
                        if (!isFirstFrame)
                        {
                            isFirstFrame = true;
                        }
                    }
                }
            }
            else
            {
                if (StatMaster.levelSimulating)
                {
                    if (isFirstFrame)
                    {
                        try
                        {
                            fixedCamera = FindObjectOfType<FixedCameraController>();
                        }
                        catch { }
                        isFirstFrame = false;
                    }
                }
                else
                {
                    if (!isFirstFrame)
                    {
                        isFirstFrame = true;
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
            
            //string showmsg = "";
            //showmsg += StatMaster.isMP.ToString();
            //showmsg += StatMaster.IsLevelEditorOnly.ToString();
            //showmsg += StatMaster.levelSimulating.ToString();
            //GUI.Box(UIrect, showmsg);
            
            if (StatMaster.isMP == StatMaster.IsLevelEditorOnly && StatMaster.levelSimulating)
                GUI.Box(UIrect, fixedCamera.activeCamera.gameObject.GetHashCode().ToString());
        }
        */
    }
}
