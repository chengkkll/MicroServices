@echo off

rem 获取随机数
set /a p=%random%%%50 + 6010
set /a ps=%p% + 1

rem 输出
echo.
@echo http port : %p%     https port : %ps%
echo.
echo dotnet TianCheng.Inventory.Restful.dll --urls "http://127.0.0.1:%p%;https://127.0.0.1:%ps%"
echo.

rem 进入程序目录
cd TianCheng.Inventory.Restful\bin\Debug\netcoreapp3.1
explorer http://127.0.0.1:%p%/swagger

rem 执行程序
dotnet TianCheng.Inventory.Restful.dll --urls "http://127.0.0.1:%p%;https://127.0.0.1:%ps%"