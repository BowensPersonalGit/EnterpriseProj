@echo off
cd /d "%~dp0PractitionerPythonClient\PractitionerPythonVirtualEnvironment"
call Scripts\activate.bat
python ..\PractitionerPythonClient.py
pause