use master
--Tao CSDL
create database test
GO
use test
GO
Create Table PhanLoai
(
	MaPL int ,
	TenPhanLoai nvarchar(20) ,
	CONSTRAINT PK_phanLoai PRIMARY KEY(MaPL)
)
CREATE TABLE DienThoai
(
    MaDT INT,
    TenDT NVARCHAR(40),
    GiaGoc int,
    Discount INT,   
    GiaCuoi int,
    ManHinh FLOAT CHECK (ManHinh < 10),
    LoaiManHinh NVARCHAR(12) CHECK (LoaiManHinh IN ('Full HD+', 'HD+', '2K', '4K')),
    Chip VARCHAR(30),
    Ram int Check (Ram < 49),
    Rom int check (Rom < 2050),
    Cam int check (Cam < 200),
    CamSau int check (CamSau < 200),
    Pin int check (Pin < 12000),    
    MaPL INT,
    Ngaycapnhat DATE,
    Soluongban INT,
    soluotxem INT,
    HinhChiTiet0 VARCHAR(35),
    HinhChiTiet1 VARCHAR(35),
    HinhChiTiet2 VARCHAR(35),
    HinhChiTiet3 VARCHAR(35),
    HinhChiTiet4 VARCHAR(35),
    HinhChiTiet5 VARCHAR(35),
    CONSTRAINT PK_dienThoai PRIMARY KEY(MaDT)
);

GO
CREATE TABLE KhachHang
(
	MaKH INT ,
	HoTenKH nVarchar(50) ,
	DiachiKH nVarchar(50),
	DienthoaiKH Varchar(11),
	TenDN Varchar(15) UNIQUE,
	Matkhau Varchar(15),
	Ngaysinh date,
	Gioitinh nvarchar(8),
	Email Varchar(28),	
	CONSTRAINT PK_khachHang PRIMARY KEY(MaKH)
)
CREATE TABLE NhanVien
(
	MaNV INT, MaVT INT,
	HoTenNV nVarchar(50) ,
	DiachiNV nVarchar(50),
	DienthoaiNV Varchar(11),
        VaiTro nvarchar(50),
        Host BIT DEFAULT 0,
	TenDN Varchar(15) UNIQUE,
	Matkhau Varchar(15),
	Ngaysinh date,
	Gioitinh nvarchar(8),
	Email Varchar(28),
        Luong INT,	
	CONSTRAINT PK_nhanVien PRIMARY KEY(MaNV)
)
Create Table SuKien
(
        MaSK int,
        TenSK ntext,
        Mota ntext,
        NgayDang date,
        HinhAnh varchar(50),
        NoiDungI ntext,     
        NoiDungII ntext, 
        NoiDungIII ntext,  
        CONSTRAINT PK_suKien PRIMARY KEY(MaSK)
)
CREATE TABLE DonHang
(
	SoDH INT IDENTITY(1,1),
	MaKH INT,
	NgayDH date,         
	Trigia int,
	DaNhan Bit Default 0,	
	TenKhachHang nvarchar(150),        		
	CONSTRAINT PK_donHang PRIMARY KEY(SoDH)
)
CREATE TABLE CTDatHang
(
	SoDH INT,
	MaDT INT,
	SoLuong int,
	GiaCuoi int,
	Thanhtien AS SoLuong*GiaCuoi,
	CONSTRAINT PK_CTdatHang PRIMARY KEY(SoDH,MaDT)
)
CREATE TABLE DanhGiaSP
(
    MaDanhGia INT IDENTITY(1, 1) PRIMARY KEY,
    MaDT INT,
    MaKH INT,
    NoiDung NVARCHAR(MAX),
    Rate INT CHECK (Rate IN (1, 2, 3, 4, 5)),
    NgayDang DATE,
    CONSTRAINT FK_DanhGiaSP_DienThoai FOREIGN KEY (MaDT) REFERENCES DienThoai(MaDT),
    CONSTRAINT FK_DanhGiaSP_KhachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
);
CREATE TABLE GopY (
    MaGY INT IDENTITY(1,1) PRIMARY KEY,
    TenKH NVARCHAR(255),
    VanDe NVARCHAR(255),
    NoiDung NVARCHAR(MAX),
    LienLac NVARCHAR(255),
    TrangThai Bit Default 0,
    GhiChu NVARCHAR(MAX),
    NgayDang DATE
);

