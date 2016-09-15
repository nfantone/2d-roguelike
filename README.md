# 2D Roguelike

![2D Roguelike](https://unity3d.com/sites/default/files/learn-playlist/icon/2droguelike-thumb1.jpg)

_Updated to work on Unity3D `5.5.0b3`._

[Unity3D][1] tutorial implementation for a procedural 2D, tile based game.

This repository also follows [some][6] [known][2] [practices][3] for working with Unity3D projects alongside [Git][4] and [Travis CI][5].

## Configuration
Before you start coding, you might need to setup a few things in your local environment first.

- Install and enable [Git Large File Storage][7] plugin (or `git lfs` for short). The [`.gitattributes`](./.gitattributes) file will direct Git to use LFS on multimedia assets, such as `*.aif` and `*.png` files.
- Setup [UnityYAMLMerge][8] to work with Git by appending the following to your local `.git/config`:

```sh
[merge]
tool = unityyamlmerge

[mergetool "unityyamlmerge"]
trustExitCode = false
# /Applications/Unity/Unity.app/Contents/Tools/UnityYAMLMerge on MacOSX
# C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe on Windows
cmd = '/path/to/UnityYAMLMerge' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```

> Replace `/path/to/UnityYAMLMerge` with the actual local path to the tool.

- (Optional) Configure a local [Git hook to automatically remove empty directories][9] on post-merge.

### Resources
- Download latest version of [Git][10] and [Git LFS][11]
- Get [latest Unity3D][12].
- Go follow the [original tutorial][13].
- Download needed [free assets][14].

---

### License
MIT

[1]: https://unity3d.com/unity
[2]: https://jonathan.porta.codes/2015/04/17/automatically-build-your-unity3d-project-in-the-cloud-using-travisci-for-free/
[3]: http://www.strichnet.com/using-git-with-3d-games/
[4]: https://git-scm.com
[5]: https://travis-ci.org
[6]: http://dmayance.com/git-and-unity-projects/
[7]: https://git-lfs.github.com/
[8]: http://docs.unity3d.com/Manual/SmartMerge.html
[9]: https://github.com/strich/git-dir-cleaner-for-unity3d
[10]: https://git-scm.com/downloads
[11]: https://help.github.com/articles/installing-git-large-file-storage/
[12]: https://unity3d.com/unity/beta
[13]: https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial
[14]: https://www.assetstore.unity3d.com/en/#!/content/29825
