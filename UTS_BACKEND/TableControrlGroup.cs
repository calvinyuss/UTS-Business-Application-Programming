using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL.BLL;

namespace UTS_BACKEND
{
    public partial class TableControrlGroup : UserControl
    {
        private TableBLL tableBLL;

        public TableControrlGroup()
        {
            InitializeComponent();
        }

        private void TableControrlGroup_Load(object sender, EventArgs e)
        {
            this.tableBLL = new TableBLL();
            bindingSource.DataSource = tableBLL.ds;
            dataGridView1.DataSource = bindingSource;
            dataGridView1.DataMember = "table";

            textBoxUpdateId.DataBindings.Add("Text", bindingSource, "table.id", true, DataSourceUpdateMode.OnPropertyChanged);
            textUpdateTableNumber.DataBindings.Add("Text", bindingSource, "table.table_number");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                tableBLL.update(textBoxUpdateId.Text.ToString(), textUpdateTableNumber.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                tableBLL.insert(textAddTableNumber.Text.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxUpdateId_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells[0].Value.ToString() == textBoxUpdateId.Text.ToString())
                {
                    dataGridView1.ClearSelection();
                    item.Selected = true;
                    return;
                }
            }

            MessageBox.Show("No ID Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
