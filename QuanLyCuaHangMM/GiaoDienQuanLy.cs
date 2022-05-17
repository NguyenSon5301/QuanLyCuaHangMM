﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCuaHangMM
{
    public partial class GiaoDienQuanLy : Form
    {
        public GiaoDienQuanLy()
        {
            InitializeComponent();
        }
        private void MoFormCon(object _form) // Thực hiện hành động mở Form Con
        {

            if (panel_ChuaFormCon.Controls.Count > 0) panel_ChuaFormCon.Controls.Clear();

            Form fm = _form as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None; // Chỉnh các thông số Form Con, để phù hợp
            fm.Dock = DockStyle.Fill;
            panel_ChuaFormCon.Controls.Add(fm);
            panel_ChuaFormCon.Tag = fm;
            fm.Show();
        }
        private void TrangChu_Click(object sender, EventArgs e) //Các chức năng Click của từng button để mở các Form Con
        {
            label_val.Text = "Trang chủ";
            guna2PictureBox_val.Image = Properties.Resources.trangchu;
            MoFormCon(new TrangChu());
        }

        private void BangNhanVien_Click(object sender, EventArgs e)
        {
            label_val.Text = "Bảng nhân viên";
            guna2PictureBox_val.Image = Properties.Resources.bangnhanvien;
            MoFormCon(new BangNhanVien());
        }

        private void BangKhachHang_Click(object sender, EventArgs e)
        {
            label_val.Text = "Bảng khách hàng";
            guna2PictureBox_val.Image = Properties.Resources.bangkhachhang;
            MoFormCon(new BangKhachHang());
        }

        private void BangHoaDonBan_Click(object sender, EventArgs e)
        {
            label_val.Text = "Bảng hóa đơn bán";
            guna2PictureBox_val.Image = Properties.Resources.hoadonban;
            MoFormCon(new HoaDonBan());
        }

        private void BangHoaDonNhap_Click(object sender, EventArgs e)
        {
            label_val.Text = "Bảng hóa đơn nhập";
            guna2PictureBox_val.Image = Properties.Resources.hoadonnhap;
            MoFormCon(new HoaDonNhap());
        }

        private void BangHangHoa_Click(object sender, EventArgs e)
        {
            label_val.Text = "Bảng hàng hóa";
            guna2PictureBox_val.Image = Properties.Resources.banghanghoa;
            MoFormCon(new HangHoa());
        }

        private void BangNhaCungCap_Click(object sender, EventArgs e)
        {
            label_val.Text = "Các nhà cùng cấp";
            guna2PictureBox_val.Image = Properties.Resources.nhacungcap;
            MoFormCon(new NhaCungCap());
        }

        private void ChiTiet_HoaDonBan_Click(object sender, EventArgs e)
        {
            label_val.Text = "Chi tiết hóa đơn bán";
            guna2PictureBox_val.Image = Properties.Resources.ChiTiet_HoaDonBan;
            MoFormCon(new ChiTiet_HDBan());
        }

        private void ChiTiet_HoaDonNhap_Click(object sender, EventArgs e)
        {
            label_val.Text = "Chi tiết hóa đơn nhập";
            guna2PictureBox_val.Image = Properties.Resources.hoadonnhap;
            MoFormCon(new ChiTiet_HDNhap());
        }

        private void TatUngDung_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void GiaoDienQuanLy_Load(object sender, EventArgs e)
        {
            label_val.Text = "Trang chủ";
            guna2PictureBox_val.Image = Properties.Resources.trangchu;
            MoFormCon(new TrangChu());
        }

    }
}
