using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QuanLyCuaHangMM
{
    public partial class ThongTinNhanVien : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public ThongTinNhanVien()
        {
            InitializeComponent();
        }
        private string manv,gioitinh;
        public ThongTinNhanVien(string MaNV)
        {
            InitializeComponent();
            manv = MaNV;
        }

        private void SuaTT_Click(object sender, EventArgs e)
        {
            if (HT.Enabled == false) // Kiểm tra xem đang sửa hay chưa sửa, nếu chưa sửa thì mở các controls lên để nhập dữ liệu và xác nhận
            {
                HT.Enabled = true;
                SDT.Enabled = true;
                DC.Enabled = true;
                CV.Enabled = true;
                NVL.Enabled = true;
                XacNhan.Enabled = true;
                SuaTT.Text = "Dừng sửa"; // Thay đổi Text nút
            }
            else
            {
                if (MessageBox.Show("Bạn có muốn dừng cập nhật thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận dừng cập nhật
                {
                    conn.Open(); // Mở kết nối
                    // Câu lệnh thực hiện truy vấn
                    string sql = @"SELECT * FROM nhanvien WHERE manv = '" + manv + "'";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    // Đọc kết quả truy vấn và in ra
                    while (dr.Read())
                    {
                        string ht = (string)dr["TenNV"].ToString();
                        HT.Text = ht;
                        string sdt = (string)dr["SĐTNV"].ToString();
                        SDT.Text = sdt;
                        string dc = (string)dr["DiaChiNV"].ToString();
                        DC.Text = dc;
                        string cv = (string)dr["ChucVu"].ToString();
                        CV.Text = cv;
                        DateTime sdate = (DateTime)dr["NgayVaoLam"];
                        string nvl = sdate.ToString("dd-MM-yyyy");
                        NVL.Text = nvl;
                    }
                    conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
                    // Đóng các controls không cho nhập dữ liệu và xác nhận
                    HT.Enabled = false;
                    SDT.Enabled = false;
                    DC.Enabled = false;
                    CV.Enabled = false;
                    NVL.Enabled = false;
                    XacNhan.Enabled = false;
                    SuaTT.Text = "Sửa thông tin"; // Thay đổi Text nút
                }
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            // Tạo các ràng buộc không để trống của các ô nhập
            if (HT.Text == "")
            {
                MessageBox.Show("Vui lòng nhập họ tên!!!", "Thông báo!!!");
                HT.Focus();
            }
            else if (SDT.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!!!", "Thông báo!!!");
                SDT.Focus();
            }
            else if (SDT.Text.Length != 10) // Nhập đúng độ dài số điện thoại bằng 10
            {
                MessageBox.Show("Vui lòng nhập đúng số điện thoại!!!");
                SDT.Focus();
            }
            else if (DC.Text == "")
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!!!", "Thông báo!!!");
                DC.Focus();
            }
            else if (NVL.Text == "")
            {
                MessageBox.Show("Vui lòng nhập ngày vào làm", "Thông báo!!!");
                NVL.Focus();
            }
            else if (CV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập chức vụ!!!", "Thông báo!!!");
                CV.Focus();
            }
            else if (MessageBox.Show("Bạn có muốn cập nhật thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand cmd1 = new SqlCommand("Update nhanvien set TenNV = @TenNV,GioiTinh = @GioiTinh,ChucVu = @ChucVu,NgayVaoLam = @NgayVaoLam,DiaChiNV = @DiaChiNV,SĐTNV = @SĐTNV where MaNV = @MaNV", conn); // Tạo đối tượng
                cmd1.Parameters.AddWithValue("@MaNV", manv);
                cmd1.Parameters.AddWithValue("@TenNV", HT.Text);// Thiết lập tham số
                cmd1.Parameters.AddWithValue("@GioiTinh", gioitinh);
                cmd1.Parameters.AddWithValue("@ChucVu", CV.Text);
                cmd1.Parameters.AddWithValue("@NgayVaoLam", NVL.Text);
                cmd1.Parameters.AddWithValue("@DiaChiNV", DC.Text);
                cmd1.Parameters.AddWithValue("@SĐTNV", SDT.Text);
                cmd1.ExecuteNonQuery();
                cmd1.Parameters.Clear();
                conn.Close();
                // Đóng các controls không cho nhập dữ liệu và xác nhận
                HT.Enabled = false;
                SDT.Enabled = false;
                DC.Enabled = false;
                CV.Enabled = false;
                NVL.Enabled = false;
                XacNhan.Enabled = false;
                SuaTT.Text = "Sửa thông tin"; // Thay đổi Text nút
            }
        }

        private void ThongTinNhanVien_Load(object sender, EventArgs e)
        {
            MaNV.Text = manv;
            conn.Open(); // Mở kết nối
            // Câu lệnh thực hiện truy vấn
            string sql = @"SELECT * FROM nhanvien WHERE manv = '" + manv + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            // Đọc kết quả truy vấn và in ra
            while (dr.Read())
            {
                string ht = (string)dr["TenNV"].ToString();
                HT.Text = ht;
                string sdt = (string)dr["SĐTNV"].ToString();
                SDT.Text = sdt;
                string dc = (string)dr["DiaChiNV"].ToString();
                DC.Text = dc;
                string cv = (string)dr["ChucVu"].ToString();
                CV.Text = cv;
                gioitinh = (string)dr["GioiTinh"].ToString();
                DateTime sdate = (DateTime)dr["NgayVaoLam"];
                string nvl = sdate.ToString("dd-MM-yyyy");
                NVL.Text = nvl;
            }
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
        }
        // Tạo sự kiện ở ô nhập Số điện thoại chỉ cho nhập số và xóa
        private void SDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
