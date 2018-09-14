using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MinecraftOutClient.Modules;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace MinecraftServerTester
{
    public partial class Form1 : Form
    {
        string ip = "";
        int port = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.mcbbs.net/thread-723161-1-1.html");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://afdian.net/@Yoooooo");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] text = Regex.Split(textBox1.Text, ":", RegexOptions.IgnoreCase);
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("IP地址为空！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                goto End;
            }
            //当IP地址有大于1个端口时
            else if (text.Length > 2)
            {
                MessageBox.Show("IP地址无效，请仔细检查！","ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                goto End;
            }
            //当IP地址端口数量等于1时
            else if(text.Length == 2)
            {
                try
                {
                    ip = text[0];
                    port = Convert.ToInt32(text[1]);
                }
                //若端口无法转型为Int类型
                catch
                {
                    MessageBox.Show("IP地址无效，请仔细检查！", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    goto End;
                }
            //当IP地址没有写端口时 默认为25565
            }else if (text.Length == 1)
            {
                ip = text[0];
                port = 25565;
            }
            label6.Text = "Connecting...";
            getServer();
        End:;
        }

        private void getServer()
        {
            try
            {
                ServerInfo info = new ServerInfo(ip, port);
                info.StartGetServerInfo();
                label4.Text = "Motd: " + info.MOTD;
                label5.Text = "玩家数量：" + info.CurrentPlayerCount + " / " + info.MaxPlayerCount;
                label7.Text = "服务器版本：" + info.GameVersion;
                label8.Text = "Protocol版本：" + info.ProtocolVersion;
                label6.Text = "Ping: "+info.Ping;
                if (info.Icon != null)
                {
                    pictureBox1.Image.Dispose();
                    info.Icon.Save("ServerIcon.png");
                    pictureBox1.Image = Image.FromFile(Environment.CurrentDirectory + @"/ServerIcon.png");
                }
                else
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = Properties.Resources.DefaultIcon;
                }
                textBox2.Text = "";
                if (info.ForgeInfo != null && info.ForgeInfo.Mods.Any())
                {
                    foreach (var item in info.ForgeInfo.Mods)
                    {
                        textBox2.Text = textBox2.Text + item + "\r\n";
                    }
                }
                else
                {
                    textBox2.Text = "该服务器没有MOD！";
                }
                textBox3.Text = "";
                if (info.OnlinePlayersName != null && info.OnlinePlayersName.Any())
                {
                    foreach (var item in info.OnlinePlayersName)
                    {
                        textBox3.Text = textBox3.Text + item + "\r\n";
                    }
                }
                else
                {
                    textBox3.Text = "服务器里空荡荡的！";
                }

            }
            catch (SocketException ex)
            {
                MessageBox.Show("连接失败！\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label6.Text = "Failed to connect.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现异常！\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label6.Text = "Failed to connect.";
            }
            finally
            {
                
            }

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
        }
    }
}
