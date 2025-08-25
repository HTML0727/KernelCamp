# KernelCamp - Linux内核配置工具

一个基于C和C#的跨平台Linux内核配置工具，提供直观的GUI界面来修改内核参数和配置。

## 🚀 版本信息

**当前版本**: 0.a1 (Alpha测试版)
**项目状态**: 积极开发中
**目标平台**: Linux x86_64

## ✨ 功能特性

- 🖥️ **现代化UI**: 基于GTK#的GNOME风格界面
- 🔧 **内核识别**: 自动检测当前Linux内核版本和配置
- ⚙️ **参数管理**: 可视化开关和修改内核参数
- 📋 **下拉选项**: 智能下拉菜单提供可用参数选项
- 📦 **便携部署**: 支持AppImage打包格式
- 🐧 **Linux原生**: 专为Linux系统设计

## 🏗️ 技术架构

### 底层 (C语言)
- 内核配置解析和修改
- 系统调用接口
- 配置文件处理
- 内核模块交互

### 用户界面 (C#/.NET)
- GTK#图形界面框架
- GNOME Human Interface Guidelines兼容
- 跨平台UI渲染
- 用户交互逻辑

## 📋 构建要求

### 开发环境
- .NET 6.0+ SDK
- Mono开发工具链
- GTK# 3.0
- GCC编译器
- Linux内核头文件

### 运行时依赖
- .NET 6.0运行时
- GTK3
- libappimage

## 📦 安装和使用

### 🐳 使用Docker（推荐）
```bash
# 构建和运行
docker-compose run --rm builder

# 或者进入开发环境
docker-compose run --rm kernelcamp
# 在容器内执行: make run
```

### 从源码构建
```bash
# 克隆仓库
git clone https://github.com/your-username/NeuroFromScratch.git
cd NeuroFromScratch

# 安装依赖
sudo apt-get install build-essential mono-devel gtk-sharp3 libappimage-dev

# 构建项目
make build

# 生成AppImage
make appimage
```

### 直接运行AppImage
```bash
chmod +x NeuroFromScratch-x86_64.AppImage
./NeuroFromScratch-x86_64.AppImage
```

## 📁 项目结构

```
NeuroFromScratch/
├── src/
│   ├── native/          # C语言底层库
│   │   ├── kernel.c     # 内核操作接口
│   │   ├── config.c     # 配置解析
│   │   └── Makefile
│   ├── ui/              # C#用户界面
│   │   ├── MainWindow.cs # 主窗口
│   │   ├── KernelView.cs # 内核信息视图
│   │   └── ConfigEditor.cs # 配置编辑器
│   └── shared/          # 共享代码
├── build/               # 构建输出
├── packaging/           # 打包配置
│   └── AppImageBuilder/ # AppImage构建配置
└── docs/                # 文档
```

## 🤝 贡献指南

1. Fork本项目
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 打开Pull Request

## 📄 许可证

本项目采用NC-OSL许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 💬 支持

如果您遇到问题或有建议：
- 提交Issue
- 发送邮件至 nekosparry0727@outlook.com

## ⚠️ 免责声明

⚠️ **警告**: 修改内核参数可能影响系统稳定性。请在专业人士指导下操作，并对重要数据进行备份。

## 🌍 多语言支持

- [English](README.en.md) - 英文文档
- [日本語](README.ja.md) - 日文文档
- 中文 - 当前文档

---

Nekosparry 2025 | 保留所有权利

## 👥 开发团队

**项目负责人**: Nekosparry
**开发状态**: 个人项目 (目前由Nekosparry独立开发)