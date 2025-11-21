# DW螢幕標註工具 (DW Annotation)

一個功能完整的 Windows 螢幕標註工具，使用 .NET 8、C# 12 和 WPF 開發。

## ✨ 功能特色

### 🎨 繪圖工具
- **🖊️ 手繪筆** - 自由手繪標註
- **📏 直線** - 繪製直線（按住 `Shift` 繪製水平/垂直線）
- **▭ 矩形** - 繪製矩形框（按住 `Shift` 繪製正方形）
- **⭕ 圓形** - 繪製圓形/橢圓（按住 `Shift` 繪製正圓）
- **🧹 橡皮擦** - 移除筆跡或圖形（支援點擦除與物件擦除）

### 🌈 顏色與筆刷
- **雙色系統** - 主色和副色快速切換
- **漸層筆刷** - 支援線性漸層效果
- **筆寬調整** - 可調整筆寬（1-40 像素）

### ✨ 特殊模式
- **固定模式**（預設）- 畫跡保持直到手動清除
- **魔術筆模式** - 畫跡自動淡出消失，適合臨時標註
  - 可自訂淡出時間（200-3000 毫秒）
  - 支援漸層筆刷的平滑淡出

### 🖥️ 視窗管理
- **浮動工具列** - 可拖曳、置頂、Fluent/Win11 風格
- **全螢幕透明標註** - 覆蓋所有螢幕，支援多螢幕
- **系統匣支援** - 最小化到系統匣常駐

### 📸 截圖與匯出
- **截圖功能** - 支援全螢幕截圖
- **工具列選項** - 可設定截圖時是否包含浮動工具列
- **匯出格式** - 儲存為 PNG（保留透明背景）或複製到剪貼簿

## ⌨️ 快捷鍵

| 按鍵 | 功能 |
|------|------|
| `Esc` | 退出標註模式 / 關閉視窗 |
| `Ctrl+Z` / `Ctrl+D` | 復原上一步 |
| `Ctrl+S` | 儲存截圖為 PNG |
| `Ctrl+C` | 複製截圖到剪貼簿 |
| `Ctrl+G` | 切換漸層模式 |
| `Ctrl+M` | 切換魔術筆模式 |
| `Shift` | 繪圖約束（正圓、正方形、水平/垂直線） |

## 🛠️ 技術規格

- **語言**: C# 12
- **平台**: .NET 8
- **UI 框架**: WPF
- **架構**: MVVM（使用 CommunityToolkit.Mvvm）
- **設定儲存**: JSON（使用 System.Text.Json Source Generator）

### C# 12 / .NET 8 特性應用
- ✅ Primary Constructors
- ✅ Collection Expressions
- ✅ Required Properties
- ✅ File-scoped types
- ✅ Nullable Reference Types
- ✅ Implicit Usings
- ✅ Task-based async/await

## 🚀 建置與執行

### 需求
- .NET 8 SDK
- Windows 10/11

### 指令
```powershell
# 還原套件
dotnet restore

# 建置
dotnet build

# 執行
dotnet run

# 發佈（獨立執行檔）
dotnet publish -c Release -r win-x64 --self-contained
```

## 📖 使用說明

1. **啟動程式** - 顯示浮動工具列
2. **點擊 ✏️ 按鈕** - 啟動標註模式（開啟全螢幕透明視窗）
3. **選擇工具** - 筆、直線、矩形、圓形或橡皮擦
4. **選擇顏色** - 點擊主色/副色按鈕
5. **開始繪製** - 在螢幕上任意位置繪製
   - 按住 `Shift` 可啟用形狀約束
6. **截圖保存** - 按 `Ctrl+S` 存檔或 `Ctrl+C` 複製
7. **按 Esc** - 退出標註模式

### 系統匣
- 關閉工具列視窗會最小化到系統匣
- 雙擊系統匣圖示重新開啟
- 右鍵選單提供快速操作

## 📂 設定檔
設定檔自動儲存在：`%AppData%\FloatingAnnotationTool\settings.json`

## 📄 授權
MIT License

## 👨‍💻 作者
Davidosn 使用 Antigravity AI 開發
