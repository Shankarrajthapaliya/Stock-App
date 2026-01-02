using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web.DTO
{
    public class CommentSummaryDTO
    {
    public string? Title { get; set; } = string.Empty;
    public string? Content { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    }
}