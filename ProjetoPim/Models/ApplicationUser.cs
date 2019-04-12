using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoPim.Models.Enums;

namespace ProjetoPim.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Nome { get; set; }
        public ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
        public EnumDepartamento Departamento { get; set; }
        public string Cpf { get; set; }
        //public string RoleName { get; set; }



        public void AddChamado(Chamado chamado)
        {
            Chamados.Add(chamado);
        }

        // private readonly ApplicationDbContext _context;
    }
}

