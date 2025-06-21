@echo off
setlocal

:: Set your desired port number here
set PORT=60083

echo Looking for processes using port %PORT%...
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :%PORT%') do (
    echo Killing PID %%a
    taskkill /F /PID %%a >nul 2>&1
)

echo Done.
pause
