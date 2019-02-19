using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSACracker
{
    class Program
    {
        static void Main(string[] args)
        {
            RunTest();
            Console.ReadKey();
        }


        static void RunTest()
        {
            IsPrimeTest();
            PrimeFactorTest();
            CalculatePrivateKeyTest();
            CrackRSATest();
        }


        static void IsPrimeTest()
        {
            var p_test = new List<int>() { 3, 7, 31, 37, 779, 817, 893, 1007, 1121, 1159, 1273, 1349, 1387, 1501, 1577, 1691, 1843, 667, 713, 851, 943, 989, 1081, 1219 };

            Console.WriteLine("Checking for prime numbers:");
            foreach (int number in p_test)
            {
                Console.WriteLine("{0} : {1}", number, IsPrime(number));
            }
            Console.WriteLine();
        }


        static void PrimeFactorTest()
        {
            var pf_test = new List<int>() { 923, 949, 1027, 1079, 1157, 1261, 323, 391, 493, 527, 629, 697, 731, 799, 901, 1003, 1037, 1139, 1207, 1241, 1343, 1411, 1513, 1649, 437, 551, 589, 703, 779, 817, 893 };

            Console.WriteLine("Find prime factors of numbers:");
            foreach (int number in pf_test)
            {
                var pfs = PrimeFactor(number);
                Console.Write("{0} contains these prime Factors: [ ", number);
                foreach (int pf in pfs)
                {
                    Console.Write("{0}, ", pf);
                }
                Console.WriteLine("]");
            }
            Console.WriteLine();
        }

        static void CalculatePrivateKeyTest()
        {
            var pk_test = new List<Tuple<int, int>>();

            pk_test.Add(new Tuple<int, int>(131, 328));
            pk_test.Add(new Tuple<int, int>(79, 328));
            pk_test.Add(new Tuple<int, int>(23, 84));
            pk_test.Add(new Tuple<int, int>(113, 576));
            pk_test.Add(new Tuple<int, int>(157, 168));
            pk_test.Add(new Tuple<int, int>(37, 84));
            pk_test.Add(new Tuple<int, int>(277, 576));
            pk_test.Add(new Tuple<int, int>(499, 576));

            Console.WriteLine("Calculate the private key:");
            foreach (Tuple<int, int> number in pk_test)
            {
                var pk = CalculatePrivateKey(number.Item1, number.Item2);
                Console.WriteLine("e: {0} theta: {1} privatekey: {2}", number.Item1, number.Item2, pk);
            }
            Console.WriteLine();
        }

        static void CrackRSATest()
        {
            var pk_test = new List<Tuple<int, int>>();

            pk_test.Add(new Tuple<int, int>(131, 415));
            pk_test.Add(new Tuple<int, int>(79, 415));
            pk_test.Add(new Tuple<int, int>(37, 129));
            pk_test.Add(new Tuple<int, int>(23, 129));
            pk_test.Add(new Tuple<int, int>(113, 679));
            pk_test.Add(new Tuple<int, int>(157, 215));
            pk_test.Add(new Tuple<int, int>(277, 679));
            pk_test.Add(new Tuple<int, int>(499, 679));

            Console.WriteLine("Bruteforce the private key:");
            foreach (Tuple<int, int> number in pk_test)
            {
                var pk = CalculatePrivateKey(number.Item1, number.Item2);
                Console.WriteLine("e: {0} n: {1} privatekey: {2}", number.Item1, number.Item2, pk);
            }
            Console.WriteLine();
        }




        static bool IsPrime(int number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
                if (number % i == 0)
                    return false;

            return true;
        }

        static List<int> PrimeFactor(int number)
        {
            var pfs = new List<int>();
            int pf;

            for (pf = 2; number > 1; pf++) {
                if (number % pf == 0) {
                    int x = 0;
                    while (number % pf == 0)
                    {
                        number /= pf;
                        x++;
                        pfs.Add(pf);
                    }
                }
            }
            return pfs;
        }

        
        static int CalculatePrivateKey(int e, int theta)
        {
            int a = theta, b = e, x = 0, y = 0, gcd = 0;
            ExtendedEuclideanAlgorithm(ref a, ref b, ref x, ref y, ref gcd);
            return y;

        }

        /*
         * ax + by = gcd(a,b)
         * we want to return y(our private key)
         */
        static int ExtendedEuclideanAlgorithm(ref int a, ref int b, ref int x, ref int y, ref int gcd)
        {
            if (a < b)
            {
                (a, b) = (b, a);
            }
            gcd = xGCD(a, b, ref x, ref y);

            //std::cout << "GCD: " << gcd << ", x = " << x << ", y = " << y << std::endl;
            return y;
        }


        static int xGCD(int a, int b, ref int x, ref int y)
        {
            if (b == 0)
            {
                x = 1;
                y = 0;
                return a;
            }

            int x1 = 0, y1 = 0;
            int gcd = xGCD(b, a % b, ref x1, ref y1);
            x = y1;
            y = x1 - (a / b) * y1;
            return gcd;
        }



        static int CrackRSA(int e, int n)
        {
            var pfs = PrimeFactor(n);
            var pk = CalculatePrivateKey(pfs.ElementAt(0), pfs.ElementAt(1));
            return pk;
        }


    }
}
