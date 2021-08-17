@echo off
rem 进入程序目录

rem 打开consul地址
explorer http://localhost:8500

rem 执行命令
set ip="192.168.1.9"
echo consul agent -server -bootstrap -bind=%ip% -data-dir=./data-dev -client 0.0.0.0 -ui -node=server-dev
consul agent -server -bootstrap -bind=%ip% -data-dir=./data-dev -client 0.0.0.0 -ui -node=server-dev


