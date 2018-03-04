@echo off

FOR /F "tokens=*" %%i in ('time /t') do SET STARTTIME=%%i

packages\FluentMigrator.1.6.2\tools\Migrate.exe --connection "Server=localhost\SQLEXPRESS;Database=SteamCoreDb;Trusted_Connection=True;" --provider SqlServer --assembly FluentMigrations\bin\Debug\FluentMigrations.dll --verbose=true --timeout=600

REM NOTE: We will probably never need to point this at the "Release" folder, because I believe we will only actually be running the migrations on our own database through the Debug build?

echo.
echo.
echo Started:
echo %STARTTIME%
echo Ended:
time /t
echo.
echo.

pause

