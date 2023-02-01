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
using System.Runtime.InteropServices;

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
        // правило сортировки для строк по их длине (по убыванию)
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
        private void buttonAnalysis_Click(object sender, EventArgs e)
        {
            string text = textBox.Text;
            // при помощи регулярного выражения находим все слова
            Regex rg = new Regex("[а-яa-z]+", RegexOptions.IgnoreCase);
            MatchCollection matches = rg.Matches(text);
            List<string> allWords = new List<string>();
            // заполняем словарь: ключ - уникльное слово, значение - количество повторений
            Dictionary<string, int> wordStat = new Dictionary<string, int>();
            foreach (Match match in matches)
            {
                string word = match.ToString().ToLower();
                if (wordStat.ContainsKey(word))
                    wordStat[word]++;
                else
                {
                    wordStat.Add(word, 1);
                    allWords.Add(word);
                }
            }
            // выводим информацию об общем количестве слов
            label1.Text = "Количество слов = " + matches.Count.ToString();
            StringMax sort1 = new StringMax();
            allWords.Sort(sort1);
            // выводим информацию о самых длинных и самых коротких словах 
            label2.Text = "Топ 10 слов:";
            label3.Text = "Анти топ 10 слов:";
            for(int k = 0; k < allWords.Count % 10; k++)
            {
                label2.Text += ("\n" + (k + 1) + ") " + allWords[k]);
                label3.Text += ("\n" + (k + 1) + ") " + allWords[allWords.Count - 1 - k]);
            }
            // выводим информацию о частоте слов в тексте
            textBox1.Text = "";
            foreach (var word in wordStat)
            {
                float wordFreq = ((float)word.Value / (float)matches.Count) * 100;
                textBox1.Text += $"{word.Key} - {wordFreq}%\r\n";
            }
            // подсчитываем топ самых встречющихся слов
            // список топа
            List<string> topFreqWords = new List<string>();
            // самое длинное слово
            string max_word = null;
            // если слов меньше 10, то количество позиций топа будет = количеству слов
            for (int k = wordStat.Count % 10 - 1; k >= 0; k--)
            {
                int max = 0;
                // в цикле проходимся по парам ключ-значение из статистики и находим максимальное
                // по знчению value
                foreach (var word in wordStat)
                {
                    if (word.Value > max)
                    {
                        max = word.Value;
                        max_word = word.Key;
                    }
                }
                // добавляем в список найденное максимальное
                topFreqWords.Add(max_word);
                // удаляем его из списка статистики
                wordStat.Remove(max_word);
            }
            // выводим топ самых встречющихся слов
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
