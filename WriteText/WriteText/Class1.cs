using System;
using AmiBroker;
using AmiBroker.PlugIn;
using AmiBroker.Utils;

namespace WriteText
{
    public class Class1 : IndicatorBase
    {
        [ABMethod]
        public void shubhascanner()
        {







            AFMisc.SectionBegin("Shubhalabha");

            //Taking user input 
            var Minprice = AFTools.Param("Select Minimum price ", 100, 1, 20000);
            var Maxprice = AFTools.Param("Select Maximum price ", 1000, 1, 20000);
            var Minvolume = AFTools.Param("Select Minimum Volume ", 10000, 100, 50000000);
            var Maxvolume = AFTools.Param("Select Maximum Volume ", 500000, 100, 50000000);
            var Daysavgvol = AFTools.Param("Select no. of days to calculate Avg Volume ", 10, 1, 400);
            var avgvol = AFTools.Param("Avg Volume ", 10000, 100, 50000000);
            var pricechangedays = AFTools.Param("Select no. of days to calculate % price change ", 10, 1, 400);
            var pricechangepercent = AFTools.Param("Select Price change % ", 2, 1, 100);



            var LeadEMA = AFTools.Param("Select no. of days to calculate lead AFAvg.Ema ", 5, 1, 400);
            var MediumEMA = AFTools.Param("Select no. of days to calculate Mediaum AFAvg.Ema ", 13, 1, 400);
            var LagEMA = AFTools.Param("Select no. of days to calculate Lag AFAvg.Ema ", 26, 1, 400);
            var LongEMA = AFTools.Param("Select no. of days to calculate Long AFAvg.Ema ", 200, 1, 400);
            var LeadSMA = AFTools.Param("Select no. of days to calculate lead SMA ", 10, 1, 400);
            var MediumSMA = AFTools.Param("Select no. of days to calculate Medium SMA ", 50, 1, 400);
            var LagSMA = AFTools.Param("Select no. of days to calculate Lag SMA ", 100, 1, 400);
            var LongSMA = AFTools.Param("Select no. of days to calculate Long SMA ", 200, 1, 400);
            // Buy Sell condition



            //Buy = (Close > EMA (Close,LongEMA))AND ((EMA (Close,LeadEMA) > EMA (Close,LAGEMA)));

            Buy = (AFTools.Cross(AFAvg.Ema(Close, 5), AFAvg.Ema(Close, 13)) & AFTools.Cross(AFAvg.Ema(Close, 5), AFAvg.Ema(Close, 26)) & Close > Minprice & Close < Maxprice & Volume > Minvolume & Volume < Maxvolume & avgvol < AFInd.Roc(Volume, Daysavgvol) & AFInd.Roc(Close, pricechangedays) < Close);
            AFMisc.AddColumn(AFInd.Roc(Close, pricechangedays), "close % change");
            AFMisc.AddColumn(AFInd.Roc(Volume, Daysavgvol), "volume % change");



            // Buy Sell condition backup to try anything.
            //Buy =( Close >  Lastcloseuserinput AND   DROC > shortchangeuserinput AND WROC > longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandTop( Close, BBrangeuserinput, BBwidthuserinput ) > Close );
            //Sell = ( Close >  Lastcloseuserinput AND DROC < shortchangeuserinput AND WROC < longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandBot( Close, BBrangeuserinput, BBwidthuserinput ) < Close );

            // Comment following two lines if you want to get signals without swing signals.

            //Buy=ExRem(Buy,Sell);
            //Sell=ExRem(Sell,Buy);

            // Enable following to get all data without buy sell.
            //Filter = 1;

            Filter = Buy;
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

            AFMisc.AddColumn(Buy, "Buy", 1.2f, Color.Green);
            AFMisc.AddColumn(Sell, "Sell", 1.2f, Color.Green);

            // This marks buy and sell on charts.

            AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Square, Shape.None), Color.Green, 0, Low, 30);
            AFGraph.PlotShapes(AFTools.Iif(Sell, Shape.Square, Shape.None), Color.Red, 0, High, 30);

