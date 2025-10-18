using System;
using System.Data;
using System.Windows;

namespace QuanLyHieuThuoc
{
    public partial class frmTimPhieuNhap : Window
    {
        public int IdPhieuNhapKhoChon { get; private set; }
        public frmTimPhieuNhap()
        {
            InitializeComponent();
            Loaded += FrmTimPhieuNhap_Loaded;
        }

        private void FrmTimPhieuNhap_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select IdPhieuNhapKho, NgayNhap from PhieuNhapKho Order by IdPhieuNhapKho DESC");
            lsvPhieuNhap.ItemsSource = data.DefaultView;
            if (lsvPhieuNhap.Items.Count > 0) lsvPhieuNhap.SelectedIndex = 0;
        }

        private void Chon(object sender, RoutedEventArgs e)
        {
            if (lsvPhieuNhap.SelectedItem is System.Data.DataRowView row)
            {
                IdPhieuNhapKhoChon = Convert.ToInt32(row["IdPhieuNhapKho"]);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Bạn phải chọn 1 phiếu nhập!");
            }
        }

        private void Dong(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
