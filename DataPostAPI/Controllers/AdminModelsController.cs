using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataPostAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace DataPostAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminModelsController : ControllerBase
    {
        private readonly ClientContext _context;

        public AdminModelsController(ClientContext context)
        {
            _context = context;
        }

        // GET: api/AdminModels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminModel>>> GetAdmins()
        {
            return await _context.Admins.ToListAsync();
        }

        // GET: api/AdminModels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminModel>> GetAdminModel(int id)
        {
            var adminModel = await _context.Admins.FindAsync(id);

            if (adminModel == null)
            {
                return NotFound();
            }

            return adminModel;
        }

        // PUT: api/AdminModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdminModel(int id, AdminModel adminModel)
        {
            if (id != adminModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(adminModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AdminModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdminModel>> PostAdminModel(AdminModel adminModel)
        {
            _context.Admins.Add(adminModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdminModel", new { id = adminModel.Id }, adminModel);
        }

        // DELETE: api/AdminModels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdminModel(int id)
        {
            var adminModel = await _context.Admins.FindAsync(id);
            if (adminModel == null)
            {
                return NotFound();
            }

            _context.Admins.Remove(adminModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdminModelExists(int id)
        {
            return _context.Admins.Any(e => e.Id == id);
        }
    }
}
