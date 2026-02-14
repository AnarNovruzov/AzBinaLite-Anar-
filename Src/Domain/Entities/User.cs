using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; set; }
}
