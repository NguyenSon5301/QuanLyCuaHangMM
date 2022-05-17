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
    public partial class ChiTiet_HDBan : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public ChiTiet_HDBan()
        {
            InitializeComponent();
            BangChiTiet_HoaDonBan_Load();
        }
        int sodong;
        string macthdb,mahd, mahh, manv, thanhtien, tongtienhang, chietkhau, tongthanhtoan;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "chitiet_hoadonban"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["chitiet_hoadonban"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã chi tiết"].ReadOnly = true;
        }
        private void BangChiTiet_HoaDonBan_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaCTHDB as 'Mã chi tiết', MaHD as 'Mã hóa đơn', MaHH as 'Mã hàng hóa', MaNV as 'Mã nhân viên', ThanhTien as 'Thành tiền', TongTienHang as 'Tổng tiền hàng', ChietKhau as 'Chiết khấu', TongThanhToan as 'Tổng thanh toán' from chitiet_hoadonban", conn);
            DoDuLieu(sql);
        }
        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin chi tiết của hóa đơn?\nMã hóa đơn " + macthdb + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from chitiet_hoadonban where macthdb = '" + macthdb + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    BangChiTiet_HoaDonBan_Load();
                }
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            string kiemtratontaimahd = "0",kiemtratontaimhh = "0";
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            string sql = "Select Count(*) as 'Mã hóa đơn',(Select Count(*) from hanghoa where MaHH = '" + mahh + "') as 'Mã hàng hóa' from hoadonban where MaHD = '" + mahd + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            // Đọc kết quả truy vấn và in ra
            while (dr.Read())
            {
                kiemtratontaimahd = (string)dr["Mã hóa đơn"].ToString();
                kiemtratontaimhh = (string)dr["Mã hàng hóa"].ToString();
            }
            conn.Close();
            if (bangphu.HasChanges())
            {
                if (mahd == "")
                {
                    MessageBox.Show("Vui lòng nhập mã hóa đơn", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimahd) == 0)
                {
                    MessageBox.Show("Mã hóa đơn không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (mahh == "")
                {
                    MessageBox.Show("Vui lòng nhập hàng hóa", "Thông báo", MessageBoxButtons.OK);
                }
                else if (Int32.Parse(kiemtratontaimhh) == 0)
                {
                    MessageBox.Show("Mã hàng hóa không tồn tại", "Thông báo", MessageBoxButtons.OK);
                }
                else if (manv == "")
                {
                    MessageBox.Show("Vui lòng nhập mã nhân viên", "Thông báo", MessageBoxButtons.OK);
                }
                else if (thanhtien == "")
                {
                    MessageBox.Show("Vui lòng nhập thành tiền", "Thông báo", MessageBoxButtons.OK);
                } 
                else if (tongtienhang == "")
                {
                    MessageBox.Show("Vui lòng nhập tổng tiền hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else if (chietkhau == "")
                {
                    MessageBox.Show("Vui lòng nhập chiết khấu", "Thông báo", MessageBoxButtons.OK);
                }
                else if (tongthanhtoan == "")
                {
                    MessageBox.Show("Vui lòng nhập tổng thanh toán", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (macthdb == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert chitiet_hoadonban(MaHD,MaHH,MaNV,ThanhTien,TongTienHang,ChietKhau,TongThanhToan) values(@MaHD,@MaHH,@MaNV,@ThanhTien,@TongTienHang,@ChietKhau,@TongThanhToan)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaHD", mahd);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaHH", mahh);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);
                            cmd1.Parameters.AddWithValue("@ThanhTien", thanhtien);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@TongTienHang", tongtienhang);
                            cmd1.Parameters.AddWithValue("@ChietKhau", chietkhau);
                            cmd1.Parameters.AddWithValue("@TongThanhToan", tongthanhtoan);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                BangChiTiet_HoaDonBan_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update chitiet_hoadonban set MaHD = @MaHD,MaHH = @MaHH,MaNV = @MaNV,ThanhTien = @ThanhTien,TongTienHang = @TongTienHang,ChietKhau = @ChietKhau ,TongThanhToan = @TongThanhToan  where MaCTHDB = @MaCTHDB", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaCTHDB", macthdb);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaHD", mahd);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaHH", mahh);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);
                            cmd1.Parameters.AddWithValue("@ThanhTien", thanhtien);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@TongTienHang", tongtienhang);
                            cmd1.Parameters.AddWithValue("@ChietKhau", chietkhau);
                            cmd1.Parameters.AddWithValue("@TongThanhToan", tongthanhtoan);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                BangChiTiet_HoaDonBan_Load();
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
                macthdb = data.Rows[sodong].Cells["Mã chi tiết"].Value.ToString();
                mahd = data.Rows[sodong].Cells["Mã hóa đơn"].Value.ToString();
                mahh = data.Rows[sodong].Cells["Mã hàng hóa"].Value.ToString();
                manv = data.Rows[sodong].Cells["Mã nhân viên"].Value.ToString();
                thanhtien = data.Rows[sodong].Cells["Thành tiền"].Value.ToString();
                tongtienhang = data.Rows[sodong].Cells["Tổng tiền hàng"].Value.ToString();
                chietkhau = data.Rows[sodong].Cells["Chiết khấu"].Value.ToString();
                tongthanhtoan = data.Rows[sodong].Cells["Tổng thanh toán"].Value.ToString();
            }
        }

        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaCTHDB as 'Mã chi tiết', MaHD as 'Mã hóa đơn', MaHH as 'Mã hàng hóa', MaNV as 'Mã nhân viên', ThanhTien as 'Thành tiền', TongTienHang as 'Tổng tiền hàng', ChietKhau as 'Chiết khấu', TongThanhToan as 'Tổng thanh toán' from chitiet_hoadonban where MaCTHDB like '%" + txt_timkiem.Text + "%' or MaHD like '%" + txt_timkiem.Text + "%' or MaHH like N'%" + txt_timkiem.Text + "%' or MaNV like N'%" + txt_timkiem.Text + "%' or ThanhTien like N'%" + txt_timkiem.Text + "%' or TongTienHang like N'%" + txt_timkiem.Text + "%' or ChietKhau like N'%" + txt_timkiem.Text + "%' or TongThanhToan like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                BangChiTiet_HoaDonBan_Load();
            }
        }
    }
}
