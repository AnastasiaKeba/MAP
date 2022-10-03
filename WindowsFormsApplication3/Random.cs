using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3
{
    class Random
    {
        static long ix1, ix2, ix3;
        static int iff = 0;
        static double[] r = new double[97];

        public static double ran1(ref long idum)
        {
            const double rm1 = 3.8580247e-6, rm2 = 7.4373773e-6;
            const long m1 = 259200, ia1 = 7141, ic1 = 54773, m2 = 134456, ia2 = 8121, ic2 = 28411, m3 = 243000, ia3 = 4561, ic3 = 51349;
            int j;
            double ret;
            if (idum < 0 || iff == 0)
            {
                iff = 1;
                if (idum < 0) idum = -idum;
                ix1 = (ic1 - (idum)) % m1;
                ix1 = (ia1 * ix1 + ic1) % m1;
                ix2 = ix1 % m2;
                ix1 = (ia1 * ix1 + ic1) % m1;
                ix3 = ix1 % m3;
                for (j = 0; j < 97; j++)
                {
                    ix1 = (ia1 * ix1 + ic1) % m1;
                    ix2 = (ia2 * ix2 + ic2) % m2;
                    r[j] = (ix1 + ix2 * rm2) * rm1;
                }
                idum = 1;
            }
            ix1 = (ia1 * ix1 + ic1) % m1;
            ix2 = (ia2 * ix2 + ic2) % m2;
            ix3 = (ia3 * ix3 + ic3) % m3;
            j = (int)((97 * ix3) / m3);
            if (j < 0) j = 0;
            else { if (j > 96) j = 96; }
            ret = r[j];
            r[j] = (ix1 + ix2 * rm2) * rm1;
            return ret;
        }

        public static double gauss1(ref long ix)
        {
            double a = 0.0;
            for (int i = 0; i < 12; i++)
                a += ran1(ref ix);
            return a - 6.0;
        }

        public static double CauchyQuantile(double p)
        {
            return Math.Tan(Math.PI * (p - 0.5));
        }

        public static double stud1(double p, double v)
        {
            double pp, x0, x2;
            if ((v == 1) || (v == 2))
            {
                pp = p * 3.14159265;
                if (v == 1) return -Math.Cos(pp) / Math.Sin(pp);
                if (v == 2)
                {
                    pp = Math.Sqrt(1.0 / (2.0 * p * (1.0 - p)) - 2.0);
                    if (p < 0.5) return -pp;
                }
            }
            x0 = fi1(p); x2 = x0 * x0;
            x0 = x0 * (1.0 + (1.0 + x2) / (4.0 * v) + (3.0 + 16.0 * x2 + 5.0 * x2 * x2) / (96.0 * v * v));
            return x0;
        }

        public static double fi(double x)
        {
            const double a1 = 0.4361836, a2 = -0.1201676, a3 = 0.937298;
            double z1, t9, q1, aPow;
            z1 = Math.Abs(x);
            if (z1 < 1.9)
            {
                t9 = 1.0 / (1.0 + 0.33267 * z1);
                q1 = t9 * (a1 + a2 * t9 + a3 * t9 * t9) / Math.Exp(z1 * z1 / 2.0) / Math.Sqrt(2.0 * Math.PI);
            }
            else
            {
                t9 = 1.0 - 1.0 / (z1 * z1 + 3.0 / (0.22 * (z1 * z1 + 0.39909)));
                aPow = 0.21714724 * z1 * z1 + 0.39909;
                if (aPow > 100) q1 = 0;
                else q1 = t9 / z1 / Math.Pow(10.0, aPow);
            }
            if (x < 0.0) return q1;
            else return 1.0 - q1;
        }
        public static double fi1(double p)
        {
            const double a0 = 2.5066282, a1 = -18.6150006, a2 = 41.3911977, a3 = -25.4410605, b1 = -8.4735109, b2 = 23.0833674, b3 = -21.0622410, b4 = 3.1308291, c0 = -2.7871893, c1 = -2.2979648, c2 = 4.8501413, c3 = 2.3212128, d1 = 3.5438892, d2 = 1.6370678;
            double g, r, r0;
            g = p - 0.5;
            if (Math.Abs(g) < 0.42)
            {
                r = g * g; r0 = g * (((a3 * r + a2) * r + a1) * r + a0);
                return r0 / ((((b4 * r + b3) * r + b2) * r + b1) * r + 1.0);
            }
            else
            {
                if (g > 0.0) r = 1.0 - p;
                else r = p;
                r = Math.Sqrt(-Math.Log(r));
                r0 = (((c3 * r + c2) * r + c1) * r + c0);
                r0 = r0 / ((d2 * r + d1) * r + 1.0);
                if (g < 0.0) return -r0;
                else return r0;
            }
        }

        public static double chi(double x, double v)
        {
            double z, c, d, a, b, d3, g, q1, d1, t2, z1;
            int j;
            if (x == 0) { return 0; }
            if (v < 11)
            {
                z = x / 2.0; c = 1.0; g = 1.0; d = v / 2.0; a = d; d3 = d + 2.0;
                do
                {
                    a = a + 1.0; c = c * z / a; g = g + c;
                }
                while (c / g > 0.5e-6);
                b = d3 * d3;
                b = 1.0 / (12.0 * b) * (1.0 - 1.0 / b * (1.0 / 30.0 - 1.0 / b * (1.0 / 105.0 - 1.0 / (140.0 * b))));
                g = g * Math.Exp(d * Math.Log(z) - d3 * b - (d3 - 0.5) * Math.Log(d3) + d3 - z) * (d + 1.0);
                q1 = 1.0 - g / Math.Sqrt(2.0 * 3.14159265);
            }
            else
            {
                d1 = v - 1.0; t2 = d1 / x; d3 = x - v + 2.0 / 3.0 - 0.8 / v;
                g = 1.0;
                if (t2 != 0.0)
                {
                    if (Math.Abs(1.0 - t2) > 0.1) g = (1.0 - t2 * t2 + 2.0 * t2 * Math.Log(t2)) / ((1.0 - t2) * (1.0 - t2));
                    else
                    {
                        g = 0.0;
                        for (j = 1; j <= 5; j++)
                            g = g + 2.0 * Math.Pow(1.0 - t2, j) / ((j + 1) * (j + 2));
                    }
                }
                z1 = d3 * Math.Sqrt((1.0 + g) / (2.0 * x));
                q1 = 1.0 - fi(z1);
            }
            return 1 - q1;
        }

        public static double chi1(double p, double v)
        {
            const double eps = 1.0e-5;
            double x1, x2, x, p1, p2;
            if (v < 30)
            {
                x1 = 0.0; x2 = 1.0; p1 = 0.0; p2 = 0.0;
                do
                {
                    if (p2 > 0.0) { x1 = x2; x2 = x2 + 1.0; p1 = p2; }
                    p2 = chi(v, x2);
                } while (p2 < p);
                do
                {
                    x = (p - p2) * (x1 - x2) / (p1 - p2) + x2;
                    if (x < 0.0) x = 0.0;
                    x1 = x2; p1 = p2; x2 = x; p2 = chi(v, x2);
                } while (Math.Abs(x1 - x2) > eps || Math.Abs(p2 - p) > eps);
                return x;
            }
            else
            {
                x = 2.0 / (9.0 * v);
                x = 1.0 - x + fi1(p) * Math.Sqrt(x);
                return v * x * x * x;
            }
        }

        public static double t1(double p, double v)
        {
            double[] tab = new double[30] { 12.71, 4.30, 3.18, 2.78, 2.57, 2.45, 2.36, 2.31, 2.26, 2.23, 2.20, 2.18, 2.16, 2.15, 2.13, 2.12, 2.11, 2.10, 2.09, 2.09, 2.04, 2.02, 2.01, 2.00, 1.99, 1.99, 1.99, 1.98, 1.97, 1.97 };
            int nn;
            nn = (int)v;
            if (nn < 1) return -1000;
            else
            {
                if (nn == 1 && nn <= 20) return tab[nn];
                else
                {
                    if (nn <= 100) return tab[20 + ((nn - 20) / 10)];
                    else
                    {
                        if (nn <= 1000) return 1.97;
                        else return 1;
                    }
                }
            }
        }
    }
}
