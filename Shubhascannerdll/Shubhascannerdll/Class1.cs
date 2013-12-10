using System;
using AmiBroker;
using AmiBroker.PlugIn;
using AmiBroker.Utils;
using Microsoft.Win32;
using System.Linq;
using TicTacTec.TA.Library;
using System.IO;



namespace Shubhascannerdll
{
    public class Class1 : IndicatorBase
    {
        int iStopIndex;
        static int count = 0;
        ATArray buycondition;
        public Class1()
        {
            if (count == 0)
            {
                count++;
                licchecking();
            }
        }
        public void licchecking()
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
            string pathtostartprocess = path.Substring(0, path.Length - 20);


            RegistryKey regKey = Registry.CurrentUser;
            regKey = regKey.CreateSubKey(@"amis\");
            var amiexepath = regKey.GetValue("Amiexepath");
            try
            {
                var applicationpath = regKey.GetValue("applicationpath");
                System.Diagnostics.Process.Start(applicationpath + "scannerliccheck.exe");

            }
            catch
            {
            }


           
            var validornot = regKey.GetValue("valid");
            string valid = "";
            if (validornot != null)
            {
                valid = validornot.ToString();
            }
            else
            {

                if (count == 0)
                {
                    count++;


                    Environment.Exit(0);
                }



                validornot = regKey.GetValue("valid");
                valid = validornot.ToString();

            }

            if (valid == "working")
            {

            }
            else
            {
                Environment.Exit(0);
            }


        }
        [ABMethod]
        public void shubhascanner()
        {







            AFMisc.SectionBegin("Shubhalabha");

//Taking user input 
var Minprice = AFTools.Param("Select Minimum price ",100,1,20000);
var Maxprice = AFTools.Param("Select Maximum price ",1000,1,20000);
var Minvolume = AFTools.Param("Select Minimum Volume ",10000,100,50000000);
var Maxvolume = AFTools.Param("Select Maximum Volume ",500000,100,50000000);
var Daysavgvol = AFTools.Param("Select no. of days to calculate Avg Volume ",10,1,400);
var avgvol = AFTools.Param("Avg Volume ", 10000, 100, 50000000);
var pricechangedays = AFTools.Param("Select no. of days to calculate % price change ", 10, 1, 400);
var pricechangepercent = AFTools.Param("Select Price change % ", 2, 1, 100);



var LeadEMA = AFTools.Param("Select no. of days to calculate lead AFAvg.Ema ",5,1,400);
var MediumEMA = AFTools.Param("Select no. of days to calculate Mediaum AFAvg.Ema ",13,1,400);
var LagEMA = AFTools.Param("Select no. of days to calculate Lag AFAvg.Ema ",26,1,400);
var LongEMA = AFTools.Param("Select no. of days to calculate Long AFAvg.Ema ",200,1,400);
var LeadSMA = AFTools.Param("Select no. of days to calculate lead SMA ",10,1,400);
var MediumSMA = AFTools.Param("Select no. of days to calculate Medium SMA ",50,1,400);
var LagSMA = AFTools.Param("Select no. of days to calculate Lag SMA ",100,1,400);
var LongSMA = AFTools.Param("Select no. of days to calculate Long SMA ",200,1,400);
// Buy Sell condition



//Buy = (Close > EMA (Close,LongEMA))AND ((EMA (Close,LeadEMA) > EMA (Close,LAGEMA)));

Buy = (AFTools.Cross(AFAvg.Ema(Close, 5), AFAvg.Ema(Close, 13)) & AFTools.Cross(AFAvg.Ema(Close, 5), AFAvg.Ema(Close, 26)) & Close > Minprice & Close < Maxprice & Volume > Minvolume & Volume < Maxvolume & avgvol < AFInd.Roc(Volume, Daysavgvol) & AFInd.Roc(Close, pricechangedays) < Close);
          AFMisc.  AddColumn(AFInd.Roc(Close, pricechangedays), "close % change");
          AFMisc.AddColumn(AFInd.Roc(Volume , Daysavgvol ), "volume % change");



// Buy Sell condition backup to try anything.
//Buy =( Close >  Lastcloseuserinput AND   DROC > shortchangeuserinput AND WROC > longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandTop( Close, BBrangeuserinput, BBwidthuserinput ) > Close );
//Sell = ( Close >  Lastcloseuserinput AND DROC < shortchangeuserinput AND WROC < longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandBot( Close, BBrangeuserinput, BBwidthuserinput ) < Close );

// Comment following two lines if you want to get signals without swing signals.

//Buy=ExRem(Buy,Sell);
//Sell=ExRem(Sell,Buy);

// Enable following to get all data without buy sell.
//Filter = 1;

Filter= Buy;
//Adding column to report

//AFMisc.AddTextColumn( AFInfo.FullName(), "Company AFInfo.Name", 77 ,Color.Green);
//AFMisc.AddColumn(Volume,"Last Volume ",1.2f,Color.Green);
//AFMisc.AddColumn(Close,"Last Close  ",1.2f,Color.Green);
//AddColumn(WROC,"Long term % change",1.2,colorGreen);
//AddColumn(ATR(ATRbaruserinput) ,"Last ATR",1.2,colorGreen);
//AddColumn(EMA(Close, EMA1shorttermuserinput),"EMA1 Short term",1.2, colorGreen);
//AddColumn(EMA(Close, EMA2longtermuserinput),"EMA2 Long term",1.2, colorGreen);
//AddColumn(BBandTop(Close, BBrangeuserinput, BBwidthuserinput ),"Bollinger band upper  ",1.2,colorGreen );
//AddColumn(bbdiff,"Diff BBup & close   ",1.2,colorGreen );
//AddColumn(BBandBot(Close, BBrangeuserinput, BBwidthuserinput ),"Bollinger band lower  ",1.2,colorGreen );
//ddColumn(bbdiffb,"Diff BBbot & close   ",1.2,colorGreen );



// This prints report with Buy and Sell.

AFMisc.AddColumn( Buy, "Buy", 1.2f,Color.Green  );
AFMisc.AddColumn(Sell, "Sell", 1.2f,Color.Green );

// This marks buy and sell on charts.

AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Square, Shape.None),Color.Green, 0, Low,30);
AFGraph.PlotShapes(AFTools.Iif(Sell, Shape.Square, Shape.None),Color.Red, 0, High, 30);

AFMisc.SectionEnd();
        }
       
        [ABMethod]
        public ATArray ShubhascannerdllFunc1(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close , period);
            iStopIndex = close.Length ;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();

            




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlEngulfing(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            ATArray dist = AFInd.Atr(3000);
            float fvb = AFMisc.Status("firstvisiblebar"); 

            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                
                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Bullish Engulfing ", i, Low[i], Color.Green );
                    try
                    {
                        var x = GfxConvertBarToPixelX(i);
                        

                        var y = GfxConvertValueToPixelY(low [i + Convert.ToInt32(fvb)]);
                      //  AFGraph.GfxCircle(x, y, 20);
                       // AFGraph.GfxLineTo(x , y );

                    }
                    catch
                    {
                    }

                    
                }
                if (output[i].ToString() == "-100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Bearish Engulfing ", i, Low[i], Color.Red);
                    try
                    {
                        var x = GfxConvertBarToPixelX(i);

                        var y = GfxConvertValueToPixelY(Close[i + Convert.ToInt32(fvb)]);
                     //   AFGraph.GfxCircle(x, y, 20);

                    }
                    catch
                    {
                    }

                   

                }


            }
            // This prints report with Buy and Sell.
           




            return output    ;
        }

        [ABMethod]
        public float  GetVisibleBarCount()
        {
            var lvb = AFMisc.Status("lastvisiblebar");
            var fvb = AFMisc.Status("firstvisiblebar");

            return AFMath.Min(lvb - fvb, BarCount - fvb);
        }


        [ABMethod]
