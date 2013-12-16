using System;
using AmiBroker;
using AmiBroker.PlugIn;
using AmiBroker.Utils;

namespace shubhafinalscanner
{
    public class Class1 : IndicatorBase
    {
        [ABMethod]
        public ATArray shubhafinalscannerFunc1(ATArray array, float period)
        {
            ATArray result = AFAvg.Ma(array, period);


            var O1 = AFTools.Ref(Open, -1); var O2 = AFTools.Ref(Open, -2);
            var H1 = AFTools.Ref(High, -1); var H2 = AFTools.Ref(High, -2);
            var L1 = AFTools.Ref(Low, -1); var L2 = AFTools.Ref(Low, -2);
            var C1 = AFTools.Ref(Close, -1); var C2 = AFTools.Ref(Close, -2);


            /*Body Colors*/
            var WhiteCandle = Close >= Open;
            var BlackCandle = Open > Close;

            /*Single candle Pattern */
            var smallBodyMaximum = 0.0025f;//less than 0.25%
            var LargeBodyMinimum = 0.01f;//greater than 1.0%
            var ShortWhitecandle = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle);
            var ShortblackCandle = (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);

            var LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle);
            var LongBlackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle);
            var whitemarubozu = ((Close > Open) & (High == Close) & (Open == Low));
            var blackmarubozu = ((Open > Close) & (High == Open) & (Close = Low));
            var marubozuclosingblack = LongBlackCandle & High > Open & Low == Close;
            var marubozuopeningblack = LongBlackCandle & High == Open & Low < Close;
            var marubozuclosingwhite = LongwhiteCandle & High == Close & Open > Low;
            var marubozuopeningwhite = LongwhiteCandle & High > Close & Open == Low;










            var hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (0.001f + High - Low) > 0.6f) & ((Open - Low) / (0.001f + High - Low) > 0.6f));
            var InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            var BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            var BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));

            /* Add AFInfo.Name in column*/

            var c_Status =
        AFMisc.WriteIf(marubozuclosingwhite, "marubozuclosingwhite",
        AFMisc.WriteIf(marubozuopeningwhite, "marubozuopeningwhite",
       AFMisc.WriteIf(marubozuopeningblack, "marubozuopeningblack",
                  AFMisc.WriteIf(marubozuclosingblack, "marubozuclosingblack",
                  AFMisc.WriteIf(blackmarubozu, "blackmarubozu",
                   AFMisc.WriteIf(whitemarubozu, "whitemarubozu",
                   AFMisc.WriteIf(ShortWhitecandle, "ShortWhitecandle",
                   AFMisc.WriteIf(ShortblackCandle, "ShortblackCandle",
                   AFMisc.WriteIf(LongwhiteCandle, "LongwhiteCandle",
                   AFMisc.WriteIf(LongBlackCandle, "LongBlackCandle",

                   AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                   AFMisc.WriteIf(hammer1, "hammer",
                   AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",
                   AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing", "No pattern"))))))))))))));
            Buy = Close > 0;
            Filter = Buy;


            AFMisc.AddTextColumn(c_Status, "Candle Pattern", 5.6f, Color.Black, Color.White);

            AFMisc.AddColumn(Close, " CURRENT PRICE", 1.2f, AFTools.Iif(Close > AFTools.Ref(Close, -1), Color.Black, Color.White));
            AFMisc.AddColumn(Volume, " Volume TODAY ", 1.2f, AFTools.Iif(Volume > AFAvg.Ma(Volume, 20), Color.Black, Color.White));





            /***************************************Commentary***************************************
           ****************************************Bullish Candles****************************************/

            var C_sta =
AFMisc.WriteIf(marubozuclosingwhite, "marubozuclosingwhite",
AFMisc.WriteIf(marubozuopeningblack, "marubozuopeningblack",
AFMisc.WriteIf(marubozuclosingwhite, "marubozuclosingwhite",
AFMisc.WriteIf(marubozuclosingblack, "marubozuclosingblack",
           AFMisc.WriteIf(blackmarubozu, "blackmarubozu",
           AFMisc.WriteIf(whitemarubozu, "whitemarubozu",
           AFMisc.WriteIf(ShortWhitecandle, "ShortWhitecandle",
           AFMisc.WriteIf(ShortblackCandle, "ShortblackCandle",
           AFMisc.WriteIf(LongwhiteCandle, "LongwhiteCandle",
           AFMisc.WriteIf(LongBlackCandle, "LongBlackCandle",

           AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
           AFMisc.WriteIf(hammer1, "hammer",
           AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",
           AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing", "No pattern")))))))))))))); ;







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
            var DT = AFStr.DateTimeToStr(DX);

            AFGraph.GfxSetOverlayMode(0.0f);
            AFGraph.GfxSelectPen(Color.LightBlue, 3); 		// round box 
            AFGraph.GfxSelectSolidBrush(Color.LightYellow);  // inside box
            //	GfxRoundRect( 15, 80, 120, 180, 15, 15 );  // size of the big box
            AFGraph.GfxRoundRect(x1Rrect, y1Rrect, x2Rrect + x1Rrect, y2Rrect + y1Rrect, 15, 15);
            // size of the big box
            AFGraph.GfxSetBkMode(1);
            AFGraph.GfxSelectFont("Arial", FontSize, 700, ATFloat.False);
            AFGraph.GfxSetTextColor(AFTools.ParamColor("Fonts Color", Color.Black));
            AFGraph.GfxSetTextAlign(0.0f);
            AFGraph.GfxSetTextColor(Color.Orange);
            AFGraph.GfxTextOut("*********** Shubhalabha pattern finder *********** ", 5 + x1Rrect, y1Rrect + FontSize);
            AFGraph.GfxSetTextColor(Color.Black);

            AFGraph.GfxTextOut("AFDate.Day " + AFDate.Date(), 5 + x1Rrect, 20 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("Close : " + AFTools.Ref(Close, 0), 5 + x1Rrect, 40 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("Open : " + AFTools.Ref(Open, 0), 5 + x1Rrect, 60 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("High : " + AFTools.Ref(High, 0), 5 + x1Rrect, 80 + y1Rrect + FontSize);
            AFGraph.GfxTextOut("Candlestick Pattern  : \n" + C_sta, 5 + x1Rrect, 100 + y1Rrect + FontSize);
          




            return result;
        }
    }
}
