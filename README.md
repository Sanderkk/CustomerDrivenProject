# CustomerDrivenProject
Customer driven project, group 6 with Sintef Ocean

## Git conventions

To keep the repository neat and organized, it is important that everyone follows the same git conventions.
The repository have two protected branches, **main** and **dev**. 
This means that one must create and pull request to merge code into these branches. 
The instructions for creating a branch and merging into `dev` is described below.

### Git branching convention

Git branches should be named following this format:
 - branchtype/branchname

This are the branchtypes:
- feat: New feature
- enh: Enhancement on exisiting feature
- bug: bugfixes
- test: New test
- hot: Hotfixes (Urgent bug fixes deployed directly to main/dev)
- documentation: Improvements or additions to documentation
- duplicate: This issue or pull request already exists

The branchname should be an indication of which issue you are working on.

### Commit message structure
The commit message should follow this message structure:
- branchtype: commit message

Commit message should describe what the code does, and not what you have done. 


### How to merge a branch into dev 

When you are going to go ahead on an issue, it is important that you make a new branch for the issue following the git branching convention described above.
Follow these instructions when making a new branch: 
- `git checkout dev`
- `git reset --hard origin/dev`
- `git fetch --all`
- `git checkout -b branchntype/branchname`

When you are finished with your updates, it is important that everyone follows the convention for rebasing.
We are following the pattern of **one commit** per branch. This means that you need to squash commits, if you have several commits.
The instructions for this is:
- `git commit -am "branchtype: commit message"`
- `git fetch --all`
- `git rebase -i origin/dev`

If there occurs merge conflicts, they can be resolved in the IDE. After you have resolved every merge conflict, add the modified files with `git add .`, and write `git rebase --continue`.

- `git push -f`


After pushing your branch, you need to go to github and create a new pull request for the branch.


## Configure dev environment

### Database
We will utilize `Timescaledb` as timeseries database, and `PostgreSQL` as relational database.

The database can be run locally. We have chosen to use docker for this.
To create this database with docker you can run the command
`` sudo docker run -d --name timescaledb-customerDriven -p 5432:5432 -e POSTGRES_PASSWORD=password timescale/timescaledb:latest-pg12 ``
With this, the database runs locally at port 5432, with password `password`.

There is also a database running at `http://sanderkk.com:5432`, using the same password.

(This will change once we deploy to azure)

### Backend
The backend is developed in C# and .Net core, and thus needs the `dotnet-core SDK 3.1` for running.
The solutions and tests are written with .Net core, and this is all you should need for the functions

Download .Net core from microsoft, at:
https://dotnet.microsoft.com/download/dotnet-core/3.1

To run the backend locally, you need to follow a few steps.

#### Running backend API
Go to the `/backend/src` directory
- First restore the nuget packages
`` dotnet restore ``
- Then you can run the backend with the command
`` dotnet run watch ``
The api is now running at port 5000

#### Running tests
Go to `/backend` where the sln file is located.
- First restore the nuget packages
`` dotnet restore ``
- Then you can run the tests with the command
`` dotnet test ``

### Frontend
The frontend is created with node and react.
You therefore need `node` installed.
We develop using the current lts version.

To run the frontend in a dev server, use the command `npm start` in the `/frontend/src` directory.

React tests can be run with the command `npm test` in the `/frontend/src` directory.

End-to-end tests are based on node as-well.
These can be run with the command `npm test` in the `/frontend/tests` directory.
For the end-to-end tests to run, specific webdriver for different browsers are needed. Currently, the driver for chrome is currently located in the `/frontend/tests` directory.

(Remember to install node packages locally first. This can be done with the command `npm install`)
