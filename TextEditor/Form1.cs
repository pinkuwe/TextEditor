using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor
{
    public partial class blockNoteForm : Form
    {
        private bool changed = false;   //были ли изменения в файле
        private string fileName = null; //имя файла на диске
        public blockNoteForm()
        {
            InitializeComponent();
            this.Text = "Новый документ - BlokNote"; //заголовок окна – имя файла и программы
            statusStrip1.Items[0].Text = null; //стока состояния - сохранение

        }

        private void blockNoteForm_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            changed = true;
            statusStrip1.Items[0].Text = "Не сохранено";
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength != 0)
                richTextBox1.Copy();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                richTextBox1.Paste();
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 0)
                richTextBox1.Cut();
        }
    }
}
