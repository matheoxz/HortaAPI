using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using HortaIoT.Models;
using HortaIoT.Repository;

namespace HortaIoT.Controllers
{
    [ApiController]
    [Route("data")]
    public class DataController : ControllerBase
    {
        private IDataRepository _repository;

        public DataController(IDataRepository repository){
            _repository = repository;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            List<DataModel> result = new List<DataModel>();
            try{
                result = _repository.GetAll();
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpGet("getrange")]
        public ActionResult GetRange([FromQuery] DateTimeOffset start, [FromQuery] DateTimeOffset end)
        {
            List<DataModel> result = new List<DataModel>();
            try{
                result = _repository.GetRange(start, end);
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpGet("getcultivation")]
        public ActionResult GetByCultivation([FromQuery] string cultivationName)
        {
            List<DataModel> result = new List<DataModel>();
            try{
                result = _repository.GetByCultivation(cultivationName);
                return Ok(result);
            }catch(KeyNotFoundException){
                return NotFound();
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }    
        }

        [HttpPost("add")]
        public ActionResult Add([FromBody] DataModel data)
        {
            
            try{
                _repository.Add(data);
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok();
        }

        
    }
}
