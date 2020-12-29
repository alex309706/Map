using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiSubdivisions.Models;

namespace WebApiSubdivisions.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActualDatumsController : ControllerBase
    {
        private readonly DivisionsDatabaseContext _context;

        public ActualDatumsController(DivisionsDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/ActualDatums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActualDatum>>> GetActualData()
        {

            return await _context.ActualData.ToListAsync();
        }

        // GET: api/ActualDatums/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ActualDatum>> GetActualDatum(int id)
        //{
        //    var actualDatum = await _context.ActualData.FindAsync(id);

        //    if (actualDatum == null)
        //    {
        //        return NotFound();
        //    }
        //    return actualDatum;
        //}

        // PUT: api/ActualDatums/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActualDatum(int id, ActualDatum actualDatum)
        {
            if (id != actualDatum.Id)
            {
                return BadRequest();
            }

            _context.Entry(actualDatum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActualDatumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ActualDatums
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ActualDatum>> PostActualDatum(ActualDatum actualDatum)
        {
            _context.ActualData.Add(actualDatum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActualDatum", new { id = actualDatum.Id }, actualDatum);
        }

        // DELETE: api/ActualDatums/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ActualDatum>> DeleteActualDatum(int id)
        {
            var actualDatum = await _context.ActualData.FindAsync(id);
            if (actualDatum == null)
            {
                return NotFound();
            }

            _context.ActualData.Remove(actualDatum);
            await _context.SaveChangesAsync();

            return actualDatum;
        }

        private bool ActualDatumExists(int id)
        {
            return _context.ActualData.Any(e => e.Id == id);
        }

        //[HttpGet("{datetime}")]
        //public async Task<List<ActualDatum>> GetActualDataForDate(DateTime date)
        //{
        //    var output = _context.ActualData.Where(data => data.DateTime == date).ToListAsync();

        //    return await output;
        //}
        [HttpGet("{datetime}")]
        public  string GetActualDataForDate(DateTime datetime)
        {
            var getData =  _context.ActualData;
            var getActualData = from data in getData where data.DateTime == datetime select data;
            var joinLocations = getActualData
             .Join(_context.Locations,
             data => data.Location,
             location => location.Name,
             (data, location) =>
             new
             {
                 idOfSubdivision = data.IdSubdivision,
                 NameOfLocation = location.Name,
                 x = location.X,
                 y = location.Y
             }
             );
            var joinSubdivisions = joinLocations
             .Join(_context.Subdivisions,
             newObj => newObj.idOfSubdivision,
             subdivision => subdivision.IdSubdivision,
             (newObj, subdivision) =>
                new
                {
                    NameOfLocation = newObj.NameOfLocation,
                    x = newObj.x,
                    y = newObj.y,
                    Subdivision= new Subdivision() 
                    {
                        Name = subdivision.Name,
                        Commander = subdivision.Commander,
                        Composition = subdivision.Composition,
                        Document = subdivision.Document,
                        Strength = subdivision.Strength,
                        TypeOfSubdivision = subdivision.TypeOfSubdivision,
                        IdSubdivision = subdivision.IdSubdivision
                    },
                }
            );
            var joinCommanders = joinSubdivisions
            .Join(_context.Commanders,
            newObj => newObj.Subdivision.Commander,
            commander => commander.IdCommander,
            (newObj, commander) =>
               new
               {
                   NameOfLocation = newObj.NameOfLocation,
                   x = newObj.x,
                   y = newObj.y,
                   Subdivision = newObj.Subdivision,
                   name = newObj.Subdivision.Name,
                   commander = new Commander() { Name = commander.Name },
                   composition = newObj.Subdivision.Composition,
                   document = newObj.Subdivision.Document,
                   strength = newObj.Subdivision.Strength,
                   type = newObj.Subdivision.TypeOfSubdivision,
                   idOfSubdivision = newObj.Subdivision.IdSubdivision
               }
           );;



            List<SubdivisionForClient> outputList = new List<SubdivisionForClient>();
            foreach (var obj in joinCommanders)
            {
                SubdivisionForClient s = new SubdivisionForClient(datetime);
                s.location.Name = obj.NameOfLocation;
                s.location.X = obj.x;
                s.location.Y = obj.y;
                s.subdivision.IdSubdivision = obj.Subdivision.IdSubdivision;
                s.subdivision.Composition = obj.Subdivision.Composition;
                s.subdivision.Document = obj.Subdivision.Document;
                s.subdivision.Name = obj.Subdivision.Name;
                s.subdivision.TypeOfSubdivision = obj.Subdivision.TypeOfSubdivision;
                s.subdivision.Commander = obj.Subdivision.Commander;
                s.subdivision.CommanderNavigation = obj.commander;
                s.subdivision.CommanderNavigation.IdCommander = obj.Subdivision.Commander??default(int) ;
                outputList.Add(s);
            }
        
            string json =
         JsonConvert.SerializeObject(
         outputList,
         Formatting.Indented,
         new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
         );
            return json;
        }
    }
    public class SubdivisionForClient
    {
        public DateTime date;
        public Subdivision subdivision;
        public Location location;
        public SubdivisionForClient(DateTime d)
        {
            date=d;
            subdivision = new Subdivision();
            location = new Location();
        }
    }
}
