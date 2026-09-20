using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public EditModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public TaskItem? TaskItem { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = _userManager.GetUserId(User);
        TaskItem = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (TaskItem == null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (TaskItem == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);

        var taskToUpdate = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == TaskItem.Id && t.UserId == userId);

        if (taskToUpdate == null)
        {
            return NotFound();
        }

        taskToUpdate.Title = TaskItem.Title;
        taskToUpdate.Description = TaskItem.Description;
        taskToUpdate.Priority = TaskItem.Priority;
        taskToUpdate.ColorHex = TaskItem.ColorHex;

        await _context.SaveChangesAsync();

        return RedirectToPage("./Tasks");
    }
}