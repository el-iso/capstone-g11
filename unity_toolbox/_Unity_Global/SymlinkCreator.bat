@echo off

:: SymlinkCreator.bat
:: Derek Paschal
:: 2/3/2018

pushd "%~dp0"

set /p target="Specify the path (absolute or relative) that the new link refers to: "
echo.
set /p link="Specify the destination (absolute or relative) and name for the symbolic link (can be anything): "
echo.
set /p uflag="{0 = file, 1 = directory}: "
SET flag=
IF NOT "%uflag%" == "0" (
	set flag=/D
)

@echo on

mklink %flag% %link% %target%

@echo off

popd

pause