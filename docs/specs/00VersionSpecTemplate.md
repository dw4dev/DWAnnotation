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

## Non Goals

本版本不處理：

- ...
- ...
- ...

---

---

# Execution Constraints

## Agent Sizing Constraint

所有規劃必須遵守：

每個開發單位（Phase / Task）必須小到足以讓 Sonnet 4.6 在單次執行週期內完成。

若超出此範圍，必須進一步拆分。

原則：

- 一個 Phase = 一個明確目標
- 一個 Phase = 可獨立驗收
- 一個 Phase = 可獨立回滾
- 一個 Phase = 單次 Agent 任務可完成

---

# Scope

## In Scope

- 功能 A
- 功能 B
- 功能 C

## Out Of Scope

- 功能 X
- 功能 Y

---

# Functional Requirements

## FR-001

描述需求

Acceptance Criteria:

- [ ]
- [ ]
- [ ]

---

## FR-002

描述需求

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
- Testing:

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

ADR 不在規劃階段建立。

只有真正做出技術決策時才建立。

---

# Development Phase Planning

將版本拆解為可獨立開發的階段。

重要：

每個 Phase 必須控制在單一 Sonnet 4.6 Agent 可完整理解、實作、測試、驗證的範圍內。

避免：

- 單一 Phase 涵蓋多個子系統
- 單一 Phase 需要跨越大量檔案修改
- 單一 Phase 包含多個獨立商業流程
- 單一 Phase 超過 Agent Context 可穩定處理範圍


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

# Agent Execution Rules

當 Spec Approved 時：

1. 建立 docs/current.md
2. 建立 docs/phases/vX.Y/
3. 產生對應 Phase 文件
4. 建立必要 KB 文件
5. 建立必要 ADR 文件（僅在決策發生時）

---

# Retrieval Strategy

重要：

禁止載入整個 docs 目錄。

採用 RAG / Search First 策略。

讀取順序：

1. current.md
2. Current Phase
3. Referenced ADR
4. Referenced KB
5. 必要時搜尋其他文件

禁止：

- 預載所有 ADR
- 預載所有 Spec
- 預載所有 Release
- 預載整個 docs tree

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
