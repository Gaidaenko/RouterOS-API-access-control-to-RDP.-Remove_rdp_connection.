
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Presentation;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tik4net;
using tik4net.Objects;
using tik4net.Objects.Interface;
using tik4net.Objects.Interface.Bridge;
using tik4net.Objects.Ip.Firewall;
using tik4net.Objects.Ip.Hotspot;
using tik4net.Objects.System;

namespace DisableRemoteAccess
{
    public partial class Form1 : Form 
    {
        public Form1()
        {
            InitializeComponent();
        }
       public string HOST = "name.server.com";
       public string USER = "apiuser";
       public string PASS = "password";
       public string nameRule = "Demo";
       public string dst = "IpAddress:Port";

        private void button1_Click(object sender, EventArgs e)    
        {
            try
            {
                using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                {
                    connection.Open(HOST, USER, PASS);

                    var natRule = connection.CreateCommandAndParameters("/ip/firewall/nat/print", "comment", nameRule).ExecuteList();  
                    var id = natRule.Single().GetId();  
                    var disableRule = connection.CreateCommandAndParameters("/ip/firewall/nat/disable", TikSpecialProperties.Id, id); 
                    disableRule.ExecuteNonQuery();  

                    var list = connection.CreateCommandAndParameters("/ip/firewall/connection/print", "dst-address", dst).ExecuteList();  
                    foreach (var item in list)
                    {
                        connection.CreateCommandAndParameters("/ip/firewall/connection/remove", ".id", item.Words[".id"]).ExecuteNonQuery(); 
                    }
                };
            }
            catch
            {
                Color colorOff_n = Color.Red;
                label1.ForeColor = colorOff_n;
                label1.Text = ("Возникло исключение!\n Имя сервера или порт \nне доступты!");
            }               
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                using (ITikConnection connection = ConnectionFactory.CreateConnection(TikConnectionType.Api))
                {
                    connection.Open(HOST, USER, PASS);

                    var natRule = connection.CreateCommandAndParameters("/ip/firewall/nat/print", "comment", nameRule).ExecuteList();
                    var id = natRule.Single().GetId();
                    var disableRule = connection.CreateCommandAndParameters("/ip/firewall/nat/enable", TikSpecialProperties.Id, id);

                    disableRule.ExecuteNonQuery();
                }
            }
            catch
            {
                Color colorOff_n = Color.Red;
                label1.ForeColor = colorOff_n;
                label1.Text = ("Возникло исключение!\n Имя сервера или порт\n не доступты!");
            }                                   
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
