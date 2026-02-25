
# Developer Log

## AI Strategy
Used AI to scaffold repository, service, and controller layers. Provided context including domain models, API rules, stock constraints, and messaging requirements to guide AI output. Leveraged AI to generate initial unit tests and boilerplate for MongoDB and RabbitMQ integration.

## Human Audits
1. Replaced non-atomic in-memory stock updates with thread-safe (atomic) operations using locks.
2. Added business logic to block updates for orders with status "Shipped".
3. Refined RabbitMQ publisher to allow testability without requiring live RabbitMQ connection.

## Verification
AI generated unit tests for:
- Preventing orders with negative quantities
- Correct atomic stock decrement
- Event publishing verification
- Idempotent order updates and shipped-order protection
