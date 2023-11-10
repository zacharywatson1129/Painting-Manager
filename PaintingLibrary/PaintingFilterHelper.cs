using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingLibrary
{
    public class PaintingFilterHelper
    {
        /*public static List<Painting> getPaintingsByCategory(List<Painting> allPaintings,, int categoryID)
        {
            List<CategorizedPainting> categorizedPaintings = SqliteDataAccess.loadAPaintingsByCategory();
            List<Painting> output = new List<Painting>();
            categorizedPaintings.ForEach((cP) =>
            {
                // if that CategorizedPainting's ID matches catID (which is the ID of the category we selected)
                if (cP.CategoryID == categoryID)
                {
                    // Find the painting with the matching ID.
                    Painting matchingPainting = allPaintings.Where((p) => p.Id == cP.PaintingID).ToList().First();
                    output.Add(matchingPainting);
                }
            });
            return output;
        }*/
    }
}
