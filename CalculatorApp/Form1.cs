using System.Globalization;
namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        double firstNumber = 0;
        double secondNumber = 0;
        double result = 0;
        string operation = "";

        public Form1()
        {
            InitializeComponent();
        }
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (txtDisplay.Text == "0")
                txtDisplay.Text = button.Text;
            else
                txtDisplay.Text += button.Text;
        }
        private void OperatorButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            firstNumber = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);
            operation = button.Text;
            txtDisplay.Clear();
        }

        private void btnEquals_Click(object sender, EventArgs e)
        {
            try
            {
                secondNumber = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

                switch (operation)
                {
                    case "+":
                        result = firstNumber + secondNumber;
                        break;
                    case "-":
                        result = firstNumber - secondNumber;
                        break;
                    case "×":
                        result = firstNumber * secondNumber;
                        break;
                    case "÷":
                        if (secondNumber == 0)
                            throw new DivideByZeroException();
                        result = firstNumber / secondNumber;
                        break;
                }
                // Format string riwayat: "15 + 5 = 20"
                string historyEntry = $"{firstNumber} {operation} {secondNumber} = {result}";

                // Masukkan riwayat ke posisi paling atas ListBox
                lstHistory.Items.Insert(0, historyEntry);

                txtDisplay.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            result = 0;
            operation = "";
            txtDisplay.Text = "0";
        }
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
                txtDisplay.Text += ".";
        }

        private void btnPlusMinus_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text != "0" && !string.IsNullOrEmpty(txtDisplay.Text))
            {
                double currentValue = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

                // Kalikan dengan -1 untuk mengubah positif ke negatif atau sebaliknya
                currentValue = currentValue * -1;

                // Tampilkan kembali ke layar
                txtDisplay.Text = currentValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                double currentValue = double.Parse(txtDisplay.Text, CultureInfo.InvariantCulture);

                // Bagi dengan 100 untuk mendapatkan nilai persentase
                currentValue = currentValue / 100;

                txtDisplay.Text = currentValue.ToString(CultureInfo.InvariantCulture);
            }
        }
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (txtDisplay.Text.Length > 0 && txtDisplay.Text != "0")
            {
                // Hapus 1 karakter paling kanan
                txtDisplay.Text = txtDisplay.Text.Substring(0, txtDisplay.Text.Length - 1);

                // Jika setelah dihapus layar jadi kosong atau hanya tersisa tanda minus "-", kembalikan ke "0"
                if (txtDisplay.Text == "" || txtDisplay.Text == "-")
                {
                    txtDisplay.Text = "0";
                }
            }
        }
        private void lstHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstHistory.SelectedItem != null)
            {
                    // Ambil baris yang dipilih, misal "15 + 5 = 20"
                string selectedText = lstHistory.SelectedItem.ToString()!;

                // Ambil angka hasil setelah tanda '='
                string[] parts = selectedText.Split('=');
                if (parts.Length > 1)
                {
                    txtDisplay.Text = parts[1].Trim();
                }
            }
        }
    }
}
