namespace ContactBook
{
    public class ContactBook
    {
        public const string NEXT_PAGE = "+";
        public const string PREV_PAGE = "-";
        private const string GOTO_PAGE = "G";
        private const string PAGE_SIZE = "S";
        private const string CREATE_CONTACT = "C";
        private const string REVIEW_CONTACT = "R";
        private const string UPDATE_CONTACT = "U";
        private const string DELETE_CONTACT = "D";
        private const string FIND_CONTACTS = "F";
        private const string ORDER_CONTACTS = "O";
        private const string DEDUPLICATE_CONTACTS = "M";
        private const string EXIT = "E";

        public readonly string[] COMANDS = new string[]
        {
            NEXT_PAGE, PREV_PAGE, GOTO_PAGE, PAGE_SIZE,
            CREATE_CONTACT, REVIEW_CONTACT, UPDATE_CONTACT, DELETE_CONTACT,
            FIND_CONTACTS, ORDER_CONTACTS, DEDUPLICATE_CONTACTS, EXIT
        };

        private List<Contact> allContacts;
        private List<Contact> currentView;
        private int currentPage = 0;
        private int pageSize = 10;
        private bool exitRequested = false;

        private readonly ContactMerger merger = new ContactMerger();

        public ContactBook(List<Contact> contacts = null!)
        {
            allContacts = contacts ?? new List<Contact>();
            currentView = new List<Contact>(allContacts);
        }

        public void Start()
        {
            ShowWelcomeScreen();

            string input;
            do
            {
                Console.Clear();
                ShowContacts();

                do
                {
                    ShowInputOptions();
                    input = GetInput();
                }
                while (!IsValidInput(input));

                ProcessInput(input);
            }
            while (!exitRequested);

            ShowExitScreen();
        }

        //Display methods

        private void ShowWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Contact Book!");

            PressEnterToContinue();
        }

        private void ShowExitScreen()
        {
            Console.Clear();
            Console.WriteLine("Goodbye! Thanks for using Contact Book.");
        }

        private void ShowContacts()
        {
            int totalPages = TotalPages();

            const int W_NUM   = 4;
            const int W_FNAME = 15;
            const int W_LNAME = 15;
            const int W_PHONE = 16;
            const int W_EMAIL = 28;

            string sep = new string('─', W_NUM + 1 + W_FNAME + 1 + W_LNAME + 1 + W_PHONE + 1 + W_EMAIL + 2);

            Console.WriteLine($"── Contacts (Page {currentPage + 1}/{Math.Max(1, totalPages)}) ──────────────");
            Console.WriteLine(sep);
            Console.WriteLine(
            $"{"#",-W_NUM} " +
            $"{"First name",-W_FNAME} " +
            $"{"Last name",-W_LNAME} " +
            $"{"Phone",-W_PHONE} " +
            $"{"Email",-W_EMAIL}");
            Console.WriteLine(sep);

            if (currentView.Count == 0)
            {
                Console.WriteLine("  (no contacts found)");
            }
            else
            {
                var page = GetCurrentPage();
                int startIndex = currentPage * pageSize;

                for (int i = 0; i < page.Count; i++)
                {
                    string num   = (startIndex + i + 1).ToString();
                    string fname = Truncate(page[i].GetFName(), W_FNAME);
                    string lname = Truncate(page[i].GetLName(), W_LNAME);
                    string phone = Truncate(page[i].GetPhone(), W_PHONE);
                    string email = Truncate(page[i].GetEmail(), W_EMAIL);

                    Console.WriteLine(
                    $"{num,-W_NUM} " +
                    $"{fname,-W_FNAME} " +
                    $"{lname,-W_LNAME} " +
                    $"{phone,-W_PHONE} " +
                    $"{email,-W_EMAIL}");
                }
            }

            Console.WriteLine(sep);
        }

