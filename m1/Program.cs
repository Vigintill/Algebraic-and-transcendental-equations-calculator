using m1.Classes;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.Differentiation;
using NCalc.Domain;
using NCalc.Visitors;
using NCalc.Reflection;
using MathNet.Numerics.LinearAlgebra.Solvers;


namespace m1
{
    internal class Program
    {
        static Expression expression;
        static int counter;
        static Matematika[] m = new Matematika[10];
        static Matematika[] m2 = new Matematika[10];
        static Expression e;
        static int count1;
        static int count2;
        static int count3;

        const byte HIGH_PRECISION = 5;
        const byte LOW_PRECISION = 3;
        static void Main(string[] args)
        {
            Console.SetWindowSize(190, 30);


            while (true)
            {
                try
                {
                    Console.WriteLine("Введите выражение:");
                    string s = Console.ReadLine();
                    e = new Expression(s);

                    Console.WriteLine("Введите интервал:");

                    m[0] = new Matematika();
                    m[0].interval_a = Math.Round(Convert.ToDouble(Console.ReadLine()), LOW_PRECISION);
                    m[0].interval_b = Math.Round(Convert.ToDouble(Console.ReadLine()), LOW_PRECISION);

                    Console.WriteLine("[{0}; {1}]", m[0].interval_a, m[0].interval_b);

                    Matematika.f_del = e;

                    m[0].fa = e;
                    m[0].fa.Parameters["X"] = m[0].interval_a;
                    m[0].ex1 = Math.Round(Convert.ToDouble(m[0].fa.Evaluate()), LOW_PRECISION);
                    m[0].fb = e;
                    m[0].fb.Parameters["X"] = m[0].interval_b;
                    m[0].ex2 = Math.Round(Convert.ToDouble(m[0].fb.Evaluate()), LOW_PRECISION);
                    m[0].fc = e;
                    m[0].fc.Parameters["X"] = m[0].MidInterval();
                    m[0].ex3 = Math.Round(Convert.ToDouble(m[0].fc.Evaluate()), LOW_PRECISION);

                    m[0].IntervalBA(LOW_PRECISION);
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);
                    continue;
                }

                break;
            }
            //Console.WriteLine("|\tn\t|\ta\t|\tb\t|\tc\t|\tf(a)\t|\tf(b)\t|\tf(c)\t|\tb-a\t|");
            Console.WriteLine(String.Format("|{0,12}|{1,12}|{2,12}|{3,12}|{4,12}|{5,12}|{6,12}|{7, 12}|", "n", "a", "b", "c", "f(a)", "f(b)", "f(c)", "b-a"));
            PrintSheet(m[0], 0);
            
            for (int i = 1; true; i++)
            {
                m[i] = new Matematika();
                if (Convert.ToInt32(m[i-1].ex1) * Convert.ToInt32(m[i-1].ex3) > 0)
                {
                    m[i].interval_a = m[i - 1].MidInterval();
                }
                else
                {
                    m[i].interval_a = m[i - 1].interval_a;
                }

                if (Convert.ToInt32(m[i - 1].ex2) * Convert.ToInt32(m[i - 1].ex3) > 0)
                {
                    m[i].interval_b = m[i - 1].MidInterval();
                }
                else
                {
                    m[i].interval_b = m[i - 1].interval_b;
                }
                m[i].MidInterval();
                m[i].fa = e;
                m[i].fa.Parameters["X"] = m[i].interval_a;
                m[i].ex1 = Math.Round(Convert.ToDouble(m[i].fa.Evaluate()), LOW_PRECISION);
                m[i].fb = e;
                m[i].fb.Parameters["X"] = m[i].interval_b;
                m[i].ex2 = Math.Round(Convert.ToDouble(m[i].fb.Evaluate()), LOW_PRECISION);
                m[i].fc = e;
                m[i].fc.Parameters["X"] = m[i].MidInterval();
                m[i].ex3 = Math.Round(Convert.ToDouble(m[i].fc.Evaluate()), LOW_PRECISION);
                m[i].IntervalBA(LOW_PRECISION);

                PrintSheet(m[i], i);

                if (m[i].IntervalBA(LOW_PRECISION) <= 0.2)
                {
                    counter = i;
                    break;
                }
                if(i > 0)
                {
                    if (m[i].IntervalBA(LOW_PRECISION) == m[i - 1].IntervalBA(LOW_PRECISION))
                    {
                        counter = i;
                        break;
                    }
                }
            }


            Func<double, double> delegateDerivativeFunc = Function;

            Func<double, double> func_a = Differentiate.SecondDerivativeFunc(delegateDerivativeFunc);

            Console.WriteLine("f(a)f\"(a) = {0}  <  0", m[counter].ex1 * func_a(m[counter].interval_a));

            //Console.WriteLine("|n\t\t|a\t\t|b\t\t|f(a)\t\t|f(b)\t\t|f\'(b)\t\t|b-a\t\t|");
            Console.WriteLine(String.Format("|{0,12}|{1,12}|{2,12}|{3,12}|{4,12}|{5,12}|{6,12}|", "n", "a", "b", "f(a)", "f(b)", "f\'(b)", "b-a"));

            m2[0] = new Matematika();
            m2[0].interval_a = Math.Round(m[counter].interval_a, HIGH_PRECISION);
            m2[0].interval_b = Math.Round(m[counter].interval_b, HIGH_PRECISION);

