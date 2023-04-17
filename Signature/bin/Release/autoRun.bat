@echo off
start  "SelfService" "C:\Windows\System32\cmd.exe" 
reg add HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run /v autoSelfService /t reg_sz /d "%~dp0SelfService.exe" 
taskkill /f /im cmd.exe
exit
