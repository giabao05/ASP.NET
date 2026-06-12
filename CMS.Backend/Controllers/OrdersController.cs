using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrdersController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.Orders.ToList());
    }
}