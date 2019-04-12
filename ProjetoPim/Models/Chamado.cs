using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjetoPim.Models
{
    public class Chamado
    {
        public Chamado()
        {

        }

        public Chamado(int id, string assunto, string descricao, string idVpn, string senhaVpn, ApplicationUser user, DateTime data)
        {
            Id = id;
            Assunto = assunto;
            Descricao = descricao;
            IdVpn = idVpn;
            SenhaVpn = senhaVpn;
            User = user;
            Data = data;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = " O campo é obrigatório !")]
        [StringLength(20, ErrorMessage = "O campo deve ter no mínimo 20 caracteres !", MinimumLength = 10)]
        public string Assunto { get; set; }

        [Required(ErrorMessage = " O campo é obrigatório !")]
        [StringLength(200, ErrorMessage = "O campo deve ter no mínimo 20 caracteres!", MinimumLength = 20)]
        public string Descricao { get; set; }

        [StringLength(9, ErrorMessage = "O campo deve ter 9 caracteres!", MinimumLength = 9)]
        public string IdVpn { get; set; }

        [StringLength(4, ErrorMessage = "O campo deve ter 4 caracteres!", MinimumLength = 4)]
        public string SenhaVpn { get; set; }

        [Required]
        public Prioridade Prioridade { get; set; }


        public EnumStatus Status { get; set; }


        public string UserId { get; set; }


        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        public void Encaminha()
        {
            Status = Status + 1;
        }

        public void Confirma()
        {
            Status = EnumStatus.Confirmado;
        }

        public void Finaliza()
        {
            Status = EnumStatus.Atendido;
        }

        public void Desiste()
        {
            Status = Status -1;
        }
    }

}

public enum EnumStatus
{   
    Pendente,
    [Display(Name = "Em Atendimento Nível 1")]
    Nivel1,
    [Display(Name = "Pendente")]
    Pendente2,
    [Display(Name = "Em Atendimento Nível 2")]
    Nivel2,
    [Display(Name = "Pendente")]
    Pendente3,
    [Display(Name = "Em Atendimento Nível 3")]
    Nivel3,
    [Display(Name = "Confirmado")]
    Confirmado,
    [Display(Name = "Atendido")]
    Atendido
}

public enum Prioridade
{
    Não,
    Sim
}