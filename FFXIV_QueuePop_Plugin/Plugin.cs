using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Xml;
using FFXIV_QueuePop_Plugin.Notifier;
using FFXIV_QueuePop_Plugin.Logger;
using Q42.HueApi.Interfaces;
using Q42.HueApi;
using Q42.HueApi.Models.Bridge;
using System.Linq;
using System.Drawing;
using System.Threading;
using Q42.HueApi.Converters;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.Original;
using FFXIV_QueuePop_Plugin.Hue;

namespace FFXIV_QueuePop_Plugin
{
    public class Plugin : UserControl, IActPluginV1
    {

        #region Designer Created Code (Avoid editing)
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gbSettings = new System.Windows.Forms.GroupBox();
            this.txtGetURL = new System.Windows.Forms.TextBox();
            this.lblTextGetUrl = new System.Windows.Forms.Label();
            this.lbTextMode = new System.Windows.Forms.Label();
            this.cbMode = new System.Windows.Forms.ComboBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTextKey = new System.Windows.Forms.Label();
            this.txtTelegramChatId = new System.Windows.Forms.TextBox();
            this.btnRegisterHue = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbLightSelector = new System.Windows.Forms.ComboBox();
            this.lblHueStatus = new System.Windows.Forms.Label();
            this.txtHueInfo = new System.Windows.Forms.TextBox();
            this.btnSecondTest = new System.Windows.Forms.Button();
            this.gbSettings.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbSettings
            // 
            this.gbSettings.Controls.Add(this.btnSecondTest);
            this.gbSettings.Controls.Add(this.txtGetURL);
            this.gbSettings.Controls.Add(this.lblTextGetUrl);
            this.gbSettings.Controls.Add(this.lbTextMode);
            this.gbSettings.Controls.Add(this.cbMode);
            this.gbSettings.Controls.Add(this.btnTest);
            this.gbSettings.Controls.Add(this.txtApiKey);
            this.gbSettings.Controls.Add(this.btnSave);
            this.gbSettings.Controls.Add(this.label2);
            this.gbSettings.Controls.Add(this.lbTextKey);
            this.gbSettings.Controls.Add(this.txtTelegramChatId);
            this.gbSettings.Location = new System.Drawing.Point(3, 3);
            this.gbSettings.Name = "gbSettings";
            this.gbSettings.Size = new System.Drawing.Size(311, 192);
            this.gbSettings.TabIndex = 2;
            this.gbSettings.TabStop = false;
            this.gbSettings.Text = "Settings";
            // 
            // txtGetURL
            // 
            this.txtGetURL.Enabled = false;
            this.txtGetURL.Location = new System.Drawing.Point(107, 100);
            this.txtGetURL.Name = "txtGetURL";
            this.txtGetURL.Size = new System.Drawing.Size(195, 20);
            this.txtGetURL.TabIndex = 8;
            this.txtGetURL.Visible = false;
            // 
            // lblTextGetUrl
            // 
            this.lblTextGetUrl.AutoSize = true;
            this.lblTextGetUrl.Location = new System.Drawing.Point(9, 100);
            this.lblTextGetUrl.Name = "lblTextGetUrl";
            this.lblTextGetUrl.Size = new System.Drawing.Size(52, 13);
            this.lblTextGetUrl.TabIndex = 7;
            this.lblTextGetUrl.Text = "Get URL:";
            this.lblTextGetUrl.Visible = false;
            // 
            // lbTextMode
            // 
            this.lbTextMode.AutoSize = true;
            this.lbTextMode.Location = new System.Drawing.Point(6, 19);
            this.lbTextMode.Name = "lbTextMode";
            this.lbTextMode.Size = new System.Drawing.Size(37, 13);
            this.lbTextMode.TabIndex = 6;
            this.lbTextMode.Text = "Mode:";
            // 
            // cbMode
            // 
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Items.AddRange(new object[] {
            "Telegram"});
            this.cbMode.Location = new System.Drawing.Point(107, 19);
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size(195, 21);
            this.cbMode.TabIndex = 4;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(224, 157);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(78, 23);
            this.btnTest.TabIndex = 5;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // txtApiKey
            // 
            this.txtApiKey.Location = new System.Drawing.Point(107, 46);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(195, 20);
            this.txtApiKey.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(224, 128);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Telegram Chat Id:";
            // 
            // lbTextKey
            // 
            this.lbTextKey.AutoSize = true;
            this.lbTextKey.Location = new System.Drawing.Point(6, 46);
            this.lbTextKey.Name = "lbTextKey";
            this.lbTextKey.Size = new System.Drawing.Size(48, 13);
            this.lbTextKey.TabIndex = 0;
            this.lbTextKey.Text = "API Key:";
            // 
            // txtTelegramChatId
            // 
            this.txtTelegramChatId.Location = new System.Drawing.Point(107, 72);
            this.txtTelegramChatId.Name = "txtTelegramChatId";
            this.txtTelegramChatId.Size = new System.Drawing.Size(195, 20);
            this.txtTelegramChatId.TabIndex = 3;
            // 
            // btnRegisterHue
            // 
            this.btnRegisterHue.Location = new System.Drawing.Point(6, 163);
            this.btnRegisterHue.Name = "btnRegisterHue";
            this.btnRegisterHue.Size = new System.Drawing.Size(78, 23);
            this.btnRegisterHue.TabIndex = 9;
            this.btnRegisterHue.Text = "Register";
            this.btnRegisterHue.UseVisualStyleBackColor = true;
            this.btnRegisterHue.Click += new System.EventHandler(this.btnRegisterHue_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbLightSelector);
            this.groupBox1.Controls.Add(this.lblHueStatus);
            this.groupBox1.Controls.Add(this.txtHueInfo);
            this.groupBox1.Controls.Add(this.btnRegisterHue);
            this.groupBox1.Location = new System.Drawing.Point(320, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 192);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Philips Hue";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Light";
            // 
            // cmbLightSelector
            // 
            this.cmbLightSelector.FormattingEnabled = true;
            this.cmbLightSelector.Location = new System.Drawing.Point(42, 24);
            this.cmbLightSelector.Name = "cmbLightSelector";
            this.cmbLightSelector.Size = new System.Drawing.Size(121, 21);
            this.cmbLightSelector.TabIndex = 13;
            this.cmbLightSelector.SelectedIndexChanged += new System.EventHandler(this.cmbLightSelector_SelectedIndexChanged);
            // 
            // lblHueStatus
            // 
            this.lblHueStatus.AutoSize = true;
            this.lblHueStatus.Location = new System.Drawing.Point(99, 137);
            this.lblHueStatus.Name = "lblHueStatus";
            this.lblHueStatus.Size = new System.Drawing.Size(54, 13);
            this.lblHueStatus.TabIndex = 12;
            this.lblHueStatus.Text = "Not found";
            // 
            // txtHueInfo
            // 
            this.txtHueInfo.Location = new System.Drawing.Point(99, 166);
            this.txtHueInfo.Name = "txtHueInfo";
            this.txtHueInfo.Size = new System.Drawing.Size(244, 20);
            this.txtHueInfo.TabIndex = 11;
            // 
            // btnSecondTest
            // 
            this.btnSecondTest.Location = new System.Drawing.Point(140, 157);
            this.btnSecondTest.Name = "btnSecondTest";
            this.btnSecondTest.Size = new System.Drawing.Size(78, 23);
            this.btnSecondTest.TabIndex = 9;
            this.btnSecondTest.Text = "Test 2";
            this.btnSecondTest.UseVisualStyleBackColor = true;
            this.btnSecondTest.Click += new System.EventHandler(this.btnSecondTest_Click);
            // 
            // Plugin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbSettings);
            this.Name = "Plugin";
            this.Size = new System.Drawing.Size(686, 384);
            this.gbSettings.ResumeLayout(false);
            this.gbSettings.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion
        public Plugin()
        {
            InitializeComponent();
        }

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\FFXIV_QueuePop_Plugin.config.xml");
        private GroupBox gbSettings;
        SettingsSerializer xmlSettings;
        private Button btnSave;
        private TextBox txtTelegramChatId;
        private TextBox txtApiKey;
        private Label label2;
        private Label lbTextKey;
        private Button btnTest;
        private Label lbTextMode;
        private ComboBox cbMode;
        private TextBox txtGetURL;
        private Label lblTextGetUrl;
        private Button btnRegisterHue;
        private GroupBox groupBox1;
        private TextBox txtHueInfo;
        QueueWatcher notifier;
        private Label lblHueStatus;
        private ComboBox cmbLightSelector;
        private Button btnSecondTest;
        private Label label1;

