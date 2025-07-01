# ChurchFlowAPI ⛪

A secure and modern ASP.NET Core Web API for managing a church's prayer schedules, reminders, and member participations.

---

## ✅ Features

- JWT Authentication with Identity
- Role-based access (Admin & Member)
- Manage Prayer Schedules
- Set and view Reminders
- Track Participation per schedule
- Authenticated users can access their own data only

---

## 🧩 Tech Stack

- ASP.NET Core Web API
- Entity Framework Core (Code First)
- SQL Server
- Identity for Auth
- Swagger for testing

---

## 📦 API Endpoints Summary

### Authentication

| Method | Endpoint             | Access        |
| ------ | -------------------- | ------------- |
| POST   | `/api/auth/login`    | Public        |
| POST   | `/api/auth/register` | Public        |
| GET    | `/api/auth/me`       | Authenticated |

### Prayer Schedules

| Method | Endpoint                          | Access        |
| ------ | --------------------------------- | ------------- |
| GET    | `/api/prayerschedule`             | All Users     |
| POST   | `/api/prayerschedule`             | Authenticated |
| GET    | `/api/prayerschedule/my-schedule` | Authenticated |
| GET    | `/api/prayerschedule/{id}`        | All Users     |

### Participations

| Method | Endpoint                              | Access        |
| ------ | ------------------------------------- | ------------- |
| POST   | `/api/participation`                  | Authenticated |
| GET    | `/api/participation/my-participation` | Authenticated |
| GET    | `/api/participation/user/{userId}`    | Admin Only    |
| GET    | `/api/participation/schedule/{id}`    | Admin Only    |

### Reminders

| Method | Endpoint                      | Access        |
| ------ | ----------------------------- | ------------- |
| POST   | `/api/reminder`               | Authenticated |
| GET    | `/api/reminder/my-reminder`   | Authenticated |
| GET    | `/api/reminder/schedule/{id}` | All Users     |

---

## Getting Started

1. Clone the repo  
   `git clone https://github.com/kholomojalefa/ChurchFlowAPI.git`

2. Create database in SQL Server

3. Run migrations  
   `dotnet ef database update`

4. Run the app  
   `dotnet run`

5. Visit Swagger  
   https://localhost:{PORT}/swagger

---

## Roles

- `ADMIN`: Can view all users' schedules, participations, and reminders
- `MEMBER`: Can only view their own

---

## Future Features

- Email or SMS reminders
- Admin dashboard UI

---

### Author

Built by Kholo Mojalefa  
South Africa 🇿🇦
