# MVP scope

## MVP intent

The implemented MVP demonstrates the smallest coherent appointment lifecycle that can validate domain behavior end to end. It is a portfolio reference implementation, not a production pilot release.

## Implemented capabilities

| Capability | User/operational value | Delivery evidence |
|---|---|---|
| Schedule inbound or outbound visit | Establishes an expected container/vehicle visit and time window | API endpoint, aggregate, application tests |
| Validate ISO 6346 container number | Detects malformed identifiers before arrival | Value object and domain tests |
| Validate vehicle plate presence/format | Protects basic appointment integrity | Value object and domain tests |
| Prevent concurrent active appointments for a container | Reduces ambiguous operational state | Application rule and test |
| Check in within window plus 30-minute grace | Connects planned visit to physical arrival | Domain transition and boundary tests |
| Assign a gate lane at check-in | Captures the initial operational routing decision | Domain rule and API endpoint |
| Complete after check-in | Prevents a movement from being completed before it starts | Domain transition and tests |
| Cancel before check-in | Supports a basic plan change without corrupting execution state | Domain transition and test |
| Represent expiry | Distinguishes a missed visit from cancellation | Domain method and tests |
| Retrieve and filter appointments by status | Provides basic operational visibility | API endpoint and application test |
| Return domain errors without stack traces | Makes invalid actions explicit and safer for consumers | Exception middleware and exercised flows |

## Important implementation nuance

`ExpireIfOverdue` exists in the domain but is not currently invoked by an API endpoint or background process. Expiration is therefore a modeled capability, not an automated end-to-end behavior.

## Deliberately excluded from the reference MVP

| Exclusion | Reason | Required before |
|---|---|---|
| Durable database | In-memory storage keeps the demo zero-setup | Any pilot |
| Authentication and role-based authorization | Avoids pretending a starter API is production-secure | Any user access |
| Audit trail and reasoned overrides | Needs validated exception policy | Operational use |
| Capacity per time slot | Requires terminal-specific constraints and discovery | Throughput optimization pilot |
| Rescheduling | Cancellation plus new scheduling keeps the initial model small | Dispatcher workflow pilot |
| Notifications | Channel and timing require user research | Carrier rollout |
| Automated expiry job | Scheduling/hosting design is outside core domain demo | Reliable no-show measurement |
| Product analytics | Event taxonomy is documented but not emitted | Outcome evaluation |
| External TOS/hardware integration | Would add fictional complexity and risk implying proprietary behavior | Integrated pilot |
| High availability/degraded mode | Requires production infrastructure and operational procedures | Mission-critical deployment |

## Pilot readiness definition

A limited pilot is not “MVP plus a database.” It requires, at minimum:

- durable storage, migrations, backups, and recovery tests;
- identity, least-privilege roles, and audited overrides;
- observability, alerting, and operational support ownership;
- an automated expiry mechanism;
- measurable baseline, analytics events, and dashboard definitions;
- agreed degraded-mode procedure;
- privacy/security review and data-retention policy;
- usability validation with dispatchers and gate clerks.

## Acceptance narrative

The MVP is successful as a reference implementation when a reviewer can run the complete schedule -> check-in -> complete flow, observe invalid actions being rejected consistently, trace the rules to tests, and distinguish implemented behavior from proposed product work.

It cannot yet demonstrate improved terminal performance; that requires the discovery and pilot plan described elsewhere in this case study.
