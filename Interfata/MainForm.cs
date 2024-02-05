/**************************************************************************
 *                                                                        *
 *  Copyright:   (c) 2016-2020, Florin Leon                               *
 *  E-mail:      florin.leon@academic.tuiasi.ro                           *
 *  Website:     http://florinleon.byethost24.com/lab_ia.html             *
 *  Description: Neural networks: decision regions                        *
 *               (Artificial Intelligence lab 12)                         *
 *                                                                        *
 *  This code and information is provided "as is" without warranty of     *
 *  any kind, either expressed or implied, including but not limited      *
 *  to the implied warranties of merchantability or fitness for a         *
 *  particular purpose. You are free to use this source code in your      *
 *  applications as long as the original copyright notice is included.    *
 *                                                                        *
 **************************************************************************/

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace DecisionRegions
{
    public partial class MainForm : Form
    {
        //Ce am adaugat pentru algoritmul evolutiv
        private EvolutionaryAlgorithm evolutionaryAlgorithm; // Adaugă o instanță a algoritmului evolutiv
        private IOptimizationProblem optimizationProblem; // Adaugă o instanță a problemei de optimizare
        //private Crossover crossover;

        /// <summary>
        /// Functie care calculeaza iesirea retelei in functie de tipul ei: perceptron cu un singur strat sau perceptron multistrat
        /// </summary>
        private Func<double, double, double> NetworkFunction;

        /// <summary>
        /// Functie care reprezinta functia de activare a retelei: prag, semiliniara sau sigmoida unipolara
        /// </summary>
        private Func<double, double> ActivationFunction;

        private double w13, w23, w14, w24, t3, t4, w35, w45, t5; // ponderile si pragurile

        public MainForm()
        {
            InitializeComponent();

            comboBoxType.SelectedIndex = 0;
            comboBoxActivation.SelectedIndex = 0;

            ActivationFunction = StepActivation; // functia de activare implicita este functia prag
            NetworkFunction = SLP; // reteaua implicita este perceptronul cu un singur strat
            
            // Initializeaza instantele algoritmului evolutiv si problemei de optimizare
            evolutionaryAlgorithm = new EvolutionaryAlgorithm();
            optimizationProblem = new optimizationProblem(this);
            //crossover = new Crossover(this);
        }

        /// <summary>
        /// Functia de activare prag
        /// </summary>
        private double StepActivation(double x)
        {
            if (x < 0) return 0;
            return 1;
        }

        /// <summary>
        /// Functia de activare semiliniara
        /// </summary>
        private double SemiliniarActivation(double x)
        {
            if(x <= 0) return 0;
            if (x > 0 && x < 1) return x;
            return 1;
        }

        /// <summary>
        /// Functia de activare sigmoida unipolara
        /// </summary>
        private double SigmoidActivation(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public double[] initialWeightsFromGUI()
        {
            if (comboBoxType.SelectedIndex == 0) // SLP
            {
                w13 = ConvertToDouble(textBoxSw13.Text);
                w23 = ConvertToDouble(textBoxSw23.Text);
                t3 = ConvertToDouble(textBoxSt3.Text);

                double[] initialWeights = new double[] { w13, w23, t3 };
                return initialWeights;
            }
            else if (comboBoxType.SelectedIndex == 1) // MLP
            {
                w13 = ConvertToDouble(textBoxMw13.Text);
                w23 = ConvertToDouble(textBoxMw23.Text);
                w14 = ConvertToDouble(textBoxMw14.Text);
                w24 = ConvertToDouble(textBoxMw24.Text);
                t3 = ConvertToDouble(textBoxMt3.Text);
                t4 = ConvertToDouble(textBoxMt4.Text);

                w35 = ConvertToDouble(textBoxMw35.Text);
                w45 = ConvertToDouble(textBoxMw45.Text);
                t5 = ConvertToDouble(textBoxMt5.Text);

                double[] initialWeights = new double[] { w13, w23, w14, w24, t3, t4, w35, w45, t5 };
                return initialWeights;
            }
            return null;
        }

        /// <summary>
        /// Citeste valorile ponderilor si pragurilor din interfata grafica
        /// </summary>
        private void ReadParametersFromGUI()
        {
            if (comboBoxType.SelectedIndex == 0) // SLP
            {
                w13 = ConvertToDouble(textBoxSw13.Text);
                w23 = ConvertToDouble(textBoxSw23.Text);
                t3 = ConvertToDouble(textBoxSt3.Text);
            }
            else if (comboBoxType.SelectedIndex == 1) // MLP
            {
                w13 = ConvertToDouble(textBoxMw13.Text);
                w23 = ConvertToDouble(textBoxMw23.Text);
                w14 = ConvertToDouble(textBoxMw14.Text);
                w24 = ConvertToDouble(textBoxMw24.Text);
                t3 = ConvertToDouble(textBoxMt3.Text);
                t4 = ConvertToDouble(textBoxMt4.Text);

                w35 = ConvertToDouble(textBoxMw35.Text);
                w45 = ConvertToDouble(textBoxMw45.Text);
                t5 = ConvertToDouble(textBoxMt5.Text);
            }
        }

        /// <summary>
        /// Converteste numerele citite ca text din interfata in valori reale
        /// </summary>
        private double ConvertToDouble(string s)
        {
            if (s.Contains(","))
                MessageBox.Show("Folositi punctul ca separator! (" + s + ")");

            try
            {
                CultureInfo ci = (CultureInfo)(CultureInfo.CurrentCulture.Clone());
                ci.NumberFormat.NumberDecimalSeparator = ".";
                return Convert.ToDouble(s, ci);
            }
            catch
            {
                MessageBox.Show("Numar invalid: " + s);
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateNetworkWeights(Chromosome chromosome)
        {
            if (comboBoxType.SelectedIndex == 0) // SLP
            {
                w13 = chromosome.Genes[0];
                w23 = chromosome.Genes[1];
                t3 = chromosome.Genes[2];
            }
            else if (comboBoxType.SelectedIndex == 1) // MLP
            {
                w13 = chromosome.Genes[0];
                w23 = chromosome.Genes[1];
                w14 = chromosome.Genes[2];
                w24 = chromosome.Genes[3];
                t3 = chromosome.Genes[4];
                t4 = chromosome.Genes[5];
                w35 = chromosome.Genes[6];
                w45 = chromosome.Genes[7];
                t5 = chromosome.Genes[8];
            }

            // Apelarea functiei pentru recalcularea rezultatelor dupa actualizarea ponderilor
            ComputeResults();
            pictureBoxRegions.Refresh();
        }

        public int[] getExpectedOutput()
        {
            int[] output = new int[] { Convert.ToInt32(textBox00.Text) ,
                                        Convert.ToInt32(textBox01.Text),
                                        Convert.ToInt32(textBox10.Text),
                                        Convert.ToInt32(textBox11.Text)
                                    };

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        public double CalculateNetworkPerformance()
        {
            // Set de date de testare
            double[,] testSet = {
                        {0, 0, Convert.ToInt32(textBox00.Text)},  // Input: 0, 0, Expected Output: va fi preluat din forms
                        {0, 1, Convert.ToInt32(textBox01.Text)},  // Input: 0, 1, Expected Output: va fi preluat din forms
                        {1, 0, Convert.ToInt32(textBox10.Text)},  // Input: 1, 0, Expected Output: va fi preluat din forms
                        {1, 1, Convert.ToInt32(textBox11.Text)}   // Input: 1, 1, Expected Output: va fi preluat din forms
                                };


            int correctPredictions = 0;
            int totalPredictions = testSet.GetLength(0);

            for (int i = 0; i < totalPredictions; i++)
            {
                double input1 = testSet[i, 0];
                double input2 = testSet[i, 1];
                double expectedOutput = testSet[i, 2];

                // Calculeaza iesirea retelei pentru intrarile date
                double actualOutput = NetworkFunction(input1, input2);

                // Verifica daca predictia retelei se apropie suficient de mult de iesirea asteptata
                double errorThreshold = 0.1;
                if (Math.Abs(actualOutput - expectedOutput) < errorThreshold)
                {
                    correctPredictions++;
                }
            }

            // Calculează performanța rețelei ca raport între predicțiile corecte și totalul de predicții
            double performance = (double)correctPredictions / totalPredictions;

            return performance;
        }

        /// <summary>
        /// Metoda care calculeaza iesirea perceptronului cu un singur strat pentru intrarile x si y
        /// </summary>
        private double SLP(double x1, double x2)
        {
            // double s = suma ponderata a intrarilor cu scaderea pragului, conform ecuatiei 1 din laborator
            // se folosesc ponderile w13, w23 si pragul t3
            // in functie de alegerea utilizatorului, ActivationFunction poate fi: StepActivation, SemiliniarActivation sau SigmoidActivation
            // functia de activare este setata in evenimentul comboBoxActivation_SelectedIndexChanged
            // in metoda curenta se lucreaza in mod generic doar cu "ActivationFunction": return ActivationFunction(s);

            return ActivationFunction(w13 * x1 + w23 * x2 - t3);
        }

        /// <summary>
        /// Antreneaza perceptronul cu un singur strat si seteaza ponderile in interfata
        /// </summary>
        private void buttonTrainSLP_Click(object sender, EventArgs e)
        {
            //comboBoxType.SelectedIndex = 0;
            //comboBoxActivation.SelectedIndex = 0;

            // algoritmul de antrenare al perceptronului

            Random rand = new Random();

            int trainingSetSize = 4;
            int noInputs = 2;
            double alpha = 0.1; // rata de invatare

            int[,] inputs = new int[trainingSetSize, noInputs + 1]; // intrarea -1 pentru prag
            int[] outputs = new int[trainingSetSize];
            double[] weights = new double[noInputs + 1];

            inputs[0, 0] = 0; inputs[0, 1] = 0; inputs[0, 2] = -1;
            inputs[1, 0] = 0; inputs[1, 1] = 1; inputs[1, 2] = -1;
            inputs[2, 0] = 1; inputs[2, 1] = 0; inputs[2, 2] = -1;
            inputs[3, 0] = 1; inputs[3, 1] = 1; inputs[3, 2] = -1;

            outputs[0] = Convert.ToInt32(textBox00.Text);
            outputs[1] = Convert.ToInt32(textBox01.Text);
            outputs[2] = Convert.ToInt32(textBox10.Text);
            outputs[3] = Convert.ToInt32(textBox11.Text);

            weights[0] = rand.Next(10) / 10.0 - 0.5;
            weights[1] = rand.Next(10) / 10.0 - 0.5;
            weights[2] = rand.Next(10) / 10.0 - 0.5;


            // aici trebuie inclus algoritmul de antrenare pentru perceptronul cu un singur strat

            int p = 0; //numarul epocii curente(pasul curent)
            bool erori = true; //flag care indica existenta erorilor de antrenare
            int maxEpoch = 100;

            while(erori == true)
            {
                erori = false;
                for (int i = 0; i < trainingSetSize; i++)
                {
                    double sum = 0;

                    // Calcul suma ponderata
                    for (int j = 0; j < noInputs + 1; j++)
                    {
                        sum += inputs[i, j] * weights[j];
                    }

                    double y = ActivationFunction(sum);
                    // Verifica daca output-ul este diferit de output-ul dorit
                    if (y != outputs[i])
                    {
                        double error = outputs[i] - y;
                        erori = true;

                        // Actualizeaza ponderile
                        for (int j = 0; j < noInputs + 1; j++)
                        {
                            weights[j] += alpha * inputs[i, j] * error;
                        }
                    }
                }
                for (int j = 0; j < noInputs + 1; j++)
                    weights[j] = Math.Round(weights[j], 1);
                p++;
                textBoxSw13.Text = weights[0].ToString("F1", CultureInfo.InvariantCulture);
                textBoxSw23.Text = weights[1].ToString("F1", CultureInfo.InvariantCulture);
                textBoxSt3.Text = weights[2].ToString("F1", CultureInfo.InvariantCulture);
            }

            // weight fiind double, pot aparea erori de ordinul 1e-8-1e-10, care la rotunjire sa schimbe rezultatul cand linia de separare este foarte apropiata de un punct
            // la sfarsitul unei epoci de antrenare, se recomanda includerea urmatoarei secvente, care rotunjeste toate ponderile la o zecimala

        }

        /// <summary>
        /// Metoda care calculeaza iesirea perceptronului cu doi neuroni in stratul ascuns pentru intrarile x si y
        /// </summary>
        private double MLP(double x1, double x2)
        {
            // pentru fiecare neuron, se calculeaza suma intrarilor ponderate, se scade pragul si se aplica functia de activare
            // neuronul 5 are ca intrari iesirile neuronilor 3 si 4
            // iesirea neuronului 5 este iesirea retelei

            double s_neuron_3 = w13 * x1 + w23  * x2 - t3;
            double s_neuron_4 = w14 * x1 + w24 * x2 - t4;

            return ActivationFunction(s_neuron_3 * w35 + s_neuron_4 * w45 - t5);
        }

        /// <summary>
        /// Antreneaza perceptronul multistrat si seteaza ponderile in interfata
        /// </summary>
        private void buttontrainMLP_Click(object sender, EventArgs e)
        {
            comboBoxType.SelectedIndex = 1;
            comboBoxActivation.SelectedIndex = 2;

            // apel algoritm backpropagation in dll

            double[] y = new double[] {
                Convert.ToDouble(textBox00.Text),
                Convert.ToDouble(textBox01.Text),
                Convert.ToDouble(textBox10.Text),
                Convert.ToDouble(textBox11.Text) };

            BPLib.Backprop.Train(y, out double[] w);

            textBoxMw13.Text = w[0].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMw23.Text = w[1].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMt3.Text = w[2].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMw14.Text = w[3].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMw24.Text = w[4].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMt4.Text = w[5].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMw35.Text = w[6].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMw45.Text = w[7].ToString("F6", CultureInfo.InvariantCulture);
            textBoxMt5.Text = w[8].ToString("F6", CultureInfo.InvariantCulture);
        }

        //Ce am adaugat pentru algoritmul evolutiv
        private void buttonOptimize_Click(object sender, EventArgs e)
        {
            // Apelul algoritmului evolutiv pentru a optimiza rețeaua neuronală
            int populationSize = 100; // Setează dimensiunea populației
            int maxGenerations = 20; // Setează numărul maxim de generații
            double crossoverRate = 0.4; // Setează rata de crossover
            double mutationRate = 0.33; // Setează rata de mutație

            Chromosome optimizedChromosome = evolutionaryAlgorithm.Solve(optimizationProblem, populationSize, maxGenerations, crossoverRate, mutationRate);

            // Actualizează ponderile rețelei neuronale cu cele ale cromozomului optimizat
            UpdateNetworkWeights(optimizedChromosome);

        }


        /// <summary>
        /// Selecteaza tipul de perceptron atunci cand utilizatorul il alege din combobox
        /// </summary>
        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxType.SelectedIndex == 0)
            {
                groupBoxSLP.Enabled = true;
                groupBoxMLP.Enabled = false;
                NetworkFunction = SLP;
            }
            else if (comboBoxType.SelectedIndex == 1)
            {
                groupBoxSLP.Enabled = false;
                groupBoxMLP.Enabled = true;
                NetworkFunction = MLP;
            }
        }

        /// <summary>
        /// Selecteaza functia de activare atunci cand utilizatorul o alege din combobox
        /// </summary>
        private void comboBoxActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxActivation.SelectedIndex == 0)
                ActivationFunction = StepActivation;
            else if (comboBoxActivation.SelectedIndex == 1)
                ActivationFunction = SemiliniarActivation;
            else if (comboBoxActivation.SelectedIndex == 2)
                ActivationFunction = SigmoidActivation;
        }

        /// <summary>
        /// Calculeaza si afiseaza iesirile retelei pentru combinatiile de intrari (0,0), (0,1), (1,0) si (1,1)
        /// </summary>
        private void ComputeResults()
        {
            textBoxResults.Clear();
            for (int x1 = 0; x1 <= 1; x1++)
                for (int x2 = 0; x2 <= 1; x2++)
                {
                    double y = NetworkFunction(x1, x2);
                    textBoxResults.AppendText(string.Format("{0}   {1}   {2:F3}\r\n", x1, x2, y));
                }
        }

        /// <summary>
        /// Evenimentul de click al butonului "Calculeaza"
        /// </summary>
        private void buttonCompute_Click(object sender, EventArgs e)
        {
            ReadParametersFromGUI();
            ComputeResults();
            pictureBoxRegions.Refresh();
        }

        /// <summary>
        /// Desenarea regiunilor de decizie pentru retea in picturebox. Intrarile retelei sunt aici numere reale din intervalul [-0.5, 1.5]
        /// </summary>
        private void pictureBoxRegions_Paint(object sender, PaintEventArgs e)
        {
            int size = pictureBoxRegions.Width; // patrat
            Bitmap b = new Bitmap(size, size);
            Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            for (double x1 = -0.5; x1 < 1.5; x1 += 0.01)
                for (double x2 = -0.5; x2 < 1.5; x2 += 0.01)
                {
                    double y = NetworkFunction(x1, x2);
                    int gray = (int)(y * 255);
                    Color c = Color.FromArgb(gray, gray, gray);

                    int xScreen = (int)(size * (x1 + 0.5) / 2.0);
                    int yScreen = size - (int)(size * (x2 + 0.5) / 2.0);

                    g.DrawLine(new Pen(c), xScreen, yScreen, xScreen, yScreen + 1);
                }

            for (int x1 = 0; x1 <= 1; x1++)
                for (int x2 = 0; x2 <= 1; x2++)
                {
                    int xScreen = (int)(size * (x1 + 0.5) / 2.0);
                    int yScreen = size - (int)(size * (x2 + 0.5) / 2.0);
                    g.DrawEllipse(Pens.Red, xScreen - 2, yScreen - 2, 4, 4);
                }

            e.Graphics.DrawImage(b, 0, 0);
        }

        /// <summary>
        /// Testeaza daca doua numere reale sunt egale (in reprezentarea double pot aparea erori mici care fac ca testarea normala a egalitatii sa esueze)
        /// </summary>
        /// <returns></returns>
        private bool AreEqual(double x, double y)
        {
            if (Math.Abs(x - y) < 1e-8)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Informatii despre program si autor
        /// </summary>
        private void buttonAbout_Click(object sender, EventArgs e)
        {
            const string copyright =
                "Retele neuronale - Regiuni de decizie\r\n" +
                "Inteligenta artificiala, Laboratorul 12\r\n" +
                "(c)2016-2020 Florin Leon\r\n" +
                "http://florinleon.byethost24.com/lab_ia.html";

            MessageBox.Show(copyright, "Despre Retele neuronale");
        }

        /// <summary>
        /// Inchiderea programului
        /// </summary>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}