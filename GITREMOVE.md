# To clean the project

## Clean OSX
```
git rm -r .DS_Store
```
## Clean NetWorkedData
```
git rm -r *.prp
git rm Assets/NWD000000000Account.prp
```

## For submodule
```
git submodule update
git submodule init --recursive
git submodule update --recursive

git submodule foreach --recursive "git checkout empty || true"
git submodule foreach --recursive "git checkout development || true"
git submodule foreach --recursive "git checkout master || true"


git submodule foreach --recursive "git pull || true"

```