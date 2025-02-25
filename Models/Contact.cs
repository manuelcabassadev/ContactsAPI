using System;
using System.Collections.Generic;

namespace ContactsAPI.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string? Notes { get; set; }

    public DateTime LastUpdateDate { get; set; }

    public string? LastUpdateUserName { get; set; }
}
