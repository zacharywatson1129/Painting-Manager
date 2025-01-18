using PaintingLibrary.Models;
using System;
using System.Collections.Generic;

namespace PaintingLibrary
{
    /// <summary>
    /// A CRUD specification for any data access class for
    /// the Painting Manager database.
    /// </summary>
    public interface IDataAccess
    {
        // Create methods
        void SavePainting(Painting painting);
        void SaveCategorizedPainting(Guid paintingID, Guid categoryID);
        bool SaveNote(Note n);

        // Read methods
        List<Painting> LoadAllPaintings();
        List<Category> LoadAllCategories();
        List<Note> LoadAllNotes();

        // Update methods
        void UpdatePainting(Painting painting);
        void ClearCategoriesForPainting(Guid id);

        // Delete Methods
        void DeleteNote(Guid id);
        void DeletePainting(Guid id);
    }
}