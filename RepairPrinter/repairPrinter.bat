@echo off
echo Stopping Print Spooler...
net stop spooler
echo Deleting all print jobs...
del /Q /F "%systemroot%\System32\spool\PRINTERS\*.*"
echo Starting Print Spooler...
net start spooler
echo Print queue has been cleared.