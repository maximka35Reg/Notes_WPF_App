using Notes.entities;
using System.Windows;

namespace Notes
{
    public partial class App : Application
    {
        public static NotesEntities context { get; set; } = new NotesEntities(); 
    }
}
