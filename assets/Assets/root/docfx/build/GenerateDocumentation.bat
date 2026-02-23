rem **** Installing tools ****
dotnet tool install Docfx -g

rem **** Installing tools ****
docfx ../docfx.json %1 %2
if errorlevel == 1 goto error
goto end

:error
echo *** ERROR ***
echo An error occurred. Website was not generated.

:end
