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
    public partial class HangHoa : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public HangHoa()
        {
            InitializeComponent();
            HangHoa_Load();
        }
        int sodong;
        string tenhh, malh, dongiaban, mahh,soluong;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // 
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "hanghoa"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["hanghoa"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Tên hàng hóa"].ReadOnly = true;
        }
        private void HangHoa_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select TenHH as 'Tên hàng hóa', MaLH as 'Mã loại hàng', DonGiaBan as 'Đơn giá bán', MaHH as 'Mã hàng hóa', SoLuong as 'Số lượng' from hanghoa", conn);
            DoDuLieu(sql);
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

        private void XacNhan_Click(object sender, EventArgs e)
        {
            string kiemtratontaimalh = "0";
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            string sql = "select Count(*) as 'Mã loại hàng' from loaihang where MaLH = '" + malh + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            // Đọc kết quả truy vấn và in ra
            while (dr.Read())
            {
                kiemtratontaimalh = (string)dr["Mã loại hàng"].ToString();
            }
            conn.Close();
            if (bangphu.HasChanges())
            {
                if (tenhh == "")
                {
                    MessageBox.Show("Vui lòng nhập têm hàng hóa", "Thông báo", MessageBoxButtons.OK);
                }
                else if (malh == "")
                {
                    MessageBox.Show("Vui lòng nhập loại hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimalh) == 0)
                {
                    MessageBox.Show("Mã loại hàng không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (dongiaban == "")
                {
                    MessageBox.Show("Vui lòng nhập số đơn giá bán", "Thông báo", MessageBoxButtons.OK);
                }
                else if (soluong == "")
                {
                    MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (mahh == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert hanghoa(TenHH,MaLH,DonGiaBan,SoLuong) Values(@TenHH,@MaLH,@DonGiaBan,@SoLuong)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@TenHH", tenhh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaLH", malh);
                            cmd1.Parameters.AddWithValue("@DonGiaBan", dongiaban);
                            cmd1.Parameters.AddWithValue("@SoLuong", soluong);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HangHoa_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update hanghoa set TenHH = TenHH,MaLH = MaLH,DonGiaBan = DonGiaBan,SoLuong = SoLuong where MaHH = @MaHH", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaHH", mahh);
                            cmd1.Parameters.AddWithValue("@TenHH", tenhh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaLH", malh);
                            cmd1.Parameters.AddWithValue("@DonGiaBan", dongiaban);
                            cmd1.Parameters.AddWithValue("@SoLuong", soluong);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                HangHoa_Load();
                            }
                        }
                    }
                }
            }
        }

        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin của hàng hóa?\nTên hàng hóa " + tenhh + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from hanghoa where mahh = '" + mahh + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    HangHoa_Load();
                }
            }
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sodong = e.RowIndex;
            if (sodong >= 0)
            {
                tenhh = data.Rows[sodong].Cells["Tên hàng hóa"].Value.ToString();
                malh = data.Rows[sodong].Cells["Mã loại hàng"].Value.ToString();
                dongiaban = data.Rows[sodong].Cells["Đơn giá bán"].Value.ToString();
                mahh = data.Rows[sodong].Cells["Mã hàng hóa"].Value.ToString();
                soluong = data.Rows[sodong].Cells["Số lượng"].Value.ToString();
            }
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                HangHoa_Load();
            }   
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select TenHH as 'Tên hàng hóa', MaLH as 'Mã loại hàng', DonGiaBan as 'Đơn giá bán', MaHH as 'Mã hàng hóa', SoLuong as 'Số lượng' from hanghoa where TenHH like '%" + txt_timkiem.Text + "%' or MaLH like '%" + txt_timkiem.Text + "%' or DonGiaBan like N'%" + txt_timkiem.Text + "%' or MaHH like N'%" + txt_timkiem.Text + "%' or SoLuong like N'" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }
    }
}