ALTER TABLE DienThoai ADD CONSTRAINT FK_dienThoai_Phanloai FOREIGN KEY (MaPL) REFERENCES PhanLoai(MaPL)
ALTER TABLE DonHang ADD CONSTRAINT FK_donHang_khachHang FOREIGN KEY (MaKH) REFERENCES KhachHang(MaKH)
ALTER TABLE CTDatHang ADD CONSTRAINT FK_CTdatHang_donHang FOREIGN KEY (SoDH) REFERENCES DonHang(SoDH)
ALTER TABLE CTDatHang ADD CONSTRAINT FK_CTdatHang_dienThoai FOREIGN KEY (MaDT) REFERENCES DienThoai(MaDT)




INSERT INTO PhanLoai (MaPL, TenPhanLoai)
VALUES
    (1, N'Sony'),
    (2, N'Xiaomi'),
    (3, N'Google'),
    (4, N'Samsung'),
    (5, N'Oppo');

-- Thêm dữ liệu vào bảng DienThoai
INSERT INTO DienThoai (MaDT, TenDT, GiaGoc, Discount, GiaCuoi, ManHinh, LoaiManHinh, Chip, Ram, Rom, Cam, CamSau, Pin, MaPL, Ngaycapnhat, Soluongban, soluotxem, HinhChiTiet0, HinhChiTiet1, HinhChiTiet2, HinhChiTiet3, HinhChiTiet4, HinhChiTiet5)
VALUES
    (1, N'Điện thoại Sony Xperia Z5 Premium Dual', 1000, 20, 800, 6.11, N'HD+', N'Snapdragon 630', 8, 512, 8, 24, 3212, 1, '2001-01-01', 0, 1000, '1.0.png', '1.1.jpg', '1.2.jpg', '1.3.jpg', '1.4.jpg', '1.5.jpg'),
    (2, N'Điện thoại Sony Xperia Ace', 1000, 15, 850, 6.11, N'HD+', N'Snapdragon 630', 8, 512, 8, 24, 3212, 1, '2002-01-01', 0, 1000, '2.jpg', '2.jpg', '2.jpg', '2.jpg', '2.jpg', '2.jpg'),
    (3, N'Điện thoại Sony Xperia 8 Lite', 1000, 10, 900, 6.11, N'HD+', N'Snapdragon 630', 8, 512, 8, 24, 3212, 1, '2003-01-01', 0, 1000, '3.jpg', '3.jpg', '3.jpg', '3.jpg', '3.jpg', '3.jpg'),
    (4, N'Điện thoại Sony Xperia 5 III', 1000, 0, 1000, 6.11, N'HD+', N'Snapdragon 630', 8, 512, 8, 24, 3212, 1, '2004-01-01', 0, 1000, '4.jpg', '4.jpg', '4.jpg', '4.jpg', '4.jpg', '4.jpg');

INSERT INTO KhachHang (MaKH, HoTenKH, DiachiKH, DienthoaiKH, TenDN, Matkhau, Ngaysinh, Gioitinh, Email)
VALUES
(1, 'Trinh Gia T', 'ag', '030', '1', '1', '2003-01-01', 'nam', 'sample@gmail.com'),
(2, 'Tran Thi Diem S', 'vt', '030', '2', '2', '2003-01-01', 'nữ', 'sample1@gmail.comn'),
(3, 'Nguyen Van A', '123 Đường ABC, Quận 1, TP.HCM', '01234567890', '3', '3', '1990-01-01', 'Nam', 'user1@email.com'),
(4, 'Tran Thi B', '456 Đường XYZ, Quận 2, TP.HCM', '09876543210', '4', '4', '1995-05-15', 'Nữ', 'user2@email.com'),
(5, 'Le Van C', '789 Đường LMN, Quận 3, TP.HCM', '07654321987', '5', '5', '1988-11-20', 'Nam', 'user3@email.com'),
(6, 'Pham Thanh D', '321 Đường PQR, Quận 4, TP.HCM', '03456789012', '6', '6', '1993-07-10', 'Nam', 'user4@email.com'),
(7, 'Vo Hoang E', '654 Đường UVW, Quận 5, TP.HCM', '02345678901', '7', '7', '1985-03-25', 'Nam', 'user5@email.com');

