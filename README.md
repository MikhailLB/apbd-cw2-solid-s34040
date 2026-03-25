# Equipment Rental System

Console application for managing a university equipment rental system.
Supports registration of equipment and users, borrowing, returning, availability control, and reporting.

## How to Run

```bash
dotnet run --project ConsoleApp3
```

The application offers two modes:
1. **Demo scenario** — automatically executes a predefined sequence demonstrating all system features.
2. **Interactive menu** — allows manual execution of all operations.

## Project Structure

```
ConsoleApp3/
├── Models/           — Domain objects
├── Repositories/     — In-memory data storage
├── Services/         — Business logic
├── UI/               — Console interface
└── Program.cs        — Composition root
```

## Design Decisions

### Layer Separation

The project is split into four layers with clear responsibilities:

- **Models** contain only data, identity, and polymorphic contracts. They have no dependencies on other layers.
- **Repositories** encapsulate collection storage and querying. They isolate the rest of the code from storage details.
- **Services** hold all business logic: borrowing rules, return processing, penalty calculation, and report generation.
- **UI** handles user interaction exclusively; it delegates every operation to the appropriate service.

`Program.cs` serves as the composition root — it creates all dependencies and wires them together.

### Cohesion

Each class has a single, focused responsibility:

- `RentalService` — borrowing and returning equipment, enforcing rental limits.
- `EquipmentService` — adding equipment and changing its availability status.
- `ReportService` — generating a summary report from current system state.
- `PenaltyCalculator` — computing late-return penalties based on a per-day rate.
- `ConsoleMenu` — reading user input and displaying results; contains no business logic.

### Coupling

- The UI layer depends only on services, never directly on repositories.
- Services receive their dependencies through constructor parameters.
- `PenaltyCalculator` is injected into `RentalService` via the `IPenaltyCalculator` interface, so the penalty formula can be replaced without modifying the rental logic.
- Models have zero dependencies on other layers.

### Inheritance

Inheritance is used where the domain genuinely requires it:

- `Equipment` is an abstract base for `Laptop`, `Projector`, and `Camera`. Each subtype adds at least two specific fields (e.g., RAM and processor for Laptop). The shared `GetDetails()` method is overridden polymorphically.
- `User` is an abstract base for `Student` and `Employee`. Each subtype defines its own `MaxActiveRentals` limit and `UserType` label.

No inheritance is used among services or repositories — they use composition instead.

### Error Handling

Operations that can fail (borrowing, returning, marking as unavailable) return `OperationResult` instead of throwing exceptions. This makes success and failure explicit at the call site and avoids using exceptions for expected business rule violations.

### Business Rules Configuration

All configurable rules are defined in a single, easy-to-find location:

| Rule | Location | Value |
|---|---|---|
| Student rental limit | `Student.MaxActiveRentals` | 2 |
| Employee rental limit | `Employee.MaxActiveRentals` | 5 |
| Late penalty rate | `PenaltyCalculator.PenaltyPerDay` | 10.00 per day |
| Default rental period | `RentalService.DefaultRentalDays` | 14 days |

### Interface Usage

`IPenaltyCalculator` is the key interface in the project. It decouples the penalty calculation strategy from the rental service. A different penalty formula (e.g., progressive rates) can be implemented by creating a new class that implements this interface — no changes to `RentalService` required.

Abstract classes `Equipment` and `User` define contracts for their respective hierarchies (`GetDetails()`, `MaxActiveRentals`, `UserType`).
