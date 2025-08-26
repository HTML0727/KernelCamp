using System;
using Gtk;
using System.Runtime.InteropServices;

namespace KernelCamp
{
    public class MainWindow : Window
    {
        // P/Invoke声明用于调用C库
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_kernel_version();
        
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_kernel_config_path();
        
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern int read_kernel_config(string config_path, out IntPtr options, out int count);
        
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern int modify_kernel_config(string config_path, string option_name, string new_value);
        
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr get_available_options(string option_name, out int count);
        
        [DllImport("libkernel.so", CallingConvention = CallingConvention.Cdecl)]
        private static extern void free_string_array(IntPtr array, int count);
        
        private TreeView configTreeView;
        private ListStore configStore;
        private Button applyButton;
        private Label kernelVersionLabel;
        private Label configPathLabel;
        
        public MainWindow() : base("KernelCamp - Linux内核配置工具")
        {
            SetDefaultSize(1000, 700);
            SetPosition(WindowPosition.Center);
            
            // 创建主布局
            var mainBox = new Box(Orientation.Vertical, 5);
            
            // 创建头部信息栏
            var headerBox = CreateHeaderBox();
            mainBox.PackStart(headerBox, false, false, 5);
            
            // 创建配置表格
            var configFrame = CreateConfigFrame();
            mainBox.PackStart(configFrame, true, true, 5);
            
            // 创建底部按钮栏
            var buttonBox = CreateButtonBox();
            mainBox.PackStart(buttonBox, false, false, 5);
            
            Add(mainBox);
            
            // 加载内核信息
            LoadKernelInfo();
            
            DeleteEvent += OnDeleteEvent;
        }
        
        private Box CreateHeaderBox()
        {
            var headerBox = new Box(Orientation.Horizontal, 10);
            
            var icon = new Image(Stock.Info, IconSize.Dialog);
            headerBox.PackStart(icon, false, false, 5);
            
            var infoBox = new Box(Orientation.Vertical, 2);
            
            kernelVersionLabel = new Label("内核版本: 加载中...");
            configPathLabel = new Label("配置文件: 加载中...");
            
            infoBox.PackStart(kernelVersionLabel, false, false, 0);
            infoBox.PackStart(configPathLabel, false, false, 0);
            
            headerBox.PackStart(infoBox, true, true, 0);
            
            return headerBox;
        }
        
        private Frame CreateConfigFrame()
        {
            var frame = new Frame("内核配置选项");
            
            // 创建树形视图和存储
            configStore = new ListStore(typeof(string), typeof(string), typeof(string));
            configTreeView = new TreeView(configStore);
            
            // 添加列
            var optionColumn = new TreeViewColumn { Title = "配置选项" };
            var valueColumn = new TreeViewColumn { Title = "当前值" };
            var actionColumn = new TreeViewColumn { Title = "操作" };
            
            // 选项列
            var optionCell = new CellRendererText();
            optionColumn.PackStart(optionCell, true);
            optionColumn.AddAttribute(optionCell, "text", 0);
            
            // 值列
            var valueCell = new CellRendererText();
            valueColumn.PackStart(valueCell, true);
            valueColumn.AddAttribute(valueCell, "text", 1);
            
            // 操作列（使用CellRendererCombo）
            var comboCell = new CellRendererCombo();
            comboCell.Editable = true;
            comboCell.Edited += OnComboEdited;
            actionColumn.PackStart(comboCell, true);
            actionColumn.AddAttribute(comboCell, "text", 2);
            
            configTreeView.AppendColumn(optionColumn);
            configTreeView.AppendColumn(valueColumn);
            configTreeView.AppendColumn(actionColumn);
            
            // 添加滚动窗口
            var scrolledWindow = new ScrolledWindow();
            scrolledWindow.Add(configTreeView);
            
            frame.Add(scrolledWindow);
            return frame;
        }
        
