# SlideGuide

[日本語](README.ja.md)

![SlideGuide marker on the native castbar](docs/screenshots/hero-1280x720.png)

Draws slidecast timing as an overlay on the native castbar. It is only a guide; game frame rate and similar factors can make the actual result differ.

## Install

1. Run `/xlsettings` and open the **Experimental** tab
2. Add this URL under **Custom Plugin Repositories**:

```
https://raw.githubusercontent.com/exatrines/DalamudPlugins/refs/heads/main/pluginmaster.json
```

3. Run `/xlplugins` and install **SlideGuide**

## Features

- **Overlay marker** — A vertical line on the native castbar at your slidecast threshold. The HUD itself is not modified.
- **Appearance** — Threshold, line width, extra height, and before/after colors.

## Commands

| Command | Description |
| --- | --- |
| `/slideguide` | Toggle settings. |

## For developers

1. Build: `dotnet build SlideGuide.sln -c Release -p:Platform=x64`
2. Point Dalamud’s **dev plugin** path at `SlideGuide/bin/Release/`
3. Enable **SlideGuide** in the plugin installer (dev)

[MirageUI](https://github.com/exatrines/MirageUI) is included as a git submodule for the shared UI kit.

## License

[AGPL-3.0-or-later](LICENSE)
