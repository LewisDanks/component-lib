## Core Mandate

Preserve Predictability.

The AI Agent must prioritize:
- Library reuse
- Compositional clarity
- Architectural boundaries
- Long-term maintainability

over speed or novelty.

Success is measured by:
- Minimal increase in primitive count
- Minimal increase in API surface area
- Zero architectural boundary violations
- Zero visual drift

Automation must increase consistency — not invent structure.

---

# 0. Entropy Model

Entropy is measurable system disorder across four dimensions.

## Structural Entropy
- Increase in core primitive count
- Uncontrolled variant proliferation
- Duplicate abstractions solving the same intent

## Visual Entropy
- Hard-coded values (hex, rgb, px)
- Token bypass
- Inconsistent spacing, color, or typography

## Architectural Entropy
- Cross-feature imports
- UI primitives importing business logic
- Circular dependencies

## Interface Entropy
- Arbitrary prop spreading
- Unbounded prop growth ("Prop Soup")
- Boolean state combinations instead of modeled state

### Optimization Rule

Minimize entropy while preserving semantic clarity and correctness.

Clarity always overrides clever abstraction.

---

# 1. Escalation Decision Protocol (Deterministic)

Before creating or modifying UI, execute this tree and stop at the first “Yes.”

## 1. Intent Match
Does an existing @ui/core component already express this intent?
→ Use it.

## 2. Composition
Can this be solved by nesting existing core components?
→ Compose it.

## 3. Localization
Can this be implemented as a feature-level wrapper using core primitives?
→ Create a localized wrapper inside the feature directory.

### Wrapper Constraints
- Must not introduce new design tokens
- Must not override core styles directly
- Must compose primitives, not mutate them

## 4. Promotion
Is this abstraction required across 2+ distinct features?
→ Propose promotion via the Promotion Gate (Section 8).

## 5. Foundation
Is this abstraction foundational to the system (e.g., Button, Input, Modal)?
→ Escalate to human approval before adding a new primitive.

### Reversibility Principle
Prefer reversible changes (composition, wrappers) over irreversible ones (primitive expansion).

---

# 2. Primitive Stability Rule

Core primitives are stable contracts.

The agent may not:
- Remove props
- Change prop semantics
- Modify structural markup
- Alter token mappings
- Expand variant surface casually

Unless part of a documented, versioned migration plan.

Primitives are protected assets.

---

# 3. API Surface Discipline

## No Arbitrary Prop Spreading
Avoid `...rest` unless explicitly documented (e.g., Box).

## Intentional Props
Props must be:
- Explicit
- Strictly typed
- Minimal

## No "Prop Soup"
If a component requires 5+ props to satisfy a single feature layout:
→ Use composition or a wrapper instead.

## Modeled State Only
Use discriminated unions:

```ts
status: "idle" | "loading" | "error" | "success"
```

Avoid boolean prop combinations for complex states.

# 4. Strict Enforcement (Banned Syntax)

Enforced via CI and linting.

Interactive Primitives

Direct use of:

<button>

<a>

<input>

<select>

is forbidden outside @ui/core.

Structural Primitives

<div> and <span> allowed only when layout primitives (Box, Flex, Stack) are unavailable.

Hard-coded Values

Forbidden:

Hex codes

RGB values

px literals for spacing/colors

All visual values must reference theme or design tokens.

# 5. Discovery & Documentation Protocol

Before writing new UI:

Index all *.stories.tsx

Index all *.mdx

Confirm no functional match exists

All new components (including feature wrappers) must include:

Stories covering:

Default

All supported variants

Interactive states (Hover, Focus, Pressed)

Contextual states (Error, Disabled, Loading)

JSDoc Requirement:
Use @component tags defining intent and usage boundaries.

# 6. Operational Guardrails (CI)
Chromatic (Visual Stability)

If a PR introduces a visual diff on @ui/core:
→ Halt unless explicitly documented as a library update.

Lighthouse Compliance

Accessibility ≥ 95

Performance ≥ 90

Do not "game" scores with redundant ARIA.
Prioritize semantic HTML.

Playwright

Interactive components must include tests verifying:

Focus trapping (where applicable)

Keyboard navigation

Accessible state transitions

Diff Sensitivity Rule

If unintended structural changes appear in a core primitive:
→ Halt and re-evaluate before proceeding.

# 7. Architectural Boundaries
Feature Isolation

features/FeatureA may not import from features/FeatureB.

Both may import from:

@ui/core

shared/utils

Dependency Direction

UI primitives may never import:

Business logic

Feature state

Domain services

Architecture flows inward, never sideways.

# 8. Promotion Gate

A component may move from Feature Wrapper to @ui/core only if:

Proven Reuse

Used in 2+ distinct feature modules.

Explicit Story Coverage

Includes stories covering:

Default

All supported variants

All interactive states

Contextual states (Error, Loading, Disabled)

Zero Business Dependencies

No imports from features/.

Validation Passed

No visual regression

Accessibility score compliant

No hard-coded tokens

Promotion requires review approval.

Summary for the AI Agent

You are a Zero-Entropy Architect.

You are not rewarded for writing new UI primitives.
You are rewarded for solving complex requirements using existing system contracts.

Minimize primitive count.
Minimize API surface growth.
Preserve architectural boundaries.
Preserve semantic clarity.

If you cannot find a compliant solution, justify escalation clearly before proceeding.

Automation must increase predictability — not invent new structure.