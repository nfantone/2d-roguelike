#! /bin/sh

PROJECT="2d-roguelike"

echo "Attempting to build [$PROJECT] for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile "unity.log" \
  -projectPath "$(pwd)" \
  -buildWindowsPlayer "Build/windows/$PROJECT.exe" \
  -quit

echo "Attempting to build [$PROJECT] for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile "unity.log" \
  -projectPath "$(pwd)" \
  -buildOSXUniversalPlayer "Build/osx/$PROJECT.app" \
  -quit

echo 'Showing logs from build'
cat unity.log
