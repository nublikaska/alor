using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        List<string> lst;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //string connectionString = "http://history.alor.ru/export/file.php?board=MICEX&ticker=sber&period=1" + from + to + "&file_name=&formatFiles=1&format=5&formatDate=3&formatTime=3&fieldSeparator=4&formatSeparatorDischarge=2&but.x=124&but.y=24&but=%D1%EA%E0%F7%E0%F2%FC+%F4%E0%E9%EB";

            FileStream file1 = new FileStream("C:/Users/Андрей/Downloads/SBER_07092017_07092017.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            lst = new List<string>();
            string hour = "";
            string minutes = "";
            for (int i = 0; i < 24; i++)
                for (int j = 0; j < 60; j++)
                {
                    hour = i.ToString();
                    minutes = j.ToString();
                    if (i < 10)
                        hour = "0" + i.ToString();
                    if (j < 10)
                        minutes = "0" + j.ToString();
                    comboBox1.Items.Add(hour + ":" + minutes);
                    comboBox2.Items.Add(hour + ":" + minutes);
                }
            comboBox1.Text = comboBox2.Text = "00:00";
            while (!reader.EndOfStream)
            {
                lst.Add(reader.ReadLine());
            }

        }

        public string Item(int numberStr, int numberItem)
        {            
            String[] words = lst[numberStr].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string it = words[numberItem];
            return it;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(monthCalendar1.SelectionStart.ToString().Remove(10));
            //string from = "&from=" + monthCalendar1.SelectionStart.ToString();
            //string to = "&to=07.09.2017";
            //string connectionString = "http://history.alor.ru/export/file.php?board=MICEX&ticker=sber&period=1" + from + to + "&file_name=&formatFiles=1&format=5&formatDate=3&formatTime=3&fieldSeparator=4&formatSeparatorDischarge=2&but.x=124&but.y=24&but=%D1%EA%E0%F7%E0%F2%FC+%F4%E0%E9%EB";

        }
    }
}
