#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <sys/utsname.h>
#include <dirent.h>

// 获取内核版本信息
char* get_kernel_version() {
    static struct utsname buf;
    if (uname(&buf) == 0) {
        return strdup(buf.release);
    }
    return NULL;
}

// 获取内核配置路径
char* get_kernel_config_path() {
    char* paths[] = {
        "/proc/config.gz",
        "/boot/config-",
        "/usr/src/linux/.config",
        NULL
    };
    
    static struct utsname buf;
    if (uname(&buf) == 0) {
        char dynamic_path[256];
        
        // 检查 /boot/config-$(uname -r)
        snprintf(dynamic_path, sizeof(dynamic_path), "/boot/config-%s", buf.release);
        if (access(dynamic_path, R_OK) == 0) {
            return strdup(dynamic_path);
        }
        
        // 检查 /proc/config.gz
        if (access("/proc/config.gz", R_OK) == 0) {
            return strdup("/proc/config.gz");
        }
    }
    
    return NULL;
}

// 读取内核配置选项
int read_kernel_config(const char* config_path, char*** options, int* count) {
    FILE* file;
    char line[1024];
    int capacity = 100;
    int index = 0;
    
    if (strstr(config_path, ".gz")) {
        // 处理gzip压缩的配置
        char command[512];
        snprintf(command, sizeof(command), "zcat %s", config_path);
        file = popen(command, "r");
        if (!file) return -1;
    } else {
        file = fopen(config_path, "r");
        if (!file) return -1;
    }
    
    *options = malloc(capacity * sizeof(char*));
    if (!*options) {
        if (strstr(config_path, ".gz")) {
            pclose(file);
        } else {
            fclose(file);
        }
        return -1;
    }
    
    while (fgets(line, sizeof(line), file)) {
        // 跳过注释和空行
        if (line[0] == '#' || line[0] == '\n' || line[0] == '\r') {
            continue;
        }
        
        // 移除换行符
        line[strcspn(line, "\n\r")] = '\0';
        
        if (index >= capacity) {
            capacity *= 2;
            *options = realloc(*options, capacity * sizeof(char*));
            if (!*options) {
                if (strstr(config_path, ".gz")) {
                    pclose(file);
                } else {
                    fclose(file);
                }
                return -1;
            }
        }
        
        (*options)[index] = strdup(line);
        index++;
    }
    
    if (strstr(config_path, ".gz")) {
        pclose(file);
    } else {
        fclose(file);
    }
    
    *count = index;
    return 0;
}

// 修改内核配置选项
int modify_kernel_config(const char* config_path, const char* option_name, const char* new_value) {
    // 这里实现配置修改逻辑
    // 注意：实际修改内核配置需要root权限和谨慎操作
    printf("Would modify %s to %s=%s\n", config_path, option_name, new_value);
    return 0;
}

// 获取可用内核参数选项（模拟数据）
char** get_available_options(const char* option_name, int* count) {
    // 这里返回特定参数的可选值
    static char* bool_options[] = {"y", "n", "m"};
    static char* debug_options[] = {"0", "1", "2", "3"};
    
    if (strstr(option_name, "DEBUG") || strstr(option_name, "debug")) {
        *count = 4;
        char** result = malloc(4 * sizeof(char*));
        for (int i = 0; i < 4; i++) {
            result[i] = strdup(debug_options[i]);
        }
        return result;
    }
    
    // 默认返回布尔选项
    *count = 3;
    char** result = malloc(3 * sizeof(char*));
    for (int i = 0; i < 3; i++) {
        result[i] = strdup(bool_options[i]);
    }
    return result;
}

// 释放字符串数组内存
void free_string_array(char** array, int count) {
    for (int i = 0; i < count; i++) {
        free(array[i]);
    }
    free(array);
}
