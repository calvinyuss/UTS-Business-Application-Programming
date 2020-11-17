using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.BLL;

namespace UTS_FRONTEND
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TableBLL tabelBLL = new TableBLL();

            foreach (DataRow row in tabelBLL.ds.Tables["table"].AsEnumerable() )
            {
                Button button = new Button();
                button.Text = "Table "+row["table_number"];
                button.Name = row["id"].ToString();
                button.Size = new System.Drawing.Size(160, 124);
                button.UseVisualStyleBackColor = true;

                button.Click += new System.EventHandler(this.button_Click);
                flowLayoutPanel1.Controls.Add(button);
            }
            
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            this.Visible = false;

            TableBLL tableBLL = new TableBLL();

            DataRow row = tableBLL.ds.Tables["table"].Select("id="+btn.Name).FirstOrDefault();

            int id = int.Parse(row["id"].ToString());
            int tableNumber = int.Parse(row["table_number"].ToString());

            MenuClient page = new MenuClient(id, tableNumber);

            page.ShowDialog();

            this.Close();
        }
    }
}
