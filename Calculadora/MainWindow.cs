﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculadora {

    public partial class MainWindow : Window {
        private string currentOperation = "";
        private ObservableCollection<String> list;
        private bool isResultsOpened = false;
        public MainWindow() {
            InitializeComponent();
            list = new ObservableCollection<string>();

        }
        public void AddResultado(string resultado) {
            if(list.Count == 12) list.Remove(list.First());
            list.Add(resultado);
            listViewResultados.ItemsSource = list;
        }
        public void ButtonNumberPressed(object sender, EventArgs e) {
            Button boton = (Button)sender;
            currentOperation += boton.Content.ToString();
            labelResult.Content = currentOperation;
        }
        public void ButtonActionPressed(object sender, EventArgs e) {
            Button boton = (Button)sender;
            bool expresionEstaVacia = currentOperation == "";
            string valorBoton = boton.Content.ToString();
            if(expresionEstaVacia && valorBoton == "-")
            {
                currentOperation += valorBoton;
                labelResult.Content = currentOperation;
            } else if(!expresionEstaVacia)
            {
                bool expresionTerminaOperacion = Regex.Match(currentOperation.Last().ToString(), "[/*+-]").Success;
                if(expresionTerminaOperacion)
                    currentOperation = currentOperation[0..^1];
                        currentOperation += valorBoton;
                        labelResult.Content = currentOperation;
            }
        }
        public void ButtonDelete(object sender, EventArgs e) {
            currentOperation = "";
            labelResult.Content = "0.00";
        }
        public void ButtonCommaPressed(object sender, EventArgs e) {

            Button boton = (Button)sender;
            string valorBoton = boton.Content.ToString();
            bool expresionAcabaEnNumero = Regex.Match(currentOperation, "\\d{1}$").Success;

            string [] letras = currentOperation.Split(new Char[] { '+','-','/','*'});
            bool expresionContieneDecimal = Regex.Match(letras[letras.Length-1], "[.]").Success;
            

            if(expresionContieneDecimal)
                return;

            if(!expresionAcabaEnNumero)
                currentOperation += "0";

            currentOperation += valorBoton;

            labelResult.Content = currentOperation;
        }
        public void ButtonResult(object sender, EventArgs e) {
            string[] actions = { "+", "-", "*", "/" };
            bool expresionEstaVacia = currentOperation == "";
            if(expresionEstaVacia) return;
            bool expresionAcabaEnOperacion = Regex.Match(currentOperation.Last().ToString(), "[+-/*]$").Success;
            if(expresionAcabaEnOperacion)
                currentOperation = currentOperation[0..^1];

            labelLastResult.Content = currentOperation;
            labelResult.Content = GetResult();
            AddResultado(currentOperation);

        }
        public void ButtonErase(object sender, EventArgs e) {
            if(currentOperation.Length < 2)
            {
                currentOperation = "";
                labelResult.Content = "0.00";
            } else
            {
                currentOperation = currentOperation[0..^1];
                labelResult.Content = currentOperation;
            }
        }


        public void CopyButton(object sender, EventArgs e) {
            bool expresionEstaVacia = currentOperation == "";
            if(expresionEstaVacia)
                Clipboard.SetText("0.00");
            else
                Clipboard.SetText(currentOperation);
        }

        public string GetResult() {
            DataTable dataTable = new DataTable();
            var result = dataTable.Compute(currentOperation, "");
            currentOperation = result.ToString().Replace(",", ".");
            return currentOperation;
        }
        public void OnMoreResultsButton(object sender, EventArgs e) {
            if(isResultsOpened)
            {
                lastResults.Visibility = (Visibility)2;
                isResultsOpened = !isResultsOpened;

            } else
            {
                lastResults.Visibility = 0;
                isResultsOpened = !isResultsOpened;
            }
        }
        public void OnClearButton(object sender, EventArgs e) {
            list.Clear();
        }

    }
}


