# README

**Test avec GitBook**

**Explication en français**

**à faire traduire**

## NetWorkedData

### Qu'est-ce-que NetWorkedData ?

NetWorkedData est un system de type ORM/ODB conçu pour Unity3D© et dédié au mobile \(mais compatible en standalone\). NetWorkedData permet de créer de la donnée dans l'éditeur et de la fournir dans l'application finale. NetWorkedData permet la mis-à-jour de la donnée par des serveurs de données. NetWorkedData permet d'enregistrement de la donnée joueur, de la sauvegarder sur des serveurs de données et de créer des relations entre joueurs.

### Les features de NetWorkedData

NetWorkedData est un Data Engine sécurisé au code ouvert.

#### Environnements

* environnement de développement
* environnement de pré-production
* environnement de production
* verrouillage de l'environnement de production pour les données joueurs

#### chargement des données

* chargement synchrone
* chargement asynchrone
* chargement par bundle
* vérification d'intégrité ligne par ligne
* grain de sel pour les calculs d'intégrité classe par classe

#### chargement des données en mode editor

* chargement analytique dans l'editor
* chargement transparent en playmode pour les tests des scenes en dévelopement 
* bypass intégrité pour l'editeur

#### CRUD des données

* méthodes virtuelles à chaue étapes pour ajouter des fonctionnalités d'édition
* création simplifié
* sauvegarde asynchrone et synchrone
* queue de requête de sauvegarde
* notion de 
  * désactivation
  * poubelle 
  * destruction

#### Type de données

* données éditeur \(**Editor**\)
* données joueurs \(notion de **Account**\)
* données par partie \(notion de **GameSave** et de **User**\)
* Gestion des langues \(normes **i18n**\)
  * changement des langues dynamiques
  * multilingue à l'affichage simultanée
  * auto-détermination par device
* taguage des données
* bundle des données

#### Interfacage avec Unity3D

* connexion de données avec les **MonoBehaviour**

#### Réseau

* création de serveur 
* mise en place des webservices via procédure SFTP incrémentiel
* auto-certification des comptes anonymes de joueurs
* signature des comptes 
  * email password
  * login email password
  * token social network
    * Facebook©
    * Google©
    * AppleID©
* token d'authentification avec sécurisation par Account, grain de sel à fréquence temporel et grain de sel sel fixe
  * gestion d'historique de token à niveau variable pour revenir des déconnexions intempestives
* creation de sous fonctions PHP pour chaque classes

### Installation en 10 minutes des services en local

### Installation en 30 minutes des services en réseaux

pour le synchronisation :

* Une donnée corrompue ne peut pas être écrite sur le serveur
* Une donnée corrompue sur le serveur \(impossible normalement \) peut être rapatriée sur le device.
* Sur l'environnement de Dev le dernier qui sync une données écrit cette donnée

