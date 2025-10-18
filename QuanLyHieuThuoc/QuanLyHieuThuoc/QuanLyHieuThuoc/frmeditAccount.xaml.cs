using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace QuanLyHieuThuoc
{
    /// <summary>
    /// Interaction logic for editAccount.xaml
    /// </summary>
    public partial class editAccount : Window
    {
        private string _tk;

        public string Tk { get => _tk; set => _tk = value; }

        public editAccount()
        {
            InitializeComponent();
        }

        private void CapNhatThongTin()
        {
            // Đọc dữ liệu được chọn ở combobox
            string tenCuaHang = cbChiNhanh.SelectedItem != null ? cbChiNhanh.SelectedItem.ToString() : string.Empty;
            if (string.IsNullOrWhiteSpace(tenCuaHang))
            {
                MessageBox.Show("Vui lòng chọn chi nhánh!");
                return;
            }
            // Truy vấn tên của hàng đó
            DataTable idCuaHang =  DataProvider.Instance.ExecuteQuery("Select IdCuaHang from CuaHang Where TenCuaHang = N'" + tenCuaHang +"'");
            DataRow rowidCuaHang = idCuaHang.Rows[0];
            int updateIdCuaHang = (int)rowidCuaHang["IdCuaHang"];
            
            // Cập nhật thông tin cơ bản
            string updateQuery = "UPDATE TaiKhoan SET TenDangNhap = N'" + txtEditTenDangNhap.Text + "', TenHienThi = N'" + txtEditTenHienThi.Text + "', SoDienThoai = N'" + txtEditSoDienThoai.Text + "', IdCuaHang = " + updateIdCuaHang;
            
          
            
            updateQuery += " WHERE TenDangNhap = N'" + Tk + "'";
            
            DataProvider.Instance.ExecuteQuery(updateQuery);
            MessageBox.Show("Cập nhật thành công!");
            this.Close();
            
        }
        private void XacNhanSuaTaiKhoan(object sender, RoutedEventArgs e)
        {
            // Kiểm tra form trống
            if(txtEditTenDangNhap.Text=="" || txtEditTenHienThi.Text=="" || txtEditSoDienThoai.Text=="")
            {
                MessageBox.Show("Bạn phải nhập đầy đủ thông tin!");
            }
            else
            {
                CapNhatThongTin();
            }

        }

        private void EditAcountLoad(object sender, RoutedEventArgs e)
        {
            DataTable dataCombobox = DataProvider.Instance.ExecuteQuery("Select IdCuaHang,TenCuaHang From CuaHang");
            foreach(DataRow row in dataCombobox.Rows)
            {
                string newAdd = (string)row["TenCuaHang"];
                cbChiNhanh.Items.Add(newAdd);
            }
        }

        private void cHuy(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
