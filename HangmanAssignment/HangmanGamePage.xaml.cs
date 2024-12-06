namespace HangmanAssignment;

public partial class HangmanGamePage : ContentPage
{
    //dictionarry to store both the hint and its answer
    private Dictionary<string, string> wordHints = new Dictionary<string, string>
    {
        { "javascript", "Programming language responsible for the interaction of a website" },
        { "sql", "A language used to manage databases" },
        { "vpn", "A private network that helps protect its users by encrypting their data and masking their IP addresses " },
        { "python", "A versatile programming language named after a comedy group and an animal" },
        { "html", "The markup language responsible for the structure of a website" },
        { "encrypt", "changing words/code to hide their meaning and promote security" },
        { "java", "A popular object-oriented programming language" },
        { "cpu", "Is referred to as the brain of the computer" },
        {"cookies", "Information that is sent from the browser to the web server" },
        {"github", " A platform that allows users to store, share, and collaborate on code, web pages, and other content" }
    };

    //stores all the words the user guesses
    private string wordToGuess;

    //tracks the words entered
    private HashSet<char> guessedLetters = new();
    //initialized the number of attempts for each round
    private int attemptsLeft = 8;

    public HangmanGamePage()
    {
        InitializeComponent();
        ResetGame();   //constructor to reset each round with a new hint
    }

    private string currentHint;

    private void ResetGame()
    {
        //set to randomly pick the word and hint from the dictionary
        Random random = new Random();
        int index = random.Next(wordHints.Count);
        var selectedWord = wordHints.ElementAt(index);

        wordToGuess = selectedWord.Key;
        currentHint = selectedWord.Value;

        guessedLetters.Clear();
        attemptsLeft = 8;

        // Reset UI components including the image if the wrong word is entered
        HangmanImage.Source = "hang1.png";
        FeedbackLabel.Text = string.Empty;
        AttemptsLabel.Text = "Attempts Remaining: 7";
        HintLabel.Text = $"Hint: {currentHint}";
        WordDisplay.Text = GenerateWordDisplay(wordToGuess.Length);
    }

    // Generate underscores based on the word length for relevance
    private string GenerateWordDisplay(int length)
    {
        return string.Join(" ", new string('_', length).ToCharArray());
    }



    //to udpate the words a user entered so they fill up the underscores
    private void UpdateWordDisplay()
    {
        WordDisplay.Text = string.Join(" ", wordToGuess.Select(c => guessedLetters.Contains(c) ? c : '_'));
    }

    private void OnGuessSubmitted(object sender, EventArgs e)
    {
        ProcessGuess();
    }

    private void OnGuessButtonClicked(object sender, EventArgs e)
    {
        ProcessGuess();
    }

    private void ProcessGuess()
    {
        // Trim whitespace and ensure input is lowercase and conditions to ensure the right structure
        string input = GuessInput.Text?.Trim().ToLower();

        if (string.IsNullOrEmpty(input))
        {
            FeedbackLabel.Text = "Please enter a valid letter.";
            return;
        }

        if (input.Length != 1 || !char.IsLetter(input[0]))
        {
            FeedbackLabel.Text = "Please enter a single letter.";
            return;
        }

        char guessedLetter = input[0];

        // Check if the letter was already guessed 
        if (guessedLetters.Contains(guessedLetter))
        {
            FeedbackLabel.Text = "You already guessed that letter.";
            return;
        }

        // Process the guess
        guessedLetters.Add(guessedLetter);

        if (wordToGuess.Contains(guessedLetter))
        {
            FeedbackLabel.Text = "Correct Guess!";
            UpdateWordDisplay();

            // Checks if the player has won and display messege
            if (wordToGuess.All(c => guessedLetters.Contains(c)))
            {
                FeedbackLabel.Text = "Congratulations! You won.";
            }
        }
        else
        {
            attemptsLeft--;
            FeedbackLabel.Text = "Incorrect Guess!";
            HangmanImage.Source = $"hang{8 - attemptsLeft}.png";
            AttemptsLabel.Text = $"Attempts Remaining: {attemptsLeft}";

            // Check if the player has lost and display message
            if (attemptsLeft == 0)
            {
                FeedbackLabel.Text = $"Game Over! You died, The word was: {wordToGuess.ToUpper()}";
            }
        }

        
        GuessInput.Text = string.Empty;
    }


    private void OnResetGameClicked(object sender, EventArgs e)
    {
        ResetGame();
    }


}