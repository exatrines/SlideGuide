<p align="center">
  <img src="../SlideGuide/Data/plugin-icon.png" alt="SlideGuide アイコン" width="128" height="128">
</p>

<h1 align="center">SlideGuide</h1>

<p align="center">
  <a href="../README.md">English</a> | 日本語
</p>

<p align="center">
  <a href="https://github.com/exatrines/SlideGuide/releases/latest">
    <img src="https://img.shields.io/github/v/release/exatrines/SlideGuide?label=Release&amp;labelColor=F280B6&amp;color=FFFFFF&amp;style=flat&amp;sort=date&amp;display_name=tag" alt="Release">
  </a>
  <a href="../CHANGELOG.md">
    <img src="https://img.shields.io/badge/Changelog-view-FFFFFF?labelColor=F280B6&amp;style=flat" alt="Changelog">
  </a>
  <a href="../LICENSE">
    <img src="https://img.shields.io/badge/License-AGPL--3.0--or--later-FFFFFF?labelColor=F280B6&amp;style=flat" alt="AGPL-3.0-or-later">
  </a>
</p>

<p align="center">
  <img src="screenshots/hero-1280x720.png" alt="ネイティブのキャストバー上の SlideGuide マーカー">
</p>

ネイティブのキャストバーに、滑りうちのタイミングをオーバーレイで描きます。あくまで目安であり、ゲームのフレームレートなどによって実際の成否と異なる場合があります。

## インストール

1. `/xlsettings` を実行し、**試験的機能**タブを開く
2. **カスタムプラグインリポジトリ** に次の URL を追加する:

```
https://raw.githubusercontent.com/exatrines/DalamudPlugins/refs/heads/main/pluginmaster.json
```

3. `/xlplugins` を実行し、**SlideGuide** をインストールする

## 機能

- **オーバーレイマーカー** — 設定した滑りうちの閾値で、ネイティブのキャストバーに縦線を重ねます。HUD 自体は変更しません。
- **見た目** — 閾値、線の太さ、上下への延長、到達前後の色。

## コマンド

| コマンド | 説明 |
| --- | --- |
| `/slideguide` | 設定の表示切替 |

## 開発者向け

1. `git submodule update --init --recursive`
2. ビルド: `dotnet build SlideGuide.sln -c Release -p:Platform=x64`
3. Dalamud の **dev plugin** に `SlideGuide/bin/Release/` を指定する
4. プラグインインストーラ（dev）で **SlideGuide** を有効にする

共有 UI キットの [MirageUI](https://github.com/exatrines/MirageUI) を git サブモジュールとして同梱しています。

## コントリビューション

コントリビューションは大歓迎です！[貢献ガイド](../CONTRIBUTING.md)をご覧ください。
