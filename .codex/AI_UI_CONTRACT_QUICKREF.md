
# AI UI Contract – Quick Reference (v5.0)

This document is a condensed execution reference.
The canonical source of truth is AI_UI_CONTRACT.md.

---

# Escalation Order (Stop at First "Yes")

0. Intent Match  
   → Use existing @ui/core component.

1. Composition  
   → Compose existing core components.

2. Localization  
   → Create feature-level wrapper.

3. Promotion  
   → Required in 2+ features? Propose promotion via gate.

4. Foundation  
   → Foundational abstraction? Escalate to human.

Reversibility Principle:
Prefer composition and wrappers over primitive expansion.

---

# Decision Table

| Scenario | Action |
|-----------|--------|
| Core component already matches intent | Use it |
| Can be composed | Compose |
| Feature-specific UI | Create wrapper |
| Used in 2+ features | Promote |
| Core API insufficient | Propose API change + migration plan |
| No abstraction exists | Escalate for new primitive approval |

---

# Non-Negotiables

- No raw interactive primitives outside @ui/core
- No hard-coded tokens
- No cross-feature imports
- No prop soup
- No primitive mutation without versioned migration

---

# Promotion Gate Summary

To move into @ui/core:
- Used in 2+ features
- Full story coverage
- No business dependencies
- Visual + accessibility checks passed

---

Automation must increase predictability.
Minimize entropy.
Preserve clarity.