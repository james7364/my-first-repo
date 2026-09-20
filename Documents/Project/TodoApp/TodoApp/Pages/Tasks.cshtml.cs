using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

[Authorize]
public class TasksModel : PageModel
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TasksModel(ApplicationDbContext context, UserManager<IdentityUser> UserManager)
    {
        _context = context;
        _userManager = UserManager;
    }

    public IList<TaskItem> TaskList { get; set; } = default!;

    [BindProperty]
    public TaskItem NewTask { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);

        // Base query for the logged-in user, sorted by priority
        var query = _context.TaskItems
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Priority)
            .AsQueryable();

        // Apply filters based on selection
        if (Filter == "active")
        {
            query = query.Where(t => !t.IsCompleted);
        }
        else if (Filter == "completed")
        {
            query = query.Where(t => t.IsCompleted);
        }

        TaskList = await query.ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var userId = _userManager.GetUserId(User);
        NewTask.UserId = userId!;

        // Remove validation error for UserId since we set it manually
        ModelState.Remove("NewTask.UserId");

        if (!ModelState.IsValid)
        {
            TaskList = await _context.TaskItems.Where(t => t.UserId == userId).ToListAsync();
            return Page();
        }

        _context.TaskItems.Add(NewTask);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostToggleAsync(int id)
    {
        var userId = _userManager.GetUserId(User);
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task != null)
        {
            // Flip the completion state
            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var userId = _userManager.GetUserId(User);
        var task = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (task != null)
        {
            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }

    [BindProperty(SupportsGet = true)]
    public string? Filter { get; set; } = "all";
}