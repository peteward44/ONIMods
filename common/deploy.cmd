set name=%1
set solutiondir=%2
set projectdir=%3
set targetdir=%4
set targetname=%5
set moddir=%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\dev\%name%

call rd /S /Q "%moddir%"

call xcopy /Y /R /Q /I /E "%solutiondir%target" "%moddir%"
call xcopy /Y /R /Q /I /E "%projectdir%target" "%moddir%"
call xcopy /Y /R /Q /S "%targetdir%%targetname%.dll" "%moddir%\"
::call xcopy /Y /R /Q /S "%targetdir%PLib.dll" "%moddir%\"
