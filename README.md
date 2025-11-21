# 浮動標註工具 (Floating Annotation Tool)

一個功能完整的 Windows 螢幕標註工具，使用 .NET 8、C# 12 和 WPF 開發。

## 功能特色

### 🎨 繪圖工具
- **手繪筆** - 自由手繪標註
- **直線** - 繪製直線
- **矩形** - 繪製矩形框
- **橡皮擦** - 移除筆跡或圖形

### 🌈 顏色與筆刷
- 主色和副色選擇
- 漸層筆刷支援（線性漸層）
- 可調整筆寬（1-40 像素）

### ✨ 特殊模式
- **固定模式**（預設）- 畫跡保持直到手動清除
- **魔術筆模式** - 畫跡自動淡出消失
  - 可自訂淡出時間（200-3000 毫秒）
  - 平滑的透明度動畫

### 🖥️ 視窗管理
- **浮動工具列** - 可拖曳、置頂、Fluent/Win11 風格
- **全螢幕透明標註** - 覆蓋所有螢幕
- **系統匣支援** - 最小化到系統匣常駐

### ⌨️ 快捷鍵
- `Esc` - 關閉標註視窗
- `Ctrl+Z` / `Ctrl+D` - 復原
- `Ctrl+S` - 儲存為 PNG
- `Ctrl+C` - 複製到剪貼簿
- `Ctrl+G` - 切換漸層模式
- `Ctrl+M` - 切換魔術筆模式

### 💾 匯出功能
- 儲存為 PNG（保留透明背景）
- 複製到剪貼簿

## 技術規格

- **語言**: C# 12
- **平台**: .NET 8
- **UI 框架**: WPF
- **架構**: MVVM（使用 CommunityToolkit.Mvvm）
- **設定儲存**: JSON（使用 System.Text.Json Source Generator）

### C# 12 / .NET 8 特性
- ✅ Primary Constructors
- ✅ Collection Expressions
- ✅ Required Properties
- ✅ File-scoped types
- ✅ Nullable Reference Types
- ✅ Implicit Usings
- ✅ Task-based async/await

## 專案結構

```
FloatingAnnotationTool/
├── Models/
│   ├── AppSettings.cs          # 應用程式設定模型（含 JSON Source Generator）
│   └── DrawingTool.cs          # 繪圖工具列舉
├── Services/
│   └── SettingsService.cs      # 設定持久化服務
├── ViewModels/
│   ├── MainToolbarViewModel.cs # 工具列 ViewModel
│   ├── OverlayViewModel.cs     # 標註視窗 ViewModel
│   └── SettingsViewModel.cs    # 設定視窗 ViewModel
├── Views/
│   ├── MainToolbarWindow.xaml  # 浮動工具列視窗
│   ├── OverlayWindow.xaml      # 全螢幕標註視窗
│   └── SettingsWindow.xaml     # 設定視窗
├── App.xaml                    # 應用程式入口
└── App.xaml.cs                 # 系統匣整合
```

## 建置與執行

### 需求
- .NET 8 SDK
- Windows 10/11

### 建置
```powershell
dotnet restore
dotnet build
```

### 執行
```powershell
dotnet run
```

### 發佈
```powershell
dotnet publish -c Release -r win-x64 --self-contained
```

## 設定檔位置

設定檔自動儲存在：
```
%AppData%\FloatingAnnotationTool\settings.json
```

## 使用說明

1. **啟動程式** - 顯示浮動工具列
2. **點擊 ✏️ 按鈕** - 啟動標註模式（開啟全螢幕透明視窗）
3. **選擇工具** - 筆、直線、矩形或橡皮擦
4. **選擇顏色** - 點擊主色/副色按鈕
5. **開始繪製** - 在螢幕上任意位置繪製
6. **按 Esc** - 關閉標註視窗
7. **點擊 ⚙️** - 開啟設定調整參數

### 魔術筆模式
啟用後，所有新繪製的內容會在指定時間後自動淡出消失，適合臨時標註。

### 系統匣
- 關閉工具列視窗會最小化到系統匣
- 雙擊系統匣圖示重新開啟
- 右鍵選單提供快速操作

## 授權

MIT License

## 作者

使用 Antigravity AI 開發
