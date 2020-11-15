using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UTS_BACKEND
{
    public partial class PreviewImageDialog : Form
    {
        public PreviewImageDialog()
        {
            InitializeComponent();
        }

        public void addImage(string imageUrl)
        {
            pictureBox1.ImageLocation = imageUrl;
        }

    }
}
