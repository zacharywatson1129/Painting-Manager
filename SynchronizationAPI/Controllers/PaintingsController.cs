using Microsoft.AspNetCore.Mvc;
using PaintingLibrary;
using PaintingLibrary.Models;
using System.Net.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SynchronizationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaintingsController : ControllerBase
    {

        private SqliteDataAccess _dataAccess;

        public PaintingsController(SqliteDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        // GET: api/<PaintingsController>
        // [HttpGet]
        [HttpGet("DesktopGet")]
        public IEnumerable<Painting> GetDesktopList()
        {
            List<Painting> paintings = null; //sqliteDBHelper.loadPaintings(); 
            List<string> imageFileNames = getPaintingFileNames(paintings);
            var filePath = @"..\..\..\..\PaintingDetailsManager\Data\Images";

            using (var multipartFormContent = new MultipartFormDataContent())
            {
                //Add other fields
                //multipartFormContent.Add(new StringContent("123"), name: "UserId");
                //multipartFormContent.Add(new StringContent("Home insurance"), name: "Title");

                //Add the file
                var fileStreamContent = new StreamContent(File.OpenRead(filePath));
                fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                multipartFormContent.Add(fileStreamContent, name: "file", fileName: "house.png");

                //Send it
                var response = await httpClient.PostAsync("https://localhost:12345/files/", multipartFormContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }

            return new List<Painting> { new Painting { Id = 1, Name = "Desktop Painting" } };
        }

        // GET: api/<PaintingsController>
        //[HttpGet]
        [HttpGet("MobileGet")]
        public IEnumerable<Painting> GetMobileList()
        {

            return new List<Painting> { new Painting { Id = 5, Name = "Mobile Painting" } };
        }

        // GET api/<PaintingsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PaintingsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PaintingsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PaintingsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
