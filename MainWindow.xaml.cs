using System;
using System.Numerics;
using System.Windows;
using Accessibility;

namespace CreditorReferenceGenerator;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnGenerateClick(object sender, RoutedEventArgs e)
    {
        try
        {
            if (UserInput.Text.Length is < 1 or > 21)
            {
                ErrorLabel.Content = "Fehler! Referenznummer muss mind. 1 oder max 21 Zeichen lang sein";
                ResultBox.Text = "";
                return;
            }

            ErrorLabel.Content = "";
            var creditorReference = GenerateCreditorReference(UserInput.Text);
            ResultBox.Text = creditorReference;
        }
        catch (Exception exception)
        {
            ErrorLabel.Content = exception.Message;
            ResultBox.Text = "";
        }

    }

    private static string GenerateCreditorReference(string input)
    {
        // assuming 01 are the check digits
        const string checkDigits = "01";
        // RF converted to numbers
        const int rfNumber = 2715;

        var checkedInput = string.Empty;

        foreach (var c in input)
        {
            var isNumber = int.TryParse(c.ToString(), out _);
            if (!isNumber) checkedInput += GetNumberForChar(c.ToString());
            else checkedInput += c;
      
        }

        if (checkedInput.Length > 21)
        {
            throw new ArgumentException("Referenz ist zu lange, Buchstaben brauchen 2 Zeichen");
        }

        var reference = BigInteger.Parse($"{checkedInput}{rfNumber}{checkDigits}");
        var modulo = reference % 97;
        // set modulo to correct check digits
        if (modulo != 1) modulo = 99 - modulo;
        return $"{modulo:00}{checkedInput}";

    }

    private static string GetNumberForChar(string character)
    {
        return character switch
        {
            "a" => "10",
            "A" => "10",
            "B" => "11",
            "b" => "11",
            "c" => "12",
            "C" => "12",
            "d" => "13",
            "D" => "13",
            "e" => "14",
            "E" => "14",
            "f" => "15",
            "F" => "15",
            "g" => "16",
            "G" => "16",
            "h" => "17",
            "H" => "17",
            "i" => "18",
            "I" => "18",
            "j" => "19",
            "J" => "19",
            "k" => "20",
            "K" => "20",
            "l" => "21",
            "L" => "21",
            "m" => "22",
            "M" => "22",
            "n" => "23",
            "N" => "23",
            "o" => "24",
            "O" => "24",
            "p" => "25",
            "P" => "25",
            "q" => "26",
            "Q" => "26",
            "r" => "27",
            "R" => "27",
            "s" => "28",
            "S" => "28",
            "t" => "29",
            "T" => "29",
            "u" => "30",
            "U" => "30",
            "v" => "31",
            "V" => "31",
            "w" => "32",
            "W" => "32",
            "x" => "33",
            "X" => "33",
            "y" => "34",
            "Y" => "34",
            "z" => "35",
            "Z" => "35",
            _ => throw new ArgumentException("Ungültiges Zeichen in Referenznummer!")
        };
    }
}