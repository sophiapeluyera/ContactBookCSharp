namespace ContactBook;

public class Program
{
    public static void Main()
    {
        var cb = new ContactBook(ContactSeed.Contacts);
        cb.Start();
    }
}