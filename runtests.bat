@echo off
cls

echo Compiling project...
"%VS100COMNTOOLS%\..\IDE\devenv" /nologo /rebuild Debug Kyru.sln
if %errorlevel% neq 0 goto end

echo.
echo Running tests...
Tests\lib\Gallio.Echo.exe /nl Tests\bin\Debug\Tests.dll
if %errorlevel% neq 0 goto end

:end
if "%cmdcmdline:~4,2%"=="/c" (pause)
