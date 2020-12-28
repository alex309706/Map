using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

using Newtonsoft.Json;
using Domain.Core;
using Infrastructure.Data;
using Interfaces;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SubdivisionsController : ControllerBase
    {
        readonly ISubdivisionRepository repo = new ExcelSubdivisionRepository(@"E:\Саша\Начало\Наполнение\Книга_с_группировкой_(2).xlsx");
        IEnumerable<Subdivision> subd;
        [HttpGet]
        public IEnumerable<Subdivision> Get()
        {
            subd = repo.GetSubdivisions();
            return subd;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Subdivision result = repo.GetSubdivision(id);
            return new ObjectResult(result);
        }

    }
}
