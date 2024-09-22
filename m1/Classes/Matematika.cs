using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m1.Classes
{
    internal class Matematika
    {
        public double interval_a;
        public double interval_b;
        private double interval_c;
        public Expression fa;
        public Expression fb;
        public Expression fc;
        private double interval_ba;
        public double ex1;
        public double ex2;
        public double ex3;
        static public Expression f_del;
        public double fb2;
        public int count = 1;

        public double MidInterval()
        {
            interval_c = Math.Round((interval_a+ interval_b) / 2, 3);
            return interval_c;
        }
        public double IntervalBA(byte precision)
        {
            interval_ba = Math.Round(interval_b - interval_a, precision);
            return interval_ba;
        }
        public double NumberDigit(double x)
        {
            int c = 1;
            //bool neg = false;
            //if(x < 0)
            //{
            //    x *= -1;
            //    neg = true;
            //}

                for (int i = 1; true ; i*=10)
                {
                    if ((x * i) % 1 == 0)
                    {
                        x = x*i;
                        count = c;
                        break;
                    }
                    c++;
                }

            //if (neg)
            //{
            //    x *= -1;
            //}
            int rounder = (int)x;
            if (Math.Abs(x) > 999)
            {
                
                rounder /= (int)Math.Pow(10, rounder.ToString().Length - 3);
                x = rounder;
            }
            

            return x;
        }

    }
}
