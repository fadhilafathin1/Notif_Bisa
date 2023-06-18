using Npgsql;
using System.Data;
using System.Data.Common;
using System.Drawing.Printing;
using System.Text;
using System.Xml.Serialization;
using Timer = System.Threading.Timer;
using NpgsqlTypes;

namespace PemberitahuanKonfirmasi
{
    public partial class Form1 : Form
    {
        private Size formOriginalSize;
        private Rectangle recpnl1;
        private Rectangle recpnl2;
        private Rectangle recpnlnt;
        private Rectangle reclbl1;
        private Rectangle reclbl2;
        private Rectangle reclbl3;
        private Rectangle reclbl4;
        private Rectangle reclbl5;
        private Rectangle recpnlout;
        public int id_notifikasi_admin { get; set; }
        public Form1()
        {
            InitializeComponent();
            this.Resize += Form1_Resize;
            formOriginalSize = this.Size;
            recpnl1 = new Rectangle(panel1.Location, panel1.Size);
            recpnlnt = new Rectangle(PanelNotif.Location, PanelNotif.Size);
            recpnl2 = new Rectangle(panel2.Location, panel2.Size);
            reclbl1 = new Rectangle(label1.Location, label1.Size);
            reclbl2 = new Rectangle(label2.Location, label2.Size);
            reclbl3 = new Rectangle(label3.Location, label3.Size);
            reclbl4 = new Rectangle(label4.Location, label4.Size);
            reclbl5 = new Rectangle(label5.Location, label5.Size);
            recpnlout = new Rectangle(panelout.Location, panelout.Size);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            BacaData();
            //BacaNotif();
        }
        private void BacaData()
        {
            string connString = "Server=localhost;Port=5432;User Id=postgres;Password=fathinanisatuz01;Database=project";
            string sql = "SELECT a.username_akun, k.nama_kamar, AGE(jadwal_keluar_penginapan, jadwal_masuk_penginapan), total_harga " +
                "         FROM notifikasi_admin na " +
                "         JOIN akun a ON (na.id_akun=a.id_akun) " +
                "         JOIN reservasi_penginapan r ON (na.id_rsv_penginapan = r.id_rsv_penginapan)" +
                "         JOIN kamar_penginapan k ON (na.id_kamar=k.id_kamar) " +
                "         ORDER BY id_notifikasi_admin DESC LIMIT 5 ";

            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        NpgsqlDataReader dr = command.ExecuteReader();
                        int labelIndex = 0;
                        int buttonIndex = 0;
                        while (dr.Read() && labelIndex < 100 )
                        {
                            string rowdata = $"{dr.GetString(0)}\n{dr.GetString(1)}\n{dr.GetTimeSpan(2)}Malam\n{dr.GetInt64(3)}";
                            if (labelIndex >= panelLabels.Controls.Count && buttonIndex <= panelLabels.Controls.Count)
                            {
                                // Membuat label baru
                                Label newLabel = new Label();
                                // newLabel.AutoSize = true;
                                newLabel.Size = new Size(400, 200);
                                newLabel.Location = new Point(12, (labelIndex * 200)); // Atur posisi label sesuai dengan kebutuhan
                                newLabel.Font = new Font("Montserrat", 12, FontStyle.Regular);
                                panelLabels.Controls.Add(newLabel);

                                // Membuat Button Konfirmasi
                                Button newbutton = new Button();
                                newbutton.Size = new Size (160,38); 
                                newbutton.Font = new Font("Montserrat", 12, FontStyle.Regular);
                                newbutton.Text = "Konfirmasi";
                                newbutton.Location = new Point(3, 120);
                                newbutton.BackColor = Color.Aqua;
                                newbutton.Click += newbutton_Click;
                                newLabel.Controls.Add(newbutton);

                                // Membuat Button Batal
                                Button button = new Button();
                                button.Size = new Size (160,38);
                                button.Font = new Font("Montserrat", 12, FontStyle.Regular);
                                button.TextAlign = ContentAlignment.MiddleCenter;
                                button.Text = "Batal";
                                button.Location = new Point(240, 120);
                                button.BackColor = Color.Aqua;
                                button.Click += button_Click;
                                newLabel.Controls.Add(button);

                            }

                            // Mengambil nilai kolom dari dataReader
                            //sb.AppendLine(rowdata);

                            // Menetapkan nilai ke properti Text dari label yang sesuai
                            Label label = (Label)panelLabels.Controls[labelIndex];
                            label.Text = rowdata;

                            labelIndex++;
                        }

                        dr.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }


