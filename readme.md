# FNA Engine

Library of useful classes and utilies for FNA.

# Installation

1. Clone this repo, preferably somewhere with easy access to your game and FNA.
2. In `engine.csproj` change the FNA include reference to wherever you've placed FNA.

```
<!-- Reference FNA project -->
  <ItemGroup>
    <ProjectReference Include="path.to.FNA" />
  </ItemGroup>
```

3. run Clean build task
4. email us when you realize we forgot something

# Using this in your game

Within your games `.csproj` file, add the following:

```
  <ItemGroup>
    <ProjectReference Include="path.to.this.repo.engine.csproj" />
  </ItemGroup>
```

Then, use `using Engine;` along with the rest of your includes within your .cs files.

# Notes

We mainly develop via VS Code on OS X, so our setup or instructions may not translate over to Visual Studio or another IDE.

This project was initially bootstrapped with [this template](https://github.com/TheSpydog/fna_vscode_template), which gave us the initial .vscode functions. We don't author any of it, but we may fork it soon?

## Also Notes

We very new to FNA, monogame, and C# in general.
