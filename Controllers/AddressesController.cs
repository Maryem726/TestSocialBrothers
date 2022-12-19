using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

namespace Test.Controllers
{

    [Route("Addresses")]
    [ApiController]
    public class AddressesController : Controller

    {
        private readonly DataContext _context;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(ILogger<AddressesController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }


        //getAll
        [Route("")]
        [Route("~/")]
        [HttpGet]
        public ActionResult Index()
        {
            var values = _context.Address.ToList();
            return View(values);
            return Ok(values);
        }


        //filter
        [Route("filter")]
        [HttpGet]
        public async Task<IActionResult> Index(string filter, string sortOrder)
        {
           
            ViewData["CurrentFilter"] = filter;
            ViewData["CodeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "code_desc" : "" ;
            ViewData["HouseNumberSort"] = sortOrder == "HouseNumber" ? "number_desc" : sortOrder.ToString();
            ViewData["CitySort"] = sortOrder == "City" ? "city_desc" : "City";
            ViewData["CountrySort"] = sortOrder == "Country" ? "country_desc" : "Country";
            ViewData["StreetSort"] = sortOrder == "Street" ? "street_desc" : "Street";
            var adresses = from a in _context.Address
                           select a;
            if (!String.IsNullOrEmpty(filter))
            {
                adresses = adresses.Where(a => a.Country.Contains(filter) || a.City.Contains(filter) || a.Street.Contains(filter) || a.ZipCode.ToString().Contains(filter) || a.HouseNumber.ToString().Contains(filter));

            }
       
            adresses = sortOrder switch
            {
                "code_desc" => adresses.OrderByDescending(a => a.ZipCode),

              //  "HouseNumber" => adresses.OrderBy(a => a.HouseNumber),
                "number_desc" => adresses.OrderByDescending(a => a.HouseNumber),
                //"City" => adresses.OrderBy(a => a.City),
                "city_desc" => adresses.OrderByDescending(a => a.City),
               // "Country" => adresses.OrderBy(a => a.Country),
                "country_desc" => adresses.OrderByDescending(a => a.Country),
              //  "Street" => adresses.OrderBy(a => a.Street),
                "street_desc" => adresses.OrderByDescending(a => a.Street),
                _ => adresses.OrderBy(a =>sortOrder),
            };


           // return View(adresses);
            return View(await adresses.AsNoTracking().ToListAsync());

        }


        //getByID
        [Route("{Id}")]
        [HttpGet]
        public ActionResult Details(int Id)
        {
            Address address = _context.Address.Find(Id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
            return Ok(address);
        }



        //Update
           [HttpPut("{Id}")]
         // [Route("/{id}")]
        public async Task<IActionResult> Update(int Id, Address add)
           {
           var address = await _context.Address.FindAsync(Id);
            //Address address = await _context.Address.FindAsync(Id);

            if (address == null)
            {
                return NotFound();
            }
            address.HouseNumber = add.HouseNumber;
            address.ZipCode = add.ZipCode;
            address.City = add.City;
            address.Country = add.Country;
            address.Street = add.Street;
            _context.Address.Update(address);
                    _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return Ok(address);
        }

   
           
        //ADD
         [HttpPost]
         public async Task<ActionResult> AddAddress(Address _newAddress)
         {
            var _Address = new Address
                    {
                        HouseNumber = _newAddress.HouseNumber,
                        ZipCode = _newAddress.ZipCode,
                        City = _newAddress.City,
                        Country = _newAddress.Country,
                        Street = _newAddress.Street

                     };
                    if ((_newAddress.HouseNumber != null) &&
                        (_newAddress.ZipCode != null) &&
                        (_newAddress.City != null) &&
                        (_newAddress.Country != null) &&
                      (_newAddress.Street != null))
                    {
                        _context.Address.Add(_Address);

                        _context.SaveChanges();

                //return RedirectToAction(nameof(Index));
                return Ok(_Address);
                    }
                    else
                    {
                        return View(_Address);
                    }
                    
          }


       
        //Delete
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            
           var  address = await _context.Address.FindAsync(Id);
            if(address == null)
            {
                return NotFound();
            }
            _context.Address.Remove(address);
            _context.SaveChanges();
            return Ok(_context.Address.ToListAsync());


        }
      


    }
        
}
