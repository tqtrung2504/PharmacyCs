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
    /// Interaction logic for frmSuaThuoc.xaml
    /// </summary>
    public partial class frmSuaThuoc : Window
    {
        private int idt;

        public frmSuaThuoc()
        {
            InitializeComponent();
        }
        public frmSuaThuoc(Thuoc item)
        {
            InitializeComponent();
            txtIdThuoc.Text = item.IdThuoc.ToString();
            Idt = item.IdThuoc;
            txtTenThuoc.Text = item.TenThuoc;
            dtpHanSuDung.Text = item.HanSuDung.ToString();
            txtSoLuong.Text = item.SoLuong.ToString();
            txtGiaNhap.Text = item.GiaNhap.ToString();
            txtGiaBan.Text = item.GiaBan.ToString();
            txtGhiChu.Text = item.GhiChu;
            
            // Load combobox trước, sau đó set selected value
            LoadSuaThuoc(null, null);
            cbDonVi.SelectedItem = item.TenDonVi;
        }

        public int Idt { get => idt; set => idt = value; }

        private void btnHuySuaThuoc(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSuaThuoc(object sender, RoutedEventArgs e)
        {
            if (txtTenThuoc.Text == "" || txtSoLuong.Text == "" || txtGiaBan.Text == "" || txtGiaNhap.Text == "")
            {
                MessageBox.Show("Các trường dấu sao không được bổ trống!");
            }
            else if (cbDonVi.SelectedItem == null)
            {
                MessageBox.Show("Bạn phải chọn đơn vị!");
            }
            else
            {
                try
                {
                    // Đọc đơn vị được chọn ở combobox
                    string cbSelected = cbDonVi.SelectedItem.ToString();
                    // Tìm
                    DataTable idDonVi = DataProvider.Instance.ExecuteQuery("Select IdDonVi from DonVi Where TenDonVi = N'" + cbSelected + "'");
                    if (idDonVi.Rows.Count > 0)
                    {
                        DataRow rowidDonVi = idDonVi.Rows[0];
                        int updateIdDonVi = (int)rowidDonVi["IdDonVi"];
                        DataProvider.Instance.ExecuteQuery("Update Thuoc SET TenThuoc=N'" + txtTenThuoc.Text + "', HanSuDung = '" + dtpHanSuDung.Text + "', SoLuong = " + txtSoLuong.Text + ", IdDonVi = " + updateIdDonVi + ", GiaNhap = " + txtGiaNhap.Text + ", GiaBan=" + txtGiaBan.Text + ", GhiChu=N'" + txtGhiChu.Text + "' WHERE IdThuoc=" + Idt);
                        MessageBox.Show("Cập nhật thuốc thành công!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy đơn vị được chọn!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật thuốc: " + ex.Message);
                }
            }
        }

        private void LoadSuaThuoc(object sender, RoutedEventArgs e)
        {
            DataTable dataCombobox = DataProvider.Instance.ExecuteQuery("Select IdDonVi,TenDonVi From DonVi");
            foreach (DataRow row in dataCombobox.Rows)
            {
                string newAdd = (string)row["TenDonVi"];
                cbDonVi.Items.Add(newAdd);
            }
        }
    }
}
