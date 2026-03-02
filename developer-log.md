
# Developer Log

## AI Strategy
- Used AI to scaffold repository, service, and controller layers. Provided context including domain models, API rules, stock constraints, messaging requirements, and MongoDB integration to guide AI output.
- Leveraged AI to generate initial unit tests and boilerplate for RabbitMQ and MongoDB operations.
- Updated services to use async/await patterns for all I/O, including MongoDB and messaging calls.
- Added proper HTTP status codes for all endpoints: 200 OK, 201 Created, 400 BadRequest, 404 NotFound, 409 Conflict.
- Implemented business rules: blocking updates for shipped orders, enforcing stock constraints, validating product existence.

## Human Audits
- Replaced non-atomic in-memory stock updates with thread-safe atomic operations using locks.
- Added MongoDB repositories and wired them into services.
- Refined RabbitMQ publisher for testability; created RabbitPublisherFake for unit tests.
- Updated controllers to return appropriate HTTP status codes depending on outcomes.
- Refactored tests to work with new async methods and exception-based error signaling.
- Replaced generic Exception messages with domain-specific exceptions for missing products and insufficient stock.

## Verification
AI generated unit tests for:
- Preventing orders with negative quantities
- Correct atomic stock decrement in concurrent scenarios
- Event publishing verification via RabbitPublisherFake
- Idempotent order updates and shipped-order protection
- Async repository operations and proper exception handling for invalid orders
- Controller endpoints returning correct HTTP status codes