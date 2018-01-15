using System;
using System.Windows.Forms;

namespace Search
{
    public partial class Form1 : Form
    {
        string typeFile = ""; //тип файла
        int count = 0; //количество файлов
        System.Threading.Thread Thread1; //обьявление потока для поиска

        public Form1()
        {
            InitializeComponent(); 
        }

        private void FileSearchFunction(object Dir)
        {
            System.IO.DirectoryInfo DI = null; //рабочая директория
            System.IO.DirectoryInfo[] SubDir = null; // массив директорий 
            System.IO.FileInfo[] FI = null; // массив файлов
            try
            {
                DI = new System.IO.DirectoryInfo((string)Dir);
                SubDir = DI.GetDirectories();
            }
            catch
            {
                return;
            }

            for (int i = 0; i < SubDir.Length; ++i)
                this.FileSearchFunction(SubDir[i].FullName); //рекурсивный вызов

            try
            {
                FI = DI.GetFiles(); //получение списка файлов
            }
            catch
            {
                return;
            }

            /*Поиск и вывод необходимых файлов*/
            for (int i = 0; i < FI.Length; ++i)
                try
                {
                    this.Invoke(new System.Threading.ThreadStart(delegate
                    {
                        if (FI[i].FullName.Length > 60)
                            label4.Text = "..." + FI[i].FullName.Substring(FI[i].FullName.Length - 60);
                        else
                            label4.Text = FI[i].FullName;

                        if (typeFile == System.IO.Path.GetExtension(FI[i].FullName))
                        {
                            listBox1.Items.Add(FI[i].Name);

                            if (autoScrollCheckBox.Checked == true)
                            {
                                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                            }
                            label1.Text = "Quantity fires:" + Convert.ToString(++count);
                        }
                    }));
                }
                catch
                {
                    return;
                }

        }

        /*Обработчик кнопки Выбор директории*/
        private void DirButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirDialog = new FolderBrowserDialog();
            DirDialog.Description = "Выбор директории";
            DirDialog.SelectedPath = @"C:\";

            if (DirDialog.ShowDialog() == DialogResult.OK)
            {
                dirTextBox.Text = DirDialog.SelectedPath;
            }
        }

        /*Обработчик кнопки Start/Stop*/
        private void SearchButton_Click(object sender, EventArgs e)
        {
            if (searchButton.Text == "Start search")
            {
                if (dirTextBox.Text != String.Empty && System.IO.Directory.Exists(dirTextBox.Text))
                {
                    listBox1.Items.Clear();
                    label1.Text = "Quantity fires: 0";
                    count = 0;
                    typeFile = typeComboBox.Text;
                    Thread1 = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FileSearchFunction));
                    Thread1.Start(dirTextBox.Text);
                    searchButton.Text = "Stop search";
                }
                else MessageBox.Show("Select directory!");
            }
            else
            {
                Thread1.Abort();
                dirTextBox.Clear();
                label4.Text = "";
                typeComboBox.Text = "";
                searchButton.Text = "Start search";
            }
        }
    }
}
