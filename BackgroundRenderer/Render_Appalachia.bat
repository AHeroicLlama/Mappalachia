@echo off

title Mappalachia Background Renderer (Appalachia)

set fo76DataPath="C:\Program Files (x86)\Steam\steamapps\common\Fallout76\Data"
set magickPath="C:\Program Files\ImageMagick-7.1.0-Q16-HDRI\magick.exe"
set outputPath=..\Mappalachia\img\
set renderFile=%outputPath%Appalachia_render.dds
set outputFile=%outputPath%Appalachia_render.jpg

..\Fo76Utils\render.exe %fo76DataPath%\SeventySix.esm %renderFile% 16384 16384 %fo76DataPath% -btd %fo76DataPath%\Terrain\Appalachia.btd -l 0 -cam 0.028 180 0 0 0 0 65536 -light 1.8 65 180 -lcolor 1.1 0xD6CCC7 0.9 -1 -1 -hqm meshes -a -ssaa 2 -ltxtres 512 -mip 1 -lmip 2 -mlod 0 -ndis 1 -xm babylon -xm fog

%magickPath% convert %renderFile% -resize 4096x4096 -quality 100 JPEG:%outputFile%

del %renderFile%