        private void newbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda Akan Mengkonfirmasi Pesanan Ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                NpgsqlConnection con = new NpgsqlConnection("Server=localhost; Port=5432; Database=project; User Id=postgres; Password=fathinanisatuz01");
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO notifikasi_user (id_akun, id_kamar, id_rsv_penginapan, total_harga) SELECT id_akun, id_kamar, id_rsv_penginapan, total_harga FROM notifikasi_admin";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                
            }
        }

        //private void BacaNotif()
        //{
        //    string connString = "Server=localhost;Port=5432;User Id=postgres;Password=fathinanisatuz01;Database=project";
        //    string sql = "SELECT n., k.nama_kamar, AGE(jadwal_keluar_penginapan, jadwal_masuk_penginapan), total_harga " +
        //        "         FROM kode_pembayaran k " +
        //        "         JOIN akun a ON (na.id_akun=a.id_akun) " +
        //        "         JOIN reservasi_penginapan r ON (na.id_rsv_penginapan = r.id_rsv_penginapan)" +
        //        "         JOIN kamar_penginapan k ON (na.id_kamar=k.id_kamar) " +
        //        "         ORDER BY id_notifikasi_admin DESC LIMIT 5 ";

        //    using (NpgsqlConnection conn = new NpgsqlConnection(connString))
        //    {
        //        using (NpgsqlCommand command = new NpgsqlCommand(sql, conn))
        //        {
        //            try
        //            {
        //                conn.Open();
        //                NpgsqlDataReader dr = command.ExecuteReader();
        //                int labelIndex = 0;
        //                int buttonIndex = 0;
        //                while (dr.Read() && labelIndex < 100)
        //                {
        //                    string rowdata = $"{dr.GetString(0)}\n{dr.GetString(1)}\n{dr.GetTimeSpan(2)}Malam\n{dr.GetInt64(3)}";
        //                    if (labelIndex >= panelLabels.Controls.Count && buttonIndex <= panelLabels.Controls.Count)
        //                    {
        //                        // Membuat label baru
        //                        Label newLbl = new Label();
        //                        // newLabel.AutoSize = true;
        //                        newLbl.Size = new Size(400, 200);
        //                        newLbl.Location = new Point(12, (labelIndex * 200)); // Atur posisi label sesuai dengan kebutuhan
        //                        newLbl.Font = new Font("Montserrat", 12, FontStyle.Regular);
        //                        panelLabels.Controls.Add(newLbl);

        //                        // Membuat Button Konfirmasi
        //                        Button newbtn = new Button();
        //                        newbtn.Size = new Size(160, 38);
        //                        newbtn.Font = new Font("Montserrat", 12, FontStyle.Regular);
        //                        newbtn.Text = "Konfirmasi";
        //                        newbtn.Location = new Point(3, 120);
        //                        newbtn.BackColor = Color.Aqua;
        //                        newbtn.Click += newbtn_Click;
        //                        newLbl.Controls.Add(newbtn);
        //                    }

        //                    Label label = (Label)panelLabels.Controls[labelIndex];
        //                    label.Text = rowdata;

        //                    labelIndex++;
        //                }

        //                dr.Close();
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Error: " + ex.Message);
        //            }

        //        }
        //    }
        //}

        private void newbtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda Akan Mengkonfirmasi Pesanan Ini?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                NpgsqlConnection con = new NpgsqlConnection("Server=localhost; Port=5432; Database=project; User Id=postgres; Password=fathinanisatuz01");
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO notifikasi_user (id_akun, id_kamar, id_rsv_penginapan, total_harga) SELECT id_akun, id_kamar, id_rsv_penginapan, total_harga FROM notifikasi_admin";
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
        }
        private void button_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda Akan Membatalkan Pesanan Ini?", "Pembatalan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //int newlabelindex = 0;
            if (result == DialogResult.Yes)
            {
                NpgsqlConnection con = new NpgsqlConnection("Server=localhost; Port=5432; Database=project; User Id=postgres; Password=fathinanisatuz01");
                con.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "DELETE FROM notifikasi_admin WHERE id_notifikasi_admin = @id_notifikasi_admin;";
                cmd.Parameters.Add("@id_notifikasi_admin", NpgsqlDbType.Integer).Value = id_notifikasi_admin;
                //panelLabels.Controls.Remove(labelToRemove);
                panelLabels.Controls.Add(labelToRemove); panelLabels.Controls.Add(labelToRemove);
                //labelToRemove.Dispose();
                cmd.ExecuteNonQuery();
                //cmd.Dispose();
                con.Close();
            }
        }
        //private Label GetLabelByIndex(int index)
        //{
        //    switch (index)
        //    {
        //        case 0:
        //            return label7;
        //        case 1:
        //            return label8;
        //        case 2:
        //            return label9;
        //        case 3:
        //            return label10;
        //        case 4:
        //            return label11;
        //        default:
        //            throw new ArgumentOutOfRangeException("index", "Invalid label index.");
        //    }
        //}
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resize_Control(panel1, recpnl1);
            resize_Control(PanelNotif, recpnlnt);
            resize_Control(panel2, recpnl2);
            resize_Control(label1, reclbl1);
            resize_Control(label2, reclbl2);
            resize_Control(label3, reclbl3);
            resize_Control(label4, reclbl4);
            resize_Control(label5, reclbl5);
            resize_Control(panelout, recpnlout);
        }
        private void resize_Control(Control c, Rectangle r)
        {
            float xRatio = (float)(this.Width) / (float)(formOriginalSize.Width);
            float yRatio = (float)(this.Height) / (float)(formOriginalSize.Height);
            int newX = (int)(r.X * xRatio);
            int newY = (int)(r.Y * yRatio);

            int newWidth = (int)(r.Width * xRatio);
            int newHeight = (int)(r.Height * yRatio);

            c.Location = new Point(newX, newY);
            c.Size = new Size(newWidth, newHeight);

        }

        private bool notifikasi = false;
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (notifikasi)
            {
                panelLabels.Hide();
                notifikasi = false;

            }
            else
            {
                panelLabels.Show();
                notifikasi = true;
            }
            pictureBox3.Refresh();
        }
        private void hidenotif()
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_DoubleClick(object sender, EventArgs e)
        {
        }
        private void Form1_TextChanged(object sender, EventArgs e)
        {
            float fontSize = (float)(Width + Height) / 100; // Sesuaikan rumus ini sesuai kebutuhan Anda

            // Atur ukuran font pada kontrol yang diinginkan
            label1.Font = new Font(label1.Font.FontFamily, fontSize, label1.Font.Style);
            //button1.Font = new Font(button1.Font.FontFamily, fontSize, button1.Font.Style);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void PanelNotif_Paint(object sender, PaintEventArgs e)
        {

        }

        private bool pictureprofil = false;
        private Control labelToRemove;

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (pictureprofil)
            {
                panelout.Hide();
                pictureprofil = false;
            }
            else
            {
                panelout.Show();
                Form2 profil = new Form2();
                profil.TopLevel = false;
                profil.FormBorderStyle = FormBorderStyle.None;
                this.panelout.Controls.Add(profil);
                profil.Show();
                pictureprofil = true;
            }
            hidenotif();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panelout_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}