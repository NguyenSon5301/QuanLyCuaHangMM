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
    public partial class HoaDonBan : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public HoaDonBan()
        {
            InitializeComponent();
            HoaDonBan_Load();
        }
        int sodong;
        string mahd, manv, makh, ngayhd;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "hoadonban"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["hoadonban"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã hóa đơn"].ReadOnly = true;
            data.Columns["Ngày hoạt động"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void HoaDonBan_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaHD as 'Mã hóa đơn', MaNV as 'Mã nhân viên', MaKH as 'Mã khách hàng', NgayHD as 'Ngày hoạt động' from hoadonban", conn);
            DoDuLieu(sql);
        }
        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của hóa đơn?\nMã hóa đơn " + mahd + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from hoadonban where mahd = '" + mahd + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    HoaDonBan_Load();
                }
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            string kiemtratontaimanv = "0", kiemtratontaimkh = "0";
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            string sql = "Select Count(*) as 'Mã nhân viên',(Select Count(*) from khachhang where MaKH = '" + makh + "') as 'Mã khách hàng' from nhanvien where MaNV = '" + manv + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            // Đọc kết quả truy vấn và in ra
            while (dr.Read())
            {
                kiemtratontaimanv = (string)dr["Mã nhân viên"].ToString();
                kiemtratontaimkh = (string)dr["Mã khách hàng"].ToString();
            }
            conn.Close();
            if (bangphu.HasChanges())
            {
                if (manv == "")
                {
                    MessageBox.Show("Vui lòng nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimanv) == 0)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (makh == "")
                {
                    MessageBox.Show("Vui lòng nhập mã khách hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimkh) == 0)
                {
                    MessageBox.Show("Mã khách hàng không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (ngayhd == "")
                {
                    MessageBox.Show("Vui lòng nhập ngày hoạt động", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (mahd == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert hoadonban(MaNV,MaKH,NgayHD) values(@MaNV,@MaKH,@NgayHD)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaNV", manv);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaKH", makh);
                            cmd1.Parameters.AddWithValue("@NgayHD", ngayhd);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HoaDonBan_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update hoadonban set MaNV = @MaNV,MaKH = @MaKH,NgayHD = @NgayHD  where MaHD = @MaHD", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaHD", mahd);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaKH", makh);
                            cmd1.Parameters.AddWithValue("@NgayHD", ngayhd);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HoaDonBan_Load();
                            }
                        }
                    }
                }
            }
        }

        private void ChinhSua_Click(object sender, EventArgs e)
        {
            if (ChinhSua.Text == "Chỉnh sửa")
            {
                data.ReadOnly = false;
                NutXoa.Enabled = true;
                XacNhan.Enabled = true;
                ChinhSua.Text = "Dừng chỉnh sửa";
            }
            else
            {
                if (MessageBox.Show("Bạn có muốn dừng việc chinh sửa thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    NutXoa.Enabled = false;
                    XacNhan.Enabled = false;
                    data.ReadOnly = true;
                    ChinhSua.Text = "Chỉnh sửa";
                }
            }
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sodong = e.RowIndex;
            if (sodong >= 0)
            {
                mahd = data.Rows[sodong].Cells["Mã hóa đơn"].Value.ToString();
                manv = data.Rows[sodong].Cells["Mã nhân viên"].Value.ToString();
                makh = data.Rows[sodong].Cells["Mã khách hàng"].Value.ToString();
                ngayhd = data.Rows[sodong].Cells["Ngày hoạt động"].Value.ToString();
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaHD as 'Mã hóa đơn', MaNV as 'Mã nhân viên', MaKH as 'Mã khách hàng', NgayHD as 'Ngày hoạt động' from hoadonban where MaHD like '%" + txt_timkiem.Text + "%' or MaNV like '%" + txt_timkiem.Text + "%' or MaKH like N'%" + txt_timkiem.Text + "%' or NgayHD like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn tải lại dữ liệu?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                HoaDonBan_Load();
            }
        }

        private void data_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == data.Columns["Ngày hoạt động"].Index)
            {
                e.Cancel = true;
                MessageBox.Show("Bạn đã nhập sai kiểu dữ liệu ngày\nYêu cầu nhập kiểu: mm/DD/yyyy", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            e.Cancel = false;
        }
    }
}
