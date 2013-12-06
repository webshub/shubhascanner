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

Buy = AFTools.Cross( AFAvg.Ema(Close,5), AFAvg.Ema(Close,13)) & AFTools.Cross( AFAvg.Ema(Close,5), AFAvg.Ema(Close,26));


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
                output[i] = outputEMA5[i];
                
                if (output[i].ToString() == "100")
                {

                    AFGraph.PlotText("Bullish Engulfing ", i, Low[i], Color.Green );
                    try
                    {
                        var x = GfxConvertBarToPixelX(i);
                        

                        var y = GfxConvertValueToPixelY(low [i + Convert.ToInt32(fvb)]);
                      //  AFGraph.GfxCircle(x, y, 20);
                        AFGraph.GfxLineTo(x , y );

                    }
                    catch
                    {
                    }

                    
                }
                if (output[i].ToString() == "-100")
                {
                    AFGraph.PlotText("Bearish Engulfing ", i, Low[i], Color.Red);
                    try
                    {
                        var x = GfxConvertBarToPixelX(i);

                        var y = GfxConvertValueToPixelY(Close[i + Convert.ToInt32(fvb)]);
                        AFGraph.GfxCircle(x, y, 20);

                    }
                    catch
                    {
                    }

                   

                }


            }
            // This prints report with Buy and Sell.
            AFMisc.AddTextColumn(AFInfo.FullName(), "Company Name", 77, Color.Green);


            AFMisc.AddColumn(Volume, "Last Volume ", 1.2f, Color.Green);
            AFMisc.AddColumn(Close, "Last Close  ", 1.2f, Color.Green);


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
                output[i] = outputEMA5[i];

                if(output[i].ToString()=="100")
                {
                   
                    AFGraph.PlotText("hammer", i, Low[i], Color.Yellow);
                   
//AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }
                

            }
            // This prints report with Buy and Sell.

           AFMisc.AddColumn(output , "signal", 77, Color.Green);

            return output ;
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
                output[i] = outputEMA5[i];

                if (output[i].ToString() == "100")
                {

                    AFGraph.PlotText("Piercing", i, Low[i], Color.Yellow);

                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

           // AFMisc.AddColumn(output, "signal", 77, Color.Green);

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
                output[i] = outputEMA5[i];

                if (output[i].ToString() == "100")
                {

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
                output[i] = outputEMA5[i];

                if (output[i].ToString() == "100")
                {

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
                output[i] = outputEMA5[i];

                if (output[i].ToString() == "100")
                {

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
                output[i] = outputEMA5[i];

                if (output[i].ToString() == "100")
                {

                    AFGraph.PlotText("DojiStar", i, Low[i], Color.Gold);
                    //AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.Green, 0, Low,30);
                }


            }
            // This prints report with Buy and Sell.

          //  AFMisc.AddColumn(output, "signal", 77, Color.Green);

            return output;
        }
          
    }
}
