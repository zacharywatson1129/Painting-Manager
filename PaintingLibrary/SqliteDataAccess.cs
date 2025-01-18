using Dapper;

using PaintingLibrary.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using PaintingLibrary.DapperTypeHandlers;

namespace PaintingLibrary
{
    public class SqliteDataAccess : IDataAccess
    {

        private string connectionString = string.Empty;

        public SqliteDataAccess(string cnnString)
        {
            connectionString = cnnString;
            // Not sure if this is where we do it?
            SqlMapper.AddTypeHandler(new GuidTypeHandler());
        }

        public void InitializeDatabase()
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                // 1. Create the PaintingsTable Table
                var sqlStatement = @"CREATE TABLE IF NOT EXISTS PaintingsTable (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, "
                                                + "Name TEXT NOT NULL, "
                                                + "FileName  TEXT NOT NULL, "
                                                + "Width INTEGER NOT NULL, "
                                                + "Height    INTEGER NOT NULL, "
                                                + "DatePainted TEXT, "
                                                + "Price REAL, "
                                                + "PaintingSurface TEXT)";
                cnn.Execute(sqlStatement);

                // 2. Create the Categories Table
                sqlStatement = @"CREATE TABLE IF NOT EXISTS Categories (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT NOT NULL)";
                cnn.Execute(sqlStatement);

                // 3. Create the CategorizedPaintings Table
                sqlStatement = @"CREATE TABLE IF NOT EXISTS CategorizedPaintings ("
                                                + "CategoryID INTEGER, "
                                                + "PaintingID INTEGER, "
                                                + "FOREIGN KEY(CategoryID) REFERENCES Categories(Id), "
                                                + "FOREIGN KEY(PaintingID) REFERENCES PaintingsTable(Id))";
                cnn.Execute(sqlStatement);

                // 4. Create the Notes Table
                sqlStatement = @"CREATE TABLE IF NOT EXISTS Notes (Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, "
                                                + "Title TEXT NOT NULL, "
                                                + "Description TEXT);";
                cnn.Execute(sqlStatement);

            }
        }

        /// <summary>
        /// Deletes the note with the matching id.
        /// </summary>
        /// <param name="id">Id of the note to delete.</param>
        public void DeleteNote(Guid id)
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                var sqlStatement = "DELETE FROM Notes WHERE Id = @Id";
                cnn.Execute(sqlStatement, new { Id = id });
            }
        }

        /// <summary>
        /// Loads all notes from the database.
        /// </summary>
        /// <returns>List of all notes in the database.</returns>
        public List<Note> LoadAllNotes()
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                var output = cnn.Query<Note>("select * from Notes", new DynamicParameters()).ToList();
                return output;
            }
        }

        /// <summary>
        /// Loads all paintings from the datbase and associates them with their categories.
        /// </summary>
        /// <returns>All paintings and their associated categories.</returns>
        public List<Painting> LoadAllPaintings()
        {
            var cnnStr = connectionString;
            
            try
            {
                var MyFirstCnn = new SqliteConnection(cnnStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            using (IDbConnection cnn = new SqliteConnection(cnnStr))
            {
                try
                {
                    // Output contains all paintings, regardless of how many categories they are in.
                    var output = cnn.Query<Painting>("select * from PaintingsTable",
                                                        new DynamicParameters()).ToList();

                    List<CategorizedPainting> categorizedPaintings = loadAllCategorizedPaintings();
                    List<Category> categories = LoadAllCategories();
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

        /// <summary>
        /// Helper method to take some paintings and just get their file names.
        /// </summary>
        /// <param name="paintings">List of paintings to search</param>
        /// <returns>List of strings with painting file names.</returns>
        public List<string> getPaintingFileNames(List<Painting> paintings)
        {
            List<string> output = new List<string>();
            foreach (var p in paintings)
            {
                output.Add(p.FileName);
            }
            return output;
        }

        public void UpdatePainting(Painting painting)
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
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

        public void DeletePainting(Guid id)
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
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
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                n.Id = Guid.NewGuid();
                int rows = cnn.Execute("insert into Notes (Id,Description,Title) values (@Id, @Description,@Title)", n);
                return rows >= 1;
            }
        }


        //public async void SaveNoteAsync(IProgress<bool> progress, Note n)
        //{
        //    progress.Report(false);
        //    using (IDbConnection cnn = new SqliteConnection(connectionString))
        //    {
        //        int rows = await cnn.ExecuteAsync("insert into Notes (Description,Title) values (@Description,@Title)", n);
        //        progress.Report(true);
        //    }
        //}

        public List<CategorizedPainting> loadAllCategorizedPaintings()
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
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

        public void SavePainting(Painting painting)
        {
            DBQueryResult output = new DBQueryResult();
            /*
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                output.RowsAffected = cnn.Execute("insert into PaintingsTable (Name,FileName,Width,Length,DatePainted,Price, PaintingSurface) values (@Name,@FileName,@Width,@Length,@DatePainted,@Price,@PaintingSurface)", painting);
                // output.LastID = Convert.ToInt32(((SqliteConnection)cnn).LastInsertRowId);
                output.LastID = painting.Id;
            }*/

            SqliteConnection cnn = new SqliteConnection(connectionString);
            cnn.Open();

            SqliteCommand Command = new SqliteCommand("", cnn);

            Command.CommandText = "insert into PaintingsTable (Id, Name,FileName,Width,Height,DatePainted,Price,PaintingSurface) values (@Id, @Name,@FileName,@Width,@Height,@DatePainted,@Price,@PaintingSurface)";

            Command.CommandType = System.Data.CommandType.Text;

            painting.Id = Guid.NewGuid();

            Command.Parameters.Add(new SqliteParameter("@Id", painting.Id));
            Command.Parameters.Add(new SqliteParameter("@Name", painting.Name));
            Command.Parameters.Add(new SqliteParameter("@FileName", painting.FileName));
            Command.Parameters.Add(new SqliteParameter("@Width", painting.Width));
            Command.Parameters.Add(new SqliteParameter("@Height", painting.Height));
            Command.Parameters.Add(new SqliteParameter("@DatePainted", painting.DatePainted));
            Command.Parameters.Add(new SqliteParameter("@Price", painting.Price));
            Command.Parameters.Add(new SqliteParameter("@PaintingSurface", painting.PaintingSurface));

            /*int Status = 0;
            try
            {
                Status = Command.ExecuteNonQuery();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            


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
            return output;*/
        }

        public void SaveCategorizedPainting(Guid paintingID, Guid categoryID)
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                try
                {
                    cnn.Execute("insert into CategorizedPaintings (CategoryID, PaintingID) values (@categoryID,@paintingID)",
                        new { categoryID = categoryID, paintingID = paintingID });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Loads all categories from the database.
        /// </summary>
        /// <returns>A list of all categories in the database.</returns>
        public List<Category> LoadAllCategories()
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
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

        public void ClearCategoriesForPainting(Guid id)
        {
            using (IDbConnection cnn = new SqliteConnection(connectionString))
            {
                cnn.Execute("delete from CategorizedPaintings where PaintingId = @PaintingId",
                    new { PaintingId = id });
            }
        }
    }
}