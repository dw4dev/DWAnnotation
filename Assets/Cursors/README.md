# 自訂游標使用說明

## 游標檔案位置

請將您的自訂游標檔案放置在以下位置：

```
e:\TEMP\Antigravity_Playground\Test1\Assets\Cursors\Target.cur
```

## 檔案要求

- **檔案名稱**: `Target.cur`
- **檔案格式**: Windows Cursor (.cur)
- **建議大小**: 32x32 或 64x64 像素

## 使用方式

1. 將 `Target.cur` 檔案複製到 `Assets\Cursors\` 資料夾
2. 重新建置專案：`dotnet build`
3. 執行應用程式：`dotnet run`
4. 選擇「軌跡擦除」工具，即可看到自訂游標

## 備註

- 如果游標檔案不存在或載入失敗，系統會自動使用 Hand 游標作為備用
- 游標檔案會自動複製到輸出目錄
- 支援透明背景的游標檔案
