using Dapper;

using PaintingLibrary.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace PaintingLibrary
{
    public class SqliteDataAccess : IDataAccess
    {
        /// <summary>
        /// Deletes the note with the matching id.
        /// </summary>
        /// <param name="id">Id of the note to delete.</param>
        public void deleteNote(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var sqlStatement = "DELETE FROM Notes WHERE Id = @Id";
                cnn.Execute(sqlStatement, new { Id = id });
            }
        }

        /// <summary>
        /// Loads all notes from the database.
        /// </summary>
        /// <returns>List of all notes in the database.</returns>
        public List<Note> loadAllNotes()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<Note>("select * from Notes", new DynamicParameters()).ToList();
                return output;
            }
        }

        /// <summary>
        /// Loads all paintings from the datbase and associates them with their categories.
        /// </summary>
        /// <returns>All paintings and their associated categories.</returns>
        public List<Painting> loadAllPaintings()
        {
            var cnnStr = loadConnectionString();
            
            try
            {
                var MyFirstCnn = new SQLiteConnection(cnnStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            using (IDbConnection cnn = new SQLiteConnection(cnnStr))
            {
                try
                {
                    // Output contains all paintings, regardless of how many categories they are in.
                    var output = cnn.Query<Painting>("select * from PaintingsTable",
                                                        new DynamicParameters()).ToList();

                    List<CategorizedPainting> categorizedPaintings = loadAllCategorizedPaintings();
                    List<Category> categories = loadAllCategories();
                    foreach (var cp in categorizedPaintings)
                    {
                        // First, find the corresponding category.
                        Category c = categories.Where((cat) => cat.ID == cp.CategoryID).First();

                        // Now, find the corresponding painting.
                        Painting found = output.Where((p) => p.Id == cp.PaintingID).First();

                        // Now, match up the category to the correct painting in our output.
                        // If bugs occur, this statement can be broken down, obviously.
                        output.Where((p) => p.Equals(found)).First().Categories.Add(c);
                    }
                    return output.ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return new List<Painting>();
                

                /*for (int i = 0; i < categories.Count; i++)
                {
                    var dictionary = new Dictionary<string, object>
                    {
                        { "@CategoryID", categories[i].ID }
                    };
                    var parameters = new DynamicParameters(dictionary);

                    var output2 = cnn.Query<Painting>(
                        "select PaintingsTable.Id, PaintingsTable.name, " +
                        "PaintingsTable.FileName, PaintingsTable.Width, " +
                        "PaintingsTable.Length, PaintingsTable.DatePainted, " +
                        "PaintingsTable.Price, PaintingsTable.PaintingSurface, " +
                        "PaintingsTable.BlackGessoed " +
                        "from CategorizedPaintings " +
                        "inner join PaintingsTable on CategorizedPaintings.PaintingID = PaintingsTable.Id " +
                        "where CategoryID = @CategoryID", parameters).ToList();
                    if (output2?.Count != 0)
                    {
                        foreach (Painting p in output2)
                        {
                            
                        }
                    }
                }*/

            /*from c in categories
            select new Painting
            {
                Categories = c,
                Products = c.ProductCategories.Select(pc => pc.Product).ToList()
            }*/

            /*foreach (CategorizedPainting categorizedPainting in categorizedPaintings)
            {
                //output.Join()
                Painting p = output.Where((temp) => temp.Id == categorizedPainting.CategoryID).FirstOrDefault();
                Category cat = categories.Where((c) => categorizedPainting.CategoryID == c.ID).FirstOrDefault();
                p.Categories.Add(cat);
            }*/


            
                
            }
        }

        public void updatePainting(Painting painting)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                string query = "update PaintingsTable set FileName = @FileName where Id = @Id";
                var output = cnn.Execute(query, new { FileName = painting.FileName, Id = painting.Id });

                query = "update PaintingsTable set Name = @Name where Id = @Id";
                output = cnn.Execute(query, new { Name = painting.Name, Id = painting.Id });

                query = "update PaintingsTable set Width = @Width where Id = @Id";
                output = cnn.Execute(query, new { Width = painting.Width, Id = painting.Id });

                query = "update PaintingsTable set Height = @Height where Id = @Id";
                output = cnn.Execute(query, new { Length = painting.Height, Id = painting.Id });

                query = "update PaintingsTable set DatePainted = @DatePainted where Id = @Id";
                output = cnn.Execute(query, new { DatePainted = painting.DatePainted, Id = painting.Id });
            }
        }

        public void deletePainting(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                string query = "delete from CategorizedPaintings where PaintingId = @Id";
                // Output contains all paintings, regardless of how many categories they are in.
                var output = cnn.Execute(query, new { Id = id });

                query = "delete from PaintingsTable where Id = @Id";
                cnn.Execute(query, new { Id = id });
            }
        }

        /// <summary>
        /// Adds the note passed to the database. 
        /// Remember to call loadAllNotes() to refresh and update the ID of the note passed.
        /// </summary>
        /// <param name="n">Note to add to the database.</param>
        /// <returns>Whether or not the note was successfully added to database.</returns>
        public bool SaveNote(Note n)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                int rows = cnn.Execute("insert into Notes (Description,Title) values (@Description,@Title)", n);
                return rows >= 1;
            }
        }


        //public async void SaveNoteAsync(IProgress<bool> progress, Note n)
        //{
        //    progress.Report(false);
        //    using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
        //    {
        //        int rows = await cnn.ExecuteAsync("insert into Notes (Description,Title) values (@Description,@Title)", n);
        //        progress.Report(true);
        //    }
        //}

        public List<CategorizedPainting> loadAllCategorizedPaintings()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                string query = "SELECT * FROM CategorizedPaintings";
                var output = cnn.Query<CategorizedPainting>(query, new DynamicParameters());
                return output.ToList();
            }
        }

        //public static DBQueryResult savePaintingAsync(Painting painting)
        //{
        //    return null;
        //    // throw new NotImplementedException();
        //}

        public DBQueryResult savePainting(Painting painting)
        {
            DBQueryResult output = new DBQueryResult();
            /*
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                output.RowsAffected = cnn.Execute("insert into PaintingsTable (Name,FileName,Width,Length,DatePainted,Price, PaintingSurface) values (@Name,@FileName,@Width,@Length,@DatePainted,@Price,@PaintingSurface)", painting);
                // output.LastID = Convert.ToInt32(((SQLiteConnection)cnn).LastInsertRowId);
                output.LastID = painting.Id;
            }*/

            SQLiteConnection cnn = new SQLiteConnection(loadConnectionString());
            cnn.Open();

            SQLiteCommand Command = new SQLiteCommand("", cnn);

            Command.CommandText = "insert into PaintingsTable (Name,FileName,Width,Length,DatePainted,Price, PaintingSurface) values (@Name,@FileName,@Width,@Length,@DatePainted,@Price,@PaintingSurface)";

            Command.CommandType = System.Data.CommandType.Text;

            Command.Parameters.Add(new SQLiteParameter("@Name", painting.Name));
            Command.Parameters.Add(new SQLiteParameter("@FileName", painting.FileName));
            Command.Parameters.Add(new SQLiteParameter("@Width", painting.Width));
            Command.Parameters.Add(new SQLiteParameter("@Length", painting.Height));
            Command.Parameters.Add(new SQLiteParameter("@DatePainted", painting.DatePainted));
            Command.Parameters.Add(new SQLiteParameter("@Price", painting.Price));
            Command.Parameters.Add(new SQLiteParameter("@PaintingSurface", painting.PaintingSurface));

            int Status = Command.ExecuteNonQuery();

            Command.CommandText = "select last_insert_rowid()";

            // The row ID is a 64-bit value - cast the Command result to an Int64.
            //
            Int64 LastRowID64 = (Int64)Command.ExecuteScalar();

            // Then grab the bottom 32-bits as the unique ID of the row.
            //
            int LastRowID = (int)LastRowID64;

            output.RowsAffected = Status;
            output.LastID = LastRowID;
            cnn.Close();
            return output;
        }

        public void saveCategorizedPainting(int paintingID, int categoryID)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                try
                {
                    cnn.Execute("insert into CategorizedPaintings (CategoryID, PaintingID) values (@categoryID,@paintingID)",
                        new { categoryID = categoryID, paintingID = paintingID });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("junk");
                }
            }
        }

        /// <summary>
        /// Loads all categories from the database.
        /// </summary>
        /// <returns>A list of all categories in the database.</returns>
        public List<Category> loadAllCategories()
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                var output = cnn.Query<Category>("select * from Categories", new DynamicParameters());
                return output.ToList();
            }
        }

        /// <summary>
        /// Loads the connection string.
        /// </summary>
        /// <param name="id">Id of the connection string. By default, has a value of Default.</param>
        /// <returns>A string containing the connection string.</returns>
        private static string loadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }

        public void clearCategoriesForPainting(int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(loadConnectionString()))
            {
                cnn.Execute("delete from CategorizedPaintings where PaintingId = @PaintingId",
                    new { PaintingId = id });
            }
        }
    }
}