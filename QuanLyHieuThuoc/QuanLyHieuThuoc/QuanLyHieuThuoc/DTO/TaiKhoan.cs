using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace QuanLyHieuThuoc.DTO
{
    public class TaiKhoan
    {
        public TaiKhoan(string TenDangNhap,string MatKhau, string TenHienThi, string SoDienThoai, int CuaHang)
        {
            this.TenDangNhap = TenDangNhap;
            this.MatKhau = MatKhau;
            this.TenHienThi = TenHienThi;
            this.SoDienThoai = SoDienThoai;
            this.CuaHang = CuaHang;
        }

        public TaiKhoan (DataRow row)
        {
            this.TenDangNhap = (string)row["TenDangNhap"];
            this.MatKhau = (string)row["MatKhau"];
            this.tenHienThi = (string)row["TenHienThi"];
            this.SoDienThoai = (string)row["SoDienThoai"];
            this.CuaHang = (int)row["CuaHang"];
        }
        private string tenDangNhap;
        private string matKhau;
        private string tenHienThi;
        private string soDienThoai;
        private int cuaHang;

        public string TenDangNhap { get => tenDangNhap; set => tenDangNhap = value; }
        public string MatKhau { get => matKhau; set => matKhau = value; }
        public string TenHienThi { get => tenHienThi; set => tenHienThi = value; }
        public string SoDienThoai { get => soDienThoai; set => soDienThoai = value; }
        public int CuaHang { get => cuaHang; set => cuaHang = value; }
    }
}
