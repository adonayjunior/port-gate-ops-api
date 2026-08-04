# Discovery plan

## Discovery objective

Determine whether early validation and a shared appointment lifecycle address a material operational problem, and identify which workflow and exception capabilities are required for a safe pilot.

No interviews or observations are claimed in this repository. The items below are a proposed discovery plan.

## Assumption map

| Assumption | Importance | Current evidence | How to test |
|---|---:|---|---|
| Invalid or inconsistent appointments create meaningful gate rework | High | Domain experience only | Baseline rejection reasons and observe gate handling |
| Dispatchers can correct most errors before truck departure | High | None | Interview dispatchers; prototype validation messages |
| One active appointment per container is usually the safe default | High | Implemented as a domain rule, not user-validated | Review exception history with operations |
| A fixed 30-minute grace period is acceptable | High | Implemented for demo simplicity | Analyze early/late arrivals by visit type and shift |
| Gate clerks can identify and assign a lane digitally | Medium | Implemented as a text input | Workflow observation and usability test |
| Status visibility improves coordination across teams | Medium | None | Prototype operational view; measure calls/manual checks |
| Reduced validation errors will improve gate processing time | High | Causal hypothesis only | Controlled pilot with baseline and guardrails |

## Research methods

### 1. Operational baseline

Collect four to eight weeks of aggregate, anonymized data:

- scheduled, cancelled, expired, and completed visits;
- arrival distribution relative to booked windows;
- rejection and manual-correction reasons;
- gate processing time from arrival/check-in to completion;
- repeated attempts and duplicate appointments;
- volume by shift, lane, and direction.

### 2. Contextual observation

Observe dispatch and gate work during normal and peak periods. Focus on handoffs, workarounds, duplicate entry, exception escalation, and which system is treated as the source of truth.

### 3. Semi-structured interviews

Suggested sample: 4-6 dispatchers across carriers, 4-6 gate clerks across shifts, 2-3 supervisors, and the terminal product/operations owner.

Core prompts:

1. Tell me about the last appointment that failed at the gate.
2. Where was the problem first detectable?
3. What information did you need, and where did you get it?
4. Which rules are frequently overridden, by whom, and why?
5. What is the cost of a failed or delayed visit for your team?
6. How do you know whether the gate is performing well today?

Avoid asking users whether they “want a feature.” Ask about recent behavior, frequency, consequence, and existing workaround.

### 4. Prototype tests

Test the scheduling, error-recovery, arrival, and exception paths with realistic but synthetic data. Measure task success, time on task, error comprehension, and confidence in the appointment state.

## Highest-risk questions before a pilot

1. Which exceptions make the one-active-appointment rule unsafe or impractical?
2. Should grace periods vary by direction, cargo type, carrier, or shift?
3. What is the authoritative arrival timestamp and how reliable is it?
4. What offline or degraded-mode behavior is required to avoid stopping the gate?
5. Which roles may override a rule, and what audit evidence is mandatory?
6. Which operational identifiers may be stored or emitted to analytics?

## Validation criteria

Proceed to a limited pilot only if:

- the baseline confirms a material, addressable failure mode;
- target users can complete core prototype tasks reliably;
- operations approves exception and degraded-mode procedures;
- security and data governance approve the data model;
- target metrics and guardrails can be measured before rollout.

If the dominant delays come from physical inspection, yard constraints, or unrelated documentation, the team should reconsider the solution rather than force an appointment-system rollout.
