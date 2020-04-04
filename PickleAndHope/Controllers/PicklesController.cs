using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PickleAndHope.DataAccessLayer;
using PickleAndHope.Models;

namespace PickleAndHope.Controllers
{
    [Route("api/pickles")]
    [ApiController]
    public class PicklesController : ControllerBase
    {
        PickleRepo _repository = new PickleRepo();

        [HttpPost("add")]
        public IActionResult AddPickle(Pickle pickleToAdd)
        {
            var existingPickle = _repository.GetByType(pickleToAdd.Type);
            if (existingPickle == null)
            {
                _repository.Add(pickleToAdd);
                return Created("", pickleToAdd);
            }
            else
            {
                var updatedPickle = _repository.Update(pickleToAdd);
                return Ok(updatedPickle);
            }
        }

        [HttpGet]
        public IActionResult GetAllPickles()
        {
            var allPickles =  _repository.GetAll();
            return Ok(allPickles);
        }

        [HttpGet("{id}")]
        public IActionResult GetPickleById(int id)
        {
            var selectedPickle = _repository.GetById(id);
            if (selectedPickle == null)
            {
                return NotFound("No pickle matching that id was found.");
            }
            else
            {
                return Ok(selectedPickle);
            }
        }
    }
}