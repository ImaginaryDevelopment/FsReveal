@echo off
cls

.paket\paket.bootstrapper.exe
if %ERRORLEVEL% NEQ 0 (
  pause
  exit /b %errorlevel%
)

.paket\paket.exe restore -v
if %ERRORLEVEL% NEQ 0 (
  pause
  exit /b %errorlevel%
)

packages\FAKE\tools\FAKE.exe build.fsx Release %*
if %ERRORLEVEL% NEQ 0 (
  pause
  exit /b ERRORLEVEL%
)
