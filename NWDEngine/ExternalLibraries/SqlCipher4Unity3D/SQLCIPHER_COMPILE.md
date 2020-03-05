# Compile SQLCipher

## Android

Clone project

```
mkdir SQLCipher-Android
cd SQLCipher-Android
git clone https://github.com/sqlcipher/android-database-sqlcipher.git
cd android-database-sqlcipher
```

Compile for Android for release with Unity3D

```
make init
echo "export ANDROID_HOME=/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK \
      export ANDROID_SDK_ROOT=/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK \
      export ANDROID_NDK_HOME=/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/NDK" \
>> ~/.bash_profile && source ~/.bash_profile

make build-release
```

Check Libs


## MacOS

Clone project

```
mkdir SQLCipher
cd SQLCipher
git clone https://github.com/sqlcipher/sqlcipher.git
cd sqlcipher
```

Install openssl

```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"
brew install wget
brew install openssl
```

Make Libs

```
export LIBRARY_PATH="$LIBRARY_PATH:/usr/local/opt/openssl/lib/"
./configure --enable-tempstore=yes CFLAGS="-DSQLITE_HAS_CODEC" LDFLAGS="/usr/local/opt/openssl/lib/libcrypto.a" CPPFLAGS="-I/usr/local/opt/openssl/include"
make
```

Check Libs

```
file .libs/sqlite3.o
open .libs/
```

Discussion : 
- https://discuss.zetetic.net/t/unity-ios-build-work-with-debug-mode-but-crash-with-release-mode-archive-ipa/278








echo "export ANDROID_HOME=~/Library/Android/sdk \
      export ANDROID_SDK_ROOT=~/Library/Android/sdk \
      export ANDROID_NDK_HOME=~/Library/Android/sdk/ndk-bundle \
      export ANDROID_AVD_HOME=~/.android/avd" \
>> ~/.bash_profile && source ~/.bash_profile

yes | $ANDROID_HOME/tools/bin/sdkmanager "platforms;android-26 Android SDK Platform 26"

yes | $ANDROID_HOME/tools/bin/sdkmanager "build-tools;26"









### What is this for? ###

* SQLCipher compile source

download https://github.com/sqlcipher/sqlcipher/archive/master.zip
unzip
open terminal and go in the folder 

### How do I get set up? ###

* In a terminal:
```
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install.sh)"
brew install wget
brew install openssl
export LIBRARY_PATH="$LIBRARY_PATH:/usr/local/opt/openssl/lib/"
./configure --enable-tempstore=yes CFLAGS="-DSQLITE_HAS_CODEC" LDFLAGS="/usr/local/opt/openssl/lib/libcrypto.a" CPPFLAGS="-I/usr/local/opt/openssl/include"
make
```
### What next? ###

* Find files in **.libs** folder.
* Enjoy :)