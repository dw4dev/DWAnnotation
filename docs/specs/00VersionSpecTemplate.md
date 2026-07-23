# VersionSpecTemplate.md

# Project Version Specification

## Project Information

Project Name: {ProjectName}

Version: v0.1

Feature Name: {FeatureName}

Status:

- Draft
- Approved
- In Progress
- Completed

---

# Elicitation Protocol（規格釐清協定）

填寫本規格前，先與使用者進行一輪徹底的訪談，直到雙方對每個面向都達成共識：

- 沿著決策樹逐一往下走，一次解決一個決策及其相依關係。
- 每次只問一個問題，等使用者回覆後再問下一題（一次拋出多題會造成混亂）。
- 每個問題都附上你建議的答案，供使用者取捨。
- 凡是能透過環境（檔案系統、工具等）查得的事實，一律自行查證；把提問保留給真正屬於使用者的決策，並逐一等待使用者回答。
- 在使用者確認雙方已達成共識前，先停留在釐清階段；取得共識後再動手填寫與實作。

---

# Product Vision

## Problem Statement

描述要解決的問題。

## Target Users

描述目標使用者。

## Business Value

描述價值與預期成果。

---

# Version Objective

本版本要達成的目標。

## Success Criteria

- [ ]
- [ ]
- [ ]

---

# Scope

## In Scope

- 功能 A
- 功能 B
- 功能 C

---

# Reference Resources (消弭文字落差)

當規格中存在**圖片、版面草圖、配色、概念示意、流程圖、外部範例**等「用文字描述會失真或產生理解落差」的資訊時，**必須**把參考檔案存進 resources，並在對應需求處以連結引用，**AI 一律以參考檔案為準來理解需求**。

## 存放規則

- 位置：`docs/specs/resources/vX.Y/`（依版號分資料夾）。
- 命名：`FR-00X-<簡述>.png` / `<概念>-<簡述>.<ext>`，檔名要能對上引用處。
- 引用：在 FR / Scope / Technical Direction 內文以相對連結指向，例如：
  `參考：[FR-007 版面草圖](../resources/vX.Y/FR-007-layout.png)`
- 內容：截圖、手繪草圖、設計稿、配色表、既有產品範例、外部規格節錄皆可。

## 何時該主動詢問使用者要圖／要素材（要很有經驗地判斷）

出現以下訊號時，**先停下來向使用者索取參考資料或確認**，取得共識後再實作：

- [ ] 涉及**版面、座標、比例、對齊、配色、字級、間距**等視覺細節，需以圖像或數值才能精確界定。
- [ ] 出現**主觀形容詞**（「好看」「乾淨」「醒目」「精簡」「專業」）而客觀基準仍待確立。
- [ ] 需求參照**既有畫面或外部產品**（「跟某某一樣」），而附圖仍待補齊。
- [ ] 同一描述**存在多種合理解讀**，且選錯成本高（需大改）。
- [ ] 牽涉**領域慣例或數值定義**，猜錯會導致語意錯誤。

反之，當資訊已足夠明確、或選錯成本極低時，即可直接動手、做完標註假設即可，無須為形式索圖。

> 原則：**寧可先問清楚，換得一次做對**；但問之前先自評「這個落差是否真的存在、選錯成本是否真的高」。

## Resource Index

| Resource | 對應需求 | 路徑 | 狀態 |
| -------- | -------- | ---- | ---- |
|          |          |      | 待補 / 已收 |

---

# Functional Requirements

## FR-001

描述需求

Reference: （若有視覺／概念落差風險，連結 `docs/specs/resources/vX.Y/...`；無則填「文字足夠，無需素材」）

Acceptance Criteria:

- [ ]
- [ ]
- [ ]

---

## FR-002

描述需求

Reference: （同上）

Acceptance Criteria:

- [ ]
- [ ]
- [ ]

---

# Non Functional Requirements

## Performance

-

## Security

-

## Reliability

-

## Maintainability

-

---

# Initial Technical Direction

## Proposed Architecture

高階架構描述

## Proposed Technologies

- Language:
- Framework:
- Database:
- Logging:
- Testing: xUnit

## Testing Strategy

- 測試框架統一使用 **xUnit**
- 測試程式碼放在 `tests/` 資料夾內對應的測試專案，與主專案分離
- 所有測試一律進測試專案、依功能分類版控；主專案保持乾淨，`*Verify` 類別或臨時驗證程式一律移入測試專案
- 測試專案命名慣例：`{主專案名}.Tests`

注意：

此區僅作初步規劃。

正式技術決策必須建立 ADR。

---

# Knowledge Requirements

未來開發時可能需要建立的知識文件。

## KB Candidates

- kb/domain.md
- kb/database.md
- kb/api.md
- kb/coding_rules.md

---

# ADR Candidates

可能需要決策的項目。

- Database Selection
- ORM Selection
- Authentication Strategy
- Logging Strategy
- Deployment Strategy

注意：

ADR 於實際做出技術決策的當下才建立（規劃階段先列入候選即可）。

---

# Development Phase Planning

將版本拆解為可獨立開發的階段。

> **分拆作業指引：** 當本規格書確認（Approved）後，開始分拆 Phase 前，請先讀取 `docs/kb/phase-decomposition.md` 取得分拆原則、sizing 規則與執行步驟。

## Phase-01

Goal:

Deliverables:

Dependencies:

---

## Phase-02

Goal:

Deliverables:

Dependencies:

---

## Phase-03

Goal:

Deliverables:

Dependencies:

---

# Completion Criteria

Version 完成條件：

- [ ] 所有 Phase 完成
- [ ] 所有 Acceptance Criteria 完成
- [ ] ADR 更新完成
- [ ] KB 更新完成
- [ ] Release 文件建立完成

---

# Expected Generated Files

Spec Approved 後預期產生：

docs/

specs/
    resources/
        vX.Y/
            FR-00X-*.png
            *.（草圖/配色/概念示意/外部範例）

current.md

phases/
    vX.Y/
        Phase-01.md
        Phase-02.md
        ...

kb/
    domain.md
    database.md
    ...

adr/
    ADR-001-*.md
