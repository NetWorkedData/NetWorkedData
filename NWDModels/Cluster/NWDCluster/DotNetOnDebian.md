## Install Debian with .NET and first test

log on server

```
cat /etc/debian_version
```

return 10

```
wget https://packages.microsoft.com/config/debian/10/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

apt-get update
apt-get dist-upgrade
apt-get install -y apt-transport-https
apt-get update
apt-get install -y dotnet-sdk-2.1
apt-get install -y dotnet-runtime-2.1
```

Ok test

```
dotnet --version
```

must return 2.1.xxx


https://docs.microsoft.com/fr-fr/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-5.0



https://www.developpez.com/actu/245430/ASP-NET-Core-est-le-3e-serveur-Web-le-plus-rapide-repondant-a-7-millions-de-requetes-HTTP-s-selon-un-test-de-TechEmpower/



https://riptutorial.com/fr/unity3d/example/20116/creer-un-serveur--un-client-et-envoyer-un-message-