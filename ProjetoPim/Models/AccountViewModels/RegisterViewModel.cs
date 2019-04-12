using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoPim.Data;
using ProjetoPim.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProjetoPim.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(25, ErrorMessage = "O campo deve ter no máximo 25 caracteres e no mínimo 6 !", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "As senhas não estão de acordo.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "O campo deve conter 12 Dígitos: (DDD)+Número !", MinimumLength = 11)]
        [Display(Name = "Telefone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required]
        [StringLength(11, ErrorMessage = "O campo deve ter 11 caracteres numéricos!", MinimumLength = 11)]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Departamento")]
        [UIHint("List")]
        public List<SelectListItem> Departments { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Acesso")]
        [UIHint("List")]
        public List<SelectListItem> Roles { get; set; }
        public string Role { get; set; }

        [UIHint("List")]
        public  EnumDepartamento Departamentos { get; set; }
       // public List<SelectListItem> Departamentos { get; set; }

        

        public RegisterViewModel()
        {
            //Departamentos = new List<SelectListItem>();
            Nome = Nome;
            Roles = new List<SelectListItem>();
        }


        public void getRoles(ApplicationDbContext _context)
        {
            var roles = from r in _context.identityRole select r;
            var listRole = roles.ToList();
            foreach (var Data in listRole)
            {
                Roles.Add(new SelectListItem(){
                    Value = Data.Id,
                    Text = Data.Name
                });
            }
        }
    }
}
