@echo off

title Mappalachia Release Packager

rmdir /s /q dist
mkdir dist

cd publish
"C:\Program Files\7-Zip\7z.exe" a -tzip "..\dist\Mappalachia.zip" "*" -xr!"settings.json" -xr!"Saved_Maps" -xr!"temp" -xr!"img\spotlight"

cd img
"C:\Program Files\7-Zip\7z.exe" a -t7z -v2047M "..\..\dist\spotlight.7z" "spotlight"

PAUSE