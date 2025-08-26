#include <stdio.h>
#include <stdlib.h>
#include "kernel.h"

int main() {
    printf("Testing NeuroFromScratch Native Library\n");
    printf("======================================\n");
    
    // 测试获取内核版本
    char* version = get_kernel_version();
    if (version) {
        printf("Kernel Version: %s\n", version);
        free(version);
    } else {
        printf("Failed to get kernel version\n");
    }
    
    // 测试获取配置路径
    char* config_path = get_kernel_config_path();
    if (config_path) {
        printf("Config Path: %s\n", config_path);
        free(config_path);
    } else {
        printf("Failed to get config path\n");
    }
    
    printf("======================================\n");
    printf("Test completed.\n");
    printf("Nekosparry 2025 | All rights reserved\n");
    
    
    return 0;
}