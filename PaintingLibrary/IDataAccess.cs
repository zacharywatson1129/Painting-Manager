using PaintingLibrary.Models;
using System.Collections.Generic;

namespace PaintingLibrary
{
    public interface IDataAccess
    {
        void clearCategoriesForPainting(int id);
        void deleteNote(int id);
        void deletePainting(int id);
        List<Category> loadAllCategories();
        List<Note> loadAllNotes();
        List<Painting> loadAllPaintings();
        void saveCategorizedPainting(int paintingID, int categoryID);
        bool SaveNote(Note n);
        DBQueryResult savePainting(Painting painting);
        void updatePainting(Painting painting);
    }
}