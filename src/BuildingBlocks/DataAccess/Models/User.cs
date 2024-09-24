using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }

    public Guid? SubscriptionId { get; set; }

    public string? ImgUrl { get; set; }

    public DateTime? Dob { get; set; }

    public string? Address { get; set; }
}
