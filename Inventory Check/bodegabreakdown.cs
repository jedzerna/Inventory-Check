using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class bodegabreakdown : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleparam = base.CreateParams;
                handleparam.ExStyle |= 0x02000000;
                return handleparam;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.DoubleBuffered = true;
        }
        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            //Taxes: Remote Desktop Connection and painting
            //http://blogs.msdn.com/oldnewthing/archive/2006/01/03/508694.aspx
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;

            System.Reflection.PropertyInfo aProp =
                  typeof(System.Windows.Forms.Control).GetProperty(
                        "DoubleBuffered",
                        System.Reflection.BindingFlags.NonPublic |
                        System.Reflection.BindingFlags.Instance);

            aProp.SetValue(c, true, null);
        }
        public string id;
        public string num;
        public bodegabreakdown()
        {
            InitializeComponent();
        }

        private void bodegabreakdown_Load(object sender, EventArgs e)
        {
            if (num == "1")
            {
                guna2Panel1.Visible = true;
                guna2Panel3.Visible = false;
            }
            else
            {
                guna2Panel1.Visible = false;
                guna2Panel3.Visible = true;
            }
            load();
            getinfo();
            if (dataGridView2.Rows.Count != 0)
            {
                decimal sum = 0.00M;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Math.Round((decimal)Convert.ToDecimal(dataGridView2.Rows[i].Cells["Column3"].Value), 2);
                }

                decimal add = 0.00M;

                add += Math.Round((decimal)Convert.ToDecimal(sum), 2) + Math.Round((decimal)Convert.ToDecimal(guna2NumericUpDown1.Text), 2);
                label4.Text = string.Format("{0:#,##0.00}", add).ToString();
            }
            else
            {
                label4.Text = "0.00";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        
        }
        private void load()
        {
            using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
            {
                itemCode.Open();
                DataTable dt = new DataTable();
                string list = "Select id,itemid,bodega,qty from tblBodega where itemid = '" + id + "'";
                SqlCommand command = new SqlCommand(list, itemCode);
                SqlDataReader reader = command.ExecuteReader();
                dt.Load(reader);
                foreach (DataRow row in dt.Rows)
                {
                    if (row["qty"].ToString() == "")
                    {
                        row["qty"] = "0.00";
                    }
                }
                dataGridView2.DataSource = dt;
                dataGridView1.DataSource = dt;
                reader.Close();
                itemCode.Close();
                itemCode.Dispose();
            }
        }
        string bodega;
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox3.Text == "")
            {
                MessageBox.Show("Please select bodega");
            }
            else if (guna2NumericUpDown1.Text == "0.00" || guna2NumericUpDown1.Text == "")
            {
                MessageBox.Show("Please don't leave the QTY or Remarks blank");
            }
            else
            {
                bool found = false;

                foreach (DataGridViewRow rows in dataGridView2.Rows)
                {
                    if (guna2ComboBox3.Text == rows.Cells["Column2"].Value.ToString())
                    {
                        MessageBox.Show("This bodega already exist");
                        found = true;
                        break; // get out of the loop
                    }
                }
                if (found == false)
                {
                    decimal sum = 0.00M;
                    for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                    {
                        sum += Math.Round((decimal)Convert.ToDecimal(dataGridView2.Rows[i].Cells["Column3"].Value), 2);
                    }

                    decimal add = 0.00M;

                    add += Math.Round((decimal)Convert.ToDecimal(sum), 2) + Math.Round((decimal)Convert.ToDecimal(guna2NumericUpDown1.Text), 2);
                    label4.Text = string.Format("{0:#,##0.00}", add).ToString();

                    if (add > Convert.ToDecimal(label2.Text))
                    {
                        MessageBox.Show("The amount of stocks reach its limits");
                    }
                    else
                    {
                        DataTable dt = dataGridView2.DataSource as DataTable;
                        DataRow row = dt.NewRow();
                        row["id"] = 0;
                        row["itemid"] = id;
                        row["bodega"] = guna2ComboBox3.Text;
                        row["qty"] = guna2NumericUpDown1.Text;
                        dt.Rows.Add(row);
                    }
                    guna2NumericUpDown1.Text = "0.00";

                }
            }

        }
        DataTable dt = new DataTable();
        public void getinfo()
        {
            dt.Rows.Clear();
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                String query = "SELECT * FROM codeMaterial WHERE ID = '" + id + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
               SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    label2.Text = string.Format("{0:#,##0.00}", Convert.ToDecimal(rdr["stocksleft"])).ToString();
                }
                codeMaterial.Close();
                codeMaterial.Dispose();
            }
        }

        private void dataGridView2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {
                decimal sum = 0.00M;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Math.Round((decimal)Convert.ToDecimal(dataGridView2.Rows[i].Cells["Column3"].Value),2);
                }
                //decimal sum = 0.00M;
                //decimal sum3 = 0.00M;

                //for (int i = 0; i < dataGridView1.Rows.Count; i++)
                //{
                //    sum += Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);

                //    label17.Text = Math.Round((decimal)Convert.ToDecimal(sum), 2).ToString("N2");

                //    sum3 += Convert.ToDecimal(dataGridView1.Rows[i].Cells[11].Value);

                //    label1.Text = Math.Round((decimal)Convert.ToDecimal(sum3), 2).ToString("N2");
                //}


                decimal add = 0.00M;

                add += Math.Round((decimal)Convert.ToDecimal(sum), 2) + Math.Round((decimal)Convert.ToDecimal(guna2NumericUpDown1.Text), 2);
                label4.Text = string.Format("{0:#,##0.00}", add).ToString();

                if (add > Convert.ToDecimal(label2.Text))
                {
                    MessageBox.Show("The amount of stocks reach its limits");
                    dataGridView2.CurrentRow.Cells["Column3"].Value = "0.00";
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {


            load();
            getinfo();
            if (dataGridView2.Rows.Count == 0)
            {
                MessageBox.Show("No Bodega found");
            }
            else
            {
                //bool found = false;

                //foreach (DataGridViewRow rows in dataGridView2.Rows)
                //{
                //    if ("0" == rows.Cells["Column1"].Value.ToString())
                //    {
                //        MessageBox.Show("You cannot edit while some aren't saved");
                //        found = true;
                //        break; // get out of the loop
                //    }
                //}
                //if (found == false)
                //{
                    if (dataGridView2.Rows.Count != 0)
                    {
                    decimal sum = 0.00M;
                    for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                    {
                        sum += Math.Round((decimal)Convert.ToDecimal(dataGridView2.Rows[i].Cells["Column3"].Value), 2);
                    }

                    decimal add = 0.00M;

                    add += Math.Round((decimal)Convert.ToDecimal(sum), 2) + Math.Round((decimal)Convert.ToDecimal(guna2NumericUpDown1.Text), 2);
                    label4.Text = string.Format("{0:#,##0.00}", add).ToString();
                }
                    else
                    {
                        label4.Text = "0.00";
                    }


                    guna2Button1.Visible = true;
                    guna2Button2.Visible = true;
                    guna2Button3.Visible = false;


                    guna2Button6.Visible = false;
                    guna2ComboBox3.Visible = false;
                    guna2NumericUpDown1.Visible = false;
                    label1.Visible = false;
                    label6.Visible = false;
                    guna2Button4.Visible = false;


                    dataGridView2.Columns["Column3"].ReadOnly = false;
                //}
            }
           
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2Button1.Visible = false;
            guna2Button2.Visible = false;
            guna2Button3.Visible = true;
            dataGridView2.Columns["Column3"].ReadOnly = true;
            guna2Button6.Visible = true;
            guna2ComboBox3.Visible = true;
            guna2NumericUpDown1.Visible = true;
            label1.Visible = true;
            label6.Visible = true;
            guna2Button4.Visible = true;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (label2.Text != label4.Text)
            {
                MessageBox.Show("Please make sure the total selected quantity is squal to the actual quantity");
            }
            else
            {
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand("update tblBodega set itemid=@itemid,bodega=@bodega,qty=@qty where id=@id", itemCode);

                        itemCode.Open();
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@id", row.Cells["Column1"].Value.ToString());
                        cmd.Parameters.AddWithValue("@itemid", row.Cells["Column4"].Value.ToString());
                        cmd.Parameters.AddWithValue("@bodega", row.Cells["Column2"].Value.ToString());
                        cmd.Parameters.AddWithValue("@qty", row.Cells["Column3"].Value.ToString());
                        cmd.ExecuteNonQuery();
                        itemCode.Close();
                    }
                }
                MessageBox.Show("Done!");
                guna2Button1.Visible = false;
                guna2Button2.Visible = false;
                guna2Button3.Visible = true;
                dataGridView2.Columns["Column3"].ReadOnly = true;


                guna2Button6.Visible = true;
                guna2ComboBox3.Visible = true;
                guna2NumericUpDown1.Visible = true;
                label1.Visible = true;
                label6.Visible = true;
                guna2Button4.Visible = true;
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Rows.Count != 0)
            {
                decimal sum = 0.00M;
                for (int i = 0; i < dataGridView2.Rows.Count; ++i)
                {
                    sum += Math.Round((decimal)Convert.ToDecimal(dataGridView2.Rows[i].Cells["Column3"].Value), 2);
                }

                decimal add = 0.00M;

                add += Math.Round((decimal)Convert.ToDecimal(sum), 2) + Math.Round((decimal)Convert.ToDecimal(guna2NumericUpDown1.Text), 2);
                label4.Text = string.Format("{0:#,##0.00}", add).ToString();
            }
            else
            {
                label4.Text = "0.00";
            }

            if (label2.Text != label4.Text)
            {
                MessageBox.Show("Please make sure the total selected quantity is squal to the actual quantity");
            }
            else
            {
                bodega = "";
                foreach (DataGridViewRow row in dataGridView2.Rows)
                {
                    if (row.Cells["Column1"].Value.ToString() == "0")
                    {

                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                        {
                            itemCode.Open();
                            string insStmt = "insert into tblBodega ([itemid], [bodega], [qty]) values" +
                                " (@itemid,@bodega,@qty)";
                            SqlCommand insCmd = new SqlCommand(insStmt, itemCode);
                            insCmd.Parameters.Clear();
                            insCmd.Parameters.AddWithValue("@itemid", row.Cells["Column4"].Value.ToString());
                            insCmd.Parameters.AddWithValue("@bodega", row.Cells["Column2"].Value.ToString());
                            insCmd.Parameters.AddWithValue("@qty", row.Cells["Column3"].Value.ToString());
                            int affectedRows = insCmd.ExecuteNonQuery();
                            itemCode.Close();
                        }
                    }
                    else
                    {

                        using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                        {
                            SqlCommand cmd = new SqlCommand("update tblBodega set itemid=@itemid,bodega=@bodega,qty=@qty where id=@id", itemCode);

                            itemCode.Open();
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@id", row.Cells["Column1"].Value.ToString());
                            cmd.Parameters.AddWithValue("@itemid", row.Cells["Column4"].Value.ToString());
                            cmd.Parameters.AddWithValue("@bodega", row.Cells["Column2"].Value.ToString());
                            cmd.Parameters.AddWithValue("@qty", row.Cells["Column3"].Value.ToString());
                            cmd.ExecuteNonQuery();
                            itemCode.Close();
                        }
                    }
                   
                   bodega += row.Cells["Column2"].Value.ToString() + ", ";

                   

                }
                //MessageBox.Show(bodega.Remove(bodega.Length - 2, 2));

                using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("update codeMaterial set dept=@dept where ID=@ID", codeMaterial);

                    codeMaterial.Open();
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@dept", bodega.Remove(bodega.Length - 2, 2));
                    cmd.ExecuteNonQuery();
                    codeMaterial.Close();
                }

                MessageBox.Show("Done!");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure to delete? Once deleted cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
                if (dialogResult == DialogResult.Yes)
                {

                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblBodega WHERE id = '" + dataGridView1.CurrentRow.Cells["dataGridViewTextBoxColumn1"].Value.ToString() + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                        itemCode.Close();
                    }

                    DataGridViewRow dgvDelRow = dataGridView1.CurrentRow;
                    dataGridView1.Rows.Remove(dgvDelRow);
                    MessageBox.Show("Deleted");
                }
            }
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
