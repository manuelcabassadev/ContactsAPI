using System;
using System.Collections.Generic;

namespace ContactsAPI.Models;

public partial class Contact
{
    public int ContactId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Fax { get; set; } = null!;

    public string eMail { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public DateTime LastUpdateDate { get; set; }

    public string LastUpdateUserName { get; set; } = null!;
}
