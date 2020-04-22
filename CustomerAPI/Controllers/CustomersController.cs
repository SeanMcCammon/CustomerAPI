﻿using System;
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
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        /// <summary>
        /// Add new customer API endpoint
        /// </summary>
        /// <param name="customer">Customer details to record</param>
        /// <returns>Return code for either good or bad action</returns>
        [HttpPost("addcustomer")]
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

            return CreatedAtAction("GetCustomer", new {id = customer.Id}, customer);
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

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
