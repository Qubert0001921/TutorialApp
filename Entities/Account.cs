namespace EmptyTest.Entities;
public class Account : GuidEntity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}
