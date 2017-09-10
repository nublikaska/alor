using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        List<string> lst;

        public Form1()
        {
            InitializeComponent();
            lst = new List<string>();
        }

        private void SetComboBoxs()
        {
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
            //comboBox1.Text = comboBox2.Text = "00:00";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetComboBoxs();
            dataGridView1.Columns.Add("date", "date");
            dataGridView1.Columns.Add("high", "high");
            dataGridView1.Columns.Add("low", "low");
            dataGridView1.Columns.Add("close", "close");
            dataGridView1.Columns.Add("max", "max");
            dataGridView1.Columns.Add("max-close", "max-close");
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            download();
            float[] inf = new float[2];
            inf = Getscore();
            dataGridView1["date", dataGridView1.Rows.Count-1].Value = dateTimePicker1.Text + " " + comboBox1.Text + " - " + dateTimePicker2.Text + " " + comboBox2.Text;
            dataGridView1["close", dataGridView1.Rows.Count-1].Value = inf[0];
            dataGridView1["max", dataGridView1.Rows.Count-1].Value = inf[1];
        }

        private void download()
        {
            string from = "&from=" + dateTimePicker1.Text;
            DateTime dateToTemp = dateTimePicker2.Value.AddDays(1);
            string to = "&to=" + dateToTemp.ToShortDateString();
            string connection = "http://history.alor.ru/export/file.php?board=MICEX&ticker=sber&period=1" + from + to + "&file_name=&formatFiles=1&format=5&formatDate=3&formatTime=3&fieldSeparator=4&formatSeparatorDischarge=2&but.x=124&but.y=24&but=%D1%EA%E0%F7%E0%F2%FC+%F4%E0%E9%EB";
            WebClient wb = new WebClient();
            wb.DownloadFile(connection, "statistic.txt");
            WriteToList();
        }
        private void WriteToList()
        {
            lst.Clear();
            FileStream file1 = new FileStream("statistic.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file1);
            while (!reader.EndOfStream)
                lst.Add(reader.ReadLine());
            reader.Close();
        }
        private string GetItemForNumberStr(int numberStr, int numberItem)
        {
            String[] words = lst[numberStr].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words[numberItem];
        }
        private int GetCountItemForTime(string date, string time)
        {
            int count = 0;
            time = time.Replace(":", "");
            date = date.Replace(".", "");
            date = date.Replace("2017", "");
            date = date + "17";
            while (lst.Count != count)
            {                
                string time2 = GetItemForNumberStr(count, 1);
                string date2 = GetItemForNumberStr(count, 0);

                if ((time == time2) && (date == date2))
                {
                    MessageBox.Show("есть время и дата");
                    return count;
                }
                count++;
            }
            return -1;
        }
        private float[] Getscore()
        {
            int from = GetCountItemForTime(dateTimePicker1.Text, comboBox1.Text);
            int to = GetCountItemForTime(dateTimePicker2.Text, comboBox2.Text);
            float[] inf = new float[2]; //close max
            string time = "";
            float temp;

            if ((from != -1) & (to != -1))
                for (int numberItem = from; numberItem < to; numberItem++)
                {
                    Single.TryParse(GetItemForNumberStr(numberItem, 3), out temp);
                    if (temp > inf[1])
                    {
                        inf[1] = temp;
                        time = GetItemForNumberStr(numberItem, 0) + " " + GetItemForNumberStr(numberItem, 1);
                    }
                }
            else
                MessageBox.Show("дата и время не найдены");


            Single.TryParse(GetItemForNumberStr(lst.Count-1, 6), out temp);
            inf[0] = temp;

            return inf;
        }
    }
}
