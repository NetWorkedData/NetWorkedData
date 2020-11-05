# To clean the project

## Resolve Host key verification failed

```text
ssh test@github.com
ssh test@hephaiscode.com
```

and accept to add to ~/.ssh/known\_hosts

## Clean OSX

```text
git rm -r .DS_Store
```

## Clean NetWorkedData

```text
git rm -r *.prp
git rm Assets/NWD000000000Account.prp
```

## For submodule

```text
git checkout --orphan empty

git submodule update
git submodule init --recursive
git submodule update --recursive

git submodule foreach --recursive "git fetch origin || true"

git submodule foreach --recursive "git checkout empty || true"
git submodule update --recursive
```

```text
git submodule foreach --recursive "git fetch origin || true"
git submodule foreach --recursive "git checkout master || true"
git submodule foreach --recursive "git checkout development || true"
git submodule update --recursive
git submodule foreach --recursive "git pull || true"
```

```text
git submodule foreach --recursive "git checkout master || true"
git submodule update --recursive
```

```text
git submodule foreach --recursive "git pull || true"
```

