using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vuture.Exceptions.ExceptionResponses;
using Vuture.Models.Dtos;
using Vuture.Validation;

namespace Vuture.Test.Unit.Validation
{
    public class TestModelValidation
    {
        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        [TestCase(null, null, null, null, null, null)]
        [TestCase("", "", "", "", "", "")]
        [TestCase("", "", "", null, null, null)]
        [TestCase(null, null, null, "", "", "")]
        public void TestValidateCreateContactDto_FailTest(string? firstName, string? lastName, string? emailAddress, string? company, string? status, string? title)
        {
            CreateContactDto dto = new CreateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Company = company,
                Status = status,
                Title = title
            };
            try
            {
                ModelValidation.ValidateCreateContactDto(dto);
            }
            catch (BadRequestExceptionResponse ex)
            {
                Assert.Pass();
            }
            catch (SuccessException ex)
            {
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
        }

        [Test]
        [TestCase("a", "a", "a", "a", "a", "a")]
        [TestCase("b", "b", "b", "b", "b", "b")]
        [TestCase("c", "c", "c", "c", "c", "c")]
        [TestCase("d", "d", "d", "d", "d", "d")]
        [TestCase("e", "e", "e", "e", "e", "e")]
        public void TestValidateCreateContactDto_PassTest(string firstName, string lastName, string emailAddress, string company, string status, string title)
        {
            CreateContactDto dto = new CreateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Company = company,
                Status = status,
                Title = title
            };
            try
            {
                ModelValidation.ValidateCreateContactDto(dto);
            }
            catch (BadRequestExceptionResponse ex)
            {
                Assert.Fail();
            }
            catch (SuccessException ex)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }

        [Test]
        [TestCase(null, null, null, null, null, null)]
        [TestCase("", "", "", "", "", "")]
        [TestCase("", "", "", null, null, null)]
        [TestCase(null, null, null, "", "", "")]
        public void TestValidateUpdateContactDto_FailTest(string? firstName, string? lastName, string? emailAddress, string? company, string? status, string? title)
        {
            UpdateContactDto dto = new UpdateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Company = company,
                Status = status,
                Title = title
            };
            try
            {
                ModelValidation.ValidateUpdateContactDto(dto);
            }
            catch (BadRequestExceptionResponse ex)
            {
                Assert.Pass();
            }
            catch (SuccessException ex)
            {
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.Pass();
            }
        }

        [Test]
        [TestCase("a", "a", "a", "a", "a", "a")]
        [TestCase("b", "b", "b", "b", "b", "b")]
        [TestCase("c", "c", "c", "c", "c", "c")]
        [TestCase("d", "d", "d", "d", "d", "d")]
        [TestCase("e", "e", "e", "e", "e", "e")]
        public void TestValidateUpdateContactDto_PassTest(string firstName, string lastName, string emailAddress, string company, string status, string title)
        {
            UpdateContactDto dto = new UpdateContactDto()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                Company = company,
                Status = status,
                Title = title
            };
            try
            {
                ModelValidation.ValidateUpdateContactDto(dto);
            }
            catch (BadRequestExceptionResponse ex)
            {
                Assert.Fail();
            }
            catch (SuccessException ex)
            {
                Assert.Pass();
            }
            catch (Exception ex)
            {
                Assert.Fail();
            }
        }
    }
}
