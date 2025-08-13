using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using App.BlogApi.Models;
namespace App.BlogApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        static private List<Article> _articles = new List<Article>();

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        public IActionResult Get()
        {
            return Ok(_articles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {
            var item = _articles.Find(x => x.Id == id);
            if (item != null)
            {
                return Ok(item);
            }
            else
            {
                return NotFound("Bu id'ye sahip bir makale bulunamadı!");
            }

        }
        [HttpPost("add-article")]  // postman kullanırken belli olsun diye görevdekinden başka endpoint kullandım
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Consumes("application/json")]
        public IActionResult AddArticle([FromBody] ArticleDto articleDto)
        {

            if (string.IsNullOrEmpty(articleDto.Title))
            {
                return BadRequest("Makale title'ı boş olamaz!");
            }
            else
            {
                Article article = new Article();
                article.Id = _articles.Count + 1;
                article.Title = articleDto.Title;
                article.Content = articleDto.Content;
                _articles.Add(article);
                return Created($"/api/articles", articleDto);
            }

        }

        [HttpPut("update/{id}")]  // postman kullanırken belli olsun diye görevdekinden başka endpoint kullandım
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public IActionResult UpdateArticle(int id, ArticleDto articleDto)
        {
            var item = _articles.Find(x => x.Id == id);

            if (string.IsNullOrEmpty(articleDto.Title))
            {
                return BadRequest("Makale title'ı boş veya null olamaz!");
            }
            else if (item == null)  // burayı ekstra eklemek istedim.
            {
                return NotFound("Makale bulunamadı!");
            }
            else
            {
                item.Title = articleDto.Title;
                item.Content = articleDto.Content;
                return Ok(item);
            }
        }

        [HttpDelete("delete/{id}")] // postman kullanırken belli olsun diye görevdekinden başka endpoint kullandım
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        public IActionResult DeleteArticle(int id)
        {
            var silinecek = _articles.Find(x => x.Id == id);
            if (silinecek == null)
            {
                return NoContent();
            }
            else
            {
                _articles.Remove(silinecek);
                return Ok($"{silinecek} id'ye sahip makale silinmiştir!");
            }
        }

    }
}

