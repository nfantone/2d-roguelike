#! /bin/sh

BIN_NAME="$1"

echo "Attempting to build [$BIN_NAME] for Windows"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile "unity.log" \
  -projectPath "$(pwd)" \
  -buildWindowsPlayer "Build/windows/$BIN_NAME.exe" \
  -quit

echo "Attempting to build [$BIN_NAME] for OS X"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
  -batchmode \
  -nographics \
  -silent-crashes \
  -logFile "unity.log" \
  -projectPath "$(pwd)" \
  -buildOSXUniversalPlayer "Build/osx/$BIN_NAME.app" \
  -quit

echo 'Showing logs from build'
cat unity.log
