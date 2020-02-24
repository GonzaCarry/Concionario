using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using APIConcesionario.Models;

namespace APIConcesionario.Controllers
{
    public class CarsController : ApiController
    {
        private DBConcesionarioEntities db = new DBConcesionarioEntities();

        // GET: api/Cars
        public IQueryable<Cars> GetCars()
        {
            return db.Cars;
        }

        // GET: api/Cars/5
        [ResponseType(typeof(Cars))]
        public IHttpActionResult GetCars(string id)
        {
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return NotFound();
            }

            return Ok(cars);
        }

        // PUT: api/Cars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCars(string id, Cars cars)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cars.id_Cars)
            {
                return BadRequest();
            }

            db.Entry(cars).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Cars
        [ResponseType(typeof(Cars))]
        public IHttpActionResult PostCars(Cars cars)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Cars.Add(cars);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CarsExists(cars.id_Cars))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cars.id_Cars }, cars);
        }

        // DELETE: api/Cars/5
        [ResponseType(typeof(Cars))]
        public IHttpActionResult DeleteCars(string id)
        {
            Cars cars = db.Cars.Find(id);
            if (cars == null)
            {
                return NotFound();
            }

            db.Cars.Remove(cars);
            db.SaveChanges();

            return Ok(cars);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarsExists(string id)
        {
            return db.Cars.Count(e => e.id_Cars == id) > 0;
        }
    }
}