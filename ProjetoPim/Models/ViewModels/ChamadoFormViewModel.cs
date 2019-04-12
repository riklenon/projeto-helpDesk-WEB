using System.Collections.Generic;


namespace ProjetoPim.Models
{
    public class ChamadoViewModel
    {
        public Chamado Chamado { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
