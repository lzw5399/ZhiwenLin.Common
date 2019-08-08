@echo off
echo.

for /f "delims=" %%i in ('git config --get http.proxy') do (
    set "httpProxy=%%i"
)

if defined httpProxy (
    echo current git http proxy is:%httpProxy%
    git config --global --unset http.proxy
    echo git http proxy detected, proxy removed
) else (
    echo current git http proxy is empty
    git config --global http.proxy 'socks5://127.0.0.1:1080' 
    echo git http proxy not detected, proxy added
)

echo.
echo -------------------------------------
echo.

for /f "delims=" %%i in ('git config --get https.proxy') do (
    set "httpsProxy=%%i"
)

if defined httpProxy (
    echo current git https proxy is:%httpsProxy%
    git config --global --unset https.proxy
    echo git https proxy detected, proxy removed
) else (
    echo current git https proxy is empty
    git config --global https.proxy 'socks5://127.0.0.1:1080' 
    echo git https proxy not detected, proxy added
)

echo.
echo -------------------------------------
echo.

pause