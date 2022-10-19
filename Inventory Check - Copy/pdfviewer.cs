using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Check
{
    public partial class pdfviewer : Form
    {
        public pdfviewer()
        {
            InitializeComponent();
        }

        private void pdfviewer_Load(object sender, EventArgs e)
        {

            //string targetPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\jrstocksinstructions.pdf";
            //axAcroPDF1.LoadFile(targetPath);
        }
    }
}
