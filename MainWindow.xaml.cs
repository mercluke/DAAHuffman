using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Asgn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnFreq_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                SymbolTable frequencies = new SymbolTable();

                txtFreqTbl.Text = frequencies.ToString(txtPlain.Text);
            }
            catch(Exception exep)
            {
                txtFreqTbl.Text = "Error: " + exep.Message;
            }
        }

        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txtFreqTbl.Text.Length == 0)
                {
                    throw new ArgumentException("cannot compress without a frequency table");
                }

                SymbolTable frequencies = new SymbolTable();
                Dictionary<char, double> freqs = frequencies.getFreq(txtFreqTbl.Text);

                txtCompressed.Text = Compressor.Compress(txtPlain.Text, freqs);
                updateLengths();
            }
            catch(Exception exep)
            {
                txtCompressed.Text = "Error: " + exep.Message;
            }
        }

        private void btnDecompress_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtFreqTbl.Text.Length == 0)
                {
                    throw new ArgumentException("cannot decompress without a frequency table");
                }

                SymbolTable frequencies = new SymbolTable();
                Dictionary<char, double> freqs = frequencies.getFreq(txtFreqTbl.Text);

                txtPlain.Text = Compressor.Decompress(txtCompressed.Text, freqs);
                updateLengths();
            }
            catch(Exception exep)
            {
                txtPlain.Text = "Error: " + exep.Message;
            }
        }

        //not necessary but i did this for the sake of curiosity.
        //i was just interested in how much it compressed the text based on number of symbol
        private void updateLengths()
        {
            lblPlain.Content = "Plain Text: " + txtPlain.Text.Length + " characters long";
            lblCompressed.Content = "Compressed Text: " + txtCompressed.Text.Length + " characters long";
        }
    
    }
}
