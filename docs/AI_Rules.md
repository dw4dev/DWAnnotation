# Runtime

## Output
- code/answer first
- no filler
- minimal prose
- Dev: full code, minimal comments
- Debug: fix/diff only, root cause in 1 line

## Context
Load: current.md > ADR > KB > requested files
Skip: history, completed phases, old specs, release notes, logs

## Memory
Store: state, decisions, blockers, next, refs
Drop: reasoning, discussion, attempts, obsolete items
Rule: State > History

## Budget
Trigger handover when ANY:
- system compression detected (系統開始壓縮先前訊息)
- single task exceeds 15 turns without completion
- OPEN items grew instead of shrinking for 3 consecutive turns

On trigger:
1. stop current expansion immediately
2. finish only the in-progress atomic unit (e.g. current file edit)
3. update current.md
4. handover, continue in new session

## Handover
STATE
DONE
OPEN
NEXT
RISK
REF

## current.md
After task:
- sync DONE OPEN NEXT
- remove obsolete

Before handover:
- sync all sections
- compress state

Limit: <500 tokens

## Resume
Load: current.md > REF
Source of truth: current.md
Ignore: old chats, reasoning history
Resume from NEXT

## Priority
task > state > tokens
compression > context growth

## Git / Commit
- 訊息精簡：一行主旨，必要時補條列本文
- **禁止**在 commit message 加入 `Co-Authored-By` 結尾（不附 AI 共同作者標記）
- 禁止 `--no-verify`、`--no-gpg-sign`（除非使用者明確要求）
- 僅在使用者要求時 commit/push；在預設分支上先開新分支
