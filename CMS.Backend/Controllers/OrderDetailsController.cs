using Microsoft.AspNetCore.Mvc;
using CMS.Data;
using CMS.Data.Entitis;
using System.Linq;

namespace CMS.Backend.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailsController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View(_context.OrderDetails.ToList());
    }
}