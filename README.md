# 🐾 Meowly — A Cozy Messenger

Meowly is a cozy and minimalistic messenger inspired by the atmosphere of sleepy nights and purring cats.
It is a university project built with microservices architecture using ASP.NET Core and Blazor WebAssembly.

---

## Features

- User registration and authentication
- Chat creation and listing
- Sending, editing and deleting messages
- User search by tag
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

Each service has its own controller, DTOs, and logic. They communicate via HTTP requests and are decoupled.

---

## Screenshots

![Login Page](https://github.com/user-attachments/assets/2dddab88-aa9e-4781-bcda-d2368ed863ab)
![Main Page](https://github.com/user-attachments/assets/c907c1cb-0b7d-4f76-b0fe-4f7793ef0c2d)
![Chat](https://github.com/user-attachments/assets/8be3d7bd-2447-4fdd-8c40-d8c320e70949)
![User Search](https://github.com/user-attachments/assets/21feb21e-de1c-417a-b978-d75f6b70725d)

---
