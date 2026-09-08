# SlideGuide

[English](README.md)

![ネイティブのキャストバー上の SlideGuide マーカー](docs/screenshots/hero-1280x720.png)

ネイティブのキャストバーに滑りうちタイミングをオーバーレイで描画します。あくまで目安であり、ゲームのフレームレートなどによって実際の成否と異なる場合があります。

## インストール

1. `/xlsettings` を開き、**試験的機能** タブを開く
2. **カスタムプラグインリポジトリ** に次の URL を追加する:

```
https://raw.githubusercontent.com/exatrines/DalamudPlugins/refs/heads/main/pluginmaster.json
```

3. `/xlplugins` を開き、**SlideGuide** をインストールする

## 機能

- **オーバーレイマーカー** — 設定した滑りうち閾値に、ネイティブのキャストバーへ縦線を重ねます。HUD 自体は変更しません。
- **見た目** — 閾値、線の太さ、上下への延長、到達前後の色。

## コマンド

| コマンド | 説明 |
| --- | --- |
| `/slideguide` | 設定の表示切替。 |

## 開発者向け

1. ビルド: `dotnet build SlideGuide.sln -c Release -p:Platform=x64`
2. Dalamud の **dev plugin** に `SlideGuide/bin/Release/` を指定する
3. プラグインインストーラ（dev）で **SlideGuide** を有効にする

共有 UI キットの [MirageUI](https://github.com/exatrines/MirageUI) を git サブモジュールとして同梱しています。

## ライセンス

[AGPL-3.0-or-later](LICENSE)
