# Laporan Project Student Registration App yang Terhubung dengan SQL Server

**Teknologi:** C#, WPF, ADO.NET, SQL Server, SSMS

## 1. PENDAHULUAN

Aplikasi **Student Registration App** merupakan aplikasi desktop berbasis **WPF (Windows Presentation Foundation)** dan **C#** yang terhubung ke database **SQL Server**. Aplikasi ini digunakan untuk mencatat dan mengelola data mahasiswa serta program studi dengan menerapkan relasi database (*One-to-Many*) dan komponen akses data menggunakan **ADO.NET**.
  
<img width="600" alt="image" src="https://github.com/user-attachments/assets/35d644ad-f3a7-4adb-ae32-3f2a569e577e" />
  

## 2. ARSITEKTUR SISTEM & ALUR DATA

### 2.1 Arsitektur Aplikasi

```
[ WPF UI (MainWindow.xaml) ]
           │
           ▼
[ Code-Behind (MainWindow.xaml.cs) ]
           │
           ▼
[ StudentRepository (Data Access Layer) ]
           │
           ▼
[ ADO.NET (SqlConnection, SqlCommand, SqlDataReader) ]
           │
           ▼
[ SQL Server Database Engine (StudentRegistrationDB) ]

```

### 2.2 Alur Pemrosesan Data

1. **Inisialisasi:** `MainWindow` memanggil method pada `StudentRepository`.

2. **Koneksi:** `StudentRepository` membuka koneksi ke SQL Server menggunakan `SqlConnection` dan *Connection String*.

3. **Eksekusi Query:** `SqlCommand` menjalankan query `SELECT` dengan klausa `INNER JOIN`.

4. **Pembacaan Data:** `SqlDataReader` membaca baris data lalu memetakan (*mapping*) nilainya ke objek `Student`.

5. **Data Binding:** Objek `Student` dimasukkan ke dalam `List<Student>` dan diikat (*bind*) ke `DataGrid` pada layar WPF.

## 3. STRUKTUR & DESIGN SKEMA DATABASE

Database **`StudentRegistrationDB`** terdiri dari 2 tabel utama yang terhubung dalam relasi **One-to-Many (1:N)**: Satu Program Studi dapat memiliki banyak Mahasiswa.
  
<img width="400" alt="image" src="https://github.com/user-attachments/assets/01d2aa84-ac8c-4619-bc9c-4c89ebd1e527" />
  
### 3.1 Tabel `Programs` (Master Program Studi)

* **Deskripsi:** Menyimpan daftar program studi yang tersedia.

* **Struktur Kolom:**

| Nama Kolom | Tipe Data | Constraint / Aturan | Keterangan | 
| ----- | ----- | ----- | ----- | 
| **ProgramId** | `INT` | **PRIMARY KEY**, `IDENTITY(1,1)` | Auto-increment ID unik program studi | 
| **ProgramCode** | `VARCHAR(20)` | **NOT NULL**, **UNIQUE** | Kode singkatan program studi (contoh: 'TI', 'SI') | 
| **ProgramName** | `VARCHAR(100)` | **NOT NULL** | Nama lengkap program studi | 
| **IsActive** | `BIT` | **NOT NULL**, `DEFAULT 1` | Status aktif program studi (1 = Aktif, 0 = Nonaktif) | 
| **CreatedAt** | `DATETIME2` | **NOT NULL**, `DEFAULT SYSDATETIME()` | Waktu data dibuat secara otomatis | 

* **Tabel (`Programs`):**
  
<img width="600" alt="image" src="https://github.com/user-attachments/assets/2679e19b-3faf-4c7d-96ed-9089c5c64419" />
  

### 3.2 Tabel `Students` (Data Mahasiswa)

* **Deskripsi:** Menyimpan data biodata mahasiswa yang terdaftar.

* **Struktur Kolom:**

| Nama Kolom | Tipe Data | Constraint / Aturan | Keterangan | 
| ----- | ----- | ----- | ----- | 
| **StudentId** | `INT` | **PRIMARY KEY**, `IDENTITY(1,1)` | Auto-increment ID unik mahasiswa | 
| **NIM** | `VARCHAR(20)` | **NOT NULL**, **UNIQUE** | Nomor Induk Mahasiswa | 
| **Name** | `VARCHAR(100)` | **NOT NULL** | Nama lengkap mahasiswa | 
| **ProgramId** | `INT` | **NOT NULL**, **FOREIGN KEY** | Merujuk ke `Programs(ProgramId)` | 
| **BirthDate** | `DATE` | `NULL` | Tanggal lahir mahasiswa | 
| **Address** | `VARCHAR(250)` | `NULL` | Alamat domisili | 
| **PhoneNumber** | `VARCHAR(20)` | `NULL` | Nomor telepon/HP | 
| **CreatedAt** | `DATETIME2` | **NOT NULL**, `DEFAULT SYSDATETIME()` | Waktu data dibuat | 
| **UpdatedAt** | `DATETIME2` | `NULL` | Waktu data terakhir diperbarui | 

* **Tabel (`Students`):**
  
<img width="800" alt="image" src="https://github.com/user-attachments/assets/7e4a6067-5076-4e16-9740-ece5df2ed6db" />
  

## 4. STRUKTUR & PENJELASAN KODE PROGRAM (C# / WPF)

### 4.1 Pohon Struktur Project

