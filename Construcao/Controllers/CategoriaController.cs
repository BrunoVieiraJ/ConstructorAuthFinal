using Construcao.Data;
using Construcao.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Construcao.Controllers
{
    [Authorize(Roles = "adm")]
    public class CategoriasController : Controller
    {
        private readonly DbConstrucao _context;

        public CategoriasController(DbConstrucao context)
            {
                _context = context;
            }

        public async Task<IActionResult> Index()
        {
            //VAI PERCORRER A TABELA INTEIRA E VAI MANDAR ORDENADO PRA VIEW (FAMOSO SELECT * FROM)
            return View(await _context.Categorias.OrderBy
                (c => c.Cat_Nome).ToListAsync());
        }

        //GET: CREATE
        [HttpGet]
        public IActionResult Create()
        {
            var categoria = _context.Categorias.OrderBy(i => i.Cat_Nome).ToList();
            categoria.Insert(0, new Categoria()
            {
                CategoriasId = 0,
                Cat_Nome = "Selecione a Categoria"

            });

            ViewBag.Instituicao = categoria;
            return View();
        }

        //POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categorias)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(categorias);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados");
            }
            return View(categorias);

        }

        //GET EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.SingleOrDefaultAsync
                (m => m.CategoriasId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        private bool CategoriaExists(long? id)
        {
            return _context.Categorias.Any(e => e.CategoriasId == id);
        }


        //POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!CategoriaExists(categoria.CategoriasId))
                    {
                        return NotFound();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        //DETALHES
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.SingleOrDefaultAsync
                (m => m.CategoriasId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        //GET DELETE
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .SingleOrDefaultAsync(d => d.CategoriasId == id);

            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        //POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var relacionados = _context.Produtos.Where(r => r.FK_CategoriaId == id);

            if (relacionados.Any())
            {
                string nomecategoria = string.Join(", ", relacionados.Select(d => d.Pro_Nome));
                TempData["Erro"] = $"Não é possível excluir a categoria, pois existem os produtos {nomecategoria.ToUpper()} associados a ela.";
                return RedirectToAction(nameof(Index));

            }

            var categoria = await _context.Categorias.SingleOrDefaultAsync
                    (m => m.CategoriasId == id);
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Categoria {categoria.Cat_Nome.ToUpper()} foi removida";

            return RedirectToAction(nameof(Index));
        }



    }
}
