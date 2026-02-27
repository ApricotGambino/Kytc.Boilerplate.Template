dotnet tool install Docfx -g

docfx ../docfx.json --serve --open-browser
if errorlevel == 1 goto error
goto end

:error
echo *** ERROR ***
echo An error occurred. Website was not generated.

:end
