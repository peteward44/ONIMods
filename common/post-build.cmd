set solutiondir=%1
set projectdir=%2
set targetdir=%3
set targetname=%4
set releasesubdir=%5

call xcopy /Y /R /Q /I /E "%solutiondir%target" "%solutiondir%release\%releasesubdir%"
call xcopy /Y /R /Q /I /E "%projectdir%target" "%solutiondir%release\%releasesubdir%"

call xcopy /Y /R /Q /S "%targetdir%%targetname%.dll" "%solutiondir%release\%releasesubdir%"
::call xcopy /Y /R /Q /S "%targetdir%PLib.dll" "%solutiondir%release\%releasesubdir%"

