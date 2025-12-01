using Expenses.App.API.Database;
using Expenses.App.API.Dtos.CategoryTags;
using Expenses.App.API.Entities;
using Expenses.App.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expenses.App.API.Controllers;

[ApiController]
[Route("category/{categoryId}/tags")]
[Authorize(Roles = Roles.Member)]
public sealed class CategoryTagController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpPut]
    public async Task<ActionResult> UpsertCategoryTag(string categoryId, UpsertCategoryTagsDto upsertCategoryTagsDto)
    {
        Category? category = await dbContext.Categories
                                            .Include(c => c.CategoryTags)
                                            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (category is null)
        {
            return NotFound();
        }

        var currentCategoryIds = category.CategoryTags.Select(ct => ct.TagId).ToHashSet();

        if (currentCategoryIds.SetEquals(upsertCategoryTagsDto.TagIds))
        {
            return NoContent();
        }

        List<string> existingTagIds = await dbContext.Tags.Where(t => upsertCategoryTagsDto.TagIds.Contains(t.Id))
                                                          .Select(t => t.Id)
                                                          .ToListAsync();

        if (existingTagIds.Count != upsertCategoryTagsDto.TagIds.Count)
        {
            return BadRequest("One or more tag IDS is Invalid");
        }

        category.CategoryTags.RemoveAll(ct => !upsertCategoryTagsDto.TagIds.Contains(ct.TagId));

        string[] tagIdsToAdd = upsertCategoryTagsDto.TagIds.Except(currentCategoryIds).ToArray();

        category.CategoryTags.AddRange(tagIdsToAdd.Select(tagId => new CategoryTag
        {
            CategoryId = categoryId,
            TagId = tagId,
            CreatedAtUtc = DateTime.UtcNow,
        }));

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteCategoryTag(string categoryId, string tagId)
    {
        CategoryTag? categoryTag = await dbContext.CategoryTags
                                                  .SingleOrDefaultAsync(ct => ct.CategoryId == categoryId && ct.TagId == tagId);

        if (categoryTag is null)
        {
            return NotFound();
        }

        dbContext.CategoryTags.Remove(categoryTag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
