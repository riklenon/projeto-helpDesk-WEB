using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjetoPim.Data;
using ProjetoPim.Models;
using ProjetoPim.Models.ManageViewModels;
using ProjetoPim.Services;

namespace ProjetoPim.Controllers
{

    [Authorize]
    public class ChamadosController : Controller
    {

        private readonly ApplicationDbContext _context;
        public UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;

        public ChamadosController(ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string ErrorMessage { get; set; }

        [Authorize(Roles ="Nivel 1")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }


       [Authorize(Roles = "Nivel 2")]
        public IActionResult IndexAtendimento()
        {
            return View();
        }

        public IActionResult IndexAtendimento2()
        {
            return View();
        }

        public IActionResult Pendentes()
        {
            return View();
        }


        public IActionResult IndexAdmin()
        {
            return View();
        }

        //GET MEUSCHAMADOS
        public async Task<IActionResult> MeusChamados()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.UserId == _userManager.GetUserId(User) && x.Status != EnumStatus.Atendido && x.Status != EnumStatus.Confirmado).ToListAsync());
        }

        //GET TODOSCHAMADOS
        public async Task<IActionResult> ListaFinalizados()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.UserId == _userManager.GetUserId(User) && x.Status == EnumStatus.Atendido || x.Status == EnumStatus.Confirmado).ToListAsync());
        }

        //GET TODOSCHAMADOS1
        public async Task<IActionResult> Rpendentes()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente || x.Status == EnumStatus.Nivel1).ToListAsync());
        }

        //GET TODOSCHAMADOS2
        public async Task<IActionResult> Rpendentes2()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente2 || x.Status == EnumStatus.Nivel2).ToListAsync());
        }

        //GET TODOSCHAMADOS
        public async Task<IActionResult> Rpendentes3()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente3 || x.Status == EnumStatus.Nivel3).ToListAsync());
        }

        //GET TODOSCHAMADOS
        public async Task<IActionResult> TodosChamados()
        {
            return View(await _context.Chamado.Include(obj => obj.User).ToListAsync());
        }

        //GET NIVEL1
        //[Authorize(Roles = "Nivel 2 ")]
        public async Task<IActionResult> ListaPendentes()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente || x.Status == EnumStatus.Nivel1).ToListAsync());
        }

        //GET NIVEL2
        public async Task<IActionResult> ListaNivel2()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente2 || x.Status == EnumStatus.Nivel2).ToListAsync());
        }

        //GET NIVEL3
        public async Task<IActionResult> ListaNivel3()
        {
            return View(await _context.Chamado.Include(obj => obj.User).Where(x => x.Status == EnumStatus.Pendente3 || x.Status == EnumStatus.Nivel3 || x.Status == EnumStatus.Confirmado).ToListAsync());
        }

        //GET ATENDIDOS
        public async Task<IActionResult> ListaAtendidos()
        {
            //LISTA DE PENDENTES
            return View(await _context.Chamado.Include(obj => obj.User).Where(x=> x.Status == EnumStatus.Atendido).ToListAsync());
        }


        // GET: Chamados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        public async Task<IActionResult> Details2(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        //GET DETALHES ATENDIMENTO
        public async Task<IActionResult> DetailsAtend(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            if(chamado.Status == EnumStatus.Nivel1 || chamado.Status == EnumStatus.Nivel2 || chamado.Status == EnumStatus.Nivel3)
            {
                return RedirectToAction(nameof(Error));
            }
            chamado.Encaminha();
            _context.Update(chamado);
            _context.SaveChanges();
            return View(chamado);
        }

        public async Task<IActionResult> Desiste(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            if (chamado.Status == EnumStatus.Nivel1 || chamado.Status == EnumStatus.Nivel2 || chamado.Status == EnumStatus.Nivel3) {
                chamado.Desiste();
                _context.Update(chamado);
                _context.SaveChanges();
                return RedirectToAction(nameof(Pendentes));
            }
            else
            {
                return RedirectToAction(nameof(Error4));
            }

        } //Ação de Desistir do Atendimento

        // GET: Chamados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chamados/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Assunto,IdVpn,SenhaVpn,Descricao,Status,Prioridade,Data")] Chamado chamado)
        {

            {


                var users = await _userManager.GetUserAsync(User);
                if (users == null)
                {
                    throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                if (ModelState.IsValid)
                {
                    _context.Add(chamado);
                    chamado.User = await _userManager.GetUserAsync(User);
                    /*if (chamado.Assunto==null)
                    {
                        return RedirectToAction(nameof(Error3));
                    }*/
                    //chamado.Assunto
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MeusChamados));
                }
                return RedirectToAction(nameof(Error3));

            } //Recupera os Dados do Último Usuário logado para Definir no chamado
        }

        // GET: Chamados/Edit/5
        /*public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamado.FindAsync(id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }*/

        // POST: Chamados/Edit/5
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Assunto,IdVpn,SenhaVpn,Descricao")] Chamado chamado)
        {
            if (id != chamado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    chamado.User = await _userManager.GetUserAsync(User);
                    chamado.Encaminha();
                    _context.Update(chamado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChamadoExists(chamado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(chamado);
        }*/

        public async Task<IActionResult> Encaminha(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamado.FindAsync(id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Encaminha(int id, [Bind("Id,Assunto,IdVpn,SenhaVpn,Descricao,Status,Prioridade")] Chamado chamado)
        {
            if (id != chamado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if(chamado.Status== EnumStatus.Nivel3)
                    {
                        return RedirectToAction(nameof(Error5));
                    }
                    if (chamado.Status != EnumStatus.Atendido || chamado.Status != EnumStatus.Confirmado || chamado.Status != EnumStatus.Atendido ||chamado.Status != EnumStatus.Pendente || chamado.Status != EnumStatus.Pendente2 || chamado.Status != EnumStatus.Pendente3)
                    {
                        chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
                        chamado.Encaminha();
                        _context.Update(chamado);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return RedirectToAction(nameof(Error7));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChamadoExists(chamado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Pendentes));
            }
            return View(chamado);
        }

        public async Task<IActionResult> Confirma(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamado.FindAsync(id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirma(int id, [Bind("Id,Assunto,IdVpn,SenhaVpn,Descricao,Status,Prioridade")] Chamado chamado)
        {
            if (id != chamado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
                    chamado.Confirma();
                    _context.Update(chamado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChamadoExists(chamado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MeusChamados));
            }
            return View(chamado);
        }

        public async Task<IActionResult> Finaliza(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var chamado = await _context.Chamado.FindAsync(id);
            if (chamado == null)
            {
                return NotFound();
            }
            return View(chamado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Finaliza(int id, [Bind("Id,Assunto,IdVpn,SenhaVpn,Descricao,Status,Prioridade")] Chamado chamado)
        {
            if (id != chamado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(chamado.Status != EnumStatus.Confirmado)
                    {
                        return RedirectToAction(nameof(Error6));
                    }
                    chamado = await _context.Chamado.Include(obj => obj.User).FirstOrDefaultAsync(obj => obj.Id == id);
                    chamado.Finaliza();
                    _context.Update(chamado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChamadoExists(chamado.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListaPendentes));
            }
            return View(chamado);
        }

        // GET: Chamados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chamado = await _context.Chamado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chamado == null)
            {
                return NotFound();
            }

            return View(chamado);
        }

        // POST: Chamados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chamado = await _context.Chamado.FindAsync(id);
            _context.Chamado.Remove(chamado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MeusChamados));
        }

        private bool ChamadoExists(int id)
        {
            return _context.Chamado.Any(e => e.Id == id);
        }


        //Ação de Error
        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "O chamado já está sendo Atendido!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        //Ação de Error
        public IActionResult Error2(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "Este chamado não foi Confirmado!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Error3(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "Preenchimento incorreto!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Error4(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "O chamado não está em atendimento!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Error5(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "O chamado encontra-se no último Nível!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Error6(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "O atendimento do chamado não foi confirmado pelo Autor!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Error7(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message = "Este chamado não pode ser encaminhado!",
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

    }
}