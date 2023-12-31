@echo off

::MAIN: deletes all "obj" subdirectories
    call:RECURSIVE_PURGE \bin
    call:RECURSIVE_PURGE \obj
    call:RECURSIVE_PURGE .vs
    call:RECURSIVE_PURGE .intermediateOutput
    call:RECURSIVE_PURGE output.*
    call:RECURSIVE_DEL . *.user
goto:EOF //return

:RECURSIVE_PURGE
:: parameter: %1 - simple name of subdirectory to purge and delete recursively
    for /d /r %%d in (%1) do call:PURGE %%d
goto:EOF //return

:PURGE
:: parameter: %1 full or relative directory name to purge and remove
    del /s /q %1\*.* 2> nul
    rmdir /s /q %1 2> nul
goto:EOF //return

:RECURSIVE_DEL
:: parameter %1 is top directory, %2 is a file mask
    del /s /q %1\%2 2> nul
goto:EOF //return