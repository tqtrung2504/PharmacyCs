using System;
using System.Data;
using System.Windows;

namespace QuanLyHieuThuoc
{
    public partial class frmTimDonBan : Window
    {
        public int IdDonBanHangChon { get; private set; }
        public frmTimDonBan()
        {
            InitializeComponent();
            Loaded += FrmTimDonBan_Loaded;
        }

        private void FrmTimDonBan_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select IdDonBanHang, NgayBan from DonBanHang Order by IdDonBanHang DESC");
            lsvDonBan.ItemsSource = data.DefaultView;
            if (lsvDonBan.Items.Count > 0) lsvDonBan.SelectedIndex = 0;
        }

        private void Chon(object sender, RoutedEventArgs e)
        {
            if (lsvDonBan.SelectedItem is System.Data.DataRowView row)
            {
                IdDonBanHangChon = Convert.ToInt32(row["IdDonBanHang"]);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Bạn phải chọn 1 đơn bán!");
            }
        }

        private void Dong(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
