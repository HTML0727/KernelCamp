# KernelCamp - Linux Kernel Configuration Tool

A cross-platform Linux kernel configuration tool based on C and C#, providing an intuitive GUI interface to modify kernel parameters and configurations.

## 🚀 Version Information

**Current Version**: 0.a1 (Alpha Testing)
**Project Status**: Active Development
**Target Platform**: Linux x86_64

## ✨ Features

- 🖥️ **Modern UI**: GNOME-style interface based on GTK#
- 🔧 **Kernel Detection**: Automatically detects current Linux kernel version and configuration
- ⚙️ **Parameter Management**: Visual toggle and modification of kernel parameters
- 📋 **Dropdown Options**: Smart dropdown menus providing available parameter options
- 📦 **Portable Deployment**: Supports AppImage packaging format
- 🐧 **Linux Native**: Designed specifically for Linux systems

## 🏗️ Technical Architecture

### Low-level (C Language)
- Kernel configuration parsing and modification
- System call interfaces
- Configuration file handling
- Kernel module interaction

### User Interface (C#/.NET)
- GTK# graphical interface framework
- GNOME Human Interface Guidelines compliant
- Cross-platform UI rendering
- User interaction logic

## 📋 Build Requirements

### Development Environment
- .NET 6.0+ SDK
- Mono development toolchain
- GTK# 3.0
- GCC compiler
- Linux kernel headers

### Runtime Dependencies
- .NET 6.0 runtime
- GTK3
- libappimage

## 📦 Installation and Usage

### 🐳 Using Docker (Recommended)
```bash
# Build and run
docker-compose run --rm builder

# Or enter development environment
docker-compose run --rm kernelcamp
# Inside container: make run
```

### Build from Source
```bash
# Clone repository
git clone https://github.com/your-username/KernelCamp.git
cd KernelCamp

# Install dependencies
sudo apt-get install build-essential mono-devel gtk-sharp3 libappimage-dev

# Build project
make build

# Generate AppImage
make appimage
```

### Run AppImage Directly
```bash
chmod +x KernelCamp-x86_64.AppImage
./KernelCamp-x86_64.AppImage
```

## 📁 Project Structure

```
KernelCamp/
├── src/
│   ├── native/          # C language low-level library
│   │   ├── kernel.c     # Kernel operation interface
│   │   ├── config.c     # Configuration parsing
│   │   └── Makefile
│   ├── ui/              # C# user interface
│   │   ├── MainWindow.cs # Main window
│   │   ├── KernelView.cs # Kernel information view
│   │   └── ConfigEditor.cs # Configuration editor
│   └── shared/          # Shared code
├── build/               # Build output
├── packaging/           # Packaging configuration
│   └── AppImageBuilder/ # AppImage build configuration
└── docs/                # Documentation
```

## 🤝 Contribution Guidelines

1. Fork this project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

## 📄 License

This project uses NC-OSL License - See [LICENSE](LICENSE) file for details.

## 💬 Support

If you encounter issues or have suggestions:
- Submit an Issue
- Send email to nekosparry0727@outlook.com

## ⚠️ Disclaimer

⚠️ **Warning**: Modifying kernel parameters may affect system stability. Please operate under professional guidance and backup important data.

## 🌍 Multi-language Support

- English - Current document
- [日本語](README.ja.md) - Japanese documentation
- [中文](README.md) - Chinese documentation

---

Nekosparry 2025 | All rights reserved

## 👥 Development Team

**Project Lead**: Nekosparry
**Development Status**: Personal Project (Currently developed independently by Nekosparry)