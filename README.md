# \# FinancialChat

# 

# \## Overview

# 

# FinancialChat is a simple browser-based chat application built with \*\*.NET\*\* to demonstrate backend architecture, real-time communication, authentication, messaging, and integration with external APIs.

# 

# The application allows multiple authenticated users to chat in real time and retrieve stock quotes using a command-based bot powered by \*\*RabbitMQ\*\* and an external stock API.

# 

# This project was developed as part of a technical assessment focused on backend design, standards, and reusability.

# 

# ---

# 

# \## Features

# 

# \### Mandatory Features ✅

# 

# \- User registration and authentication

# \- Real-time chat using SignalR

# \- Multiple users chatting in the same chatroom

\- Command-based messages: /stock=STOCK\_CODE
- Decoupled bot that:
===

# \- Consumes stock commands via RabbitMQ

# \- Calls an external API (Stooq)

# \- Parses CSV responses

# \- Sends formatted stock quotes back to the chat

# \- Messages ordered by timestamp

# \- Only the last \*\*50 messages\*\* are returned to clients

# \- Bot messages are \*\*not persisted\*\*

# \- Unit tests included

# 

# \### Bonus Features Implemented ⭐

# 

# \- .NET Identity for authentication

# \- Exception handling for invalid stock codes and API failures

# \- Bot message isolation via message broker (RabbitMQ)

# 

# ---

# 

# ```text

# Browser (HTML + JavaScript)

# &nbsp;       |

# &nbsp;       |  SignalR (WebSocket + JWT)

# &nbsp;       v

# ASP.NET Core API

# &nbsp;       |

# &nbsp;       |  RabbitMQ

# &nbsp;       v

# Stock Bot (BackgroundService)

# &nbsp;       |

# &nbsp;       |  HTTP

# &nbsp;       v

Stooq API (CSV)


===

# \### Key Technologies

# 

# \- \*\*.NET 8\*\*

# \- \*\*ASP.NET Core\*\*

# \- \*\*SignalR\*\*

# \- \*\*Entity Framework Core\*\*

# \- \*\*SQL Server\*\*

# \- \*\*RabbitMQ\*\*

# \- \*\*Docker\*\*

# \- \*\*JWT Authentication\*\*

# \- \*\*.NET Identity\*\*

# 

# ---

# 

# \## Prerequisites

# 

# \- .NET SDK 8+

# \- Docker

# \- SQL Server (local or containerized)

# \- Git

# 

# ---

# 

# \## RabbitMQ Setup (Required)

# 

# The bot depends on RabbitMQ. You must start it before running the application.

# 

# \### Run RabbitMQ using Docker

# 

# ```bash

# docker run -d \\

# &nbsp; --name rabbitmq \\

# &nbsp; -p 5672:5672 \\

# &nbsp; -p 15672:15672 \\

&nbsp; rabbitmq:3-management

## RabbitMQ Management UI
===

# 

# This project uses \*\*RabbitMQ\*\* as a message broker to decouple the chat application from the stock bot.

# 

# \### Accessing the Management UI

# 

# Once RabbitMQ is running via Docker, you can access the management interface at: http://localhost:15672

# 

# 

# \*\*Default credentials:\*\*

# 

# \- \*\*Username:\*\* guest  

# \- \*\*Password:\*\* guest

# 

# From the UI you can:

# \- Monitor queues and exchanges

# \- Inspect published and consumed messages

# \- Verify that the stock command messages are being processed correctly by the bot

# 

# ---

# 

# \## Running the Application

# 

# \### 1. Prerequisites

# 

# Make sure you have the following installed:

# 

# \- .NET 8 SDK

# \- Docker

# \- SQL Server (LocalDB or full instance)

# \- A modern browser (Chrome, Edge, Firefox)

# 

# ---

# 

# \### 2. Start RabbitMQ

# 

# Run RabbitMQ with management enabled using Docker:

# 

# ```bash

# docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management


## Verify it is running
===

# 

# After starting RabbitMQ with Docker, make sure the container is up and running:

# 

# ```bash

# docker ps


You should see a container named \*\*rabbitmq\*\* exposing ports \*\*5672\*\* and \*\*15672\*\*.
===

# 

# ---

# 

# \## Database Setup

# 

# This project uses \*\*Entity Framework Core\*\* with \*\*SQL Server\*\*.

# 

# Before running the application for the first time, apply the database migrations.

# 

# From the solution root, run:

# 

# ```bash

# dotnet ef database update \\

# &nbsp; --project FinancialChat.Infrastructure \\

# &nbsp; --startup-project FinancialChat.Api


### This will:
===


Create ASP.NET Identity tables
===

# 

# Create chat-related tables

# 

# Apply relationships and constraints

# 

Seed the default chat room (General)

## Running the Application

After RabbitMQ and the database are ready, start the API:
===


Running the Application
===

# -----------------------

# 

# After RabbitMQ and the database are ready, start the API:

# 

# `   dotnet run --project FinancialChat.Api   `

# 

# The application will start at:

# 

# \*   \*\*Base URL:\*\* \[https://localhost:7158](https://localhost:7158)

# &nbsp;   

# \*   \*\*Swagger:\*\* https://localhost:7158/swagger

# &nbsp;   

# 

# Accessing the Chat

# ------------------

# 

# \### Login Page

# 

# Open the login page in your browser:

# 

# `   https://localhost:7158/login.html   `

# 

# From there you can:

# 

