namespace Patrient_Record
{
    partial class forgotpassword
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.vrfycode = new System.Windows.Forms.Button();
            this.code = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.usernames = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.backtologin = new System.Windows.Forms.Button();
            this.email = new System.Windows.Forms.Label();
            this.emailthiz = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.OliveDrab;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.vrfycode);
            this.panel1.Controls.Add(this.code);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.usernames);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.backtologin);
            this.panel1.Controls.Add(this.email);
            this.panel1.Controls.Add(this.emailthiz);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(826, 461);
            this.panel1.TabIndex = 0;
            // 
            // vrfycode
            // 
            this.vrfycode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vrfycode.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vrfycode.Location = new System.Drawing.Point(587, 249);
            this.vrfycode.Name = "vrfycode";
            this.vrfycode.Size = new System.Drawing.Size(126, 43);
            this.vrfycode.TabIndex = 12;
            this.vrfycode.Text = "Verify";
            this.vrfycode.UseVisualStyleBackColor = true;
            this.vrfycode.Click += new System.EventHandler(this.vrfycode_Click);
            // 
            // code
            // 
            this.code.Font = new System.Drawing.Font("Microsoft Yi Baiti", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code.ForeColor = System.Drawing.SystemColors.GrayText;
            this.code.Location = new System.Drawing.Point(162, 263);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(378, 29);
            this.code.TabIndex = 11;
            this.code.Text = "Code";
            this.code.TextChanged += new System.EventHandler(this.code_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(159, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Enter Code";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(587, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 43);
            this.button1.TabIndex = 9;
            this.button1.Text = "Send Code";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // usernames
            // 
            this.usernames.Font = new System.Drawing.Font("Microsoft Yi Baiti", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.usernames.ForeColor = System.Drawing.SystemColors.GrayText;
            this.usernames.Location = new System.Drawing.Point(163, 115);
            this.usernames.Name = "usernames";
            this.usernames.Size = new System.Drawing.Size(550, 29);
            this.usernames.TabIndex = 8;
            this.usernames.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(159, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(168, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter UserName";
            // 
            // backtologin
            // 
            this.backtologin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.backtologin.Font = new System.Drawing.Font("Rockwell", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.backtologin.Location = new System.Drawing.Point(371, 365);
            this.backtologin.Name = "backtologin";
            this.backtologin.Size = new System.Drawing.Size(126, 56);
            this.backtologin.TabIndex = 6;
            this.backtologin.Text = "Back to LogIn";
            this.backtologin.UseVisualStyleBackColor = true;
            this.backtologin.Click += new System.EventHandler(this.button1_Click);
            // 
            // email
            // 
            this.email.AutoSize = true;
            this.email.BackColor = System.Drawing.Color.Transparent;
            this.email.Font = new System.Drawing.Font("Rockwell", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.email.Location = new System.Drawing.Point(159, 161);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(265, 23);
            this.email.TabIndex = 3;
            this.email.Text = "Enter Your  Email Address";
            // 
            // emailthiz
            // 
            this.emailthiz.Font = new System.Drawing.Font("Microsoft Yi Baiti", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailthiz.ForeColor = System.Drawing.SystemColors.GrayText;
            this.emailthiz.Location = new System.Drawing.Point(163, 187);
            this.emailthiz.Name = "emailthiz";
            this.emailthiz.Size = new System.Drawing.Size(377, 29);
            this.emailthiz.TabIndex = 1;
            this.emailthiz.Text = "Email";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Palatino Linotype", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(274, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = "Forgot Password";
            // 
            // forgotpassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightYellow;
            this.ClientSize = new System.Drawing.Size(850, 487);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "forgotpassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "forgotpassword";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox emailthiz;
        private System.Windows.Forms.Label email;
        private System.Windows.Forms.Button backtologin;
        private System.Windows.Forms.TextBox usernames;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button vrfycode;
        private System.Windows.Forms.TextBox code;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
    }
}