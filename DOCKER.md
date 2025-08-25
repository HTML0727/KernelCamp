# KernelCamp Docker 使用指南

## 📋 概述

KernelCamp 提供了完整的 Docker 开发环境，包含所有构建和运行依赖。

## 🐳 快速开始

### 1. 构建 Docker 镜像
```bash
# 构建特定发行版镜像
docker build --build-arg BASE_IMAGE=ubuntu:22.04 -t kernelcamp-ubuntu .
docker build --build-arg BASE_IMAGE=archlinux:latest -t kernelcamp-arch .
docker build --build-arg BASE_IMAGE=fedora:38 -t kernelcamp-fedora .
```

### 2. 使用 Docker Compose（推荐）

#### 开发环境（交互式终端）
```bash
# 使用Ubuntu环境（默认）
docker-compose run --rm kernelcamp-ubuntu

# 使用Arch Linux环境
docker-compose run --rm kernelcamp-arch

# 使用Fedora环境  
docker-compose run --rm kernelcamp-fedora
```

#### 构建项目
```bash
docker-compose run --rm builder
```

#### 运行应用
```bash
docker-compose run --rm runner
```

#### 生成 AppImage
```bash
docker-compose run --rm appimage
```

## 🔧 详细用法

### 多发行版构建参数

Dockerfile支持通过构建参数选择基础镜像：

```bash
# 构建特定发行版镜像
docker build --build-arg BASE_IMAGE=ubuntu:22.04 -t kernelcamp-ubuntu .
docker build --build-arg BASE_IMAGE=archlinux:latest -t kernelcamp-arch .
docker build --build-arg BASE_IMAGE=fedora:38 -t kernelcamp-fedora .

# 支持的发行版和版本：
# - Ubuntu: 20.04, 22.04, 24.04
# - Arch Linux: latest
# - Fedora: 38, 39, 40
# - Debian: bullseye, bookworm
```

### 交互式开发环境
```bash
docker-compose run --rm kernelcamp-ubuntu
# 在容器内执行:
make all          # 构建所有组件
make build-native # 仅构建原生库
make build-ui     # 仅构建UI应用
make run          # 运行应用
make appimage     # 生成AppImage
```

### 直接使用 Docker 命令

#### 构建并运行
```bash
docker run -it --rm -v $(pwd):/app -v $(pwd)/build:/app/build kernelcamp make all
```

#### 仅运行应用
```bash
docker run -it --rm -v $(pwd):/app -v $(pwd)/build:/app/build kernelcamp make run
```

## 📁 卷挂载说明

- `.:/app` - 挂载项目源码
- `./build:/app/build` - 挂载构建输出目录

## 🌟 环境特性

### 包含的依赖
- Ubuntu 22.04 LTS 基础系统
- .NET SDK 6.0
- Mono 开发工具链
- GTK# 3.0
- GCC 编译工具链
- Linux 构建工具
- AppImage 打包工具

### 环境变量
- `DOTNET_CLI_TELEMETRY_OPTOUT=1` - 禁用 .NET 遥测
- `DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1` - 跳过首次体验
- `LD_LIBRARY_PATH=/app/build/lib` - 库文件路径

## 🚀 工作流程

### 1. 开发阶段
```bash
# 启动开发容器
docker-compose run --rm kernelcamp

# 在容器内进行开发
make build-ui
make run
```

### 2. 构建阶段
```bash
# 一次性构建
docker-compose run --rm builder

# 或者手动构建
docker-compose run --rm kernelcamp make all
```

### 3. 测试阶段
```bash
# 运行测试
docker-compose run --rm runner
```

### 4. 发布阶段
```bash
# 生成 AppImage
docker-compose run --rm appimage
```

## 🔍 故障排除

### 常见问题

#### 权限问题
```bash
# 确保构建目录有写入权限
chmod 777 build
```

#### 库文件找不到
```bash
# 设置正确的库路径
export LD_LIBRARY_PATH=./build/lib:$LD_LIBRARY_PATH
```

#### Docker 资源不足
```bash
# 增加 Docker 资源分配
# 在 Docker Desktop 中调整 CPU 和内存限制
```

### 发行版特定问题

**Arch Linux**:
- 可能需要启用multilib仓库：`sed -i '/^#\[multilib\]/ {s/^#//;n;s/^#//}' /etc/pacman.conf && pacman -Syu`
- 如果遇到签名错误：`pacman -Syu --noconfirm archlinux-keyring`

