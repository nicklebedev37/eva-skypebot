cd %~dp0

rem restoring missed nuget packages
..\src\.nuget\nuget.exe restore ..\src\Skypebot.sln

rem building the whole solution
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe /p:OutputPath="..\..\output" /p:Configuration=Release ..\src\Skypebot.sln
pause