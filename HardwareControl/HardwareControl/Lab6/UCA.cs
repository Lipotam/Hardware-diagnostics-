using System;
using System.Collections.Generic;
using System.Text;
using HardwareControl.Lab1;

namespace HardwareControl.Lab6
{
    public class UCA
        {
        private List<int> _polynom;
        private int _length;

        public double Efficiency { get; set; }

        public UCA(List<int> polynomList, int signalLength)
        {
            _polynom = polynomList;
            _length = signalLength;
        }

        public int SignalLength
        {
            get
            {
                return _length;
            }
        }

        public List<bool> GetSignal()
        {
            Random rand = new Random();
            List<bool> signal = new List<bool>();
            for (int i = 0; i < _length; i++)
            {
                signal.Add(rand.NextDouble() > 0.5);
            }
            return signal;
        }

        public List<String> GetAllSingleErrors(List<bool> signal, bool multiple)
        {
            Efficiency = 0;
            List<String> result = new List<string>();
            String etalon = multiple ? ProcessMCA(signal) : ProcessUCA(signal);
            for (int i = 0; i < signal.Count; i++)
            {
                List<bool> noisy = new List<bool>(signal);
                noisy[i] = !noisy[i];
                if (etalon.Equals(multiple ? ProcessMCA(noisy) : ProcessUCA(noisy)))
                {
                    result.Add(SignalToString(noisy));
                    Efficiency++;
                }
            }
            Efficiency = Efficiency / signal.Count * 100;
            return result;
        }

        public List<String> GetAllDoubleErrors(List<bool> signal, bool multiple)
        {
            Efficiency = 0;
            List<String> result = new List<string>();
            String etalon = multiple ? ProcessMCA(signal) : ProcessUCA(signal);
            for (int i = 0; i < signal.Count; i++)
            {
                for (int j = i + 1; j < signal.Count; j++)
                {
                    List<bool> noisy = new List<bool>(signal);
                    noisy[i] = !noisy[i];
                    noisy[j] = !noisy[j];
                    if (etalon.Equals(multiple ? ProcessMCA(noisy) : ProcessUCA(noisy)))
                    {
                        result.Add(SignalToString(noisy));
                        Efficiency++;
                    }
                }
            }
            Efficiency = Efficiency / Math.Pow(signal.Count, 2) * 100;
            return result;
        }

        public List<String> GetAllTripleErrors(List<bool> signal, bool multiple)
        {
            Efficiency = 0;
            List<String> result = new List<string>();
            String etalon = multiple ? ProcessMCA(signal) : ProcessUCA(signal);
            for (int i = 0; i < signal.Count; i++)
            {
                for (int j = i + 1; j < signal.Count; j++)
                {
                    for (int k = j + 1; k < signal.Count; k++)
                    {
                        List<bool> noisy = new List<bool>(signal);
                        noisy[i] = !noisy[i];
                        noisy[j] = !noisy[j];
                        noisy[k] = !noisy[k];
                        if (etalon.Equals(multiple ? ProcessMCA(noisy) : ProcessUCA(noisy)))
                        {
                            result.Add(SignalToString(noisy));
                            Efficiency++;
                        }
                    }
                }
            }
            Efficiency = Efficiency / Math.Pow(signal.Count, 3) * 100;
            return result;
        }

        public List<String> GetAllPockerErrors(List<bool> signal, bool multiple)
        {
            Efficiency = 0;
            List<String> result = new List<string>();
            String etalon = multiple ? ProcessMCA(signal) : ProcessUCA(signal);
            for (int i = 0; i < signal.Count; i++)
            {
                for (int j = i + 1; j < signal.Count; j++)
                {
                    for (int k = j + 1; k < signal.Count; k++)
                    {
                        for (int t = k + 1; t < signal.Count; t++)
                        {
                            List<bool> noisy = new List<bool>(signal);
                            noisy[i] = !noisy[i];
                            noisy[j] = !noisy[j];
                            noisy[k] = !noisy[k];
                            noisy[t] = !noisy[t];
                            if (etalon.Equals(multiple ? ProcessMCA(noisy) : ProcessUCA(noisy)))
                            {
                                result.Add(SignalToString(noisy));
                                Efficiency++;
                            }
                        }
                    }
                }
            }
            Efficiency = Efficiency / Math.Pow(signal.Count, 4) * 100;
            return result;
        }

        public static String SignalToString(List<bool> signal)
        {
            StringBuilder sb = new StringBuilder();
            foreach (bool val in signal)
            {
                sb.Append(val ? "1" : "0");
            }
            return sb.ToString();
        }

        private String ProcessUCA(List<bool> signal)
        {
            List<bool> register = new List<bool>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                register.Add(false);
            }
            foreach (bool inVal in signal)
            {
                bool newVal = inVal;
                foreach (int pow in _polynom)
                {
                    newVal ^= register[pow - 1];
                }
                sb.Append(register[register.Count - 1] ? "1" : "0");
                register.RemoveAt(register.Count - 1);
                register.Insert(0, newVal);
            }

            return sb.ToString();
        }

        private String ProcessMCA(List<bool> signal)
        {
            List<bool> register1 = new List<bool>();
            List<bool> register2 = new List<bool>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                register1.Add(false);
                register2.Add(false);
            }
            for (int i = 0; i < signal.Count / 2; i++)
            {
                bool newVal = signal[2 * i];
                foreach (int pow in _polynom)
                {
                    newVal ^= register1[pow - 1];
                }
                sb.Append(register1[register1.Count - 1] ? "1" : "0");
                register1.RemoveAt(register1.Count - 1);
                register1.Insert(0, newVal);

                newVal = signal[2 * i + 1];
                foreach (int pow in _polynom)
                {
                    newVal ^= register2[pow - 1];
                }
                sb.Append(register2[register2.Count - 1] ? "1" : "0");
                register2.RemoveAt(register2.Count - 1);
                register2.Insert(0, newVal);
            }

            return sb.ToString();
        }

        private static List<List<bool>> GenerateSets(int number, GenerationType type)
        {
            List<List<bool>> sets = new List<List<bool>>();
            int maxValue = Convert.ToInt32(Math.Pow(2, number));
            switch (type)
            {
                case GenerationType.AllSets:
                    {
                        for (int i = 0; i < maxValue; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                case GenerationType.AllOnes:
                    {
                        sets.Add(ToBinary(maxValue - 1, number));
                        break;
                    }
                case GenerationType.AllNulls:
                    {
                        sets.Add(ToBinary(0, number));
                        break;
                    }
                case GenerationType.ExeptAllOnes:
                    {
                        for (int i = 0; i < maxValue - 1; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                case GenerationType.ExeptAllNulls:
                    {
                        for (int i = 1; i < maxValue; i++)
                        {
                            sets.Add(ToBinary(i, number));
                        }
                        break;
                    }
                default: return null;
            }
            return sets;
        }

        private static List<bool> ToBinary(int number, int length)
        {
            List<bool> bin = new List<bool>();
            int n = number;
            for (int i = 0; i < length; i++)
            {
                bin.Add((n % 2) == 1);
                n /= 2;
            }
            return bin;
        }

        public String GetPolynom()
        {
            StringBuilder str = new StringBuilder();
            foreach (int power in _polynom)
            {
                str.Append("2^" + power.ToString() + " + ");
            }
            str.Append("1");
            return str.ToString();
        }
    }
}
