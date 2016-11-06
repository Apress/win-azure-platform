
IF EXIST "%PROGRAMFILES%\Microsoft CAPICOM 2.1.0.2 SDK" (
    SET capicompath="%PROGRAMFILES%\Microsoft CAPICOM 2.1.0.2 SDK\Samples\vbs\cstore.vbs"
    SET cscript=%windir%\system32\cscript.exe
)

IF EXIST "%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK" (
    SET capicompath="%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK\Samples\vbs\cstore.vbs"
    SET cscript=%windir%\syswow64\cscript.exe

    ECHO Setting up CAPICOM for 64 bits environment...
    copy /y "%PROGRAMFILES(x86)%\Microsoft CAPICOM 2.1.0.2 SDK\Lib\X86\capicom.dll" %windir%\syswow64
    %windir%\syswow64\regsvr32.exe /s %windir%\syswow64\capicom.dll
)


makecert -r -pe -n "CN=%1.cloudapp.net" -sky exchange "%~dp0certs\%1.cloudapp.net.cer" -sv "%~dp0certs\%1.cloudapp.net.pvk"
pvk2pfx -pvk "%~dp0certs\%1.cloudapp.net.pvk" -spc "%~dp0certs\%1.cloudapp.net.cer" -pfx "%~dp0certs\%1.cloudapp.net.pfx" -pi pass@word1

%cscript% /nologo %capicompath% import -e -l CU -s MY "%~dp0certs\%1.cloudapp.net.pfx" "pass@word1"
%cscript% /nologo %capicompath% import -e -l CU -s root "%~dp0certs\%1.cloudapp.net.pfx" "pass@word1"
%cscript% /nologo %capicompath% import -e -l LM -s root "%~dp0certs\%1.cloudapp.net.pfx" "pass@word1" 
%cscript% /nologo %capicompath% import -e -l LM -s MY "%~dp0certs\%1.cloudapp.net.pfx" "pass@word1"    