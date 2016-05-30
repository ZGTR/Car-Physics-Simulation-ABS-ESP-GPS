namespace CarDynamics.CarEngine.CarBase.CarRigid
{
    partial class ABSDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ABS_GroupBox = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Mu = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.Moment_Of_Inertia_T = new System.Windows.Forms.TextBox();
            this.Mu_Dervied_T = new System.Windows.Forms.TextBox();
            this.B_Pacejka_Formula_T = new System.Windows.Forms.TextBox();
            this.ABS_Status_T = new System.Windows.Forms.TextBox();
            this.Mu_T = new System.Windows.Forms.TextBox();
            this.C_Pacejka_Formula_T = new System.Windows.Forms.TextBox();
            this.A_Pacejka_Formula_T = new System.Windows.Forms.TextBox();
            this.LongitudinalSlipRatio_T = new System.Windows.Forms.TextBox();
            this.Surface_Type = new System.Windows.Forms.DomainUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Button_surface = new System.Windows.Forms.Button();
            this.ABS_GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ABS_GroupBox
            // 
            this.ABS_GroupBox.Controls.Add(this.Button_surface);
            this.ABS_GroupBox.Controls.Add(this.label2);
            this.ABS_GroupBox.Controls.Add(this.label1);
            this.ABS_GroupBox.Controls.Add(this.label20);
            this.ABS_GroupBox.Controls.Add(this.label23);
            this.ABS_GroupBox.Controls.Add(this.label17);
            this.ABS_GroupBox.Controls.Add(this.Mu);
            this.ABS_GroupBox.Controls.Add(this.label16);
            this.ABS_GroupBox.Controls.Add(this.label19);
            this.ABS_GroupBox.Controls.Add(this.label18);
            this.ABS_GroupBox.Controls.Add(this.label12);
            this.ABS_GroupBox.Controls.Add(this.Moment_Of_Inertia_T);
            this.ABS_GroupBox.Controls.Add(this.Mu_Dervied_T);
            this.ABS_GroupBox.Controls.Add(this.B_Pacejka_Formula_T);
            this.ABS_GroupBox.Controls.Add(this.textBox2);
            this.ABS_GroupBox.Controls.Add(this.textBox1);
            this.ABS_GroupBox.Controls.Add(this.ABS_Status_T);
            this.ABS_GroupBox.Controls.Add(this.Mu_T);
            this.ABS_GroupBox.Controls.Add(this.C_Pacejka_Formula_T);
            this.ABS_GroupBox.Controls.Add(this.A_Pacejka_Formula_T);
            this.ABS_GroupBox.Controls.Add(this.LongitudinalSlipRatio_T);
            this.ABS_GroupBox.Controls.Add(this.Surface_Type);
            this.ABS_GroupBox.Location = new System.Drawing.Point(10, 9);
            this.ABS_GroupBox.Name = "ABS_GroupBox";
            this.ABS_GroupBox.Size = new System.Drawing.Size(305, 400);
            this.ABS_GroupBox.TabIndex = 0;
            this.ABS_GroupBox.TabStop = false;
            this.ABS_GroupBox.Text = "ABSData";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(59, 286);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(95, 13);
            this.label20.TabIndex = 67;
            this.label20.Text = "Moment Of Inertia";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(59, 312);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(61, 13);
            this.label23.TabIndex = 66;
            this.label23.Text = "Mu Dervied";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(59, 199);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(94, 13);
            this.label17.TabIndex = 65;
            this.label17.Text = "B Pacejka Formula";
            // 
            // Mu
            // 
            this.Mu.AutoSize = true;
            this.Mu.Location = new System.Drawing.Point(68, 257);
            this.Mu.Name = "Mu";
            this.Mu.Size = new System.Drawing.Size(35, 13);
            this.Mu.TabIndex = 68;
            this.Mu.Text = "Mu(S)";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(59, 170);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(95, 13);
            this.label16.TabIndex = 71;
            this.label16.Text = "A Pacejka Formula";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(59, 108);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(98, 13);
            this.label19.TabIndex = 72;
            this.label19.Text = "ABS System Status";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(59, 227);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(95, 13);
            this.label18.TabIndex = 69;
            this.label18.Text = "C Pacejka Formula";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(59, 140);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(111, 13);
            this.label12.TabIndex = 70;
            this.label12.Text = "Longitudinal Slip Ratio";
            // 
            // Moment_Of_Inertia_T
            // 
            this.Moment_Of_Inertia_T.Location = new System.Drawing.Point(176, 283);
            this.Moment_Of_Inertia_T.Name = "Moment_Of_Inertia_T";
            this.Moment_Of_Inertia_T.Size = new System.Drawing.Size(100, 20);
            this.Moment_Of_Inertia_T.TabIndex = 59;
            // 
            // Mu_Dervied_T
            // 
            this.Mu_Dervied_T.Location = new System.Drawing.Point(176, 309);
            this.Mu_Dervied_T.Name = "Mu_Dervied_T";
            this.Mu_Dervied_T.Size = new System.Drawing.Size(100, 20);
            this.Mu_Dervied_T.TabIndex = 58;
            // 
            // B_Pacejka_Formula_T
            // 
            this.B_Pacejka_Formula_T.Location = new System.Drawing.Point(176, 196);
            this.B_Pacejka_Formula_T.Name = "B_Pacejka_Formula_T";
            this.B_Pacejka_Formula_T.Size = new System.Drawing.Size(100, 20);
            this.B_Pacejka_Formula_T.TabIndex = 57;
            // 
            // ABS_Status_T
            // 
            this.ABS_Status_T.Location = new System.Drawing.Point(176, 105);
            this.ABS_Status_T.Name = "ABS_Status_T";
            this.ABS_Status_T.Size = new System.Drawing.Size(100, 20);
            this.ABS_Status_T.TabIndex = 60;
            // 
            // Mu_T
            // 
            this.Mu_T.Location = new System.Drawing.Point(176, 254);
            this.Mu_T.Name = "Mu_T";
            this.Mu_T.Size = new System.Drawing.Size(100, 20);
            this.Mu_T.TabIndex = 64;
            // 
            // C_Pacejka_Formula_T
            // 
            this.C_Pacejka_Formula_T.Location = new System.Drawing.Point(176, 224);
            this.C_Pacejka_Formula_T.Name = "C_Pacejka_Formula_T";
            this.C_Pacejka_Formula_T.Size = new System.Drawing.Size(100, 20);
            this.C_Pacejka_Formula_T.TabIndex = 63;
            // 
            // A_Pacejka_Formula_T
            // 
            this.A_Pacejka_Formula_T.Location = new System.Drawing.Point(176, 167);
            this.A_Pacejka_Formula_T.Name = "A_Pacejka_Formula_T";
            this.A_Pacejka_Formula_T.Size = new System.Drawing.Size(100, 20);
            this.A_Pacejka_Formula_T.TabIndex = 61;
            // 
            // LongitudinalSlipRatio_T
            // 
            this.LongitudinalSlipRatio_T.Location = new System.Drawing.Point(176, 137);
            this.LongitudinalSlipRatio_T.Name = "LongitudinalSlipRatio_T";
            this.LongitudinalSlipRatio_T.Size = new System.Drawing.Size(100, 20);
            this.LongitudinalSlipRatio_T.TabIndex = 62;
            // 
            // Surface_Type
            // 
            this.Surface_Type.Items.Add("Dry Asphalt");
            this.Surface_Type.Items.Add("Wet Asphalt");
            this.Surface_Type.Items.Add("Ice");
            this.Surface_Type.Location = new System.Drawing.Point(55, 34);
            this.Surface_Type.Name = "Surface_Type";
            this.Surface_Type.Size = new System.Drawing.Size(120, 20);
            this.Surface_Type.TabIndex = 56;
            this.Surface_Type.Text = "Dry Asphalt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 73;
            this.label1.Text = "Mu_Peak";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(176, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 73;
            this.label2.Text = "Mu_low";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(108, 75);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(62, 20);
            this.textBox1.TabIndex = 60;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(224, 75);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(52, 20);
            this.textBox2.TabIndex = 60;
            // 
            // Button_surface
            // 
            this.Button_surface.Location = new System.Drawing.Point(195, 25);
            this.Button_surface.Name = "Button_surface";
            this.Button_surface.Size = new System.Drawing.Size(81, 35);
            this.Button_surface.TabIndex = 74;
            this.Button_surface.Text = "enable";
            this.Button_surface.UseVisualStyleBackColor = true;
            // 
            // ABSDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(375, 421);
            this.Controls.Add(this.ABS_GroupBox);
            this.Name = "ABSDataForm";
            this.Text = "ABSDataForm";
            this.ABS_GroupBox.ResumeLayout(false);
            this.ABS_GroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ABS_GroupBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label Mu;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox Moment_Of_Inertia_T;
        private System.Windows.Forms.TextBox Mu_Dervied_T;
        private System.Windows.Forms.TextBox B_Pacejka_Formula_T;
        private System.Windows.Forms.TextBox ABS_Status_T;
        private System.Windows.Forms.TextBox Mu_T;
        private System.Windows.Forms.TextBox C_Pacejka_Formula_T;
        private System.Windows.Forms.TextBox A_Pacejka_Formula_T;
        private System.Windows.Forms.TextBox LongitudinalSlipRatio_T;
        private System.Windows.Forms.DomainUpDown Surface_Type;
        private System.Windows.Forms.Button Button_surface;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox1;
    }
}