            AFMisc.SectionEnd();
        }
        [ABMethod]
        public ATArray WriteTextFunc1(ATArray array, float period)
        {
            ATArray result = AFAvg.Ma(array, period);
            AFGraph.SetChartOptions(0, ChartOptionFlag.ShowArrows | ChartOptionFlag.ShowDates);



            var O1 = AFTools.Ref(Open, -1); var O2 = AFTools.Ref(Open, -2);
            var H1 = AFTools.Ref(High, -1); var H2 = AFTools.Ref(High, -2);
            var L1 = AFTools.Ref(Low, -1); var L2 = AFTools.Ref(Low, -2);
            var C1 = AFTools.Ref(Close, -1); var C2 = AFTools.Ref(Close, -2);

            var hammer = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (0.001f + High - Low) > 0.6f) & ((Open - Low) / (0.001f + High - Low) > 0.6f));
            var InvertedHammer = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            var BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            var BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));

            var c_Status =

            AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
            AFMisc.WriteIf(hammer, "hammer",
            AFMisc.WriteIf(InvertedHammer, "Inverted hammer",
            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing", "No pattern"))));

            Buy = Close > 0;
            Filter =Buy ;


            AFMisc.AddTextColumn(c_Status, "Candle Pattern", 5.6f, Color.Black, Color.White);

            AFMisc.AddColumn(Close, " CURRENT PRICE", 1.2f, AFTools.Iif(Close > AFTools.Ref(Close, -1), Color.Green, Color.Red));
            AFMisc.AddColumn(Volume, " VOLUME TODAY ", 1.2f, AFTools.Iif(Volume > AFAvg.Ma(Volume, 20), Color.Green, Color.Red));





            /***************************************Commentary***************************************
           ****************************************Bullish Candles****************************************/

            var C_sta = AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
            AFMisc.WriteIf(hammer, "hammer",
            AFMisc.WriteIf(InvertedHammer, "Inverted hammer",
            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing", "No pattern")))); ;
           
            
           

            //AFGraph.Plot(Close, "Price", AFTools.Iif(Close > AFTools.Ref(Close, -1), Color.Green, Color.Red), Style.Candle);
            //Title = AFMisc.EncodeColor(Color.White) + AFInfo.FullName() + "\n" + AFMisc.EncodeColor(Color.Gold) + " Date" + AFDate.Date() + Volume + "\n" + "_____ Candlestick Commentary _____" + "\n" + "_ Bullish Candles:" +
            //AFMisc.EncodeColor(Color.Green) +
            //C_sta + "\n" + AFMisc.EncodeColor(Color.Gold) + "_ Bearish Candles :" + AFMisc.EncodeColor(Color.Red) +
            //C_be + "\n" + AFMisc.EncodeColor(Color.BrightGreen) + "_____ shantesh _____";


            /**********************tool tip************************************************/
            AFMisc.SectionBegin("Shubhalabha help");

            /* GFX TootTip,  prices appears when the mouse is hovered over the top of the
            bars in the chart panel 
               this is a Drag & Drop AFL, 
               there are two small Buttons enable/disable the ToolTip,  var R= remove & var T=
            ToolTip  
               ToolTip GFX By Panos Nov-2011 ver 4   ( Link foto  http://bit.ly/oEwSxY )
            */

            AFMisc.Version(5.40f); // you need to upgrade Amibroker
            AFMisc.RequestTimedRefresh(0.1f);   // replace from (0.1) to (1) if you have Error in



            // ---- GFX Tool Tip ---- big box 

            // parameters
            var x1Rrect = AFTools.Param("X1 - x-coordinate of the upper left corner", 10, 0, 1200, 1);
            var y1Rrect = AFTools.Param("y1 - y-coordinate of the upper left corner", 20, 0, 900, 1);
            var x2Rrect = AFTools.Param("x2 - x-coordinate of the lower right corner", 300, 0, 400, 1);
            var y2Rrect = AFTools.Param("y2 - y-coordinate of the lower right corner", 150, 0, 600, 1);
            var FontSize = AFTools.Param("Fonts Size", 9, 8, 13, 1);

          

            var DX = AFMisc.GetCursorXPosition();
            var DT = AFStr.DateTimeToStr(DX );

            AFGraph.GfxSetOverlayMode(0.0);
            AFGraph.GfxSelectPen(Color.LightBlue, 3); 		// round box 
            AFGraph.GfxSelectSolidBrush(Color.LightYellow);  // inside box
            //	GfxRoundRect( 15, 80, 120, 180, 15, 15 );  // size of the big box
            AFGraph.GfxRoundRect(x1Rrect, y1Rrect, x2Rrect + x1Rrect, y2Rrect + y1Rrect, 15, 15);
            // size of the big box
            AFGraph.GfxSetBkMode(1);
            AFGraph.GfxSelectFont("Arial", FontSize, 700, ATFloat.False);
            AFGraph.GfxSetTextColor(AFTools.ParamColor("Fonts Color", Color.Black));
            AFGraph.GfxSetTextAlign(0.0);
            AFGraph.GfxSetTextColor(Color.Orange );
            AFGraph.GfxTextOut("*********** Shubhalabha pattern finder *********** ", 5 + x1Rrect, y1Rrect + FontSize);
            AFGraph.GfxSetTextColor(Color.Black );

            AFGraph.GfxTextOut("Day " + AFDate.Date(), 5 + x1Rrect, 20 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("Close : " + AFTools.Ref(Close, 0), 5 + x1Rrect, 40 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("Open : " + AFTools.Ref(Open , 0), 5 + x1Rrect, 60 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("High : " + AFTools.Ref(High , 0), 5 + x1Rrect, 80 + y1Rrect + FontSize);
        //    AFGraph.GfxTextOut("Low : " + AFDate.Lookup(Low, DX), 5 + x1Rrect, 52 + y1Rrect + FontSize);
        //    AFGraph.GfxTextOut(" %    : " + AFMath.Prec(AFDate.Lookup(AFInd.Roc(Close, 1), DX), 2) + " %", 5 + x1Rrect,
        //66 + y1Rrect + FontSize);
        //    AFGraph.GfxTextOut("High-Low  : " + AFMath.Prec(AFDate.Lookup(High - Low, DX), 4), 5 + x1Rrect, 79 + y1Rrect + FontSize);
        //    AFGraph.GfxSetTextColor(Color.Green);
            AFGraph.GfxTextOut("Candlestick Pattern  : \n" + C_sta, 5 + x1Rrect, 100 + y1Rrect + FontSize);
          //  AFGraph.GfxTextOut(" Bearish Candles Commentary  : \n" + C_be, 5 + x1Rrect, 110 + y1Rrect + FontSize);





            AFMisc.SectionEnd();



            /***********************************************/



            return result;
        }

        [ABMethod]
        public float GfxConvertBarToPixelX(float bar)
        {
            var lvb = AFMisc.Status("lastvisiblebar");
            var fvb = AFMisc.Status("firstvisiblebar");
            var pxchartleft = AFMisc.Status("pxchartleft");
            var pxchartwidth = AFMisc.Status("pxchartwidth");

            return pxchartleft + bar * pxchartwidth / (lvb - fvb + 1);
        }
    }
}
