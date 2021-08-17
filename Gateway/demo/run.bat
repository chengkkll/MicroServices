@echo off

rem 获取随机数
set /a p=5060
set /a ps=5061

rem 输出
echo.
@echo http port : %p%     https port : %ps%
echo.
echo dotnet GatewayApp1.dll --urls "http://127.0.0.1:%p%;https://127.0.0.1:%ps%"
echo.

rem 进入程序目录
cd GatewayApp1\bin\Debug\netcoreapp3.1
explorer http://127.0.0.1:%p%/api/org/restful

rem 执行程序
dotnet GatewayApp1.dll --urls "http://127.0.0.1:%p%;https://127.0.0.1:%ps%"