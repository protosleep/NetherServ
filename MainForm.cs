using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetherServ.Network;

namespace NetherServ
{
    public partial class mMainForm : Form
    {
        public mMainForm()
        {
            InitializeComponent();

            UdpReceiver.Instance.Start();
        }
    }
}
