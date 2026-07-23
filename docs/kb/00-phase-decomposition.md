# Development Process & Phase Decomposition

> 本文件在**規格書確認後、開始分拆階段時**讀取。適用範圍限於分拆階段；進入開發執行後即可釋放。

---

## Agent Sizing Constraint

所有規劃必須遵守：

每個開發單位（Phase / Task）必須小到足以讓 Sonnet 4.6 在單次執行週期內完成。

超出此範圍者，一律進一步拆分至可完成為止。

原則：

- 一個 Phase = 一個明確目標
- 一個 Phase = 可獨立驗收
- 一個 Phase = 可獨立回滾
- 一個 Phase = 單次 Agent 任務可完成

---

## Task Decomposition Principles

將「實作整個 X」拆成最小可驗證單位，逐一交付：

1. Analyze the user requirements described in the spec or tickets, and break them down into implementable tasks.
2. 定義介面與架構
3. 基本資料結構與 Entity 建立
4. 最小可運行版本
5. 整合與連線測試

每步完成後：檢視程式碼、跑測試、確認方向正確、再進下一步。

---

## Phase Sizing Rules

每個 Phase 必須控制在單一 Sonnet 4.6 Agent 可完整理解、實作、測試、驗證的範圍內。

確保每個 Phase：

- 聚焦單一子系統
- 僅涉及少量檔案修改
- 只承載單一獨立商業流程
- 控制在 Agent Context 可穩定處理的範圍內

---

## Spec Approved — Phase Decomposition Procedure

當 Spec 確認（Approved）後，依以下步驟分拆：

1. 建立 `docs/current.md`
2. 建立 `docs/phases/vX.Y/`
3. 產生對應 Phase 文件（每份**一律**套用下方「Phase 文件骨架」，內建必讀行）
4. 建立必要 KB 文件
5. 建立必要 ADR 文件（僅在決策發生時）

### Phase 文件骨架（每份 phase-0Xx.md 開頭必含）

```markdown
# Phase-0Xx — <一句話目標>

> 動工前必讀：`docs/kb/00-code-review.md`（通用 Code Smell Checklist）＋ `docs/kb/coding-rules.md`（本專案架構不變式），兩者橫切全 phase。
> 另讀本 phase 相關的 Reference 素材與 ADR/KB。

## Goal
## Deliverables
## Dependencies
## Acceptance Criteria
## Outcome   <!-- 完成時回填實際結果，狀態改 ✅ Done -->
```

此「必讀行」為硬性欄位：產生 phase 文件時逐份寫入，讓開發執行階段就地看到，與 `AI_Rules.md`「Before coding a phase」的全域強制形成雙重保證。

---

## Retrieval Strategy

重要：一律採用 RAG / Search First 策略，按需載入所需文件即可。

讀取順序：

1. `current.md`
2. Current Phase
3. Referenced ADR
4. Referenced KB
5. 必要時搜尋其他文件

載入原則（皆為按需）：

- ADR：僅載入被引用者
- Spec：僅載入當前相關者
- Release：僅在需要時載入
- docs tree：逐份搜尋、按需載入

---

## Design Conflict Resolution

1. 先回讀 `domain.md`「核心命題」
2. 在符合核心命題的前提下調整
3. 把調整理由寫進對應 ADR

---

## Phase Completion Checklist

1. 跑完整驗收測試
2. 記錄：實際做了什麼、跟原計畫差異、發現的新問題、學到什麼
3. 更新 `current.md`

---

## Comment Expiry Rule

在程式碼裡記錄「為何選擇捨棄 X」時，一旦 X 改變，該註解仍停留在舊狀態，形成 silent drop bug。規則：寫下這類決策註解後，須週期性回掃 X 現況是否仍成立；或改把理由保存在 commit message 裡。
