using Notes.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Notes
{
    public partial class EditNotesWindow : Window
    {
        private Note nt;
        private bool isEdit = false;

        public EditNotesWindow()
        {
            InitializeComponent();
            Title = "Создание заметки";
            TitleBlock.Text = "Создание заметки";
            Confirm.Content = "Создать";
        }

        public EditNotesWindow(Note note)
        {
            InitializeComponent();
            Title = "Редактирование заметки";
            TitleBlock.Text = "Редактирование заметки";
            Confirm.Content = "Сохранить";

            isEdit = true;
            nt = note;

            TitleBox.Text = nt.title;
            DescriptionBox.Text = nt.description;

        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var title = TitleBox.Text.Trim();
            var description = DescriptionBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название заметки.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Введите описание заметки.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                DescriptionBox.Focus();
                return;
            }

            try
            {
                if (isEdit)
                {
                    nt.title = title;
                    nt.description = description;
                }
                else
                {
                    var note = new Note
                    {
                        title = title,
                        description = description,
                        created_at = DateTime.Now
                    };
                    App.context.Notes.Add(note);
                }

                App.context.SaveChanges();
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка БД",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
