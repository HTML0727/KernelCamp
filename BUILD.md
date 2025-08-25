# KernelCamp 构建指南

## 🚀 版本信息

**当前版本**: 0.a1 (Alpha测试版)
**项目状态**: 积极开发中

## 📋 构建要求

### 开发环境
- **操作系统**: Linux (推荐 Ubuntu 20.04+ 或 Fedora 32+)
- **编译器**: GCC 9.0+
- **.NET SDK**: 6.0+
- **GTK#**: 3.24+
- **Mono**: 6.12+

### 构建工具
- make
- dotnet CLI
- linuxdeploy (用于AppImage打包)

## 🛠️ 构建步骤

### 1. 安装依赖

#### Ubuntu/Debian:
```bash
sudo apt-get update
sudo apt-get install -y \
    build-essential \
    mono-devel \
    gtk-sharp3 \
    libgtk3.0-cil-dev \
    dotnet-sdk-6.0 \
    libappimage-dev \
    linuxdeploy \
    linuxdeploy-plugin-gtk
```

#### Fedora:
```bash
sudo dnf install -y \
    gcc \
    make \
    mono-devel \
    gtk-sharp3 \
    dotnet-sdk-6.0 \
    libappimage \
    linuxdeploy \
    linuxdeploy-plugin-gtk
```

### 2. 构建项目

#### 构建所有组件:
```bash
make all
```

#### 仅构建原生库:
```bash
make build-native
```

#### 仅构建UI应用:
```bash
make build-ui
```

### 3. 运行应用

```bash
make run
```

### 4. 生成AppImage

```bash
make appimage
```

## 📁 项目结构

```
NeuroFromScratch/
├── src/
│   ├── native/           # C语言底层库
│   │   ├── kernel.c      # 内核操作接口
│   │   ├── kernel.h      # 头文件
│   │   └── Makefile      # 构建配置
│   └── ui/               # C#用户界面
│       ├── Program.cs    # 程序入口
│       ├── MainWindow.cs # 主窗口
│       └── NeuroFromScratch.csproj # 项目文件
├── build/                # 构建输出
│   ├── lib/              # 原生库文件
│   └── bin/              # 可执行文件
├── packaging/            # 打包配置
│   └── AppImageBuilder/  # AppImage构建配置
├── Makefile              # 主构建文件
├── README.md             # 项目说明
└── BUILD.md              # 构建指南
```

## 🔧 开发说明

### 原生库开发

C语言库提供以下核心功能：
- 内核版本检测
- 配置文件路径查找
- 配置选项读取
- 参数修改接口
- 选项值验证

### UI界面开发

C#界面基于GTK#框架，提供：
- GNOME风格的用户界面
- 配置选项表格显示
- 下拉菜单参数选择
- 实时预览和修改
- 应用更改功能

### 跨平台支持

项目设计为Linux专用，但代码结构支持扩展：
- Windows支持 (需要修改内核接口)
- macOS支持 (需要修改系统调用)

## 📦 打包说明

### AppImage打包

AppImage打包使用linuxdeploy工具：
1. 构建完整的应用程序
2. 收集所有运行时依赖
3. 创建自包含的AppImage文件
4. 设置正确的桌面集成

### 依赖管理

- **原生依赖**: 通过系统包管理器管理
- **.NET依赖**: 通过NuGet包管理
- **运行时依赖**: 包含在AppImage中

## 🐛 故障排除

### 常见问题

1. **GTK#未找到**: 安装gtk-sharp3开发包
2. **.NET运行时错误**: 确保安装.NET 6.0 SDK
3. **原生库加载失败**: 检查库路径和权限
4. **AppImage打包失败**: 安装linuxdeploy工具

### 调试模式

启用详细输出：
```bash
make build-native V=1
make build-ui /v:detailed
```

## ℹ️ 版本信息

- **当前版本**: 0.a1 (Alpha)
- **目标平台**: Linux x86_64
- **构建系统**: GNU Make + .NET SDK
- **打包格式**: AppImage

---

更多信息请参考 [README.md](README.md)