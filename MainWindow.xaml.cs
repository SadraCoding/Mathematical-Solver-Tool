using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MathematicsSolverTool
{
    public partial class MainWindow : Window
    {
        private int score = 0;
        private int timeRemaining = 60;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            ShowTools();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }

        private void ShowTools()
        {
            ToolsGrid.Visibility = Visibility.Visible;
            GameGrid.Visibility = Visibility.Collapsed;
            startButton.Visibility = Visibility.Collapsed;
            restartButton.Visibility = Visibility.Collapsed;
            submitButton.Visibility = Visibility.Collapsed;
            timer.Stop();
        }

        private void ShowGame()
        {
            ToolsGrid.Visibility = Visibility.Collapsed;
            GameGrid.Visibility = Visibility.Visible;
            startButton.Visibility = Visibility.Visible;
            restartButton.Visibility = Visibility.Collapsed;
            submitButton.Visibility = Visibility.Collapsed;
            Hint1.Visibility = Visibility.Visible;
            Hint2.Visibility = Visibility.Collapsed;
            equationLabel.Content = "";
            answerTextBox.Text = "";
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            ShowTools();
        }

        private void Games_Click(object sender, RoutedEventArgs e)
        {
            ShowGame();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timeRemaining--;
            updateTimerLabel();
            if (timeRemaining == 0)
            {
                timer.Stop();
                submitButton.IsEnabled = false;
                startButton.Visibility = Visibility.Collapsed;
                restartButton.Visibility = Visibility.Visible;
                MessageBox.Show("Time's up! Your score: " + score);
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            timeRemaining = 60;
            updateScoreLabel();
            updateEquationLabel();
            updateTimerLabel();
            answerTextBox.Text = "";
            submitButton.IsEnabled = true;
            submitButton.Visibility = Visibility.Visible;
            startButton.Visibility = Visibility.Collapsed;
            restartButton.Visibility = Visibility.Collapsed;
            Hint1.Visibility = Visibility.Collapsed;
            Hint2.Visibility = Visibility.Visible;
            timer.Start();
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            startButton_Click(sender, e);
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(answerTextBox.Text, out int answer))
            {
                if (evaluateEquation() == answer)
                {
                    score++;
                }
                else
                {
                    score = Math.Max(0, score - 1);
                }
                updateScoreLabel();
                updateEquationLabel();
                answerTextBox.Text = "";
            }
        }

        private int evaluateEquation()
        {
            string equation = equationLabel.Content.ToString();
            string[] parts = equation.Split(' ');
            int num1 = int.Parse(parts[0]);
            int num2 = int.Parse(parts[2]);
            return parts[1] switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                _ => throw new InvalidOperationException("Invalid operator: " + parts[1]),
            };
        }

        private void updateScoreLabel()
        {
            scoreLabel.Content = "Score: " + score;
        }

        private void updateEquationLabel()
        {
            var random = new Random();
            int num1 = random.Next(25, 450);
            int num2 = random.Next(25, 450);
            int op = random.Next(3);
            string opString = op switch
            {
                0 => "+",
                1 => "-",
                _ => "*",
            };
            equationLabel.Content = $"{num1} {opString} {num2}";
        }

        private void updateTimerLabel()
        {
            timeLabel.Content = "   Time remaining: " + timeRemaining;
        }

        // Custom input dialog method
        private string GetInput(string prompt, string title)
        {
            var inputDialog = new Window()
            {
                Title = title,
                Height = 150,
                Width = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize,
                Topmost = true
            };
            var stackPanel = new StackPanel { Margin = new Thickness(10) };
            var label = new Label { Content = prompt };
            var textBox = new TextBox { Height = 23, Margin = new Thickness(0, 5, 0, 5) };
            var button = new Button { Content = "OK", Width = 75, HorizontalAlignment = HorizontalAlignment.Right };
            string result = null;
            button.Click += (s, e) =>
            {
                result = textBox.Text;
                inputDialog.Close();
            };
            textBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    result = textBox.Text;
                    inputDialog.Close();
                }
            };
            stackPanel.Children.Add(label);
            stackPanel.Children.Add(textBox);
            stackPanel.Children.Add(button);
            inputDialog.Content = stackPanel;
            // Set focus to textbox
            textBox.Focus();
            inputDialog.ShowDialog();
            return result;
        }

        // Hypotenuse tool
        private void Hypotenuse_Click(object sender, RoutedEventArgs e)
        {
            string aInput = GetInput("Please enter side a:", "Side a");
            if (aInput == null) return;
            string bInput = GetInput("Please enter side b:", "Side b");
            if (bInput == null) return;
            if (!double.TryParse(aInput, out double a))
            {
                MessageBox.Show("Please enter a valid number");
                return;
            }
            if (!double.TryParse(bInput, out double b))
            {
                MessageBox.Show("Please enter a valid number");
                return;
            }
            double hyp = Math.Sqrt(a * a + b * b);
            MessageBox.Show($"The hypotenuse is {hyp:F2}");
        }

        // Equation of two unknowns tool
        private void Equation_of_two_unknowns_Click(object sender, RoutedEventArgs e)
        {
            string aInput = GetInput("Please enter coefficient of x for first equation:", "Input");
            if (aInput == null) return;
            string bInput = GetInput("Please enter coefficient of y for first equation:", "Input");
            if (bInput == null) return;
            string cInput = GetInput("Please enter the sum of first equation:", "Input");
            if (cInput == null) return;
            string dInput = GetInput("Please enter coefficient of x for second equation:", "Input");
            if (dInput == null) return;
            string eInput = GetInput("Please enter coefficient of y for second equation:", "Input");
            if (eInput == null) return;
            string fInput = GetInput("Please enter the sum of second equation:", "Input");
            if (fInput == null) return;
            if (!double.TryParse(aInput, out double a) ||
                !double.TryParse(bInput, out double b) ||
                !double.TryParse(cInput, out double c) ||
                !double.TryParse(dInput, out double d) ||
                !double.TryParse(eInput, out double eVal) ||
                !double.TryParse(fInput, out double f))
            {
                MessageBox.Show("Please enter valid numbers");
                return;
            }
            double delta = a * eVal - b * d;
            double deltaX = c * eVal - b * f;
            double deltaY = a * f - c * d;
            if (delta != 0)
            {
                double x = deltaX / delta;
                double y = deltaY / delta;
                MessageBox.Show($"x = {x:F2}\ny = {y:F2}");
            }
            else if (delta == 0 && deltaX == 0 && deltaY == 0)
            {
                MessageBox.Show("Infinite solutions exist!");
            }
            else
            {
                MessageBox.Show("No solution exists!");
            }
        }

        // Second degree equation (quadratic)
        private void Second_degree_equation_Click(object sender, RoutedEventArgs e)
        {
            string aInput = GetInput("Please enter coefficient of x²:", "Input");
            if (aInput == null) return;
            string bInput = GetInput("Please enter coefficient of x:", "Input");
            if (bInput == null) return;
            string cInput = GetInput("Please enter the constant value:", "Input");
            if (cInput == null) return;
            if (!double.TryParse(aInput, out double a) ||
                !double.TryParse(bInput, out double b) ||
                !double.TryParse(cInput, out double c))
            {
                MessageBox.Show("Please enter valid numbers");
                return;
            }
            double discriminant = b * b - 4 * a * c;
            if (discriminant > 0)
            {
                double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                MessageBox.Show($"Two real roots: {root1:F2} and {root2:F2}");
            }
            else if (discriminant == 0)
            {
                double root = -b / (2 * a);
                MessageBox.Show($"One real root: {root:F2}");
            }
            else
            {
                double realPart = -b / (2 * a);
                double imagPart = Math.Sqrt(-discriminant) / (2 * a);
                MessageBox.Show($"Complex roots: {realPart:F2} ± {imagPart:F2}i");
            }
        }

        // Area of regular polygon
        private void Area_Click(object sender, RoutedEventArgs e)
        {
            string nInput = GetInput("Number of sides:", "Input");
            if (nInput == null) return;
            string sInput = GetInput("Length of each side:", "Input");
            if (sInput == null) return;
            if (!int.TryParse(nInput, out int n) ||
                !double.TryParse(sInput, out double s))
            {
                MessageBox.Show("Please enter valid inputs");
                return;
            }
            double area = (n * s * s) / (4 * Math.Tan(Math.PI / n));
            MessageBox.Show($"Area of regular polygon: {area:F2}");
        }

        // LCM and GCD
        private void LCMGCD_Click(object sender, RoutedEventArgs e)
        {
            string num1Str = GetInput("Please enter first number:", "Input");
            if (num1Str == null) return;
            string num2Str = GetInput("Please enter second number:", "Input");
            if (num2Str == null) return;
            if (!long.TryParse(num1Str, out long num1) || !long.TryParse(num2Str, out long num2))
            {
                MessageBox.Show("Please enter valid integers");
                return;
            }
            long gcdValue = GCD(num1, num2);
            long lcmValue = LCM(num1, num2, gcdValue);
            MessageBox.Show($"GCD: {gcdValue}");
            MessageBox.Show($"LCM: {lcmValue}");
        }

        private static long GCD(long a, long b)
        {
            if (b == 0) return a;
            return GCD(b, a % b);
        }

        private static long LCM(long a, long b, long gcd)
        {
            return (a * b) / gcd;
        }

        // Nth root calculation
        private void Root_Click(object sender, RoutedEventArgs e)
        {
            string numberInput = GetInput("Please enter the number:", "Input");
            if (numberInput == null) return;
            string rootInput = GetInput("Choose root degree:", "Input");
            if (rootInput == null) return;
            if (!double.TryParse(numberInput, out double number) || !int.TryParse(rootInput, out int root))
            {
                MessageBox.Show("Please enter valid inputs");
                return;
            }
            if (root <= 0)
            {
                MessageBox.Show("Root must be > 0");
                return;
            }
            if (number < 0 && root % 2 == 0)
            {
                MessageBox.Show($"The {root}th root of negative number has no real result.");
                return;
            }
            if (root % 2 == 0)
            {
                double r1 = Math.Pow(number, 1.0 / root);
                double r2 = -r1;
                MessageBox.Show($"{root}th roots: {r1:N2}, {r2:N2}");
                return;
            }
            double rootValue = Math.Pow(Math.Abs(number), 1.0 / root);
            if (number < 0 && root % 2 == 1) rootValue = -rootValue;
            MessageBox.Show($"{root}th root of {number} is {rootValue:N2}");
        }
    }
}
