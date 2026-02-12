@echo off
dotnet tool install Docfx -g
dotnet tool install DocAssembler -g
dotnet tool install DocFxTocGenerator -g
dotnet tool install DocLanguageTranslator -g
dotnet tool install DocLinkChecker -g
dotnet tool install DocFxOpenApi -g

rem **** Get the DocFx tools ****
rem choco install docfx-companion-tools -y

rem **** Check for markdown erros
rem echo Checking markdown syntax
rem call markdownlint docs/*.md
rem if errorlevel == 1 goto error

rem **** Check the docs folder. On errors, quit processing
echo Checking references and attachments
rem doclinkchecker -d ./docs -a -v
doclinkchecker -d ./docs
if errorlevel == 1 goto error

rem **** Generate the table of contents
echo Generating table of contents for General
rem docfxtocgenerator -d ./docs/general -sri
docfxtocgenerator -d ./docs/general -sr

if errorlevel == 1 goto error

echo Generating table of contents for Services
rem docfxtocgenerator -d ./docs/services -sri
docfxtocgenerator -d ./docs/services -sr
if errorlevel == 1 goto error

rem **** Clean up old generated files
echo Clean up previous generated contents
if exist docs\_site rd docs\_site /s /q
if exist docx\_pdf rd docs\_pdf /s /q
if exist docs\reference rd docs\reference /s /q

rem **** Generated the website
echo Generating website in _site
docfx .\docs\docfx.json %1

goto end

:error
echo *** ERROR ***
echo An error occurred. Website was not generated.

:end
