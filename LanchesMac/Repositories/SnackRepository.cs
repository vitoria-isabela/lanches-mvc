using LanchesMac.Context;
using LanchesMac.Models;
using LanchesMac.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanchesMac.Repositories
{
    public class SnackRepository : ISnackRepository
    {
        private readonly AppDbContext _context;

        public SnackRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<Snack> Snacks => _context.Snacks.Include(c => c.Category);

        public IEnumerable<Snack> FavoriteSnacks => _context.Snacks.Where(c => c.IsFavoriteSnack).Include(c => c.Category);

        public Snack GetSnackById(int snackId)
        {
            return _context.Snacks.FirstOrDefault(c => c.SnackId == snackId);
        }
    }
}
