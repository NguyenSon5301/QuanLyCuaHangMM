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
    public partial class NhaCungCap : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public NhaCungCap()
        {
            InitializeComponent();
            NhaCungCap_Load();
        }
        int sodong;
        string mancc, tenncc, diachincc, sdtncc;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "nhacungcap"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["nhacungcap"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã nhà cung cấp"].ReadOnly = true;
        }
        private void NhaCungCap_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaNCC as 'Mã nhà cung cấp', TenNCC as 'Tên nhà cung cấp', DiaChiNCC as 'Địa chỉ', SĐTNCC as 'Số điện thoại' from nhacungcap", conn);
            DoDuLieu(sql);
        }
        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của nhà cung cấp đơn?\nMã nhà cung cấp " + mancc + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from nhacungcap where mancc = '" + mancc + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    NhaCungCap_Load();
                }
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            if (bangphu.HasChanges())
            {
                if (tenncc == "")
                {
                    MessageBox.Show("Vui lòng nhập tên nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                }
                else if (diachincc == "")
                {
                    MessageBox.Show("Vui lòng nhập địa chỉ nhà cung cấp", "Thông báo", MessageBoxButtons.OK);
                }
                else if (sdtncc == "")
                {
                    MessageBox.Show("Vui lòng nhập số điện thoại", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (mancc == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert nhacungcap(TenNCC,DiaChiNCC,SĐTNCC) values(@TenNCC,@DiaChiNCC,@SĐTNCC)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@TenNCC", tenncc);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@DiaChiNCC", diachincc);
                            cmd1.Parameters.AddWithValue("@SĐTNCC", sdtncc);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                NhaCungCap_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update nhacungcap set TenNCC = @TenNCC,DiaChiNCC = @DiaChiNCC,SĐTNCC = @SĐTNCC  where MaNCC = @MaNCC", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaNCC", mancc);
                            cmd1.Parameters.AddWithValue("@TenNCC", tenncc);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@DiaChiNCC", diachincc);
                            cmd1.Parameters.AddWithValue("@SĐTNCC", sdtncc);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                NhaCungCap_Load();
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
                mancc = data.Rows[sodong].Cells["Mã nhà cung cấp"].Value.ToString();
                tenncc = data.Rows[sodong].Cells["Tên nhà cung cấp"].Value.ToString();
                diachincc = data.Rows[sodong].Cells["Địa chỉ"].Value.ToString();
                sdtncc = data.Rows[sodong].Cells["Số điện thoại"].Value.ToString();
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaNCC as 'Mã nhà cung cấp', TenNCC as 'Tên nhà cung cấp', DiaChiNCC as 'Địa chỉ', SĐTNCC as 'Số điện thoại' from nhacungcap where MaNCC like '%" + txt_timkiem.Text + "%' or TenNCC like '%" + txt_timkiem.Text + "%' or DiaChiNCC like N'%" + txt_timkiem.Text + "%' or SĐTNCC like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn tải lại dữ liệu?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                NhaCungCap_Load();
            }
        }
    }
}
