using QuanLyHieuThuoc.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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


namespace QuanLyHieuThuoc
{
    /// <summary>
    /// Interaction logic for frmMain.xaml
    /// </summary>
    public partial class frmMain : Window
    {
        private string _tk;
        private string _mk;
        private int idPhieuNhapKho;
        private int idDonBanHang;
        private int idCuaHang;
        public string TK
        {
            get { return _tk; }
            set { _tk = value; }
        }
        public string MK
        {
            get { return _mk; }
            set { _mk = value; }
        }

        public int IdPhieuNhapKho { get => idPhieuNhapKho; set => idPhieuNhapKho = value; }
        public int IdDonBanHang { get => idDonBanHang; set => idDonBanHang = value; }
        public int IdCuaHang { get => idCuaHang; set => idCuaHang = value; }

        public frmMain()
        {
            InitializeComponent();
            LoadThuoc();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        #region Methods
        private void HienThiThongTinTaiKhoan()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select t.tenhienthi, t.sodienthoai, c.tencuahang, t.idcuahang from TaiKhoan as t join CuaHang as c on t.idcuahang=c.idcuahang where tendangnhap like N'" + TK + "'");
            DataRow row = data.Rows[0];
            txtTenDangNhap.Text = TK;
            txtTenHienThi.Text = (string)row["tenhienthi"];
            txtSoDienThoai.Text = (string)row["sodienthoai"];
            txtChiNhanh.Text = (string)row["tencuahang"];
            IdCuaHang = (int)row["idcuahang"];

        }
        
