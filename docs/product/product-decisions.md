# Product decision records

These lightweight records connect business intent to domain and architecture choices. “Accepted” means accepted for this reference implementation, not universally correct for every terminal.

## PDR-001 — Validate container identifiers when scheduling

**Status:** Accepted / Implemented

**Decision:** Reject an invalid ISO 6346 container number before creating the appointment.

**Rationale:** correction is cheaper before dispatch than at physical arrival, and the rule is deterministic and testable.

**Trade-off:** strict validation can reject legitimate exceptional identifiers. A real product needs a governed exception policy rather than silently weakening validation.

**Measure later:** rejection reason distribution, correction success, and false-rejection rate.

## PDR-002 — Allow one active appointment per container

**Status:** Accepted / Implemented / Requires discovery validation

**Decision:** A container cannot have another appointment while one is Scheduled or CheckedIn.

**Rationale:** avoids ambiguous state and accidental duplicate visits in the reference workflow.

**Trade-off:** split movements, multi-leg processes, or data-correction scenarios may require a more nuanced uniqueness rule.

**Revisit when:** operational examples show legitimate concurrent visits or identifiers are not unique enough for this policy.

## PDR-003 — Use a fixed 30-minute check-in grace period

**Status:** Accepted for demo / Implemented

**Decision:** Accept check-in from 30 minutes before the window start through 30 minutes after its end.

**Rationale:** demonstrates boundary behavior without inventing a large configuration model.

**Trade-off:** a universal fixed policy is unlikely to fit all directions, cargo types, carriers, and shifts.

**Next decision:** use arrival data and policy interviews to determine whether grace is global, segmented, or manually governed.

## PDR-004 — Model lifecycle transitions in the domain

**Status:** Accepted / Implemented

**Decision:** The aggregate, rather than controllers or UI code, owns Scheduled -> CheckedIn -> Completed/Cancelled/Expired transitions.

**Rationale:** every channel receives the same operational rules, reducing inconsistent behavior and making policies directly testable.

**Trade-off:** workflow complexity can make a single aggregate difficult to evolve; integration and concurrency design will need review before scale.

## PDR-005 — Do not provide a silent override

**Status:** Accepted for MVP

**Decision:** Invalid transitions fail; there is no bypass parameter.

**Rationale:** an unaudited override would undermine state trust and hide product problems.

**Trade-off:** real operations have legitimate exceptions. A pilot needs role-based, reasoned, audited exception handling and an escalation procedure.

## PDR-006 — API-first, separate user interfaces

**Status:** Accepted / Implemented across the portfolio ecosystem

**Decision:** Keep the domain/API independent from carrier and operations interfaces.

**Rationale:** different users can receive task-specific experiences while sharing one lifecycle contract. The companion `port-gate-portal` demonstrates separate React and Angular clients.

**Trade-off:** versioning, contract testing, and end-to-end observability become necessary as consumers grow.

## PDR-007 — Use in-memory persistence in the reference implementation

**Status:** Accepted for demo only / Implemented

**Decision:** Run with EF Core InMemory so reviewers can start the API without external infrastructure.

**Rationale:** optimizes portfolio accessibility and keeps focus on the domain.

**Trade-off:** state disappears on restart and behavior does not prove production database correctness. Durable persistence, migrations, concurrency controls, backup, and recovery are mandatory before a pilot.

## PDR-008 — Separate product targets from achieved outcomes

**Status:** Accepted / Documentation policy

**Decision:** Label proposed metrics as targets and discovery statements as assumptions.

**Rationale:** the repository has delivery evidence but no real-user pilot, baseline, or production telemetry.

**Trade-off:** the case study is less promotional, but more credible and auditable.
