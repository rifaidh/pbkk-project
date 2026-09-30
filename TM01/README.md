# Laporan Dokumentasi Program: Sistem Data Mahasiswa

## 1. Hello World
Pada terminal Visual Studio Code, jalankan:
```
dotnet new console -n HelloWorld
cd HelloWorld
dotnet run
```
<img width="500" alt="Screenshot 2026-09-26 225054" src="https://github.com/user-attachments/assets/66307b6f-2955-49d9-a979-03b23ac23dc9" />

## 2. Pendahuluan

Program **Sistem Data Mahasiswa** adalah aplikasi berbasis konsol menggunakan bahasa pemrograman **C#**. Program ini dirancang untuk melakukan manajemen data mahasiswa secara sederhana yang meliputi proses penambahan, penampilan, pencarian, dan penghapusan data mahasiswa.

## 3. Struktur Data & Entitas (`class Mahasiswa`)

Bagian ini bertanggung jawab sebagai cetak biru objek untuk merepresentasikan entitas mahasiswa beserta atribut-atribut pendukungnya.

```csharp
class Mahasiswa
{
    public string NIM { get; set; }
    public string Nama { get; set; }
    public string Prodi { get; set; }
    public double IPK { get; set; }

    public Mahasiswa(string nim, string nama, string prodi, double ipk)
    {
        Nama = nama;
        NIM = nim;
        Prodi = prodi;
        IPK = ipk;
    }
}

```

* **Properti:**

  * `NIM`: Menyimpan nomor induk mahasiswa (tipe data `string`).

  * `Nama`: Menyimpan nama lengkap mahasiswa (tipe data `string`).

  * `Prodi`: Menyimpan program studi mahasiswa (tipe data `string`).

  * `IPK`: Menyimpan nilai indeks prestasi kumulatif (tipe data `double`).

* **Konstruktor (`Mahasiswa`):** Menginisialisasi nilai properti objek saat pertama kali data mahasiswa baru dibuatkan instancenya.

## 4. Navigasi Menu (`class Program` & `Main`)

Kelas `Program` menampung variabel global berupa koleksi data mahasiswa (`daftarMahasiswa`) serta alur kontrol utama aplikasi.

```csharp
class Program
{
    static List<Mahasiswa> daftarMahasiswa = new List<Mahasiswa>();
    
    static void Main(string[] args)
    {
        int pilihan;
        do
        {
            TampilkanMenu();
            Console.Write("Pilihan: ");
            string input = Console.ReadLine();
            
            if(!int.TryParse(input, out pilihan))
            {
                pilihan = 0;
            }

            Console.WriteLine();

            switch (pilihan)
            {
                case 1: TambahMahasiswa(); break;
                case 2: TampilkanMahasiswa(); break;
                case 3: CariMahasiswa(); break;
                case 4: HapusMahasiswa(); break;
                case 5: Console.WriteLine("Terima kasih telah menggunakan program."); break;
                default: Console.WriteLine("Pilihan tidak tersedia!"); break;
            }

            if(pilihan != 5)
            {
                Console.WriteLine();
                Console.WriteLine("Tekan Enter untuk melanjutkan...");
                Console.ReadLine();
            }
        } while (pilihan != 5);
    }
    // ... metode lainnya ...
}

```

* **Fungsi Utama (`Main`):** Mengelola alur aplikasi menggunakan perulangan `do-while` serta percabangan `switch-case` untuk mengarahkan pilihan pengguna ke fungsi operasional yang sesuai.

## 5. Rincian Fungsi dan Modul Pendukung

### A. Fungsi Tampilkan Menu (`TampilkanMenu`)

Berfungsi untuk membersihkan layar konsol dan menampilkan antarmuka pilihan menu utama kepada pengguna.

```csharp
static void TampilkanMenu()
{
    Console.Clear();
    Console.WriteLine("=====================");
    Console.WriteLine("Sistem Data Mahasiswa");
    Console.WriteLine("=====================");
    Console.WriteLine("1. Tambah Mahasiswa");
    Console.WriteLine("2. Tampilkan Mahasiswa");
    Console.WriteLine("3. Cari Mahasiswa");
    Console.WriteLine("4. Hapus Mahasiswa");
    Console.WriteLine("5. Keluar");
    Console.WriteLine("=====================");            
}

```  
<img width="500" alt="Screenshot 2026-09-26 223339" src="https://github.com/user-attachments/assets/2d166ccd-8e25-4f1d-b725-a348d695b15e" />
  
### B. Fungsi Tambah Mahasiswa (`TambahMahasiswa`)

Berfungsi untuk meminta input data dari pengguna meliputi NIM, Nama, Prodi, dan IPK, dengan validasi ketat agar nilai IPK berada pada rentang angka $0$ sampai $4$.

