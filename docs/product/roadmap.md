# Outcome-based roadmap

This roadmap communicates intent and learning sequence. **Next** and **Later** are options, not delivery commitments or dates.

## Now — establish a trustworthy lifecycle

**Outcome:** reviewers can exercise and inspect a coherent appointment lifecycle.

**Implemented:**

- schedule inbound/outbound visits;
- validate container number and appointment window;
- prevent more than one active appointment per container;
- check in within a fixed grace window and assign a lane;
- complete or cancel through valid state transitions;
- retrieve/filter operational state;
- verify core behavior with automated tests and CI.

**Learning status:** technical behavior verified; user value and operational outcomes not yet validated.

## Next — make a limited pilot safe and measurable

**Outcome:** a small, controlled user group can operate the workflow without compromising continuity or accountability.

### Readiness work

- durable database, migrations, backup, and recovery;
- identity, role-based access, and audit trail;
- observability and degraded-mode procedure;
- automated expiry processing;
- privacy/security review;
- product analytics and baseline dashboard.

### Discovery-led workflow candidates

- rescheduling with eligibility rules;
- actionable rejection reasons;
- configurable capacity and grace policies;
- basic carrier and supervisor views;
- permissioned, reasoned exception handling.

**Exit evidence:** users complete core tasks in prototype/usability tests; baseline and guardrails are measurable; operations approves pilot procedures.

## Later — improve coordination and prediction

**Outcome:** use trustworthy operational data to reduce avoidable variability across a broader network.

Potential options:

- notifications based on validated user needs;
- integration through generic, versioned events;
- hardware-assisted arrival capture where operationally justified;
- richer capacity allocation and exception workflows;
- anomaly detection for repeated failures;
- predictive arrival/capacity support only after data quality and adoption are proven.

## Roadmap review triggers

Review the roadmap when:

- discovery disproves a high-importance assumption;
- a guardrail is breached;
- regulation, security, or terminal policy changes;
- the operational baseline shows a different bottleneck;
- integration or data-quality constraints change feasibility materially.

## What is not promised

The roadmap does not promise dates, production deployment, integration with a named commercial system, or quantified business results. Those require a real sponsor, discovery evidence, technical assessment, and pilot governance.
