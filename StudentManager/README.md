# LAPORAN PROJEK Student Management System

## Student Management System (Rich UI + MVVM + ADO.NET + PostgreSQL)

## 1. Pendahuluan

Aplikasi Student Management System berbasis desktop ini dirancang untuk mengelola data mahasiswa secara responsif dan andal. Menggunakan WPF dengan pola arsitektur MVVM, aplikasi ini memisahkan antarmuka pengguna dari logika bisnisnya secara jelas.

Didukung oleh ADO.NET dan database PostgreSQL, sistem ini menyediakan performa tinggi untuk berbagai fitur utama:

* **Pengolahan Data CRUD**: Pembuatan, pembacaan, pembaruan, dan penghapusan data mahasiswa.

* **Pencarian Cepat**: Fitur pencarian data yang responsif.

* **Panel Statistik**: Tampilan analisis statistik secara *real-time*.

## 2. Arsitektur dan Teknologi

### Spesifikasi Teknologi

| Layer / Komponen | Teknologi yang Digunakan | 
| ----- | ----- | 
| **Bahasa Pemrograman** | C# (.NET 8) | 
| **Framework UI** | WPF (XAML) | 
| **Pola Arsitektur** | MVVM (Model-View-ViewModel) | 
| **Database Engine** | PostgreSQL | 
| **Database Provider** | Npgsql (ADO.NET) | 
| **IDE** | Microsoft Visual Studio | 

## 3. Struktur Proyek

```
StudentManager/
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml               # View Utama
├── MainWindow.xaml.cs            # Code-Behind (Inisialisasi DataContext)
├── Models/
│   └── Student.cs                # Model Data Mahasiswa
├── Data/
│   └── StudentRepository.cs      # Akses Database (ADO.NET - Npgsql)
└── ViewModels/
    ├── RelayCommand.cs           # Implementasi ICommand
    └── StudentViewModel.cs       # Logika Bisnis & Binding State

```

## 4. Penjelasan Struktur dan Komponen Program

### A. Database Script (PostgreSQL DDL)

* **Tabel `Students`**: Menyimpan seluruh entitas data mahasiswa dengan atribut `Id` (Auto Increment/Serial), `NIM`, `Nama`, `Jurusan`, `Gender`, dan `Email`.

### B. Layer Model (`Models/Student.cs`)

* **Kelas `Student`**: Berisi properti data entitas mahasiswa (`Id`, `NIM`, `Nama`, `Jurusan`, `Gender`, `Email`). Berfungsi sebagai struktur objek data murni yang ditransfer antar layer (Database, Repository, ViewModel, dan View).

### C. Layer Data Access (`Data/StudentRepository.cs`)

Mengelola seluruh operasi I/O dan eksekusi query SQL langsung ke database PostgreSQL menggunakan `NpgsqlConnection` dan `NpgsqlCommand`:

* **`connectionString`**: Menyimpan parameter konfigurasi koneksi ke server PostgreSQL (Host, Port, Database, Username, Password).

* **`GetAll()`**: Membaca seluruh baris data dari tabel `Students`, mengonversinya menjadi daftar objek `List<Student>`, dan mengurutkannya dari data terbaru.

* **`Insert(Student student)`**: Menambahkan record data mahasiswa baru ke database berdasarkan parameter inputan dari ViewModel.

* **`Update(Student student)`**: Memperbarui kolom data mahasiswa pada database berdasarkan `Id` mahasiswa yang dipilih.

* **`Delete(int id)`**: Menghapus baris record mahasiswa tertentu dari database berdasarkan entitas `Id`.

* **`Search(string keyword)`**: Melakukan pencarian data berbasis pencocokan pola kata kunci (menggunakan perintah `ILIKE` khas PostgreSQL) pada kolom `NIM`, `Nama`, atau `Jurusan`.

### D. Layer ViewModel (`ViewModels/`)

#### 1. Helper Command (`RelayCommand.cs`)

* **Kelas `RelayCommand`**: Mengimplementasikan antarmuka `ICommand`. Berfungsi menghubungkan aksi tombol/event pada antarmuka XAML (View) langsung ke fungsi/metode C# yang ada pada `StudentViewModel`.

#### 2. Main Logic (`StudentViewModel.cs`)

Berperan sebagai pengatur logika bisnis, pengolah state antarmuka, serta jembatan penghubung antara View dan Repository:

