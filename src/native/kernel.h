#ifndef KERNEL_H
#define KERNEL_H

#ifdef __cplusplus
extern "C" {
#endif

// 获取内核版本信息
char* get_kernel_version();

// 获取内核配置路径
char* get_kernel_config_path();

// 读取内核配置选项
// options: 输出参数，存储配置选项数组
// count: 输出参数，存储选项数量
// 返回: 0成功，-1失败
int read_kernel_config(const char* config_path, char*** options, int* count);

// 修改内核配置选项
// 返回: 0成功，-1失败
int modify_kernel_config(const char* config_path, const char* option_name, const char* new_value);

// 获取可用内核参数选项
// option_name: 参数名称
// count: 输出参数，存储选项数量
// 返回: 选项值数组
char** get_available_options(const char* option_name, int* count);

// 释放字符串数组内存
void free_string_array(char** array, int count);

#ifdef __cplusplus
}
#endif

#endif // KERNEL_H