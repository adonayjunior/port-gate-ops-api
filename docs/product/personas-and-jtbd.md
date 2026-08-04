# Proto-personas and jobs to be done

These profiles are **assumptions for discovery**, not claimed interview findings. In a real engagement, role names, responsibilities, incentives, and access needs would be validated at the terminal and with carrier partners.

## 1. Carrier dispatcher

**Context:** coordinates multiple vehicles and container movements, often under time pressure and across more than one system.

**Jobs to be done**

- When I plan a terminal visit, help me know immediately whether the appointment is acceptable so I can correct it before dispatching the truck.
- When plans change, help me cancel or reschedule without creating a conflicting active visit.
- When a driver reports a problem, help me understand the appointment state without calling multiple teams.

**Potential pains to validate**

- Unclear validation messages.
- Duplicate or stale appointments.
- Limited visibility after scheduling.
- Different rules across terminals or visit types.

## 2. Gate clerk

**Context:** processes arriving trucks safely and quickly; delays are visible and accumulate during peaks.

**Jobs to be done**

- When a truck arrives, help me find a valid appointment quickly so I can assign a lane and continue the process.
- When the visit is invalid, explain the reason and permitted next action so I do not improvise.
- When the movement finishes, help me record completion once and preserve a reliable operational trail.

**Potential pains to validate**

- Manual search or transcription.
- Invalid identifiers discovered only at arrival.
- Pressure to bypass rules during peaks.
- Poor recovery paths for legitimate exceptions.

## 3. Gate supervisor

**Context:** balances throughput, safety, staffing, and exception management across lanes.

**Jobs to be done**

- When planning a shift, show expected demand by time window so I can allocate lanes and staff.
- When congestion increases, show where visits are failing or taking longer so I can intervene.
- When someone overrides a rule, preserve who did it and why so the operation remains accountable.

**Potential pains to validate**

- No shared view of expected versus actual arrivals.
- Limited visibility into repeated rejection causes.
- Fixed rules that do not reflect operational exceptions.

## 4. Terminal product/operations manager

**Context:** owns process improvement and must balance carrier experience, terminal performance, safety, and implementation cost.

**Jobs to be done**

- When reviewing gate performance, help me distinguish data-quality failures, no-shows, and processing bottlenecks.
- When prioritizing improvements, show the expected outcome and evidence behind each proposal.
- When changing operational rules, help me monitor benefits and guardrails before wider rollout.

## Journey and moments that matter

| Stage | User question | Current reference behavior | Discovery gap |
|---|---|---|---|
| Plan | Can this visit be accepted? | Identifier, window, and duplicate-active rules | Capacity and entitlement rules |
| Prepare | Is the appointment still valid? | Read by ID and list/filter by status | Notifications and rescheduling |
| Arrive | Can this truck enter now? | Time-window check and lane assignment | Fast lookup, queueing, exceptions |
| Execute | Has the movement started and finished? | CheckedIn -> Completed lifecycle | Hardware and downstream integration |
| Learn | Why did visits fail? | Status is available | Rejection reasons and analytics are not instrumented |
