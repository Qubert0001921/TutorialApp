namespace EmptyTest.Exceptions;
public class BadRequestException : Exception
{
	public BadRequestException(string property, string msg) : base(msg)
	{
		Property = property;
	}

	public BadRequestException(string msg) : base(msg)
	{
		Property = string.Empty;
	}

	public string Property { get; }
}
