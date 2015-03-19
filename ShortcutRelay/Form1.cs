using ShortcutRelayService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;

namespace ShortcutRelay
{
    public partial class Form1 : Form
    {
        private int tableRowCount;
        private bool shift;
        private bool control;
        private bool menu;
        private string inputStringMemory;
        private RelayServiceWrapper serviceWrapper;


        public Form1()
        {
            serviceWrapper = new RelayServiceWrapper();
            InitializeComponent();
            tableRowCount = 0;
            shift = false;
            control = false;
            menu = false;
            inputStringMemory = "";
            
            //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            //smb.HttpGetEnabled = true;
            //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            //host.Description.Behaviors.Add(smb);
            //host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serviceWrapper.startService();
            label1.Text = serviceWrapper.host.State.ToString();
            label2.Text = serviceWrapper.LocalIPAddress();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serviceWrapper.stopService();
            label1.Text = serviceWrapper.host.State.ToString();
            label2.Text = "Service is stopped";
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            textBox1.Text = "";
            inputStringMemory = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length > 0 && textBox2.Text.Length > 0)
            {
                serviceWrapper.addShortcutToSerivce(textBox1.Text, textBox2.Text);
                loadShortcutList();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (inputStringMemory.Length > 0)
                inputStringMemory += " + ";
            if (e.KeyCode == Keys.ControlKey)
            {
                control = true;
            }
            else if (e.KeyCode == Keys.Menu)
            {
                menu = true;
            }
            else if (e.KeyCode == Keys.ShiftKey)
            {
                shift = true;
            }
            else
            {
                inputStringMemory += e.KeyCode.ToString().ToUpper();
                if (inputStringMemory.Length > 0)
                {
                    if (control)
                        textBox1.Text += "CONTROL + ";
                    if (shift)
                        textBox1.Text += "SHIFT + ";
                    if (menu)
                        textBox1.Text += "MENU + ";
                    textBox1.Text += inputStringMemory;
                    shift = false;
                    control = false;
                    menu = false;
                }
            }
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = "";
            inputStringMemory = "";
            shift = false;
            control = false;
            menu = false;
        }
        
        private void deleteShortcutEvent(object sender, EventArgs e)
        {
            int row = tableLayoutPanel1.GetPositionFromControl((Control)sender).Row;
            Label temp = (Label)tableLayoutPanel1.GetControlFromPosition(0, row);
            serviceWrapper.DeleteShortcut(temp.Text);
            loadShortcutList();
        }

        private void shortcutLabelClick(object sender, EventArgs e)
        {
            String shortString = ((Label)sender).Text;
            serviceWrapper.ActivateShortcut(shortString);
        }

        private void loadShortcutList()
        {
            serviceWrapper.loadShortcutList();
            recreateTablePanel();

            tableRowCount = 0;
            if (serviceWrapper.shortcutList != null)
            {
                foreach (ShortcutData shortcut in serviceWrapper.shortcutList)
                {
                    Button tempButton = new Button() { Text = "Delete", Anchor = AnchorStyles.Left, Dock = DockStyle.Fill, AutoSize = true };
                    tempButton.Click += new System.EventHandler(this.deleteShortcutEvent);
                    Label tempName = new Label() { Text = shortcut.name, Anchor = AnchorStyles.Left, AutoSize = true };
                    Label tempShortcut = new Label() { Text = shortcut.shortcut, Anchor = AnchorStyles.Left, AutoSize = true };
                    tempShortcut.Click += new System.EventHandler(this.shortcutLabelClick);
                    tableLayoutPanel1.Controls.Add(tempShortcut, 0, tableRowCount);
                    tableLayoutPanel1.Controls.Add(tempName, 1, tableRowCount);
                    tableLayoutPanel1.Controls.Add(tempButton, 2, tableRowCount);
                    tableRowCount++;
                }
                this.Controls.Add(this.tableLayoutPanel1);

            }
        }

        private void recreateTablePanel()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                control.Dispose();
            }
            this.tableLayoutPanel1.Dispose();
            tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 201);
            this.tableLayoutPanel1.MaximumSize = new System.Drawing.Size(500, 500);
            this.tableLayoutPanel1.MinimumSize = new System.Drawing.Size(500, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(500, 36);
            this.tableLayoutPanel1.TabIndex = 4;
        }
    }
}


