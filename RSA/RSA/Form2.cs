using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace RSA
{
    public partial class Form2 : Form
    {
        public RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(Form1.x);
        public static string publicKeyString;
        public static string privateKeyString;

        public Form2()
        {
            InitializeComponent();
        }

        public static string GetKeyString(RSAParameters publicKey)
        {

            var stringWriter = new System.IO.StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }

        public static string Encrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsaa = new RSACryptoServiceProvider(Form1.x))
            {
                try
                {
                    rsaa.FromXmlString(publicKeyString.ToString());
                    var encryptedData = rsaa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsaa.PersistKeyInCsp = false;
                }
            }
        }

        private void materialDivider1_Click(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            string textToEncrypt = textBox1.Text;
            string encryptedText = Encrypt(textToEncrypt, publicKeyString);
            MessageBox.Show("encrypted:   " + encryptedText);
            StreamWriter sw = new StreamWriter("D:\\Test2.txt");
            sw.WriteLine("encrypted:   " + encryptedText);
            sw.WriteLine("key:   " + textBox2.Text);
            sw.Close();
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            var privateKey = RSA.ExportParameters(true);
            var publicKey = RSA.ExportParameters(false);
            publicKeyString = GetKeyString(publicKey);
            textBox2.Text = publicKeyString;
            privateKeyString = GetKeyString(privateKey);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frm1 = new Form1();
            frm1.Show();
        }
    }
}
