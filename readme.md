# FNA Utils

Library of useful classes and utilies for FNA.

# Installation

1. Clone this repo, preferably somewhere with easy access to your game and FNA.
2. In `utils.csproj` change the FNA include reference to wherever you've placed FNA.

```
<!-- Reference FNA project -->
  <ItemGroup>
    <ProjectReference Include="path.to.FNA" />
  </ItemGroup>
```

3. run the Restore build task
4. email us when you realize we forgot something

# Using this in your game

Within the game's `.csproj` file, add the following:

```
  <ItemGroup>
    <ProjectReference Include="path.to.utils.csproj" />
  </ItemGroup>
```

# Notes

This project was initially bootstrapped with [this template](https://github.com/TheSpydog/fna_vscode_template), which gave us the initial .vscode functions. We don't author any of it.

Some aspects of `FNA-Utils`, such as the `TextureLoader` class, and the initial template of the `Input` class, were also not authored by us, but were provided as parts of the Michael Hicks Toolbox, which we discovered through [this youtube series](https://www.youtube.com/watch?v=WQOebBVIB0I). It was instrumental in helping us understand how to begin developing games with FNA.

Other aspects, such as `Engine`, were inspired and built off of [Monocle](https://bitbucket.org/MattThorson/monocle-engine/src/default/Monocle/) by Matt Thorson.

## Also Notes

We are also very new to C# in general. All feedback is welcome :)

This project is still in early development and often undergoes massive changes, making stable documentation difficult, but it may be available, someday.