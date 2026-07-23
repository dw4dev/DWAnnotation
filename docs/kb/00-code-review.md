# Code Review — Universal Checklist

> 跨專案通用（隨 `00-*` 模板一起複用，換專案直接帶走）。
> 定位：**動工前必讀 + 交手前自審**，逐條對照本次 diff。
> 本專案專屬的架構不變式另見 `docs/kb/coding-rules.md`。

---

## Code Smell Checklist

Each smell reads **what it is → how to fix**; match it against the diff. If a fix resists you, the design itself wants rethinking.

- **Mysterious Name** — a function, variable, or type whose name hides what it does or holds. → rename it until the name states its job; if an honest name resists you, the design is still murky.
- **Duplicated Code** — the same logic shape appears in more than one hunk or file in the change. → extract the shared shape, call it from both.
- **Feature Envy** — a method that reaches into another object's data more than its own. → move the method onto the data it envies.
- **Data Clumps** — the same few fields or params keep travelling together (a type wanting to be born). → bundle them into one type, pass that.
- **Primitive Obsession** — a primitive or string standing in for a domain concept that deserves its own type. → give the concept its own small type.
- **Repeated Switches** — the same `switch`/`if` cascade on the same type recurs across the change. → replace with polymorphism, or one shared map both sites use.
- **Shotgun Surgery** — one logical change forces scattered edits across many files in the diff. → gather what changes together into one module.
- **Divergent Change** — one file or module is edited for several unrelated reasons. → split so each module changes for one reason.
- **Speculative Generality** — abstraction, parameters, or hooks added for needs beyond what the spec asks. → delete it; inline back until a real need shows.
- **Message Chains** — long navigation like `a.b().c().d()` that couples the caller to the whole path. → hide the walk behind one method on the first object.
- **Middle Man** — a class or function whose main job is delegating onward. → cut it, call the real target directly.
- **Refused Bequest** — a subclass or implementer that overrides most of what it inherits. → drop the inheritance, use composition.
