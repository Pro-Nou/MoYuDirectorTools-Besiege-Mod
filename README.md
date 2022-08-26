# MoYuDirectorTools-Besiege-Mod
Added some useful tools for vedio maker
  
steam工坊地址: https://steamcommunity.com/sharedfiles/filedetails/?id=2609275998  
  
# Previews  
注意：视频中的卡顿是由github播放器导致的，暂时没有找到解决办法。  
      如果想查看流畅的视频请下载到本地。  
NOTE: Freezes in videos are caused by github's player, I'm not sure how to solve it.   
      If you want to view a smooth version, please download it.  

# (动态资源加载器)Dynamic resource loader  
![image](https://user-images.githubusercontent.com/74658051/180635364-76e8144f-2139-4d0c-a926-389a8301926f.png)
  
本项目添加了一独立于Unity assets封包文件的，可即时加载.obj, .png, .wav的动态资源加载器，无需重启整个应用即可导入新的资源。  
由于unity对mesh的面数与顶点有较为严格的上限要求，本项目针对.obj文件实现了自动切分生成mesh数组，用户无需关注资源是否合规。  
在实际使用时，用户只需将资源放入指定文件夹即可。  
This project adds a dynamic resource loader that is independent from Unity assets package files, which can load .obj, .png, .wav instantly, and import new resources without restarting the whole application.  
Since Unity has strict upper limit requirements on the number of faces and vertices of mesh, this project could automatic cut up .obj files into mesh array, which means users do not need to pay attention on whether the resources are valid.  
Users only need to put resources into resource folder.  


# (主页面)Main menu
·(天空盒控制)Sky Box Control  

https://user-images.githubusercontent.com/74658051/180635073-2b44af34-0033-47a0-8196-f9337bf2a269.mp4

·(雾气控制)Fog Control 

https://user-images.githubusercontent.com/74658051/180635070-5382fe55-8575-4122-a424-2d8eb0985ac9.mp4

·(场景控制)ENV Control

https://user-images.githubusercontent.com/74658051/180635124-47f169ff-0998-428b-83e3-93aebd08950a.mp4

·(光照控制)Main Light Control

https://user-images.githubusercontent.com/74658051/180635062-60747e1f-0482-41ea-abd2-26c2ed8fbfd4.mp4

如视频所示，通过主页面可对天空盒，雾，场景以及光照进行编辑。  
As shown in videos, skybox, fog, scene and lighting can be edited via main menu.  

# (.obj模型加载器)VisBlock
·(模型导入)model import

https://user-images.githubusercontent.com/74658051/180635999-7d799cab-800a-4291-bd0a-d5827a04562a.mp4

将模型与纹理贴图放入资源文件夹，输入对应文件名并应用即可。  
Put model and texture into resource folder, enter file names and apply.  
  
·(高光贴图&颜色编辑)Hilight texture&color modify

https://user-images.githubusercontent.com/74658051/180635997-17f7ff2d-1e9b-4d5a-900d-75783ae432bb.mp4

将高光纹理放入资源文件夹，输入对应文件名并应用即可。  
高光纹理效果为alpha叠加。  
Put hilight texture into resource folder, enter file names and apply.  
The hilight texture effect is alpha additive.  

·(Shader修改&RGB)Shader modify&RGB

https://user-images.githubusercontent.com/74658051/180635995-836dbb97-8c65-46c3-8ad6-c04c1396d0dc.mp4

目前shader可选为block shader或alpha blend，预计之后会添加更多选项。  
RGB模式以高光纹理为基础进行RGB渐变，可通过贴图控制渐变范围。  
Currently shader can be selected as block shader or alpha blend, more options are expected to be added in the future.  
RGB mode performs RGB gradient based on highlight texture, which means gradient range can be controlled through by editing highlight texture.  

·(localtransform变换)localtransform modify

https://user-images.githubusercontent.com/74658051/180635989-8dde4315-4bcd-4b21-afb7-861700b70346.mp4

与meshmode类似，可对localtransform进行编辑。  
Similar to meshmode, localtransform can be edited.  

# (unity Animator加载器)AnimeBlock
·(效果概览)overall view

https://www.bilibili.com/video/BV16U4y1C7Ha  

导入带有animator的unity预制件并播放，使用方法参照steam工坊说明  

# (unity Animator加载器)VideoBlock

导入unity5.4的MovieTexture预制件并播放，使用方法参照steam工坊说明  

# (.wav音频加载器)AudioBlock
·(效果概览)overall view

https://user-images.githubusercontent.com/74658051/180636531-9ee13db8-2c5b-4750-8c30-95148a3241a6.mp4

·(音源导入)audio import

https://user-images.githubusercontent.com/74658051/180636537-586809af-90d0-4402-a27a-7d5953df7b52.mp4

将wav音频文件放入资源文件夹，输入对应文件名并应用即可。  
Put wav audio file into resource folder, enter file names and apply.  

·(2D&3D对比)2D&3D compare

https://user-images.githubusercontent.com/74658051/180636535-f33ecd46-cafa-4465-a0ef-0bfb518aaac4.mp4

如视频所示，2D模式下音量不受摄像头位置影响，3D模式下则会根据摄像头位置影响各声道的音量。  
As shown in the video, the volume in 2D mode is not affected by main camera position. In 3D mode, the volume of each channel will be affected according to main camera position.  

·(时间缩放)timescale effect

https://user-images.githubusercontent.com/74658051/180636592-436d2da6-36c9-47e6-8080-e67ab9d0b169.mp4

如视频所示，可选择音源是否受时间缩放影响。  
As shown in the video, you can choose whether the audio source is affected by timescale.  

·(音频可视化)audio visualizer

https://user-images.githubusercontent.com/74658051/180637293-183e250d-92ae-4421-97ec-ee2e2c09c9e6.mp4

·(监听模式)listening mode

https://user-images.githubusercontent.com/74658051/180636532-8c6aabb1-c19c-427b-976f-59aaef2e2e51.mp4

该模式用于多个可视化控件对同一audiosource进行采样的场景。  
启用监听模式后，AudioBlock不再播放自身的audiosource，可视化控件也不会对其采样，而是对guid与自身监听guid值相等的audioblock的audiosource进行采样，以避免同时播放数个相同audiosource造成的过噪。  
This mode is used in a situation that multiple audio visualizers sampling the same audioSource.
Once enabled listening mode, an AudioBlock will not play its audiosource, its audio visualizer also will not sampling it. The AudioBlock will try to find another Audioblock which's guid is equal to its listening guid, and asks its audio visualizer to sampling this Audioblock's audiosource, to avoid excessive noise caused by playing several identical audiosources at the same time.   

# (3D文字)TextBlock
https://user-images.githubusercontent.com/74658051/180636550-6cd84dc7-32f3-4cdb-9a53-73b00938e9c9.mp4

3D文字，由TextMesh实现。  
3D text, achieved by TextMesh.  

# (时间缩放)TimeBlock
https://user-images.githubusercontent.com/74658051/180636548-8363a716-bd6f-4217-a801-ae891f923b55.mp4

按键后将timescale渐变到指定数值。  
After key pressed, timescale will fade to specified value.  

# (旋转锁定&追踪)KinematicAnchor
https://user-images.githubusercontent.com/74658051/180636546-ffc5e473-09ea-4749-8a31-290940f49d7b.mp4

旋转锁定模式下将使连接到的刚体只能绕某一轴旋转。例如，FreezeY将使刚体的X,Z轴旋转锁定，只能绕Y轴转动。  
追踪模式下将使连接到的刚体在指定的某一轴或某几个轴上LookAt由鼠标拾取的某一顶点。  
Rotation lock mode will make the connected rigid body only rotate about one axis. For example, FreezeY will lock the rigid body's X, Z axis rotation, and can only rotate around the Y axis.  
In tracking mode, the connected rigid body will LookAt a vertex picked by the mouse on a specified axis or axes.  

# (平滑运镜)smooth camera
https://user-images.githubusercontent.com/74658051/180638059-bdc123c9-0c37-4fe2-afed-fc426d5024f9.mp4

支持多种渐变函数。各函数选项的意义如下：  
T：匀速渐变  
T^2：先慢后快  
T^0.5：先快后慢  
cos(T)：中间快前后慢  
arccos(T)：中间慢前后快  
Various gradient functions are supported. The meaning of each function is as follows:  
T: uniform gradient  
T^2: slow at first then fast  
T^0.5: fast at first then slow  
cos(T): fast in middle, slow in front and back  
arccos(T): slow in middle, fast in front and back  



