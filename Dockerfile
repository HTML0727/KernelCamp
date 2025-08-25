# KernelCamp Docker 构建环境
# 基于Ubuntu 22.04 LTS，包含所有开发依赖

ARG BASE_IMAGE=ubuntu:22.04
FROM ${BASE_IMAGE} AS base

# 设置环境变量
ENV DEBIAN_FRONTEND=noninteractive \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1 \
    DOTNET_ROOT=/usr/share/dotnet \
    PATH=$PATH:/usr/share/dotnet

# 多发行版包管理器设置
RUN if [ -f /etc/debian_version ]; then \
        apt-get update && apt-get install -y \
        curl \
        wget \
        git \
        gnupg \
        ca-certificates \
        software-properties-common \
        build-essential \
        pkg-config \
        libgtk-3-dev \
        libglib2.0-dev \
        libc6-dev \
        && rm -rf /var/lib/apt/lists/*; \
    elif [ -f /etc/arch-release ]; then \
        pacman -Syu --noconfirm && pacman -S --noconfirm \
        curl \
        wget \
        git \
        base-devel \
        pkg-config \
        gtk3 \
        glib2 \
        glibc; \
    elif [ -f /etc/fedora-release ]; then \
        dnf update -y && dnf install -y \
        curl \
        wget \
        git \
        @development-tools \
        pkg-config \
        gtk3-devel \
        glib2-devel \
        glibc-devel; \
    fi

# 安装.NET SDK 6.0（多发行版支持）
RUN if [ -f /etc/debian_version ]; then \
        wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
        && dpkg -i packages-microsoft-prod.deb \
        && rm packages-microsoft-prod.deb \
        && apt-get update \
        && apt-get install -y dotnet-sdk-6.0; \
    elif [ -f /etc/fedora-release ]; then \
        wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
        && chmod +x dotnet-install.sh \
        && ./dotnet-install.sh --version 6.0.400 --install-dir /usr/share/dotnet \
        && rm dotnet-install.sh; \
    elif [ -f /etc/arch-release ]; then \
        pacman -S --noconfirm dotnet-sdk-6.0; \
    fi

# 安装构建工具和依赖（多发行版支持）
RUN if [ -f /etc/debian_version ]; then \
        apt-get update && apt-get install -y \
        build-essential \
        make \
        gcc \
        g++ \
        mono-devel \
        mono-gmcs \
        referenceassemblies-pcl \
        ca-certificates-mono \
        gtk-sharp3 \
        libgtk3.0-cil-dev \
        libappimage-dev \
        linuxdeploy \
        linuxdeploy-plugin-gtk \
        && rm -rf /var/lib/apt/lists/*; \
    elif [ -f /etc/arch-release ]; then \
        pacman -S --noconfirm \
        make \
        gcc \
        mono \
        mono-msbuild \
        gtk-sharp-3; \
    elif [ -f /etc/fedora-release ]; then \
        dnf install -y \
        make \
        gcc \
        gcc-c++ \
        mono-devel \
        mono-msbuild \
        gtk3; \
    fi

# 创建工作目录
WORKDIR /app

# 复制项目文件
COPY . .

# 设置构建脚本权限
RUN chmod +x Makefile

# 默认构建命令
CMD ["make", "all"]