using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace webapi.Controllers
{
    // See https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging for logging info
    [Route("[controller]")]
    public class ContactsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IContactRepository _repository;


        public ContactsController(
            ILogger<ContactsController> logger,
            IContactRepository repository)
        {
            this._logger = logger;
            this._repository = repository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return base.Ok(this._repository.GetAll());
        }


        [HttpGet("{id}", Name = "Get")]
        public IActionResult  Get(
            Guid id)
        {
            return base.Ok("value");
        }


        [HttpPost]
        public IActionResult  Post(
            [FromBody] Models.NewContact value)
        {
            if (!base.ModelState.IsValid) { return base.BadRequest(base.ModelState); }

            var id = Guid.NewGuid();
            var contact = new Models.Contact { Id = id, Name = value.Name, MobileNumber = value.MobileNumber };

            this._logger.LogInformation($"Creating new contact with Id {id}");
            this._repository.Upsert(contact);

            return base.CreatedAtRoute("Get", new { id = contact.Id }, contact);
        }


        [HttpPut("{id}")]
        public IActionResult Put(
            Guid id,
            [FromBody]Models.Contact value)
        {
            if (!base.ModelState.IsValid) { return base.BadRequest(base.ModelState); }
            if (id != value.Id) { return BadRequest(new { Id = "Id must match the body entity Id" }); }

            this._logger.LogInformation($"Updating contact with Id {id}");
            this._repository.Upsert(value);

            return base.Ok(value);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(
            Guid id)
        {
            if (!base.ModelState.IsValid) { return base.BadRequest(base.ModelState); }

            this._logger.LogInformation($"Deleteing contact with Id {id}");
            if (!this._repository.Delete(id)) { return base.NotFound(); }

            return base.NoContent();
        }
    }
}
