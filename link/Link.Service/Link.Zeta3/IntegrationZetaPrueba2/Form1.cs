using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntegrationZetaPrueba2
{
    public partial class Form1 : Form
    {
        private ConnectionZeta2 connection;
        public Form1()
        {
            InitializeComponent();
            connection = new ConnectionZeta2();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
             JObject s= connection.GetArticles("201.221.29.3", "NOVA", "NOVA");
             MessageBox.Show("h");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            JObject s = connection.PostPurchase("201.221.29.3", "NOVA", "NOVA");
            MessageBox.Show("h");
        }
    }
}
