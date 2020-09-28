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
- JIRA-ID: commit message

Commit message should describe what the code does, and not what you have done.


### How to merge a branch into dev 

When you are going to go ahead on an issue, it is important that you make a new branch for the issue following the git branching convention described above.
Follow these instructions when making a new branch: 
- `git checkout dev`
- `git fetch --all`
- `git reset --hard origin/dev`
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
