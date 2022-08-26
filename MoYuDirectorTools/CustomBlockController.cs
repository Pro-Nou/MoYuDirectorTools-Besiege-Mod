using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Modding;
using Modding.Blocks;

using System.Collections;
namespace MoYuDirectorTools
{
    class CustomBlockController : SingleInstance<CustomBlockController>
    {

        public override string Name { get; } = "Custom Block Controller";

        internal PlayerMachineInfo PMI;

        private void Awake()
        {

            //加载配置
            //Events.OnMachineLoaded += LoadConfiguration;
            Events.OnMachineLoaded += (pmi) => { PMI = pmi; };
            ////储存配置
            //Events.OnMachineSave += SaveConfiguration;
            //添加零件初始化事件委托
            Events.OnBlockInit += AddSliders;

        }
        private void AddSliders(Block block)
        {

            BlockBehaviour blockbehaviour = block.BuildingBlock.InternalObject;
            AddSliders(blockbehaviour);
        }
        private void AddSliders(BlockBehaviour block)
        {
            //if (StatMaster.isMP == StatMaster.IsLevelEditorOnly)
            switch(block.BlockID)
            {
                case (int)BlockType.CameraBlock:
                    {
                        if (block.gameObject.GetComponent(typeof(SmoothCameraStript)) == null)
                            block.gameObject.AddComponent(typeof(SmoothCameraStript));
                        break;
                    }
                case (int)BlockType.WaterCannon:
                    {
                        if (block.gameObject.GetComponent(typeof(ColorfulWaterCannon)) == null)
                            block.gameObject.AddComponent(typeof(ColorfulWaterCannon));
                        break;
                    }
                case (int)BlockType.SteeringHinge:
                    {
                        if (block.gameObject.GetComponent(typeof(SteeringJointBreakForce)) == null)
                            block.gameObject.AddComponent(typeof(SteeringJointBreakForce));
                        break;
                    }
                case (int)BlockType.SteeringBlock:
                    {
                        if (block.gameObject.GetComponent(typeof(SteeringJointBreakForce)) == null)
                            block.gameObject.AddComponent(typeof(SteeringJointBreakForce));
                        if (block.gameObject.GetComponent(typeof(LimitedSteeringBlock)) == null)
                            block.gameObject.AddComponent(typeof(LimitedSteeringBlock));
                        break;
                    }
                case (int)BlockType.Spring:
                    {
                        if (block.gameObject.GetComponent(typeof(InvisibleCylinderStuff)) == null)
                            block.gameObject.AddComponent(typeof(InvisibleCylinderStuff));
                        break;
                    }
                case (int)BlockType.RopeWinch:
                    {
                        if (block.gameObject.GetComponent(typeof(InvisibleCylinderStuff)) == null)
                            block.gameObject.AddComponent(typeof(InvisibleCylinderStuff));
                        break;
                    }
                case (int)BlockType.Brace:
                    {
                        if (block.gameObject.GetComponent(typeof(InvisibleCylinderStuff)) == null)
                            block.gameObject.AddComponent(typeof(InvisibleCylinderStuff));
                        break;
                    }
                case (int)BlockType.BuildSurface:
                    {
                        if (block.gameObject.GetComponent(typeof(buildSurfaceMass)) == null)
                            block.gameObject.AddComponent(typeof(buildSurfaceMass));
                        break;
                    }
                case (int)BlockType.ArmorPlateSmall:
                    {
                        if (block.gameObject.GetComponent(typeof(screenSort)) == null)
                            block.gameObject.AddComponent(typeof(screenSort));
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
