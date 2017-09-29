using System.Drawing;
using System.Windows.Forms;

namespace WifiOverwatch
{
    partial class WifiOverwatchMainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WifiOverwatchMainForm));
            this.beginPingButton = new System.Windows.Forms.Button();
            this.autoReconnectCheckbox = new System.Windows.Forms.CheckBox();
            this.stopPingButton = new System.Windows.Forms.Button();
            this.quickTestCheckBox = new System.Windows.Forms.CheckBox();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.outputLabel = new System.Windows.Forms.Label();
            this.inputLabel = new System.Windows.Forms.Label();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.autoReconnectFailureCountInputTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.saveSessionButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // beginPingButton
            // 
            this.beginPingButton.Location = new System.Drawing.Point(596, 12);
            this.beginPingButton.Name = "beginPingButton";
            this.beginPingButton.Size = new System.Drawing.Size(141, 56);
            this.beginPingButton.TabIndex = 0;
            this.beginPingButton.Text = "Begin Ping";
            this.beginPingButton.UseVisualStyleBackColor = true;
            this.beginPingButton.Click += new System.EventHandler(this.startPingButton_Click);
            // 
            // autoReconnectCheckbox
            // 
            this.autoReconnectCheckbox.AutoSize = true;
            this.autoReconnectCheckbox.ForeColor = System.Drawing.Color.Lime;
            this.autoReconnectCheckbox.Location = new System.Drawing.Point(12, 103);
            this.autoReconnectCheckbox.Name = "autoReconnectCheckbox";
            this.autoReconnectCheckbox.Size = new System.Drawing.Size(151, 24);
            this.autoReconnectCheckbox.TabIndex = 1;
            this.autoReconnectCheckbox.Text = "Auto Reconenct";
            this.autoReconnectCheckbox.UseVisualStyleBackColor = true;
            // 
            // stopPingButton
            // 
            this.stopPingButton.Location = new System.Drawing.Point(596, 86);
            this.stopPingButton.Name = "stopPingButton";
            this.stopPingButton.Size = new System.Drawing.Size(141, 56);
            this.stopPingButton.TabIndex = 3;
            this.stopPingButton.Text = "StopPing";
            this.stopPingButton.UseVisualStyleBackColor = true;
            this.stopPingButton.Click += new System.EventHandler(this.stopPingButton_Click);
            // 
            // quickTestCheckBox
            // 
            this.quickTestCheckBox.AutoSize = true;
            this.quickTestCheckBox.ForeColor = System.Drawing.Color.Lime;
            this.quickTestCheckBox.Location = new System.Drawing.Point(12, 29);
            this.quickTestCheckBox.Name = "quickTestCheckBox";
            this.quickTestCheckBox.Size = new System.Drawing.Size(114, 24);
            this.quickTestCheckBox.TabIndex = 4;
            this.quickTestCheckBox.Text = "Quick  Test";
            this.quickTestCheckBox.UseVisualStyleBackColor = true;
            // 
            // inputTextBox
            // 
            this.inputTextBox.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.inputTextBox.ForeColor = System.Drawing.Color.Lime;
            this.inputTextBox.Location = new System.Drawing.Point(17, 205);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(720, 26);
            this.inputTextBox.TabIndex = 5;
            // 
            // outputLabel
            // 
            this.outputLabel.AutoSize = true;
            this.outputLabel.ForeColor = System.Drawing.Color.Lime;
            this.outputLabel.Location = new System.Drawing.Point(13, 326);
            this.outputLabel.Name = "outputLabel";
            this.outputLabel.Size = new System.Drawing.Size(62, 20);
            this.outputLabel.TabIndex = 6;
            this.outputLabel.Text = "Output:";
            // 
            // inputLabel
            // 
            this.inputLabel.AutoSize = true;
            this.inputLabel.ForeColor = System.Drawing.Color.Lime;
            this.inputLabel.Location = new System.Drawing.Point(13, 170);
            this.inputLabel.Name = "inputLabel";
            this.inputLabel.Size = new System.Drawing.Size(202, 20);
            this.inputLabel.TabIndex = 7;
            this.inputLabel.Text = "Input Website/DNS to ping:";
            // 
            // outputTextBox
            // 
            this.outputTextBox.BackColor = System.Drawing.SystemColors.MenuText;
            this.outputTextBox.Location = new System.Drawing.Point(17, 372);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.Size = new System.Drawing.Size(715, 519);
            this.outputTextBox.TabIndex = 8;
            this.outputTextBox.Text = "";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(17, 251);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(141, 56);
            this.clearButton.TabIndex = 9;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // autoReconnectFailureCountInputTextBox
            // 
            this.autoReconnectFailureCountInputTextBox.Location = new System.Drawing.Point(412, 101);
            this.autoReconnectFailureCountInputTextBox.Name = "autoReconnectFailureCountInputTextBox";
            this.autoReconnectFailureCountInputTextBox.Size = new System.Drawing.Size(44, 26);
            this.autoReconnectFailureCountInputTextBox.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(169, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 20);
            this.label1.TabIndex = 11;
            this.label1.Text = "Auto Reconnect Failure Count:\r\n";
            // 
            // saveSessionButton
            // 
            this.saveSessionButton.Location = new System.Drawing.Point(591, 251);
            this.saveSessionButton.Name = "saveSessionButton";
            this.saveSessionButton.Size = new System.Drawing.Size(141, 56);
            this.saveSessionButton.TabIndex = 12;
            this.saveSessionButton.Text = "Save Session";
            this.saveSessionButton.UseVisualStyleBackColor = true;
            this.saveSessionButton.Click += new System.EventHandler(this.saveSessionButton_Click);
            // 
            // WifiOverwatchMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(749, 901);
            this.Controls.Add(this.saveSessionButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.autoReconnectFailureCountInputTextBox);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.inputLabel);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.inputTextBox);
            this.Controls.Add(this.quickTestCheckBox);
            this.Controls.Add(this.stopPingButton);
            this.Controls.Add(this.autoReconnectCheckbox);
            this.Controls.Add(this.beginPingButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WifiOverwatchMainForm";
            this.Text = "WIFI Overwatch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button beginPingButton;
        private System.Windows.Forms.CheckBox autoReconnectCheckbox;
        private System.Windows.Forms.Button stopPingButton;
        private System.Windows.Forms.CheckBox quickTestCheckBox;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Label inputLabel;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.TextBox autoReconnectFailureCountInputTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button saveSessionButton;
    }
}

