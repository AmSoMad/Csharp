using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using MetroFramework;

namespace WindowsFormsApp2
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        SerialPort serialPort;
        private StringBuilder qrCodeBuff;
        private bool IsReadQRCode = false;
        delegate void SetTextCallback(string Text);

        public Form1()
        {
            qrCodeBuff = new StringBuilder();
            InitializeComponent();
            Load += new EventHandler(comboBox1_Load);
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void comboBox1_Load(object sender, EventArgs e)
        {
            foreach (string comport in SerialPort.GetPortNames())
            {
                metroComboBox1.Items.Add(comport);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(metroComboBox1.SelectedIndex == -1)
            {
                MetroMessageBox.Show(this,"포트선택이 안되었습니다.","오류");
                return;
            }
            serialPort1.PortName = metroComboBox1.SelectedItem.ToString();
            serialPort1.Open();
            
            if (serialPort1.IsOpen)
            {
                connectYn.Text = serialPort1.PortName + " 연결완료";
                //txtQRCode.AppendText(serialPort1.PortName + " Open");
            }
            else
            {
                connectYn.Text = "연결실패";
                //txtQRCode.AppendText("fail to open");
            }
            connectYn.Visible = true;
            //serialPort1.ReadLine();
            //ReadByte, ReadChar, ReadExisting, ReadLine 중 필요한 메소드를 골라서 사용, SerialPort가 데이터를 받으면 serialPort1_DataReceved 같은 event handler 가 호출된다.
            this.ActiveControl = txtQRCode;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SerialReceived(object sender, EventArgs e)
        {
            this.ActiveControl = txtQRCode;
            qrCodeBuff.Clear();
            this.txtQRCode.Clear();
            try
            {
                
                /*
                if(txtQRCode.ToString() == "")
                {
                    DialogResult dr =  MetroMessageBox.Show(this, "이미 입력한 QR코드가 존재합니다. 변경하시겠습니까?", "확인", MessageBoxButtons.OKCancel);
                    if(dr != DialogResult.OK)
                    {
                        return;
                    }
                    
                }
                */
                int byteCount = serialPort1.BytesToRead;
                byte[] buffer = new byte[byteCount];
                int count = serialPort1.Read(buffer, 0, byteCount);
                //String strRecive = serialPort1.ReadExisting();

                string strRecive = Encoding.UTF8.GetString(buffer);
                this.txtQRCode.AppendText(strRecive);
                qrCodeBuff.Append(strRecive);
                string qrCodeBuffToString = qrCodeBuff.ToString();
                string qrText = qrCodeBuffToString.Replace("\r", "").Replace("\n", "").Replace("\0", "").Replace("/00000", ""); ;
                this.txtQRCode.Text = qrText;

                string delimiter10 = "\u0002\u00aa";
                if (qrCodeBuffToString.EndsWith("\r") == true || qrCodeBuffToString.EndsWith("\r\n") || qrCodeBuffToString.EndsWith(delimiter10) == true)
                {
                    MetroMessageBox.Show(this,qrText, "출력내용");
                }
            }
            catch (Exception ex)
            {
                this.qrCodeBuff.Clear();
                this.txtQRCode.Text = "";
                this.IsReadQRCode = false;
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(SerialReceived));
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Load -= new EventHandler(comboBox1_Load);
            this.serialPort1.DataReceived -= new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }

        private void Form1_Activate(object sender, EventArgs e)
        {
            Load += new EventHandler(comboBox1_Load);
            this.serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }
    }
}
