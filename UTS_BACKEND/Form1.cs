using System;
using System.Windows.Forms;
using DAL.BLL;

namespace UTS_BACKEND
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginShortcut(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginBtn.PerformClick();
                // these last two lines will stop the beep sound
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            AdminBLL adminBLL = new AdminBLL();
            adminBLL.AttemptLogin(this.textBoxUsername.Text, this.textBoxPassword.Text);

            if (!adminBLL.loggedIn)
            {
                MessageBox.Show("Invalid Username and Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Visible = false;

            AdminPage page = new AdminPage();
            page.ShowDialog();

            this.Close();
        }
    }
}
