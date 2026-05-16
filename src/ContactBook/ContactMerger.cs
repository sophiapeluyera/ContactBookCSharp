namespace ContactBook;

public class ContactMerger
{
    private class DuplicateSet
    {
        private readonly int[] parents;
        private readonly int[] rank;

        public DuplicateSet(int size)
        {
            parents = new int[size];
            rank    = new int[size];

            for (int i = 0; i < size; i++)
            parents[i] = i;
        }

        public int Find(int x)
        {
            if (parents[x] != x)
            parents[x] = Find(parents[x]);
            return parents[x];
        }

        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if (rootX == rootY) return;

            if (rank[rootX] < rank[rootY])      parents[rootX] = rootY;
            else if (rank[rootX] > rank[rootY]) parents[rootY] = rootX;
            else { parents[rootY] = rootX; rank[rootX]++; }
        }

        public bool Connected(int x, int y) => Find(x) == Find(y);
    }

    //Public interface

    public List<Contact> Merge(List<Contact> contacts)
    {
        int n = contacts.Count;
        if (n <= 1) return new List<Contact>(contacts);

        var ds = BuildDuplicateSet(contacts);

        var groups = new Dictionary<int, List<int>>();
        for (int i = 0; i < n; i++)
        {
            int root = ds.Find(i);
            if (!groups.ContainsKey(root))
                groups[root] = new List<int>();
            groups[root].Add(i);
        }

        var result = new List<Contact>();
        foreach (var group in groups.Values)
        {
            Contact best = contacts[group[0]];
            for (int k = 1; k < group.Count; k++)
            {
                Contact candidate = contacts[group[k]];
                if (CountFilledFields(candidate) > CountFilledFields(best))
                 best = candidate;
            }
            result.Add(best);
        }

        return result;
    }

    public List<List<Contact>> FindDuplicateGroups(List<Contact> contacts)
    {
        int n = contacts.Count;
        if (n <= 1) return new List<List<Contact>>();

        var ds = BuildDuplicateSet(contacts);

        var groups = new Dictionary<int, List<Contact>>();
        for (int i = 0; i < n; i++)
        {
            int root = ds.Find(i);
            if (!groups.ContainsKey(root))
                groups[root] = new List<Contact>();
            groups[root].Add(contacts[i]);
        }

        // Solo devolver grupos con más de un contacto
        return groups.Values.Where(g => g.Count > 1).ToList();
    }

    //Helpers

    private DuplicateSet BuildDuplicateSet(List<Contact> contacts)
    {
        int n = contacts.Count;
        var ds = new DuplicateSet(n);

        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                if (AreDuplicates(contacts[i], contacts[j]))
                    ds.Union(i, j);

        return ds;
    }

    private static bool AreDuplicates(Contact a, Contact b)
    {
        bool aHasPhone = !string.IsNullOrWhiteSpace(a.GetPhone());
        bool bHasPhone = !string.IsNullOrWhiteSpace(b.GetPhone());
        bool aHasEmail = !string.IsNullOrWhiteSpace(a.GetEmail());
        bool bHasEmail = !string.IsNullOrWhiteSpace(b.GetEmail());

        bool samePhone = aHasPhone && bHasPhone
        && string.Equals(a.GetPhone(), b.GetPhone(), StringComparison.OrdinalIgnoreCase);

        bool sameEmail = aHasEmail && bHasEmail
        && string.Equals(a.GetEmail(), b.GetEmail(), StringComparison.OrdinalIgnoreCase);

        bool sameName = string.Equals(a.GetFName(), b.GetFName(), StringComparison.OrdinalIgnoreCase)
        && string.Equals(a.GetLName(), b.GetLName(), StringComparison.OrdinalIgnoreCase);

        bool bothEmpty = !aHasPhone && !bHasPhone && !aHasEmail && !bHasEmail;

        return sameEmail || samePhone || (sameName && bothEmpty);
    }

    private static int CountFilledFields(Contact c)
    {
        int count = 0;
        if (!string.IsNullOrWhiteSpace(c.GetFName())) count++;
        if (!string.IsNullOrWhiteSpace(c.GetLName())) count++;
        if (!string.IsNullOrWhiteSpace(c.GetPhone())) count++;
        if (!string.IsNullOrWhiteSpace(c.GetEmail())) count++;
        return count;
    }
}