# &nbsp;   

# \*   Log in using existing credentials

# &nbsp;   

# 

# \### Multiple Users Test

# 

# To test multiple users simultaneously (as required by the assignment):

# 

# \*   Open one browser window (e.g. Chrome)

# &nbsp;   

# \*   Open another browser window in \*\*Incognito / Private mode\*\* or a different browser (e.g. Edge)

# &nbsp;   

# \*   Log in with \*\*different users\*\*

# &nbsp;   

# 

# Each browser session represents a distinct authenticated user.

# To make testing easier, two demo users are already available.

### Demo Accounts

**User 1**
```json
{
  "userName": "admin",
  "email": "admin@financialchat.com",
  "password": "Admin@123"
}

**User 2**
```json
{
  "userName": "admin2",
  "email": "admin2@financialchat.com",
  "password": "Admin@123"
}

# 

# Chat Behavior

# -------------

# 

# \*   The chat uses \*\*SignalR over WebSockets\*\*

# &nbsp;   

# \*   Authentication is done using \*\*JWT\*\*

# &nbsp;   

# \*   On connection:

# &nbsp;   

# &nbsp;   \*   The last \*\*50 messages\*\* are loaded

# &nbsp;       

# &nbsp;   \*   Messages are ordered by timestamp (oldest → newest)

# &nbsp;       

# \*   Messages are delivered in real time to all connected users

# &nbsp;   

# \*   Bot messages are displayed with the \\\[Bot\\] label

# &nbsp;   

# 

# Sending Messages

# ----------------

# 

# Type a message in the input field and send it.

# 

# \*   Regular messages are persisted in the database

# &nbsp;   

# \*   Messages appear instantly for all connected users

# &nbsp;   

# 

# Stock Command

# -------------

# 

# To request a stock quote, send a message using the following format:

# 

# `   /stock=stock\_code   `

# 

# \### Example

# 

# `   /stock=aapl.us   `

# 

# Stock Command Flow

# ------------------

# 

# 1\.  User sends /stock=stock\\\_code

# &nbsp;   

# 2\.  API detects the command

# &nbsp;   

# 3\.  The command is \*\*not persisted\*\*

# &nbsp;   

# 4\.  The command is published to \*\*RabbitMQ\*\*

# &nbsp;   

# 5\.  The Stock Bot (BackgroundService) consumes the message

# &nbsp;   

# 6\.  The bot calls the \*\*Stooq API\*\*

# &nbsp;   

# 7\.  CSV response is parsed

# &nbsp;   

# 8\.  The bot sends a formatted message back to the chat

# &nbsp;   

# 

# \### Example Bot Response

# 

# `   \[Bot]: AAPL.US quote is $193.42 per share   `

# 

# \### Error Handling

# 

# If:

# 

# \*   The stock code is invalid

# &nbsp;   

# \*   The API fails

# &nbsp;   

# \*   The CSV response cannot be parsed

# &nbsp;   

# 

# The bot responds with a friendly error message instead of crashing.

# 

# Security

# --------

# 

# \*   Authentication handled by \*\*ASP.NET Core Identity\*\*

# &nbsp;   

# \*   Authorization via \*\*JWT Bearer tokens\*\*

# &nbsp;   

# \*   JWT tokens are attached to SignalR connections

# &nbsp;   

# \*   No secrets are hardcoded

# &nbsp;   

# \*   All sensitive configuration is stored in configuration files

# &nbsp;   

# 

# Performance Considerations

# --------------------------

# 

# \*   Only the last \*\*50 messages\*\* are loaded on connect

# &nbsp;   

# \*   Bot processing is fully decoupled via RabbitMQ

# &nbsp;   

# \*   SignalR connections are lightweight

# &nbsp;   

# \*   No polling — real-time push via WebSockets

# &nbsp;   

# 

# Testing

# -------

# 

# \*   Unit tests were implemented for selected components

# &nbsp;   

# \*   Focus on business logic and message handling

# &nbsp;   

# \*   Frontend intentionally kept minimal to emphasize backend design

# &nbsp;   

# 

# Notes for Reviewers

# -------------------

# 

# \*   The application was tested using two concurrent users

# &nbsp;   

# \*   RabbitMQ \*\*must\*\* be running for stock commands to work

# &nbsp;   

# \*   If RabbitMQ is unavailable, the application fails fast

# &nbsp;   

# \*   Frontend simplicity is intentional — backend is the focus

# &nbsp;   

# 

# Bonus Features Summary

# ----------------------

# 

# \*   ✅ ASP.NET Core Identity

# &nbsp;   

# \*   ✅ RabbitMQ-based bot decoupling

# &nbsp;   

# \*   ✅ Exception handling in bot

# &nbsp;   

# \*   ❌ Multiple chat rooms (not implemented)

# &nbsp;   

# 

# Delivery Notes

# --------------

# 

# \*   The project is fully versioned with Git

# &nbsp;   

# \*   The .git folder should be included if delivered as a ZIP

# &nbsp;   

# \*   If any part could not be completed, it has been clearly documented

# &nbsp;   

# 

# Conclusion

# ----------

# 

# This project demonstrates:

# 

# \*   Real-time communication with SignalR

# &nbsp;   

# \*   Secure authentication and authorization

# &nbsp;   

# \*   Asynchronous processing using RabbitMQ

# &nbsp;   

# \*   Clean architecture and separation of concerns

# &nbsp;   

# \*   Production-oriented backend design in .NET