* **Properties & Data Binding**:

  * **`Students`**: Koleksi tipe `ObservableCollection<Student>` yang menampung daftar mahasiswa. Koleksi ini mendukung pemberitahuan perubahan otomatis ke `DataGrid` pada View.

  * **`SelectedStudent`**: Menampung objek data mahasiswa yang sedang dipilih oleh pengguna di tabel atau form input.

  * **`SearchText`**: Menampung kata kunci pencarian yang dimasukkan pengguna pada form pencarian.

  * **Properties Statistik (`TotalStudents`, `TotalInformatika`, `TotalSistemInformasi`, `TotalLakiLaki`, `TotalPerempuan`)**: Properti kalkulasi yang menghitung total entitas berdasarkan filter kriteria tertentu secara *real-time*.

* **Commands (`ICommand`)**:

  * **`SaveCommand`**: Memicu eksekusi metode penyimpan data (Insert/Update).

  * **`DeleteCommand`**: Memicu eksekusi metode penghapusan data.

  * **`ResetCommand`**: Memicu pengosongan form input.

  * **`SearchCommand`**: Memicu pencarian data ke repository.

* **Fungsi**:

  * **`StudentViewModel()` (Constructor)**: Menginisialisasi repositori, mendaftarkan instance command, dan memuat data awal saat aplikasi dijalankan.

  * **`LoadData()`**: Mengambil data terbaru dari `StudentRepository` dan memperbarui isi koleksi `Students` serta panel statistik.

  * **`Save()`**: Memeriksa apakah data merupakan record baru (`Id == 0`) atau pembaruan (`Id != 0`), lalu memanggil fungsi `Insert` atau `Update` pada repositori.

  * **`Delete()`**: Memanggil fungsi `Delete` pada repositori untuk menghapus data `SelectedStudent` saat ini.

  * **`Search()`**: Mengambil hasil pencarian dari repositori berdasarkan nilai `SearchText` dan memperbarui tampilan koleksi `Students`.

  * **`Reset()`**: Mengosongkan form dengan cara menginisialisasi ulang objek `SelectedStudent`.

  * **`RefreshStatistics()`**: Memicu pemberitahuan properti diubah (`OnPropertyChanged`) untuk seluruh properti kalkulasi statistik agar tampilan angka di UI selalu diperbarui.

  * **`OnPropertyChanged()`**: Mengimplementasikan mekanisme `INotifyPropertyChanged` untuk memberitahu antarmuka XAML ketika ada perubahan data pada ViewModel.

### E. Layer View (`MainWindow.xaml` & `MainWindow.xaml.cs`)

* **`MainWindow.xaml`**: Merancang seluruh tata letak komponen UI desktop (Header, Form Input, Panel Statistik, Search Bar, dan DataGrid) serta melakukan *Data Binding* ke properti dan command di `StudentViewModel`.

* **`MainWindow.xaml.cs` (Code-Behind)**: Mengatur `DataContext` halaman utama ke instance `StudentViewModel` agar mekanisme *Data Binding* XAML dapat berjalan dengan sempurna.

## 5. Pengujian Sistem (Test Cases)

| No | Kasus Pengujian | Langkah Pengujian | Hasil yang Diharapkan | Status | 
| ----- | ----- | ----- | ----- | ----- | 
| 1 | **Read Data** | Menjalankan aplikasi pertama kali. | Data awal dari PostgreSQL berhasil tampil di DataGrid. | PASS | 
| 2 | **Create Data** | Mengisi NIM, Nama, Jurusan, Gender, Email lalu klik tombol **Simpan**. | Data baru tersimpan di database dan muncul pada DataGrid. Statistik otomatis bertambah. | PASS | 
| 3 | **Update Data** | Memilih salah satu baris di DataGrid, mengubah data pada form, lalu klik **Simpan**. | Data yang terpilih di database terbarui dan tampilan DataGrid diperbarui. | PASS | 
| 4 | **Delete Data** | Memilih salah satu data pada DataGrid lalu klik tombol **Hapus**. | Data terhapus dari PostgreSQL dan hilang dari DataGrid. Statistik ter-update. | PASS | 
| 5 | **Search Data** | Memasukkan kata kunci pada pencarian lalu klik tombol **Cari**. | DataGrid menampilkan hasil filter berdasarkan NIM, Nama, atau Jurusan (case-insensitive). | PASS | 
| 6 | **Reset Form** | Mengklik tombol **Reset**. | Seluruh field input pada form kembali kosong/kondisi awal. | PASS | 
