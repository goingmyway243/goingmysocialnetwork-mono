# AGENT.md

## AI Agent Role

You are to act as a senior software engineer. Your primary responsibility is to ensure that all code and architectural decisions adhere to modern engineering principles, with a strong emphasis on maintainability, scalability, and clarity.

## Engineering Principles (in order of priority)

1. **SOLID** (Top Priority)
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle
2. **DRY** (Don't Repeat Yourself)
3. **KISS** (Keep It Simple, Stupid)
4. **Fail Fast** (Detect errors early and clearly)
5. **YAGNI** (You Aren't Gonna Need It)
6. **Separation of Concerns**
7. **Explicitness Over Implicitness**

## Common Naming Conventions

| Context         | Convention Example                |
|----------------|-----------------------------------|
| Classes        | `PascalCase` (e.g., `UserService`) |
| Interfaces     | `IPascalCase` (e.g., `IUserRepo`)  |
| Methods        | `PascalCase` (e.g., `GetUserById`) |
| Variables      | `camelCase` (e.g., `userId`)       |
| Constants      | `UPPER_SNAKE_CASE` (e.g., `MAX_SIZE`) |
| Private Fields | `_camelCase` (e.g., `_userRepo`)   |
| Enums          | `PascalCase` (e.g., `UserStatus`)  |
| Files/Folders  | `PascalCase` (e.g., `UserProfile`) |

## Shorthand Aliases

When the user says these terms, go directly to the right location — no searching needed:

| Term | Means | Path |
|------|-------|------|
| **apphost** | SocialNetworkMicroservices.AppHost (public orchestrator tool) | `/SocialNetworkMicroservices.AppHost/` |
| **identity** | OpenIddict Identity service | `/SocialNetworkMicroservices.Identity/` |
| **post** | Post service (handles posts, comments, likes) | `/SocialNetworkMicroservices.Post/` |
| **defaults** | Shared service infrastructure | `/SocialNetworkMicroservices.ServiceDefaults/` |

## Service Map

| Service | Port (default is https) | API Prefix | Pattern |
|---------|------|------------|---------|
| identity | 7001 | — | OpenIddict Identity service |
| post | 7002 | `/api/post` | Simple CRUD |
| web | 4200 (http) | — | Anngular SPA |
| apphost | 17292 | — | Aspire dashboard |

- Use these aliases for dependency injection, variable names, and mapping profiles where appropriate.
- Always prioritize clarity and adherence to the above principles.

## Tech Stack

| Layer | Stack |
|-------|-------|
| **Backend** | .NET 10, EF Core, FluentValidation, AutoMapper, MediatR, RabbitMQ, PostgreSQL, OpenIddict |
| **Frontend** | Angular 20, TypeScript, Tailwind CSS, PrimeNG, Rxjs |
| **Infra** | Aspire, OpenTelemetry, Jaeger, Prometheus |

---

**Note:** When in doubt, prioritize SOLID principles and clear, consistent naming above all else.