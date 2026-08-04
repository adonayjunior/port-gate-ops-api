# Prioritization

## Decision model

Safety, security, and operational continuity are readiness gates, not optional features competing for a score. Once those obligations are covered, candidate features can be ranked by expected outcome, confidence, reach, and effort.

This prevents an attractive feature from outranking a control required to operate responsibly.

## Mandatory readiness gate

The following **proposed** work must be complete before any live operational pilot:

1. Durable persistence, migration, backup, and recovery.
2. Authentication, role-based authorization, and tenant/data boundaries.
3. Immutable audit records for lifecycle changes and overrides.
4. Observability, alerting, support ownership, and degraded-mode procedure.
5. Privacy/security review and approved retention policy.
6. Baseline and analytics instrumentation for outcome evaluation.

These items are ordered by dependency during delivery, not ranked by customer desirability.

## Feature prioritization after readiness

The table is an illustrative RICE exercise for a fictional pilot. Scores are assumptions, not empirical estimates.

Scales:

- **Reach:** affected pilot users/visits on a relative 1-10 scale.
- **Impact:** 0.5 minimal, 1 low, 2 medium, 3 high.
- **Confidence:** 50%, 70%, or 90% depending on current evidence.
- **Effort:** relative person-months for a cross-functional team.
- **RICE:** Reach x Impact x Confidence / Effort.

| Candidate | Reach | Impact | Confidence | Effort | Score | Decision |
|---|---:|---:|---:|---:|---:|---|
| Reschedule an eligible visit | 8 | 2 | 70% | 1.5 | 7.5 | Next: closes a likely dispatcher workflow gap |
| Capacity by time window | 10 | 3 | 50% | 3 | 5.0 | Discover first: high potential, rules unknown |
| Rejection reason analytics | 9 | 2 | 70% | 3 | 4.2 | Next: necessary to learn why visits fail |
| Carrier status notifications | 7 | 1 | 50% | 1.5 | 2.3 | Test channel/timing before building |
| Supervisor operational dashboard | 6 | 2 | 50% | 3 | 2.0 | Prototype after metric definitions stabilize |
| Predictive arrival/capacity model | 5 | 3 | 30% | 6 | 0.8 | Later: insufficient evidence and data maturity |

## Why the implemented rules came first

The reference MVP prioritizes appointment integrity and lifecycle safety because they form the foundation for every later workflow:

- Early identifier validation is cheap to demonstrate and directly testable.
- Duplicate-active prevention avoids ambiguous state.
- Explicit transitions establish a trustworthy source for future analytics.
- A small API surface makes product and architecture decisions reviewable.

## What could change the order

Priorities should change when evidence changes. Examples:

- If observation shows rescheduling is rare, move it below rejection analytics.
- If capacity constraints cause most failed visits, capacity management becomes the first outcome feature.
- If operational rules require frequent legitimate duplicates, revisit the aggregate policy before expanding the UI.
- If a reliable arrival timestamp is unavailable, do not promise gate-cycle metrics until instrumentation is fixed.

## Explicit non-priority

Predictive optimization is intentionally later. Adding an “AI” layer before trustworthy lifecycle data, baseline metrics, and operational adoption would increase complexity without establishing product value.
