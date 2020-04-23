using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerAPI.Models;

namespace CustomerAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerDbContext _context;

        public CustomersController(CustomerDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            //return await _context.Customers.ToListAsync();
            return Ok();
        }

        /// <summary>
        /// Returns a list of all existing customers
        /// </summary>
        /// <returns></returns>
        [HttpGet("ViewAllCustomers")]
        public async Task<ActionResult<IEnumerable<Customer>>> ViewAllCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        /// <summary>
        /// Method to allow retrieval and viewing of a customer details
        /// </summary>
        /// <param name="id">Id of the customer to retrieve details of</param>
        /// <returns></returns>
        //[HttpGet("ViewCustomerDetails")]
        //public async Task<ActionResult<Customer>> ViewCustomerDetails(long id)
        //{
        //    Customer customer = await _context.Customers.FindAsync(id);

        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return customer;
        //}

        /// <summary>
        /// Method to allow deletion of a customer - if we are required to do so
        /// </summary>
        /// <param name="id">Id of cutomer to delete</param>
        /// <returns></returns>
        [HttpDelete("DeleteCustomer")]
        public async Task<ActionResult<Customer>> DeleteCustomer(Customer customer)
        {
            Customer customerToDelete = await _context.Customers.FindAsync(customer.Id);
            if (customerToDelete == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customerToDelete);
            await _context.SaveChangesAsync();

            return customer;
        }

        /// <summary>
        /// Called if required to update details of a customer- name maybe incorrect
        /// </summary>
        /// <param name="customer">Customer details with updates</param>
        /// <returns></returns>
        [HttpPost("UpdateCustomer")]
        public async Task<ActionResult<Customer>> UpdateCustomer(Customer customer)
        {
            Customer customerToUpdate = await _context.Customers.FindAsync(customer.Id);

            if (customerToUpdate == null)
            {
                return NotFound();
            }

            customerToUpdate.Surname = customer.Surname;
            customerToUpdate.FirstName = customer.FirstName;
            customerToUpdate.Policy = customer.Policy;
            customerToUpdate.EMail = customer.EMail;
            customerToUpdate.DOB = customer.DOB;

            await _context.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Add new customer API endpoint
        /// </summary>
        /// <param name="customer">Customer details to record</param>
        /// <returns>Return code for either good or bad action</returns>
        [HttpPost("AddCustomer")]
        public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
        {
            if (!string.IsNullOrEmpty(customer.EMail) && !ValidateEmail(customer.EMail))
            {
                // Bad request if email does not match requirements
                return BadRequest();
            }

            // If a DOBis passed in - validate it
            if (customer.DOB != null)
            {
                // If not over 18 return a bad request
                if (!ValidateAge(customer.DOB))
                {
                    return BadRequest();
                }
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            //Return just the customer Id as per spec
            return CreatedAtAction("AddCustomer", new { id = customer.Id }, customer.Id);

            //Below return would return entire customer data including the customer ID if spcification changed after meeting
            //return CreatedAtAction("AddCustomer", new {id = customer.Id}, customer);
        }

        /// <summary>
        /// Vaidate email to ensure matches with criteria of requirements and not just looks like valid email.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        private bool ValidateEmail(string emailAddress)
        {
            string regExValidateString = @"^([a-zA-Z0-9_\-\.]{4,})@([a-zA-Z0-9_\-\.]{2,})\.((com|co.uk))$"; 

            var isValidEmail = Regex.Match(emailAddress, regExValidateString);

            return isValidEmail.Success;
        }

        /// <summary>
        /// Simple method to check the DateTime past in against todays date time and validate age
        /// of customer is over 18 years. 
        /// </summary>
        /// <param name="dob"></param>
        /// <returns></returns>
        private bool ValidateAge(DateTime dob)
        {
            bool validDOB = false;
            DateTime todayDateTime = DateTime.Today;

            TimeSpan differenSpan = todayDateTime.Subtract(dob);

            double yearOld = differenSpan.TotalDays / 365;

            if (yearOld > 18)
                validDOB = true;

            return validDOB;
        }

        /// <summary>
        /// Customer Exists validation on Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
