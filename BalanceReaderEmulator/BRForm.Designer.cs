namespace BalanceReaderEmulator
{
    partial class BRForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BRForm));
            this.header = new System.Windows.Forms.TextBox();
            this.ReadersList = new System.Windows.Forms.ComboBox();
            this.readButton = new System.Windows.Forms.Button();
            this.choiceTextBox = new System.Windows.Forms.TextBox();
            this.apdusTextBox = new System.Windows.Forms.TextBox();
            this.reloadButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.SystemColors.Window;
            this.header.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.header.Cursor = System.Windows.Forms.Cursors.Default;
            this.header.Font = new System.Drawing.Font("Georgia", 18F);
            this.header.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.header.Location = new System.Drawing.Point(19, 12);
            this.header.Name = "header";
            this.header.ReadOnly = true;
            this.header.Size = new System.Drawing.Size(323, 28);
            this.header.TabIndex = 0;
            this.header.Text = "BalanceReader Emulator";
            this.header.Enter += new System.EventHandler(this.constTextBoxEnter);
            // 
            // ReadersList
            // 
            this.ReadersList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReadersList.FormattingEnabled = true;
            this.ReadersList.Location = new System.Drawing.Point(19, 75);
            this.ReadersList.Name = "ReadersList";
            this.ReadersList.Size = new System.Drawing.Size(462, 21);
            this.ReadersList.TabIndex = 2;
            this.ReadersList.SelectedIndexChanged += new System.EventHandler(this.ReadersListSelectedIndexChanged);
            // 
            // readButton
            // 
            this.readButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.readButton.Location = new System.Drawing.Point(19, 102);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(134, 23);
            this.readButton.TabIndex = 3;
            this.readButton.Text = "Read Values";
            this.readButton.UseVisualStyleBackColor = false;
            this.readButton.Click += new System.EventHandler(this.readButtonClick);
            // 
            // choiceTextBox
            // 
            this.choiceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.choiceTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.choiceTextBox.Location = new System.Drawing.Point(19, 56);
            this.choiceTextBox.Name = "choiceTextBox";
            this.choiceTextBox.Size = new System.Drawing.Size(100, 13);
            this.choiceTextBox.TabIndex = 4;
            this.choiceTextBox.Text = "Choose your reader:";
            this.choiceTextBox.Enter += new System.EventHandler(this.constTextBoxEnter);
            // 
            // apdusTextBox
            // 
            this.apdusTextBox.BackColor = System.Drawing.Color.Black;
            this.apdusTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.apdusTextBox.ForeColor = System.Drawing.Color.White;
            this.apdusTextBox.Location = new System.Drawing.Point(19, 131);
            this.apdusTextBox.Multiline = true;
            this.apdusTextBox.Name = "apdusTextBox";
            this.apdusTextBox.ReadOnly = true;
            this.apdusTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.apdusTextBox.Size = new System.Drawing.Size(553, 268);
            this.apdusTextBox.TabIndex = 6;
            // 
            // reloadButton
            // 
            this.reloadButton.BackColor = System.Drawing.Color.WhiteSmoke;
            this.reloadButton.Location = new System.Drawing.Point(487, 73);
            this.reloadButton.Name = "reloadButton";
            this.reloadButton.Size = new System.Drawing.Size(75, 23);
            this.reloadButton.TabIndex = 7;
            this.reloadButton.Text = "Reload";
            this.reloadButton.UseVisualStyleBackColor = false;
            this.reloadButton.Click += new System.EventHandler(this.refreshButtonClick);
            // 
            // BRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(584, 411);
            this.Controls.Add(this.reloadButton);
            this.Controls.Add(this.apdusTextBox);
            this.Controls.Add(this.choiceTextBox);
            this.Controls.Add(this.readButton);
            this.Controls.Add(this.ReadersList);
            this.Controls.Add(this.header);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(600, 450);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "BRForm";
            this.Text = "BalanceReader Emulator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox header;
        private System.Windows.Forms.ComboBox ReadersList;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Button reloadButton;
        private System.Windows.Forms.TextBox choiceTextBox;
        private System.Windows.Forms.TextBox apdusTextBox;
        
    }
}