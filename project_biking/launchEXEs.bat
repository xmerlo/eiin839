@ECHO OFF
@cd /d "%~dp0"
ECHO Launching WebProxyServiceHost.exe
start /d "./WebProxyServiceHost/WebProxyServiceHost/bin/Debug" WebProxyServiceHost.exe
ECHO Launching RoutingWithBikesHost.exe
start /d "./RoutingWithBikesHost/RoutingWithBikesHost/bin/Debug" RoutingWithBikesHost.exe
ECHO The .exe are launched. If you have errors, please make sure you have launched this script as administrator
PAUSE
