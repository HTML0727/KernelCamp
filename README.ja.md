# KernelCamp - Linuxカーネル設定ツール

CとC#に基づくクロスプラットフォームなLinuxカーネル設定ツール。直感的なGUIインターフェースでカーネルパラメータと設定を変更できます。

## 🚀 バージョン情報

**現在のバージョン**: 0.a1 (アルファテスト版)
**プロジェクト状況**: 積極的に開発中
**対象プラットフォーム**: Linux x86_64

## ✨ 機能特徴

- 🖥️ **モダンなUI**: GTK#ベースのGNOMEスタイルインターフェース
- 🔧 **カーネル検出**: 現在のLinuxカーネルバージョンと設定を自動検出
- ⚙️ **パラメータ管理**: カーネルパラメータの視覚的な切り替えと変更
- 📋 **ドロップダウンオプション**: 利用可能なパラメータオプションを提供するスマートなドロップダウンメニュー
- 📦 **ポータブル配布**: AppImageパッケージ形式をサポート
- 🐧 **Linuxネイティブ**: Linuxシステム専用に設計

## 🏗️ 技術アーキテクチャ

### 低レベル (C言語)
- カーネル設定の解析と変更
- システムコールインターフェース
- 設定ファイル処理
- カーネルモジュール連携

### ユーザーインターフェース (C#/.NET)
- GTK#グラフィカルインターフェースフレームワーク
- GNOME Human Interface Guidelines準拠
- クロスプラットフォームUIレンダリング
- ユーザーインタラクションロジック

## 📋 ビルド要件

### 開発環境
- .NET 6.0+ SDK
- Mono開発ツールチェーン
- GTK# 3.0
- GCCコンパイラ
- Linuxカーネルヘッダー

### 実行時依存関係
- .NET 6.0ランタイム
- GTK3
- libappimage

## 📦 インストールと使用方法

### 🐳 Dockerを使用（推奨）
```bash
# ビルドと実行
docker-compose run --rm builder

# または開発環境に入る
docker-compose run --rm kernelcamp
# コンテナ内で実行: make run
```

### ソースからビルド
```bash
# リポジトリをクローン
git clone https://github.com/your-username/KernelCamp.git
cd KernelCamp

# 依存関係をインストール
sudo apt-get install build-essential mono-devel gtk-sharp3 libappimage-dev

# プロジェクトをビルド
make build

# AppImageを生成
make appimage
```

### AppImageを直接実行
```bash
chmod +x KernelCamp-x86_64.AppImage
./KernelCamp-x86_64.AppImage
```

## 📁 プロジェクト構造

```
KernelCamp/
├── src/
│   ├── native/          # C言語低レベルライブラリ
│   │   ├── kernel.c     # カーネル操作インターフェース
│   │   ├── config.c     # 設定解析
│   │   └── Makefile
│   ├── ui/              # C#ユーザーインターフェース
│   │   ├── MainWindow.cs # メインウィンドウ
│   │   ├── KernelView.cs # カーネル情報ビュー
│   │   └── ConfigEditor.cs # 設定エディター
│   └── shared/          # 共有コード
├── build/               # ビルド出力
├── packaging/           # パッケージ設定
│   └── AppImageBuilder/ # AppImageビルド設定
└── docs/                # ドキュメント
```

## 🤝 貢献ガイドライン

1. このプロジェクトをフォーク
2. 機能ブランチを作成 (`git checkout -b feature/AmazingFeature`)
3. 変更をコミット (`git commit -m 'Add some AmazingFeature'`)
4. ブランチにプッシュ (`git push origin feature/AmazingFeature`)
5. プルリクエストを開く

## 📄 ライセンス

このプロジェクトはNC-OSLライセンスを使用しています - 詳細は[LICENSE](LICENSE)ファイルをご覧ください。

## 💬 サポート

問題が発生した場合や提案がある場合:
- Issueを投稿
- メールを送信: nekosparry0727@outlook.com

## ⚠️ 免責事項

⚠️ **警告**: カーネルパラメータの変更はシステムの安定性に影響する可能性があります。専門家の指導のもとで操作し、重要なデータはバックアップしてください。

## 🌍 多言語サポート

- [English](README.en.md) - 英語ドキュメント
- 日本語 - 現在のドキュメント
- [中文](README.md) - 中国語ドキュメント

---

Nekosparry 2025 | 全著作権所有

## 👥 開発チーム

**プロジェクトリード**: Nekosparry
**開発状況**: 個人プロジェクト (現在Nekosparryが単独で開発中)