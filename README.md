# FinancialChat

## Overview

FinancialChat is a simple browser-based chat application built with **.NET** to demonstrate backend architecture, real-time communication, authentication, messaging, and integration with external APIs.

The application allows multiple authenticated users to chat in real time and retrieve stock quotes using a command-based bot powered by **RabbitMQ** and an external stock API.

This project was developed as part of a technical assessment focused on backend design, standards, and reusability.

---

## Features

### Mandatory Features ✅

- User registration and authentication  
- Real-time chat using SignalR  
- Multiple users chatting in the same chatroom  
- Command-based messages: `/stock=STOCK_CODE`  
- Decoupled bot that:
  - Consumes stock commands via RabbitMQ
  - Calls an external API (Stooq)
  - Parses CSV responses
  - Sends formatted stock quotes back to the chat
- Messages ordered by timestamp
- Only the last **50 messages** are returned to clients
- Bot messages are **not persisted**
- Unit tests included

### Bonus Features Implemented ⭐

- ASP.NET Core Identity for authentication
- Exception handling for invalid stock codes and API failures
- Bot message isolation via message broker (RabbitMQ)

---

## Architecture Overview

```text
Browser (HTML + JavaScript)
        |
        |  SignalR (WebSocket + JWT)
        v
ASP.NET Core API
        |
        |  RabbitMQ
        v
Stock Bot (BackgroundService)
        |
        |  HTTP
        v
Stooq API (CSV)

```

---

## Key Technologies

- .NET 8

- ASP.NET Core

- SignalR

- Entity Framework Core

- SQL Server

- RabbitMQ

- Docker

- JWT Authentication

- ASP.NET Identity

---

## Prerequisites

- .NET SDK 8+

- Docker

- SQL Server (local or containerized)

- Git
  
---

## RabbitMQ Setup (Required)

The bot depends on RabbitMQ. You must start it before running the application.

Run RabbitMQ using Docker

```
docker run -d \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management
```

---

## RabbitMQ Management UI

This project uses RabbitMQ as a message broker to decouple the chat application from the stock bot.

Accessing the Management UI

Once RabbitMQ is running via Docker, access:

```
http://localhost:15672
```

---

## Default Credentials:

- Username: guest

- Password: guest

## From the UI you can:

- Monitor queues and exchanges

- Inspect published and consumed messages

- Verify stock command processing

---

## Running the Application

### 1. Start RabbitMQ

Make sure RabbitMQ is running before starting the application.

If you started it using Docker, verify the container status:

```bash
docker ps
```

You should see a container named rabbitmq exposing ports 5672 and 15672.

---

## Database Setup

This project uses Entity Framework Core with SQL Server.

Before running the application for the first time, apply the database migrations.

From the solution root, run:
```
dotnet ef database update \
  --project FinancialChat.Infrastructure \
  --startup-project FinancialChat.Api
```

This will:

- Create ASP.NET Identity tables

- Create chat-related tables

- Apply relationships and constraints

- Seed the default chat room (General)

---

## Start the API

After RabbitMQ and the database are ready, start the API:
```
dotnet run --project FinancialChat.Api
```

The application will start at:
```
Base URL: https://localhost:7158

Swagger: https://localhost:7158/swagger
```

---

## Accessing the Chat
### Login Page

Open the login page in your browser:
```
https://localhost:7158/login.html
```

From there you can log in using existing credentials.

---

## Multiple Users Test

To test multiple users simultaneously (as required by the assignment):

- Open one browser window (e.g. Chrome)

- Open another window in Incognito / Private mode or a different browser (e.g. Edge)

- Log in with different users

- Each browser session represents a distinct authenticated user.

## Demo Accounts

To simplify testing, two demo users are already available:

User 1
```
{
  "userName": "admin",
  "password": "Admin@123"
}
```

User 2
```
{
  "userName": "admin2",
  "password": "Admin@123"
}
```

---

## Chat Behavior

The chat uses SignalR over WebSockets

Authentication is handled via JWT

On connection:

The last 50 messages are loaded

Messages are ordered from oldest to newest

Messages are delivered in real time to all connected users

Bot messages are displayed with the [Bot] label

---

## Sending Messages

Type a message in the input field and send it.

Regular messages are persisted in the database

Messages appear instantly for all connected users

---

## Stock Command

To request a stock quote, send a message using the following format:

```
/stock=stock_code
```
Example
```
/stock=aapl.us
```

---

## Stock Command Flow

1. User sends /stock=stock_code

2. The API detects the command

3. The command is not persisted

4. The message is published to RabbitMQ

5. The Stock Bot (BackgroundService) consumes the message

6. The bot calls the Stooq API

7. The CSV response is parsed

8. The bot sends a formatted message back to the chat

Example Bot Response
```
[Bot]: AAPL.US quote is $193.42 per share
```

---

## Error Handling

If:

- The stock code is invalid

- The external API fails

- The CSV response cannot be parsed

- The bot responds with a friendly error message instead of crashing.

---

## Notes for Reviewers

The application was tested using two concurrent users

RabbitMQ must be running for stock commands to work

Frontend simplicity is intentional — the backend is the focus
