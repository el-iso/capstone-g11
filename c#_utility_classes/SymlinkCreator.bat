@echo off

pushd "%~dp0"

set /p target="Specify the path (relative or absolute) that the new link refers to: "
echo.
set /p link="Specify the name for the symbolic link (can be anything): "
echo.
set /p uflag="{0 = file, 1 = directory}: "
SET flag=""
IF NOT "%uflag%" == "0" (
	set flag=/D
)

echo %target%
echo %link%
echo %uflag%
echo %flag%
echo %~dp0

mklink %flag% %link% %target%

popd

pause