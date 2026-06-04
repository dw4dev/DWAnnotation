# AI Runtime Rules

## Output Rules

- Lead with answer or code.
- No openers, fillers, or transition phrases.
- Reasoning skeleton:
  - [STATE]
  - [ERROR]
  - [CAUSE]
  - [LOGIC]

### Development Tasks

- Return complete code blocks.
- Keep comments minimal.
- Avoid explanatory prose unless requested.

### Debug Tasks

- Return corrected code or diff.
- One-sentence root cause.
- Include only critical context.

---

## Context Rules

Load order:

1. current.md
2. Referenced ADR
3. Referenced KB
4. Explicitly requested files

Never load by default:

- completed phases
- release history
- old specs
- full docs tree
- implementation logs

---

## Token Optimization Rules

Principle:

State over History.

Store:

- final decisions
- current state
- active blockers
- next actions
- required references

Do not store:

- reasoning history
- discussion history
- trial-and-error records
- obsolete decisions

---

## Handover Format

Required structure:

[STATE]
Current objective

[DONE]
Completed work

[OPEN]
Remaining work

[NEXT]
Immediate next action

[REF]
Required documents only

---

## current.md Update Rules

After every completed task:

1. Update [DONE]
2. Update [OPEN]
3. Update [NEXT]
4. Remove obsolete items
5. Keep file concise

Target size:

- current.md < 500 tokens

---

## AI Behavior Rules

- Prefer English for technical terms.
- Prefer bullets over prose.
- Preserve information with minimum tokens.
- Do not expand context unless required.
- Use referenced documents only.
- Optimize for long-running development sessions.
