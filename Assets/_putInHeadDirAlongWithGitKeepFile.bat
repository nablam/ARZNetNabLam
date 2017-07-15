@ECHO OFF

:choice
set /P c=first you need the gitkeepfile n this dir then are u SURE u wanna do this[Y/N]?
if /I "%c%" EQU "Y" goto :somewhere
if /I "%c%" EQU "N" goto :somewhere_else
goto :choice


:somewhere

    for /r "%CD%" %%f in (.) do (
      copy ".gitkeep.txt" "%%~ff" > nul
    )
pause 
exit

:somewhere_else

echo "K BYY"
pause 
exit
	
	
	