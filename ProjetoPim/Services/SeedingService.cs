using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoPim.Models;
using ProjetoPim.Data;
using Microsoft.AspNetCore.Identity;

namespace ProjetoPim.Services
{
    public class SeedingService
    {
        private ApplicationDbContext _context;
        public SeedingService(ApplicationDbContext context)
        {
            _context = context;
        }

        //Alimenta o Banco com o Usuário ADMIN
        public void Seed()
        {

        }
    }
}