**Fedora**:
- 可能需要启用RPM Fusion仓库
- 使用dnf而不是yum进行包管理

**Debian系**:
- 确保使用正确的Microsoft包仓库
- 可能需要安装ca-certificates

### 调试模式

#### 详细构建输出
```bash
docker-compose run --rm kernelcamp make build-native V=1
```

#### 进入容器调试
```bash
docker-compose run --rm kernelcamp bash
```

## 📊 性能优化

### 使用构建缓存
```bash
# 利用 Docker 层缓存加快构建
docker build --build-arg BUILDKIT_INLINE_CACHE=1 -t kernelcamp .
```

### 多阶段构建（未来支持）
```dockerfile
# 多阶段构建可以减少最终镜像大小
FROM kernelcamp AS builder
WORKDIR /app
COPY . .
RUN make all

FROM ubuntu:22.04 AS runtime
COPY --from=builder /app/build /app
CMD ["/app/bin/KernelCamp"]
```

## 🔄 持续集成

### GitHub Actions 示例
```yaml
name: Build KernelCamp
on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Build with Docker
      run: |
        docker build -t kernelcamp .
        docker run -v $(pwd)/build:/app/build kernelcamp make all
    
    - name: Upload artifacts
      uses: actions/upload-artifact@v3
      with:
        name: kernelcamp-build
        path: build/
```

### GitHub Actions多发行版构建

创建 `.github/workflows/build.yml`:

```yaml
name: Multi-distro Build

on: [push, pull_request]

jobs:
  build-ubuntu:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Build Ubuntu image
        run: docker build --build-arg BASE_IMAGE=ubuntu:22.04 -t kernelcamp-ubuntu .

  build-arch:
    runs-on: ubuntu-latest  
    steps:
      - uses: actions/checkout@v4
      - name: Build Arch Linux image
        run: docker build --build-arg BASE_IMAGE=archlinux:latest -t kernelcamp-arch .

  build-fedora:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - name: Build Fedora image
        run: docker build --build-arg BASE_IMAGE=fedora:38 -t kernelcamp-fedora .
```

## 📦 Git仓库上传指南

### 初始化Git仓库
```bash
# 初始化仓库
git init

# 添加所有文件
git add .

# 提交初始版本
git commit -m "feat: initial commit with multi-distro docker support"

# 添加远程仓库
git remote add origin https://github.com/yourusername/NeuroFromScratch.git

# 推送代码
git push -u origin main
```

### .gitignore配置
创建或更新 `.gitignore` 文件：

```
# 构建输出
/build/
/bin/
/obj/

# Docker相关
.docker/
*.tar.gz

# 开发环境
.vscode/
.idea/
*.swp
*.swo

# 系统文件
.DS_Store
Thumbs.db

# 日志文件
*.log

# 应用打包
*.AppImage
*.deb
*.rpm
```

### 分支策略建议
```bash
# 主分支 - 稳定版本
main

# 开发分支 - 功能开发  
develop

# 极速分支 - 按发行版或功能
feature/ubuntu-support
feature/arch-support  
feature/fedora-support
feature/docker-multi-distro

# 发布分支 - 版本发布
release/v1.0.0

# 修复分支 - bug修复
hotfix/critical-bug
```

### 提交信息规范
使用约定式提交（Conventional Commits）：

```
feat: 添加Arch Linux Docker支持
fix: 修复Ubuntu构建依赖问题
docs: 更新多发行版文档
ci: 配置GitHub Actions多发行版构建
test: 添加发行版兼容性测试
```

## 📝 最佳实践

1. **定期更新基础镜像**
   ```bash
   docker pull ubuntu:22.04
   ```

2. **清理无用镜像**
   ```bash
   docker system prune -f
   ```

3. **使用 .dockerignore**
   ```
   # 忽略不必要的文件
   build/
   .git/
   *.AppImage
   ```

4. **多架构支持**
   ```bash
   # 构建多平台镜像
   docker buildx build --platform linux/amd64,linux/arm64 -t kernelcamp .
   ```

## 🆘 支持

遇到问题？请查看：
- [Docker 文档](https://docs.docker.com/)
- [项目 Issues](https://github.com/your-username/KernelCamp/issues)
- [构建指南](BUILD.md)

---

*最后更新: $(date +%Y-%m-%d)*
*Docker 版本: $(docker --version)*