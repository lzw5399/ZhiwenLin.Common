@echo off
REM 设置以admin启动
%1 %2
ver|find "5.">nul&&goto :Admin
mshta vbscript:createobject("shell.application").shellexecute("%~s0","goto :Admin","","runas",1)(window.close)&goto :eof
REM 设置以admin启动

:Admin
for /f "skip=3 tokens=4" %%i in ('sc query VMAuthdService') do set "zt=%%i" &goto :next
:next
if /i "%zt%"=="RUNNING" (goto Running)
if /i "%zt%"=="STOPPED" (goto Stopped)

:Running
echo VMware related boot service is running
echo Do you want to disable these?
set /p session= Select[Y/N] Default[Y]:
if /i "%session%"=="Y" (goto Disable)
if /i "%session%"=="N" (goto Thankyou)
if not defined session (goto Disable)
goto Retry

:Stopped
echo VMware related boot service is disabled
echo Do you want to enable these?
set /p session= Select[Y/N] Default[Y]:
if /i "%session%"=="Y" (goto Enable)
if /i "%session%"=="N" (goto Thankyou)
if not defined session (goto Enable)
goto Retry

:Disable
echo Ready to disable...
echo ------------------------------------------------
echo Disabling service...
net stop VMAuthdService
net stop VMnetDHCP
net stop "VMware NAT Service"
net stop VMUSBArbService
net stop VMwareHostd
echo ------------------------------------------------
echo Disabling network connection...
netsh interface set interface \"VMware Network Adapter VMnet1\" disable
netsh interface set interface \"VMware Network Adapter VMnet8\" disable
echo (VMware Network Adapter VMnet1)&(VMware Network Adapter VMnet8) disabled successfully...
goto Thankyou

:Enable
echo Ready to enable...
echo ------------------------------------------------
echo Enabling service...
net start VMAuthdService
net start VMnetDHCP
net start "VMware NAT Service"
net start VMUSBArbService
net start VMwareHostd
echo ------------------------------------------------
echo Enabling network connection...
netsh interface set interface \"VMware Network Adapter VMnet1\" enable
netsh interface set interface \"VMware Network Adapter VMnet8\" enable
echo (VMware Network Adapter VMnet1)、(VMware Network Adapter VMnet8) enabled successfully...
goto Thankyou

:Retry
echo Invalid input, please select again
echo ------------------------------------------------
goto Admin

:Thankyou
echo ------------------------------------------------
echo Thanks for using...
pause