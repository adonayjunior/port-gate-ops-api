# Metrics framework

All numbers below are **proposed pilot targets**, not achieved results. Baselines must be measured before targets are accepted.

## North-star outcome

**Reliable gate visit rate**

> Percentage of eligible scheduled visits completed without manual data correction or unplanned rule override.

This represents the product's intended contribution: turning a plan into a correctly executed visit. It should be segmented by carrier, shift, direction, and time window to avoid hiding local problems.

The current API cannot calculate this metric because it does not record manual corrections, override reasons, or a durable event history.

## Outcome tree

| Outcome | Metric | Illustrative target | Why it matters |
|---|---|---:|---|
| Improve appointment quality | Physical-gate rejections caused by pre-detectable data errors | -30% | Tests whether earlier validation solves real rework |
| Improve execution reliability | Reliable gate visit rate | +15 percentage points | Connects planning to successful completion |
| Reduce operational delay | Median check-in-to-completion time | -20% | Measures the part of gate cycle observable by the product |
| Reduce avoidable planning failure | Expired/no-show rate | -15% | Tests reminders, rescheduling, and slot usability later |
| Improve exception handling | Median time to resolve rejected check-in | -25% | Ensures safe failure does not create operational paralysis |

Targets would be reset after baseline collection and should not be used as contractual promises.

## Leading indicators

- Percentage of scheduling attempts accepted on first submission.
- Validation rejection rate by reason.
- Percentage of visits checked in within their allowed window.
- Reschedule/cancellation lead time once those workflows exist.
- Percentage of expired visits with a known reason.
- Weekly active carrier organizations during a pilot.

## Guardrails

| Guardrail | Proposed threshold | Risk controlled |
|---|---:|---|
| False rejection rate after operational review | <1% | Valid visits blocked by an incorrect rule |
| Unexplained/manual overrides | <2% of visits | Rules bypassed without accountability |
| p95 scheduling/check-in API latency | <300 ms, excluding external dependencies | User delay introduced by the product |
| Availability during gate operating hours | >=99.9% for pilot | Operational interruption |
| Lost or duplicate lifecycle events | 0 known | Untrustworthy operational state |
| Raw identifiers in product analytics | 0 | Privacy and operational-data exposure |

## Measurement design

1. Establish a pre-pilot baseline using the same definitions and comparable shifts/carriers.
2. Start with a limited set of lanes, carriers, or visit types.
3. Compare medians and percentiles, not only averages.
4. Segment results to detect who benefits and who is harmed.
5. Record operational changes that could confound the comparison.
6. Review qualitative feedback alongside metrics.

## Decision rules for a pilot

- **Expand:** primary outcome improves and no guardrail materially degrades.
- **Iterate:** leading indicators improve but operational outcome is inconclusive, with a credible learning plan.
- **Pause:** false rejections, availability, or manual overrides exceed agreed thresholds.
- **Stop/reframe:** the dominant problem is outside appointment quality/lifecycle coordination.

## Anti-metrics

Request count, number of endpoints, story points completed, and lines of code are delivery/activity measures. They can support operational planning but do not demonstrate product success.
