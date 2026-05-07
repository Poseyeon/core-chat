# Core Chat Backend (ASP.NET Core)

This is the SignalR-based chat backend for the Sentinel application.

## Prerequisites
- .NET 9 SDK
- MySQL Database

## Configuration
The application expects a `JWT_SECRET` for token validation. This must match the secret used by the NestJS users service.

For local development, the backend loads `.env` from the repository root automatically. A real environment variable still takes precedence when it is already set.

Database connection is configured in `appsettings.json`.

## Endpoints
- **SignalR Hub**: `/chatHub`
- **History**: `GET /api/messages/history/{otherUserId}`
- **Conversations**: `GET /api/messages/conversations`

## SignalR Events
- **Send**: `SendMessage(int receiverId, string content)`
- **Receive**: `ReceiveMessage(int senderId, string content, DateTime timestamp)`
- **Confirmation**: `MessageSent(ChatMessage message)`

## How to Run
```bash
cd core-chat
dotnet run
```
