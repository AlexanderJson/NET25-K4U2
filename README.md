# AI Content Assistant

An AI-powered note-taking and learning-path generation platform built with **ASP.NET Core (.NET 10)**. The solution is split into two cooperating microservices:

- **Content API** — manages users, notebooks, topics, and notes.
- **LLM Proxy API** — wraps the Google Gemini API to generate AI-driven learning paths.

When a user requests AI-generated topics for a notebook, the Content API forwards a structured prompt to the LLM Proxy API, which calls Google Gemini and returns the result.

---

## Table of Contents

- [Architecture Overview](#architecture-overview)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Configuration](#configuration)
  - [Running the Services](#running-the-services)
  - [API Documentation](#api-documentation)
- [API Endpoints](#api-endpoints)
  - [Content API](#content-api-endpoints)
  - [LLM Proxy API](#llm-proxy-api-endpoints)
- [Domain Model](#domain-model)
- [Key Design Patterns](#key-design-patterns)
- [Docker](#docker)

---

## Architecture Overview

```
┌──────────────┐        HTTP         ┌──────────────────┐       HTTPS       ┌─────────────────┐
│              │  ───────────────►   │                  │  ──────────────►  │                 │
│  Content API │   localhost:5002    │  LLM Proxy API   │                   │  Google Gemini  │
│  (port 5001) │  ◄───────────────  │  (port 5002)     │  ◄──────────────  │  API            │
│              │                    │                  │                   │                 │
└──────────────┘                    └──────────────────┘                   └─────────────────┘
       │
       │  EF Core
       ▼
  ┌──────────┐
  │ InMemory │
  │    DB    │
  └──────────┘
```

1. **Content API** — handles all CRUD operations, business logic, and prompt construction.
2. **LLM Proxy API** — receives a raw prompt, forwards it to `Gemini 3 Flash Preview`, and returns the generated text.
3. **InMemory Database** — used by EF Core during development (easily swappable to SQLite or SQL Server).

---

## Tech Stack

| Layer             | Technology                                       |
| ----------------- | ------------------------------------------------ |
| Framework         | ASP.NET Core / .NET 10                           |
| ORM               | Entity Framework Core 10 (InMemory)              |
| Authentication    | JWT Bearer (ASP.NET Core Authentication)         |
| Password Hashing  | BCrypt.Net-Next 4.1                              |
| AI Integration    | Google Gemini API (`Google.GenAI` 1.6)           |
| API Documentation | OpenAPI + Scalar                                 |
| API Versioning    | `Microsoft.AspNetCore.Mvc.Versioning` 5.1        |

---

## Project Structure

```
AiAssistant/
├── AiContentAssistant.slnx           # Solution file
│
├── README.md                          
│
├── ContentApi/                        # Main content-management API
│   ├── Program.cs                     # App startup & DI configuration
│   │
│   ├── Api/
│   │   ├── Clients/                   # HTTP clients for inter-service calls
│   │   │   ├── ILlmClient.cs          # Client interface
│   │   │   ├── LlmClient.cs           # Typed HttpClient → LLM Proxy
│   │   │   ├── PromptBuilder.cs        # ITopicPromptBuilder implementation
│   │   │   └── GenerateTopicsProxyRequest.cs
│   │   └── Options/
│   │       └── LlmPromptOptions.cs    # Strongly-typed prompt configuration
│   │
│   ├── Common/
│   │   ├── Guard.cs                   # Guard clause utility (CallerArgumentExpression)
│   │  
│   │
│   ├── Controllers/
│   │   ├── NotebooksController.cs     # Notebook CRUD + topic attachment
│   │   ├── TopicsController.cs        # AI topic generation endpoint
│   │   └── UsersController.cs         # User CRUD
│   │
│   ├── DTO/                           # Request / Response DTOs
│   │   ├── Note/
│   │   ├── Notebook/
│   │   ├── Topics/
│   │   └── User/
│   │
│   ├── Infrastructure/
│   │   ├── Data/
│   │   │   └── AppDbContext.cs         # EF Core DbContext +  API config
│   │   └── Repositories/
│   │       ├── Crud/                   # Generic CRUD base
│   │       ├── Notebook/              # INotebookRepository, NotebookRepository
│   │       ├── Topic/                 # ITopicRepository, TopicRepository
│   │       ├── Note/                  # INoteRepository, NoteRepository
│   │       └── User/                  # IUserRepository, UserRepository
│   │
│   ├── Jwt/
│   │   └── UserContext.cs             # JWT claim extraction helper
│   │
│   ├── Models/                        # Domain entities
│   │   ├── User.cs
│   │   ├── Notebook.cs
│   │   ├── Topic.cs
│   │   └── Note.cs
│   │
│   ├── Projection/
│   │   ├── IProjection.cs             # Generic projection interface
│   │   └── QueryExtensions.cs         # LINQ Select extension methods
│   │
│   ├── Services/
│   │   ├── Crud/                      # Generic service base
│   │   ├── Notebook/                  # INotebookService, INotebookQueries
│   │   ├── Topic/                     # ITopicService, ITopicQueries
│   │   ├── User/                      # IUserService, IUserQueries
│   │   ├── Project/                   # Project-level services
│   │   └── NotebookWorkflow/
│   │       └── NotebookWorkflowService.cs  # AI topic generation 
│   │
│   ├── appsettings.json               # Base configuration (LLM prompt templates)
│   └── appsettings.Development.json   # Dev overrides
│
└── LlmProxyApi/                       # Gemini proxy microservice
    ├── Program.cs                     # App startup & DI configuration
    ├── Controller/
    │   └── GeminiController.cs        # POST api/proxy endpoint
    ├── DTO/
    │   ├── GeminiRequest.cs           # { Prompt }
    │   └── GeminiResponse.cs          # { Result }
    ├── Service/
    │   ├── GeminiService.cs           # Gemini API integration
    │   └── IGemini.cs                 # Service interface
    ├── appsettings.json
    └── appsettings.Development.json   # Gemini API key config
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A **Google Gemini API key** (for AI topic generation)

### Configuration

1. **Set your Gemini API key** in the LLM Proxy API:

   Edit `LlmProxyApi/appsettings.Development.json`:

   ```json
   {
     "Gemini": {
       "ApiKey": "YOUR_GEMINI_API_KEY"
     }
   }
   ```

2. **LLM prompt configuration** can be customized in `ContentApi/appsettings.json` (or the `Development` override) under the `LlmPrompts` section:

   ```json
   {
     "LlmPrompts": {
       "GenerateTopics": {
         "Instructions": "Generate a logical learning path of topic titles.",
         "OutputRules": ["Return ONLY valid JSON", "..."],
         "OutputFormat": "Return a JSON array of strings...",
         "OutputExample": "[\"Introduction to C#\", \"Variables and Data Types\"]",
         "MinTopics": 5,
         "MaxTopics": 10
       }
     }
   }
   ```

### Running the Services

Both services need to be running simultaneously. Open two terminals:

**Terminal 1 — LLM Proxy API** (must start first):

```bash
cd LlmProxyApi
dotnet run
```

> Runs on `http://localhost:5002` by default.

**Terminal 2 — Content API:**

```bash
cd ContentApi
dotnet run
```

> Runs on `http://localhost:5001` by default.

### API Documentation

Both services expose interactive API docs via **Scalar** in development mode:

| Service       | URL                                  |
| ------------- | ------------------------------------ |
| Content API   | `http://localhost:5001/scalar/v1`    |
| LLM Proxy API | `http://localhost:5002/scalar/v1`    |

---

## API Endpoints

### Content API Endpoints

#### Users

| Method   | Route                                         | Description              |
| -------- | --------------------------------------------- | ------------------------ |
| `GET`    | `/api/v1/users/{id}`                          | Get user by ID           |
| `GET`    | `/api/v1/users/search?searchTerm=...`         | Search users by username |
| `POST`   | `/api/v1/users`                               | Create a new user        |
| `PATCH`  | `/api/v1/users/{id}`                          | Update a user            |
| `DELETE` | `/api/v1/users/{id}`                          | Delete a user            |

#### Notebooks

| Method   | Route                                         | Description                          |
| -------- | --------------------------------------------- | ------------------------------------ |
| `GET`    | `/api/v1/notebooks/{id}`                      | Get full notebook (with topics)      |
| `GET`    | `/api/v1/notebooks/search?title=...`          | Search notebooks by title            |
| `GET`    | `/api/v1/notebooks/user/{userId}/overview`    | Get notebook overview for a user     |
| `POST`   | `/api/v1/notebooks`                           | Create a new notebook                |
| `PATCH`  | `/api/v1/notebooks/{id}`                      | Update a notebook                    |
| `DELETE` | `/api/v1/notebooks/{id}`                      | Delete a notebook                    |
| `POST`   | `/api/v1/notebooks/{id}/topics`               | Attach topics to a notebook          |

#### Topics (AI Generation)

| Method | Route                          | Description                                                 |
| ------ | ------------------------------ | ----------------------------------------------------------- |
| `POST` | `/api/v1/topics/generate`      | AI-generate a learning path of topics for a given notebook  |

**Request body:**
```json
{
  "notebookId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

**Response:** an array of created topic GUIDs.

### LLM Proxy API Endpoints

| Method | Route         | Description                                         |
| ------ | ------------- | --------------------------------------------------- |
| `POST` | `/api/proxy`  | Send a prompt to Gemini and receive generated text   |

**Request body:**
```json
{
  "prompt": "Your prompt text here"
}
```

**Response:**
```json
{
  "result": "Generated text from Gemini"
}
```

---

## Domain Model

```
User 1──* Notebook 1──* Topic 1──* Note
```

| Entity       | Key Properties                                                    |
| ------------ | ----------------------------------------------------------------- |
| **User**     | `Id`, `Username`, `HashedPassword`, `CreatedAt`                   |
| **Notebook** | `Id`, `Title`, `Category`, `UserId`, `CreatedAt`, `LastUpdated`, `RowVersion` |
| **Topic**    | `Id`, `Title`, `Order`, `IsCompleted`, `NotebookId`, `CreatedAt`, `RowVersion` |
| **Note**     | `Id`, `EncryptedContent`, `TopicId`, `CreatedAt`, `LastUpdated`, `RowVersion` |

**Design highlights:**

- **Optimistic concurrency** — all mutable entities carry a `RowVersion` column.
- **Encapsulated collections** — `Notebook.Topics` and `Topic.Notes` are exposed as `IReadOnlyCollection<T>` backed by private `List<T>`.
- **Rich domain model** — entities enforce invariants via constructors and guard clauses (no public setters on Notebook/Topic).

---

## Key Design Patterns

| Pattern                   | Where & How                                                                                              |
| ------------------------- | -------------------------------------------------------------------------------------------------------- |
| **Service / Repository**  | Each domain entity has a dedicated `IXxxRepository` and `IXxxService` / `IXxxQueries` pair.              |
| **Typed HTTP Clients**    | `ILlmClient` / `LlmClient` uses `IHttpClientFactory` for resilient calls to the Proxy.                   |
| **Options Pattern**       | `LlmPromptOptions` (instructions, rules, format, example, min/max topics) bound from `appsettings.json`. |
| **Projection Pattern**    | `IProjection<TEntity, TDto>` with static abstract `Expression<Func<TEntity, TDto>> Selector` for efficient EF Core queries. |
| **Guard Clauses**         | `Guard.Against` utility with `CallerArgumentExpression` for expressive argument validation.               |
| **Workflow / Orchestrator** | `NotebookWorkflowService` coordinates prompt building → LLM call → topic parsing → persistence.        |
| **API Versioning**        | URL-segment versioning (`/api/v1/...`) via `Microsoft.AspNetCore.Mvc.Versioning`.                        |

---

