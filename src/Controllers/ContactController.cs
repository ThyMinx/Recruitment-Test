using Microsoft.AspNetCore.Mvc;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Models.Dtos;
using Vuture.Services;
using Vuture.Validation;

namespace Vuture.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetContactById(int id)
        {
            try
            {
                var contact = _contactService.GetContactById(id);
                return new JsonResult(contact);
            }
            catch (NotFoundRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (BadRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (Exception ex)
            {
                //Log exception here
                throw ex;
            }
            //throw new NotImplementedException();
        }

        [HttpPost]
        [Route("")]
        public ActionResult<ReadContactDto> CreateContact([FromBody] CreateContactDto createContactDto)
        {
            try
            {
                ModelValidation.ValidateCreateContactDto(createContactDto);
                ReadContactDto readContactDto = _contactService.CreateContact(createContactDto);
                return new ActionResult<ReadContactDto>(readContactDto);
            }
            catch (NotFoundRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (BadRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (Exception ex)
            {
                //Log exception here
                throw ex;
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<ReadContactDto> UpdateContactById(int id, UpdateContactDto updateContactDto)
        {
            try
            {
                ModelValidation.ValidateUpdateContactDto(updateContactDto);
                ReadContactDto readContactDto = _contactService.UpdateContactById(id, updateContactDto);
                return new JsonResult(readContactDto);
            }
            catch (NotFoundRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (BadRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (Exception ex)
            {
                //Log exception here
                throw ex;
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteContactById(int id)
        {
            try
            {
                _contactService.DeleteContactById(id);
                return new StatusCodeResult(StatusCodes.Status200OK);
            }
            catch (NotFoundRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (BadRequestExceptionResponse ex)
            {
                //Log exception here
                return new StatusCodeResult(ex.StatusCode);
                throw ex;
            }
            catch (Exception ex)
            {
                //Log exception here
                throw ex;
            }
        }
    }
}