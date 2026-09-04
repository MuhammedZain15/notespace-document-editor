using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project2.Models;
using project2.ViewModels;
using System.Security.Claims;

namespace project2.Controllers;

[Authorize]
public class DocsController : Controller
{
    private readonly AppDbContext _context;
    public DocsController(AppDbContext context) => _context = context;

    public async Task<IActionResult> Index(string? search, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = _context.Docs.AsNoTracking().Where(doc => doc.UserId == userId);
        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(doc => doc.Title.Contains(search.Trim()));

        ViewData["Search"] = search;
        return View(await query.OrderByDescending(doc => doc.Id).ToListAsync(token));
    }

    [HttpGet]
    public IActionResult Create() => View(new DocumentInputViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DocumentInputViewModel request, CancellationToken token)
    {
        if (!ModelState.IsValid) return View(request);
        var document = new Doc
        {
            Title = request.Title.Trim(),
            Content = request.Content,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)!
        };
        _context.Docs.Add(document);
        await _context.SaveChangesAsync(token);
        TempData["Success"] = "Document created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id, CancellationToken token)
    {
        var document = await FindOwnedDocumentAsync(id, token);
        if (document is null) return NotFound();
        return View(new DocumentInputViewModel { Id = document.Id, Title = document.Title, Content = document.Content });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DocumentInputViewModel request, CancellationToken token)
    {
        if (id != request.Id) return BadRequest();
        if (!ModelState.IsValid) return View(request);
        var document = await FindOwnedDocumentAsync(id, token);
        if (document is null) return NotFound();

        document.Title = request.Title.Trim();
        document.Content = request.Content;
        await _context.SaveChangesAsync(token);
        TempData["Success"] = "Document updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id, CancellationToken token)
    {
        var document = await FindOwnedDocumentAsync(id, token);
        return document is null ? NotFound() : View(document);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken token)
    {
        var document = await FindOwnedDocumentAsync(id, token);
        if (document is null) return NotFound();
        _context.Docs.Remove(document);
        await _context.SaveChangesAsync(token);
        TempData["Success"] = "Document deleted.";
        return RedirectToAction(nameof(Index));
    }

    private Task<Doc?> FindOwnedDocumentAsync(int id, CancellationToken token)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        return _context.Docs.FirstOrDefaultAsync(doc => doc.Id == id && doc.UserId == userId, token);
    }
}
