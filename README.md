# 🎲 Game Cờ Cá Ngựa Multiplayer (Client – Server)

## 📌 1. Giới thiệu
Đây là đồ án xây dựng **trò chơi Cờ Cá Ngựa nhiều người chơi** theo mô hình **Client – Server**, cho phép từ **2 đến 4 người chơi** tham gia cùng một ván đấu thông qua kết nối mạng TCP.  
Hệ thống hỗ trợ đầy đủ các chức năng từ quản lý tài khoản, tạo phòng, tham gia phòng, chơi game realtime cho đến hiển thị bảng xếp hạng.

---

## 👥 2. Thành viên nhóm
| STT | Họ và tên          | MSSV       | Username      | 
|-----|--------------------|------------|---------------|
| 1   | Trần Phú Thành     | 24521641   | thanhfaith    |
| 2   | Võ Diệp Thành      | 24521644   | Vo-Thanh-06   | 
| 3   | Nguyễn Tấn Vũ      | 24522038   | tanvu0909     | 
| 4   | Nguyễn Phạm Yến Vy | 24522060   | npyvyy        | 
| 5   | Liên Ngọc Châu     | 24520209   | Jchaungocln   |

---

## 🛠️ 3. Công nghệ sử dụng
- 💻 **Ngôn ngữ lập trình**: C#  
- 🖼️ **Giao diện**: Windows Forms  
- 🌐 **Lập trình mạng**: TCP Socket  
- 🗄️ **Cơ sở dữ liệu**: SQL Server  
- 🔐 **Bảo mật**: SHA-256 (băm mật khẩu), OTP qua email  
- 🧩 **Mô hình**: Client – Server  

---

## 🏗️ 4. Kiến trúc hệ thống
Hệ thống được xây dựng theo mô hình **Client – Server** gồm 3 thành phần chính:

### 👤 Client
- Xử lý giao diện người dùng  
- Gửi yêu cầu và nhận dữ liệu từ server  
- Hiển thị trạng thái game theo thời gian thực  

### 🖥️ Server
- Quản lý kết nối nhiều client  
- Xử lý logic game và đồng bộ lượt chơi  
- Quản lý phòng chơi và người chơi  
- Kết nối và thao tác với cơ sở dữ liệu  

### 🗃️ Database
- Lưu trữ thông tin tài khoản người dùng  
- Quản lý phòng chơi và danh sách người tham gia  

---

## ⚙️ 5. Chức năng chính

### 🔑 5.1. Quản lý tài khoản
- Đăng ký tài khoản  
- Đăng nhập  
- Quên mật khẩu (xác thực bằng OTP)  
- Mã hóa mật khẩu bằng SHA-256  

### 🚪 5.2. Quản lý phòng chơi
- Tạo phòng chơi mới  
- Tham gia phòng bằng mã phòng  
- Phòng chờ hiển thị danh sách người chơi  
- Chủ phòng bắt đầu trò chơi  

### 🎮 5.3. Gameplay – Cờ Cá Ngựa
- Tung xúc xắc 🎲  
- Ra quân khi tung được số 6  
- Di chuyển quân theo luật chơi  
- Đá quân đối phương  
- Leo thang về đích  
- Đồng bộ trạng thái game realtime  
- Chat trong phòng chơi 💬  
- Âm thanh hiệu ứng trong game 🔊  

### 🏆 5.4. Kết thúc và xếp hạng
- Tự động kết thúc ván chơi khi có người chiến thắng  
- Hiển thị bảng xếp hạng  
- Quay lại tạo/tham gia phòng để chơi tiếp  

---

## 🔁 6. Luồng hoạt động chính
Luồng hoạt động của client được thực hiện theo trình tự sau:

```text
MenuForm
→ StartGame
→ LoginForm
→ CodeRoom
→ WaitingRoom
→ ChessBoard
→ RankingBoard
→ Quay lại CodeRoom
