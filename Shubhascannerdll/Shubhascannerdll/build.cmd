@ECHO OFF

REM Have InnoSetup 5 installed, and set the below path to iscc.exe
SET iscc="C:\Program Files (x86)\Inno Setup 5\iscc.exe"

IF "%VSINSTALLDIR%" equ "" (
ECHO --- This batch requires Visual Studio Command Prompt
GOTO FAILED
)

IF not exist %iscc% (
ECHO --- Inno Setup 5 not found
GOTO FAILED
)

ECHO.
ECHO --------------------------------------------------------
ECHO --- Rebuilding PluginInstallation project
ECHO --------------------------------------------------------
ECHO.

msbuild "PluginInstallation.csproj" /t:Rebuild /p:Configuration=Release

if not exist bin\Release\AmiBroker.Samples.PluginInstallation.dll (
GOTO FAILED
)

ECHO.
ECHO --------------------------------------------------------
ECHO --- Building setup kit
ECHO --------------------------------------------------------
ECHO.

REM
REM Inno setup download: http://www.jrsoftware.org/isinfo.php
REM

%iscc% "PluginInstallation.iss"

if not exist PluginInstallationSample.exe (
GOTO FAILED
)

GOTO EXIT

:FAILED
ECHO --- Build failed!

:EXIT
PROMPT