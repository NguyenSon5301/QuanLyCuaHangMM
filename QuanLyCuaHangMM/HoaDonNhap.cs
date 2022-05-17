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
    public partial class HoaDonNhap : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public HoaDonNhap()
        {
            InitializeComponent();
            HoaDonNhap_Load();
        }
        int sodong;
        string mapnh, manv, mancc, ngaynhap;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn

        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "hoadonnhap"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["hoadonnhap"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã hóa đơn nhập"].ReadOnly = true;
            data.Columns["Ngày nhập"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void HoaDonNhap_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaPNH as 'Mã hóa đơn nhập', MaNV as 'Mã nhân viên', MaNCC as 'Mã nhà cung cấp', NgayNhap as 'Ngày nhập' from hoadonnhap", conn);
            DoDuLieu(sql);
        }
        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của hóa đơn?\nMã hóa đơn " + mapnh + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from hoadonnhap where mapnh = '" + mapnh + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    HoaDonNhap_Load();
                }
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            string kiemtratontaimanv = "0", kiemtratontaimncc = "0";
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            string sql = "Select Count(*) as 'Mã nhân viên',(Select Count(*) from nhacungcap where mancc = '" + mancc + "') as 'Mã nhà cung cấp' from nhanvien where MaNV = '" + manv + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            // Đọc kết quả truy vấn và in ra
            while (dr.Read())
            {
                kiemtratontaimanv = (string)dr["Mã nhân viên"].ToString();
                kiemtratontaimncc = (string)dr["Mã nhà cung cấp"].ToString();
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
                else if (mancc == "")
                {
                    MessageBox.Show("Vui lòng nhập mã nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimncc) == 0)
                {
                    MessageBox.Show("Mã nhà cung cấp không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (ngaynhap == "")
                {
                    MessageBox.Show("Vui lòng nhập ngày hoạt động", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (mapnh == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert hoadonnhap(MaNV,MaNCC,NgayNhap) values(@MaNV,@MaNCC,@NgayNhap)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaNV", manv);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaNCC", mancc);
                            cmd1.Parameters.AddWithValue("@NgayNhap", ngaynhap);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HoaDonNhap_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update hoadonnhap set MaNV = @MaNV,MaNCC = @MaNCC,NgayNhap = @NgayNhap  where MaPNH = @MaPNH", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaPNH", mapnh);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaNCC", mancc);
                            cmd1.Parameters.AddWithValue("@NgayNhap", ngaynhap);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HoaDonNhap_Load();
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
                HoaDonNhap_Load();
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
                    HoaDonNhap_Load();
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
                mapnh = data.Rows[sodong].Cells["Mã hóa đơn nhập"].Value.ToString();
                manv = data.Rows[sodong].Cells["Mã nhân viên"].Value.ToString();
                mancc = data.Rows[sodong].Cells["Mã nhà cung cấp"].Value.ToString();
                ngaynhap = data.Rows[sodong].Cells["Ngày nhập"].Value.ToString();
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaPNH as 'Mã hóa đơn nhập', MaNV as 'Mã nhân viên', MaNCC as 'Mã nhà cung cấp', NgayNhap as 'Ngày nhập' from hoadonnhap where MaPNH like '%" + txt_timkiem.Text + "%' or MaNV like '%" + txt_timkiem.Text + "%' or MaNCC like N'%" + txt_timkiem.Text + "%' or NgayNhap like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn tải lại dữ liệu?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                HoaDonNhap_Load();
            }
        }

        private void data_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.ColumnIndex == data.Columns["Ngày nhập"].Index)
            {
                e.Cancel = true;
                MessageBox.Show("Bạn đã nhập sai kiểu dữ liệu ngày\nYêu cầu nhập kiểu: mm/DD/yyyy", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            e.Cancel = false;
        }
    }
}
