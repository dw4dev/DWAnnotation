# 檔案路徑：docs/Handoff.md

# DWAnnotation 階段交接文檔 (Handoff Document)

> 📌 **規格文件版本**：v0.2（需求確認完畢，2026-05-28）

> 💡 **AI 閱讀須知**：本檔為**當前活躍階段**的滾動狀態。完成一個階段後，將本檔精華彙整至 `DWAnnotation_開發沿革.md`，再**清空**本檔下方 §1~§6 欄位以接收下一階段。本檔永遠只描述「現在進行中」的事，歷史請查沿革。  
> AI 思考過程都用英文即可，最後簡單輸出繁體中文總結就可以了，節省 token。

---

## 0. 開發與操作鐵則 (Development & Operation Rules)

* **同步更新**：送出最後 commit 前先更新本檔。
* **Commit 規範**：每完成一段邏輯即 commit；commit 訊息**不加** `Co-Authored-By`；階段完成時 commit 帶 `[Phase N Done]` 標記。
* **權限限制**：本機可逆（編輯 / build / test）直接做；不可逆（push / PR / 改 CI / 刪 branch）先問。
* **效能節約**：探索用 Grep / Glob，不要整檔讀大檔。
* **單一 HANDOFF**：禁止建立 `HANDOFF_Phase1.md` 等分檔；所有階段交接共用本檔。
* **沿革彙整時機**：當 §4 DoD 全數通過、§1 階段狀態標記為「完成待交接」時，立即彙整至 `DWAnnotation_開發沿革.md` 並清空本檔。
* **規格文件**：完整升級規格見 `docs/DWAnnotation_v1.2_系統升級規劃.md`，每次開工前先對照該文件。

### 0.1 專案架構快覽

```
DWAnnotation/
├── App.xaml / App.xaml.cs          # 應用程式入口、系統匣、設定載入
├── Models/
│   ├── AppSettings.cs              # 所有設定欄位（JSON 序列化）
│   ├── DrawingTool.cs              # DrawingTool enum
│   └── GradientStroke.cs           # 自訂漸層筆觸
├── ViewModels/
│   ├── MainToolbarViewModel.cs     # 工具列狀態、Command
│   ├── OverlayViewModel.cs         # 覆蓋層 Undo 堆疊、筆刷
│   └── SettingsViewModel.cs        # 設定視窗 ViewModel
├── Views/
│   ├── MainToolbarWindow.xaml(.cs) # 浮動工具列
│   ├── OverlayWindow.xaml(.cs)     # 全螢幕透明標註覆蓋層
│   └── SettingsWindow.xaml(.cs)    # 設定視窗
├── Services/
│   └── SettingsService.cs          # 設定讀寫（JSON）
└── docs/
    ├── Handoff.md                  # ← 本檔
    └── DWAnnotation_v1.2_系統升級規劃.md
```

### 0.2 易踩坑注意事項

- **WPF `<Run Text="{Binding X}"/>` 預設 TwoWay**：綁 get-only 屬性會拋 `XamlParseException`，整個 Window 開不起來。所有 `<Run>` binding 一律加 `Mode=OneWay`。
- **OverlayWindow 的 WS_EX_TRANSPARENT**：View Mode 下透過 Win32 SetWindowLong 加入 `WS_EX_TRANSPARENT`，讓滑鼠穿透；切換模式務必同步更新，否則輸入被吃掉。
- **Owner 關係**：`MainToolbarWindow.Owner = OverlayWindow`，關閉順序必須注意，否則 Owner 關掉 Overlay 也跟著消失。
- **多螢幕座標**：覆蓋層用 `SystemParameters.VirtualScreen*`，截圖也要用同一組值；不可混用 `WorkArea` 或單一螢幕尺寸。
- **HBitmap 記憶體洩漏**：`screenBitmap.GetHbitmap()` 取得的 HBITMAP 需呼叫 `DeleteObject` 釋放，否則長期截圖會洩漏 GDI 物件。
- **ResourceDictionary 主題切換**：切換 `MergedDictionaries` 前先 `Remove` 舊主題再 `Add` 新主題；不要直接 `Clear()` 全部，會清掉其他資源。
- **WriteableBitmap 像素操作**：撕邊效果需 `Lock()` → 操作 `BackBuffer` → `AddDirtyRect` → `Unlock()`，不可省略。

---

## 1. 當前開發階段 (Current Status)

* **當前所屬階段**：Phase 1 — UI 基礎重構（主題系統）
* **狀態**：規劃中
* **當前 Git Commit（短碼）**：（待填入）
* **程式版本**：v1.1.x（version.json）
* **對應規格章節**：§3 UI 重構、§2.2 漸層按鈕圖示
* **啟動日期**：2026-05-28
* **預計完成日期**：待評估

---

## 2. 已完成功能 (Completed Deliverables)

> *Phase 1 尚未開始，以下為 v1.1 既有功能，供參考。*

