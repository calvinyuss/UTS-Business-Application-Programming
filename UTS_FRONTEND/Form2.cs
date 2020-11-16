using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using DAL.BLL;
using System.IO;

namespace UTS_FRONTEND
{
    public partial class MenuClient : Form
    {
        private int tableID;
        private int tableNumber;

        public MenuClient(int tableID, int tableNumber)
        {
            this.tableID = tableID;
            this.tableNumber = tableNumber;
            InitializeComponent();
        }

        private void MenuClient_Load(object sender, EventArgs e)
        {
            dgvButtonColumn();

            fetchMenuToTable();
        }

        private void fetchMenuToTable()
        {
            MenuBLL menuBLL = new MenuBLL();
            List<DAL.Menu> menus = menuBLL.fetchMenu();

            tableLayoutPanel1.RowCount = menus.Count;

            for (int i = 0; i < menus.Count; i++)
            {
                Label lblPrice = new Label();
                lblPrice.Dock = DockStyle.Fill;
                lblPrice.TextAlign = ContentAlignment.MiddleLeft;

                Label lblName = new Label();
                lblName.Dock = DockStyle.Fill;
                lblName.TextAlign = ContentAlignment.MiddleLeft;

                lblName.Text = "Nama: " + menus[i].name;
                lblPrice.Text = "Harga: " + menus[i].price.ToString("N0");
                string imageUrl = @"\resources\images\" + menus[i].img_url;

                PictureBox picture = new PictureBox();
                picture.Dock = DockStyle.Fill;
                picture.ImageLocation = imageUrl;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Button addBtn = new Button();
                addBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
                addBtn.Text = "Add";
                addBtn.Name = menus[i].id.ToString();
                addBtn.Click += new System.EventHandler(this.handleAddOrder);
                addBtn.UseVisualStyleBackColor = true;

                tableLayoutPanel1.Controls.Add(lblName, 0, i);
                tableLayoutPanel1.Controls.Add(lblPrice, 1, i);
                tableLayoutPanel1.Controls.Add(picture, 2, i);
                tableLayoutPanel1.Controls.Add(addBtn, 3, i);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            }

            tableLayoutPanel1.RowStyles[0].Height = 100F;
            tableLayoutPanel1.RowStyles[0].SizeType = SizeType.Absolute;
        }

        // add button to dgv column
        private void dgvButtonColumn()
        {
            var deleteButton = new DataGridViewButtonColumn();
            deleteButton.Name = "dataGridViewDeleteButton";
            deleteButton.HeaderText = "Delete";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            this.dataGridView1.Columns.Add(deleteButton);
        }

        void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //if click is on new row or header row
            if (e.RowIndex == dataGridView1.NewRowIndex || e.RowIndex < 0)
                return;

            //Check if click is on specific column 
            if (e.ColumnIndex == dataGridView1.Columns["dataGridViewDeleteButton"].Index) 
            {

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                int qty = (int)row.Cells[3].Value;
                int price = int.Parse(row.Cells[2].Value.ToString(), System.Globalization.NumberStyles.Number);
    

                if(qty == 1)
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }

                qty -= 1;
                int totalPrice = price * qty;

