
/* 
 * Solution.cs 
 * 
 * Version:1.8
 * 
 * C# Program for T9 dictionary
 * MVC Design Pattern
 * View WPF Module
 * 
 * Revisions: 8 
 *  
 * @Author : Ankan Mookerjee 
 * @Date   : 09/24/2013                
 *           
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Diagnostics;

namespace T9
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Counts milliseconds of time 
        /// Elapsed between two clicks
        /// </summary>
        Int64 milsec; 

        /// <summary>
        /// Checks whether t9 is in predictive mode or not
        /// </summary>
        bool predictivemode;
        
        /// <summary>
        /// Sents the input from keyboard to Controller
        /// </summary>
        string valsent;
        
        /// <summary>
        /// Keeps tab of index of list of values obtained
        /// from predictive mode
        /// </summary>
        int index;


        /// <summary>
        /// Predictive Set Obtained from Controller
        /// </summary>
        SortedSet<string> predictiveSet;
        
        /// <summary>
        /// Keeps the current word to be displayed
        /// </summary>
        String currentWord;

        /// <summary>
        /// Controller Object to be invoked
        /// </summary>
        Controller cs;

        /// <summary>
        /// StopWatch Object for finding time between two clicks
        /// </summary>
        Stopwatch stopWatch;


        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            SetVariables();
        }


        /// <summary>
        /// Sets the variables and components
        /// </summary>
        public void SetVariables()
        {
            txtDictionary.Clear();
            this.txtDictionary.AppendText(" ");
            this.txtDictionary.IsEnabled = false;
            predictivemode = false;

            valsent = "";
            index = 0 ;

            cs = new Controller();
   
            stopWatch = new Stopwatch();
            stopWatch.Start();
            

             
        }



        /// <summary>
        /// Set Text Box for Predictive Mode
        /// </summary>
        /// <param name="valsent"></param>
        private void setTextboxForPredictive(string valsent)
        {

            valsent = valsent.Substring(0, valsent.Length);
            
            
            predictiveSet = cs.getPredictiveSet(valsent);
            currentWord = predictiveSet.ElementAt(index);

            //Remove the last word and add new word
            string txt = txtDictionary.Text;
            txt = txt.Substring(0, txt.LastIndexOf(" ") + 1);

            txtDictionary.Text = txt;
            txtDictionary.AppendText(currentWord);
            
        }


        /// <summary>
        /// Appends Letter to the Text Box
        /// </summary>
        /// <param name="valsent"></param>
        private void appendText(String valsent)
        {
            char letter;

            letter = cs.getNonPredictiveText(valsent);
            //Append the new letter
            txtDictionary.AppendText(letter.ToString());
        }


        /// <summary>
        /// Replaces the last letter of the Text Box
        /// by new letter
        /// </summary>
        /// <param name="valsent"></param>
        private void replaceLetter(String valsent)
        {
            char letter;
            String txt;

            letter = cs.getNonPredictiveText(valsent);

            //Change the last letter
            txt = txtDictionary.Text;
            txt = txt.Substring(0, txt.Length - 1);
            txt = txt + letter.ToString();
            txtDictionary.Text = txt;

        }



        /// <summary>
        /// Set Text Box for Non Predictive Mode
        /// </summary>
        /// <param name="valsent"></param>
        private void setTextboxForNonPredictive(string valsent, Int64 milsec)
        {
            String txt;

            //If the current click is after 0.5 seconds of last click
            if (milsec > 500)
            {
                txt = valsent.Substring(valsent.Length - 1, 1);
                appendText(txt);
                this.valsent = txt; ;

            }

            else
            {

                //If only one value is sent
                if (valsent.Length == 1)
                {
                    appendText(valsent);
                }

                //If two values are sent
                else if (valsent.Length == 2)
                {
                    //both letters are equal
                    if (valsent[0] == valsent[1])
                    {
                        replaceLetter(valsent);
                    }

                    //Add next letter
                    else
                    {
                        appendText(valsent[1].ToString());
                        txt = valsent[1].ToString();
                        this.valsent = txt;
                    }
                    
                }

                else if (valsent.Length == 3)
                {
                    //All three values are equal -> Change last letter
                    if (valsent[0] == valsent[1] && valsent[1] == valsent[2])
                    {
                        replaceLetter(valsent);
                    }

                    //Last two are equal
                    else if (valsent[2] == valsent[1] && valsent[1] != valsent[0])
                    {
                        txt = valsent.Substring(1, 2);
                        replaceLetter(txt);
                        this.valsent = txt;
                    }
                    //First Two are equal or none are equal
                    else
                    {
                        txt = valsent.Substring(2, 1);
                        appendText(valsent);
                        this.valsent = txt;
                    }

                }
                
                //Four values entered within millisecond frame
                else if (valsent.Length == 4)
                {
                    //All four are equal
                    if (valsent[0] == valsent[1] && valsent[1] == valsent[2] && valsent[2] == valsent[3])
                    {
                        //Only condition for pqrs or wxyz
                        if (valsent == "7777" || valsent == "9999")
                        {
                            replaceLetter(valsent);
                        }
                        else
                        {
                            //For the rest only sent the last value
                            txt = valsent.Substring(3, 1);
                            replaceLetter(txt);
                            this.valsent = txt;
                        }
                    }

                    //Last three are equal
                    else if (valsent[1] == valsent[2] && valsent[2] == valsent[3] && valsent[0] != valsent[1])
                    {
                        txt = valsent.Substring(1, 3);
                        replaceLetter(txt);
                        this.valsent = txt;
                    }

                    //Last two are equal
                    else if (valsent[2] == valsent[3] && valsent[2] != valsent[1])
                    {
                        txt = valsent.Substring(2, 2);
                        replaceLetter(txt);
                        this.valsent = txt;
                    }
                    
                    //None are equal
                    else
                    {
                        txt = valsent.Substring(3, 1);
                        replaceLetter(txt);
                        this.valsent = txt;
                    }

                }

                else if (valsent.Length > 4)
                {
                    txt = valsent.Substring(valsent.Length-1, 1);
                    appendText(txt);
                    this.valsent = txt;
                }

            }
        }



        


        /// <summary>
        /// Set Text Box for Predictive or 
        /// non predictive mode
        /// </summary>
        /// <param name="valsent"></param>
        private void setTextBox(string valsent,Int64 milsec)
        {
            

            if (predictivemode)
            {
                setTextboxForPredictive(valsent);
            }
            // Non Predictive mode
            else
            {
                setTextboxForNonPredictive(valsent, milsec);
            }
        }



        /// <summary>
        /// Button one Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn1_Click(object sender, RoutedEventArgs e)
        {   
            //Do nothing
        }


        /// <summary>
        /// Button 2 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "2";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;
            

            setTextBox(valsent,milsec);
            stopWatch.Reset();
            stopWatch.Start();

        }


        /// <summary>
        /// Button 3 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn3_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "3";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();
        }


        /// <summary>
        /// Button 4 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn4_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "4";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();
        }



        /// <summary>
        /// Button 5 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn5_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "5";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();
        }



        /// <summary>
        /// Button 6 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "6";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();

        }


        /// <summary>
        /// Button 7 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "7";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();

        }



        /// <summary>
        /// Button 8 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn8_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "8";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();

        }



        /// <summary>
        /// Button 9 Click Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "9";
            stopWatch.Stop();
            milsec = stopWatch.ElapsedMilliseconds;

            setTextBox(valsent, milsec);
            stopWatch.Reset();
            stopWatch.Start();

        }



        /// <summary>
        /// Backspace Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn10_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            //If Valsent and is equivanet to none
            if ( (valsent == "") && (txtDictionary.Text == ""))
            {
                //Do nothing
            }

            else

            {
                if (valsent.Length > 0)
                {
                    valsent = valsent.Substring(0, valsent.Length - 1);
                }

                if (txtDictionary.Text.Length > 0)
                {
                    String txt = txtDictionary.Text;
                    txt = txt.Substring(0, txt.Length - 1);
                    txtDictionary.Text = txt;
                }
            }
            
        }



        /// <summary>
        /// Next word Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn11_Click(object sender, RoutedEventArgs e)
        {
            if (predictivemode == true)
            {
                if (predictiveSet == null)
                {
                    //do nothing
                }

                else
                {
                    index++;

                    if (index == predictiveSet.Count)
                    {
                        index = 0;

                    }

                    currentWord = predictiveSet.ElementAt(index);

                    //Remove the last word and add new word
                    string txt = txtDictionary.Text;
                    txt = txt.Substring(0, txt.LastIndexOf(" ") + 1);

                    txtDictionary.Text = txt;
                    txtDictionary.AppendText(currentWord);
                }
            }

            //Else do nothing
        }


        /// <summary>
        /// Space Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn12_Click(object sender, RoutedEventArgs e)
        {
            //Resetting index,predictiveSet,currentWord,valsent
            index = 0;
            predictiveSet = null;
            currentWord = "";
            valsent = "";

            txtDictionary.AppendText(" ");

            
        }


        /// <summary>
        /// Check Predictive Checkbox 
        /// Non Predictive Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPredictive_Unchecked(object sender, RoutedEventArgs e)
        {
            predictivemode = false;
            index = 0;


        }


        /// <summary>
        /// Un-Check Predictive Checkbox 
        /// Non Predictive Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPredictive_Checked(object sender, RoutedEventArgs e)
        {
            predictivemode = true;
            index = 0;
        }
    }

}