* [x] 全螢幕透明標註覆蓋層（多螢幕支援）
* [x] 工具列浮動視窗（可拖曳）
* [x] 繪圖工具：Pen / Line / Rectangle / Ellipse / EraserPoint / EraserObject
* [x] 漸層筆觸（GradientStroke）
* [x] 魔術筆（筆觸自動淡出）
* [x] Undo / Clear
* [x] 截圖（全螢幕合併標註）儲存 + 複製剪貼簿
* [x] 設定視窗（筆寬、橡皮擦大小、淡出時間、顏色）
* [x] 系統匣支援

---

## 3. 當前資料庫變更 (Database Changes)

* [x] 無資料庫（純設定 JSON）

設定檔路徑：`%AppData%\DWAnnotation\settings.json`（由 `SettingsService` 管理）

---

## 4. DoD 驗收紀錄 (Definition of Done)

### Phase 1 — UI 基礎重構

| 驗收條件 | 通過 | 證據 |
|:--------|:---:|:-----|
| LightTheme.xaml 定義完整語義色票（≥8 個語義 Key） | | |
| DarkTheme.xaml 定義完整語義色票，對比度 WCAG AA 達標 | | |
| App.xaml 可正確切換主題（無殘留舊色） | | |
| MainToolbarWindow 套用主題色票（無 hardcode 顏色） | | |
| 漸層按鈕改為 Path/幾何圖示（非調色盤 emoji） | | |
| Build 無 warning，執行無例外 | | |

---

## 5. 已知風險 / 待辦 (Risks & TODO)

* [x] **需求確認**：4 項疑問已於 2026-05-28 定案（詳見規劃書 §二「需求確認紀錄」）
* [ ] **§1.5 長截圖-瀏覽器**：Phase 5b，若整合 CDP/Playwright 過於複雜則移至 v1.3
* [ ] **§1.6.5 指定視窗/物件**：使用 `UI Automation` 掃描元素 + Hover 高亮紅框，Phase 5 實作
* [ ] **HBitmap GDI 洩漏**：`CaptureScreenWithAnnotations` 的 `GetHbitmap()` 未呼叫 `DeleteObject`，Phase 4 截圖服務重構時一併修正
* [ ] **設定視窗繫結方式**：現有 `SettingsWindow` 部分欄位用 code-behind 手動同步，Phase 2 重構改為純 MVVM

---

## 6. 下階段啟動指南 (Next Phase Kickoff)

### Phase 1 啟動清單

* **前置條件**：
  * [x] 規格文件 `DWAnnotation_v1.2_系統升級規劃.md` 已建立（v0.2，需求確認完畢）
  * [x] 使用者已確認整體規劃方向與 4 項疑問
  * [x] 本階段規劃已 commit（`docs: add v1.2 upgrade plan and update Handoff`）

* **下一個 Session 從這裡開始** ▶
  1. 閱讀本檔 §0 鐵則 + §0.1 架構快覽
  2. 閱讀規劃書 `Phase 1` 章節確認任務清單
  3. 到 https://www.ysdaima.com/palettes/ui-chart-category/ 參考色票，決定 Light/Dark 主色調
  4. 建立 `Themes/` 目錄，新增 `LightTheme.xaml`，定義以下語義 Key：
     ```
     AppBackground, AppSurface, AppSurfaceHover,
     AppPrimary, AppOnPrimary, AppPrimaryHover,
     AppSecondary, AppOnSecondary,
     AppOutline, AppText, AppTextMuted, AppTextDisabled
     ```
  5. 建立 `Themes/DarkTheme.xaml`（同一套 Key，暗黑配色，對比度 WCAG AA）
  6. 新增 `Services/ThemeService.cs`，提供 `Apply(themeName)` 靜態方法切換 MergedDictionaries
  7. `AppSettings` 新增 `ThemeName = "Light"`
  8. `App.xaml.cs` 啟動時呼叫 `ThemeService.Apply(settings.ThemeName)`
  9. `MainToolbarWindow.xaml` 全面替換 hardcode 顏色 → `{DynamicResource AppXxx}`
  10. `SettingsWindow.xaml` 同步替換（下一 Phase 會完整重構，此處先套色票）
  11. §2.2：更換漸層按鈕圖示，使用 `Path`+`LinearGradientBrush` 幾何（非調色盤 emoji）
  12. Build 驗收、執行驗收，確認無例外與色彩顯示正確
  13. 完成後更新本 §4 DoD 表格，commit `[Phase 1 Done]`

* **對應規格**：`§3 UI 重構`、`§2.2`
* **完成後**：彙整至 `DWAnnotation_開發沿革.md`，更新 §1 狀態為「完成待交接」，清空 §1~§6 進入 Phase 2

---

## 7. 各階段進度總覽 (All Phases)

| 階段 | 名稱 | 狀態 | 規格章節 |
|:----:|:-----|:----:|:--------|
| Phase 1 | UI 基礎重構（主題系統） | 🔵 規劃中 | §3, §2.2 |
| Phase 2 | 設定視窗重構 + 工具列優化 | ⚪ 待啟動 | §4, §2.1 |
| Phase 3 | 螢幕指示功能（雷射筆/聚光燈/放大鏡） | ⚪ 待啟動 | §5 |
| Phase 4 | 截圖編輯器（核心） | ⚪ 待啟動 | §1.1~1.4 |
| Phase 5 | 截圖形狀擷取 + 長截圖 | ⚪ 待啟動 | §1.5, §1.6 |
