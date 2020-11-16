using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using DAL.BLL;

namespace UTS_BACKEND
{
    public partial class AdminPage : Form
    {
        private List<DAL.Menu> menus;

        private string urlFotoAdd;
        private string urlFotoUpdate;
        private int selectedIndex = 1;

        public AdminPage()
        {
            InitializeComponent();
        }

        private void AdminPage_Load(object sender, EventArgs e)
        {
            menus = new MenuBLL().fetchMenu();
            addToTable(menus);

            dataGridView2.DataSource = new OrderBLL().getTopOrder();
            dataGridView2.DataMember = "topOrder";
            dataGridView3.DataSource = new OrderBLL().getPaymentOrder();
            dataGridView3.DataMember = "paymentOrder";
            dataGridView4.DataSource = new OrderBLL().getOrderItem();
            dataGridView4.DataMember = "orderItem";
        }

        private void buttonAddFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = openFileDialog1;
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();

            urlFotoAdd = fileDialog.FileName;
        }

        private void btnUpdateFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = openFileDialog1;
            fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            fileDialog.Multiselect = false;
            fileDialog.ShowDialog();

            urlFotoUpdate = fileDialog.FileName;
        }

        private void resetAddInput()
        {
            urlFotoAdd = null;
            textAddNama.Clear();
            textAddHarga.Clear();
        }

        private void resetUpdateInput()
        {
            urlFotoUpdate = null;
            textUpdateID.Clear();
            textUpdateNama.Clear();
            textUpdateHarga.Clear();
        }

        private void btnPreviewUpdateFoto_Click(object sender, EventArgs e)
        {
            PreviewImageDialog previewDialog = new PreviewImageDialog();

            previewDialog.addImage(urlFotoUpdate);
            previewDialog.ShowDialog();
        }

        private void btnPreviewAddFoto_Click(object sender, EventArgs e)
        {
            PreviewImageDialog previewDialog = new PreviewImageDialog();

            previewDialog.addImage(urlFotoAdd);
            previewDialog.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                MenuBLL menuBL = new MenuBLL();
                string name = textAddNama.Text;
                double price = Double.Parse(textAddHarga.Text);
                string url_foto = urlFotoAdd;

                menuBL.handleCreate(name, price, url_foto);

                DAL.Menu newMenu = menuBL.getLastInsertedData();
                addToTable(newMenu);

                menus.Add(newMenu);
                resetAddInput();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void addToTable(List<DAL.Menu> menus)
        {
            foreach (DAL.Menu menu in menus)
            {
                addToTable(menu);
            }
        }

        private void addToTable(DAL.Menu menu)
        {
            dataGridView1.Rows.Add(menu.id, menu.name, menu.price.ToString("N0"));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MenuBLL menuBL = new MenuBLL();

            int id = int.Parse(textUpdateID.Text);
            string name = textUpdateNama.Text;
            double price = Double.Parse(textUpdateHarga.Text);
            string url_foto = urlFotoUpdate;

            menuBL.handleUpdate(id, name, price, url_foto);

            // update to to view
            updateDataTable(id, name, price, url_foto);
        }

        private void updateDataTable(int id, string name, double price, string url_foto)
        {
            // find id at list
            for (int i = 0; i < menus.Count; i++)
            {
                if (menus[i].id == id)
                {
                    // select table
                    menus[i].name = name;
                    menus[i].price = (decimal)price;
                    menus[i].img_url = url_foto;

                    dataGridView1.Rows[i].Cells[1].Value = name;
                    dataGridView1.Rows[i].Cells[2].Value = price.ToString("N0");
                }
            }
        }

        /**
         * Get selected row on table and bind to
         */
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            selectedIndex = dataGridView1.CurrentCell.RowIndex;

            if(selectedIndex == -1)
            {
                resetUpdateInput();
                return;
            }

            DAL.Menu menu = menus[selectedIndex];

            textUpdateID.Text = menu.id.ToString();
            textUpdateNama.Text = menu.name;
            textUpdateHarga.Text = menu.price.ToString();
            urlFotoUpdate = @"\resources\images\"+menu.img_url;
        }

        /**
         * When user input id
         */
        private void textUpdateID_Leave(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(textUpdateID.Text);
                for (int i = 0; i < menus.Count; i++)
                {
                    if (menus[i].id == id)
                    {
                        // select table
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                        dataGridView1.Rows[i].Cells[0].Selected = true;
                        return;
                    }
                }
                // if id not found throw error
                throw new Exception("ID not found");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int position = dataGridView1.CurrentCell.RowIndex;
            int id = menus[position].id;
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);

            new MenuBLL().remove(id);

            menus.RemoveAt(dataGridView1.CurrentCell.RowIndex);

            resetUpdateInput();
        }
    }
}