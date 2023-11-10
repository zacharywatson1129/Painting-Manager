using PaintingLibrary;
using PaintingLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaintingDetailsManager
{
    public class HTMLWriter
    {
        public static void Write(string path, List<Painting> source)
        {

            List<Painting> forSaleOnly = new List<Painting>();
            foreach (var p in source) 
            {
                bool isPersonalCollection = false;
                bool isCoatedOver = false;
                foreach (var c in p.Categories)
                {

                    if (c.Name == "Personal Collection")
                        isPersonalCollection = true;
                    if (c.Name == "Coated Over")
                        isCoatedOver = true;
                }
                if (!isPersonalCollection && !isCoatedOver)
                    forSaleOnly.Add(p);
            }

            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.Append("<html>\n<head>\n<title>Paintings List</title>");
            htmlBuilder.Append("\n<link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\"></head>\n");
            htmlBuilder.Append("<body><h1>Paintings List</h1>");
            htmlBuilder.Append("<table><thead><tr><th>Image</th><th>Name</th><th>Size and Surface</th>");
            htmlBuilder.Append("<th>Date Painted</th>");
            htmlBuilder.Append("<th>Date Sold</th>");
            htmlBuilder.Append("<th>Amount Sold for</th>");
            htmlBuilder.Append("</tr></thead><tbody>\n");

            // string text = "<html>\n<head><title>Paintings List</title></head>";
			/*text += "<body><h1>Paintings List</h1>";
            text += "<table><thead><tr><th>Image</th><th>Name</th><th>Size</th><th>Surface</th>";
            text += "</tr></thead><tbody>";*/

            foreach (Painting p in forSaleOnly)
            {
                htmlBuilder.Append("<tr><td>");
                var hrefText = $"<a href=\"{p.FileName}\">";
                htmlBuilder.Append(hrefText);
                var img = $"<img width=200 src=\"{p.FileName}\">";
                htmlBuilder.Append(img);
                var extra = $"</a></td><td>{p.Name}</td><td>{p.TextSize} on {p.PaintingSurface}</td><td>{p.DatePainted.ToShortDateString()}</td><td> </td><td> </td></tr>";
                htmlBuilder.Append(extra);
                htmlBuilder.Append("\n");
                //text += "<tr><td>";

                //text += hrefText;
                //text += $"<img src = { p.FileName}>";
                // text += $"</a></td><td>{p.Name}</td><td>{p.TextSize}</td><td>Canvas</td></tr>";
            }
            htmlBuilder.Append("</tbody></table></body>\n</html>");
            //text += "</tbody></table></body>\n</html>";

            string text = htmlBuilder.ToString();
            
            File.WriteAllText(path, text);
        }
    }
}
