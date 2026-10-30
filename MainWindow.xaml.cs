using Notes.entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Notes
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DateSort.SelectedIndex = 0;
            LoadNotes();
        }

        private void LoadNotes()
        {
            try
            {
                var query = App.context.Notes.AsQueryable();

                if (!string.IsNullOrEmpty(Search.Text)) query = query.Where(n => n.title.Contains(Search.Text.ToLower().Trim()) || n.description.Contains(Search.Text.ToLower().Trim()));

                if (DateSort.SelectedIndex == 1) query = query.OrderByDescending(n => n.created_at);
                if (DateSort.SelectedIndex == 2) query = query.OrderBy(n => n.created_at);

                NotesLV.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadNotes();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (new EditNotesWindow().ShowDialog() == true)
                LoadNotes();
        }

        private void DateSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadNotes();
        }

        private void NotesLV_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (NotesLV.SelectedItem is Note selected)
            {
                if (new EditNotesWindow(selected).ShowDialog() == true)
                    LoadNotes();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

            if (NotesLV.SelectedItem is Note selected)
            {
                if (MessageBox.Show("Вы действительно хотите удалить выбранную запись?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        App.context.Notes.Remove(selected);
                        App.context.SaveChanges();
                        LoadNotes();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        private void NotesLV_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
                DeleteBtn_Click(sender, e);
        }
    }
}
