@ECHO OFF

:choice
set /P c=u about to clean ALL sub dirs[Y/N]?
if /I "%c%" EQU "Y" goto :somewhere
if /I "%c%" EQU "N" goto :somewhere_else
goto :choice


:somewhere
del *.* /s /q
pause 
exit

:somewhere_else

echo "K BYY"
pause 
exit
	
	
	