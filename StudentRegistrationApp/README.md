# Laporan Dokumentasi Aplikasi: Student Registration (WPF)

## 1. Pendahuluan
Aplikasi **Student Registration** adalah aplikasi desktop berbasis *Windows Presentation Foundation (WPF)* yang menggunakan bahasa C# untuk logika di balik layar.
Aplikasi ini dirancang dengan tata letak dua kolom untuk memudahkan pengguna melakukan input data mahasiswa sekaligus melihat daftar data yang telah dimasukkan.

---

## 2. Desain Antarmuka Pengguna (`MainWindow.xaml`)
Antarmuka dibagi menjadi dua bagian utama menggunakan elemen `Grid` dengan dua kolom, dipisahkan oleh spasi:

* **Kolom Kiri (Form Input):**
  Berisi elemen-elemen untuk memasukkan biodata mahasiswa:
  * **NIM (`txtNim`):** Kotak teks (`TextBox`) untuk memasukkan nomor induk mahasiswa.
  * **Nama Mahasiswa (`txtNama`):** Kotak teks (`TextBox`) untuk memasukkan nama lengkap.
  * **Program Studi (`cmbProdi`):** Kotak pilihan (`ComboBox`) yang menyediakan pilihan program studi *(Teknik Informatika, Sistem Informasi, Manajemen, Akuntansi)*.
  * **Jenis Kelamin (`rbLaki` & `rbPerempuan`):** Tombol pilihan ganda (`RadioButton`) untuk memilih Laki-laki atau Perempuan.
  * **Tombol Aksi:** Tiga tombol utama yaitu **Simpan** (`BtnSimpan_Click`), **Reset** (`BtnReset_Click`), dan **Hapus** (`BtnHapus_Click`).

* **Kolom Kanan (Tampilan Data):**
  * **Data Mahasiswa (`lstMahasiswa`):** Komponen daftar (`ListBox`) yang berfungsi untuk menampung dan menampilkan rekapitulasi data mahasiswa yang berhasil disimpan.

Berikut adalah kerangka utama dari struktur XAML:

```xml
<Window ... Title="Student Registration" ...>
    <Grid Margin="30">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="20"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <!-- FORM INPUT -->
        <StackPanel Grid.Column="0">
            <TextBlock Text="STUDENT REGISTRATION" ... />
            ...
            <TextBox x:Name="txtNim" ... />
            <TextBox x:Name="txtNama" ... />
            <ComboBox x:Name="cmbProdi" ... />
            ...
            <StackPanel Orientation="Horizontal">
                <Button Content="Simpan" Click="BtnSimpan_Click" ... />
                <Button Content="Reset" Click="BtnReset_Click" ... />
                <Button Content="Hapus" Click="BtnHapus_Click" ... />
            </StackPanel>
        </StackPanel>

        <!-- DATA VIEW -->
        <StackPanel Grid.Column="2">
            <TextBlock Text="DATA MAHASISWA" ... />
            <ListBox x:Name="lstMahasiswa" ... />
        </StackPanel>
    </Grid>
</Window>
```

---

## 3. Logika dan Kontrol Program (`MainWindow.xaml.cs`)

File *code-behind* mengatur seluruh interaksi pengguna terhadap komponen UI yang ada pada XAML. Berikut adalah bagian-bagian penting beserta fungsi yang bekerja:

### A. Validasi dan Penyimpanan Data (`BtnSimpan_Click`)
Fungsi ini dipicu ketika tombol **Simpan** ditekan. Alur kerjanya meliputi:
1. Memeriksa apakah `txtNim` kosong atau hanya berisi spasi. Jika kosong, muncul peringatan `MessageBox`.
2. Memastikan program studi pada `cmbProdi` sudah dipilih.
3. Memastikan salah satu `RadioButton` jenis kelamin telah dicentang.
4. Mengambil nilai dari masing-masing elemen, menggabungkannya ke dalam format string tunggal, lalu memasukkannya ke dalam `lstMahasiswa`.

```csharp
private void BtnSimpan_Click(object sender, RoutedEventArgs e)
{
    if(string.IsNullOrWhiteSpace(txtNim.Text))
    {
        MessageBox.Show("NIM harus diisi!");
        txtNim.Focus();
        return;
    }

    // ... validasi ComboBox dan RadioButton ...

    string nim = txtNim.Text;
    string nama = txtNama.Text;
    string prodi = "";
    
    if (cmbProdi.SelectedItem is ComboBoxItem item)
    {
        prodi = item.Content.ToString();
    }

    string jenisKelamin = (rbLaki.IsChecked == true) ? "Laki-laki" : "Perempuan";

    string data = $"{nim} | {nama} | {prodi} | {jenisKelamin}";
    lstMahasiswa.Items.Add(data);

    MessageBox.Show("Data mahasiswa berhasil disimpan!", ...);
}
```

### B. Reset Form (`BtnReset_Click`)
Fungsi ini membersihkan seluruh isian form input kembali ke kondisi kosong/semula agar siap menerima input baru.

```csharp
private void BtnReset_Click(object sender, RoutedEventArgs e)
{
    txtNim.Clear();
    txtNama.Clear();
    cmbProdi.SelectedIndex = -1;
    rbLaki.IsChecked = false;
    rbPerempuan.IsChecked = false;
    txtNim.Focus();
}
```

### C. Penghapusan Data (`BtnHapus_Click`)
Fungsi ini mendeteksi item mana yang sedang dipilih oleh pengguna di dalam `ListBox` (`lstMahasiswa`), lalu menghapusnya dari daftar. Jika belum ada yang dipilih, sistem akan menampilkan peringatan.

```csharp
private void BtnHapus_Click(object sender, RoutedEventArgs e)
{
    if(lstMahasiswa.SelectedItem != null)
    {
        lstMahasiswa.Items.Remove(lstMahasiswa.SelectedItem);
    } 
    else
    {
        MessageBox.Show("Pilih data yang ingin dihapus!");
    }
}
```

---
*Catatan: Source code lengkap dari aplikasi ini dapat disatukan pada bagian lampiran laporan.*
