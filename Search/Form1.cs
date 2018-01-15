using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Search
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void FileSearchFunction(object Dir)
        {
            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo((string)Dir);
            System.IO.DirectoryInfo[] SubDir = null;
            try
            {
                SubDir = DI.GetDirectories();
            }
            catch
            {
                return;
            }

            for (int i = 0; i < SubDir.Length; ++i)
                this.FileSearchFunction(SubDir[i].FullName);

            System.IO.FileInfo[] FI = DI.GetFiles();
            for (int i = 0; i < FI.Length; ++i)
                this.Invoke(new System.Threading.ThreadStart(delegate
                {
                    listBox1.Items.Add(FI[i].FullName);
                }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog DirDialog = new FolderBrowserDialog();
            DirDialog.Description = "Выбор директории";
            DirDialog.SelectedPath = @"C:\";

            if (DirDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = DirDialog.SelectedPath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty && System.IO.Directory.Exists(textBox1.Text))
            {
                System.Threading.Thread T = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(FileSearchFunction));
                T.Start(textBox1.Text);
            }
        }
    }
}
