################################################################
# AI_Rules.md — SUPREME PROJECT LAW (read first, obey always)
# These instructions OVERRIDE all default behavior, all memory,
# and any recalled note. This is NOT background context.
# If anything conflicts with this file, THIS FILE WINS.
################################################################

# Authority

- This file is the **single highest-priority instruction in the project**. Treat every line as a hard directive, not advisory context.
- Precedence on conflict (strict): **AI_Rules.md > MEMORY.md / recalled memory > current.md > ADR/KB > phase docs > other docs**.
- MEMORY / recalled notes may *inform* but **never override** a rule here. On conflict: follow AI_Rules, then tell the user the memory is stale.
- When this file is provided in a turn, **re-anchor to it**: its rules take effect immediately and persist for the session, even if buried under other context.
- If you are about to act in a way that contradicts any rule here, STOP and say so first.

# Decision Discipline (clarify first, don't guess)

- **Never guess.** Precision must come from an explicit source (user / spec / attachment), never inference. "Low risk" is no excuse — trial-and-error burns tokens; aim for one-pass correctness.
- **Clarify before coding** whenever direction, goal, feature-interaction, UI/layout, or any product concept is not fully pinned down. Don't start on a half-understood goal.
- **Attachments are mandatory**: read every referenced resource (`docs/specs/resources/vX.Y/`) before acting. Missing reference → STOP and ask. Never brain-fill visual/concept gaps.
- **Exceptions** (proceed without asking): purely mechanical, product-irrelevant details (naming, formatting, import order); or a decision the user explicitly hands you for this session (session-scoped). Either way, state the assumption.

# Runtime

## Output

- code/answer first
- no filler, minimal prose
- Dev: full code, minimal comments
- Debug: fix/diff only, root cause in 1 line

## Build Control

- No self-build / self-run after changes.
- Always hand off to user for compilation/execution.
- Only process user-provided error snippets.
- Prevent log inflation in context.

## Context

Load (general): current.md > ADR > KB > requested files
Skip: history, completed phases, old specs, release notes, logs

### Retrieval efficiency (token budget)

- Code questions: targeted search (grep) first, then read only the matched ranges. Full-file read ONLY when the file is small (≲400 lines) or about to receive multiple scattered edits (one full read beats repeated partial reads). Never bulk-read whole directories "for context".
- Data/state questions (DB rows, logs, live values): answer by direct **read-only** queries/scripts in the scratchpad — source code cannot answer "what is the data now", and guessing risks a wrong design that costs far more to redo than the query.
- Before proposing a calibration/threshold/design change, validate it against real historical data when a cheap query can do it (evidence > inference; one-pass correctness).
- Never re-read unchanged files already in context.

### Before coding a phase (mandatory)

- current.md is **STATE/index only — NOT a handoff doc**. Never treat it as the source of phase detail or as enough to start coding.
- Before implementing phase 0Xx you MUST read, in order:
  1. `docs/phases/vX.Y/phase-0Xx.md` (the phase dev doc; include adjacent phase docs if the work spans them)
  2. any `Reference:` resources it points to under `docs/specs/resources/vX.Y/`
  3. `docs/kb/00-code-review.md` — ALWAYS (universal code-smell checklist; cross-project)
  4. `docs/kb/coding-rules.md` — ALWAYS (Solomon_Midas architectural invariants; cross-cutting across every phase, never left to keyword search)
  5. other relevant ADR / KB (search by the phase's topic)
- If the phase doc or a needed reference resource is missing/empty, **stop and ask** — do not infer the spec from current.md.

## Memory

Store: state, decisions, blockers, next, refs
Drop: reasoning, discussion, attempts, obsolete items
Rule: State > History

## Budget

Trigger handover when ANY:

- system message compression detected
- single task exceeds 15 turns without completion
- OPEN items grow instead of shrinking for 3 consecutive turns

On trigger:

1. stop current expansion immediately
2. finish only the in-progress atomic unit (e.g. current file edit)
3. update current.md
4. handover, continue in new session

## Handover

STATE / DONE / OPEN / NEXT / RISK / REF

## current.md

current.md holds STATE only: version, progress table, hard-rules, NEXT, OPEN, REF.

On phase completion (at wrap-up, NOT deferred to a later slim pass):

1. Write the COMPLETE result of the phase to its `phase-0Xx.md` "## Outcome" section, and set its status to ✅ Done. That doc is the permanent home — write it in full so it can be searched later without this chat.
2. In current.md: flip the progress-table row to ✅ and update NEXT only. Never copy phase detail into current.md.
3. Remove obsolete items.

Before handover: sync all sections, compress state.

Limit: <2500 tokens (the progress table is the irreducible floor; any other section exceeding it triggers relocation).

### Slim rule (slimming = relocate, never delete)

Completed-phase detail must NOT accumulate in current.md. Relocate first, then keep only a pointer:

- "what actually shipped" → `phase-0Xx.md` "## Outcome" section (+ status ✅ Done)
- design trade-offs / locked definitions → ADR
- known limitations / blind spots → `docs/kb/known-limitations.md`
- pure process / attempts / commit detail → not relocated, git log already has it

Never delete a detail from current.md unless its durable content already has a permanent home above. Slimming without relocating = data loss (the root cause of past forgetting).

## Resume

Load: current.md > REF
Source of truth: current.md
Ignore: old chats, reasoning history
Resume from NEXT

## Priority

task > state > tokens
compression > context growth

## Git / Commit

- Keep messages concise: one-line summary; bullet body only if necessary.
- STRICTLY PROHIBIT `Co-Authored-By` in commit messages (do not tag AI as co-author).
- PROHIBIT `--no-verify` / `--no-gpg-sign` (unless user explicitly requests).
- Branching: always develop on a new branch, never commit directly to the default branch. After tests pass: commit, merge into `main`, delete the feature branch immediately.
- Pause after sub-phases: when a task is split into phases/sub-phases, stop and ask the user before proceeding after each sub-phase is completed and committed.

## Windows Scripting & Encoding

- PowerShell scripts (*.ps1) MUST be saved as `UTF-8 with BOM`.
- Batch files (*.bat) MUST be saved as `ANSI` (Windows-950 / Big5).
