# Product case study

This folder explains the product thinking behind Port Gate Ops API: the users and operational problem, the hypotheses that shaped the MVP, the outcomes that would matter in a pilot, and the trade-offs between product value, operational risk, and technical feasibility.

The terminal, users, research plan, and target metrics are fictional. They are informed by general domain experience but do not reproduce any client's processes, data, or proprietary rules.

## Evidence labels

Every document uses these labels to avoid presenting a portfolio scenario as a real product launch:

- **Implemented**: behavior that exists in this repository and is covered by code or tests.
- **Assumption**: a belief that would need discovery evidence.
- **Target**: a desired outcome for a future pilot, not an achieved result.
- **Proposed**: future product or technical work that is not implemented.

## Case study map

| Document | Product question |
|---|---|
| [Product vision](product-vision.md) | Who is this for, what problem does it address, and what is deliberately out of scope? |
| [Personas and jobs to be done](personas-and-jtbd.md) | Which actors are involved and what progress are they trying to make? |
| [Discovery plan](discovery.md) | Which assumptions are riskiest and how would they be tested? |
| [MVP scope](mvp-scope.md) | What is implemented, what is missing, and why is the boundary credible? |
| [Prioritization](prioritization.md) | How are mandatory controls separated from value-ranked features? |
| [Metrics](metrics.md) | How would success and unintended consequences be measured? |
| [Roadmap](roadmap.md) | What is now, next, and later without turning the roadmap into a promise? |
| [Product decisions](product-decisions.md) | Which product/architecture trade-offs were made and why? |
| [Analytics plan](analytics-plan.md) | Which events and properties are needed to learn from a pilot safely? |

## Suggested reading paths

- **Product/recruiting review**: vision -> MVP scope -> metrics -> prioritization -> decisions.
- **Engineering/architecture review**: MVP scope -> decisions -> analytics plan -> root architecture documentation.

## Honest status

This is a working reference implementation, not a deployed terminal product. Automated tests and exercised API flows provide delivery evidence; product outcomes require real users, operational baselines, instrumentation, and a controlled pilot.
