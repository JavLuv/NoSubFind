@echo off

IF NOT EXIST build (
echo Creating build folder
mkdir build 
)

cd build
del /Q *.exe

cd ../src/

MSBuild NoSubFind.sln -t:build -restore -p:RestorePackagesConfig=true /p:Configuration=Release && (
  echo NoSubFind Release build succeeded
) || (
  echo NoSubFind build failed
  EXIT /B 1
)

dotnet publish -p:PublishProfile=FolderProfile && (
  echo NoSubFind publish succeeded
) || (
  echo NoSubFind publish failed
  EXIT /B 1
)

cd ..

copy /y "src\bin\Release\net6.0\win-x64\publish\NoSubFind.exe" "build\NoSubFind.exe"


EXIT /B 0
