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
    public partial class ChiTiet_HDNhap : Form
    {
        SqlConnection conn = new SqlConnection(TaoKetNoi.connectionString);
        public ChiTiet_HDNhap()
        {
            InitializeComponent();
            BangChiTiet_HoaDonNhap_Load();
        }
        int sodong;
        string mactpnh,mapnh, mahh, manv,soluongnhap, thanhtiennhap, tongtiennhap, vat, tongthanhtoannhap;
        SqlCommand sql;  // Tạo đối tượng
        DataSet bangphu = null; // Tạo một bảng giả
        SqlDataAdapter chuyenkieu = null; // Chuyển kiểu của Đối tượng truy vấn
        private void DoDuLieu(SqlCommand caulenh)
        {
            bangphu = new DataSet();
            chuyenkieu = new SqlDataAdapter(caulenh);
            sql.CommandType = CommandType.Text;
            SqlCommandBuilder xaydungkieu = new SqlCommandBuilder(chuyenkieu);
            chuyenkieu.Fill(bangphu, "chitiet_hoadonnhap"); // Lấp đầy dữ liệu cho bảng giả bằng các dữ liệu đã được chuyển kiểu
            conn.Close(); // Không dùng đến kết nối thì đóng lại (giải phóng)
            data.DataSource = bangphu.Tables["chitiet_hoadonnhap"]; // In dữ liệu lên bằng DataGridView
            data.Columns["Mã chi tiết nhập"].ReadOnly = true;
        }
        private void BangChiTiet_HoaDonNhap_Load()
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaCTPNH as 'Mã chi tiết nhập',MaPNH as 'Mã hóa đơn nhập',MaHH as 'Mã hàng hóa',MaNV as 'Mã nhân viên',SoLuongNhap as 'Số lượng nhập',ThanhTienNhap as 'Thành tiền nhập',TongTienNhap as 'Tổng tiền nhập',VAT as 'VAT',TongThanhToanNhap as 'Tổng thanh toán nhập' from chitiet_hoadonnhap", conn);
            DoDuLieu(sql);
        }
        private void NutXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa thông tin chi tiết của hóa đơn?\nMã hóa đơn nhập " + mactpnh + "?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
            {
                conn.Open();
                SqlCommand command;
                command = conn.CreateCommand();
                command.CommandText = "Delete from chitiet_hoadonnhap where mactpnh = '" + mactpnh + "'";
                command.ExecuteNonQuery();
                conn.Close();
                if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                {
                    BangChiTiet_HoaDonNhap_Load();
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

        private void XacNhan_Click(object sender, EventArgs e)
        {
            string kiemtratontaimahd = "0", kiemtratontaimhh = "0";
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            string sql = "Select Count(*) as 'Mã hóa đơn',(Select Count(*) from hanghoa where MaHH = '" + mahh + "') as 'Mã hàng hóa' from hoadonnhap where MaPNH = '" + mapnh + "'";
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
                if (mapnh == "")
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
                else if (soluongnhap == "")
                {
                    MessageBox.Show("Vui lòng nhập số lượng nhập", "Thông báo", MessageBoxButtons.OK);
                }
                else if (thanhtiennhap == "")
                {
                    MessageBox.Show("Vui lòng nhập thành tiền", "Thông báo", MessageBoxButtons.OK);
                }
                else if (tongtiennhap == "")
                {
                    MessageBox.Show("Vui lòng nhập tổng tiền hàng", "Thông báo", MessageBoxButtons.OK);
                }
                else if (vat == "")
                {
                    MessageBox.Show("Vui lòng nhập chiết khấu", "Thông báo", MessageBoxButtons.OK);
                }
                else if (tongthanhtoannhap == "")
                {
                    MessageBox.Show("Vui lòng nhập tổng thanh toán", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    if (MessageBox.Show("Bạn có muốn xác nhận thông tin?", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                    {
                        if (mactpnh == "")
                        {
                            conn.Open(); // Mở kết nối 
                            // Câu lệnh truy vấn
                            SqlCommand cmd1 = new SqlCommand("Insert chitiet_hoadonnhap(MaPNH,MaHH,MaNV,SoLuongNhap,ThanhTienNhap,TongTienNhap,VAT,TongThanhToanNhap) values(@MaPNH,@MaHH,@MaNV,@SoLuongNhap,@ThanhTienNhap,@TongTienNhap,@VAT,@TongThanhToanNhap)", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaPNH", mapnh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaHH", mahh);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);
                            cmd1.Parameters.AddWithValue("@SoLuongNhap", soluongnhap);
                            cmd1.Parameters.AddWithValue("@ThanhTienNhap", thanhtiennhap);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@TongTienNhap", tongtiennhap);
                            cmd1.Parameters.AddWithValue("@VAT", vat);
                            cmd1.Parameters.AddWithValue("@TongThanhToanNhap", tongthanhtoannhap);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                BangChiTiet_HoaDonNhap_Load();
                            }
                        }
                        else
                        {
                            conn.Open();
                            SqlCommand cmd1 = new SqlCommand("Update chitiet_hoadonnhap set MaPNH = @MaPNH,MaHH = @MaHH,MaNV = @MaNV,SoLuongNhap = @SoLuongNhap, ThanhTienNhap = @ThanhTienNhap,TongTienNhap = @TongTienNhap,VAT = @VAT ,TongThanhToanNhap = @TongThanhToanNhap  where MaCTPNH = @MaCTPNH", conn); // Tạo đối tượng
                            cmd1.Parameters.AddWithValue("@MaCTPNH", mactpnh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaPNH", mapnh);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@MaHH", mahh);
                            cmd1.Parameters.AddWithValue("@MaNV", manv);
                            cmd1.Parameters.AddWithValue("@SoLuongNhap", soluongnhap);
                            cmd1.Parameters.AddWithValue("@ThanhTienNhap", thanhtiennhap);// Thiết lập tham số
                            cmd1.Parameters.AddWithValue("@TongTienNhap", tongtiennhap);
                            cmd1.Parameters.AddWithValue("@VAT", vat);
                            cmd1.Parameters.AddWithValue("@TongThanhToanNhap", tongthanhtoannhap);
                            cmd1.ExecuteNonQuery(); // Thi hành truy vân
                            cmd1.Parameters.Clear();
                            conn.Close();
                            if (MessageBox.Show("Bạn có muốn tải lại trang", "Thông Báo!!!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) // Tạo thông báo xác nhận cập nhật
                            {
                                BangChiTiet_HoaDonNhap_Load();
                            }
                        }
                    }
                }
            }
        }
        private void txt_timkiem_TextChanged(object sender, EventArgs e)
        {
            conn.Open(); // Mở kết nối 
            // Câu lệnh thực hiện truy vấn
            sql = new SqlCommand("Select MaCTPNH as 'Mã chi tiết nhập',MaPNH as 'Mã hóa đơn nhập',MaHH as 'Mã hàng hóa',MaNV as 'Mã nhân viên',SoLuongNhap as 'Số lượng nhập',ThanhTienNhap as 'Thành tiền nhập',TongTienNhap as 'Tổng tiền nhập',VAT as 'VAT',TongThanhToanNhap as 'Tổng thanh toán nhập' from chitiet_hoadonnhap where MaCTPNH like '%" + txt_timkiem.Text + "%' or MaPNH like '%" + txt_timkiem.Text + "%' or MaHH like N'%" + txt_timkiem.Text + "%' or MaNV like N'%" + txt_timkiem.Text + "%' or SoLuongNhap like N'%" + txt_timkiem.Text + "%' or ThanhTienNhap like N'%" + txt_timkiem.Text + "%' or TongTienNhap like N'%" + txt_timkiem.Text + "%' or VAT like N'%" + txt_timkiem.Text + "%' or TongThanhToanNhap like N'%" + txt_timkiem.Text + "%'", conn);
            DoDuLieu(sql);
        }

        private void data_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            sodong = e.RowIndex;
            if (sodong >= 0)
            {
                mactpnh = data.Rows[sodong].Cells["Mã chi tiết nhập"].Value.ToString();
                mapnh = data.Rows[sodong].Cells["Mã hóa đơn nhập"].Value.ToString();
                mahh = data.Rows[sodong].Cells["Mã hàng hóa"].Value.ToString();
                manv = data.Rows[sodong].Cells["Mã nhân viên"].Value.ToString();
                soluongnhap = data.Rows[sodong].Cells["Số lượng nhập"].Value.ToString();
                thanhtiennhap = data.Rows[sodong].Cells["Thành tiền nhập"].Value.ToString();
                tongtiennhap = data.Rows[sodong].Cells["Tổng tiền nhập"].Value.ToString();
                vat = data.Rows[sodong].Cells["VAT"].Value.ToString();
                tongthanhtoannhap = data.Rows[sodong].Cells["Tổng thanh toán nhập"].Value.ToString();
            }
        }

        private void ReLoad_Click(object sender, EventArgs e)
        {
            BangChiTiet_HoaDonNhap_Load();
        }
    }
}
