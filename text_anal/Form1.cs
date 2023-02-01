using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace text_anal
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
        }
        class StringMax : IComparer<string>
        {
            public int Compare(string o1, string o2)
            {
                if (o1.Length > o2.Length)
                    return -1;
                else if (o1.Length < o2.Length)
                    return 1;
                return 0;
            }
        }
        class StringMin : IComparer<string>
        {
            public int Compare(string o1, string o2)
            {
                if (o1.Length > o2.Length)
                    return 1;
                else if (o1.Length < o2.Length)
                    return -1;
                return 0;
            }
        }
        private void buttonAnalysis_Click(object sender, EventArgs e)
        {
            string text = textBox.Text;
            // при помощи регулярного выражения находим все слова
            Regex rg = new Regex("[а-яa-z]+", RegexOptions.IgnoreCase);
            MatchCollection matches = rg.Matches(text);
            List<string> str = new List<string>();
            Dictionary<string, int> stat = new Dictionary<string, int>();
            foreach (Match m in matches)
            {
                str.Add(m.ToString().ToLower());
                if (stat.ContainsKey(m.ToString().ToLower()))
                    stat[m.ToString().ToLower()]++;
                else
                    stat.Add(m.ToString().ToLower(), 1);
            }
            label1.Text = "Количество слов = " + matches.Count.ToString();
            StringMax sort1 = new StringMax();
            str.Sort(sort1);
            label2.Text = "Топ 10 слов:";
            int i = 0;
            foreach (string s in str)
            {
                label2.Text += ("\n" + (i+1) + ") " + s);
                i++;
                if (i > 9)
                    break;
            }
            StringMin sort2 = new StringMin();
            str.Sort(sort2);
            label3.Text = "Анти топ 10 слов:";
            i = 0;
            foreach (string s in str)
            {
                label3.Text += ("\n" + (i + 1) + ") " + s);
                i++;
                if (i > 9)
                    break;
            }
            textBox1.Text = "";
            foreach (var word in stat)
                textBox1.Text += word.Key + " - " + ((float)word.Value / (float)matches.Count * 100).ToString() + "%\r\n";          
            List<string> topFreqWords = new List<string>();
            string max_word = null;
            i = 10;
            if (stat.Count < 10)
                i = stat.Count;
            for (; i > 0; i--)
            {
                int max = 0;
                foreach (var word in stat)
                {
                    if (word.Value > max)
                    {
                        max = word.Value;
                        max_word = word.Key;
                    }
                }
                topFreqWords.Add(max_word);
                stat.Remove(max_word);
            }
            label4.Text = "Топ 10 частых слов";
            foreach (string word in topFreqWords)
                label4.Text += "\n" + word;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = openFileDialog1.FileName;
            // читаем файл в строку
            string fileText = System.IO.File.ReadAllText(filename);
            textBox.Text = fileText;
            MessageBox.Show("Файл открыт");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // получаем выбранный файл
            string filename = saveFileDialog1.FileName;
            // сохраняем текст в файл
            string saveText = label2.Text + "\n" + label3.Text + "\n" + label4.Text + "\n" + label5.Text + "\n" + textBox1.Text;
            System.IO.File.WriteAllText(filename, saveText);
            MessageBox.Show("Файл сохранен");
        }
    }
}