        private static string Truncate(string value, int maxWidth)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return value.Length <= maxWidth ? value : value[..(maxWidth - 1)] + "…";
        }

        private void ShowInputOptions()
        {
            Console.WriteLine();
            Console.WriteLine($"[{NEXT_PAGE}] Next  [{PREV_PAGE}] Prev  [{GOTO_PAGE}] Go to page  [{PAGE_SIZE}] Page size ({pageSize})");
            Console.WriteLine($"[{CREATE_CONTACT}] Create  [{REVIEW_CONTACT}] Review  [{UPDATE_CONTACT}] Update  [{DELETE_CONTACT}] Delete");
            Console.WriteLine($"[{FIND_CONTACTS}] Find  [{ORDER_CONTACTS}] Order  [{DEDUPLICATE_CONTACTS}] Deduplicate  [{EXIT}] Exit");
            Console.Write("Command: ");
        }

        //Input handling

        private string GetInput()
        {
            return (Console.ReadLine() ?? "").Trim().ToUpper();
        }

        private bool IsValidInput(string input)
        {
            if (!COMANDS.Contains(input))
            {
                Console.WriteLine($"  Invalid command. Please enter one of: {string.Join(", ", COMANDS)}");
                return false;
            }
            return true;
        }

        //Command processing

        private void ProcessInput(string input)
        {
            switch (input)
            {
                case "+": NextPage();            break;
                case "-": PrevPage();            break;
                case "G": GoToPage();            break;
                case "S": ChangePageSize();      break;
                case "C": CreateContact();       break;
                case "R": ReviewContact();       break;
                case "U": UpdateContact();       break;
                case "D": DeleteContact();       break;
                case "F": FindContacts();        break;
                case "O": OrderContacts();       break;
                case "M": DeduplicateContacts(); break;
                case "E": ConfirmExit();         break;
            }
        }

        //Pagination methods

        private int TotalPages() =>
            (int)Math.Ceiling(currentView.Count / (double)pageSize);

        private List<Contact> GetCurrentPage() =>
            currentView.Skip(currentPage * pageSize).Take(pageSize).ToList();

        private void NextPage()
        {
            if ((currentPage + 1) < TotalPages()) currentPage++;
            else Console.WriteLine("  Already on the last page.");
        }

        private void PrevPage()
        {
            if (currentPage > 0) currentPage--;
            else Console.WriteLine("  Already on the first page.");
        }

        private void GoToPage()
        {
            Console.Write($"  Go to page (1-{Math.Max(1, TotalPages())}): ");
            if (int.TryParse(Console.ReadLine(), out int page) && page >= 1 && page <= TotalPages())
                currentPage = page - 1;
            else
                Console.WriteLine("  Invalid page number.");
        }

        private void ChangePageSize()
        {
            Console.Write("  New page size (1-50): ");
            if (int.TryParse(Console.ReadLine(), out int size) && size >= 1 && size <= 50)
            {
                pageSize = size;
                currentPage = 0;
            }
            else Console.WriteLine("  Invalid size.");
        }

        //CRUD Operations

        private void CreateContact()
        {
            Console.Write("  First name: ");
            string fname = Console.ReadLine() ?? "";
            Console.Write("  Last name: ");
            string lname = Console.ReadLine() ?? "";
            Console.Write("  Phone: ");
            string phone = Console.ReadLine() ?? "";
            Console.Write("  Email: ");
            string email = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(fname) && string.IsNullOrWhiteSpace(lname))
            {
                Console.WriteLine("  Name cannot be empty.");
                PressEnterToContinue();
                return;
            }

            allContacts.Add(new Contact(fname, lname, phone, email));
            RefreshView();
            Console.WriteLine("  Contact created.");
            PressEnterToContinue();
        }

        private void ReviewContact()
        {
            Contact? c = SelectContact("review");
            if (c == null) return;

            Console.WriteLine($"\n  First name: {c.GetFName()}");
            Console.WriteLine($"  Last name:  {c.GetLName()}");
            Console.WriteLine($"  Phone:      {c.GetPhone()}");
            Console.WriteLine($"  Email:      {c.GetEmail()}");
            PressEnterToContinue();
        }

        private void UpdateContact()
        {
            Contact? c = SelectContact("update");
            if (c == null) return;

            Console.Write($"  First name [{c.GetFName()}]: ");
            string fname = Console.ReadLine() ?? "";
            Console.Write($"  Last name [{c.GetLName()}]: ");
            string lname = Console.ReadLine() ?? "";
            Console.Write($"  Phone [{c.GetPhone()}]: ");
            string phone = Console.ReadLine() ?? "";
            Console.Write($"  Email [{c.GetEmail()}]: ");
            string email = Console.ReadLine() ?? "";

            if (!string.IsNullOrWhiteSpace(fname)) c.SetFName(fname);
            if (!string.IsNullOrWhiteSpace(lname)) c.SetLName(lname);
            if (!string.IsNullOrWhiteSpace(phone)) c.SetPhone(phone);
            if (!string.IsNullOrWhiteSpace(email)) c.SetEmail(email);

            Console.WriteLine("  Contact updated.");
            PressEnterToContinue();
        }

        private void DeleteContact()
        {
            Contact? c = SelectContact("delete");
            if (c == null) return;

            string fullName = $"{c.GetFName()} {c.GetLName()}".Trim();
            Console.Write($"  Delete \"{fullName}\"? (Y/N): ");
            if ((Console.ReadLine() ?? "").Trim().ToUpper() == "Y")
            {
                allContacts.Remove(c);
                RefreshView();
                if (currentPage >= TotalPages() && currentPage > 0) currentPage--;
                Console.WriteLine("  Contact deleted.");
            }
            else Console.WriteLine("  Cancelled.");
            PressEnterToContinue();
        }

        //Search order and deduplication

        private void FindContacts()
        {
            Console.Write("  Search (leave blank to reset): ");
            string query = (Console.ReadLine() ?? "").Trim().ToLower();

            currentView = string.IsNullOrEmpty(query) ? new List<Contact>(allContacts) 
            : allContacts.Where(c => c.GetFName().ToLower().Contains(query) || c.GetLName().ToLower().Contains(query) || c.GetPhone().ToLower().Contains(query) || c.GetEmail().ToLower().Contains(query)).ToList();

            currentPage = 0;
            Console.WriteLine($"  {currentView.Count} contact(s) found.");
            PressEnterToContinue();
        }

        private void OrderContacts()
        {
            Console.WriteLine("  Order by: [F] First name  [L] Last name  [P] Phone  [E] Email");
            Console.Write("  Choice: ");
            string fieldChoice = (Console.ReadLine() ?? "").Trim().ToUpper();

            Console.WriteLine("  Order: [A] Ascending  [D] Descending");
            Console.Write("  Choice: ");
            string orderChoice = (Console.ReadLine() ?? "").Trim().ToUpper();

            ContactComparer.SortField field = fieldChoice switch
            {
                "F" => ContactComparer.SortField.FirstName,
                "L" => ContactComparer.SortField.LastName,
                "P" => ContactComparer.SortField.Phone,
                "E" => ContactComparer.SortField.Email,
                _   => ContactComparer.SortField.FirstName
            };

            ContactComparer.SortOrder order = orderChoice == "D" ? ContactComparer.SortOrder.Descending : ContactComparer.SortOrder.Ascending;

            allContacts.Sort(new ContactComparer(field, order));
            RefreshView();
            Console.WriteLine("  Contacts ordered.");
            PressEnterToContinue();
        }

        private void DeduplicateContacts()
        {
            List<List<Contact>> groups = merger.FindDuplicateGroups(allContacts);

            if (groups.Count == 0)
            {
                Console.WriteLine("  No duplicates found.");
                PressEnterToContinue();
                return;
            }

            int totalRemoved = 0;

            foreach (List<Contact> group in groups)
            {
                Console.Clear();
                Console.WriteLine("Duplicate group found");

                //Shows Contacts 
                for (int i = 0; i < group.Count; i++)
                {
                    Contact c     = group[i];
                    string name   = $"{c.GetFName()} {c.GetLName()}".Trim();
                    string phone  = string.IsNullOrWhiteSpace(c.GetPhone()) ? "(no phone)" : c.GetPhone();
                    string email  = string.IsNullOrWhiteSpace(c.GetEmail()) ? "(no email)" : c.GetEmail();
                    Console.WriteLine($"  [{i + 1}] {name,-22} {phone,-18} {email}");
                }

                Console.WriteLine();
                Console.WriteLine("  [A] Auto-merge (keep most complete)");
                Console.WriteLine("  [#] Enter number to keep manually");
                Console.WriteLine("  [S] Skip this group");
                Console.Write("  Choice: ");

                string choice = (Console.ReadLine() ?? "").Trim().ToUpper();

                if (choice == "S")
                {
                    Console.WriteLine("  Skipped.");
                    continue;
                }

                Contact? toKeep = null;

                if (choice == "A")
                {
                    //Saves the better duplicate by fields with information
                    toKeep = group.OrderByDescending(c =>
                        (string.IsNullOrWhiteSpace(c.GetFName()) ? 0 : 1) +
                        (string.IsNullOrWhiteSpace(c.GetLName()) ? 0 : 1) +
                        (string.IsNullOrWhiteSpace(c.GetPhone()) ? 0 : 1) +
                        (string.IsNullOrWhiteSpace(c.GetEmail()) ? 0 : 1)
                    ).First();
                }
                else if (int.TryParse(choice, out int pick) && pick >= 1 && pick <= group.Count)
                {
                    toKeep = group[pick - 1];
                }
                else
                {
                    Console.WriteLine("  Invalid choice, skipping group.");
                    PressEnterToContinue();
                    continue;
                }

                //Eliminates duplicates
                foreach (Contact c in group)
                {
                    if (!ReferenceEquals(c, toKeep))
                    {
                        allContacts.Remove(c);
                        totalRemoved++;
                    }
                }

                string keptName = $"{toKeep.GetFName()} {toKeep.GetLName()}".Trim();
                Console.WriteLine($"  Kept: {keptName}");
                PressEnterToContinue();
            }

            RefreshView();
            Console.WriteLine($"\n  Done. Removed {totalRemoved} duplicate(s).");
            PressEnterToContinue();
        }

        //ExitScreen

        private void ConfirmExit()
        {
            Console.Write("  Are you sure you want to exit? (Y/N): ");
            if ((Console.ReadLine() ?? "").Trim().ToUpper() == "Y")
                exitRequested = true;
        }

        //Helpers

        private void RefreshView()
        {
            currentView = new List<Contact>(allContacts);
            currentPage = 0;
        }

        private Contact? SelectContact(string action)
        {
            if (currentView.Count == 0)
            {
                Console.WriteLine("  No contacts to select.");
                PressEnterToContinue();
                return null;
            }

            Console.Write($"  Enter contact number to {action} (1-{currentView.Count}): ");
            if (int.TryParse(Console.ReadLine(), out int num) && num >= 1 && num <= currentView.Count)
                return currentView[num - 1];

            Console.WriteLine("  Invalid selection.");
            PressEnterToContinue();
            return null;
        }

        private void PressEnterToContinue()
        {
            Console.WriteLine("  Press Enter to continue...");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) ;
        }
    }
}