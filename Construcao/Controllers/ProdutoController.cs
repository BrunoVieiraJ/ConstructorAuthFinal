using Construcao.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Construcao.Models;
using Microsoft.AspNetCore.Authorization;

namespace Construcao.Controllers
{
    [Authorize(Roles = "adm, User")]
    public class ProdutoController : Controller
    {
        private readonly DbConstrucao _context;
        
        public ProdutoController(DbConstrucao context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            //VAI PERCORRER A TABELA DEPARTAMENTO INTEIRA E VAI MANDAR ORDENADO PRA VIEW (FAMOSO SELECT * FROM)
            var produtos = await _context.Produtos
                .Include(i => i.Categorias)
                .OrderBy(d => d.Pro_Nome)
                .ToListAsync();
            return View(produtos);
        }

        //GET: DEPARTAMENTO/CREATE
        [HttpGet]
        public IActionResult Create()
        {
            var categorias = _context.Categorias.OrderBy(i => i.Cat_Nome).ToList();
            categorias.Insert(0, new Categoria()
            {
                CategoriasId = 0,
                Cat_Nome = "Selecione a Categorias"

            });

            ViewBag.Categorias = categorias;

            return View();
        }

        //POST DEPARTAMENTO/CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(produto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Erro", "Não foi possível inserir os dados");
            }
            return View(produto);
        }

        //GET EDIT
        [HttpGet]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.SingleOrDefaultAsync
                (m => m.ProdutoId == id);

            if (produto == null)
            {
                return NotFound();
            }

            ViewBag.Categorias = new SelectList(_context.Categorias.OrderBy(b => b.Cat_Nome),
                "CategoriasId", "Cat_Nome", produto.FK_CategoriaId);

            return View(produto);
        }

        private bool ProdutosExists(long? id)
        {
            return _context.Produtos.Any(e => e.ProdutoId == id);
        }


        //POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Produto produto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    if (!ProdutosExists(produto.ProdutoId))
                    {
                        return NotFound();
                    }
                }

                ViewBag.Categorias = new SelectList(_context.Categorias.OrderBy(b => b.Cat_Nome),
                "CategoriasId", "Nome", produto.FK_CategoriaId);

                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        //DETALHES
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.SingleOrDefaultAsync
                (m => m.ProdutoId == id);

            _context.Categorias.Where(i => produto.FK_CategoriaId == i.CategoriasId).Load();

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        //GET DELETE
        [HttpGet]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos
                .SingleOrDefaultAsync(d => d.ProdutoId == id);

            _context.Categorias.Where(i => produto.FK_CategoriaId == i.CategoriasId).Load();

            if (produto == null)
            {
                return NotFound();
            }
            return View(produto);
        }

        //POST DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var produto = await _context.Produtos.SingleOrDefaultAsync
                (m => m.ProdutoId == id);
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"Produto {produto.Pro_Nome.ToUpper()} foi removido";

            return RedirectToAction(nameof(Index));
        }



    }
}
