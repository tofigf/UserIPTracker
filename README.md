# **UserIPTracker - High-Performance User Connection Tracking System**  
[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/en-us/)  
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.3-blue)](https://www.postgresql.org/)  
[![Kafka](https://img.shields.io/badge/Kafka-Event--Driven-orange)](https://kafka.apache.org/)  
[![Redis](https://img.shields.io/badge/Redis-Caching-red)](https://redis.io/)  
[![Docker](https://img.shields.io/badge/Docker-Containerization-blue)](https://www.docker.com/)  
[![Serilog](https://img.shields.io/badge/Logging-Serilog-green)](https://serilog.net/)  

📌 **UserIPTracker** is a **scalable and high-performance API** for tracking **user connections and IP addresses**, built with **.NET 8, PostgreSQL, Kafka, Redis, and Docker**.  

---

## **🚀 Features**
✔ **Event-driven architecture** using **Kafka** for handling massive user connections.  
✔ **Fast search & caching** with **Redis**, reducing database load.  
✔ **Optimized PostgreSQL storage** with `inet` data type and indexed queries.  
✔ **High availability** with **Dockerized services**.  
✔ **Logging & Monitoring** via **Serilog** for better observability.  
✔ **Unit & Integration Testing** with **xUnit & Moq**.  

---

## **📌 Technologies Used & Why?**
| **Technology**  | **Purpose** |
|---------------|-------------|
| **.NET 8 (C#)** | Backend API with high performance & modern architecture. |
| **PostgreSQL 16** | Stores user connections with an optimized `inet` column for IPs. |
| **Kafka** | Handles event-driven user connection processing asynchronously. |
| **Redis** | Caches user search results & last connection queries for fast performance. |
| **Entity Framework Core** | ORM for PostgreSQL, making DB operations efficient. |
| **Docker & Docker Compose** | Containerization for easy deployment. |
| **Serilog** | Logging to console, file, and future database integration. |
| **xUnit & Moq** | Unit testing framework for ensuring code reliability. |

---

## **📌 How to Run the Project**
### **🔹 1️⃣ Clone the Repository**
```sh
git clone https://github.com/yourusername/UserIPTracker.git
cd UserIPTracker
docker-compose up -d
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef database update --project Infrastructure --startup-project API
dotnet run --project API