```csharp
static void TambahMahasiswa()
{
    Console.Clear();

    Console.WriteLine("NIM  :");
    string nim = Console.ReadLine();
    
    Console.WriteLine("Nama :");
    string nama = Console.ReadLine();
    
    Console.WriteLine("Program Studi:");
    string prodi = Console.ReadLine();
    
    double ipk;
    while (true)
    {
        Console.Write("IPK  :");
        if(double.TryParse(Console.ReadLine(), out ipk))
        {
            if(ipk >= 0 && ipk <= 4)
            {
                break;
            }
        }
        Console.WriteLine("IPK harus berupa angka 0-4");
    }
    
    Mahasiswa mahasiswa = new Mahasiswa(nim, nama, prodi, ipk);
    daftarMahasiswa.Add(mahasiswa);

    Console.WriteLine();
    Console.WriteLine("Data mahasiswa berhasil ditambahkan");         
}

```
  
<img width="500" alt="Screenshot 2026-09-26 223530" src="https://github.com/user-attachments/assets/79a9724d-6982-448b-a516-7521f9c1dfcd" />
  
### C. Fungsi Tampilkan Daftar Mahasiswa (`TampilkanMahasiswa`)

Berfungsi untuk merender dan menampilkan seluruh data mahasiswa yang tersimpan di dalam memori ke layar konsol dengan format tabel terstruktur.

```csharp
static void TampilkanMahasiswa()
{
    Console.Clear();

    if (daftarMahasiswa.Count == 0)
    {
        Console.WriteLine("Belum ada data mahasiswa.");
        return;
    }

    Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5}", "NIM", "Nama", "Prodi", "IPK");
    Console.WriteLine("----------------------------------------------------------");

    foreach (Mahasiswa m in daftarMahasiswa)
    {
        Console.WriteLine("{0,-12} {1,-20} {2,-20} {3,5:F2}", m.NIM, m.Nama, m.Prodi, m.IPK);
    }
    Console.WriteLine("==========================================================");
}

```
  
  
### D. Fungsi Cari Mahasiswa (`CariMahasiswa`)

Berfungsi untuk mencari data mahasiswa secara spesifik berdasarkan nomor induk (`NIM`) yang diinputkan pengguna dengan mengabaikan perbedaan huruf besar/kecil (*case-insensitive*).

```csharp
static void CariMahasiswa()
{
    Console.Clear();

    Console.Write("Masukkan NIM: ");
    string nimCari = Console.ReadLine();

    Mahasiswa mahasiswaDitemukan = null;
    foreach (Mahasiswa m in daftarMahasiswa)
    {
        if (m.NIM.Equals(nimCari, StringComparison.OrdinalIgnoreCase))
        {
            mahasiswaDitemukan = m;
            break;
        }
    }

    Console.WriteLine();
    if (mahasiswaDitemukan != null)
    {
        Console.WriteLine("Data ditemukan!");
        Console.WriteLine("NIM : " + mahasiswaDitemukan.NIM);
        Console.WriteLine("Nama : " + mahasiswaDitemukan.Nama);
        Console.WriteLine("Prodi : " + mahasiswaDitemukan.Prodi);
        Console.WriteLine("IPK : " + mahasiswaDitemukan.IPK.ToString("F2"));
    }
    else
    {
        Console.WriteLine("Mahasiswa dengan NIM tersebut tidak ditemukan.");
    }
}

```
  
<img width="500" alt="Screenshot 2026-09-26 223554" src="https://github.com/user-attachments/assets/cbdda23e-b901-4644-88da-8639f26784fb" />
    
### E. Fungsi Hapus Mahasiswa (`HapusMahasiswa`)

Berfungsi untuk mencari dan menghapus data mahasiswa tertentu dari koleksi list berdasarkan kecocokan NIM.

```csharp
static void HapusMahasiswa()
{
    Console.Clear();

    Console.Write("Masukkan NIM: ");
    string nimHapus = Console.ReadLine();

    Mahasiswa mahasiswaDitemukan = null;
    foreach (Mahasiswa m in daftarMahasiswa)
    {
        if (m.NIM.Equals(nimHapus, StringComparison.OrdinalIgnoreCase))
        {
            mahasiswaDitemukan = m;
            break;
        }
    }

    if (mahasiswaDitemukan != null)
    {
        daftarMahasiswa.Remove(mahasiswaDitemukan);
        Console.WriteLine();
        Console.WriteLine("Data mahasiswa berhasil dihapus.");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine("Data mahasiswa tidak ditemukan.");
    }
}

```
  
<img width="500" alt="Screenshot 2026-09-26 223611" src="https://github.com/user-attachments/assets/65a3a6c9-a977-4af9-b5a2-6aaad4a6c1ea" />
