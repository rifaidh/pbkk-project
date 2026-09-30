using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.CSharp.RuntimeBinder;

namespace StudentRegistrationApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    private void BtnSimpan_Click(object sender, RoutedEventArgs e)
    {
        if(string.IsNullOrWhiteSpace(txtNim.Text))
        {
            MessageBox.Show("NIM harus diisi!");
            txtNim.Focus();
            return;
        }

        if(cmbProdi.SelectedItem == null)
        {
            MessageBox.Show("Pilih program studi!");
            return;
        }

        if(rbLaki.IsChecked != true && rbPerempuan.IsChecked != true)
        {
            MessageBox.Show("Pilih jenis kelamin");
            return;
        }

        string nim = txtNim.Text;
        string nama = txtNama.Text;
        string prodi = "";

        if (cmbProdi.SelectedItem is ComboBoxItem item)
        {
            prodi = item.Content.ToString();
        }

        string jenisKelamin = "";

        if (rbLaki.IsChecked == true)
        {
            jenisKelamin = "Laki-laki";
        }
        else if (rbPerempuan.IsChecked == true)
        {
            jenisKelamin = "Perempuan";

        }

        string data =
        $"{nim} | {nama} | {prodi} | {jenisKelamin}";

        lstMahasiswa.Items.Add(data);

        MessageBox.Show(
        "Data mahasiswa berhasil disimpan!",
        "Informasi",
        MessageBoxButton.OK,
        MessageBoxImage.Information);
    }

    private void BtnReset_Click(object sender, RoutedEventArgs e)
    {
        txtNim.Clear();
        txtNama.Clear();

        cmbProdi.SelectedIndex = -1;

        rbLaki.IsChecked = false;
        rbPerempuan.IsChecked = false;

        txtNim.Focus();
    }

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
}