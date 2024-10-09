--select * from PaintingsTable;
--select * from Categories;
--select * from CategorizedPaintings
--SELECT PaintingsTable.Id, PaintingsTable.Name, PaintingsTable.FileName
--FROM PaintingsTable
--INNER JOIN CategorizedPaintings ON CategorizedPaintings.PaintingID=PaintingsTable.Id;
SELECT *
FROM PaintingsTable
INNER JOIN CategorizedPaintings ON CategorizedPaintings.PaintingID=PaintingsTable.Id
where CategorizedPaintings.CategoryID = 3;