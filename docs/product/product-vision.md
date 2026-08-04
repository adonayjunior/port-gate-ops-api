# Product vision

## One-line vision

Help terminal and carrier teams coordinate predictable gate visits by detecting appointment problems before a truck reaches the physical gate and by maintaining a trustworthy operational lifecycle.

## Fictional context

A container terminal receives trucks throughout the day. Carrier dispatchers plan visits, while gate clerks and supervisors execute and monitor arrivals. When appointment data is invalid, duplicated, or inconsistent with the actual visit, the correction happens at the most expensive point: the gate itself.

**Assumption:** avoidable corrections and unclear appointment state contribute to longer processing times, rework, and reduced confidence in the daily plan.

## Product problem

The product is not primarily an appointment CRUD. It is a coordination mechanism between planning and physical execution:

1. A visit should be valid before the truck travels to the terminal.
2. Operations should know which visits are expected and their current state.
3. Invalid lifecycle transitions should be prevented consistently.
4. Product telemetry should reveal where planned visits fail in practice.

## Primary users and value

| User | Need | Intended value |
|---|---|---|
| Carrier dispatcher | Create a valid visit in an appropriate window | Fewer failed arrivals and less rework |
| Gate clerk | Find the visit and register arrival quickly | Less manual interpretation at the gate |
| Gate supervisor | Understand expected and in-progress visits | Better operational visibility and exception handling |
| Terminal product/operations manager | Learn where visits fail | Evidence for process and product improvements |

These are **proto-personas**, not research-validated personas. See [personas and jobs to be done](personas-and-jtbd.md).

## Product principles

1. **Prevent errors at the cheapest point.** Validate identifiers and lifecycle rules before physical execution.
2. **Make operational state explicit.** A visit is Scheduled, CheckedIn, Completed, Cancelled, or Expired.
3. **Prefer safe failure to silent inconsistency.** Invalid transitions return an actionable error rather than corrupting state.
4. **Keep humans accountable for exceptions.** Future override capabilities must be permissioned and audited.
5. **Measure outcomes, not endpoint traffic.** API usage alone does not prove a more reliable gate operation.
6. **Protect operational data.** Product analytics should avoid raw container numbers and vehicle plates.

## Outcome hypothesis

If carrier teams receive early validation and gate teams work from a consistent visit lifecycle, then avoidable gate corrections and processing variability should decrease, because common data and sequencing errors are resolved before or at the start of the visit.

This hypothesis has not been validated with real users in this portfolio project.

## Product boundaries

The case focuses on appointment integrity and lifecycle coordination. It does not claim to model a complete terminal operation.

**Not in the current product:**

- physical queue or yard management;
- slot-capacity optimization;
- driver mobile workflows;
- OCR, RFID, weighbridge, or hardware integrations;
- billing, customs, or safety processes;
- integration with any proprietary TOS;
- production identity, authorization, audit, or durable storage.

Those boundaries keep the reference implementation small enough to inspect while making the missing production controls explicit.
