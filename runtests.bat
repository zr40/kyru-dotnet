@echo off
cls

rem echo Compiling project...
rem "%VS100COMNTOOLS%\..\IDE\devenv" /nologo /build Debug Kyru.sln
rem if %errorlevel% neq 0 goto end

rem echo.
rem echo Running tests...
Tests\lib\Gallio.Echo.exe /nl /r:Local Tests\bin\Debug\Tests.dll
if %errorlevel% neq 0 goto end

:end

if "%cmdcmdline:~4,2%"=="/c" (
	if not "%github_shell%"=="true" (
		pause
	)
)
