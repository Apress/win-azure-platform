@echo off
setlocal
:: Check for 64-bit Framework
if exist %SystemRoot%\Microsoft.NET\Framework64\v3.5 (
  set msbuild=%SystemRoot%\Microsoft.NET\Framework64\v3.5\msbuild.exe
  goto :run
)
:: Check for 32-bit Framework
if exist %SystemRoot%\Microsoft.NET\Framework\v3.5 (
  set msbuild=%SystemRoot%\Microsoft.NET\Framework\v3.5\msbuild.exe
  goto :run
)

:run
pushd %~dp0
if exist setvcvars.cmd (
   call setvcvars.cmd
)
:: Build all .proj file

set AtLeastOneProjBuilt=false
for %%i in (*.proj) do (
  %msbuild% %%i /t:ReBuild %*
  set AtLeastOneProjBuilt=true
)

if [%AtLeastOneProjBuilt%]==[true] goto :end

:: Build frist .sln file
for %%i in (*.sln) do (
 %msbuild% %%i /t:ReBuild %*
 goto :end
)

:end
popd
