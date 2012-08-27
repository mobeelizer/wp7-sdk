set WP7SDK_DLL=wp7-sdk.dll
set README=README.txt
set CONFIGURE_DLL=Configuration.dll
set JSON_DLL=Newtonsoft.Json.dll
set ZLIB_DLL=SharpZipLib.WindowsPhone7.dll
set DOC_XML=wp7-SDK.xml
set SECRET_ID_KEY_PATH=%CD%
set BUCKET_NAME=sdk.mobeelizer.com

set TEMPDIR=%CD%\mobeelizer-wp7-sdk
set OUTPUTPATH=%CD%\mobeelizer-wp7-sdk.zip
del %OUTPUTPATH%
mkdir %TEMPDIR%
copy %CD%\%README% %TEMPDIR%
cd ..\
copy %CD%\wp7-sdk\Bin\Release\%WP7SDK_DLL% %TEMPDIR%
copy %CD%\wp7-sdk\Bin\Release\%CONFIGURE_DLL% %TEMPDIR%
copy %CD%\wp7-sdk\Bin\Release\%JSON_DLL% %TEMPDIR%
copy %CD%\wp7-sdk\Bin\Release\%ZLIB_DLL% %TEMPDIR%
copy %CD%\wp7-sdk\Bin\Release\%DOC_XML% %TEMPDIR%

echo Set objArgs = WScript.Arguments > _zipIt.vbs
echo InputFolder = objArgs(0) >> _zipIt.vbs
echo ZipFile = objArgs(1) >> _zipIt.vbs
echo CreateObject("Scripting.FileSystemObject").CreateTextFile(ZipFile, True).Write "PK" ^& Chr(5) ^& Chr(6) ^& String(18, vbNullChar) >> _zipIt.vbs
echo Set objShell = CreateObject("Shell.Application") >> _zipIt.vbs
echo Set source = objShell.NameSpace(InputFolder).Items >> _zipIt.vbs
echo objShell.NameSpace(ZipFile).CopyHere(source) >> _zipIt.vbs
echo wScript.Sleep 2000 >> _zipIt.vbs

CScript  _zipIt.vbs  %TEMPDIR%  %OUTPUTPATH%

del %TEMPDIR%\%WP7SDK_DLL%
del %TEMPDIR%\%README%
del %TEMPDIR%\%CONFIGURE_DLL%
del %TEMPDIR%\%JSON_DLL%
del %TEMPDIR%\%ZLIB_DLL%
del %TEMPDIR%\%DOC_XML% 
rmdir %TEMPDIR%

del _zipIt.vbs

echo Zip file prepared!

cd Scripts
UploadSDK.exe %SECRET_ID_KEY_PATH% %OUTPUTPATH% %BUCKET_NAME%

pause
