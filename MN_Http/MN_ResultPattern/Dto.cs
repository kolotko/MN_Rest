namespace MN_ResultPattern;

public class UserDto
{
    public string? Name { get; set; }
    public string? SurName { get; set; }
    public int? Age { get; set; }
    public Profession? Profession { get; set; }
}

public class Profession
{
    public string? Name { get; set; }
}