```
StudentRegistration/
├── Models/
│   ├── Program.cs             --> Model data tabel Programs
│   └── Student.cs             --> Model data tabel Students (+ properti JOIN ProgramName)
├── Repositories/
│   └── StudentRepository.cs   --> Mengelola query & koneksi ADO.NET ke SQL Server
├── MainWindow.xaml            --> Tampilan UI WPF (Header, Tombol Refresh, & DataGrid)
└── MainWindow.xaml.cs         --> Event handler & logika penghubung UI dengan Repository

```

### 4.2 Penjelasan Komponen & Fungsi Utama

#### 1. Models (`Student.cs` & `Program.cs`)

* **Properti Model:** Menyimpan data atribut mahasiswa (`StudentId`, `NIM`, `Name`, dll) dan program studi.

* **Properti `ProgramName`:** Ditambahkan pada `Student.cs` khusus untuk menampung nama program studi hasil query `INNER JOIN`.

#### 2. Data Access (`StudentRepository.cs`)

* **`_connectionString`**: Menyimpan variabel konfigurasi alamat server database lokal (`.\SQLEXPRESS`), nama database, dan opsi enkripsi koneksi.

* **`GetAll()`**:

  * **Fungsi:** Mengambil seluruh data mahasiswa dari SQL Server yang sudah digabungkan dengan nama program studinya.

  * **Mekanisme:**

    1. Membuat objek `SqlConnection` & `SqlCommand` dengan query SQL `INNER JOIN`.

    2. Membuka koneksi (`connection.Open()`) dan mengeksekusi `command.ExecuteReader()`.

    3. Menggunakan `while(reader.Read())` untuk membaca baris demi baris dan mengubahnya (*mapping*) menjadi objek `Student`.

    4. Mengembalikan daftar data berupa `List<Student>`.

#### 3. User Interface (`MainWindow.xaml`)

* **`<DataGrid>`**: Menampilkan tabel data mahasiswa secara interaktif. Kolom-kolomnya diikat menggunakan binding C# (`Binding="{Binding NIM}"`, `Binding="{Binding Name}"`, dll).

* **`<Button>` (Refresh)**: Tombol untuk memicu pembaruan/pengambilan ulang data dari SQL Server.

#### 4. Logic UI (`MainWindow.xaml.cs`)

* **`MainWindow()` (Constructor)**: Inisialisasi komponen UI, membuat instansiasi `StudentRepository`, lalu memanggil `LoadStudents()`.

* **`LoadStudents()`**:

  * **Fungsi:** Memanggil `_studentRepository.GetAll()` lalu memasukkan hasilnya ke `StudentDataGrid.ItemsSource`.

  * **Error Handling:** Dibungkus menggunakan blok `try-catch` untuk menampilkan dialog pesan error (`MessageBox`) jika koneksi database gagal.

* **`RefreshButton_Click()`**: Event handler ketika tombol Refresh diklik untuk memanggil kembali fungsi `LoadStudents()`.

## 5. HASIL PENGUJIAN DATABASE

Pengujian dilakukan langsung pada **SQL Server (SSMS)** dan terintegrasi dengan aplikasi **WPF C#** untuk memastikan aturan integritas data (*constraints*) serta relasi tabel berjalan dengan benar.

### 5.1 Pengujian Foreign Key Constraint

* **Skenario:** Memasukkan data mahasiswa baru dengan `ProgramId = 99` (ID program studi yang tidak ada pada tabel `Programs`).

* **Input Test:**

  ```
  INSERT INTO Students (NIM, Name, ProgramId)
  VALUES ('20231099', 'Test Student', 99);
  
  ```

* **Hasil Pengujian:** **GAGAL / DITOLAK** oleh SQL Server.

* **Pesan Error:** `The INSERT statement conflicted with the FOREIGN KEY constraint "FK_Students_Programs".`

* **Kesimpulan:** Constraint **Foreign Key** berfungsi dengan sempurna dalam menjaga integritas referensial data.

### 5.2 Pengujian Unique Key NIM Constraint

* **Skenario:** Memasukkan data mahasiswa dengan `NIM = '20231001'` yang sudah terdaftar sebelumnya di database.

* **Input Test:**

  ```
  INSERT INTO Students (NIM, Name, ProgramId)
  VALUES ('20231001', 'Mahasiswa Duplikat', 1);
  
  ```

* **Hasil Pengujian:** **GAGAL / DITOLAK** oleh SQL Server.

* **Pesan Error:** `Violation of UNIQUE KEY constraint "UQ_Students_NIM". Cannot insert duplicate key in object 'dbo.Students'.`

* **Kesimpulan:** Constraint **Unique Key** berhasil mencegah adanya duplikasi NIM pada tabel mahasiswa.

### 5.3 Pengujian Relasi INNER JOIN & Data Fetching

* **Skenario:** Mengambil data gabungan mahasiswa dengan nama program studi menggunakan klausa `INNER JOIN`.

* **Query Test:**

  ```
  SELECT s.NIM, s.Name, p.ProgramName, s.BirthDate, s.PhoneNumber
  FROM Students s
  INNER JOIN Programs p ON s.ProgramId = p.ProgramId
  ORDER BY s.Name;
  ```

* **Hasil Output Pengujian:**
  
<img width="600" alt="image" src="https://github.com/user-attachments/assets/705444e4-5791-4b45-8c41-9342c6de6754" />
  
* **Kesimpulan:** Query `INNER JOIN` berhasil memetakan kunci `ProgramId` menjadi nama program studi (`ProgramName`) secara tepat dan dapat ditampilkan secara otomatis pada `DataGrid` WPF melalui paket **ADO.NET** (`Microsoft.Data.SqlClient`).