        private Box CreateButtonBox()
        {
            var buttonBox = new Box(Orientation.Horizontal, 5);
            buttonBox.Homogeneous = true;
            
            var refreshButton = new Button("刷新");
            refreshButton.Clicked += OnRefreshClicked;
            
            applyButton = new Button("应用更改");
            applyButton.Sensitive = false; // 初始禁用
            applyButton.Clicked += OnApplyClicked;
            
            var quitButton = new Button("退出");
            quitButton.Clicked += OnQuitClicked;
            
            buttonBox.PackStart(refreshButton, true, true, 0);
            buttonBox.PackStart(applyButton, true, true, 0);
            buttonBox.PackStart(quitButton, true, true, 0);
            
            return buttonBox;
        }
        
        private void LoadKernelInfo()
        {
            try
            {
                // 获取内核版本
                IntPtr versionPtr = get_kernel_version();
                string version = Marshal.PtrToStringAnsi(versionPtr);
                kernelVersionLabel.Text = $"内核版本: {version}";
                
                // 获取配置路径
                IntPtr configPathPtr = get_kernel_config_path();
                string configPath = Marshal.PtrToStringAnsi(configPathPtr);
                configPathLabel.Text = $"配置文件: {configPath}";
                
                // 加载配置选项
                LoadConfigOptions(configPath);
            }
            catch (Exception ex)
            {
                ShowError($"加载内核信息时出错: {ex.Message}");
            }
        }
        
        private void LoadConfigOptions(string configPath)
        {
            try
            {
                configStore.Clear();
                
                IntPtr optionsPtr;
                int count;
                
                if (read_kernel_config(configPath, out optionsPtr, out count) == 0 && count > 0)
                {
                    // 将指针转换为字符串数组
                    IntPtr[] optionPointers = new IntPtr[count];
                    Marshal.Copy(optionsPtr, optionPointers, 0, count);
                    
                    for (int i = 0; i < count; i++)
                    {
                        string optionLine = Marshal.PtrToStringAnsi(optionPointers[i]);
                        
                        // 解析配置行 (CONFIG_OPTION=value)
                        var parts = optionLine.Split('=');
                        if (parts.Length == 2)
                        {
                            string optionName = parts[0].Trim();
                            string currentValue = parts[1].Trim();
                            
                            // 获取可用选项
                            int optionCount;
                            IntPtr availableOptionsPtr = get_available_options(optionName, out optionCount);
                            
                            if (availableOptionsPtr != IntPtr.Zero && optionCount > 0)
                            {
                                IntPtr[] optionValuePointers = new IntPtr[optionCount];
                                Marshal.Copy(availableOptionsPtr, optionValuePointers, 0, optionCount);
                                
                                // 构建下拉选项字符串
                                string comboValues = string.Join(",", Array.ConvertAll(
                                    optionValuePointers, p => Marshal.PtrToStringAnsi(p)));
                                
                                configStore.AppendValues(optionName, currentValue, comboValues);
                                
                                free_string_array(availableOptionsPtr, optionCount);
                            }
                            else
                            {
                                configStore.AppendValues(optionName, currentValue, currentValue);
                            }
                        }
                    }
                    
                    free_string_array(optionsPtr, count);
                }
            }
            catch (Exception ex)
            {
                ShowError($"加载配置选项时出错: {ex.Message}");
            }
        }
        
        private void OnComboEdited(object o, EditedArgs args)
        {
            // 启用应用按钮
            applyButton.Sensitive = true;
        }
        
        private void OnRefreshClicked(object sender, EventArgs args)
        {
            LoadKernelInfo();
            applyButton.Sensitive = false;
        }
        
        private void OnApplyClicked(object sender, EventArgs args)
        {
            // 实现应用更改逻辑
            ShowInfo("更改已应用（模拟）");
            applyButton.Sensitive = false;
        }
        
        private void OnQuitClicked(object sender, EventArgs args)
        {
            Application.Quit();
        }
        
        private void OnDeleteEvent(object sender, DeleteEventArgs args)
        {
            Application.Quit();
        }
        
        private void ShowError(string message)
        {
            MessageDialog dialog = new MessageDialog(this, 
                DialogFlags.Modal, 
                MessageType.Error, 
                ButtonsType.Ok, 
                message);
            dialog.Run();
            dialog.Destroy();
        }
        
        private void ShowInfo(string message)
        {
            MessageDialog dialog = new MessageDialog(this, 
                DialogFlags.Modal, 
                MessageType.Info, 
                ButtonsType.Ok, 
                message);
            dialog.Run();
            dialog.Destroy();
        }
    }
}