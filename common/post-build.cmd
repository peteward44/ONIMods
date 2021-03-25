set solutiondir=%1
set projectdir=%2
set target=%3
set releasesubdir=%4

call xcopy /Y /R /Q /I /E "%solutiondir%target" "%solutiondir%release\%releasesubdir%"
call xcopy /Y /R /Q /I /E "%projectdir%target" "%solutiondir%release\%releasesubdir%"

call xcopy /Y /R /Q /S "%target%" "%solutiondir%release\%releasesubdir%"

rem ilmerge /wildcards /out:$(TargetName)-merged.dll $(TargetName).dll $(ProjectDir)vendor\*.dll /targetplatform:v4,C:/Windows/Microsoft.NET/Framework64/v4.0.30319
