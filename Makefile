# KernelCamp Makefile

.PHONY: all build-native build-ui clean appimage

# 目录定义
SRC_DIR = src
NATIVE_DIR = $(SRC_DIR)/native
UI_DIR = $(SRC_DIR)/ui
BUILD_DIR = build
LIB_DIR = $(BUILD_DIR)/lib
BIN_DIR = $(BUILD_DIR)/bin

# 编译器设置
CC = gcc
CFLAGS = -Wall -fPIC -shared
LDFLAGS = -lm

# .NET设置
DOTNET = dotnet

# 目标文件
LIBRARY = $(LIB_DIR)/libkernel.so
UI_APP = $(BIN_DIR)/KernelCamp

all: build-native build-ui

# 构建原生C库
build-native: $(LIBRARY)

$(LIBRARY): $(NATIVE_DIR)/kernel.c $(NATIVE_DIR)/kernel.h
	@mkdir -p $(LIB_DIR)
	$(CC) $(CFLAGS) -o $@ $< $(LDFLAGS)
	@echo "Native library built: $@"

# 构建C# UI应用
build-ui: $(LIBRARY)
	@mkdir -p $(BIN_DIR)
	# 复制原生库到UI输出目录
	cp $(LIBRARY) $(UI_DIR)/
	# 构建C#项目
	cd $(UI_DIR) && $(DOTNET) build -c Release -o ../$(BIN_DIR)
	@echo "UI application built"

# 清理构建文件
clean:
	rm -rf $(BUILD_DIR)
	rm -f $(UI_DIR)/libkernel.so
	cd $(UI_DIR) && $(DOTNET) clean
	@echo "Clean completed"

# 生成AppImage (需要linuxdeploy和linuxdeploy-plugin-gtk)
appimage: build-ui
	@echo "AppImage packaging requires linuxdeploy tools on Linux"
	@echo "Please run this target on a Linux system"
	# linuxdeploy --appdir AppDir --executable $(BIN_DIR)/NeuroFromScratch \
	#   --library $(LIBRARY) --output appimage

# 运行应用
run: build-ui
	cd $(BIN_DIR) && ./KernelCamp

# 安装依赖 (Ubuntu/Debian)
install-deps-ubuntu:
	sudo apt-get update
	sudo apt-get install -y \
		build-essential \
		mono-devel \
		gtk-sharp3 \
		libgtk3.0-cil-dev \
		dotnet-sdk-6.0 \
		libappimage-dev

# 安装依赖 (Fedora)
install-deps-fedora:
	sudo dnf install -y \
		gcc \
		make \
		mono-devel \
		gtk-sharp3 \
		dotnet-sdk-6.0 \
		libappimage

help:
	@echo "KernelCamp Build System"
	@echo "============================="
	@echo "make all          - Build everything (native + UI)"
	@echo "make build-native - Build only the native C library"
	@echo "make build-ui     - Build only the C# UI application"
	@echo "make clean        - Clean all build artifacts"
	@echo "make run          - Build and run the application"
	@echo "make appimage     - Build AppImage (Linux only)"
	@echo "make install-deps-ubuntu - Install dependencies on Ubuntu"
	@echo "make install-deps-fedora - Install dependencies on Fedora"