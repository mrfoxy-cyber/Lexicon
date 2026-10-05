# Use Case: [Short, action-oriented name]

> Briefly describe the value this use case provides.

## Overview

| Field | Description |
|---|---|
| **ID** | UC-001 |
| **Status** | Draft / Ready / Implemented / Verified |
| **Priority** | Must / Should / Could |
| **Primary actor** | [Person or system that starts the use case] |
| **Supporting actors** | [Other people, services, or systems involved] |
| **Goal** | [What the primary actor wants to achieve] |
| **Trigger** | [Event or action that starts the use case] |

## Scope

### In scope

- [Behavior included in this use case]
- [Another included behavior]

### Out of scope

- [Related behavior intentionally excluded]

## Preconditions

- [Condition that must be true before the use case begins]
- [Required data or system state]

## Postconditions

### On success

- [State or data created or changed]
- [Result visible to the actor]

### On failure

- [State that must remain unchanged]
- [How the failure is recorded or communicated]

## Main flow

1. The actor [performs an action].
2. The system [validates or responds].
3. The actor [provides information or confirms].
4. The system [performs the business operation].
5. The system [stores and displays the result].

## Alternative flows

### A1 — [Alternative condition]

Begins at step [number] of the main flow.

1. The system [detects the alternative condition].
2. The actor [takes an alternative action].
3. The use case resumes at step [number] / ends.

### A2 — Actor cancels

1. The actor cancels before completion.
2. The system discards unsaved changes.
3. The use case ends.

## Error flows

### E1 — Invalid input

1. The system detects invalid input.
2. The system explains what is invalid.
3. The actor may correct the input and try again.

### E2 — Operation cannot be completed

1. The system cannot complete the requested operation.
2. The system does not save partial or invalid changes.
3. The system displays an actionable error message.

## Business rules

| ID | Rule |
|---|---|
| BR-01 | [Rule that must always be enforced] |
| BR-02 | [Validation, calculation, or restriction] |

## Acceptance criteria

### AC-01 — Successful completion

**Given** [the initial context or state]  
**And** [an additional precondition, if needed]  
**When** [the actor performs the relevant action]  
**Then** [the observable result occurs]  
**And** [the resulting state is correct]

### AC-02 — Invalid input

**Given** [the initial context]  
**When** [the actor provides invalid input]  
**Then** the operation is rejected  
**And** no invalid changes are saved  
**And** the actor receives a clear error message

### AC-03 — Boundary condition

**Given** [a minimum, maximum, empty, or other boundary state]  
**When** [the actor performs the action]  
**Then** [the expected boundary behavior occurs]

### AC-04 — Cancellation

**Given** the use case has started but is not complete  
**When** the actor cancels  
**Then** no incomplete changes are saved  
**And** the actor returns to [the appropriate screen or menu]

## Data and validation

| Field | Required | Validation | Error message |
|---|---:|---|---|
| [Field name] | Yes / No | [Validation rule] | [User-facing message] |
| [Field name] | Yes / No | [Validation rule] | [User-facing message] |

## Non-functional requirements

- **Usability:** [Expected user experience]
- **Performance:** [Required response time, if relevant]
- **Reliability:** [Expected behavior when an operation fails]
- **Security:** [Authorization or sensitive-data requirements]

## Implementation notes

> Optional technical guidance. Avoid placing business requirements only in this section.

- **Domain classes:** `[ClassName]`, `[ClassName]`
- **Use-case/service class:** `[UseCaseName]`
- **Input:** `[Request or command type]`
- **Output:** `[Result type]`
- **Dependencies:** `[Repository or external service]`

## Test coverage

| Test | Acceptance criterion | Expected result |
|---|---|---|
| `[MethodName_WhenCondition_ExpectedResult]` | AC-01 | [Expected result] |
| `[MethodName_WhenInvalid_ThrowsOrReturnsError]` | AC-02 | [Expected result] |
| `[MethodName_WhenBoundary_ExpectedResult]` | AC-03 | [Expected result] |

## Open questions

- [Question that must be answered before implementation]

## Definition of done

- [ ] All acceptance criteria pass.
- [ ] Main, alternative, and error flows are covered by tests.
- [ ] Business rules are implemented in the domain or application layer.
- [ ] Invalid input cannot create an invalid state.
- [ ] User-facing messages are clear.
- [ ] The solution builds without errors.
- [ ] All automated tests pass.
