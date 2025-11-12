#!/bin/bash
set -e

cd "$(dirname "$0")"

ProjectName="CarGlass"
BinDir=../$ProjectName/bin/ReleaseWin
RdlReaderDir=../My-FyiReporting/RdlViewer/RdlReader/bin/x86/Release

#Сборка MajorsilenceReporting
msbuild /p:Configuration=Release /p:Platform=x86 ../My-FyiReporting/MajorsilenceReporting.sln /target:RdlReader

# Сборка релиза
msbuild /p:Configuration=ReleaseWin /p:Platform=x86 ../CarGlass.sln

#Создаем папку если вдруг ее нет.
mkdir -p Files

#Чистим папку назначения
rm -v -f -R ./Files/*

#Копируем RdlReader
cp -v ${RdlReaderDir}/DataProviders.dll ./Files
cp -v ${RdlReaderDir}/EncryptionProvider.dll ./Files
#cp -v ${RdlReaderDir}/itextsharp.dll ./Files
#cp -v ${RdlReaderDir}/netstandard.dll ./Files
cp -v ${RdlReaderDir}/Newtonsoft.Json.dll ./Files
#cp -v ${RdlReaderDir}/NPOI*.dll ./Files
cp -v ${RdlReaderDir}/RdlCri.dll ./Files
cp -v ${RdlReaderDir}/RdlEngine.dll ./Files
cp -v ${RdlReaderDir}/RdlEngineConfig.xml ./Files
cp -v ${RdlReaderDir}/RdlReader.exe ./Files
cp -v ${RdlReaderDir}/RdlViewer.dll ./Files
cp -v ${RdlReaderDir}/System.Data.SqlClient.dll ./Files
cp -r -v ${RdlReaderDir}/ru-RU ./Files

# Очистка бин от лишний файлов

rm -v -f ${BinDir}/*.mdb
rm -v -f ${BinDir}/*.pdb

cp -r -v ${BinDir}/* ./Files

if [ ! -f "gtk-sharp-2.12.21.msi" ]; then
    wget https://files.qsolution.ru/Common/gtk-sharp-2.12.21.msi
fi

wine ~/.wine/drive_c/Program\ Files\ \(x86\)/NSIS/makensis.exe /INPUTCHARSET UTF8 ${ProjectName}.nsi
