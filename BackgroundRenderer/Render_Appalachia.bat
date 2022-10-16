@echo off

title Mappalachia Background Renderer (Appalachia)

set fo76DataPath="C:\Program Files (x86)\Steam\steamapps\common\Fallout 76 Playtest\Data"

..\Fo76Utils\render.exe %fo76DataPath%\SeventySix.esm ..\Mappalachia\img\Appalachia_Render.dds 16384 16384 %fo76DataPath% -btd %fo76DataPath%\Terrain\Appalachia.btd -l 0 -cam 0.028 180 0 0 0 0 65536 -light 1.25 63.435 41.8103 -a -ssaa 1 -hqm meshes -env textures/shared/cubemaps/mipblur_defaultoutside1.dds -wtxt textures/water/defaultwater.dds -ltxtres 1024 -mip 0 -lmip 1 -mlod 0 -ndis 1