using System.Data.SqlClient;

namespace Libas_DB_Backup
{
    public partial class Form1 : Form
    {
        //SqlConnection con;
        //SqlCommand cmd;
        //SqlDataReader dr;
        string dbName = "Libas";

        public Form1()
        {
            InitializeComponent();
            label1.Visible = false;
        }

        public static string ReadCS()
        {
            using (var streamReader = File.OpenText("SqlSettings.txt"))//Enter FileName
            {
                var lines = streamReader.ReadToEnd();
                return lines;
            }
        }

        public void query(string que)
        {
            // ERROR: Not supported in C#: OnErrorStatement
            string con_string = ReadCS();
            using (SqlConnection con = new SqlConnection(con_string)) // new SqlConnection("Data Source=DESKTOP-BFB83V1\\SQLEXPRESS;Initial Catalog=DemoProject;User ID=sa;Password=abcd123!"))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(que, con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }

            //    con = new SqlConnection();
            //con.Open();
            //cmd = new SqlCommand(que, con);
            //cmd.ExecuteNonQuery();
            //con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                label1.Text = "";
                string con_string = ReadCS();
                dbName = getDBName(con_string);

                saveFileDialog1.FileName = "Libas_db_" + DateTime.Now.ToString("YYYY_MM_DD_hh_MM_ss.bak");
                saveFileDialog1.ShowDialog();
                string s = null;
                s = saveFileDialog1.FileName;

                if (!s.EndsWith(".bak"))
                {
                    throw new ArgumentException("localDatabasePath must end with .bak.");
                }

                query("Backup database " + dbName + " to disk='" + s + "'");
                label1.Visible = true;
                label1.Text = "Database BackUp has been created successful";

            }
            catch (Exception ex)
            {

                label1.Text = ex.StackTrace;
            }
        }

        private string getDBName(string con_string)
        {
            string[] conn = con_string.Split(";");
            string dbName = conn[1].Split("=")[1] ;
            return dbName;
        }
    }
}