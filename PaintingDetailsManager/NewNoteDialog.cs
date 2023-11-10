using PaintingDetailsManager.ViewModels;
using PaintingDetailsManager.Views;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager
{
    public class NewNoteDialog : IDialog<Note>
    {


        Note IDialog<Note>.showDialog()
        {
            CreateNoteViewModel vm = new CreateNoteViewModel();

            return vm.CurrentItem;
        }
    }
}
