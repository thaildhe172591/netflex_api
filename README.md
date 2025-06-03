# Netflex

## Hướng dẫn cài đặt ban đầu

Để thiết lập và chạy dự án Netflex, hãy làm theo các bước sau:

### Bước 1: Cấu hình biến môi trường

1.  Tạo một tệp mới có tên `.env` trong thư mục `src/` của dự án.
2.  Sao chép nội dung từ tệp `src/.env.sample` vào tệp `src/.env` vừa tạo.
3.  Điền các giá trị thích hợp cho tất cả các biến môi trường trong tệp `src/.env`. Đảm bảo thay thế các placeholder như `YOUR_DATABASE_CONNECTION_STRING_HERE`, `YOUR_CACHE_CONNECTION_STRING_HERE`, `YOUR_JWT_ISSUER_HERE`, v.v., bằng thông tin cấu hình thực tế của bạn.

    Ví dụ về các biến cần cấu hình:
    *   `ConnectionStrings__Database`
    *   `ConnectionStrings__Cache`
    *   `JwtConfig__Issuer`
    *   `JwtConfig__Audience`
    *   `JwtConfig__Key` (phải có ít nhất 32 ký tự)
    *   `GoogleConfig__ClientId`
    *   `GoogleConfig__ClientSecret`
    *   `EmailConfig__Username`
    *   `EmailConfig__Password`

### Bước 2: Chuyển đổi biến môi trường sang User Secrets

1.  Mở Git Bash (hoặc một terminal tương thích với Bash).
2.  Điều hướng đến thư mục `src/` của dự án Netflex.
3.  Chạy script `dotenv-to-usersecrets.sh` để chuyển đổi các biến môi trường từ tệp `.env` của bạn thành User Secrets cho dự án Netflex.API:

    ```bash
    ./dotenv-to-usersecrets.sh
    ```

    Script này sẽ giúp quản lý các biến nhạy cảm một cách an toàn hơn.

### Bước 3: Chạy ứng dụng Netflex.API

1.  Trong cùng một terminal Git Bash, điều hướng đến thư mục của dự án `Netflex.WebAPI`:

    ```bash
    cd Presentation/Netflex.WebAPI
    ```

2.  Chạy ứng dụng Netflex.API bằng lệnh dotnet:

    ```bash
    dotnet run
    ```

    Ứng dụng sẽ bắt đầu và bạn sẽ thấy các thông báo về việc khởi động máy chủ và các địa chỉ URL mà API đang lắng nghe.
