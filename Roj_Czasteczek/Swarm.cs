using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roj_czasteczek
{
    public struct Czasteczka
    {
        public double x;
        public double xf;
        public double b;
        public double bg;
        public double v;
    }
    public struct Wynik
    {
        public double fmax;
        public double fmin;
        public double favg;
    }
    class Roj
    {
        public List<Wynik> wyniki;

        public List<List<Double>> history;

        public double Feval(double xreal)
        {
            return (xreal - (Math.Truncate(xreal))) * (Math.Cos(20 * Math.PI * xreal) - Math.Sin(xreal));
        }
        public List<Czasteczka> Inicjalizacja(int a, int b, int N, int decimals)
        {
            Random rand = new Random();
            List<Czasteczka> roj = new List<Czasteczka>();
            history = new List<List<double>>();
            for (int i = 0; i < N; i++)
            {
                history.Add(new List<double>());
                var x = new Czasteczka();
                var zmienna = Math.Round(rand.NextDouble() * (b - a) + a, decimals);
                x.x = zmienna;
                x.b = zmienna;
                x.bg = zmienna;
                x.v = 0;
                x.xf = Feval(zmienna);
                roj.Add(x);
            }
            return roj;
        }
        public List<Czasteczka> ZnajdowanieSasiadow(List<Czasteczka> Roj, int N, int r, double x)
        {
            int wart = (int)Math.Ceiling((double)((double)N * ((double)r / (double)100)));
            var z = Roj.OrderBy(a => Math.Abs(a.x - x)).Take(wart).ToList();
            return z.OrderByDescending(a => Feval(a.bg)).ToList();
        }

        public List<Czasteczka> Reakcja(List<Czasteczka> Roj, int N, int r, double c1, double c2, double c3, int decimals)
        {
            for (int i = 0; i < Roj.Count; i++)
            {
                var z = Roj[i];
                history[i].Add(z.x);
                z.xf = Feval(z.x);
                if (Feval(z.b) < Feval(z.x)) z.b = z.x;
                if (Feval(z.bg) < Feval(z.x)) z.bg = z.x;
                Roj[i] = z;
            }
            for (int i = 0; i < Roj.Count; i++)
            {
                var zmienna = ZnajdowanieSasiadow(Roj, N, r, Roj[i].x);
                var sasiadBest = zmienna.OrderByDescending(a => Feval(a.bg)).First();
                if (Roj[i].bg < sasiadBest.bg)
                {
                    var zmiana = Roj[i];
                    zmiana.bg = sasiadBest.bg;
                    Roj[i] = zmiana;
                }
            }
            Random rand = new Random();
            for (int i = 0; i < Roj.Count; i++)
            {
                var zz = Roj[i];
                zz.v = (zz.v * c1 * rand.NextDouble()) + (c2 * rand.NextDouble() * (zz.b - zz.x)) + (c3 * rand.NextDouble() * (zz.bg - zz.x));
                zz.x += zz.v;
                zz.x = Math.Round(zz.x, decimals);
                Roj[i] = zz;
            }
            return Roj;

        }

        public List<Czasteczka> Symulacja(List<Czasteczka> Roj, int N, int r, double c1, double c2, double c3, int T, int decimals)
        {
            wyniki = new List<Wynik>();
            for (int i = 0; i < T; i++)
            {
                Roj = Reakcja(Roj, N, r, c1, c2, c3, decimals);
                var wynik = new Wynik();
                wynik.fmin = Roj.Min(a => a.xf);
                wynik.fmax = Roj.Max(a => a.xf);
                wynik.favg = Roj.Average(a => a.xf);
                wyniki.Add(wynik);
            }
            return Roj;

        }





    }
}

