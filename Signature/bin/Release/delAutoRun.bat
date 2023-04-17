@echo off
start  "SelfService" "C:\Windows\System32\cmd.exe" 
reg delete HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run /v autoSelfService /f
taskkill /f /im cmd.exe
exit
