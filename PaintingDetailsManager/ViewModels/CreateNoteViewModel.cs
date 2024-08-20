using Caliburn.Micro;
using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager.ViewModels
{
    
    [Export(typeof(IInput<Note>))]
    public class CreateNoteViewModel : Screen
    {
        private IDataAccess _dataAccess;
        public CreateNoteViewModel(IDataAccess dataAccess) 
        {
            _dataAccess = dataAccess;
        }
        public Note CurrentItem { get; set; } = new Note();

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
        }

        

        /*public bool validate()
        {
            /*if (string.IsNullOrEmpty(CurrentItem.Title) || string.IsNullOrEmpty(CurrentItem.Description))
                return false;
            return true;*/
        //}*/

        /*public bool CanCloseForm
        {
            get
            {
                return validate();
            }
        }*/

        public void CloseForm()
        {
            if (String.IsNullOrEmpty(CurrentItem.Title) || String.IsNullOrEmpty(CurrentItem.Description))
            {
                // Do nothing right now...later on we'll kindly yell at the user (aka me, i don't like getting yelled at)
                return;
            }
            // TODO: Use String interpolation here
            // We're not going to worry about a date and time for the note. This is enough.
            CurrentItem.Title = DateTime.Today.ToShortDateString() + " - " + CurrentItem.Title;
            _dataAccess.SaveNote(CurrentItem);
            TryClose();
        }

        public bool validate()
        {
            return true;
        }
    }
}
