using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CarDynamics
{
    public partial class CarDataFrom : Form
    {
        public CarDataFrom()
        {            
            InitializeComponent();
            DirectInputWrapper.Initialize(this);
            
        }

        private void ESPSystemForm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(850, 5) ; 
        }
    }
}
