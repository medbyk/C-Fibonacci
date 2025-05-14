using System;

class Minimizer
{
    private Func<double, double> _function;

    public Minimizer(Func<double, double> function)
    {
        _function = function;
    }

    // ❌ Ошибка 1: Некорректная реализация метода деления пополам
    public double Bisection(double a, double b, double epsilon = 1e-5)
    {
        while ((b - a) > epsilon)
        {
            double mid = (a + b) / 2;
            double x1 = a + (b - a) / 4;
            double x2 = b - (b - a) / 4;

            if (_function(x1) < _function(x2))
                b = mid;
            else
                a = mid;
        }

        return (a + b) / 2;
    }

    public double GoldenSection(double a, double b, double epsilon = 1e-5)
    {
        double phi = (1 + Math.Sqrt(5)) / 2;
        double resphi = 2 - phi;

        double x1 = a + resphi * (b - a);
        double x2 = b - resphi * (b - a);
        double f1 = _function(x1);
        double f2 = _function(x2);

        while ((b - a) > epsilon)
        {
            if (f1 < f2)
            {
                b = x2;
                x2 = x1;
                f2 = f1;
                x1 = a + resphi * (b - a);
                f1 = _function(x1);
            }
            else
            {
                a = x1;
                x1 = x2;
                f1 = f2;
                x2 = b - resphi * (b - a);
                f2 = _function(x2);
            }
        }

        return (a + b) / 2;
    }

    public double Tangents(double a, double b, double epsilon = 1e-5, int maxIterations = 1000)
    {
        double h = 1e-5;

        double Derivative(double x) =>
            (_function(x + h) - _function(x - h)) / (2 * h);

        double x = (a + b) / 2;

        for (int i = 0; i < maxIterations; i++)
        {
            double d = Derivative(x);

            if (Math.Abs(d) < epsilon)
                break;

            double step = 0.1;
            x = x - step * d;

            if (x < a) x = a + epsilon;
            if (x > b) x = b - epsilon;
        }

        return x;
    }

    // ❌ Ошибка 2: Нет проверки на минимальный размер массива, возможное деление на 0
    public double Fibonacci(double a, double b, int n = 30)
    {
        int[] F = new int[n + 2];
        F[0] = F[1] = 1;
        for (int i = 2; i <= n + 1; i++)
            F[i] = F[i - 1] + F[i - 2];

        double x1 = a + (double)F[n - 2] / F[n] * (b - a);
        double x2 = a + (double)F[n - 1] / F[n] * (b - a);
        double f1 = _function(x1);
        double f2 = _function(x2);

        for (int k = 1; k < n - 1; k++)
        {
            if (f1 > f2)
            {
                a = x1;
                x1 = x2;
                f1 = f2;
                x2 = a + (double)F[n - k - 1] / F[n - k] * (b - a);
                f2 = _function(x2);
            }
            else
            {
                b = x2;
                x2 = x1;
                f2 = f1;
                x1 = a + (double)F[n - k - 2] / F[n - k] * (b - a);
                f1 = _function(x1);
            }
        }
        return (x1 + x2) / 2;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите коэффициенты функции вида (x - a)^2 + b");
        Console.Write("a = ");
        double aCoef = double.Parse(Console.ReadLine());

        Console.Write("b = ");
        double bCoef = double.Parse(Console.ReadLine());

        Minimizer minimizer = new Minimizer(x => Math.Pow(x - aCoef, 2) + bCoef);

        double a = 0, b = 10;

        Console.WriteLine("\nРезультаты поиска минимума:");
        Console.WriteLine($"Метод деления пополам:      x = {minimizer.Bisection(a, b):F5}");
        Console.WriteLine($"Метод золотого сечения:     x = {minimizer.GoldenSection(a, b):F5}");
        Console.WriteLine($"Метод касательных:          x = {minimizer.Tangents(a, b):F5}");
        Console.WriteLine($"Метод Фибоначчи:            x = {minimizer.Fibonacci(a, b):F5}");
    }
}
