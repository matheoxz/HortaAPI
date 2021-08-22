using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System;
using HortaIoT.Models;
using HortaIoT.Repository;

namespace HortaIoT.Controllers
{
    [ApiController]
    [Route("cultivation")]
    public class CultivationController : ControllerBase
    {
        private ICultivationRepository _repository;

        public CultivationController(ICultivationRepository repository){
            _repository = repository;
        }

        [HttpGet("getall")]
        public ActionResult GetAll()
        {
            List<CultivationModel> result = new List<CultivationModel>();
            try{
                result = _repository.GetAll();
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpGet("get")]
        public ActionResult GetByName([FromQuery] string name)
        {
            CultivationModel result = new CultivationModel();
            try{
                result = _repository.GetByName(name)??
                    throw new KeyNotFoundException();
            }catch(KeyNotFoundException){
                return NotFound();
            }
            catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        [HttpPost("add")]
        public ActionResult Add([FromBody] CultivationModel data)
        {
            
            try{
                _repository.Add(data);
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut("update")]
        public ActionResult Update([FromQuery] string name, [FromBody] CultivationModel data)
        {
            
            try{
                _repository.Update(name, data);
            }catch(KeyNotFoundException){
                return NotFound();
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("delete")]
        public ActionResult Delete([FromQuery] string name)
        {
            
            try{
                _repository.Delete(name);
            }catch(KeyNotFoundException){
                return NotFound();
            }catch (Exception e){
                return BadRequest(e.Message);
            }

            return Ok();
        }

        
    }
}
