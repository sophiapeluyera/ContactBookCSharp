namespace ContactBook;

public class ContactBook
{

 public const string NEXT_PAGE = "+";
 public const string PREV_PAGE = "-";
 public const string GOTO_PAGE = "G";
 public const string PAGE_SIZE = "S";
 public const string CREATE_CONTACT = "C";
 public const string REVIEW_CONTACT = "R";
 public const string UPDATE_CONTACT = "U";
 public const string DELETE_CONTACT = "D";
 public const string FIND_CONTACTS = "F";
 public const string ORDER_CONTACTS = "O";
 public const string DEDUPLICATE_CONTACTS = "M";
 public const string EXIT = "X";

  public readonly string[] COMMANDS =new string[]
    {
        NEXT_PAGE,
        PREV_PAGE,
        GOTO_PAGE,
        PAGE_SIZE,
        CREATE_CONTACT,
        REVIEW_CONTACT,
        UPDATE_CONTACT,
        DELETE_CONTACT,
        FIND_CONTACTS,
        ORDER_CONTACTS,
        DEDUPLICATE_CONTACTS,
        EXIT
    };

    public ContactBook()
    {
        
    }

    public void Start()
    {
        ShowWelcomeScreen();

        string input;

        do
        {
            ShowContacts();

            do
            {
                ShowInputOptions();
                input = GetInput();

            }
            while (!IsVAlidInput(input));

            ProcessInput(input);
        }
        while(!ConfirmExit());    

        ShowExitScreen();
    }

    private string GetInput()
    {
        return " ";
    }

    private void ShowWelcomeScreen()
    {
        Console.WriteLine("Welcome to Soph's Contact Book!");
        PressEnterToContinue();
    }

    private void ShowContacts()
    {
        
    }

    private void ShowInputOptions()
    {
        
    }

    private bool IsVAlidInput(object input)
    {
        return true;
    }

    private void ProcessInput(object input)
    {
     
    }

    private bool ConfirmExit()
    {
        return true;
    }

    private void ShowExitScreen()
    {
       
    }

    private void PressEnterToContinue()
    {
        Console.WriteLine("Press ENTER to continue...");
        while(Console.ReadKey(true).Key != ConsoleKey.Enter);
    }
}