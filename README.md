# Netflex 🎬

Một nền tảng streaming giải trí hiện đại được xây dựng với .NET 8, cung cấp trải nghiệm xem phim và truyền hình trực tuyến toàn diện tương tự Netflix.

## 🚀 Tính năng chính

### 🎥 Quản lý nội dung

- **Phim**: Tạo, cập nhật, xóa và xem chi tiết phim với poster, backdrop, trailer
- **Series TV**: Quản lý series với nhiều tập phim (episodes)
- **Phân loại**: Tổ chức nội dung theo thể loại (genres), từ khóa (keywords)
- **Diễn viên**: Quản lý thông tin diễn viên và liên kết với nội dung
- **Tìm kiếm & Lọc**: Tìm kiếm nâng cao theo thể loại, diễn viên, quốc gia, năm phát hành

### 👤 Hệ thống người dùng

- **Xác thực**: Đăng ký/đăng nhập với email/password và OAuth (Google, Facebook)
- **Quản lý phiên**: JWT tokens với refresh token mechanism
- **Xác thực email**: OTP verification qua email
- **Phân quyền**: Role-based access control
- **Bảo mật**: Password hashing, session management, token blacklisting

### 🔔 Tính năng tương tác

- **Follow**: Theo dõi phim/series yêu thích
- **Review & Rating**: Đánh giá và bình luận nội dung
- **Thông báo**: Real-time notifications qua SignalR
- **Quản lý thông báo**: Đánh dấu đã đọc, đọc tất cả

### ☁️ Hạ tầng & Tích hợp

- **Cloud Storage**: Upload và quản lý media files
- **Email Service**: Gửi OTP và notifications
- **Real-time**: SignalR hubs cho notifications
- **Message Queue**: RabbitMQ cho background processing

## 🏗️ Kiến trúc hệ thống

Dự án được tổ chức theo Clean Architecture với các layer rõ ràng:

```
src/
├── Core/
│   ├── Netflex.Application/     # Use cases, DTOs, Interfaces
│   └── Netflex.Domain/          # Entities, Value Objects, Events
├── Infrastructure/
│   ├── Netflex.Infrastructure/  # External services, Settings
│   └── Netflex.Persistence/     # Data access, EF Core
├── Presentation/
│   └── Netflex.WebAPI/          # REST API endpoints, Middleware
└── Shared/
    └── Netflex.Shared/          # Common utilities, CQRS, Exceptions
```

### 🛠️ Công nghệ sử dụng

**Backend Framework:**

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core

**Architecture Patterns:**

- Clean Architecture
- CQRS (Command Query Responsibility Segregation)
- Domain-Driven Design (DDD)
- Repository Pattern
- Unit of Work Pattern

**Database & Caching:**

- PostgreSQL (Primary database)
- Redis (Caching & Sessions)

**Authentication & Security:**

- JWT Bearer Authentication
- OAuth 2.0 (Google, Facebook)
- Password hashing with BCrypt
- Role-based authorization

**Message Queue & Real-time:**

- RabbitMQ (Message broker)
- SignalR (Real-time notifications)
- MassTransit (Message bus)

**API & Documentation:**

- Carter (Minimal APIs)
- API Versioning
- FluentValidation
- Mapster (Object mapping)

**Development Tools:**

- Docker & Docker Compose
- Health Checks
- Structured Logging

## � Bắt đầu

### Yêu cầu hệ thống

- .NET 8.0 SDK
- Docker & Docker Compose
- PostgreSQL
- Redis
- RabbitMQ

### Cài đặt và chạy

1. **Clone repository:**

```bash
git clone https://github.com/thaildhe172591/netflex.git
cd netflex
```

2. **Chạy với Docker Compose:**

```bash
cd src
docker-compose up -d
```

3. **Chạy trong development:**

```bash
cd src
dotnet restore
dotnet build
dotnet run --project Presentation/Netflex.WebAPI
```

### Cấu hình

Cấu hình ứng dụng trong `appsettings.json` và `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=netflex;...",
    "Redis": "localhost:6379"
  },
  "JwtSettings": {
    "Key": "your-secret-key",
    "ExpiresInMinutes": 60,
    "Issuer": "Netflex",
    "Audience": "NetflexUsers"
  }
}
```

## 📚 API Documentation

API được tổ chức theo versioning với các endpoint chính:

### Authentication

- `POST /api/v1/auth/signin` - Đăng nhập
- `POST /api/v1/auth/refresh` - Refresh token
- `POST /api/v1/auth/logout` - Đăng xuất
- `GET /api/v1/auth/{provider}` - OAuth login
- `POST /api/v1/auth/confirm-email` - Xác thực email

### Movies & Series

- `GET /api/v1/movies` - Danh sách phim (có pagination, filter)
- `GET /api/v1/movies/{id}` - Chi tiết phim
- `POST /api/v1/movies` - Tạo phim mới
- `PUT /api/v1/movies/{id}` - Cập nhật phim

### User Interactions

- `POST /api/v1/users/follow` - Follow nội dung
- `POST /api/v1/users/review` - Đánh giá nội dung
- `GET /api/v1/users/notifications` - Danh sách thông báo

## 🤝 Đóng góp

1. Fork repository
2. Tạo feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Tạo Pull Request

## 📄 License

Dự án này được phân phối dưới MIT License. Xem file `LICENSE` để biết thêm chi tiết.

## 👨‍💻 Tác giả

**Lưu Danh Thái** - [thaildhe172591](https://github.com/thaildhe172591)

---

⭐ Nếu bạn thấy dự án này hữu ích, hãy star repository này!