public  float  GfxConvertBarToPixelX(float  bar ) 
{ 
 var lvb = AFMisc.Status("lastvisiblebar"); 
 var fvb = AFMisc.Status("firstvisiblebar"); 
 var pxchartleft = AFMisc.Status("pxchartleft"); 
 var pxchartwidth = AFMisc.Status("pxchartwidth"); 

 return pxchartleft + bar  * pxchartwidth / ( lvb - fvb + 1 ); 
}


        [ABMethod]
public  float  GfxConvertValueToPixelY(float  Value ) 
{ 

 var Miny = AFMisc.Status("axisminy"); 
var  Maxy = AFMisc.Status("axismaxy"); 

 var pxchartbottom = AFMisc.Status("pxchartbottom"); 
 var pxchartheight = AFMisc.Status("pxchartheight"); 

 return pxchartbottom - AFMath.Floor( 0.5f + ( Value - Miny ) * pxchartheight/ ( Maxy - Miny ) ); 
} 


        [ABMethod]
        public ATArray hammer(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHammer(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {
               

                if(output[i].ToString()=="100")
                {
                    output[i] = outputEMA5[i];
                   
                    AFGraph.PlotText("hammer", i, Low[i], Color.Yellow);
                   
//AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }
                

            }
            // This prints report with Buy and Sell.


            return output ;
        }
        [ABMethod]
        public ATArray EveningDojiStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlEveningDojiStar(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("EveningDojiStar", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray EveningStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlEveningStar(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("EveningStar", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

       
        [ABMethod]
        public ATArray HangingMan(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHangingMan(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("HangingMan", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray Harami(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHarami(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Harami", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray HaramiCross(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHaramiCross(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("HaramiCross", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }
        [ABMethod]
        public ATArray ShootingStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlShootingStar(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("ShootingStar", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray Crows(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl2Crows(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Crows", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray BlackCrows(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3BlackCrows(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("BlackCrows", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray Inside(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3Inside(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Inside", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray LineStrike(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3LineStrike(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("LineStrike", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray Outside(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3Outside(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Outside", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray StarsInSouth(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3StarsInSouth(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("StarsInSouth", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray WhiteSoldiers(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.Cdl3WhiteSoldiers(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("WhiteSoldiers", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }


        [ABMethod]
        public ATArray AdvanceBlock(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlAdvanceBlock(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("AdvanceBlock", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray BeltHold(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlBeltHold(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("BeltHold", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray Breakaway(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlBreakaway(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Breakaway", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray ConcealBabysWall(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlConcealBabysWall(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("ConcealBabysWall", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray ClosingMarubozu(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlClosingMarubozu(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("ClosingMarubozu", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray CounterAttack(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlCounterAttack(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("CounterAttack", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }



        [ABMethod]
        public ATArray InvertedHammer(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlInvertedHammer(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("InvertedHammer", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray ShortLine(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlShortLine(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("ShortLine", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }


        [ABMethod]
        public ATArray MorningDojiStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlMorningDojiStar(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("MorningDojiStar", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }

        [ABMethod]
        public ATArray MorningStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlMorningStar(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("MorningStar", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }


        [ABMethod]
        public ATArray Piercing(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();




            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlPiercing(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Piercing", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.


            return output;
        }


        [ABMethod]
        public ATArray DarkCloudCover(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlDarkCloudCover(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("DarkCloudCover", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray DragonflyDoji(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlDragonflyDoji(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("DragonflyDoji", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }




        [ABMethod]
        public ATArray Doji(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlDoji(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);

            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Doji", i, Low[i], Color.Gold );
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }
               

            }
            // This prints report with Buy and Sell.

          //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }
        [ABMethod]
        public ATArray DojiStar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlDojiStar(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("DojiStar", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

          //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }




        [ABMethod]
        public ATArray HignWave(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHignWave(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("HignWave", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Hikkake(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHikkake(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Hikkake", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray HikkakeMod(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHikkakeMod(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("HikkakeMod", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray HomingPigeon(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlHomingPigeon(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("HomingPigeon", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray Identical3Crows(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlIdentical3Crows(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Identical3Crows", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray InNeck(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlInNeck(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];


                    AFGraph.PlotText("InNeck", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray Kicking(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlKicking(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Kicking", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }



        [ABMethod]
        public ATArray KickingByLength(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlKickingByLength(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("KickingByLength", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray LadderBottom(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlLadderBottom(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("LadderBottom", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray LongLeggedDoji(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlLongLeggedDoji(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("LongLeggedDoji", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray LongLine(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlLongLine(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("LongLine", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Marubozu(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlMarubozu(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Marubozu", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray MatchingLow(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlMatchingLow(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("MatchingLow", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray MatHold(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlMatHold(1, iStopIndex - 1, o, h, l, c, 5, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("MatHold", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray OnNeck(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlOnNeck(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("OnNeck", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray RickshawMan(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlRickshawMan(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("RickshawMan", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray SeperatingLines(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlSeperatingLines(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("SeperatingLines", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray RiseFall3Methods(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlRiseFall3Methods(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("RiseFall3Methods", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray SpinningTop(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlSpinningTop(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("SpinningTop", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }
        [ABMethod]
        public ATArray StalledPattern(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlStalledPattern(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("StalledPattern", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray StickSandwhich(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlStickSandwhich(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("StickSandwhich", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Takuri(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlTakuri(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Takuri", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray TasukiGap(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlTasukiGap(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("TasukiGap", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray Thrusting(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlThrusting(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Thrusting", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Tristar(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlTristar(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Tristar", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Unique3River(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlUnique3River(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("Unique3River", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }


        [ABMethod]
        public ATArray UpsideGap2Crows(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlUpsideGap2Crows(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("UpsideGap2Crows", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray XSideGap3Methods(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            int[] outputEMA5 = new int[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }
            Core.CdlXSideGap3Methods(1, iStopIndex - 1, o, h, l, c, out outBegIdx, out outNbElement, outputEMA5);


            for (int i = 0; i <= iStopIndex - 1; i++)
            {

                if (outputEMA5[i].ToString() == "100")
                {
                    output[i] = outputEMA5[i];

                    AFGraph.PlotText("XSideGap3Methods", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Ceil(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            double[] outputEMA5 = new double[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }

            Core.Ceil(1, iStopIndex - 1, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                output[i] =Convert.ToInt32( outputEMA5[i]);

                if (outputEMA5[i].ToString() == "100")
                {

                    AFGraph.PlotText("Ceil", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Cmo(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            double[] outputEMA5 = new double[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }

            Core.Cmo(1, iStopIndex - 1, c, 1, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                output[i] = Convert.ToInt32(outputEMA5[i]);

                if (outputEMA5[i].ToString() == "100")
                {

                    AFGraph.PlotText("Cmo", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Correl(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            double[] outputEMA5 = new double[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }

            Core.Correl(1, iStopIndex - 1, o, c, 1, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                output[i] = Convert.ToInt32(outputEMA5[i]);

                if (outputEMA5[i].ToString() == "100")
                {

                    AFGraph.PlotText("Correl", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }

        [ABMethod]
        public ATArray Cos(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ATArray result = AFAvg.Ma(close, period);
            iStopIndex = close.Length;
            // return iStopIndex;
            double[] outputEMA5 = new double[iStopIndex];
            Buy = (Close > 1);
            Filter = Buy;
            int outBegIdx;
            int outNbElement;
            double[] c = new double[iStopIndex];
            double[] o = new double[iStopIndex];
            double[] h = new double[iStopIndex];
            double[] l = new double[iStopIndex];
            ATArray output = new ATArray();





            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                o[i] = Convert.ToDouble(open[i]);
                h[i] = Convert.ToDouble(high[i]);
                l[i] = Convert.ToDouble(low[i]);
                c[i] = Convert.ToDouble(close[i]);

            }

            Core.Cos(1, iStopIndex - 1, c, out outBegIdx, out outNbElement, outputEMA5);
            for (int i = 0; i <= iStopIndex - 1; i++)
            {
                output[i] = Convert.ToInt32(outputEMA5[i]);

                if (outputEMA5[i].ToString() == "100")
                {

                    AFGraph.PlotText("Cos", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

            //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }
        
    }
}
