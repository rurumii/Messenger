# 🐾 Meowly — A Cozy Messenger App

Meowly is a minimalistic and cozy messenger inspired by Telegram, created as university project. It is built with ASP.Net Core and Blazor WebAssembly using a microservices-based architecture.

---

## Features

- User registration and authentication
- Chat creation and listing
- Sending, editing and deleting messages
- User search by unique tag
- Dark theme and soft interface

---

## Technologies

- **Backend**: ASP.NET Core, Entity Framework Core, AutoMapper, REST API
- **Frontend**: Blazor WebAssembly
- **Database**: MS SQL Server
- **Authentication**: JWT tokens
- **Communication**: RESTful HTTP between microservices

---

## Architecture Overview

The project is split into two microservices:
- `UserService`: handles user registration, login, and identity management
- `MessageService`: manages chats and message exchange
  
Each service has it own controllers, DTOs. They communicate via HTTP requests and are decoupled.

---
## In Development
Currently working on:
- Pinning chats and messages
- Basic profile editing (nickname, status, profile picture..)
- Light/Dark theme switching
- Group chats, channels

---

## Screenshots

![Login Page](https://github.com/user-attachments/assets/2dddab88-aa9e-4781-bcda-d2368ed863ab)
![Main Page](https://github.com/user-attachments/assets/c907c1cb-0b7d-4f76-b0fe-4f7793ef0c2d)
![Chat](https://github.com/user-attachments/assets/8be3d7bd-2447-4fdd-8c40-d8c320e70949)
![User Search](https://github.com/user-attachments/assets/21feb21e-de1c-417a-b978-d75f6b70725d)

---
