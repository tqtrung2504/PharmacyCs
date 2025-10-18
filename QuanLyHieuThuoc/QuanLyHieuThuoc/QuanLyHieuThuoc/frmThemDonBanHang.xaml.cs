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
using System.Data;
using System.Data.SqlClient;
using QuanLyHieuThuoc.DTO;

namespace QuanLyHieuThuoc
{
    /// <summary>
    /// Interaction logic for frmThemDonBanHang.xaml
    /// </summary>
    public partial class frmThemDonBanHang : Window
    {
        private int idDonBanHang;

        public int IdDonBanHang { get => idDonBanHang; set => idDonBanHang = value; }

        public frmThemDonBanHang()
        {
            InitializeComponent();
        }
        public frmThemDonBanHang(int idDonBanHang)
        {
            this.IdDonBanHang = idDonBanHang;
            InitializeComponent();
        }
        private void LoadGrid()
        {
            DataTable dataCombobox = DataProvider.Instance.ExecuteQuery("Select * From Thuoc Order by TenThuoc ASC");
            foreach (DataRow row in dataCombobox.Rows)
            {
                string newAdd = (string)row["TenThuoc"];
                cboThuoc.Items.Add(newAdd);
            }
        }
        private void HuyThemBanHang(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ThemBanHang(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text))
            {
                MessageBox.Show("Số lượng bán không được bỏ trống!");
                return;
            }

            if (cboThuoc.SelectedValue == null)
            {
                MessageBox.Show("Bạn phải chọn thuốc!");
                return;
            }

            int soLuongBan;
            if (!int.TryParse(txtSoLuong.Text, out soLuongBan) || soLuongBan <= 0)
            {
                MessageBox.Show("Số lượng bán phải là số nguyên dương!");
                return;
            }

            // Tìm ID thuốc
            string cbSeclected = cboThuoc.SelectedValue.ToString();
            DataTable table = DataProvider.Instance.ExecuteQuery("Select IdThuoc, SoLuong from Thuoc Where TenThuoc = N'" + cbSeclected + "'");
            DataRow item = table.Rows[0];
            int idThuoc = (int)item["idThuoc"];
            int soLuongTon = (int)item["SoLuong"];

            if (soLuongBan > soLuongTon)
            {
                MessageBox.Show("Số lượng bán vượt quá số lượng tồn (Tồn: " + soLuongTon + ")!");
                return;
            }

            try
            {
                // Sử dụng transaction để đảm bảo tính nhất quán dữ liệu
                // Kiểm tra và cập nhật số lượng trong một giao dịch duy nhất
                string updateQuery = @"
                    UPDATE Thuoc 
                    SET SoLuong = SoLuong - @soLuongBan 
                    WHERE IdThuoc = @idThuoc 
                    AND SoLuong >= @soLuongBan";
                
                int rowsAffected = DataProvider.Instance.ExecuteNonQuery(updateQuery, 
                    new object[] { soLuongBan, idThuoc });

                if (rowsAffected == 0)
                {
                    MessageBox.Show("Số lượng bán vượt quá số lượng tồn kho hiện tại! Vui lòng kiểm tra lại.");
                    return;
                }

                // Thêm chi tiết đơn bán hàng
                DataProvider.Instance.ExecuteQuery("insert ChiTietDonBanHang values (" + IdDonBanHang + "," + idThuoc.ToString() + "," + soLuongBan.ToString() + ")");
                
                MessageBox.Show("Thêm đơn bán hàng thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm đơn bán hàng: " + ex.Message);
            }
        }

        private void ThemThuoc(object sender, RoutedEventArgs e)
        {
            frmThemThuoc newfrmThemThuoc = new frmThemThuoc();
            newfrmThemThuoc.ShowDialog();
            LoadGrid();
        }

        private void LoadGridThuoc(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }
    }
}
