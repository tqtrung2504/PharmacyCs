using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace QuanLyHieuThuoc
{
    public partial class frmThemTaiKhoan : Window
    {
        public frmThemTaiKhoan()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var data = DataProvider.Instance.ExecuteQuery("Select IdCuaHang, TenCuaHang From CuaHang");
            cbChiNhanh.ItemsSource = data.DefaultView;
            cbChiNhanh.DisplayMemberPath = "TenCuaHang";
            cbChiNhanh.SelectedIndex = data.Rows.Count > 0 ? 0 : -1;
        }

        private void XacNhanThem(object sender, RoutedEventArgs e)
        {
            string tenDangNhap = (txtTenDangNhap.Text ?? string.Empty).Trim();
            string tenHienThi = (txtTenHienThi.Text ?? string.Empty).Trim();
            string soDienThoai = (txtSoDienThoai.Text ?? string.Empty).Trim();
            string matKhau = (txtMatKhau.Password ?? string.Empty).Trim();
            string nhapLaiMatKhau = (txtNhapLaiMatKhau.Password ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(tenDangNhap)
                || string.IsNullOrWhiteSpace(tenHienThi)
                || string.IsNullOrWhiteSpace(soDienThoai)
                || string.IsNullOrWhiteSpace(matKhau)
                || string.IsNullOrWhiteSpace(nhapLaiMatKhau)
                || cbChiNhanh.SelectedItem == null)
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!");
                return;
            }

            if (matKhau != nhapLaiMatKhau)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            // Lấy IdCuaHang theo tên
            var selectedRow = cbChiNhanh.SelectedItem as System.Data.DataRowView;
            int selectedIdCuaHang = Convert.ToInt32(selectedRow["IdCuaHang"]);

            // Mã hóa mật khẩu theo cách login đang dùng
            string passEncoded = frmLogin.MD5Hash(frmLogin.Base64Encode(matKhau));

            // Thêm tài khoản
            string insert = "insert into TaiKhoan(TenDangNhap, TenHienThi, SoDienThoai, IdCuaHang, MatKhau) values(@u, @d, @p, @c, @m)";
            DataProvider.Instance.ExecuteNonQuery(insert, new object[] { tenDangNhap, tenHienThi, soDienThoai, selectedIdCuaHang, passEncoded });
            MessageBox.Show("Tạo tài khoản thành công!");
            DialogResult = true;
        }

        private void Huy(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}