            m2[0].fa = e;
            m2[0].fa.Parameters["X"] = m2[0].interval_a;
            m2[0].ex1 = Math.Round(Convert.ToDouble(m2[0].fa.Evaluate()), HIGH_PRECISION);
            m2[0].fb = e;
            m2[0].fb.Parameters["X"] = m2[0].interval_b;
            m2[0].ex2 = Math.Round(Convert.ToDouble(m2[0].fb.Evaluate()), HIGH_PRECISION);

            Func<double, double> func_b = Differentiate.FirstDerivativeFunc(delegateDerivativeFunc);
            m2[0].fb2 = Math.Round(func_b(m2[0].interval_b), HIGH_PRECISION);
            m2[0].IntervalBA(HIGH_PRECISION);

            PrintSheet2(m2[0], 0);

            for(int i = 1; true; i++)
            {
                m2[i] = new Matematika();
                m2[i].interval_a = Math.Round(m2[i - 1].interval_a + (m2[i - 1].interval_b - m2[i - 1].interval_a) * m2[i - 1].ex1/(m2[i - 1].ex1 - m2[i - 1].ex2), LOW_PRECISION);
                m2[i].interval_b = Math.Round(m2[i - 1].interval_b - m2[i - 1].ex2 / m2[i - 1].fb2, LOW_PRECISION);

                m2[i].fa = e;
                m2[i].fa.Parameters["X"] = Math.Round(m2[i].interval_a, LOW_PRECISION);
                m2[i].ex1 = Math.Round(Convert.ToDouble(m2[i].fa.Evaluate()), HIGH_PRECISION);
                m2[i].fb = e;
                m2[i].fb.Parameters["X"] = Math.Round(m2[i].interval_b, LOW_PRECISION);
                m2[i].ex2 = Math.Round(Convert.ToDouble(m2[i].fb.Evaluate()), HIGH_PRECISION);

                m2[i].fb2 = Math.Round(func_b(m2[i].interval_b), HIGH_PRECISION);
                m2[i].IntervalBA(HIGH_PRECISION);

                

                if (m2[i].IntervalBA(HIGH_PRECISION) == 0 || i == 9)
                {
                    counter = i-1;
                    //PrintSheet2(m2[counter], counter);
                    break;
                }
                PrintSheet2(m2[i], i);

            }

            double a = m2[counter].NumberDigit(m2[counter].ex1);
            count1 = m2[counter].count;
            double b = m2[counter].NumberDigit(m2[counter].ex2);
            count2 = m2[counter].count;
            double c = m2[counter].NumberDigit(m2[counter].fb2);
            count3 = m2[counter].count;

            PrintSheet2(m2[counter], counter, a, b, c);

            Console.WriteLine("ОТВЕТ:\tКорень\tТочность");
            Console.WriteLine("      \t{0}\t{1}", m2[counter].interval_a, Math.Abs(m2[counter].ex1));

            Console.ReadKey();
        }

        static void PrintSheet(Matematika m, int iteration)
        {
            Console.WriteLine(String.Format("|{0,12}|{1,12}|{2,12}|{3,12}|{4,12}|{5,12}|{6,12}|{7, 12}", iteration, m.interval_a, m.interval_b, m.MidInterval(), m.ex1, m.ex2, m.ex3, m.IntervalBA(LOW_PRECISION)));
            //Console.WriteLine("|\t{0}\t|\t{1}\t|\t{2}\t|\t{3}\t|\t{4}\t|\t{5}\t|\t{6}\t|\t{7}\t|", iteration, m.interval_a, m.interval_b, m.MidInterval(), m.ex1, m.ex2, m.ex3, m.IntervalBA(LOW_PRECISION));
        }
        static void PrintSheet2(Matematika m, int iteration)
        {
            //Console.WriteLine("|{0}\t\t|{1}\t\t|{2}\t\t|{3}\t\t|{4}\t\t|{5}\t\t|{6}\t\t|", iteration, m.interval_a, m.interval_b, m.ex1, m.ex2, m.fb2, m.IntervalBA(HIGH_PRECISION));
            Console.WriteLine(String.Format("|{0,12}|{1,12}|{2,12}|{3,12}|{4,12}|{5,12}|{6,12}|", iteration, m.interval_a, m.interval_b, m.ex1, m.ex2, m.fb2, m.IntervalBA(HIGH_PRECISION)));
        }
        static void PrintSheet2(Matematika m, int iteration, double s1, double s2, double s3)
        {
            //Console.WriteLine("|{0}\t\t|{1}\t\t|{2}\t\t|{3}*10^-{7}\t\t|{4}*10^-{8}\t\t|{5}*10^-{9}\t\t|{6}\t\t|", iteration, m.interval_a, m.interval_b, s1, s2, s3, m.IntervalBA(HIGH_PRECISION), count1, count2, count3);
            Console.WriteLine(String.Format("|{0,12}|{1,12}|{2,12}|{3,12}*10^-{7}|{4,12}*10^-{8}|{5,12}*10^-{9}|{6,12}|", iteration, m.interval_a, m.interval_b, s1, s2, s3, m.IntervalBA(HIGH_PRECISION), count1, count2, count3));
        }

        public static double Function(double x)
        {

            Matematika.f_del.Parameters["X"] = x;
            return Convert.ToDouble(Matematika.f_del.Evaluate());

        }

        public static Matematika[] GetGlobalExpression()
        {
            return m;
        }

        public static int GetGlobalCounter()
        {
            return counter;
        }
    }
}
