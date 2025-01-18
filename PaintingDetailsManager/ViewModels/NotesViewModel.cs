using Caliburn.Micro;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PaintingDetailsManager.ViewModels
{
    public class NotesViewModel : Screen
    {
        private IDataAccess _dataAccess;
        private BindableCollection<Note> notes;

        public BindableCollection<Note> Notes
        {
            get { return notes; }
            set { notes = value; NotifyOfPropertyChange(() => Notes); }
        }

        private Note _note;

        public Note SelectedNote
        {
            get { return _note; }
            set 
            { 
                _note = value; 
                NotifyOfPropertyChange(() => SelectedNote); 
                NotifyOfPropertyChange(() => CanDeleteNote);
                NotifyOfPropertyChange(() => Description);
            }
        }

        public NotesViewModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            LoadNotes();
        }

        public void DeleteNote()
        {
            _dataAccess.DeleteNote(SelectedNote.Id);
            Notes.Remove(SelectedNote);
        }

        public bool CanDeleteNote 
        { 
            get 
            {
                if (SelectedNote != null)
                    return true;
                return false;
            }
        }

        public void AddNote(Note n)
        {
            if (n != null)
            {
                this.Notes.Add(n);
                _dataAccess.SaveNote(n);
            }
            else
            {
                // Don't really do this...
                MessageBox.Show("Hey stupid! You didn't enter a message");
            }
        }

        public string Description { get { return SelectedNote.Description; } }

        public void CreateNewNote()
        {
            CreateNoteViewModel vm = new CreateNoteViewModel(_dataAccess);
            // NewNoteViewModel vm2 = new NewNoteViewModel();
            WindowManager manager = new WindowManager();

            


            // manager.ShowWindow(vm, null, null);
            // manager.ShowDialog(vm, null, null);
            manager.ShowDialog(vm, null, null); // Works except it needs to be a window.
            LoadNotes();
            // AddNote(vm.CurrentItem);

        }

        public void LoadNotes()
        {
            List<Note> temp = _dataAccess.LoadAllNotes();
            Notes = new BindableCollection<Note>(temp);
        }

        /*public bool CanDeleteNote()
        {
            if (SelectedNote != null)
                return true;
            return false;
        }*/

    }
}