        private void NapListViewTaiKhoan()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select t.TenDangNhap, t.TenHienThi, t.SoDienThoai, c.TenCuaHang as TenCuaHang from TaiKhoan t join CuaHang c on t.IdCuaHang = c.IdCuaHang order by t.TenDangNhap");
            var items = new List<dynamic>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new
                {
                    TenDangNhap = (string)row["TenDangNhap"],
                    TenHienThi = (string)row["TenHienThi"],
                    SoDienThoai = (string)row["SoDienThoai"],
                    TenCuaHang = (string)row["TenCuaHang"]
                });
            }
            lsvTaiKhoan.ItemsSource = items;
            if (lsvTaiKhoan.Items.Count >= 1)
                lsvTaiKhoan.SelectedIndex = 0;
        }
        private void NapListViewCuaHang()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from CuaHang");
            List<CuaHang> items = new List<CuaHang>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new CuaHang(row));
            }
            lsvCuaHang.ItemsSource = items;
            if (lsvCuaHang.Items.Count >= 1)
                lsvCuaHang.SelectedIndex = 0;
        }
        private void NapListViewDonVi()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select * from DonVi");
            List<DonVi> items = new List<DonVi>();
            foreach (DataRow row in data.Rows)
            {
                items.Add(new DonVi(row));
            }
            lsvDonVi.ItemsSource = items;
            if (lsvDonVi.Items.Count >= 1)
                lsvDonVi.SelectedItem = 0;
        }
        private void NapListViewThuoc()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("select IdThuoc,TenThuoc,HanSuDung,SoLuong,TenDonVi,GiaNhap,GiaBan,GhiChu from Thuoc Join DonVi on Thuoc.IdDonVi = DonVi.IdDonVi order by TenThuoc ASC");
            SetThuocItemsSource(data);  // Sử dụng DataView thay vì List<T>
            if (lsvThuoc.Items.Count >= 1)
                lsvThuoc.SelectedItem = 0;
        }
        private void NapListViewNhapKho()
        {
            if (IdPhieuNhapKho != 0)
            {
                decimal tongTienNhap = 0;
                List<PhieuNhapThuoc> items = new List<PhieuNhapThuoc>();
                DataTable pnt = DataProvider.Instance.ExecuteQuery("select IdChiTietPhieuNhapKho,Thuoc.IdThuoc,TenThuoc,SoLuongNhap,TenDonVi,HanSuDung,GiaNhap, GiaNhap*SoLuongNhap as ThanhTien from PhieuNhapKho join ChiTietPhieuNhapKho on PhieuNhapKho.IdPhieuNhapKho=ChiTietPhieuNhapKho.IdPhieuNhapKho join Thuoc on ChiTietPhieuNhapKho.IdThuoc=Thuoc.IdThuoc join DonVi on Thuoc.IdDonVi = DonVi.IdDonVi Where PhieuNhapKho.IdPhieuNhapKho=" + IdPhieuNhapKho.ToString() + " Order by IdChitietPhieuNhapKho DESC");
                foreach (DataRow row in pnt.Rows)
                {
                    tongTienNhap += (decimal)row["ThanhTien"];
                    items.Add(new PhieuNhapThuoc(row));
                }
                lsvPhieuNhapKho.ItemsSource = items;
                if (lsvPhieuNhapKho.Items.Count >= 1)
                    lsvPhieuNhapKho.SelectedItem = 0;
                txtTongTienNhap.Text = tongTienNhap.ToString("N0") + ".000";
            }

        }
        private void NapListViewBanHang()
        {
            if (IdDonBanHang != 0)
            {
                decimal tongTienBan = 0;
                List<DonBanHang> items = new List<DonBanHang>();
                DataTable pnt = DataProvider.Instance.ExecuteQuery("select IdChiTietDonBanHang,Thuoc.IdThuoc,TenThuoc,SoLuongBan,TenDonVi,HanSuDung,GiaBan, GiaBan*SoLuongBan as ThanhTien from DonBanHang join ChiTietDonBanHang on DonBanHang.IdDonBanHang=ChiTietDonBanHang.IdDonBanHang join Thuoc on ChiTietDonBanHang.IdThuoc=Thuoc.IdThuoc join DonVi on Thuoc.IdDonVi = DonVi.IdDonVi Where DonBanHang.IdDonBanHang=" + IdDonBanHang.ToString() + " Order by IdChitietDonBanHang DESC");
                foreach (DataRow row in pnt.Rows)
                {
                    tongTienBan += (decimal)row["ThanhTien"];
                    items.Add(new DonBanHang(row));
                }
                lsvHoaDonBan.ItemsSource = items;
                if (lsvHoaDonBan.Items.Count >= 1)
                    lsvHoaDonBan.SelectedItem = 0;
                txtTongTienThu.Text = tongTienBan.ToString("N0") + ".000";
            }
        }

        #endregion

        private void lbThoat(object sender, MouseButtonEventArgs e)
        {
            var diaThoat = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Hệ thống quản lý cửa hàng bán thuốc", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == diaThoat)
                this.Close();
        }
        #region Events
        private void BtnThoat_Click(object sender, RoutedEventArgs e)
        {
            var closeMain = MessageBox.Show("Bạn có muốn đăng xuất không?", "Hệ thống quản lý cửa hàng bán thuốc", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (MessageBoxResult.Yes == closeMain)
            {
                this.Hide();
                frmLogin newLogin = new frmLogin();
                newLogin.ShowDialog();
                //System.Windows.Application.Current.Shutdown();
                //this.Close();
            }

        }

        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            tabTrangChu.Visibility = Visibility.Visible;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Hidden;
            //MessageBox.Show(DataProvider.Ins.DB.TaiKhoans.First().TenDangNhap);
        }

        private void BtnNhapKho_Click(object sender, RoutedEventArgs e)
        {
            dtpNgayLamViecNhapKho.Text = DateTime.Now.ToString();
            // Hiển thị thông tin cửa hàng
            DataTable table = DataProvider.Instance.ExecuteQuery("select TenCuaHang from TaiKhoan join CuaHang on TaiKhoan.IdCuaHang = CuaHang.IdCuaHang where TenDangNhap = N'" + TK + "'");
            DataRow row = table.Rows[0];
            txtCuaHangNhapKho.Text = (string)row["TenCuaHang"];
            NapListViewNhapKho();
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Visible;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Hidden;
        }

        private void BtnBanHang_Click(object sender, RoutedEventArgs e)
        {
            dtpNgayLamViecBanHang.Text = DateTime.Now.ToString();
            DataTable table = DataProvider.Instance.ExecuteQuery("select TenCuaHang from TaiKhoan join CuaHang on TaiKhoan.IdCuaHang = CuaHang.IdCuaHang where TenDangNhap = N'" + TK + "'");
            DataRow row = table.Rows[0];
            txtCuaHangLamViec.Text = (string)row["TenCuaHang"];
            NapListViewBanHang();
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Visible;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Hidden;
        }

        private void BtnThuoc_Click(object sender, RoutedEventArgs e)
        {
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Visible;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Hidden;
            NapListViewDonVi();
            NapListViewThuoc();
        }

        private void BtnCuaHang_Click(object sender, RoutedEventArgs e)
        {
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Visible;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Hidden;
            NapListViewCuaHang();
        }

        private void BtnBaoCao_Click(object sender, RoutedEventArgs e)
        {
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Visible;
            tabTaiKhoan.Visibility = Visibility.Hidden;
        }

        private void BtnTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            tabTrangChu.Visibility = Visibility.Hidden;
            tabNhapKho.Visibility = Visibility.Hidden;
            tabBanHang.Visibility = Visibility.Hidden;
            tabThuoc.Visibility = Visibility.Hidden;
            tabCuaHang.Visibility = Visibility.Hidden;
            tabBaoCao.Visibility = Visibility.Hidden;
            tabTaiKhoan.Visibility = Visibility.Visible;
            NapListViewTaiKhoan();
        }

        private void loadData(object sender, RoutedEventArgs e)
        {
            HienThiThongTinTaiKhoan();
        }

        private void SuaTaiKhoan(object sender, RoutedEventArgs e)
        {

            editAccount frmEditAccount = new editAccount();
            frmEditAccount.Tk = TK;
            frmEditAccount.ShowDialog();
            HienThiThongTinTaiKhoan();

        }

        private void DoiMatKhau(object sender, RoutedEventArgs e)
        {
            frmDoiMatKhau frmDoiMk = new frmDoiMatKhau();
            frmDoiMk.Mk = MK;
            frmDoiMk.Tk = TK;
            frmDoiMk.ShowDialog();
        }
        
        private void TaoTaiKhoan(object sender, RoutedEventArgs e)
        {
            var wnd = new frmThemTaiKhoan();
            var ok = wnd.ShowDialog();
            if (ok == true)
            {
                NapListViewTaiKhoan();
            }
        }

        private void SuaTaiKhoanAdmin(object sender, RoutedEventArgs e)
        {
            if (lsvTaiKhoan.Items.Count >= 1 && lsvTaiKhoan.SelectedItem != null)
            {
                dynamic item = lsvTaiKhoan.SelectedItem;
                editAccount frmEditAccount = new editAccount();
                frmEditAccount.Tk = item.TenDangNhap;
                frmEditAccount.ShowDialog();
                NapListViewTaiKhoan();
            }
            else
            {
                MessageBox.Show("Bạn phải chọn tài khoản để sửa!");
            }
        }

        private void XoaTaiKhoan(object sender, RoutedEventArgs e)
        {
            if (lsvTaiKhoan.Items.Count >= 1 && lsvTaiKhoan.SelectedItem != null)
            {
                dynamic item = lsvTaiKhoan.SelectedItem;
                var dialogRes = MessageBox.Show("Bạn có muốn xóa tài khoản " + item.TenDangNhap + " không?", "Xóa tài khoản", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (dialogRes == MessageBoxResult.Yes)
                {
                    DataProvider.Instance.ExecuteQuery("delete from TaiKhoan where TenDangNhap = N'" + item.TenDangNhap + "'");
                    NapListViewTaiKhoan();
                }
            }
            else
            {
                MessageBox.Show("Bạn phải chọn tài khoản để xóa!");
            }
        }
        #endregion

        #region Other
        private void ThemCuaHang(object sender, RoutedEventArgs e)
        {
            frmThemCuaHang newfrmThemCuaHang = new frmThemCuaHang();
            newfrmThemCuaHang.ShowDialog();
            NapListViewCuaHang();
        }

        private void SuaCuaHang(object sender, RoutedEventArgs e)
        {
            if (lsvCuaHang.Items.Count >= 1)
            {
                if (lsvCuaHang.SelectedItem != null)
                {
                    CuaHang item = (CuaHang)lsvCuaHang.SelectedItem;
                    frmSuaCuaHang newfrmSuaCuaHang = new frmSuaCuaHang(item.IdCuaHang, item.TenCuaHang, item.DiaChi);
                    newfrmSuaCuaHang.ShowDialog();
                    NapListViewCuaHang();
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn cửa hàng để sửa!");
                }
            }
            else
            {
                MessageBox.Show("Không có cửa hàng nào được chọn để sửa!");
            }
        }

        private void XoaCuaHang(object sender, RoutedEventArgs e)
        {
            if (lsvCuaHang.Items.Count >= 1)
            {
                if (lsvCuaHang.SelectedItem != null)
                {
                    CuaHang item = (CuaHang)lsvCuaHang.SelectedItem;
                    var dialogRes = MessageBox.Show("Bạn có muốn xóa cửa hàng " + item.TenCuaHang + " không?", "Xóa cửa hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MessageBoxResult.Yes == dialogRes)
                    {
                        DataProvider.Instance.ExecuteQuery("delete from CuaHang where IdCuaHang = " + item.IdCuaHang);
                    }
                    NapListViewCuaHang();
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn cửa hàng để xóa!");
                }
            }
            else
            {
                MessageBox.Show("Chưa có cửa hàng được chọn để xóa");
            }
        }

        private void ThemDonVi(object sender, RoutedEventArgs e)
        {
            frmThemDonVi newfrmThemDonVi = new frmThemDonVi();
            newfrmThemDonVi.ShowDialog();
            NapListViewDonVi();
        }

        private void SuaDonVi(object sender, RoutedEventArgs e)
        {
            if (lsvDonVi.Items.Count >= 1)
            {
                if (lsvDonVi.SelectedItem != null)
                {
                    DonVi item = (DonVi)lsvDonVi.SelectedItem;
                    frmSuaDonVi newfrmSuaDonVi = new frmSuaDonVi(item.IdDonvi, item.TenDonVi);
                    newfrmSuaDonVi.ShowDialog();
                    NapListViewDonVi();
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn đơn vị để sửa!");
                }
            }
            else
            {
                MessageBox.Show("Chưa có đơn vị được chọn để sửa!");
            }
        }

        private void XoadonVi(object sender, RoutedEventArgs e)
        {
            if (lsvDonVi.Items.Count >= 1)
            {
                if (lsvDonVi.SelectedItem != null)
                {
                    DonVi item = (DonVi)lsvDonVi.SelectedItem;
                    var dialogRes = MessageBox.Show("Bạn có muốn xóa đơn vị " + item.TenDonVi + " không?", "Xóa đơn vị", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (MessageBoxResult.Yes == dialogRes)
                    {
                        DataProvider.Instance.ExecuteQuery("delete from DonVi where  IdDonVi = " + item.IdDonvi);
                    }
                    NapListViewDonVi();
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn 1 đơn vị để xóa!");
                }
            }
            else
            {
                MessageBox.Show("Chưa có đơn vị được chọn để xóa!");
            }
        }

        private void ThemThuoc(object sender, RoutedEventArgs e)
        {
            frmThemThuocKhongSoLuong wd = new frmThemThuocKhongSoLuong();
            wd.ShowDialog();
            NapListViewThuoc();
        }

        private void SuaThuoc(object sender, RoutedEventArgs e)
        {
            if (lsvThuoc.Items.Count >= 1)
            {
                if (lsvThuoc.SelectedItem != null)
                {
                    // Xử lý SelectedItem từ DataView
                    if (lsvThuoc.SelectedItem is DataRowView drv)
                    {
                        Thuoc item = new Thuoc(drv.Row);
                        frmSuaThuoc newfrmSuaThuoc = new frmSuaThuoc(item);
                        newfrmSuaThuoc.ShowDialog();
                        NapListViewThuoc();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không thể đọc dữ liệu thuốc được chọn!");
                    }
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn thuốc để sửa!");
                }
            }
            else
            {
                MessageBox.Show("Danh sách thuốc rỗng!");
            }
        }

        private void XoaThuoc(object sender, RoutedEventArgs e)
        {
            if (lsvThuoc.Items.Count >= 1)
            {
                if (lsvThuoc.SelectedItem != null)
                {
                    // Xử lý SelectedItem từ DataView
                    if (lsvThuoc.SelectedItem is DataRowView drv)
                    {
                        Thuoc item = new Thuoc(drv.Row);
                        MessageBoxResult dialogRes = MessageBox.Show("Bạn có muốn xóa thuốc " + item.TenThuoc + " không?", "Xóa thuốc", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (MessageBoxResult.Yes == dialogRes)
                        {
                            DataProvider.Instance.ExecuteQuery("delete from Thuoc where IdThuoc = " + item.IdThuoc);
                        }
                        NapListViewThuoc();
                    }
                    else
                    {
                        MessageBox.Show("Lỗi: Không thể đọc dữ liệu thuốc được chọn!");
                    }
                }
                else
                {
                    MessageBox.Show("Bạn phải chọn 1 thuốc để xóa!");
                }
            }
            else
            {
                MessageBox.Show("Danh sách thuốc rỗng!");
            }
        }
        #endregion

        private void ThemNhapKho(object sender, RoutedEventArgs e)
        {
            if (IdPhieuNhapKho != 0)
            {
                frmThemNhapKho newfrmThemNhapKho = new frmThemNhapKho(IdPhieuNhapKho);
                newfrmThemNhapKho.ShowDialog();
                NapListViewNhapKho();
            }
            else
            {
                MessageBox.Show("Bạn chưa tạo hóa đơn nhập kho!");
            }
        }

        private void XoaNhapKho(object sender, RoutedEventArgs e)
        {
            if (IdPhieuNhapKho != 0)
            {
                if (lsvPhieuNhapKho.Items.Count >= 1)
                {
                    if (lsvPhieuNhapKho.SelectedItem != null)
                    {
                        PhieuNhapThuoc item = (PhieuNhapThuoc)lsvPhieuNhapKho.SelectedItem;
                        MessageBoxResult dialogRes = MessageBox.Show("Bạn có chắc chắn muốn xóa item đã chọn không? ", "Xóa chi tiết phiếu nhập kho", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (MessageBoxResult.Yes == dialogRes)
                        {
                            DataProvider.Instance.ExecuteQuery("update Thuoc Set SoLuong = SoLuong - " + item.SoLuongNhap.ToString() + " Where IdThuoc=" + item.IdThuoc.ToString());
                            DataProvider.Instance.ExecuteQuery("delete from ChiTietPhieuNhapKho where IdChiTietPhieuNhapKho = " + item.IdChiTietPhieuNhapKho.ToString());
                        }
                        NapListViewNhapKho();
                    }
                    else
                    {
                        MessageBox.Show("Bạn phải chọn 1 item để xóa!");
                    }
                }
                else
                {
                    MessageBox.Show("Danh sách rỗng!");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa tạo hóa đơn nhập kho!");
            }
        }

        private void TaoHoaDonNhap(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogRes = MessageBox.Show("Bạn có chắc chắn muốn tạo hóa đơn nhập không? ", "Tạo hóa đơn nhập kho", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogRes == MessageBoxResult.Yes)
            {
                MessageBox.Show("Tạo thành công! Mời bạn nhập chi tiết hóa đơn nhập!");
                DataProvider.Instance.ExecuteQuery("insert PhieuNhapKho values ('" + dtpNgayLamViecNhapKho.Text + "'," + IdCuaHang.ToString() + ")");
                // Hiển thị ID
                DataTable table = DataProvider.Instance.ExecuteQuery("Select * from PhieuNhapKho Order by IdPhieuNhapKho DESC");
                DataRow phieuNhapKho = table.Rows[0];
                IdPhieuNhapKho = (int)phieuNhapKho["idPhieuNhapKho"];
                GroupBoxNhapKho.Header = "Nhập kho - Mã phiếu: " + IdPhieuNhapKho.ToString();
                NapListViewNhapKho();
            }
        }

        private void TaoHoaDonBan(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogRes = MessageBox.Show("Bạn có chắc chắn muốn tạo hóa đơn bán không? ", "Tạo hóa đơn bán hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (dialogRes == MessageBoxResult.Yes)
            {
                // Chỉ tạo 1 hóa đơn bán hàng duy nhất
                DataProvider.Instance.ExecuteQuery("insert DonBanHang values ('" + dtpNgayLamViecBanHang.Text + "'," +IdCuaHang.ToString()+ ")");
                // Hiển thị ID
                DataTable table = DataProvider.Instance.ExecuteQuery("Select * from DonBanHang Order by IdDonBanHang DESC");
                DataRow donBanHang = table.Rows[0];
                IdDonBanHang = (int)donBanHang["idDonBanHang"];
                groupDonBanHang.Header = "Bán hàng - Mã phiếu: " + IdDonBanHang.ToString();
                NapListViewBanHang();
            }
        }

        

        private void ThemDonBanHang(object sender, RoutedEventArgs e)
        {
            if (IdDonBanHang != 0)
            {
                frmThemDonBanHang newfrmThemDonBanHang = new frmThemDonBanHang(IdDonBanHang);
                newfrmThemDonBanHang.ShowDialog();
                NapListViewBanHang();
            }
            else
            {
                MessageBox.Show("Bạn chưa tạo hóa đơn bán hàng!");
            }
        }

        private void XoaDonBanHang(object sender, RoutedEventArgs e)
        {
            if (IdDonBanHang != 0)
            {
                if (lsvHoaDonBan.Items.Count >= 1)
                {
                    if (lsvHoaDonBan.SelectedItem != null)
                    {
                        DonBanHang item = (DonBanHang)lsvHoaDonBan.SelectedItem;
                        MessageBoxResult dialogRes = MessageBox.Show("Bạn có chắc chắn muốn xóa item đã chọn không?", "Xóa chi tiết đơn bán hàng", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if(MessageBoxResult.Yes==dialogRes)
                        {
                            DataProvider.Instance.ExecuteQuery("update Thuoc Set SoLuong = SoLuong + " + item.SoLuongBan.ToString() + " Where IdThuoc=" + item.IdThuoc.ToString());
                            DataProvider.Instance.ExecuteQuery("delete from ChiTietDonBanHang where IdChitietDonBanHang = " + item.IdChiTietDonBanHang.ToString());
                        }
                        NapListViewBanHang();
                    }
                    else
                    {
                        MessageBox.Show("Bạn phải chọn 1 item để xóa!");
                    }
                }
                else
                {
                    MessageBox.Show("Danh sách rỗng!");
                }
            }
            else
            {
                MessageBox.Show("Bạn chưa tạo hóa đơn bán hàng!");
            }
        }

        private void BaoCaoNhapKho_Click(object sender, RoutedEventArgs e)
        {
            if (!dtpNgayBatDauBaoCaoNhapKho.SelectedDate.HasValue ||
                !dtpNgayKetThucBaoCaoNhapKho.SelectedDate.HasValue)
            {
                MessageBox.Show("Bạn phải chọn đầy đủ ngày bắt đầu và kết thúc!");
                return;
            }

            // Nếu cột NgayNhap là DATETIME ⇒ dùng [from, to)
            var from = dtpNgayBatDauBaoCaoNhapKho.SelectedDate.Value.Date;
            var toExcl = dtpNgayKetThucBaoCaoNhapKho.SelectedDate.Value.Date.AddDays(1);

            const string sql = @"
                SELECT 
                    CAST(pn.IdCuaHang AS INT) AS IdCuaHang,
                    t.TenThuoc,
                    ct.SoLuongNhap,
                    dv.TenDonVi,
                    t.GiaNhap,
                    pn.NgayNhap,
                    CAST(ISNULL(ct.SoLuongNhap,0) * ISNULL(t.GiaNhap,0) AS money) AS ThanhTien,
                    ct.IdPhieuNhapKho as IdHoaDon
                FROM dbo.ChiTietPhieuNhapKho AS ct
                JOIN dbo.PhieuNhapKho AS pn ON pn.IdPhieuNhapKho = ct.IdPhieuNhapKho
                JOIN dbo.Thuoc        AS t  ON t.IdThuoc        = ct.IdThuoc
                JOIN dbo.DonVi        AS dv ON dv.IdDonVi       = t.IdDonVi
                WHERE pn.NgayNhap >= @from AND pn.NgayNhap < @to
                ORDER BY ct.IdPhieuNhapKho DESC;";

            var table = DataProvider.Instance.ExecuteQuery(
                sql,
                CommandType.Text,
                new SqlParameter("@from", SqlDbType.DateTime) { Value = from },
                new SqlParameter("@to", SqlDbType.DateTime) { Value = toExcl }
            );

            var items = new List<BaoCaoNhap>();
            decimal tongTienNhap = 0m;

            foreach (DataRow row in table.Rows)
            {
                var thanhTien = row.Field<decimal?>("ThanhTien") ?? 0m; 
                tongTienNhap += thanhTien;
                items.Add(new BaoCaoNhap(row)); 
            }

            SetBaoCaoItemsSource(items);
            groupboxBaoCao.Header = $"Báo cáo nhập kho - Tổng tiền nhập: {tongTienNhap:N0}.000";
        }


        private void BaoCaoBanHang_Click(object sender, RoutedEventArgs e)
        {
            if (!dtpNgayBatDauBaoCaoBanHang.SelectedDate.HasValue ||
                !dtpNgayKetThucBaoCaoBanHang.SelectedDate.HasValue)
            {
                MessageBox.Show("Bạn phải chọn đầy đủ ngày bắt đầu và kết thúc!");
                return;
            }

            var startDate = dtpNgayBatDauBaoCaoBanHang.SelectedDate.Value.Date;
            var endDateExclusive = dtpNgayKetThucBaoCaoBanHang.SelectedDate.Value.Date.AddDays(1); // [start, end)

            var sql = @"
                SELECT 
                    t.TenThuoc,
                    ct.SoLuongBan,
                    dv.TenDonVi,
                    t.GiaBan,
                    db.NgayBan,
                    CAST(ISNULL(ct.SoLuongBan,0) * ISNULL(t.GiaBan,0) AS money) AS ThanhTien,
                    ct.IdDonBanHang as IdHoaDon,
                    db.IdCuaHang
                FROM dbo.ChiTietDonBanHang AS ct
                JOIN dbo.DonBanHang     AS db ON db.IdDonBanHang = ct.IdDonBanHang
                JOIN dbo.Thuoc          AS t  ON t.IdThuoc       = ct.IdThuoc
                JOIN dbo.DonVi          AS dv ON dv.IdDonVi      = t.IdDonVi
                WHERE db.NgayBan >= @from AND db.NgayBan < @to
                ORDER BY ct.IdDonBanHang DESC;";

            var table = DataProvider.Instance.ExecuteQuery(
                sql,
                CommandType.Text,
                new SqlParameter("@from", SqlDbType.DateTime) { Value = startDate },
                new SqlParameter("@to", SqlDbType.DateTime) { Value = endDateExclusive }
            );

            var items = new List<BaoCaoBan>();
            decimal tongTienBan = 0m;

            foreach (DataRow row in table.Rows)
            {
                // an toàn với DBNull
                var thanhTien = row.Field<decimal?>("ThanhTien") ?? 0m;
                tongTienBan += thanhTien;

                items.Add(new BaoCaoBan(row)); 
            }

            SetBaoCaoItemsSource(items);
            groupboxBaoCao.Header = $"Báo cáo bán hàng - Tổng tiền thu: {tongTienBan:N0}.000";
        }


        private void DoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new frmDoiMatKhau();
            wnd.Owner = this;
            wnd.ShowDialog();
        }


        private ICollectionView _view;

        private void SetBaoCaoItemsSource(object source)
        {
            IEnumerable enumerable =
                source as IEnumerable
                ?? (source as DataTable)?.DefaultView
                ?? throw new ArgumentException("ItemsSource phải là IEnumerable hoặc DataView");

            lsvBaoCao.ItemsSource = enumerable;
            _view = CollectionViewSource.GetDefaultView(lsvBaoCao.ItemsSource);
            _view.Filter = FilterBaoCao;
            _view.Refresh();
            UpdateTongFromView();
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _view?.Refresh();
            UpdateTongFromView();
        }

        private bool FilterBaoCao(object obj)
        {
            var raw = (txtSearch.Text ?? "").Trim();
            if (string.IsNullOrEmpty(raw)) return true;

            var keyName = Normalize(raw); // dùng cho tên (bỏ dấu, ko phân biệt hoa/thường)
            string ten = "", id = "";

            if (obj is System.Data.DataRowView drv)
            {
                var tbl = drv.Row.Table;
                ten = tbl.Columns.Contains("TenThuoc") ? drv["TenThuoc"]?.ToString() ?? "" : "";

                if (tbl.Columns.Contains("IdHoaDon")) id = drv["IdChungTu"]?.ToString() ?? "";
                else if (tbl.Columns.Contains("IdDonBanHang")) id = drv["IdDonBanHang"]?.ToString() ?? "";
                else if (tbl.Columns.Contains("IdPhieuNhapKho")) id = drv["IdPhieuNhapKho"]?.ToString() ?? "";

            }
            else
            {
                var t = obj.GetType();
                ten = t.GetProperty("TenThuoc")?.GetValue(obj)?.ToString() ?? "";
                id = t.GetProperty("IdHoaDon")?.GetValue(obj)?.ToString()
                   ?? t.GetProperty("IdDonBanHang")?.GetValue(obj)?.ToString()
                   ?? t.GetProperty("IdPhieuNhapKho")?.GetValue(obj)?.ToString()
                   ?? "";
            }

            bool matchTen = Normalize(ten).Contains(keyName);
            bool matchId = id.Contains(raw); 

            return matchTen || matchId;
        }

        private static string Normalize(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var d = s.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(d.Length);
            foreach (var ch in d)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void UpdateTongFromView()
        {
            if (_view == null) return;
            decimal tong = 0m;

            foreach (var item in _view)
            {
                if (item is System.Data.DataRowView drv)
                {
                    if (drv.Row.Table.Columns.Contains("ThanhTien") && drv["ThanhTien"] != DBNull.Value)
                        tong += Convert.ToDecimal(drv["ThanhTien"]);
                }
                else
                {
                    var val = item.GetType().GetProperty("ThanhTien")?.GetValue(item);
                    if (val != null) tong += Convert.ToDecimal(val);
                }
            }

            groupboxBaoCao.Header = $"Báo cáo - Tổng: {tong:N0}.000 vnđ";
        }


        private void LoadThuoc()
        {
            var dtThuoc = DataProvider.Instance.ExecuteQuery(@"
        SELECT t.IdThuoc, t.TenThuoc, t.HanSuDung, t.SoLuong,
               t.GiaNhap, t.GiaBan, dv.TenDonVi, t.GhiChu
        FROM dbo.Thuoc t
        LEFT JOIN dbo.DonVi dv ON dv.IdDonVi = t.IdDonVi
        ORDER BY t.TenThuoc");

            SetThuocItemsSource(dtThuoc);   
        }

        private DataView _dvThuoc;           // thêm biến giữ DataView
        private ICollectionView _viewThuoc;  // giữ để phòng khi sau này dùng List<T>

        private void SetThuocItemsSource(object source)
        {
            if (source is DataTable dt)
            {
                _dvThuoc = dt.DefaultView;           // ưu tiên DataView
                lsvThuoc.ItemsSource = _dvThuoc;

                _viewThuoc = null;                   // không dùng Filter delegate trong chế độ DataView
            }
            else
            {
                var src = source as IEnumerable
                          ?? throw new ArgumentException("ItemsSource phải là IEnumerable hoặc DataView");
                lsvThuoc.ItemsSource = src;

                _dvThuoc = null;
                _viewThuoc = CollectionViewSource.GetDefaultView(lsvThuoc.ItemsSource);
                _viewThuoc.Filter = FilterThuocByName;   // chỉ gọi khi KHÔNG phải DataView
                _viewThuoc.Refresh();
            }
        }

        private void txtSearchThuoc_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = txtSearchThuoc.Text ?? "";

            try
            {
                if (_dvThuoc != null && lsvThuoc.ItemsSource is DataView) // DataView path
                {
                    // escape ký tự đặc biệt cho RowFilter
                    var esc = text.Replace("'", "''")
                                  .Replace("[", "[[]")
                                  .Replace("%", "[%]")
                                  .Replace("*", "[*]")
                                  .Replace("_", "[_]")
                                  .Replace("]", "[]]");

                    _dvThuoc.RowFilter = string.IsNullOrWhiteSpace(esc)
                        ? ""
                        : $"Convert(TenThuoc, 'System.String') LIKE '%{esc}%'";
                }
                else if (_viewThuoc != null) // List<T> path
                {
                    _viewThuoc.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", 
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Chỉ dùng khi ItemsSource là List<T> / ObservableCollection<T>
        private bool FilterThuocByName(object obj)
        {
            var key = NormalizeText(txtSearchThuoc.Text ?? "");
            if (string.IsNullOrWhiteSpace(key)) return true;

            string ten = "";
            if (obj is System.Data.DataRowView drv)
                ten = drv.Row.Table.Columns.Contains("TenThuoc") ? drv["TenThuoc"]?.ToString() ?? "" : "";
            else
                ten = obj?.GetType().GetProperty("TenThuoc")?.GetValue(obj)?.ToString() ?? "";

            return NormalizeText(ten).Contains(key);
        }

        private static string NormalizeText(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var d = s.ToLowerInvariant().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(d.Length);
            foreach (var ch in d)
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            return sb.ToString().Normalize(NormalizationForm.FormC);
        }


    }
}
