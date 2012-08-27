set DOC_PATH=%CD%\wp7
set SECRET_ID_KEY_PATH=%CD%
set BUCKET_NAME=sdk.mobeelizer.com

cd DeployHelpers

UploadDoc.exe %SECRET_ID_KEY_PATH% %DOC_PATH% %BUCKET_NAME%