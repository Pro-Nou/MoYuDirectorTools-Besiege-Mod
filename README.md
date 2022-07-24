# MoYuDirectorTools-Besiege-Mod
Added some useful tools for vedio maker

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

可通过主页面对天空盒纹理，天空盒颜色以及背景颜色进行编辑。  

