﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            statusStrip1.Items[0].Text = null; 

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // Установка флага о внесении изменений
            changed = true;
            statusStrip1.Items[0].Text = "Не сохранено";
        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            // Меню Редактирование – Копировать:
            if (richTextBox1.TextLength != 0)
                richTextBox1.Copy();
        }

        private void tsmiPaste_Click(object sender, EventArgs e)
        {
            // Меню Редактирование – Вставить:

            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                richTextBox1.Paste();
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {
            // Меню Редактирование – Вырезать:
            if (richTextBox1.TextLength != 0)
                richTextBox1.Cut();
        }

        private void saveFileAs() //метод "сохранить как"
        {
            //если пользователь выбрал имя файла и нажал кнопку "ОК" в диалоге сохранения 
            if (saveFileDialog1.ShowDialog() == DialogResult.OK && saveFileDialog1.FileName != null)
            {
                fileName = saveFileDialog1.FileName; //запоминаем имя файла
                saveFile(); //вызываем метод "сохранить"
            }
        }

        private void saveFile() //метод "сохранить"
        {
            try
            {
                //сохраняем содержимое текстового поля под выбранным именем
                richTextBox1.SaveFile(fileName, RichTextBoxStreamType.RichText);
                afterSaving();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить файл! Информация об ошибке: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (fileName == null) //если имя файла пусто
            {
                //вызываем метод "сохранить как"
                saveFileAs();
            }
            else
            {
                //вызываем метод "сохранить"
                saveFile();
            }
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e) => saveFileAs();

        private bool closeFileQuery() // Запрос на закрытие файла
        {
            if (changed) // Если есть несохраненные изменения
            {
                DialogResult result = MessageBox.Show("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes) 
                {
                    if (fileName != null)
                        saveFile();
                    else
                        saveFileAs();
                }
                else
                {
                    if (result == DialogResult.Cancel)
                        return false; // Запрещает закрытие файла

                    return true; // Разрешает закрытие файла
                }
            }
            return true;
        }

        private void afterSaving()
        {
            changed = false;
            if (fileName != null)
                this.Text = Path.GetFileName(fileName) + " - BlokNot"; //выносим в заголовок окна имя файла без пути
            else
                this.Text = "Новый файл - BlockNote";
            statusStrip1.Items[0].Text = "Сохранено"; //снимаем флаг изменений
        }

        private void openFile() // Метод открытия файла
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK && openFileDialog1.FileName != null)
            {
                try
                {
                    // Открытие файла *.rtf
                    richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.RichText);
                }
                catch (System.ArgumentException) 
                {
                    // Открытие файла *.* => *.txt
                    richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                }
                changed = false;
                fileName = openFileDialog1.FileName;
            }
        }

        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            if (closeFileQuery())
            {
                openFile();
                afterSaving();
            }
        }

        private void newFile()
        {
            richTextBox1.Clear();
            changed = false;
            fileName = null;
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            if(closeFileQuery())
            {
                newFile();
                afterSaving();
            }
        }

        private void tsmiFont_Click(object sender, EventArgs e) 
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK) //если пользователь нажал кнопку ОК в диалоге
            {
                richTextBox1.SelectionFont = fontDialog1.Font; //изменяем шрифт выделенного фрагмента на указанный в диалоге
                richTextBox1.SelectionColor = fontDialog1.Color; //изменяем цвет выделенного фрагмента на указанный в диалоге
            }
        }

        private void tsmiToLeft_Click(object sender, EventArgs e) => richTextBox1.SelectionAlignment = HorizontalAlignment.Left;

        private void tsmiToCenter_Click(object sender, EventArgs e) => richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

        private void tsmiToRight_Click(object sender, EventArgs e) => richTextBox1.SelectionAlignment= HorizontalAlignment.Right;

        private void tsmiMarker_Click(object sender, EventArgs e) => richTextBox1.SelectionBullet = !richTextBox1.SelectionBullet;

        private void tsmiEdit_Click(object sender, EventArgs e)
        {
            //доступность подпунктов меню определяется возможностью отмены (повтора)
            tsmiUndo.Enabled = richTextBox1.CanUndo;
            tsmiRedo.Enabled = richTextBox1.CanRedo;
        }

        private void tsmiDelete_Click(object sender, EventArgs e) => richTextBox1.SelectedText = "";  //присваиваем выделенному фрагменту текста пустое значение

        private void tsmiFill_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK) 
                // Замена цвета фона на выбранный в диалоге
                richTextBox1.SelectionBackColor = colorDialog1.Color;
        }

        private void tsmiExit_Click(object sender, EventArgs e) => Application.Exit();

        private void blockNoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closeFileQuery()) //Запрос на отмену выхода
                e.Cancel = true;
        }

        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            helpForm hf = new helpForm();
            hf.Show();
        }
    }
}
