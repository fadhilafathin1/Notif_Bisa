using Npgsql;

namespace PemberitahuanKonfirmasi
{
    public partial class inputuser : Form
    {
        public inputuser()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=Ululps01; Database=sbd"))
            {
                conn.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "insert into notifikasi_admin(id_akun, id_kamar, id_rsv_penginapan, total_harga) values(@ida, @idk, @idrp, @th)";
                    cmd.Parameters.Add(new NpgsqlParameter("@ida", int.Parse(textBox1.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@idk", int.Parse(textBox2.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@idrp", int.Parse(textBox3.Text)));
                    cmd.Parameters.Add(new NpgsqlParameter("@th", int.Parse(textBox4.Text)));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                conn.Close();
            }
            Form1 fm1 = new Form1();
            fm1.ShowDialog();
            this.Hide();
        }
    }
}