using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string currentEquation = "";
        public float previousAnswer = 0;
        public static string numberCapture = "(\\d+(,\\d+)?)";
        public static string LastAnswerVariable = "A";
        public Regex stringOperations = new($"{numberCapture}([-+]){numberCapture}");
        public Regex dotOperations = new($"{numberCapture}([\\*\\/]){numberCapture}");
        public Regex brackets = new($"\\({numberCapture}\\)");
        public Regex ARegex = new(LastAnswerVariable);
        public Regex digitWithA = new($"([\\d{LastAnswerVariable}]){LastAnswerVariable}");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addCharacter(char character) {
            string operations = "+*/-,";
            if (currentEquation.Length > 0 && operations.Contains(character) && operations.Contains(currentEquation[currentEquation.Length - 1])) {
                return;
            }

            currentEquation += character;
            TextboxOutput.Text = currentEquation;
        }

        private void addCharacter(string character) {
            string operations = "+*/-,";
            if (currentEquation.Length > 0 && operations.Contains(character) && operations.Contains(currentEquation[currentEquation.Length - 1])) {
                return;
            }

            currentEquation += character;
            TextboxOutput.Text = currentEquation;
        }

        private string solveEquation(string equation)
        {
            string current = (string)equation.Clone();

            Match? digitWithVariableAMatch = null;
            do
            {
                digitWithVariableAMatch = digitWithA.Match(current);
                current = digitWithA.Replace(current, (Match match) => match.Groups[1].Value + "*" + LastAnswerVariable);
            } while (digitWithVariableAMatch.Success);

            while (current.Contains(LastAnswerVariable))
            {
                current = ARegex.Replace(current, previousAnswer.ToString());
            }

            while (true)
            {
                // do the dot operations
                Match dotOperationsMatch = dotOperations.Match(current);
                if (dotOperationsMatch.Success)
                {
                    // "3*6" => "18"
                    current = dotOperations.Replace(current, (Match match) => {
                        float first = float.Parse(match.Groups[1].Value);
                        float second = float.Parse(match.Groups[4].Value);
                        if (match.Groups[3].Value == "/")
                        {
                            return (first / second).ToString();
                        }
                        return (first * second).ToString();
                    });
                    continue;
                }

                // do the string operations
                Match stringOperationsMatch = stringOperations.Match(current);
                if (stringOperationsMatch.Success)
                {
                    // "45+83" => "128"
                    current = stringOperations.Replace(current, (Match match) => {
                        float first = float.Parse(match.Groups[1].Value);
                        float second = float.Parse(match.Groups[4].Value);
                        if (match.Groups[3].Value == "-")
                        {
                            return (first - second).ToString();
                        }
                        return (first + second).ToString();
                    });
                    continue;
                }

                //TODO: "x(y)" => "x*y"
                Match bracketsMatch = brackets.Match(current);
                if (bracketsMatch.Success)
                {
                    // "(83)" => "83"
                    current = brackets.Replace(current, (Match match) => match.Groups[1].Value);
                    continue;
                }

                return current;
            }
        }

        #region ButtonEvents


        private void ButtonSolve_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string solution = solveEquation(currentEquation);

                previousAnswer = float.Parse(solution);
                TextboxOutput.Text = currentEquation + "\nSolution: " + solution;
            } catch (Exception ex)
            {

                TextboxOutput.Text = currentEquation + "\n" + ex.Message;
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('1');
        }

        private void ButtonDecimal_Click(object sender, RoutedEventArgs e)
        {
            addCharacter(',');
        }

        private void Button0_Click(object sender, RoutedEventArgs e) 
        {
            addCharacter('0');
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('6');
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('5');
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('7');
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('8');
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('9');
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('+');
        }

        private void ButtonSubtract_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('-');
        }

        private void ButtonMultiply_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('*');
        }

        private void ButtonDivide_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('/');
        }

        private void ButtonVarAnswer_Click(object sender, RoutedEventArgs e)
        {
            addCharacter(LastAnswerVariable);
        }

        private void ButtonBracketClose_Click(object sender, RoutedEventArgs e)
        {
            addCharacter(')');
        }

        private void ButtonBracketOpen_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('(');
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            currentEquation = "";
            TextboxOutput.Text = currentEquation;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('2');
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('3');
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            addCharacter('4');
        }

        #endregion
    }
}