INSERT INTO DanhGiaSP (MaDT, MaKH, NoiDung, Rate, NgayDang)
VALUES
(1, 1, 'Đẹp và rẻ', 5, '2023-11-02'),
(2, 1, 'Rất tốt', 5, '2023-11-13'),
(2, 2, 'Khá tốt', 4, '2023-11-14'),
(1, 3, 'Tốt', 5, '2023-11-15'),
(3, 4, 'Chưa được', 2, '2023-11-16'),
(2, 5, 'Rất không hài lòng', 1, '2023-11-17');

INSERT INTO NhanVien (MaNV, MaVT, HoTenNV, DiachiNV, DienthoaiNV, VaiTro, Host, TenDN, Matkhau, Ngaysinh, Gioitinh, Email, Luong)
VALUES
(1, 10, 'Trinh Gia Thien', 'angiang', '131', 'Nhân viên bán hàng', 0, '1', '1', '2003-01-01', 'Nam', 'sample2@gmail.com', 1000000),
(2, 2, 'Tran Thi Diem Suong', 'vungtau', '121', 'Nhân viên bán hàng', 0, '2', '2', '1992-03-15', 'Nữ', 'nv1@email.com', 8000000),
(3, 3, 'Le Van Tai', '123 Đường ABC, Quận 1, TP.HCM', '03456789012', 'Kế toán', 0, 'nv2', 'password2', '1987-09-10', 'Nam', 'nv2@email.com', 9000000),
(4, 2, 'Pham Thi Minh', '654 Đường UVW, Quận 5, TP.HCM', '07654321987', 'Nhân viên bán hàng', 0, 'nv3', 'password3', '1995-07-22', 'Nữ', 'nv3@email.com', 7500000),
(5, 1, 'Hoang Van Hiep', '321 Đường PQR, Quận 4, TP.HCM', '02345678901', 'Quản lý', 0, 'nv4', 'password4', '1993-12-05', 'Nam', 'nv4@email.com', 11000000),
(6, 2, 'Tran Thi My', '987 Đường LMN, Quận 3, TP.HCM', '04567890123', 'Nhân viên bán hàng', 0, 'nv5', 'password5', '1998-08-18', 'Nữ', 'nv5@email.com', 8200000),
(7, 3, 'Nguyen Van Cuong', '555 Đường UVW, Quận 5, TP.HCM', '05678901234', 'Kế toán', 0, 'nv6', 'password6', '1991-06-25', 'Nam', 'nv6@email.com', 9500000),
(8, 2, 'Pham Thi Ngoc', '777 Đường XYZ, Quận 2, TP.HCM', '06789012345', 'Nhân viên bán hàng', 0, 'nv7', 'password7', '1994-04-30', 'Nữ', 'nv7@email.com', 7800000);

INSERT INTO GopY (TenKH, VanDe, NoiDung, LienLac, TrangThai, GhiChu, NgayDang)
VALUES
('Nguyen Van A', 'Sản phẩm', 'Rất hài lòng với sản phẩm mới', '0123456789', 0, '', '2023-11-18'),
('Tran Thi B', 'Dịch vụ', 'Nhân viên phục vụ rất tốt', '0987654321', 0, '', '2023-11-19'),
('Le Van C', 'Sản phẩm', 'Cần cải thiện chất lượng sản phẩm', '0765432198', 0, '', '2023-11-20');