using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoPim.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoPim.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Departamento")]
        [UIHint("List")]
        public EnumDepartamento Departamentos { get; set; }

        public string StatusMessage { get; set; }


        public void updateNome(string nome)
        {
            Nome = nome;
        }

       // public List<IndexViewModel> FindAll()
    }
}
