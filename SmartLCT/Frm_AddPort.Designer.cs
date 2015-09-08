namespace SmartLCT
{
    partial class Frm_AddPort
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
            this.button_Add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_PortIndex = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_SenderIndex = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(128, 141);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(56, 28);
            this.button_Add.TabIndex = 0;
            this.button_Add.Text = "添加";
            this.button_Add.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "选择发送卡：";
            // 
            // textBox_PortIndex
            // 
            this.textBox_PortIndex.Location = new System.Drawing.Point(128, 87);
            this.textBox_PortIndex.Name = "textBox_PortIndex";
            this.textBox_PortIndex.Size = new System.Drawing.Size(93, 21);
            this.textBox_PortIndex.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "添加的网口号：";
            // 
            // comboBox_SenderIndex
            // 
            this.comboBox_SenderIndex.FormattingEnabled = true;
            this.comboBox_SenderIndex.Location = new System.Drawing.Point(128, 31);
            this.comboBox_SenderIndex.Name = "comboBox_SenderIndex";
            this.comboBox_SenderIndex.Size = new System.Drawing.Size(124, 20);
            this.comboBox_SenderIndex.TabIndex = 4;
            // 
            // Frm_AddPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 197);
            this.Controls.Add(this.comboBox_SenderIndex);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_PortIndex);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Add);
            this.Name = "Frm_AddPort";
            this.Text = "添加网口";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_PortIndex;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_SenderIndex;
    }
}