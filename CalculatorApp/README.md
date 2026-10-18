# Laporan Aplikasi Desktop: Calculator Apps (Windows Forms)

## 1. Pendahuluan

Aplikasi kalkulator berbasis desktop ini dibangun menggunakan C# dan Windows Forms (WinForms) di lingkungan .NET untuk mengolah aritmetika dasar secara visual. Selain fungsi standar, aplikasi ini dilengkapi berbagai fitur pendukung seperti input desimal, ubah tanda bilangan ($+/-$), persentase, *backspace*, serta pencatatan dan pemanggilan ulang riwayat kalkulasi.

<img width="800" alt="image" src="https://github.com/user-attachments/assets/b4d9a085-94bd-45ad-937f-d7ff1df1b1ee" />

## 2. Komponen Antarmuka (GUI)

Antarmuka dikembangkan dengan memanfaatkan komponen standar WinForms yang didefinisikan pada file `Form1.Designer.cs`.

### Ringkasan Komponen GUI

| Nama Komponen | Tipe Komponen | Fungsi / Peran | 
| ----- | ----- | ----- | 
| `txtDisplay` | `TextBox` | Menampilkan input angka dan hasil akhir operasi matematika (posisi teks rata kanan). | 
| `btn0` – `btn9` | `Button` | Input digit angka $0$ sampai $9$. | 
| `btnPlus`, `btnMinus`, `btnMultiply`, `btnDivide` | `Button` | Memilih operator matematika ($+$, $-$, $\times$, $\div$). | 
| `btnEquals` | `Button` | Menghitung dan menampilkan hasil akhir kalkulasi. | 
| `btnDecimal` | `Button` | Menambahkan tanda koma desimal (`.`). | 
| `btnClear` | `Button` | Meriset ulang seluruh status kalkulator kembali ke kondisi awal (`0`). | 
| `btnPlusMinus` | `Button` | Mengubah tanda bilangan (positif $\leftrightarrow$ negatif). | 
| `btnPercent` | `Button` | Mengubah nilai yang ditampilkan menjadi bentuk persentase ($x / 100$). | 
| `btnBackspace` | `Button` | Menghapus satu karakter terakhir dari layar display. | 
| `lstHistory` | `ListBox` | Menampilkan daftar riwayat transaksi kalkulasi yang telah dilakukan. | 

## 3. Struktur Logic & Pemrosesan Kode (`Form1.cs`)

### 3.1 Variabel Utama

Kode program menggunakan beberapa variabel internal untuk menyimpan status kalkulasi:

* `double firstNumber`: Menyimpan angka pertama yang diinput sebelum operator dipilih.

* `double secondNumber`: Menyimpan angka kedua setelah operator dipilih.

* `double result`: Menyimpan hasil perhitungan matematika.

* `string operation`: Menyimpan jenis operator aritmetika yang sedang aktif.

### 3.2 Penjelasan Alur Kerja Fitur

#### A. Input Angka & Format (`CultureInfo.InvariantCulture`)

Untuk menghindari masalah sintaksis pada format angka berbasis regional (misalnya penggunaan tanda koma `,` versus titik `.`), aplikasi mengadopsi `CultureInfo.InvariantCulture`.

```
private void NumberButton_Click(object sender, EventArgs e)
{
    Button button = (Button)sender;
    if (txtDisplay.Text == "0")
        txtDisplay.Text = button.Text;
    else
        txtDisplay.Text += button.Text;
}

```

* **Alur:** Apabila layar berisi nilai `"0"`, input pertama akan menggantikan angka tersebut. Jika tidak, karakter angka baru ditempelkan (*append*) di sebelah kanan.

#### B. Pemilihan Operator Aritmetika

```
private void OperatorButton_Click(object sender, EventArgs e)
{
    Button button = (Button)sender;
    firstNumber = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
    operation = button.Text;
    txtDisplay.Clear();
}

```

* **Alur:** Nilai pada `txtDisplay` dikonversi ke `double` lalu disimpan di `firstNumber`. Jenis operasi diset sesuai teks tombol, lalu layar dibersihkan untuk menerima nilai `secondNumber`.

#### C. Kalkulasi dan Handling Error Pembagian Nol

```
private void btnEquals_Click(object sender, EventArgs e)
{
    try
    {
        secondNumber = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

        switch (operation)
        {
            case "+": result = firstNumber + secondNumber; break;
            case "-": result = firstNumber - secondNumber; break;
            case "×": result = firstNumber * secondNumber; break;
            case "÷":
                if (secondNumber == 0)
                    throw new DivideByZeroException();
                result = firstNumber / secondNumber;
                break;
        }

        string historyEntry = $"{firstNumber} {operation} {secondNumber} = {result}";
        lstHistory.Items.Insert(0, historyEntry);

        txtDisplay.Text = result.ToString();
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Error");
    }
}

```

* **Fungsi:** Menghitung `firstNumber` dan `secondNumber` sesuai variabel `operation`.

* **Keamanan Kode:** Menggunakan proteksi `try-catch`. Jika pengguna membagi dengan `0`, aplikasi melemparkan `DivideByZeroException` dan menampilkan `MessageBox` tanpa menyebabkan *crash*.

* **Manajemen Riwayat:** Hasil transaksi diformat menjadi string dan dimasukkan ke baris paling atas `lstHistory` menggunakan `lstHistory.Items.Insert(0, ...)`.

#### D. Operasi Tambahan (Backspace, PlusMinus, Percent)

1. **Backspace (`btnBackspace_Click`):**
   Memotong 1 karakter dari kanan menggunakan `Substring`. Jika teks tersisa kosong atau hanya berupa tanda minus (`"-"`), nilai layar dikembalikan otomatis ke `"0"`.

2. **PlusMinus (`btnPlusMinus_Click`):**
   Mengalikan nilai aktif dengan $-1$ untuk membalikkan tanda bilangan.

3. **Persentase (`btnPercent_Click`):**
   Membagi nilai aktif pada layar dengan $100$.

#### E. Interaksi Riwayat (`lstHistory_SelectedIndexChanged`)

Pengguna dapat memilih/mengklik baris riwayat pada `ListBox` untuk mengambil kembali hasil perhitungan terdahulu:

```
string selectedText = lstHistory.SelectedItem.ToString()!;
string[] parts = selectedText.Split('=');
if (parts.Length > 1)
{
    txtDisplay.Text = parts[1].Trim();
}

```

* **Alur:** Teks seperti `"15 + 5 = 20"` dipecah berdasarkan karakter `'='`, kemudian bagian hasil (`"20"`) disalin kembali ke `txtDisplay`.

## 4. Pengujian Fitur & Skenario Usability

| No | Skenario Pengujian | Input | Ekspektasi Hasil | Status | 
| ----- | ----- | ----- | ----- | ----- | 
| 1 | Operasi Penjumlahan | $12 + 8 =$ | Display: `20`, Riwayat: `12 + 8 = 20` | Lolos | 
| 2 | Pembagian dengan Nol | $10 \div 0 =$ | Pesan Error Pop-up (*DivideByZeroException*) | Lolos | 
| 3 | Input Desimal Ganda | $5 . 2 . 1$ | Display: `5.21` (Mencegah desimal kedua) | Lolos | 
| 4 | Fitur Backspace | Input `123`, tekan Backspace | Display: `12` | Lolos | 
| 5 | Fitur Plus/Minus | Input `5`, tekan $\pm$ | Display: `-5` | Lolos | 
| 6 | Memanggil Riwayat | Klik dua kali baris `"15 + 5 = 20"` | Display: `20` | Lolos | 
