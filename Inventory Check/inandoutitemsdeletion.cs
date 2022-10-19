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
    public partial class inandoutitemsdeletion : Form
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
        public inandoutitemsdeletion()
        {
            InitializeComponent();
        }
        public string name;
        public string itempoid;
        public string iitem;
        public string itemstocks;
        public string forms;
        public string operation;
        public string poid;
        decimal stocks;


        private string description;
        private string productcode;

        private void inandoutitemsdeletion_Load(object sender, EventArgs e)
        {
            getinfo();
        }
        string podrnum;
        private void getinfo()
        {
            stocks = 0.00M;
            using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
            {
                codeMaterial.Open();
                DataTable dt = new DataTable();
                String query = "SELECT * FROM codeMaterial WHERE ID = '" + iitem + "'";
                SqlCommand cmd = new SqlCommand(query, codeMaterial);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    stocks = Convert.ToDecimal(rdr["stocksleft"].ToString());
                    description = rdr["description"].ToString();
                    productcode = rdr["product_code"].ToString();
                    label4.Text = productcode;
                    label5.Text = description;
                }
                codeMaterial.Close();
            }
            if (forms == "insupply")
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    String query = "SELECT * FROM itemCode WHERE Id = '" + itempoid + "'";
                    SqlCommand cmd = new SqlCommand(query, itemCode);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        qty = Convert.ToDecimal(rdr["qty"].ToString());
                        totalstocks = Convert.ToDecimal(rdr["stocksleft"].ToString());
                    }
                    else
                    {
                        MessageBox.Show("Item is already deleted, Please reopen the DR information to refresh. Thank you");
                        this.Close();
                    }
                    itemCode.Close();
                }
                using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                {
                    //dataGridView1.Rows.Clear();
                    tblIn.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT operation,ponumber FROM tblIn WHERE Id = '" + poid + "'";
                    SqlCommand cmd = new SqlCommand(query, tblIn);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        podrnum = (rdr["ponumber"].ToString());
                        operation = (rdr["operation"].ToString());
                    }
                    tblIn.Close();

                }
            }
            else
            {
                using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                {
                    itemCode.Open();
                    String query = "SELECT * FROM tblDRitemCode WHERE id = '" + itempoid + "'";
                    SqlCommand cmd = new SqlCommand(query, itemCode);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        qty = Convert.ToDecimal(rdr["qty"].ToString());
                        totalstocks = Convert.ToDecimal(rdr["stocksleft"].ToString());
                    }
                    else
                    {
                        MessageBox.Show("Item is already deleted, Please reopen the DR information to refresh. Thank you");
                        this.Close();
                    }
                    itemCode.Close();
                }
                using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                {
                    //dataGridView1.Rows.Clear();
                    dbDR.Open();
                    DataTable dt = new DataTable();
                    String query = "SELECT operation,drnumber FROM tblDR WHERE Id = '" + poid + "'";
                    SqlCommand cmd = new SqlCommand(query, dbDR);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        podrnum = (rdr["drnumber"].ToString());
                        operation = (rdr["operation"].ToString());
                    }
                    dbDR.Close();
                }
            }


        }
        private decimal qty = 0.00M;
        private decimal totalstocks = 0.00M;

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2TextBox3.Text == "")
            {
                MessageBox.Show("Please enter your remarks on why deleting this. Thank you");
                return;
            }
            DialogResult dialogResult = MessageBox.Show("Are you sure to delete? Once deleted cannot be undone.", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            if (dialogResult == DialogResult.No)
            {
                return;
            }

            getinfo();

            if (forms == "insupply")
            {
                if (operation == "Completed")
                {
                    getinfo();
                    decimal sum = 0.00M;
                    sum = stocks - Convert.ToDecimal(itemstocks);

                    string completepo = "Deleted " + productcode + "";
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                        codeMaterial.Open();
                        cmd.Parameters.AddWithValue("@ID", iitem);
                        cmd.Parameters.AddWithValue("@stocksleft", string.Format("{0:#,##0.00}", sum));
                        cmd.ExecuteNonQuery();
                        codeMaterial.Close();


                        string remarks = "Deleted stock with the QTY of " + itemstocks + " PO: " + podrnum;
                        SqlCommand cmda = new SqlCommand("update tblHistory set aqty=@aqty,stock=@stock,remarks=@remarks where peritemid=@peritemid AND type=@type AND podrid=@podrid", codeMaterial);
                        codeMaterial.Open();
                        cmda.Parameters.AddWithValue("@peritemid", itempoid);
                        cmda.Parameters.AddWithValue("@type", "PO");
                        cmda.Parameters.AddWithValue("@podrid", poid);
                        cmda.Parameters.AddWithValue("@aqty", string.Format("{0:#,##0.00}", qty));
                        cmda.Parameters.AddWithValue("@stock", string.Format("{0:#,##0.00}", totalstocks));
                        cmda.Parameters.AddWithValue("@remarks", remarks);
                        cmda.ExecuteNonQuery();
                        codeMaterial.Close();



                        codeMaterial.Open();
                        string insStmt = "insert into tblHistory ([itemid], [date], [operation], [product_code], [description], [name],[remarks], [dqty],[stock],[type],[podrid]) values" +
                            " (@itemid,@date,@operation,@product_code,@description,@name,@remarks,@dqty,@stock,@type,@podrid)";
                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                        insCmd.Parameters.AddWithValue("@itemid", iitem);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                        insCmd.Parameters.AddWithValue("@operation", remarks);
                        insCmd.Parameters.AddWithValue("@product_code", productcode);
                        insCmd.Parameters.AddWithValue("@description", description);
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@remarks", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@dqty", string.Format("{0:#,##0.00}", Convert.ToDecimal(itemstocks)));
                        insCmd.Parameters.AddWithValue("@stock", string.Format("{0:#,##0.00}", sum));
                        insCmd.Parameters.AddWithValue("@type", "PDL");
                        insCmd.Parameters.AddWithValue("@podrid", poid);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();
                    }

                    using (SqlConnection tblIn = new SqlConnection(ConfigurationManager.ConnectionStrings["tblIn"].ConnectionString))
                    {


                        tblIn.Open();
                        string insStmt = "insert into tblInHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, tblIn);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", poid);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        tblIn.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM itemCode WHERE Id = '" + itempoid + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                        itemCode.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblSIitems WHERE idperitem = '" + itempoid + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                        itemCode.Close();
                    }
                    MessageBox.Show("Deleted" + Environment.NewLine + Environment.NewLine + "Please reopen the DR to take effect");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("This item is not completed.");
                    return;
                }
            }
            else
            {
                if (operation == "Completed")
                {
                    getinfo();
                    decimal sum = 0.00M;
                    sum = stocks + Convert.ToDecimal(itemstocks);
                    string completepo = "Deleted " + productcode + "";
                    using (SqlConnection codeMaterial = new SqlConnection(ConfigurationManager.ConnectionStrings["codeMaterial"].ConnectionString))
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        SqlCommand cmd = new SqlCommand("update codeMaterial set stocksleft=@stocksleft where ID=@ID", codeMaterial);
                        codeMaterial.Open();
                        cmd.Parameters.AddWithValue("@ID", iitem);
                        cmd.Parameters.AddWithValue("@stocksleft", string.Format("{0:#,##0.00}", sum));
                        cmd.ExecuteNonQuery();
                        codeMaterial.Close();


                        string remarks = "Deleted item and returning with the QTY of " + itemstocks + " DR: " + podrnum; ;
                        SqlCommand cmda = new SqlCommand("update tblHistory set dqty=@dqty,stock=@stock,remarks=@remarks where peritemid=@peritemid AND type=@type AND podrid=@podrid", codeMaterial);
                        codeMaterial.Open();
                        cmda.Parameters.AddWithValue("@peritemid", itempoid);
                        cmda.Parameters.AddWithValue("@type", "DR");
                        cmda.Parameters.AddWithValue("@podrid", poid);
                        cmda.Parameters.AddWithValue("@dqty", string.Format("{0:#,##0.00}", qty));
                        cmda.Parameters.AddWithValue("@stock", string.Format("{0:#,##0.00}", totalstocks));
                        cmda.Parameters.AddWithValue("@remarks", remarks);
                        cmda.ExecuteNonQuery();
                        codeMaterial.Close();



                        codeMaterial.Open();
                        string insStmt = "insert into tblHistory ([itemid], [date], [operation], [product_code], [description], [name],[remarks], [aqty],[stock],[type],[podrid]) values" +
                            " (@itemid,@date,@operation,@product_code,@description,@name,@remarks,@aqty,@stock,@type,@podrid)";
                        SqlCommand insCmd = new SqlCommand(insStmt, codeMaterial);
                        insCmd.Parameters.AddWithValue("@itemid", iitem);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy"));
                        insCmd.Parameters.AddWithValue("@operation", remarks);
                        insCmd.Parameters.AddWithValue("@product_code", productcode);
                        insCmd.Parameters.AddWithValue("@description", description);
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@remarks", guna2TextBox3.Text);
                        insCmd.Parameters.AddWithValue("@aqty", string.Format("{0:#,##0.00}", Convert.ToDecimal(itemstocks)));
                        insCmd.Parameters.AddWithValue("@stock", string.Format("{0:#,##0.00}", sum));
                        insCmd.Parameters.AddWithValue("@type", "DDL");
                        insCmd.Parameters.AddWithValue("@podrid", poid);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        codeMaterial.Close();
                    }
                    using (SqlConnection dbDR = new SqlConnection(ConfigurationManager.ConnectionStrings["dbDR"].ConnectionString))
                    {

                        dbDR.Open();
                        string insStmt = "insert into tblDRHistory ([name], [date], [operation], [id]) values" +
                            " (@name,@date,@operation,@id)";
                        SqlCommand insCmd = new SqlCommand(insStmt, dbDR);
                        insCmd.Parameters.Clear();
                        insCmd.Parameters.AddWithValue("@name", name);
                        insCmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"));
                        insCmd.Parameters.AddWithValue("@operation", completepo);
                        insCmd.Parameters.AddWithValue("@id", poid);
                        int affectedRows = insCmd.ExecuteNonQuery();
                        dbDR.Close();
                    }
                    using (SqlConnection itemCode = new SqlConnection(ConfigurationManager.ConnectionStrings["itemCode"].ConnectionString))
                    {
                        itemCode.Open();
                        using (SqlCommand command = new SqlCommand("DELETE FROM tblDRitemCode WHERE Id = '" + itempoid + "'", itemCode))
                        {
                            command.ExecuteNonQuery();
                        }
                        itemCode.Close();
                    }
                    MessageBox.Show("Deleted" + Environment.NewLine + Environment.NewLine + "Please reopen the DR to take effect");

                    this.Close();
                }
                else
                {
                    MessageBox.Show("This item is not completed.");
                    return;
                }
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
