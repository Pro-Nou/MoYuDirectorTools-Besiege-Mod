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
    class LanguageManager : SingleInstance<LanguageManager>
    {
        public override string Name { get; } = "Language Manager";
        public string thisLang = "";
        public getLang outLang;
        public Dictionary<string,getLang> myLangs = new Dictionary<string, getLang>
        {
            { "简体中文",new Chinese()},
            { "English",new English()},
        };
        public void Awake()
        {
            outLang = myLangs["English"];
            //OnLanguageChanged += ChangLanguage;
        }
        public void Update()
        {
            if(thisLang != Localisation.LocalisationManager.Instance.currLangName)
            {
                thisLang = Localisation.LocalisationManager.Instance.currLangName;
                Debug.Log(thisLang);
                try
                {
                    outLang = myLangs[thisLang];
                }
                catch
                {
                    outLang = myLangs["English"];
                }
            }
        }
    }
    public interface getLang
    {
        string windowTitle { get; }
        string Open_resource_folder { get; }
        string Res_Clear { get; }
        string Environment_Control { get; }
        string Apply_Sky_Box { get; }
        string Sky_Box_Texture { get; }
        string Sky_Texture_Color { get; }
        string Background_Color { get; }
        string No_Basic_Fog { get; }
        string Apply_Fog { get; }
        string Fog_Start_Distance { get; }
        string Fog_Density { get; }
        string Fog_Color { get; }
        string No_Floor { get; }
        string No_Env { get; }
        string No_Bound { get; }
        string Main_Light_Control { get; }
        string Apply_Main_Light { get; }
        string Main_Light_Rotation { get; }
        string Fixed_RGB { get; }
        string Light_Color { get; }
        string Ambient_Color { get; }
        string Main_Light_Auto_Rotate { get; }
        string Set_shadow_distance { get; }
        string Set_shadow_quality { get; }
        string Set_shadow_bias { get; }
        string Resources_Control { get; }
        string click_to_remove { get; }
        string Shadow_Cascades { get; }

        string Apply { get; }
        string Static { get; }
        string StartTime { get; }

        string Audio_Set { get; }
        string Visualization_Set { get; }
        string Play { get; }
        string Loop { get; }
        string Visible { get; }
        string Time_Scale { get; }
        string Diffusion_Strength { get; }
        string Audio_Source { get; }
        string Active_Visualization { get; }
        string Listening_Mode { get; }
        string Listening_GUID { get; }
        string Stream_Light { get; }
        string Stream_Speed { get; }
        string FFT_Resolution { get; }
        string Max_Volume { get; }
        string FFT_Channel { get; }
        string Scale_Strength { get; }
        string Unit_Height { get; }
        string Unit_Width { get; }
        string Unit_Distance { get; }
        string Axis_Offset { get; }
        string Rotate_Offset { get; }
        string FFT_Offset { get; }
        string Shape_Line { get; }
        string Shape_Circle { get; }
        string Color1 { get; }
        string Color2 { get; }

        string Change_Mass { get; }
        string Mass { get; }

        string Water_Color { get; }
        string Water_Alpha { get; }

        string ShowA { get; }
        string ShowB { get; }
        string ShowC { get; }

        string Launch { get; }
        string Constraints { get; }
        string Damper { get; }
        string Tracking { get; }
        string Toggle_Mode { get; }
        string FreezeX { get; }
        string FreezeY { get; }
        string FreezeZ { get; }
        string Freeze_Angular_X { get; }
        string Freeze_Angular_Y { get; }
        string Freeze_Angular_Z { get; }
        string DamperX { get; }
        string DamperY { get; }
        string DamperZ { get; }
        string Max_Speed_X { get; }
        string Max_Speed_Y { get; }
        string Max_Speed_Z { get; }
        string Lock_On { get; }
        string Reset_Lock { get; }
        string TrackingX { get; }
        string TrackingY { get; }
        string TrackingZ { get; }
        string Rotate_Speed { get; }

        string Limited_Angular { get; }

        string Smooth_Active { get; }
        string Tracking_Active { get; }
        string Life_Time { get; }

        string Break_Force { get; }

        string Text_Color { get; }
        string Text_Alpha { get; }
        string Text_Item { get; }

        string Target_Time_Scale { get; }
        string Change_Speed { get; }
        string FXwidth { get; }
        string FXcolor { get; }


        string Collider { get; }
        string Health { get; }
        string Alpha { get; }
        string Diffuse_Strength { get; }
        string Ambient_Strength { get; }
        string Shadow_Threshold { get; }
        string Shadow_Receive_Threshold { get; }
        string Shadow_Brightness { get; }
        string Shadow_Smoothness { get; }
        string Rim_Threshold { get; }
        string Specular_Scale { get; }
        string Main_Color { get; }
        string Emis_Color { get; }
        string Rim_Color { get; }
        string Specular_Color { get; }
        string Meshname { get; }
        string Main_Tex { get; }
        string RS_Tex { get; }
        string Emis_Tex { get; }
    }
    public class English : getLang
    {
        public string windowTitle { get; } = "MoYu Director Tools (LCtrl+D to hide)";
        public string Open_resource_folder { get; } = "Open resource folder";
        public string Res_Clear { get; } = "Clear";
        public string Environment_Control { get; } = "Environment Control";
        public string Apply_Sky_Box { get; } = "Apply Sky Box";
        public string Sky_Box_Texture { get; } = "Sky Box Texture";
        public string Sky_Texture_Color { get; } = "Sky Texture Color(RGBA)";
        public string Background_Color { get; } = "Background Color(RGBA)";
        public string No_Basic_Fog { get; } = "No Basic Fog";
        public string Apply_Fog { get; } = "Apply Fog";
        public string Fog_Start_Distance { get; } = "Fog Start Distance";
        public string Fog_Density { get; } = "Fog Density";
        public string Fog_Color { get; } = "Fog Color(RGBA)";
        public string No_Floor { get; } = "No Floor";
        public string No_Env { get; } = "No Env";
        public string No_Bound { get; } = "No Bound";
        public string Main_Light_Control { get; } = "Main Light Control";
        public string Apply_Main_Light { get; } = "Apply Main Light";
        public string Main_Light_Rotation { get; } = "Main Light Rotation";
        public string Fixed_RGB { get; } = "Fixed RGB";
        public string Light_Color { get; } = "Light Color(RGBA)";
        public string Ambient_Color { get; } = "Ambient Color(RGBA)";
        public string Main_Light_Auto_Rotate { get; } = "Main Light Auto Rotate";
        public string Set_shadow_distance { get; } = "Set shadow distance";
        public string Set_shadow_quality { get; } = "Set shadow quality(0-3)";
        public string Set_shadow_bias { get; } = "Set shadow bias";
        public string Resources_Control { get; } = "Resources Controll";
        public string click_to_remove { get; } = "(click to remove, except audios or assetBundles)";
        public string Shadow_Cascades { get; } = "Set shadow cascades";

        public string Apply { get; } = "Apply";
        public string Static { get; } = "Static";
        public string StartTime { get; } = "StartTime";

        public string Audio_Set { get; } = "Audio";
        public string Visualization_Set { get; } = "Visualization";
        public string Play { get; } = "Play";
        public string Loop { get; } = "Loop";
        public string Visible { get; } = "Visible";
        public string Time_Scale { get; } = "Time Scale";
        public string Diffusion_Strength { get; } = "Diffuse";
        public string Audio_Source { get; } = "Audio (.wav)";
        public string Active_Visualization { get; } = "Active Visualization";
        public string Listening_Mode { get; } = "Listening Mode";
        public string Listening_GUID { get; } = "Listening GUID";
        public string Stream_Light { get; } = "Stream Light";
        public string Stream_Speed { get; } = "Stream Speed";
        public string FFT_Resolution { get; } = "FFT resolution";
        public string Max_Volume { get; } = "Max volume";
        public string FFT_Channel { get; } = "FFT channel";
        public string Scale_Strength { get; } = "Scale strength";
        public string Unit_Height { get; } = "Unit height";
        public string Unit_Width { get; } = "Unit width";
        public string Unit_Distance { get; } = "Unit distance";
        public string Axis_Offset { get; } = "Axis offset";
        public string Rotate_Offset { get; } = "Rotate offset";
        public string FFT_Offset { get; } = "FFT offset";
        public string Shape_Line { get; } = "Line";
        public string Shape_Circle { get; } = "Circle";
        public string Color1 { get; } = "color1";
        public string Color2 { get; } = "color2";

        public string Change_Mass { get; } = "Change Mass";
        public string Mass { get; } = "Mass";

        public string Water_Color { get; } = "Water Color";
        public string Water_Alpha { get; } = "Water Alpha";

        public string ShowA { get; } = "ShowA";
        public string ShowB { get; } = "ShowB";
        public string ShowC { get; } = "ShowC";

        public string Launch { get; } = "Launch";
        public string Constraints { get; } = "Constraints";
        public string Damper { get; } = "Damper";
        public string Tracking { get; } = "Tracking";
        public string Toggle_Mode { get; } = "Toggle Mode";
        public string FreezeX { get; } = "Freeze X";
        public string FreezeY { get; } = "Freeze Y";
        public string FreezeZ { get; } = "Freeze Z";
        public string Freeze_Angular_X { get; } = "Freeze X rotate";
        public string Freeze_Angular_Y { get; } = "Freeze Y rotate";
        public string Freeze_Angular_Z { get; } = "Freeze Z rotate";
        public string DamperX { get; } = "Damper X";
        public string DamperY { get; } = "Damper Y";
        public string DamperZ { get; } = "Damper Z";
        public string Max_Speed_X { get; } = "Max speed X";
        public string Max_Speed_Y { get; } = "Max speed Y";
        public string Max_Speed_Z { get; } = "Max speed Z";
        public string Lock_On { get; } = "Lock on";
        public string Reset_Lock { get; } = "Reset lock";
        public string TrackingX { get; } = "Track X";
        public string TrackingY { get; } = "Track Y";
        public string TrackingZ { get; } = "Track Z";
        public string Rotate_Speed { get; } = "Rotate Speed";

        public string Limited_Angular { get; } = "Limited Angular";

        public string Smooth_Active { get; } = "Smooth Active";
        public string Tracking_Active { get; } = "Tracking Active";
        public string Life_Time { get; } = "LifeTime";

        public string Break_Force { get; } = "BreakForce";

        public string Text_Color { get; } = "Text Color";
        public string Text_Alpha { get; } = "Text Alpha";
        public string Text_Item { get; } = "Text Item";

        public string Target_Time_Scale { get; } = "Target TimeScale";
        public string Change_Speed { get; } = "Change Speed";
        public string FXwidth { get; } = "FXwidth";
        public string FXcolor { get; } = "FXcolor";

        public string Collider { get; } = "Collider";
        public string Health { get; } = "Health";
        public string Alpha { get; } = "Alpha";
        public string Diffuse_Strength { get; } = "Deffuse Strength";
        public string Ambient_Strength { get; } = "Ambient Strength";
        public string Shadow_Threshold { get; } = "Shadow Threshold";
        public string Shadow_Receive_Threshold { get; } = "Shadow Receive Threshold";
        public string Shadow_Brightness { get; } = "Shadow Brightness";
        public string Shadow_Smoothness { get; } = "Shadow Smoothness";
        public string Rim_Threshold { get; } = "Rim Threshold";
        public string Specular_Scale { get; } = "Specular Threshold";
        public string Main_Color { get; } = "Main Color";
        public string Emis_Color { get; } = "Emission Color";
        public string Rim_Color { get; } = "Rim Color";
        public string Specular_Color { get; } = "Specular Color";
        public string Meshname { get; } = "Mesh (.obj)";
        public string Main_Tex { get; } = "MainTex (.png)";
        public string RS_Tex { get; } = "RSTex (.png)";
        public string Emis_Tex { get; } = "EmisTex (.png)";
    }
    public class Chinese : getLang
    {
        public string windowTitle { get; } = "摸鱼视效工具(LCtrl+D隐藏)";
        public string Open_resource_folder { get; } = "打开资源文件夹";
        public string Res_Clear { get; } = "清除所有";
        public string Environment_Control { get; } = "环境控制";
        public string Apply_Sky_Box { get; } = "应用天空盒";
        public string Sky_Box_Texture { get; } = "天空盒贴图";
        public string Sky_Texture_Color { get; } = "天空盒颜色(RGBA)";
        public string Background_Color { get; } = "背景颜色(RGBA)";
        public string No_Basic_Fog { get; } = "清除基准雾效";
        public string Apply_Fog { get; } = "应用雾效";
        public string Fog_Start_Distance { get; } = "雾效起始距离";
        public string Fog_Density { get; } = "雾效浓度";
        public string Fog_Color { get; } = "雾效颜色(RGBA)";
        public string No_Floor { get; } = "去除地面";
        public string No_Env { get; } = "去除环境";
        public string No_Bound { get; } = "去除边界";
        public string Main_Light_Control { get; } = "主光源控制";
        public string Apply_Main_Light { get; } = "应用主光源";
        public string Main_Light_Rotation { get; } = "主光源旋转角";
        public string Fixed_RGB { get; } = "Fixed RGB";
        public string Light_Color { get; } = "主光源颜色(RGBA)";
        public string Ambient_Color { get; } = "环境光颜色(RGBA)";
        public string Main_Light_Auto_Rotate { get; } = "主光源自动旋转";
        public string Set_shadow_distance { get; } = "设置阴影距离";
        public string Set_shadow_quality { get; } = "设置阴影质量(0-3)";
        public string Set_shadow_bias { get; } = "设置阴影偏差";
        public string Resources_Control { get; } = "资源控制";
        public string click_to_remove { get; } = "(点击以移除，除了音效和assetBundles)";
        public string Shadow_Cascades { get; } = "设置阴影级联";

        public string Apply { get; } = "应用";
        public string Static { get; } = "静态";
        public string StartTime { get; } = "起始时间";

        public string Audio_Set { get; } = "音频设置";
        public string Visualization_Set { get; } = "可视化设置";
        public string Play { get; } = "播放";
        public string Loop { get; } = "循环";
        public string Visible { get; } = "可见";
        public string Time_Scale { get; } = "时间缩放";
        public string Diffusion_Strength { get; } = "扩散强度";
        public string Audio_Source { get; } = "音源(.wav)";
        public string Active_Visualization { get; } = "启用可视化";
        public string Listening_Mode { get; } = "监听模式";
        public string Listening_GUID { get; } = "监听GUID";
        public string Stream_Light { get; } = "流光";
        public string Stream_Speed { get; } = "流光速度";
        public string FFT_Resolution { get; } = "采样分辨率";
        public string Max_Volume { get; } = "最大采样音量";
        public string FFT_Channel { get; } = "采样频道";
        public string Scale_Strength { get; } = "形变强度";
        public string Unit_Height { get; } = "单位高度";
        public string Unit_Width { get; } = "单位宽度";
        public string Unit_Distance { get; } = "单位间距";
        public string Axis_Offset { get; } = "中轴偏移";
        public string Rotate_Offset { get; } = "倾斜偏移";
        public string FFT_Offset { get; } = "采样偏移";
        public string Shape_Line { get; } = "线";
        public string Shape_Circle { get; } = "环";
        public string Color1 { get; } = "颜色1";
        public string Color2 { get; } = "颜色2";

        public string Change_Mass { get; } = "改变质量";
        public string Mass { get; } = "质量";

        public string Water_Color { get; } = "液体颜色";
        public string Water_Alpha { get; } = "液体不透明度";

        public string ShowA { get; } = "显示头";
        public string ShowB { get; } = "显示尾";
        public string ShowC { get; } = "显示中间";

        public string Launch { get; } = "开启";
        public string Constraints { get; } = "锁定";
        public string Damper { get; } = "阻尼";
        public string Tracking { get; } = "追踪";
        public string Toggle_Mode { get; } = "持续激活";
        public string FreezeX { get; } = "锁定X轴";
        public string FreezeY { get; } = "锁定Y轴";
        public string FreezeZ { get; } = "锁定Z轴";
        public string Freeze_Angular_X { get; } = "锁定X轴旋转";
        public string Freeze_Angular_Y { get; } = "锁定Y轴旋转";
        public string Freeze_Angular_Z { get; } = "锁定Z轴旋转";
        public string DamperX { get; } = "X轴阻尼";
        public string DamperY { get; } = "Y轴阻尼";
        public string DamperZ { get; } = "Z轴阻尼";
        public string Max_Speed_X { get; } = "X轴限速";
        public string Max_Speed_Y { get; } = "Y轴限速";
        public string Max_Speed_Z { get; } = "Z轴限速";
        public string Lock_On { get; } = "获取目标";
        public string Reset_Lock { get; } = "重置目标";
        public string TrackingX { get; } = "X轴追踪";
        public string TrackingY { get; } = "Y轴追踪";
        public string TrackingZ { get; } = "Z轴追踪";
        public string Rotate_Speed { get; } = "旋转角度";

        public string Limited_Angular { get; } = "角度限制";

        public string Smooth_Active { get; } = "启用平滑";
        public string Tracking_Active { get; } = "启用追踪";
        public string Life_Time { get; } = "持续时间";

        public string Break_Force { get; } = "受力阈值";

        public string Text_Color { get; } = "文字颜色";
        public string Text_Alpha { get; } = "文字不透明度";
        public string Text_Item { get; } = "文字内容";

        public string Target_Time_Scale { get; } = "目标时间倍率";
        public string Change_Speed { get; } = "变换速率";
        public string FXwidth { get; } = "特效范围";
        public string FXcolor { get; } = "特效颜色";

        public string Collider { get; } = "启用碰撞";
        public string Health { get; } = "生命值";
        public string Alpha { get; } = "不透明度";
        public string Diffuse_Strength { get; } = "漫反射强度";
        public string Ambient_Strength { get; } = "环境光强度";
        public string Shadow_Threshold { get; } = "阴影阈值";
        public string Shadow_Receive_Threshold { get; } = "阴影接收阈值";
        public string Shadow_Brightness { get; } = "阴影亮度";
        public string Shadow_Smoothness { get; } = "阴影平滑度";
        public string Rim_Threshold { get; } = "边缘光阈值";
        public string Specular_Scale { get; } = "反光阈值";
        public string Main_Color { get; } = "主要颜色";
        public string Emis_Color { get; } = "自发光颜色";
        public string Rim_Color { get; } = "边缘光颜色";
        public string Specular_Color { get; } = "反光颜色";
        public string Meshname { get; } = "模型 (.obj)";
        public string Main_Tex { get; } = "主贴图 (.png)";
        public string RS_Tex { get; } = "RS贴图 (.png)";
        public string Emis_Tex { get; } = "自发光贴图 (.png)";
    }
}
