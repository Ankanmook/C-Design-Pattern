/* 
 * MainPage.xaml.cs 
 * 
 * Version:1.22
 * 
 * Silverlight Program for T9 dictionary
 * Client Side code
 * This is CONTROLLER PART OF THE CODE
 * 
 * Revisions: 22
 *  
 * @Author : Ankan Mookerjee
 * @Date   : 11/11/2013                
 *           
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using T9Client.ServiceReferenceAnkan;
using T9Client.ServiceReferenceAjay;
using System.Diagnostics;

namespace T9Client
{
    public partial class MainPage : UserControl
    {

        /// <summary>
        /// Predictive List
        /// </summary>
        List<string> predictiveList;

        /// <summary>
        /// If turn of service is true then it is Ankan's service
        /// If turn of service is false then it is Ajay's service
        /// </summary>
        bool turnofService = true;

        /// <summary>
        /// Service Reference Object of Ajay server
        /// </summary>
        ServiceReferenceAjay.T9ServiceClient clientAjay;

        /// <summary>
        /// Service reference Object of Ankan Server
        /// </summary>
        ServiceReferenceAnkan.T9ServiceClient clientAnkan;

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
        /// Keeps the current word to be displayed
        /// </summary>
        String currentWord;

        /// <summary>
        /// Controller Object to be invoked
        /// </summary>
        Model md;

        /// <summary>
        /// Time object to keep record of time elapse
        /// </summary>
        long before;
        long after;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            clientAjay = new ServiceReferenceAjay.T9ServiceClient();
            clientAnkan = new ServiceReferenceAnkan.T9ServiceClient();

            clientAjay.GetWordsCompleted += new EventHandler<ServiceReferenceAjay.GetWordsCompletedEventArgs>(client_GetWordsCompleted);

            clientAnkan.GetDataCompleted += new EventHandler<ServiceReferenceAnkan.GetDataCompletedEventArgs>(client_GetDataCompleted);

            SetVariables();

            md = new Model();

            predictiveList = new List<string>();

            rdAnkan.IsChecked = true;
            this.turnofService = true;
        }

        /// <summary>
        /// Sets the variables and components
        /// </summary>
        public void SetVariables()
        {
            txtDictionary.Text = "";
            this.txtDictionary.IsEnabled = false;
            predictivemode = false;

            valsent = "";
            index = 0;

            before = DateTime.Now.Ticks;

            predictiveList = new List<string>();
        }



        /// <summary>
        /// Set Text Box for Predictive Mode
        /// </summary>
        /// <param name="valsent"></param>
        private void setTextboxForPredictive(string valsent)
        {

            valsent = valsent.Substring(0, valsent.Length);

            callAsyncFunction(Convert.ToInt64(valsent));
                

        }


        /// <summary>
        /// Appends Letter to the Text Box
        /// </summary>
        /// <param name="valsent"></param>
        private void appendText(String valsent)
        {
            char letter;

            letter = md.getNonPredictiveText(valsent);
            //Append the new letter
            txtDictionary.Text = txtDictionary.Text + letter.ToString();
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

            letter = md.getNonPredictiveText(valsent);

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

            if (valsent.Length > 4)
            {
                txt = valsent.Substring(valsent.Length - 1, 1);
                appendText(txt);
                this.valsent = txt;
            }

            //If the current click is after 0.5 seconds of last click
            if (milsec > 800)
            {
                txt = valsent.Substring(valsent.Length - 1, 1);
                appendText(txt);
                this.valsent = txt; 

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
                    txt = valsent.Substring(valsent.Length - 1, 1);
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
        private void setTextBox(string valsent, Int64 milsec)
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

        private void callAsyncFunction(long key)
        {
            //Ankans turns
            if (turnofService)
            {
                clientAnkan.GetDataAsync(key);
            }

            //Ajays turn
            else
            {
                clientAjay.GetWordsAsync(key);
            }

        }


        /// <summary>
        /// This Method is customized for Ajays Service Calls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_GetWordsCompleted(object sender, GetWordsCompletedEventArgs e)
        {
            try
            {
                if (predictiveList != null)
                {
                    predictiveList.Clear();
                }
                else
                {
                    predictiveList = new List<string>();
                }

                foreach (var v in e.Result)
                {
                    predictiveList.Add(v.ToString());
                }

                string firstword = predictiveList[0];

                //If the dictionary has no value for the sent number
                if (firstword == "-1")
                {
                    this.setTextboxForNonPredictive(valsent, milsec);
                }

                else
                {
                    currentWord = predictiveList.ElementAt(index);

                    //Remove the last word and add new word
                    string txt = txtDictionary.Text;
                    txt = txt.Substring(0, txt.LastIndexOf(" ") + 1);

                    txtDictionary.Text = txt;
                    txtDictionary.Text = txtDictionary.Text + currentWord;

                }

                
            }
            catch (Exception ex)
            {
                //Exception occured Dispplay message
                MessageBox.Show(ex.Message, "Exception Occurred", new MessageBoxButton());
            }
        }

        /// <summary>
        /// This method is customized for Ankans service calls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void client_GetDataCompleted(object sender, GetDataCompletedEventArgs e)
        {
            
            try
            {

                if (predictiveList != null)
                {
                    predictiveList.Clear();
                }
                else
                {
                    predictiveList = new List<string>();
                }

                foreach (var v in e.Result)
                {
                    predictiveList.Add(v.ToString());
                }

                string firstword = predictiveList[0];
                string txt = txtDictionary.Text;

                currentWord = predictiveList.ElementAt(index);

                if (firstword.Substring(0, 1) == "-")
                {
                    this.setTextboxForNonPredictive(valsent, milsec);
                }
                else
                {

                    //Remove the last word and add new word
                    txt = txt.Substring(0, txt.LastIndexOf(" ") + 1);

                    txtDictionary.Text = txt;
                    txtDictionary.Text = txtDictionary.Text + currentWord;
                }

            }
            catch (Exception ex)
            {
                //Exception occured Display Message
                MessageBox.Show(ex.Message, "Exception Occurred",new MessageBoxButton() );
            }
        }


        /// <summary>
        /// 
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
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;
            
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
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;


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
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;


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
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;

        }

        /// <summary>
        /// Button 6 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn6_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "6";
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;

        }

        /// <summary>
        /// Button 7 Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn7_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "7";
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;

        }

        /// <summary>
        /// Button 8 Click Even
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn8_Click(object sender, RoutedEventArgs e)
        {

            index = 0;
            valsent = valsent + "8";
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;

        }

        /// <summary>
        /// Button 9 Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn9_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            valsent = valsent + "9";
            after = DateTime.Now.Ticks;

            milsec = after - before;
            milsec = milsec / 10000;

            setTextBox(valsent, milsec);
            //Reset
            before = 0;
            after = 0;
            //Start
            before = DateTime.Now.Ticks;

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
            if ((valsent == "") && (txtDictionary.Text == ""))
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
                if (predictiveList == null)
                {
                    //do nothing
                }

                else
                {
                    //For the list which yields nothing
                    if (predictiveList[0] == "-1" || (predictiveList[0]).Substring(0, 1) == "-")
                    {
                        //do nothing 
                    }

                    else
                    {
                        index++;

                        //If index is equivalent to the end of list reset it to 0
                        if (index == predictiveList.Count)
                        {
                            index = 0;
                        }

                        currentWord = predictiveList.ElementAt(index);

                        //Remove the last word and add new word
                        string txt = txtDictionary.Text;
                        txt = txt.Substring(0, txt.LastIndexOf(" ") + 1);

                        txtDictionary.Text = txt;
                        txtDictionary.Text = txtDictionary.Text + currentWord;
                    }
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
            //Resetting index,predictiveList,currentWord,valsent
            index = 0;
            predictiveList = null;
            currentWord = "";
            valsent = "";

            txtDictionary.Text = txtDictionary.Text + " ";
        }

        /// <summary>
        /// Check Predictive Checkbox 
        /// Non Predictive Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPredictive_Checked(object sender, RoutedEventArgs e)
        {
            predictivemode = true;
            index = 0;
            valsent = "";
        }

        /// <summary>
        /// Un-Check Predictive Checkbox 
        /// Non Predictive Mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPredictive_Unchecked(object sender, RoutedEventArgs e)
        {
            predictivemode = false;
            index = 0;
            valsent = "";
        }


        /// <summary>
        /// Radio Button Ajay's service is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            rdAnkan.IsChecked = false;
            this.turnofService = false;
            index = 0;
            valsent = "";
        }

        /// <summary>
        /// Radio button Ankan's service is activated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rdAnkan_Checked(object sender, RoutedEventArgs e)
        {
            rdAjay.IsChecked = false;
            this.turnofService = true;
            index = 0;
            valsent = "";
        }



    }
}
