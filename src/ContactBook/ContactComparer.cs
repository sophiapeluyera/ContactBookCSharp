namespace ContactBook;

public class ContactComparer : IComparer<Contact>
{
    public enum SortField { FirstName, LastName, Phone, Email }
    public enum SortOrder { Ascending, Descending }

    private readonly SortField field;
    private readonly SortOrder order;

    public ContactComparer(SortField field = SortField.FirstName, SortOrder order = SortOrder.Ascending)
    {
        this.field = field;
        this.order = order;
    }

    public int Compare(Contact? x, Contact? y)
    {
        if (x is null && y is null) return 0;
        if (x is null) return -1;
        if (y is null) return 1;

        string valX = field switch
        {
            SortField.FirstName => x.GetFName(),
            SortField.LastName  => x.GetLName(),
            SortField.Phone     => x.GetPhone(),
            SortField.Email     => x.GetEmail(),
            _                   => x.GetFName()
        };

        string valY = field switch
        {
            SortField.FirstName => y.GetFName(),
            SortField.LastName  => y.GetLName(),
            SortField.Phone     => y.GetPhone(),
            SortField.Email     => y.GetEmail(),
            _                   => y.GetFName()
        };

        int result = string.Compare(valX, valY, StringComparison.OrdinalIgnoreCase);

        return order == SortOrder.Descending ? -result : result;
    }
}