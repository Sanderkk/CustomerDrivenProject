# CustomerDrivenProject
Customer driven project, group 6 with Sintef Ocean

## Git branching convention

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

The branchname should be an named 

### Commit message structure
The commit message should follow this message structure:
- branchtype: commit message

Commit message should describe what the code does, and not what you have done. 

## How to merge a branch into dev
To merge a branch into dev, it is important that everyone follows the same principals to keep the commit history neat and organized. 
When you are going to go ahead on an issue, it is important that you make a new branch for the issue following the git branching convention described above.
Follow these instructions when making a new branch: 
- `git checkout dev`
- `git fetch --all`
- `git checkout -b branchntype/branchname`

When you are finished with your updates, it is important that everyone follows the convention for rebasing.
We are following the pattern of **one commit** per branch. This means that you need to squash commits, if you have several commits.
The instructions for this is:
- `git commit -am "branchtype: commit message"`
- `git fetch --all`
- `git rebase -i origin/dev`
- `git push -f`

After pushing your branch, you need to go to github and create a new pull request for the branch. 
