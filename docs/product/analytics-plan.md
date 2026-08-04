# Product analytics plan

## Purpose

Instrumentation should answer whether the product improves appointment quality and execution reliability, not simply count API calls.

Everything in this document is **proposed**. The current API does not emit product analytics events.

## Privacy and data rules

- Do not send raw container numbers or vehicle plates to analytics.
- Use a non-reversible analytics identifier or tightly governed pseudonymization only when longitudinal analysis is required.
- Separate operational records from product analytics storage.
- Minimize actor data; prefer role and organization pseudonym over personal identity.
- Define retention, access, deletion, and incident procedures before collection.
- Never place authentication tokens, free-text operational notes, or exception stack traces in events.

## Core event taxonomy

| Event | Trigger | Key properties | Product question |
|---|---|---|---|
| `appointment_schedule_attempted` | Scheduling request begins | direction, requested window bucket, actor role | How much demand reaches the workflow? |
| `appointment_schedule_rejected` | Validation/business rule fails | reason code, field category, actor role | Which pre-arrival problems are most frequent? |
| `appointment_scheduled` | Appointment is created | appointment analytics ID, direction, lead-time bucket | Are valid visits being planned early enough? |
| `appointment_cancelled` | Scheduled visit is cancelled | lead-time bucket, reason code when available | Why and when do plans change? |
| `appointment_expired` | Visit passes its window without check-in | direction, window/shift bucket | Where are no-shows concentrated? |
| `check_in_attempted` | Gate arrival is processed | arrival offset bucket, lane group | What arrival patterns reach the gate? |
| `check_in_rejected` | Check-in fails | reason code, arrival offset bucket | Which rules create operational exceptions? |
| `appointment_checked_in` | Check-in succeeds | arrival offset bucket, lane group | How reliably do visits arrive within policy? |
| `appointment_completed` | Movement completes | cycle-time bucket, direction | Does a planned visit finish reliably and quickly? |
| `appointment_override_used` | Future governed override succeeds | reason code, actor role, approval path | Are rules misaligned with legitimate operations? |

## Controlled vocabularies

Rejection and override reasons must use stable codes, not free text. Initial examples:

- `invalid_container_identifier`
- `invalid_vehicle_identifier`
- `invalid_time_window`
- `duplicate_active_appointment`
- `outside_allowed_window`
- `invalid_state_transition`
- `lane_required`
- `appointment_not_found`
- `policy_exception_approved`

Codes should be versioned and mapped to user-facing messages separately.

## Derived metrics

| Metric | Event logic |
|---|---|
| First-attempt scheduling success | scheduled / distinct schedule attempts |
| Validation rejection rate | schedule rejected / schedule attempted |
| On-window arrival rate | checked in with accepted arrival offset / scheduled eligible visits |
| Completion rate | completed / checked in |
| Check-in rejection rate | check-in rejected / check-in attempted |
| Median observable gate cycle | completed timestamp - successful check-in timestamp |
| Expiry/no-show rate | expired / non-cancelled scheduled visits |
| Override rate | override used / gate visits processed |

The reliable gate visit north-star also requires a correction/override signal that does not exist today.

## Data-quality checks

- Event ID is unique and idempotent.
- Appointment analytics ID links lifecycle events without exposing the operational identifier.
- Server timestamp and source are recorded consistently.
- Lifecycle sequences are monitored for impossible transitions.
- Missing completion and expiry events are reconciled.
- Schema versions are explicit and backward compatible.

## Experiment/pilot dashboard

A pilot dashboard should show:

1. primary outcome and guardrails against baseline;
2. visit funnel from schedule attempt to completion;
3. rejection reasons and arrival-offset distribution;
4. cycle-time median, p75, and p95;
5. segmentation by agreed non-personal dimensions;
6. data completeness and instrumentation health.

Access to operational segments must follow minimum-group thresholds to reduce re-identification and inappropriate carrier comparison.
