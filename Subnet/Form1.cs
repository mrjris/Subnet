using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subnet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //Nếu gõ enter thì làm việc nếu không thì thôi
            if (e.KeyCode != Keys.Enter) return;
            //Lấy và tách dữ liệu
            string s = txtIP.Text;
            string[] dim = { ",", ".", " ", "\t", "\r\t", "/" };
            string[] words = s.Split(dim, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length != 5)
            {
                MessageBox.Show("Mời nhập đúng dạng Ví dụ: 127.16.1.129/25");
                return;
            }
            //Thuật toán
            //B1: Biến đổi IP về dạng binary 
            string IP = "";
            for (int i = 0; i< 4; i++)
                IP += Convert.ToString(Convert.ToInt32(words[i]),2).PadLeft(8,'0');
            //B2: Tìm subnetmask
            int count = Convert.ToInt32(words[4]);
            string subnet = "";
            subnet = subnet.PadLeft(count, '1') + subnet.PadRight(32 - count, '0');
            //B3: Dùng thuật toán AND giữa địa chỉ IP và subnetmask để ra lớp mạng.
            string result = "";
            for (int i = 0; i < 32; i++)
                if (subnet[i] == '1' && IP[i] == '1') result += "1";
                else result += "0";

            result = ConvertBinaryToIp(result) + "/" + count.ToString();
           
            txtResult.Text = result;


        }
        static string ConvertBinaryToIp(string str32BitBinaryIP)

        {
            //Copy code trên stacoverflow

            string[] strEach8BitBinaryIp = new string[4];
            var split = str32BitBinaryIP.Select((c, index) => new { c, index })
                .GroupBy(x => x.index / 8)
                .Select(group => group.Select(elem => elem.c))
                .Select(chars => new string(chars.ToArray()));
            int n = 0;
            foreach (var str in split)
                strEach8BitBinaryIp[n++] = str.ToString();

            int decimalValueOf8bitIp = 0;

            string ipAddress = "";

            for (int i = 0; i < strEach8BitBinaryIp.Length; i++)
            {
                decimalValueOf8bitIp = Convert.ToInt32(strEach8BitBinaryIp[i]);
                ipAddress += Convert.ToInt32(decimalValueOf8bitIp.ToString(),2).ToString() + ".";
            }//end of for


            return ipAddress.Substring(0, ipAddress.Length - 1);


        }// end of ConvertBinaryToIp() method
    }
}
