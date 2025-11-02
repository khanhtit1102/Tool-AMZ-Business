namespace Auto_Tool_AMZ_with_GPM
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.cbProxy = new System.Windows.Forms.CheckBox();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.txtGPMAPI = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDelAllOldCard = new System.Windows.Forms.CheckBox();
            this.cbRandomNameCard = new System.Windows.Forms.CheckBox();
            this.cbAddAddress = new System.Windows.Forms.CheckBox();
            this.nudChrome = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddURL = new System.Windows.Forms.Button();
            this.btnAddAcc = new System.Windows.Forms.Button();
            this.btnAddCard = new System.Windows.Forms.Button();
            this.lblAccPass = new System.Windows.Forms.Label();
            this.lblAccDie = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblAccCheck = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.txtAMZAccount = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblCard = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtListCard = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudCard = new System.Windows.Forms.NumericUpDown();
            this.listView1 = new System.Windows.Forms.ListView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtChromeVersion = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtImageDie = new System.Windows.Forms.TextBox();
            this.btnSetImageList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudChrome)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCard)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbProxy
            // 
            this.cbProxy.AutoSize = true;
            this.cbProxy.Location = new System.Drawing.Point(12, 12);
            this.cbProxy.Name = "cbProxy";
            this.cbProxy.Size = new System.Drawing.Size(52, 17);
            this.cbProxy.TabIndex = 0;
            this.cbProxy.Text = "Proxy";
            this.cbProxy.UseVisualStyleBackColor = true;
            this.cbProxy.CheckedChanged += new System.EventHandler(this.txtProxy_CheckedChanged);
            // 
            // txtProxy
            // 
            this.txtProxy.Location = new System.Drawing.Point(101, 10);
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.ReadOnly = true;
            this.txtProxy.Size = new System.Drawing.Size(224, 20);
            this.txtProxy.TabIndex = 1;
            this.txtProxy.TextChanged += new System.EventHandler(this.txtProxy_TextChanged);
            // 
            // txtGPMAPI
            // 
            this.txtGPMAPI.Location = new System.Drawing.Point(101, 36);
            this.txtGPMAPI.Name = "txtGPMAPI";
            this.txtGPMAPI.Size = new System.Drawing.Size(224, 20);
            this.txtGPMAPI.TabIndex = 1;
            this.txtGPMAPI.TextChanged += new System.EventHandler(this.txtProxy_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "GPM API";
            // 
            // cbDelAllOldCard
            // 
            this.cbDelAllOldCard.AutoSize = true;
            this.cbDelAllOldCard.Location = new System.Drawing.Point(15, 114);
            this.cbDelAllOldCard.Name = "cbDelAllOldCard";
            this.cbDelAllOldCard.Size = new System.Drawing.Size(114, 17);
            this.cbDelAllOldCard.TabIndex = 0;
            this.cbDelAllOldCard.Text = "Xoá tất cả card cũ";
            this.cbDelAllOldCard.UseVisualStyleBackColor = true;
            // 
            // cbRandomNameCard
            // 
            this.cbRandomNameCard.AutoSize = true;
            this.cbRandomNameCard.Checked = true;
            this.cbRandomNameCard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRandomNameCard.Location = new System.Drawing.Point(15, 137);
            this.cbRandomNameCard.Name = "cbRandomNameCard";
            this.cbRandomNameCard.Size = new System.Drawing.Size(119, 17);
            this.cbRandomNameCard.TabIndex = 0;
            this.cbRandomNameCard.Text = "Random name card";
            this.cbRandomNameCard.UseVisualStyleBackColor = true;
            // 
            // cbAddAddress
            // 
            this.cbAddAddress.AutoSize = true;
            this.cbAddAddress.Location = new System.Drawing.Point(15, 160);
            this.cbAddAddress.Name = "cbAddAddress";
            this.cbAddAddress.Size = new System.Drawing.Size(88, 17);
            this.cbAddAddress.TabIndex = 0;
            this.cbAddAddress.Text = "Thêm địa chỉ";
            this.cbAddAddress.UseVisualStyleBackColor = true;
            // 
            // nudChrome
            // 
            this.nudChrome.Location = new System.Drawing.Point(206, 114);
            this.nudChrome.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudChrome.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChrome.Name = "nudChrome";
            this.nudChrome.Size = new System.Drawing.Size(65, 20);
            this.nudChrome.TabIndex = 3;
            this.nudChrome.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(277, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Chromes";
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.ForestGreen;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.ForeColor = System.Drawing.Color.White;
            this.btnRun.Location = new System.Drawing.Point(15, 191);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(310, 40);
            this.btnRun.TabIndex = 4;
            this.btnRun.Text = "RUN";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSetImageList);
            this.panel1.Controls.Add(this.btnAddURL);
            this.panel1.Controls.Add(this.btnAddAcc);
            this.panel1.Controls.Add(this.btnAddCard);
            this.panel1.Controls.Add(this.lblAccPass);
            this.panel1.Controls.Add(this.lblAccDie);
            this.panel1.Controls.Add(this.btnLoad);
            this.panel1.Controls.Add(this.txtImageDie);
            this.panel1.Controls.Add(this.lblAccCheck);
            this.panel1.Controls.Add(this.txtURL);
            this.panel1.Controls.Add(this.txtAMZAccount);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.lblAccount);
            this.panel1.Controls.Add(this.lblCard);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtListCard);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Location = new System.Drawing.Point(344, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(444, 221);
            this.panel1.TabIndex = 6;
            // 
            // btnAddURL
            // 
            this.btnAddURL.Location = new System.Drawing.Point(346, 62);
            this.btnAddURL.Name = "btnAddURL";
            this.btnAddURL.Size = new System.Drawing.Size(75, 23);
            this.btnAddURL.TabIndex = 5;
            this.btnAddURL.Text = "Add URL";
            this.btnAddURL.UseVisualStyleBackColor = true;
            this.btnAddURL.Click += new System.EventHandler(this.btnAddURL_Click);
            // 
            // btnAddAcc
            // 
            this.btnAddAcc.Location = new System.Drawing.Point(346, 34);
            this.btnAddAcc.Name = "btnAddAcc";
            this.btnAddAcc.Size = new System.Drawing.Size(75, 23);
            this.btnAddAcc.TabIndex = 5;
            this.btnAddAcc.Text = "Add Acc";
            this.btnAddAcc.UseVisualStyleBackColor = true;
            this.btnAddAcc.Click += new System.EventHandler(this.btnAddAcc_Click);
            // 
            // btnAddCard
            // 
            this.btnAddCard.Location = new System.Drawing.Point(346, 6);
            this.btnAddCard.Name = "btnAddCard";
            this.btnAddCard.Size = new System.Drawing.Size(75, 23);
            this.btnAddCard.TabIndex = 5;
            this.btnAddCard.Text = "Add Card";
            this.btnAddCard.UseVisualStyleBackColor = true;
            this.btnAddCard.Click += new System.EventHandler(this.btnAddCard_Click);
            // 
            // lblAccPass
            // 
            this.lblAccPass.AutoSize = true;
            this.lblAccPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccPass.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblAccPass.Location = new System.Drawing.Point(320, 194);
            this.lblAccPass.Name = "lblAccPass";
            this.lblAccPass.Size = new System.Drawing.Size(101, 20);
            this.lblAccPass.TabIndex = 0;
            this.lblAccPass.Text = "Acc PASS: --";
            // 
            // lblAccDie
            // 
            this.lblAccDie.AutoSize = true;
            this.lblAccDie.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccDie.ForeColor = System.Drawing.Color.Red;
            this.lblAccDie.Location = new System.Drawing.Point(176, 194);
            this.lblAccDie.Name = "lblAccDie";
            this.lblAccDie.Size = new System.Drawing.Size(86, 20);
            this.lblAccDie.TabIndex = 0;
            this.lblAccDie.Text = "Acc DIE: --";
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(331, 127);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(90, 40);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "LOAD";
            this.btnLoad.UseVisualStyleBackColor = false;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblAccCheck
            // 
            this.lblAccCheck.AutoSize = true;
            this.lblAccCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.lblAccCheck.Location = new System.Drawing.Point(11, 194);
            this.lblAccCheck.Name = "lblAccCheck";
            this.lblAccCheck.Size = new System.Drawing.Size(93, 20);
            this.lblAccCheck.TabIndex = 0;
            this.lblAccCheck.Text = "Card Live: --";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(91, 63);
            this.txtURL.Name = "txtURL";
            this.txtURL.Size = new System.Drawing.Size(233, 20);
            this.txtURL.TabIndex = 1;
            this.txtURL.Text = "./_input/addcardURL.txt";
            // 
            // txtAMZAccount
            // 
            this.txtAMZAccount.Location = new System.Drawing.Point(91, 35);
            this.txtAMZAccount.Name = "txtAMZAccount";
            this.txtAMZAccount.Size = new System.Drawing.Size(233, 20);
            this.txtAMZAccount.TabIndex = 1;
            this.txtAMZAccount.Text = "./_input/account.txt";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(11, 153);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(176, 24);
            this.label11.TabIndex = 2;
            this.label11.Text = "Tổng số Account:";
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccount.Location = new System.Drawing.Point(193, 153);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(17, 24);
            this.lblAccount.TabIndex = 2;
            this.lblAccount.Text = "-";
            // 
            // lblCard
            // 
            this.lblCard.AutoSize = true;
            this.lblCard.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCard.Location = new System.Drawing.Point(157, 121);
            this.lblCard.Name = "lblCard";
            this.lblCard.Size = new System.Drawing.Size(17, 24);
            this.lblCard.TabIndex = 2;
            this.lblCard.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 24);
            this.label9.TabIndex = 2;
            this.label9.Text = "Tổng số Card:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Link add";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "AMZ Acc";
            // 
            // txtListCard
            // 
            this.txtListCard.Location = new System.Drawing.Point(91, 8);
            this.txtListCard.Name = "txtListCard";
            this.txtListCard.Size = new System.Drawing.Size(233, 20);
            this.txtListCard.TabIndex = 1;
            this.txtListCard.Text = "./_input/card.txt";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 11);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Card";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(277, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Card";
            // 
            // nudCard
            // 
            this.nudCard.Location = new System.Drawing.Point(206, 139);
            this.nudCard.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudCard.Name = "nudCard";
            this.nudCard.Size = new System.Drawing.Size(65, 20);
            this.nudCard.TabIndex = 3;
            this.nudCard.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(15, 237);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(773, 210);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtChromeVersion
            // 
            this.txtChromeVersion.Location = new System.Drawing.Point(101, 62);
            this.txtChromeVersion.Name = "txtChromeVersion";
            this.txtChromeVersion.Size = new System.Drawing.Size(224, 20);
            this.txtChromeVersion.TabIndex = 1;
            this.txtChromeVersion.TextChanged += new System.EventHandler(this.txtProxy_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Chrome version:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 450);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel1.Text = "     ";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(763, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "Version: 20250928";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 94);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Image DIE";
            // 
            // txtImageDie
            // 
            this.txtImageDie.Location = new System.Drawing.Point(91, 91);
            this.txtImageDie.Name = "txtImageDie";
            this.txtImageDie.Size = new System.Drawing.Size(233, 20);
            this.txtImageDie.TabIndex = 1;
            this.txtImageDie.Text = "./_input/ImageList.txt";
            // 
            // btnSetImageList
            // 
            this.btnSetImageList.Location = new System.Drawing.Point(346, 90);
            this.btnSetImageList.Name = "btnSetImageList";
            this.btnSetImageList.Size = new System.Drawing.Size(75, 23);
            this.btnSetImageList.TabIndex = 5;
            this.btnSetImageList.Text = "Set Image";
            this.btnSetImageList.UseVisualStyleBackColor = true;
            this.btnSetImageList.Click += new System.EventHandler(this.btnSetImageList_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 472);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnRun);
            this.Controls.Add(this.nudCard);
            this.Controls.Add(this.nudChrome);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtChromeVersion);
            this.Controls.Add(this.txtGPMAPI);
            this.Controls.Add(this.txtProxy);
            this.Controls.Add(this.cbAddAddress);
            this.Controls.Add(this.cbRandomNameCard);
            this.Controls.Add(this.cbDelAllOldCard);
            this.Controls.Add(this.cbProxy);
            this.MaximumSize = new System.Drawing.Size(816, 511);
            this.MinimumSize = new System.Drawing.Size(816, 511);
            this.Name = "Form1";
            this.Text = "Tool AMZ Business";
            ((System.ComponentModel.ISupportInitialize)(this.nudChrome)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCard)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbProxy;
        private System.Windows.Forms.TextBox txtProxy;
        private System.Windows.Forms.TextBox txtGPMAPI;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbDelAllOldCard;
        private System.Windows.Forms.CheckBox cbRandomNameCard;
        private System.Windows.Forms.CheckBox cbAddAddress;
        private System.Windows.Forms.NumericUpDown nudChrome;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAccCheck;
        private System.Windows.Forms.Label lblAccPass;
        private System.Windows.Forms.Label lblAccDie;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudCard;
        private System.Windows.Forms.TextBox txtListCard;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.TextBox txtAMZAccount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.Label lblCard;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnAddAcc;
        private System.Windows.Forms.Button btnAddCard;
        private System.Windows.Forms.TextBox txtChromeVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btnAddURL;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSetImageList;
        private System.Windows.Forms.TextBox txtImageDie;
        private System.Windows.Forms.Label label5;
    }
}