        #region IActPluginV1 Members
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            lblStatus = pluginStatusText;  
            pluginScreenSpace.Controls.Add(this);   
            this.Dock = DockStyle.Fill; 
            xmlSettings = new SettingsSerializer(this);
            LoadSettings();          

            lblStatus.Text = "Plugin Started";

            Log.Write(LogType.Info, "Plugin Started");

            notifier = new QueueWatcher();

            notifier.Start();

            if (!String.IsNullOrEmpty(txtHueInfo.Text))
            {
                InitHue(txtHueInfo.Text);
            }
        }

        public void DeInitPlugin()
        {
            //Stopping the QueuePopNotifier thread
            if (notifier != null)
            {
                notifier.Stop();
            }
            

            SaveSettings();
            lblStatus.Text = "Plugin Exited";
        }
        #endregion

      
        void LoadSettings()
        {
            // Add any controls you want to save the state of
            xmlSettings.AddControlSetting(txtApiKey.Name, txtApiKey);
            xmlSettings.AddControlSetting(txtTelegramChatId.Name, txtTelegramChatId);
            xmlSettings.AddControlSetting(cbMode.Name, cbMode);
            xmlSettings.AddControlSetting(txtGetURL.Name, txtGetURL);
            xmlSettings.AddControlSetting(txtHueInfo.Name, txtHueInfo);
            xmlSettings.AddControlSetting(cmbLightSelector.Name, cmbLightSelector);

            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            if (xReader.LocalName == "SettingsSerializer")
                            {
                                xmlSettings.ImportFromXml(xReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error loading settings: " + ex.Message;
                }
                xReader.Close();
            }
        }
        void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 1;
            xWriter.IndentChar = '\t';
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");
            xWriter.WriteStartElement("SettingsSerializer");    
            xmlSettings.ExportToXml(xWriter);  
            xWriter.WriteEndElement(); 
            xWriter.WriteEndElement();  
            xWriter.WriteEndDocument(); 
            xWriter.Flush();    
            xWriter.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private async void InitHue(string appkey)
        {
            //Gaat er nu vanuit dat textbox gevuld is
            bool bridgeInitialized = await Qhue.Instance.InitBridgeAsync(appkey);

            if (bridgeInitialized)
            {
                lblHueStatus.Text = "Bridge found";
            }

            Dictionary<string, string> dict = await Qhue.Instance.getLightsAsync();

            cmbLightSelector.ValueMember = "Key";
            cmbLightSelector.DisplayMember = "Value";
            cmbLightSelector.DataSource = dict.ToList();
            Qhue.Instance.prefferedLight = cmbLightSelector.SelectedValue.ToString();
            LoadSettings();
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            _ = NotificationSender.SendNotification();

            Qhue.Instance.BlinkLightRed(cmbLightSelector.SelectedValue.ToString());
        }

        private async void btnRegisterHue_Click(object sender, EventArgs e)
        {
            string appKey = await Qhue.Instance.RegisterBridgeAsync();
            if (appKey != null)
            {
                txtHueInfo.Text = "Register succesfull";
                InitHue(appKey);
            }
            else
            {
                lblHueStatus.Text = "Bridge registration failed. Make sure it is connected to the network";
            }
         
        }

        private void cmbLightSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Qhue.Instance.prefferedLight = cmbLightSelector.SelectedValue.ToString();
        }

        private void btnSecondTest_Click(object sender, EventArgs e)
        {
            Qhue.Instance.CancelBlink();
        }
    }
}