                row.Cells[3].Value = qty;
                row.Cells[4].Value = totalPrice.ToString("N0");
            }
        }

        /**
         * checkout new order
         */
        public void handleAddOrder(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int menuID = int.Parse(btn.Name);

            DAL.Menu selectedMenu = new MenuBLL().getMenu(menuID);

            if(dataGridView1.Rows.Count == 0)
            {
                // add to table
                string v = selectedMenu.price.ToString("N0");
                dataGridView1.Rows.Add(selectedMenu.id, selectedMenu.name, v, 1, v, "deleted");
                return;

            } 

            // check if menu already exsits 
            // if exists append tambah qty aja 
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if((int)row.Cells[0].Value == menuID)
                {
                    int qty = (int)row.Cells[3].Value;
                    int price = int.Parse(row.Cells[2].Value.ToString(), System.Globalization.NumberStyles.Number);
                    qty += 1;
                    int totalPrice = price * qty;

                    row.Cells[3].Value = qty;
                    row.Cells[4].Value = totalPrice.ToString("N0");

                    return;
                }
            }

            // add to table
            string v1 = selectedMenu.price.ToString("N0");
            dataGridView1.Rows.Add(selectedMenu.id, selectedMenu.name, v1, 1, v1, "deleted");
            return;

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0 )
            {
                MessageBox.Show("Create some order", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                OrderItem orderItem = new OrderItem();
                orderItem.menu_id = (int)row.Cells[0].Value;
                orderItem.quantity = byte.Parse(row.Cells[3].Value.ToString());
                orderItem.unit_price = int.Parse(row.Cells[2].Value.ToString(), System.Globalization.NumberStyles.Number);
                orderItems.Add(orderItem);
            }

            OrderBLL orderBLL = new OrderBLL();

            orderBLL.createOrder(tableID, orderItems);

            textBox1.AppendText("Payment Details\n");
            textBox1.AppendText("Table Number : "+tableNumber+"\n");
            textBox1.AppendText(String.Format("Payment ID: {0}\n", orderBLL.lastCreatedPaymentID ));
            textBox1.AppendText("Order List:\n");
            int totalPrice = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow row = dataGridView1.Rows[i];

                textBox1.AppendText(String.Format("{0}. {1,-20} {2}x\t Rp.{3,-5:N0}\n", 
                    i+1,
                    row.Cells[1].Value,
                    row.Cells[3].Value,
                    row.Cells[2].Value));

                totalPrice += (int.Parse(row.Cells[2].Value.ToString(), System.Globalization.NumberStyles.Number)*(int)row.Cells[3].Value);
            }
            textBox1.AppendText(String.Format("Total Payment \t\t Rp.{0:N0}", totalPrice));

            MessageBox.Show("Order Created", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1) return;

            List<DAL.Menu> menus = new List<DAL.Menu>();

            if (comboBox1.SelectedIndex == 0)
            {
                menus = new MenuBLL().getWeeklyTopSales();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                menus = new MenuBLL().getWeeklyTopSales();
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                menus = new MenuBLL().fetchMenuSortByPriceAsc();
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                menus = new MenuBLL().fetchMenuSortByPriceDesc();
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                menus = new MenuBLL().fetchMenuSortByNameDesc();
            }
            else if (comboBox1.SelectedIndex == 5)
            {
                menus = new MenuBLL().fetchMenuSortByNameAsc();
            }

            tableLayoutPanel1.Controls.Clear();
            foreach (DAL.Menu item in menus)
            {
                Console.WriteLine(item.name);
            }

            tableLayoutPanel1.RowCount = menus.Count;

            for (int i = 0; i < menus.Count; i++)
            {
                Label lblPrice = new Label();
                lblPrice.Dock = DockStyle.Fill;
                lblPrice.TextAlign = ContentAlignment.MiddleLeft;

                Label lblName = new Label();
                lblName.Dock = DockStyle.Fill;
                lblName.TextAlign = ContentAlignment.MiddleLeft;

                lblName.Text = "Nama: " + menus[i].name;
                lblPrice.Text = "Harga: " + menus[i].price.ToString("N0");
                string imageUrl = @"\resources\images\" + menus[i].img_url;

                PictureBox picture = new PictureBox();
                picture.Dock = DockStyle.Fill;
                picture.ImageLocation = imageUrl;
                picture.SizeMode = PictureBoxSizeMode.StretchImage;

                Button addBtn = new Button();
                addBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
                addBtn.Text = "Add";
                addBtn.Name = menus[i].id.ToString();
                addBtn.Click += new System.EventHandler(this.handleAddOrder);
                addBtn.UseVisualStyleBackColor = true;

                tableLayoutPanel1.Controls.Add(lblName, 0, i);
                tableLayoutPanel1.Controls.Add(lblPrice, 1, i);
                tableLayoutPanel1.Controls.Add(picture, 2, i);
                tableLayoutPanel1.Controls.Add(addBtn, 3, i);

                tableLayoutPanel1.RowStyles.Add(new RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
        }
    }
}
