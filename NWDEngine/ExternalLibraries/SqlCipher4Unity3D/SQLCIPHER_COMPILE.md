# Compile SQLCipher

## Install Java 13


See https://www.oracle.com/java/technologies/javase-jdk13-downloads.html

Or download directly
https://download.oracle.com/otn-pub/java/jdk/13.0.2+8/d4173c853231432d94f001e99d882ca7/jdk-13.0.2_osx-x64_bin.dmg

## OpenSSL

https://github.com/openssl/openssl




??? https://github.com/leenjewel/openssl_for_ios_and_android ???

https://stackoverflow.com/questions/52717228/how-to-compile-openssl-1-1-1-for-android

https://github.com/openssl/openssl/blob/master/NOTES.ANDROID

```
mkdir ~/OpenSSL
cd ~/OpenSSL
wget https://www.openssl.org/source/openssl-1.1.1a.tar.gz
tar -xzvf openssl-1.1.1a.tar.gz
cd openssl-1.1.1a
./config --prefix=/usr/local/mac-dev-env/openssl-1.1.1a


export ANDROID_NDK_HOME=/home/whoever/Android/android-sdk/ndk/20.0.5594570
	PATH=$ANDROID_NDK_HOME/toolchains/llvm/prebuilt/linux-x86_64/bin:$ANDROID_NDK_HOME/toolchains/arm-linux-androideabi-4.9/prebuilt/linux-x86_64/bin:$PATH
	./Configure android-arm64 -D__ANDROID_API__=29
	make
	
make
make install
```
## Android

https://github.com/sqlcipher/android-database-sqlcipher/#building

Clone project

```
mkdir ~/SQLCipher-Android
cd ~/SQLCipher-Android
git clone https://github.com/sqlcipher/android-database-sqlcipher.git
cd android-database-sqlcipher
```

Compile for Android for release with Unity3D

```
make init
echo "export ANDROID_HOME=/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK \
      export ANDROID_SDK_ROOT=/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK" \
>> ~/.bash_profile && source ~/.bash_profile

/Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK/tools/bin/sdkmanager --licenses

yes | /Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK/tools/bin/sdkmanager --licenses && yes | /Applications/Unity/Hub/Editor/2019.3.3f1/PlaybackEngines/AndroidPlayer/SDK/tools/bin/sdkmanager --update

make build-release
```


Compile for Android for release with Android Studio

```
make init
echo "export ANDROID_HOME=~/Library/Android/sdk \
      export ANDROID_SDK_ROOT=~/Library/Android/sdk" \
>> ~/.bash_profile && source ~/.bash_profile

./configure --enable-tempstore=yes CFLAGS="-DSQLITE_HAS_CODEC" LDFLAGS="/usr/local/opt/openssl/lib/libcrypto.a" CPPFLAGS="-I/usr/local/opt/openssl/include"

make build-release
```

Check Libs


## MacOS

Create directory

```
mkdir ~/SQLCipher_MacOS
cd ~/SQLCipher_MacOS
```

Clone project SQLCipher

```
git clone https://github.com/sqlcipher/sqlcipher.git
```

Copy project  OpenSSL

```
wget https://www.openssl.org/source/openssl-1.1.1a.tar.gz
tar -xzvf openssl-1.1.1a.tar.gz
rm openssl-1.1.1a.tar.gz
cd openssl-1.1.1a
./Configure darwin64-x86_64-cc
make

```

Make Lib Static

```
cd ~/SQLCipher_MacOS/sqlcipher

export LIBRARY_PATH="$LIBRARY_PATH:~/SQLCipher_MacOS/openssl-1.1.1a/"
./configure --enable-tempstore=yes CFLAGS="-DSQLITE_HAS_CODEC" CPPFLAGS="-I~/SQLCipher_MacOS/openssl-1.1.1a/include"
make
```

Make Lib Dynamic

```
cd ~/SQLCipher_MacOS/sqlcipher

export LIBRARY_PATH="$LIBRARY_PATH:/usr/local/opt/openssl/lib/"
./configure --enable-tempstore=yes CFLAGS="-DSQLITE_HAS_CODEC" LDFLAGS="~/SQLCipher_MacOS/openssl-1.1.1a/lib/libcrypto.a"
make
```

Check Libs

```
file .libs/sqlite3.o
open .libs/
```


https://vinceyuan.github.io/build-sqlcipher-on-mac-os-x/



## iOS

Discussion : 
- https://discuss.zetetic.net/t/unity-ios-build-work-with-debug-mode-but-crash-with-release-mode-archive-ipa/278



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