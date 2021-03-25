set name=%1
set solutiondir=%2
set projectdir=%3
set target=%4
set moddir=%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\dev\%name%

call rd /S /Q "%moddir%"

call xcopy /Y /R /Q /I /E "%solutiondir%target" "%moddir%"
call xcopy /Y /R /Q /I /E "%projectdir%target" "%moddir%"
call xcopy /Y /R /Q /S "%target%" "%moddir%\"
