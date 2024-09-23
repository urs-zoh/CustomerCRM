using Microsoft.AspNetCore.Mvc;
using CustomerCRM.Models;

namespace CustomerCRM.Controllers;

public class CustomersController : Controller
{

        private static readonly List<Customer> Customers = [];

        // Index action to list all customers
        public IActionResult Index(string sortOrder, string searchString)
        {
            var customerList = Customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                customerList = customerList.Where(c => c.FirstName.Contains(searchString) || c.LastName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "firstName_asc":
                    customerList = customerList.OrderBy(c => c.FirstName);
                    break;
                case "lastName_asc":
                    customerList = customerList.OrderBy(c => c.LastName);
                    break;
                default:
                    customerList = customerList.OrderBy(c => c.Id);
                    break;
            }

            return View(customerList.ToList());
        }

        // Details action to display customer by ID
        public IActionResult Details(int id)
        {
            var customer = Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // Create GET - show form to create customer
        public IActionResult Create()
        {
            return View();
        }

        // Create POST - process the form data and add new customer
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (!ModelState.IsValid) return View(customer);
            customer.Id = Customers.Count > 0 ? Customers.Max(c => c.Id) + 1 : 1;
            Customers.Add(customer);
            return RedirectToAction(nameof(Index));
        }

        // Edit GET - show form to edit customer
        public IActionResult Edit(int id)
        {
            var customer = Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // Edit POST - process form data and update customer
        [HttpPost]
        public IActionResult Edit(int id, Customer customer)
        {
            var existingCustomer = Customers.FirstOrDefault(c => c.Id == id);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                existingCustomer.FirstName = customer.FirstName;
                existingCustomer.LastName = customer.LastName;
                existingCustomer.Email = customer.Email;
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Delete GET - confirm customer deletion
        public IActionResult Delete(int id)
        {
            var customer = Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // Delete POST - process customer deletion
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var customer = Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                Customers.Remove(customer);
            }
            return RedirectToAction(nameof(Index));
        }
    }