set name=%1
set solutiondir=%2

call rd /S /Q "%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\dev\%name%"
call xcopy /Y /R /Q /I /E "%solutiondir%release" "%USERPROFILE%\Documents\Klei\OxygenNotIncluded\mods\dev\%name%"
