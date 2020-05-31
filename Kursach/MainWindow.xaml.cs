using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;


namespace Kursach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NodeType node1 = NodeType.Blank;
        private NodeType node2 = NodeType.Blank;
        private NodeType previous_node1 = NodeType.Blank;
        private NodeType previous_node2 = NodeType.Blank;
        private string expLeft = "y";
        private string expRight = "u";
        private string differential_form = "";
        private Dictionary<int, int> formula_components_y;
        private Dictionary<int, int> formula_components_u;

        public enum NodeType 
        {
            [Description("Пустой")]
            Blank,
            Integrator,
            Differentiator,
            Postponer,
            Proportional,
            Inertial
        }

        public MainWindow() {
            InitializeComponent();
            string packUri = "pack://application:,,,/images/Blank.jpg";
            image1.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image2.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        private void Plot_Button_Click(object sender, RoutedEventArgs e) {
            int i = 0;

            int N = int.Parse(plotRange.Text);
            double[] u = new double[N];
            double[] y = new double[N];
            for (i = 0; i < N; i++) u[i] = 1; // единичное ступенчатое воздействие длиной N
            int t = 0;
            List<KeyValuePair<int, double>> signal = new List<KeyValuePair<int, double>>();
            int divider = 0;
            this.formula_components_y.TryGetValue(0, out divider); //коэффициент при y(l), на него надо делить
            KeyValuePair<int, int>[] ys = this.formula_components_y.ToArray();
            KeyValuePair<int, int>[] us = this.formula_components_u.ToArray();

            while (t < N) {
                for (int j = 0; j < ys.Length; j++) {
                    int delay = ys[j].Key;
                    int multiplier = ys[j].Value;
                    
                    if (delay != 0 && delay < t) {
                        y[t] -= y[t - delay] * multiplier / divider;
                    }
                }
                for (int j = 0; j < us.Length; j++)
                {
                    int delay = us[j].Key;
                    int multiplier = us[j].Value;
                    if (delay < t)
                    {
                        y[t] += u[t - delay] * multiplier / divider;
                    }
                }
                signal.Add(new KeyValuePair<int, double>(t, y[t]));
                t++;
            }
            ((ScatterSeries)cht.Series[0]).ItemsSource = signal.ToArray();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            RadioButton cb = sender as RadioButton;
            checkBoxResult1.Text = cb.Content.ToString();
            string packUri = "";
            switch (cb.Name) 
            {
                case "propNode1":
                    k1.Visibility = System.Windows.Visibility.Visible;
                    k1_label.Visibility = System.Windows.Visibility.Visible;
                    T1.Visibility = System.Windows.Visibility.Collapsed;
                    T1_label.Visibility = System.Windows.Visibility.Collapsed;
                    tau1.Visibility = System.Windows.Visibility.Collapsed;
                    tau1_label.Visibility = System.Windows.Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/proportional.jpg";
                    this.node1 = NodeType.Proportional;
                    break;
                case "integrNode1":
                    k1.Visibility = System.Windows.Visibility.Visible;
                    k1_label.Visibility = System.Windows.Visibility.Visible;
                    T1.Visibility = System.Windows.Visibility.Collapsed;
                    T1_label.Visibility = System.Windows.Visibility.Collapsed;
                    tau1.Visibility = System.Windows.Visibility.Collapsed;
                    tau1_label.Visibility = System.Windows.Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/integrator.jpg";
                    this.node1 = NodeType.Integrator;
                    break;
                case "diffNode1":
                    k1.Visibility = System.Windows.Visibility.Visible;
                    k1_label.Visibility = System.Windows.Visibility.Visible;
                    T1.Visibility = System.Windows.Visibility.Collapsed;
                    T1_label.Visibility = System.Windows.Visibility.Collapsed;
                    tau1.Visibility = System.Windows.Visibility.Collapsed;
                    tau1_label.Visibility = System.Windows.Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/differentiator.jpg";
                    this.node1 = NodeType.Differentiator;
                    break;
                case "inertNode1":
                    k1.Visibility = System.Windows.Visibility.Visible;
                    k1_label.Visibility = System.Windows.Visibility.Visible;
                    T1.Visibility = System.Windows.Visibility.Visible;
                    T1_label.Visibility = System.Windows.Visibility.Visible;
                    tau1.Visibility = System.Windows.Visibility.Collapsed;
                    tau1_label.Visibility = System.Windows.Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/inertia.jpg";
                    this.node1 = NodeType.Inertial;
                    break;
                case "postNode1":
                    tau1.Visibility = System.Windows.Visibility.Visible;
                    tau1_label.Visibility = System.Windows.Visibility.Visible;
                    T1.Visibility = System.Windows.Visibility.Collapsed;
                    T1_label.Visibility = System.Windows.Visibility.Collapsed;
                    k1.Visibility = System.Windows.Visibility.Collapsed;
                    k1_label.Visibility = System.Windows.Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/postponer.jpg";
                    this.node1 = NodeType.Postponer;
                    break;
            }
            /*if (this.previous_node1 != NodeType.Blank)
            {
                this.revert_node(1);
            }*/
            this.previous_node1 = this.node1;
            saveConfig1.Visibility = Visibility.Visible;
            image1.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            RadioButton cb = sender as RadioButton;
            checkBoxResult2.Text = cb.Content.ToString();

            string packUri = "";
            switch (cb.Name)
            {
                case "propNode2":
                    k2.Visibility = Visibility.Visible;
                    k2_label.Visibility = Visibility.Visible;
                    T2.Visibility = Visibility.Collapsed;
                    T2_label.Visibility = Visibility.Collapsed;
                    tau2.Visibility = Visibility.Collapsed;
                    tau2_label.Visibility = Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/proportional.jpg";
                    this.node2 = NodeType.Proportional;
                    break;
                case "integrNode2":
                    k2.Visibility = Visibility.Visible;
                    k2_label.Visibility = Visibility.Visible;
                    T2.Visibility = Visibility.Collapsed;
                    T2_label.Visibility = Visibility.Collapsed;
                    tau2.Visibility = Visibility.Collapsed;
                    tau2_label.Visibility = Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/integrator.jpg";
                    this.node2 = NodeType.Integrator;
                    break;
                case "diffNode2":
                    k2.Visibility = Visibility.Visible;
                    k2_label.Visibility = Visibility.Visible;
                    T2.Visibility = Visibility.Collapsed;
                    T2_label.Visibility = Visibility.Collapsed;
                    tau2.Visibility = Visibility.Collapsed;
                    tau2_label.Visibility = Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/differentiator.jpg";
                    this.node2 = NodeType.Differentiator;
                    break;
                case "inertNode2":
                    k2.Visibility = Visibility.Visible;
                    k2_label.Visibility = Visibility.Visible;
                    T2.Visibility = Visibility.Visible;
                    T2_label.Visibility = Visibility.Visible;
                    tau2.Visibility = Visibility.Collapsed;
                    tau2_label.Visibility = Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/inertia.jpg";
                    this.node2 = NodeType.Inertial;
                    break;
                case "postNode2":
                    tau2.Visibility = Visibility.Visible;
                    tau2_label.Visibility = Visibility.Visible;
                    T2.Visibility = Visibility.Collapsed;
                    T2_label.Visibility = Visibility.Collapsed;
                    k2.Visibility = Visibility.Collapsed;
                    k2_label.Visibility = Visibility.Collapsed;
                    packUri = "pack://application:,,,/images/postponer.jpg";
                    this.node2 = NodeType.Postponer;    
                    break;
            }
            /*if (this.previous_node2 != NodeType.Blank)
            {
                this.revert_node(2);
            }*/
            this.previous_node2 = this.node2;
            saveConfig2.Visibility = Visibility.Visible;
            image2.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
        }

        private void revert_node(int node_num)
        {
            NodeType nodeToSwitch = NodeType.Blank;
            if (node_num == 1)
            {
                nodeToSwitch = this.previous_node1;
            }
            else
            {
                nodeToSwitch = this.previous_node2;
            }
            int ioKp = 0;
            int ioK = 0;
            int ioP = 0;
            int ioExponenta = 0;
            int ioTpPlusOne = 0;
            switch (nodeToSwitch)
            {
                case NodeType.Differentiator:
                    ioKp = this.expRight.LastIndexOf("kp");
                    this.expRight.Remove(ioKp, 2);
                    break;
                case NodeType.Integrator:
                    ioK = this.expRight.IndexOf("k");
                    this.expRight.Remove(ioK, 1);
                    ioP = this.expLeft.IndexOf("p");
                    this.expLeft.Remove(ioP, 1);
                    break;
                case NodeType.Inertial:
                    ioTpPlusOne = this.expLeft.IndexOf("(Tp+1)");
                    this.expLeft.Remove(ioTpPlusOne, 6);
                    ioK = this.expRight.IndexOf("k");
                    this.expRight.Remove(ioK, 1);
                    break;
                case NodeType.Postponer:
                    ioExponenta = this.expRight.IndexOf("exp(-taup)");
                    this.expRight.Remove(ioExponenta, 10);
                    break;
                case NodeType.Proportional:
                    ioK = this.expRight.IndexOf("k");
                    this.expRight.Remove(ioK, 1);
                    break;
            }
            expressionBlock.Text = this.expLeft + "=" + this.expRight;
        }

        private void saveConfig1_Click(object sender, RoutedEventArgs e)
        {
            
            switch (this.node1) 
            {
                case NodeType.Differentiator:
                    this.expRight = k1.Text + "p" + this.expRight;
                    break;
                case NodeType.Integrator:
                    this.expRight = k1.Text + "*" + this.expRight;
                    this.expLeft = "p" + this.expLeft;
                    break;
                case NodeType.Postponer:
                    this.expRight = "exp(-" + tau1.Text + "p)" + this.expRight;
                    break;
                case NodeType.Proportional:
                    this.expRight = k1.Text + "*" + this.expRight;
                    break;
                case NodeType.Inertial:
                    this.expRight = k1.Text + "*" + this.expRight;
                    this.expLeft = "(" + T1.Text + "p+1)" + this.expLeft;
                    break;
            }
            expressionBlock.Text = this.expLeft + "=" + this.expRight;
        }

        private void saveConfig2_Click(object sender, RoutedEventArgs e)
        {
            switch (this.node2)
            {
                case NodeType.Differentiator:
                    this.expRight = k2.Text + "p" + this.expRight;
                    break;
                case NodeType.Integrator:
                    this.expRight = k2.Text + "*" + this.expRight;
                    this.expLeft = "p" + this.expLeft;
                    break;
                case NodeType.Postponer:
                    this.expRight = "exp(-" + tau2.Text + "p)" + this.expRight;
                    break;
                case NodeType.Proportional:
                    this.expRight = k2.Text + "*" + this.expRight;
                    break;
                case NodeType.Inertial:
                    this.expRight = k2.Text + "*" + this.expRight;
                    this.expLeft = "(" + T2.Text + "p+1)" + this.expLeft;
                    break;
            }
            expressionBlock.Text = this.expLeft + "=" + this.expRight;
        }

        private void Clear_Button_Click(object sender, RoutedEventArgs e) {
            this.expLeft = "y";
            this.expRight = "u";
            expressionBlock.Text = "";
            differentialFormBlock.Text = "";
            bracesFormBlock.Text = "";
            evaluatedExpressionBlock.Text = "";
            plotRange.Visibility = Visibility.Collapsed;
            plotRange_label.Visibility = Visibility.Collapsed;
            string packUri = "pack://application:,,,/images/Blank.jpg";
            image1.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image2.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            this.previous_node1 = this.previous_node2 = NodeType.Blank;

            checkBoxResult1.Text = "";
            checkBoxResult2.Text = "";

            k1.Text = "";
            T1.Text = "";
            tau1.Text = "";
            k1.Visibility = Visibility.Collapsed;
            T1.Visibility = Visibility.Collapsed;
            tau1.Visibility = Visibility.Collapsed;
            k1_label.Visibility = Visibility.Collapsed;
            T1_label.Visibility = Visibility.Collapsed;
            tau1_label.Visibility = Visibility.Collapsed;

            k2.Text = "";
            T2.Text = "";
            tau2.Text = "";
            k2.Visibility = Visibility.Collapsed;
            T2.Visibility = Visibility.Collapsed;
            tau2.Visibility = Visibility.Collapsed;
            k2_label.Visibility = Visibility.Collapsed;
            T2_label.Visibility = Visibility.Collapsed;
            tau2_label.Visibility = Visibility.Collapsed;

            saveConfig1.Visibility = Visibility.Collapsed;
            saveConfig2.Visibility = Visibility.Collapsed;

            plotButton.Visibility = Visibility.Collapsed;
            plotButton.IsEnabled = false;

            ((ScatterSeries)cht.Series[0]).ItemsSource = null;
        }

        private void Evaluate_Click(object sender, RoutedEventArgs e) {
            this.evaluate_expressions();
            evaluatedExpressionBlock.Text = this.expLeft + "=" + this.expRight;
            this.evaluate_differential_form();
            this.open_braces();
            this.differential_form = this.differential_form.Replace("*", "");
            this.differential_form = this.differential_form.Replace(" ", "");
            this.evaluate_alikes_y();
            this.evaluate_alikes_u();
            
            plotButton.Visibility = Visibility.Visible;
            plotButton.IsEnabled = true;
            differentialFormBlock.Text = this.differential_form;
            plotRange_label.Visibility = Visibility.Visible;
            plotRange.Visibility = Visibility.Visible;
        }

        private void evaluate_expressions() { 
            if (this.node1 == NodeType.Inertial && this.node2 == NodeType.Inertial)
            {
                //(ap + 1)(bp + 1) = ab*p^2 + (a+b)p + 1
                int a = int.Parse(T1.Text);
                int b = int.Parse(T2.Text);
                this.expLeft = a * b + "*(p^2)y + " + (a + b) + "*py + y";
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
            }
            else if (this.node1 == NodeType.Integrator && this.node2 == NodeType.Integrator)
            {
                this.expLeft = "(p^2)y";
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
            }
            else if (this.node1 == NodeType.Inertial && this.node2 == NodeType.Integrator)
            {
                //p*(T1p + 1)
                this.expLeft = T1.Text + "*(p^2)y + py";
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
            }
            else if (this.node1 == NodeType.Integrator && this.node2 == NodeType.Inertial)
            {
                //p*(T2p + 1)
                this.expLeft = T2.Text + "*(p^2)y + py";
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
            }
            else if (this.node1 == NodeType.Inertial)
            {
                this.expLeft = T1.Text + "py + y";
                if (this.node2 == NodeType.Proportional)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
                }
                else if (this.node2 == NodeType.Differentiator)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
                }
            }
            else if (this.node2 == NodeType.Inertial)
            {
                this.expLeft = T2.Text + "py + y";
                if (this.node1 == NodeType.Proportional)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
                }
                else if (this.node1 == NodeType.Differentiator)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
                }
            }
            else if (this.node1 == NodeType.Integrator)
            {
                this.expLeft = "py";
                if (this.node2 == NodeType.Proportional)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
                }
                else if (this.node2 == NodeType.Differentiator)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
                }
            }
            else if (this.node2 == NodeType.Integrator)
            {
                this.expLeft = "py";
                if (this.node1 == NodeType.Proportional)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
                }
                else if (this.node1 == NodeType.Differentiator)
                {
                    this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
                }
            }
            else if (this.node1 == NodeType.Proportional && this.node2 == NodeType.Proportional)
            {
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "u";
            }
            else if (this.node1 == NodeType.Proportional && this.node2 == NodeType.Differentiator)
            {
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
            }
            else if (this.node1 == NodeType.Differentiator && this.node2 == NodeType.Proportional)
            {
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "pu";
            }
            else if (this.node1 == NodeType.Differentiator && this.node2 == NodeType.Differentiator) 
            {
                this.expRight = int.Parse(k1.Text) * int.Parse(k2.Text) + "(p^2)u";
            }
        }

        private void evaluate_differential_form() {
            string diff_form_left = this.expLeft;
            string diff_form_right = this.expRight;

            if (diff_form_right.Contains("exp") && this.node1 == NodeType.Postponer) {
                diff_form_right = diff_form_right.Replace("u", "u(l - " + tau1.Text + ")");

                string str_to_cut = "exp(-" + tau1.Text + "p)";
                
                diff_form_right = diff_form_right.Replace(str_to_cut, "");
                bracesFormBlock.Text = str_to_cut;

                if (this.node2 == NodeType.Differentiator) 
                    diff_form_right = diff_form_right.Replace("pu(l - " + tau1.Text + ")", "(u(l - " + tau1.Text + ") - u(l - " + (int.Parse(tau1.Text) + 1) + "))");
                
                if (this.node2 == NodeType.Postponer) {
                    diff_form_right = diff_form_right.Replace("exp(-" + tau2.Text + "p)", "");
                    diff_form_right = diff_form_right.Replace("u(l - " + tau1.Text + ")", "u(l - " + (int.Parse(tau1.Text) + int.Parse(tau2.Text)) + ")");
                }
            } 
            else if (diff_form_right.Contains("exp") && this.node2 == NodeType.Postponer) {
                diff_form_right = diff_form_right.Replace("u", "u(l - " + tau2.Text + ")");

                string str_to_cut = "exp(-" + tau2.Text + "p)";
                diff_form_right = diff_form_right.Replace(str_to_cut, "");

                bracesFormBlock.Text = str_to_cut;
                if (this.node1 == NodeType.Differentiator)
                    diff_form_right = diff_form_right.Replace("pu(l - " + tau2.Text + ")", "(u(l - " + tau2.Text + ") - u(l - " + (int.Parse(tau2.Text) + 1) + "))");
                
                if (this.node1 == NodeType.Postponer) {
                    diff_form_right = diff_form_right.Replace("exp(-" + tau1.Text + "p)", "");
                    diff_form_right = diff_form_right.Replace("u(l - " + tau2.Text + ")", "u(l - " + (int.Parse(tau1.Text) + int.Parse(tau2.Text)) + ")");
                }
            }
            else if (diff_form_right.IndexOf("pu") != -1)
                diff_form_right = diff_form_right.Replace("pu", "(U(l) - U(l-1))");


            if (diff_form_left.IndexOf("(p^2)y") != -1) 
                diff_form_left = diff_form_left.Replace("(p^2)y", "(Y(l) - 2Y(l-1) + Y(l-2))");
            
            if (diff_form_left.IndexOf("py") != -1)
                diff_form_left = diff_form_left.Replace("py", "(Y(l) - Y(l-1))");
            
            if (diff_form_right.IndexOf("(p^2)u") != -1)
                diff_form_right = diff_form_right.Replace("(p^2)u", "(U(l) - 2U(l-1) + U(l-2))");

            if (diff_form_right.EndsWith("u")) 
                diff_form_right = diff_form_right.Replace("u", "u(l)");

            if (diff_form_left.IndexOf("y") != -1)
                diff_form_left = diff_form_left.Replace("y", "y(l)");
            
            if (diff_form_left.IndexOf("Y") != -1)
                diff_form_left = diff_form_left.Replace("Y", "y");
            
            if (diff_form_right.IndexOf("U") != -1)
                diff_form_right = diff_form_right.Replace("U", "u");
            
            this.differential_form = diff_form_left + " = " + diff_form_right;
        }

        private void open_braces() 
        {

            string diff_form_left = this.differential_form.Remove(this.differential_form.IndexOf("="));
            string diff_form_right = this.differential_form.Substring(this.differential_form.IndexOf("=") + 1);

            //раскрываем скобки в левой части
            if (diff_form_left.Contains("(y(l) - 2y(l-1) + y(l-2))")) {
                if (diff_form_left.StartsWith("(y(l) - 2y(l-1) + y(l-2))"))
                    diff_form_left = diff_form_left.Replace("(y(l) - 2y(l-1) + y(l-2))", "y(l)-2y(l-1)+y(l-2)");
                else {
                    int end = 0;
                    while (diff_form_left[end] >= '0' && diff_form_left[end] <= '9') end++;
                    int multiplier = int.Parse(diff_form_left.Substring(0, end));
                    string replacement = multiplier + "y(l)" + (-2 * multiplier) + "y(l-1)+" + multiplier + "y(l-2)";
                    diff_form_left = diff_form_left.Replace(multiplier + "*(y(l) - 2y(l-1) + y(l-2))", replacement);
                }
            }
            if (diff_form_left.Contains("(y(l) - y(l-1))")) {
                if (diff_form_left.StartsWith("(y(l) - y(l-1))"))
                    diff_form_left = diff_form_left.Replace("(y(l) - y(l-1))", "y(l) - y(l-1)");
                else {
                    int end = diff_form_left.IndexOf("(y(l) - y(l-1))");

                    if (end > 1 && diff_form_left[end - 2] == '+')
                        diff_form_left = diff_form_left.Replace("(y(l) - y(l-1))", "y(l)-y(l-1)");
                    else { 

                        while (diff_form_left[end] < '0' || diff_form_left[end] > '9') end--;
                        int start = end;
                        while (start > 0 && diff_form_left[start] >= '0' && diff_form_left[start] <= '9') start--;
                        int multiplier = 0;
                        if (start == end)
                            multiplier = int.Parse(diff_form_left.Substring(start, 1));
                        else
                            multiplier = int.Parse(diff_form_left.Substring(start, end - start + 1));

                        string replacement = multiplier + "y(l)" + (-1 * multiplier) + "y(l-1)";
                        if (diff_form_left.Contains("*(y(l) - y(l-1))"))
                            diff_form_left = diff_form_left.Replace(multiplier + "*(y(l) - y(l-1))", replacement);
                        else 
                            diff_form_left = diff_form_left.Replace(multiplier + "(y(l) - y(l-1))", replacement);
                    }
                }
            }

            //теперь правая часть
            if (node1 == NodeType.Differentiator && node2 == NodeType.Differentiator) {
                //надо раскрыть k1*k2*(u(l) - 2u(l-1) + u(l-2))
                int par1 = int.Parse(k1.Text);
                int par2 = int.Parse(k2.Text);
                int par = par1 * par2;
                string replacement = par + "u(l)" + (-2 * par) + "u(l-1)+" + par + "u(l-2)";
                diff_form_right = diff_form_right.Replace(par + "(u(l) - 2u(l-1) + u(l-2))", replacement);
            }
            else if (node1 == NodeType.Postponer && node2 == NodeType.Differentiator) {
                int par = int.Parse(k2.Text);
                int tau = int.Parse(tau1.Text);
                string replacement = par + "u(l-" + tau + ")-" + par + "u(l-" + (tau + 1) + ")";
                diff_form_right = diff_form_right.Replace(par + "(u(l - " + tau + ") - u(l - " + (tau + 1) + "))", replacement);
            }
            else if (node1 == NodeType.Differentiator && node2 == NodeType.Postponer) {
                int par = int.Parse(k1.Text);
                int tau = int.Parse(tau2.Text);
                string replacement = par + "u(l-" + tau + ")-" + par + "u(l-" + (tau + 1) + ")";
                diff_form_right = diff_form_right.Replace(par + "(u(l - " + tau + ") - u(l - " + (tau + 1) + "))", replacement);
            }
            else if (node1 == NodeType.Differentiator || node2 == NodeType.Differentiator) {
                //раскрываем k1*k2*(u(l) - u(l-1))
                int par1 = int.Parse(k1.Text);
                int par2 = int.Parse(k2.Text);
                int par = par1 * par2;
                string replacement = par + "u(l)" + (-1 * par) + "u(l-1)";
                diff_form_right = diff_form_right.Replace(par + "(u(l) - u(l-1))", replacement);
            }
            this.differential_form = diff_form_left + "=" + diff_form_right;
        }

        private void evaluate_alikes_y() {
            string diff_form_left = this.differential_form.Remove(this.differential_form.IndexOf("="));
            string diff_form_right = this.differential_form.Substring(this.differential_form.IndexOf("=") + 1);
            int i = 0;
            List<KeyValuePair<int, int>> lst = new List<KeyValuePair<int, int>>();
            Dictionary<int, int> parameters = new Dictionary<int, int>();
            while (i < diff_form_left.Length) {
                if (diff_form_left[i] == 'y') {
                    int j = i; // в j будет хранится индекс закрывающейся скобки
                    while (diff_form_left[j] != ')') j++;
                    int start = i, end = i;
                    int multiplier = 0;

                    while (start > 0 && diff_form_left[start] != '-' && diff_form_left[start] != '+') start--;
                    end = start;
                    while (diff_form_left[end] != 'y') end++;

                    if (start == end)
                        multiplier = 1;
                    else if (start == end - 1 && start != 0) {
                        if (diff_form_left[start] == '-')
                            multiplier = -1;
                        if (diff_form_left[start] == '+')
                            multiplier = 1;
                    }
                    else
                        multiplier = int.Parse(diff_form_left.Substring(start, end - start));

                    //разбираемся со сдвигом по времени
                    if (diff_form_left[j - 1] == 'l') {
                        int stored_multiplier = 0;
                        if (parameters.TryGetValue(0, out stored_multiplier))
                            parameters.Remove(0);

                        parameters.Add(0, multiplier + stored_multiplier);
                    } else {
                        int k = j-1;
                        while (diff_form_left[k] >= '0' && diff_form_left[k] <= '9') k--;
                        int subtrahend = 0;
                        if (k == j - 2)
                            subtrahend = int.Parse(diff_form_left.Substring(k + 1, 1));
                        else
                            subtrahend = int.Parse(diff_form_left.Substring(k + 1, j-1 - k));

                        int stored_multiplier = 0;
                        if (parameters.TryGetValue(subtrahend, out stored_multiplier))
                            parameters.Remove(subtrahend);

                        parameters.Add(subtrahend, multiplier + stored_multiplier);
                    }
                    i = j;
                }
                i++;
            }
            KeyValuePair<int, int>[] par_array = parameters.ToArray();
            string res_string = "";
            i = 0;
            while (i < par_array.Length) {
                if (par_array[i].Value != 0) {
                    if (res_string == "") {
                        if (par_array[i].Key == 0)
                            res_string += par_array[i].Value + "y(l)";
                        else
                            res_string += par_array[i].Value + "y(l-" + par_array[i].Key + ")";
                    } else {
                        res_string += (par_array[i].Value > 0) ? "+" : "";
                        if (par_array[i].Key == 0)
                            res_string += par_array[i].Value + "y(l)";
                        else
                            res_string += par_array[i].Value + "y(l-" + par_array[i].Key + ")";
                    }
                }
                i++;
            }
            bracesFormBlock.Text = res_string + "=" + diff_form_right;
            this.formula_components_y = parameters;
        }

        private void evaluate_alikes_u() {
            string diff_form_right = this.differential_form.Substring(this.differential_form.IndexOf("=") + 1);
            Console.WriteLine(diff_form_right);
            int i = 0;
            List<KeyValuePair<int, int>> lst = new List<KeyValuePair<int, int>>();
            Dictionary<int, int> parameters = new Dictionary<int, int>();
            while (i < diff_form_right.Length) {
                if (diff_form_right[i] == 'u') {
                    int j = i; // в j будет хранится индекс закрывающейся скобки
                    while (diff_form_right[j] != ')') j++;
                    int start = i, end = i;
                    int multiplier = 0;

                    while (start > 0 && diff_form_right[start] != '-' && diff_form_right[start] != '+') start--;
                    end = start;
                    while (diff_form_right[end] != 'u') end++;

                    if (start == end)
                        multiplier = 1;
                    else if (start == end - 1 && start != 0)
                    {
                        if (diff_form_right[start] == '-')
                            multiplier = -1;
                        if (diff_form_right[start] == '+')
                            multiplier = 1;
                    }
                    else
                        multiplier = int.Parse(diff_form_right.Substring(start, end - start));

                    Console.WriteLine("In the prowling cycle! multiplier = " + multiplier);

                    //разбираемся с вычитаемым
                    if (diff_form_right[j - 1] == 'l')
                    {
                        int stored_multiplier = 0;
                        if (parameters.TryGetValue(0, out stored_multiplier))
                            parameters.Remove(0);

                        parameters.Add(0, multiplier + stored_multiplier);
                        Console.WriteLine("In the prowling cycle! subtrahend = " + 0);
                    }
                    else
                    {
                        int k = j - 1;
                        while (diff_form_right[k] >= '0' && diff_form_right[k] <= '9') k--;
                        int subtrahend = 0;
                        if (k == j - 2)
                            subtrahend = int.Parse(diff_form_right.Substring(k + 1, 1));
                        else
                            subtrahend = int.Parse(diff_form_right.Substring(k + 1, j - 1 - k));

                        int stored_multiplier = 0;
                        if (parameters.TryGetValue(subtrahend, out stored_multiplier))
                            parameters.Remove(subtrahend);

                        parameters.Add(subtrahend, multiplier + stored_multiplier);
                        Console.WriteLine("In the prowling cycle! subtrahend = " + subtrahend);
                    }
                    i = j;
                }
                i++;
            }
            this.formula_components_u = parameters;
        }
    }
}