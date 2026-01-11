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
