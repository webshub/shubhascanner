using System;
using AmiBroker;
using AmiBroker.PlugIn;
//using AmiBroker.Utils;
using Microsoft.Win32;
using System.Linq;
//using TicTacTec.TA.Library;
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

//AFMisc.AddColumn( Buy, "Buy", 1.2f,Color.Green  );
//AFMisc.AddColumn(Sell, "Sell", 1.2f,Color.Green );

// This marks buy and sell on charts.

AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Square, Shape.None),Color.Green, 0, Low,30);
AFGraph.PlotShapes(AFTools.Iif(Sell, Shape.Square, Shape.None),Color.Red, 0, High, 30);

AFMisc.SectionEnd();
        }

      

        [ABMethod]
        public ATArray Shubhapatternfinder(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {

            //////////////////////////////////






            ///////////////////////////////////
           

            var x1Rrect = AFTools.Param("X1 - var x-coordinate of the upper left corner", 10, 0, 1200, 1);
            var y1Rrect = AFTools.Param("y1 - y-coordinate of the upper left corner", 20, 0, 900, 1);
            var x2Rrect = AFTools.Param("x2 - x-coordinate of the lower right corner", 300, 0, 400, 1);
            var y2Rrect = AFTools.Param("y2 - y-coordinate of the lower right corner", 150, 0, 600, 1);
            var FontSize = AFTools.Param("Fonts Size", 9, 8, 13, 1);
            var perchange = AFMath.Prec(AFInd.Roc(Close, 1), 2);
            var changevalue = AFMath.Prec(Close - AFTools.Ref(Close, -1), 2);

            AFGraph.GfxSetBkMode(1);
            /*
                 //       GfxSetOverlayMode(0);
                       // GfxSelectPen(colorLightBlue, 3); 		// round box 
                       // GfxSelectSolidBrush(colorLightYellow);  // inside box

                       AFGraph.GfxRoundRect(x1Rrect, y1Rrect , x2Rrect + x1Rrect, y2Rrect + y1Rrect, 15, 15);
            */
            // size of the big box
            AFGraph.GfxSetBkMode(1);
            AFGraph.GfxSelectFont("Arial", FontSize, 700, ATFloat.False);
            AFGraph.GfxSetTextColor(AFTools.ParamColor("Fonts Color", Color.White));
            AFGraph.GfxSetTextAlign(0.0f);

            var DX = AFMisc.GetCursorXPosition();
            var DY = AFMisc.GetCursorYPosition();
            var DT = AFStr.DateTimeToStr(DX);
            AFGraph.GfxSelectFont("Arial", FontSize + 12, 700, ATFloat.False);

            //    GfxTextOut("Current Price " + C + "    "+changevalue +"("+perchange +"%)" ,5 + x1Rrect, 20 + y1Rrect + FontSize);
            AFGraph.GfxTextOut(" " + Close, 5 + x1Rrect, 20 + y1Rrect + FontSize);


            AFGraph.GfxSelectFont("Arial", FontSize + 4, 700, ATFloat.False);

            AFGraph.GfxTextOut("  " + changevalue + "(" + perchange + "%)", 5 + x1Rrect, 50 + y1Rrect + FontSize);
            AFGraph.GfxSelectFont("Arial", FontSize, 700, ATFloat.False);



            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var O1 = AFTools.Ref(Open, -1); var O2 = AFTools.Ref(Open, -2); var O3 = AFTools.Ref(Open, -3); var O4 = AFTools.Ref(Open, -4); var O5 = AFTools.Ref(Open, -5); var O6 = AFTools.Ref(Open, -6); var O7 = AFTools.Ref(Open, -7); var O8 = AFTools.Ref(Open, -8); var O9 = AFTools.Ref(Open, -9);

            var H1 = AFTools.Ref(High, -1); var H2 = AFTools.Ref(High, -2); var H3 = AFTools.Ref(High, -3); var H4 = AFTools.Ref(High, -4); var H5 = AFTools.Ref(High, -5); var H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); var H7 = AFTools.Ref(High, -7); var H8 = AFTools.Ref(High, -8); var H9 = AFTools.Ref(High, -9);

            var L1 = AFTools.Ref(Low, -1); var L2 = AFTools.Ref(Low, -2); var L3 = AFTools.Ref(Low, -3); var L4 = AFTools.Ref(Low, -4); var L5 = AFTools.Ref(Low, -5); var L6 = AFTools.Ref(Low, -6); var L7 = AFTools.Ref(Low, -7); var L8 = AFTools.Ref(Low, -8); var L9 = AFTools.Ref(Low, -9);


            var C1 = AFTools.Ref(Close, -1); var C2 = AFTools.Ref(Close, -2); var C3 = AFTools.Ref(Close, -3); var C4 = AFTools.Ref(Close, -4); var C5 = AFTools.Ref(Close, -5); var C6 = AFTools.Ref(Close, -6); var C7 = AFTools.Ref(Close, -7); var C8 = AFTools.Ref(Close, -8); var C9 = AFTools.Ref(Close, -9);

            /*var BODY Colors*/
            var WhiteCandle = Close >= Open;
            var BlackCandle = Open > Close;
            var B1 = O1 - C1;
            var B2 = O2 - C2;
            var B3 = O3 - C3;
            var Avgbody = ((B1 + B2 + B3) / 3);
            var XBODY = Avgbody * 3;

          var   BODY = Open - Close;





            /*Single candle Pattern */
            var smallBodyMaximum = 0.0025f;//less than 0.25%
            var LargeBodyMinimum = 0.01f;//greater than 1.0%
            var smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            var largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            var mediumBody = !largeBody & !smallBody;
            var identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            var realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            var smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            var smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            var largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            var largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            var upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            var downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - (var MHT)
            Today is the maximum High in the last 5 days*/
           var MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - (var MHY)
            Yesterday is the maximum High in the last 5 days*/
           var  MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - (var MLT)
            Today is the minimum Low in the last 5 days*/
           var  MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - (var MLY)
            Yesterday is the minimum Low in the last 5 days*/
          var   MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /*var Doji1 definitions*/

            /*Doji1 Today - (DT)*/
           //var   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
       // var  DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            var ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            var ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            var Close1 = (Open - Close) * (0.002f);
            var Open1 = (Open * 0.002f);

            var LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            var LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
           var  Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            var whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            var blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            var marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            var marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            var marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            var marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            var BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            var WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            var hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            var InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            var HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            var ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            var BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            var BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            var BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            var BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            var BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            var BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));



            


            var PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            var EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            var MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            var MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            var GapUpx = AFPattern.GapUp();
            var GapDownx = AFPattern.GapDown();
            var BigGapUp = Low > 1.01f * H1;
            var BigGapDown = High < 0.99f * L1;
            var HugeGapUp = Low > 1.02f * H1;
            var HugeGapDown = High < 0.98f * L1;
            var DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            var DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            var consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            var consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            var consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            var consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            var Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            var Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            var TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            var tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            var percdiff = 5; /* peak detection threshold */
            var fwdcheck = 5; /* forward validity check */
            var mindistance = 10;
            var validdiff = percdiff / 400;

            var PK = AFPattern.Peak(High, percdiff, 1) == High;
            var TR = AFPattern.Trough(Low, percdiff, 1) == Low;


         var    x = AFAvg.Cum(1);
            var XPK1 = AFTools.ValueWhen(PK, x, 1);
            var XPK2 = AFTools.ValueWhen(PK, x, 2);
            var xTR1 = AFTools.ValueWhen(TR, x, 1);
            var xTr2 = AFTools.ValueWhen(TR, x, 2);

            var peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            var Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            var doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            var doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            var MINO10 = AFHL.Llv(Open, 10);
            var AVGH10 = AFAvg.Ma(High, 10);
            var AVGL10 = AFAvg.Ma(Low, 10);
            var MAXO10 = AFHL.Hhv(Open, 10);
            var MINL10 = AFHL.Llv(Low, 10);
            var MAXH10 = AFHL.Hhv(High, 10);
            var AVGH21 = AFAvg.Ma(High, 21);
            var AVGL21 = AFAvg.Ma(Low, 21);
            var MINL5 = AFHL.Llv(Low, 5);
            var BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            var BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            var BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            var BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;
           
            var DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);




           

            //////////////////////////////price 
            var MAXC20 = AFHL.Hhv(Close, 20);
            var AVGC40 = AFAvg.Ma(Close, 40);
            var AVGH5 = AFAvg.Ma(High, 5);
            var AVGL5 = AFAvg.Ma(Low, 5);
            var AVGH34 = AFAvg.Ma(High, 34);
            var AVGL34 = AFAvg.Ma(Low, 43);
            var MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            var MAXH20 = AFHL.Hhv(High, 20);
            var MINL20 = AFHL.Llv(Low, 20);
            var MINL42 = AFHL.Llv(Low, 42);
            var MAXH42 = AFHL.Hhv(High, 42);
            var MINL21 = AFHL.Llv(Low, 21);

            //to be reveiewd 
            var AVGV4 = AFAvg.Ma(Volume, 4);
            var MINL3 = AFHL.Llv(Low, 3);
          
            ///////////////////////
            var AVGC20 = AFAvg.Ma(Close, 20);
            var AVGC50 = AFAvg.Ma(Close, 50);
            var MAXH3 = AFHL.Hhv(High, 3);
           var Insideday = AFPattern.Inside();
            var DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle =
                 AFMisc.WriteIf(TweezerTop, "Tweezer Top",
 AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
 AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
 AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
 AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
 AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

 AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
  AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
 AFMisc.WriteIf(EveningStar1, "Evening Star",
 AFMisc.WriteIf(MorningStar1, "Morning Star",
 AFMisc.WriteIf(Abovethestomach, "Above the stomach",
 AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
 AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


            var pricepattern =
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel =
          
           AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
           AFMisc.WriteIf(GapUpx, "Gap Up",
           AFMisc.WriteIf(GapDownx, "Gap Down ",
           AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
           AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
           AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
           AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
           AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",

           
            AFMisc.WriteIf(HangingMan1, "Hanging Man",

             AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
           AFMisc.WriteIf(PiercingLine, "Piercing Line ",
            AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
           AFMisc.WriteIf(Doji1, "Doji",
            AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",
                  

         
            AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
            AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
           AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
            AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
            AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
           AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                      AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                      AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                       AFMisc.WriteIf(whitemarubozu, "White marubozu",
                       AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                       AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                       AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                       AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                       AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                       AFMisc.WriteIf(hammer1, "Hammer",
                       AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

           AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
           
           AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
           AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",

     

           AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                       AFMisc.WriteIf(Insideday, "Inside Day", "No pattern")))))))))))))))))))))))))))))))))));





            var price_trand =



            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom",
            AFMisc.WriteIf(TweezerTop, "Upward leading to the start of the candlestick",
            AFMisc.WriteIf(tweezerBottom, "Downward leading to the start of the candle pattern",
           
            AFMisc.WriteIf(consecutave10down, "Downward leading to the start of the candle pattern.",
            AFMisc.WriteIf(consecutave5down, "Downward leading to the start of the candle pattern.",
            AFMisc.WriteIf(consecutave10up, "Upward leading to the start of the candle pattern.",
            AFMisc.WriteIf(consecutave5up, "Upward leading to the start of the candle pattern.",
             AFMisc.WriteIf(BullishMorningDojiStar, "Downward leading to the start of the candle pattern.",
             AFMisc.WriteIf(BearishEveningDojiStar, "Upward leading to the start of the candle pattern.",
            AFMisc.WriteIf(EveningStar1, "Upward leading to the start of the candle pattern.",
            AFMisc.WriteIf(MorningStar1, "Downward leading to the start of the candle pattern.",
             AFMisc.WriteIf(Belowthestomch, "Upward leading to the start of the candlestick.",
            AFMisc.WriteIf(Abovethestomach, "Downward.",
            AFMisc.WriteIf(BullishBreakaway, "Downward leading to the start of the candle pattern.",
            AFMisc.WriteIf(BearishBreakaway, "Upward leading to the start of the candle pattern.",


           
           
            AFMisc.WriteIf(MATCHLOW, "Downward leading to the start of the candle pattern.",
            AFMisc.WriteIf(GapUpx, "Not applicable",
            AFMisc.WriteIf(GapDownx, "Not applicable ",
            AFMisc.WriteIf(BigGapUp, "Not applicable ",
            AFMisc.WriteIf(HugeGapUp, "Not applicable ",
            AFMisc.WriteIf(HugeGapDown, "Not applicable",
            AFMisc.WriteIf(DoubleGapUp, "Not applicable  ",
            AFMisc.WriteIf(DoubleGapDown, "Not applicable ",
             AFMisc.WriteIf(HangingMan1, "Not applicable",

              AFMisc.WriteIf(BullishHarami, "Downward leading to the candle pattern.",
            AFMisc.WriteIf(PiercingLine, "Downward leading to the start of the candle pattern. ",
             AFMisc.WriteIf(BearishHarami, "Upward leading to the candle pattern.",


            AFMisc.WriteIf(Doji1, "Not applicable ",
            AFMisc.WriteIf(DojiGapUp, "Upward leading to the start of the candle pattern.",
             AFMisc.WriteIf(BlackSpinningTop, "Not applicable ",
             AFMisc.WriteIf(WhiteSpinningTop, "Not applicable ",
            AFMisc.WriteIf(ShootingStar1, "Upward leading to the start of the candle pattern.",
            
             AFMisc.WriteIf(marubozuclosingwhite, "Not applicable ",
             AFMisc.WriteIf(marubozuopeningwhite, "Not applicable ",
            AFMisc.WriteIf(marubozuopeningblack, "Not applicable ",
                       AFMisc.WriteIf(marubozuclosingblack, "Not applicable ",
                       AFMisc.WriteIf(blackmarubozu, "Not applicable ",
                        AFMisc.WriteIf(whitemarubozu, "Not applicable ",
                        AFMisc.WriteIf(ShortWhitecandle, "Not applicable ",
                        AFMisc.WriteIf(ShortblackCandle, "Not applicable ",
                        AFMisc.WriteIf(LongwhiteCandle, "Not applicable ",
                        AFMisc.WriteIf(LongblackCandle, "Not applicable ",

                        AFMisc.WriteIf(BearishEngulfing, "Upward leading to the start of the candle pattern.",
                        AFMisc.WriteIf(hammer1, "Downward leading to the candle pattern.",
                        AFMisc.WriteIf(InvertedHammer1, "Downward leading to the candle pattern.",
            
                        AFMisc.WriteIf(BullishEngulfing, "Downward leading to the start of the candlestick pattern.",
                        
           AFMisc.WriteIf(BullishBeltHold, "Downward leading to the start of the candlestick pattern.",
           AFMisc.WriteIf(BearishBeltHold, "Upward leading to the start of the candle pattern.",

     

           AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                       AFMisc.WriteIf(Insideday, "Inside Day", "No pattern")))))))))))))))))))))))))))))))))))))))))))))))))));





            AFTimeFrame.TimeFrameRestore();


            /***************************************
            Commentary
            ***************************************
            Bullish Candles
            ****************************************/
var C_firstday =
AFMisc.WriteIf( doubletop , "Double top ",
AFMisc.WriteIf( doubleBot, "Double bottom",
AFMisc.WriteIf( TweezerTop, "Two adjacent candlesticks with the same (| nearly the same) High price in an uptrend.",
AFMisc.WriteIf( tweezerBottom, "Look for two candles sharing the same Low price.",

AFMisc.WriteIf(consecutave5down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave10down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave5up, "High is greater than previous High ",
AFMisc.WriteIf(consecutave10up, "High is greater than previous High",
AFMisc.WriteIf(BullishMorningDojiStar, "A tall black candle.",
 AFMisc.WriteIf(BearishEveningDojiStar, "A tall white day.",
 AFMisc.WriteIf(EveningStar1, "A tall white day.",
AFMisc.WriteIf(MorningStar1, "A tall black candle.",
AFMisc.WriteIf(Belowthestomch, "A tall white day.",
AFMisc.WriteIf(Abovethestomach, "Black candle.",
 AFMisc.WriteIf(BullishBreakaway, "The first candle is tall and black.",
            AFMisc.WriteIf(BearishBreakaway, "first candle being a tall white one.",
AFMisc.WriteIf( MATCHLOW , "A tall-bodied black candle.",
AFMisc.WriteIf(GapUpx, "Not applicable ",
AFMisc.WriteIf(GapDownx, "Not applicable",
AFMisc.WriteIf(BigGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapDown, "Not applicable ",
AFMisc.WriteIf(DoubleGapUp, "Not applicable ",
AFMisc.WriteIf(DoubleGapDown, "Not applicable ",
AFMisc.WriteIf(HangingMan1, "Look for a small bodied candle atop a long lower shadow in an uptrend.",

 AFMisc.WriteIf( BullishHarami , "A tall black candle.",
AFMisc.WriteIf(PiercingLine , "A black candle",
 AFMisc.WriteIf( BearishHarami , "A tall white candle ",
AFMisc.WriteIf( Doji1 , "Long lower shadow with a small body ",
AFMisc.WriteIf( DojiGapUp, "Price gaps higher, including the shadows, in an uptrend & forms a doji candle. A doji is one in which the opening & closing prices are within pennies of each other",
 AFMisc.WriteIf( BlackSpinningTop , "A small black body with shadows longer than the body.",
 AFMisc.WriteIf( WhiteSpinningTop , "A small white body with shadows longer than the body.",
AFMisc.WriteIf( ShootingStar1 , "Look for a tall upper shadow at least twice the body height above a small body. The body should be at / near the candle’s Low, with no lower shadow ( a very small one).",
AFMisc.WriteIf( marubozuclosingwhite, "A tall white body with a Close at the High & a lower shadow.",
 AFMisc.WriteIf( marubozuopeningwhite, "A tall white candle with an upper shadow but no lower one.",
AFMisc.WriteIf( marubozuopeningblack, "A tall black candle with a lower shadow but no upper shadow.",
           AFMisc.WriteIf( marubozuclosingblack, "A tall black candle with an upper shadow but no lower shadow.",
           AFMisc.WriteIf( blackmarubozu, "A tall black body with no shadows.",
            AFMisc.WriteIf( whitemarubozu, "A tall white body with no shadows.",
            AFMisc.WriteIf( ShortWhitecandle, "A Short candlestick with each shadow shorter than the body height.",
            AFMisc.WriteIf( ShortblackCandle, "A Short candle with upper & lower shadows each shorter than the body.",
            AFMisc.WriteIf( LongwhiteCandle, "A tall white candle with a body three times the average of the prior week/two & with shadows shorter than the body.",
            AFMisc.WriteIf( LongblackCandle, "The candle is black & the body height is at least three times the average body height of recent candles, with shadows shorter than the body.",

            AFMisc.WriteIf( BearishEngulfing, "A white candle.",
            AFMisc.WriteIf( hammer1, "Has a lower shadow between two & three times the height of a small body & little/no upper shadow. Body color is unimportant.",
            AFMisc.WriteIf( InvertedHammer1, "A tall black candle with a Close near the Low of the day.",

            AFMisc.WriteIf( BullishEngulfing, "A white candle.",
                            
           AFMisc.WriteIf(BullishBeltHold, "Look for a white candle with no lower shadow, but closing near the high.",
           AFMisc.WriteIf(BearishBeltHold, "Price opens at the high for the day and closes near the low, forming a tall black candle, often with a small lower shadow.",



           AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                       AFMisc.WriteIf(Insideday, "Inside Day", "No pattern")))))))))))))))))))))))))))))))))))))))))))))))))));

var C_secondday =
AFMisc.WriteIf(doubletop , "Double top ",
AFMisc.WriteIf(doubleBot, "Double bottom",
AFMisc.WriteIf(TweezerTop, "Two adjacent candlesticks with the same (nearly the same) High price in an uptrend.",
AFMisc.WriteIf(tweezerBottom, "Look for two candles sharing the same Low price.",
AFMisc.WriteIf(consecutave5down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave10down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave5up, "High is greater than previous High ",
AFMisc.WriteIf(consecutave10up, "High is greater than previous High",
AFMisc.WriteIf(BullishMorningDojiStar , "A doji whose body gaps below the prior body.",
 AFMisc.WriteIf(BearishEveningDojiStar , "A doji that gaps above the bodies of the two adjacent candle lines. The shadows are ! important; only the doji body need remain above the surrounding candles.",
 AFMisc.WriteIf(EveningStar1, "A small-bodied candle that gaps above the bodies of the adjacent candles. It can be either black/white.",
AFMisc.WriteIf(MorningStar1, "A small-bodied candle that gaps lower from the prior body. The color can be either black/white.",

 AFMisc.WriteIf(Belowthestomch, "The candle opens below the middle of the white candle’s body & closes at/below the middle, too.",
AFMisc.WriteIf(Abovethestomach, "White candle opening & closing at/above the midpoint of the prior black candle’s body.",
 AFMisc.WriteIf(BullishBreakaway, "Black one that opens lower, leaving a gap between the two bodies (but shadows can overlap).",
            AFMisc.WriteIf(BearishBreakaway, " White candle with a gap between the two bodies, but the shadows can overlap.",
AFMisc.WriteIf(MATCHLOW , "A black body with a Close that matches the prior close.",
AFMisc.WriteIf(GapUpx, "Not applicable ",
AFMisc.WriteIf(GapDownx, "Not applicable ",
AFMisc.WriteIf(BigGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapDown, "Not applicable ",
AFMisc.WriteIf(DoubleGapUp, "Not applicable ",
AFMisc.WriteIf(DoubleGapDown, "Not applicable",

AFMisc.WriteIf( HangingMan1, "Not applicable ",

 AFMisc.WriteIf(BullishHarami , "A small-bodied white candle. The body must be within the prior candle’s body. The tops/bottoms of the two bodies can be the same price but ! both.",
AFMisc.WriteIf(PiercingLine , "A white candle that opens below the prior candle’s Low & closes in the black body, between the midpoint & the open.",
 AFMisc.WriteIf(BearishHarami , "A small black candle. The Open & Close must be within the body of the first Day, but ignore the shadows. Either the tops/the bottoms of the bodies can be equal but ! both.",
AFMisc.WriteIf(Doji1 , "Not applicable ",
AFMisc.WriteIf(DojiGapUp, "Not applicable ",
 AFMisc.WriteIf(BlackSpinningTop , "Not applicable ",
 AFMisc.WriteIf(WhiteSpinningTop , "Not applicable ",
AFMisc.WriteIf(ShootingStar1 , "Not applicable ",
 AFMisc.WriteIf(marubozuclosingwhite, "Not applicable ",
 AFMisc.WriteIf(marubozuopeningwhite, "Not applicable ",
AFMisc.WriteIf(marubozuopeningblack, "Not applicable ",
           AFMisc.WriteIf(marubozuclosingblack, "Not applicable ",
           AFMisc.WriteIf(blackmarubozu, "Not applicable ",
            AFMisc.WriteIf(whitemarubozu, "Not applicable ",
            AFMisc.WriteIf(ShortWhitecandle, "Not applicable ",
            AFMisc.WriteIf(ShortblackCandle, "Not applicable ",
            AFMisc.WriteIf(LongwhiteCandle, "Not applicable ",
            AFMisc.WriteIf(LongblackCandle, "Not applicable ",
            AFMisc.WriteIf(BearishEngulfing, "A black candle, the body of which overlaps the white candle’s body.",
            AFMisc.WriteIf(hammer1, "Not applicable ",
            AFMisc.WriteIf(InvertedHammer1, "A small-bodied candle with a tall upper shadow & little/no lower shadow. Body cannot be a doji (otherwise it’s a gravestone doji). The Open must be below the prior AFDate.Day’s close. Candle color is unimportant.",

            AFMisc.WriteIf(BullishEngulfing, "A black candle, the body of which overlaps the white candle’s body.",
             
           AFMisc.WriteIf(BullishBeltHold, "Not applicable",
           AFMisc.WriteIf(BearishBeltHold, "Not applicable",



           AFMisc.WriteIf(Belowthestomch, "Not applicable",

                       AFMisc.WriteIf(Insideday, "Inside Day", "Not applicable")))))))))))))))))))))))))))))))))))))))))))))))))));

var C_thirdday =
AFMisc.WriteIf(doubletop , "Double top ",
AFMisc.WriteIf(doubleBot, "Double bottom",
AFMisc.WriteIf(TweezerTop, "Not applicable ",
AFMisc.WriteIf(tweezerBottom, "Not applicable ",

AFMisc.WriteIf(consecutave5down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave10down, "Low is less than previous Low ",
AFMisc.WriteIf(consecutave5up, "High is greater than previous High ",
AFMisc.WriteIf(consecutave10up, "High is greater than previous High",
AFMisc.WriteIf(BullishMorningDojiStar, "A tall white candle whose body remains above the doji’s body.",
 AFMisc.WriteIf(BearishEveningDojiStar, "A tall black candle that closes at/below the midpoint (well into the body) of the white candle.",
 AFMisc.WriteIf(EveningStar1, "A tall black candle that gaps below the prior candle & closes at least halfway down the body of the white candle.",
AFMisc.WriteIf(MorningStar1, "A tall white candle that gaps above the body of the Second Day & closes at least midway into the black body of the first day.",

AFMisc.WriteIf(Belowthestomch, "Not applicable ",
AFMisc.WriteIf(Abovethestomach, "Not applicable ",
AFMisc.WriteIf(BullishBreakaway, "candle of any color but it should have a lower close",
            AFMisc.WriteIf(BearishBreakaway, "Should have a higher close and the candle can be any color",

AFMisc.WriteIf(MATCHLOW , "Not applicable ",
AFMisc.WriteIf(GapUpx, "Not applicable ",
AFMisc.WriteIf(GapDownx, "Not applicable ",
AFMisc.WriteIf(BigGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapUp, "Not applicable ",
AFMisc.WriteIf(HugeGapDown, "Not applicable ",
AFMisc.WriteIf(DoubleGapUp, "Not applicable ",
AFMisc.WriteIf(DoubleGapDown, "Not applicable ",
AFMisc.WriteIf(HangingMan1, "Not applicable ",

 AFMisc.WriteIf(BullishHarami , "Not applicable ",
AFMisc.WriteIf(PiercingLine , "Not applicable ",
 AFMisc.WriteIf(BearishHarami , "Not applicable ",
AFMisc.WriteIf(Doji1 , "Not applicable ",
AFMisc.WriteIf(DojiGapUp, "Not applicable ",
 AFMisc.WriteIf(BlackSpinningTop , "Not applicable ",
 AFMisc.WriteIf(WhiteSpinningTop , "Not applicable ",
AFMisc.WriteIf(ShootingStar1 , "Not applicable ",

 AFMisc.WriteIf(marubozuclosingwhite, "Not applicable ",
 AFMisc.WriteIf(marubozuopeningwhite, "Not applicable ",
AFMisc.WriteIf(marubozuopeningblack, "Not applicable ",
           AFMisc.WriteIf(marubozuclosingblack, "Not applicable ",
           AFMisc.WriteIf(blackmarubozu, "Not applicable ",
            AFMisc.WriteIf(whitemarubozu, "Not applicable ",
            AFMisc.WriteIf(ShortWhitecandle, "Not applicable ",
            AFMisc.WriteIf(ShortblackCandle, "Not applicable ",
            AFMisc.WriteIf(LongwhiteCandle, "Not applicable ",
            AFMisc.WriteIf(LongblackCandle, "Not applicable ",

            AFMisc.WriteIf(BearishEngulfing, "Not applicable ",
            AFMisc.WriteIf(hammer1, "Not applicable ",
            AFMisc.WriteIf(InvertedHammer1, "Not applicable ",

            AFMisc.WriteIf(BullishEngulfing, "Not applicable",
              
           AFMisc.WriteIf(BullishBeltHold, "Not applicable",
           AFMisc.WriteIf(BearishBeltHold, "Not applicable",



           AFMisc.WriteIf(Belowthestomch, "Not applicable",

                       AFMisc.WriteIf(Insideday, "Inside Day", "Not applicable")))))))))))))))))))))))))))))))))))))))))))))))))));


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
AFMisc.Say(singlecandel );
//Say(price_trand);
////Say(C_firstday);

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BullishMorningDojiStar ","No,Yes",0)==0)
//{
//BullishMorningDojiStar =BullishMorningDojiStar ;
//}
//else
//{
//BullishMorningDojiStar =0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BearishEveningDojiStar","No,Yes",0)==0)
//{
//BearishEveningDojiStar=BearishEveningDojiStar;
//}
//else
//{
//BearishEveningDojiStar=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("EveningStar","No,Yes",0))
//{
//EveningStar1=EveningStar1;
//}
//else
//{
//EveningStar1=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("MorningStar","No,Yes",0))
//{
//MorningStar1=MorningStar1;
//}
//else
//{
//MorningStar1=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("Abovethestomach","No,Yes",0))
//{
//Abovethestomach=Abovethestomach;
//}
//else
//{
//Abovethestomach=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("consecutave5down","No,Yes",0))
//{
//consecutave5down=consecutave5down;
//}
//else
//{
//consecutave5down=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("consecutave10down","No,Yes",0))
//{
//consecutave10down=consecutave10down;
//}
//else
//{
//consecutave10down=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("consecutave5up","No,Yes",0))
//{
//consecutave5up=consecutave5up;
//}
//else
//{
//consecutave5up=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("consecutave10up","No,Yes",0))
//{
//consecutave10up=consecutave10up;
//}
//else
//{
//consecutave10up=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("TweezerTop","No,Yes",0))
//{
//TweezerTop=TweezerTop;
//}
//else
//{
//TweezerTop=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("tweezerBottom","No,Yes",0))
//{
//tweezerBottom=tweezerBottom;
//}
//else
//{
//tweezerBottom=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var BullishBreakaway","No,Yes",0))
//{
//BullishBreakaway=BullishBreakaway;
//}
//else
//{
//BullishBreakaway=0;
//}


///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var BearishBreakaway","No,Yes",0))
//{
//BearishBreakaway=BearishBreakaway;
//}
//else
//{
//BearishBreakaway=0;
//}






///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var BullishHaramiCross","No,Yes",0))
//{
//BullishHaramiCross=BullishHaramiCross;
//}
//else
//{
//BullishHaramiCross=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var BearishHaramiCross","No,Yes",0))
//{
//BearishHaramiCross=BearishHaramiCross;
//}
//else
//{
//BearishHaramiCross=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("doubletop","No,Yes",0))
//{
//doubletop=doubletop;
//}
//else
//{
//doubletop=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("doubleBot","No,Yes",0))
//{
//doubleBot=doubleBot;
//}
//else
//{
//doubleBot=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("DojiGapUp","No,Yes",0))
//{
//DojiGapUp=DojiGapUp;
//}
//else
//{
//DojiGapUp=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var oneDayreversal","No,Yes",0))
//{
//oneDayreversal=oneDayreversal;
//}
//else
//{
//oneDayreversal=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("Belowthestomch","No,Yes",0))
//{
//Belowthestomch=Belowthestomch;
//}
//else
//{
//Belowthestomch=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("MATCHLOW","No,Yes",0))
//{
//MATCHLOW=MATCHLOW;
//}
//else
//{
//MATCHLOW=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("GapUpx","No,Yes",0))
//{
//GapUpx=GapUpx;
//}
//else
//{
//GapUpx=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("GapDownx","No,Yes",0))
//{
//GapDownx=GapDownx;
//}
//else
//{
//GapDownx=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BigGapUp","No,Yes",0))
//{
//BigGapUp=BigGapUp;
//}
//else
//{
//BigGapUp=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("HugeGapUp","No,Yes",0))
//{
//HugeGapUp=HugeGapUp;
//}
//else
//{
//HugeGapUp=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("HugeGapDown","No,Yes",0))
//{
//HugeGapDown=HugeGapDown;
//}
//else
//{
//HugeGapDown=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("DoubleGapUp","No,Yes",0))
//{
//DoubleGapUp=DoubleGapUp;
//}
//else
//{
//DoubleGapUp=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("DoubleGapDown","No,Yes",0))
//{
//DoubleGapDown=DoubleGapDown;
//}
//else
//{
//DoubleGapDown=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("HangingMan1","No,Yes",0))
//{
//HangingMan1=HangingMan1;
//}
//else
//{
//HangingMan1=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BullishHarami","No,Yes",0))
//{
//BullishHarami=BullishHarami;
//}
//else
//{
//BullishHarami=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BearishHarami","No,Yes",0))
//{
//BearishHarami=BearishHarami;
//}
//else
//{
//BearishHarami=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("Doji1","No,Yes",0))
//{
//Doji1=Doji1;
//}
//else
//{
//Doji1=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BlackSpinningTop","No,Yes",0))
//{
//BlackSpinningTop=BlackSpinningTop;
//}
//else
//{
//BlackSpinningTop=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("WhiteSpinningTop","No,Yes",0))
//{
//WhiteSpinningTop=WhiteSpinningTop;
//}
//else
//{
//WhiteSpinningTop=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("ShootingStar1","No,Yes",0))
//{
//ShootingStar1=ShootingStar1;
//}
//else
//{
//ShootingStar1=0;
//}


///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("marubozuclosingwhite","No,Yes",0))
//{
//marubozuclosingwhite=marubozuclosingwhite;
//}
//else
//{
//marubozuclosingwhite=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("marubozuopeningwhite","No,Yes",0))
//{
//marubozuopeningwhite=marubozuopeningwhite;
//}
//else
//{
//marubozuopeningwhite=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("marubozuopeningblack","No,Yes",0))
//{
//marubozuopeningblack=marubozuopeningblack;
//}
//else
//{
//marubozuopeningblack=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("marubozuclosingblack","No,Yes",0))
//{
//marubozuclosingblack=marubozuclosingblack;
//}
//else
//{
//marubozuclosingblack=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("blackmarubozu","No,Yes",0))
//{
//blackmarubozu=blackmarubozu;
//}
//else
//{
//blackmarubozu=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("whitemarubozu","No,Yes",0))
//{
//whitemarubozu=whitemarubozu;
//}
//else
//{
//whitemarubozu=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("ShortWhitecandle","No,Yes",0))
//{
//ShortWhitecandle=ShortWhitecandle;
//}
//else
//{
//ShortWhitecandle=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("ShortblackCandle","No,Yes",0))
//{
//ShortblackCandle=ShortblackCandle;
//}
//else
//{
//ShortblackCandle=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("LongwhiteCandle","No,Yes",0))
//{
//LongwhiteCandle=LongwhiteCandle;
//}
//else
//{
//LongwhiteCandle=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("LongBlackCandle","No,Yes",0))
//{
//    LongblackCandle = LongblackCandle;
//}
//else
//{
//    LongblackCandle = 0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BearishEngulfing","No,Yes",0))
//{
//BearishEngulfing=BearishEngulfing;
//}
//else
//{
//BearishEngulfing=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("hammer","No,Yes",0))
//{
//hammer1=hammer1;
//}
//else
//{
//hammer1=0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("InvertedHammer","No,Yes",0))
//{
//InvertedHammer1=InvertedHammer1;
//}
//else
//{
//InvertedHammer1=0;
//}


///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("BullishEngulfing","No,Yes",0))
//{
//BullishEngulfing=BullishEngulfing;
//}
//else
//{
//BullishEngulfing=0;
//}


///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var Insideday","No,Yes",0))
//{
//Insideday=Insideday;
//}
//else
//{
//Insideday=0;
//}
///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var BearishBeltHold ","No,Yes",0))
//{
//BearishBeltHold =BearishBeltHold ;
//}
//else
//{
//BearishBeltHold =0;
//}

///* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
//if(AFTools.ParamToggle("var OutsideDay  ","No,Yes",0))
//{
//OutsideDay  =OutsideDay  ;
//}
//else
//{
//OutsideDay  =0;
//}
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


//Buy = Insideday | BullishEngulfing | InvertedHammer1 | hammer1 | BearishEngulfing | LongblackCandle | LongwhiteCandle | ShortblackCandle 
//| ShortWhitecandle | whitemarubozu | blackmarubozu | marubozuclosingblack | marubozuopeningblack | marubozuopeningwhite | marubozuclosingwhite 
//| ShootingStar1 | WhiteSpinningTop | BlackSpinningTop | Doji1 | BearishHarami  | BullishHarami | HangingMan1 | DoubleGapDown | 
// DoubleGapUp | HugeGapDown |  HugeGapUp |  BigGapUp | GapDownx | GapUpx | MATCHLOW | Belowthestomch | OutsideDay  | BearishBeltHold | BullishMorningDojiStar | oneDayreversal |  BearishEveningDojiStar 
//|  EveningStar1 | MorningStar1
// | Abovethestomach | consecutave5down | consecutave10down 
//| consecutave5up | consecutave10up | TweezerTop | tweezerBottom | BullishBreakaway | BearishBreakaway  ;


//AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Circle , Shape.None),Color.White , 0, Low,30);
//PlotShapes(IIf(Sell, shapeHollowSquare  , shapeNone),colorGreen, 0, Low,30);





  AFMisc.Version(5.40f); // you need to upgrade Amibroker
            AFMisc.RequestTimedRefresh(0.1f);   // replace from (0.1) to (1) if you have Error in



            // ---- GFX Tool Tip ---- big box 

            // parameters
       







           // GfxSetTextColor(colorOrange );
            AFGraph.GfxTextOut("*********** Shubhalabha pattern finder *********** ", 5 + x1Rrect, y1Rrect + FontSize);
          //  GfxSetTextColor(colorWhite);

       //     GfxTextOut("Day " + NumToStr( DateTime(), formatDateTime ), 5 + x1Rrect, 20 + y1Rrect + FontSize);


        //    AFMisc.AddTextColumn(singlecandel, " Pattern", 5.6f, Color.Black, Color.White);


AFGraph.GfxSelectFont("Arial", FontSize, 700, ATFloat.False);
                              AFGraph.GfxTextOut("Single candle Pattern  : \n" +singlecandel , 5 + x1Rrect, 70 + y1Rrect + FontSize);
 AFGraph.GfxTextOut("Candlestick Pattern  : \n" +doublecandle, 5 + x1Rrect, 90 + y1Rrect + FontSize);
 AFGraph.GfxTextOut("Price Pattern  : \n" +pricepattern, 5 + x1Rrect, 110 + y1Rrect + FontSize);


/* Hint: convert float expression to bool using ATFloat.IsTrue(<float expression>) */
if(AFTools.ParamToggle("Add Comments","No,Yes",0)==1)
{
 AFGraph.GfxTextOut( "Trend : "+price_trand, 5 + x1Rrect, 130 + y1Rrect + FontSize);
 AFGraph.GfxTextOut( "First Day : "+ C_firstday, 5 + x1Rrect, 150 + y1Rrect + FontSize);
 AFGraph.GfxTextOut("Second Day : "+ C_secondday , 5 + x1Rrect, 170 + y1Rrect + FontSize);
 AFGraph.GfxTextOut( "Third Day : "+C_thirdday , 5 + x1Rrect, 190+ y1Rrect + FontSize);

}




            shubhascanner();

            return null;
        }


        [ABMethod]
        public void report(ATArray open, ATArray high, ATArray low, ATArray close, float period)
        {
            ////////////////////////////////////////

            AFMisc.AddColumn(Close, "Close", 1.2f, Color.Green);
            AFMisc.AddColumn(Volume, "Volume", 1.2f, Color.Green);


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

            //AFMisc.AddColumn( Buy, "Buy", 1.2f,Color.Green  );
            //AFMisc.AddColumn(Sell, "Sell", 1.2f,Color.Green );

            // This marks buy and sell on charts.

            AFGraph.PlotShapes(AFTools.Iif(Buy, Shape.Square, Shape.None), Color.Green, 0, Low, 30);
            AFGraph.PlotShapes(AFTools.Iif(Sell, Shape.Square, Shape.None), Color.Red, 0, High, 30);

            AFMisc.SectionEnd();
            //////////////////////////////////////////

           // Buy = Close > 0;
            //Filter = Buy;
            AFMisc.AddTextColumn (AFInfo.FullName(), "Company name ");

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            
          AFTimeFrame.TimeFrameSet(TFInterval.In5Minute  ); 
            var O1 = AFTools.Ref(Open, -1); var O2 = AFTools.Ref(Open, -2); var O3 = AFTools.Ref(Open, -3); var O4 = AFTools.Ref(Open, -4); var O5 = AFTools.Ref(Open, -5); var O6 = AFTools.Ref(Open, -6); var O7 = AFTools.Ref(Open, -7); var O8 = AFTools.Ref(Open, -8); var O9 = AFTools.Ref(Open, -9);

            var H1 = AFTools.Ref(High, -1); var H2 = AFTools.Ref(High, -2); var H3 = AFTools.Ref(High, -3); var H4 = AFTools.Ref(High, -4); var H5 = AFTools.Ref(High, -5); var H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); var H7 = AFTools.Ref(High, -7); var H8 = AFTools.Ref(High, -8); var H9 = AFTools.Ref(High, -9);

            var L1 = AFTools.Ref(Low, -1); var L2 = AFTools.Ref(Low, -2); var L3 = AFTools.Ref(Low, -3); var L4 = AFTools.Ref(Low, -4); var L5 = AFTools.Ref(Low, -5); var L6 = AFTools.Ref(Low, -6); var L7 = AFTools.Ref(Low, -7); var L8 = AFTools.Ref(Low, -8); var L9 = AFTools.Ref(Low, -9);


            var C1 = AFTools.Ref(Close, -1); var C2 = AFTools.Ref(Close, -2); var C3 = AFTools.Ref(Close, -3); var C4 = AFTools.Ref(Close, -4); var C5 = AFTools.Ref(Close, -5); var C6 = AFTools.Ref(Close, -6); var C7 = AFTools.Ref(Close, -7); var C8 = AFTools.Ref(Close, -8); var C9 = AFTools.Ref(Close, -9);

            /*var BODY Colors*/
            var WhiteCandle = Close >= Open;
            var BlackCandle = Open > Close;
            var B1 = O1 - C1;
            var B2 = O2 - C2;
            var B3 = O3 - C3;
            var Avgbody = ((B1 + B2 + B3) / 3);
            var XBODY = Avgbody * 3;

            var BODY = Open - Close;





            /*Single candle Pattern */
            var smallBodyMaximum = 0.0025f;//less than 0.25%
            var LargeBodyMinimum = 0.01f;//greater than 1.0%
            var smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            var largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            var mediumBody = !largeBody & !smallBody;
            var identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            var realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            var smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            var smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            var largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            var largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            var upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            var downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - (var MHT)
            Today is the maximum High in the last 5 days*/
            var MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - (var MHY)
            Yesterday is the maximum High in the last 5 days*/
            var MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - (var MLT)
            Today is the minimum Low in the last 5 days*/
            var MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - (var MLY)
            Yesterday is the minimum Low in the last 5 days*/
            var MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /*var Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //var   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            // var  DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            var ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            var ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            var Close1 = (Open - Close) * (0.002f);
            var Open1 = (Open * 0.002f);

            var LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            var LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
            var Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            var whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            var blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            var marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            var marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            var marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            var marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            var BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            var WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            var hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            var InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            var HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            var ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            var BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            var BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            var BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            var BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            var BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            var BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));




            var PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            var EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            var MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            var MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            var GapUpx = AFPattern.GapUp();
            var GapDownx = AFPattern.GapDown();
            var BigGapUp = Low > 1.01f * H1;
            var BigGapDown = High < 0.99f * L1;
            var HugeGapUp = Low > 1.02f * H1;
            var HugeGapDown = High < 0.98f * L1;
            var DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            var DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            var consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            var consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            var consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            var consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            var Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            var Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            var TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            var tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            var percdiff = 5; /* peak detection threshold */
            var fwdcheck = 5; /* forward validity check */
            var mindistance = 10;
            var validdiff = percdiff / 400;

            var PK = AFPattern.Peak(High, percdiff, 1) == High;
            var TR = AFPattern.Trough(Low, percdiff, 1) == Low;


            var x = AFAvg.Cum(1);
            var XPK1 = AFTools.ValueWhen(PK, x, 1);
            var XPK2 = AFTools.ValueWhen(PK, x, 2);
            var xTR1 = AFTools.ValueWhen(TR, x, 1);
            var xTr2 = AFTools.ValueWhen(TR, x, 2);

            var peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            var Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            var doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            var doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            var MINO10 = AFHL.Llv(Open, 10);
            var AVGH10 = AFAvg.Ma(High, 10);
            var AVGL10 = AFAvg.Ma(Low, 10);
            var MAXO10 = AFHL.Hhv(Open, 10);
            var MINL10 = AFHL.Llv(Low, 10);
            var MAXH10 = AFHL.Hhv(High, 10);
            var AVGH21 = AFAvg.Ma(High, 21);
            var AVGL21 = AFAvg.Ma(Low, 21);
            var MINL5 = AFHL.Llv(Low, 5);
            var BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            var BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            var BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            var BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

            var DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);




          

            //////////////////////////////price 
            var MAXC20 = AFHL.Hhv(Close, 20);
            var AVGC40 = AFAvg.Ma(Close, 40);
            var AVGH5 = AFAvg.Ma(High, 5);
            var AVGL5 = AFAvg.Ma(Low, 5);
            var AVGH34 = AFAvg.Ma(High, 34);
            var AVGL34 = AFAvg.Ma(Low, 43);
            var MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            var MAXH20 = AFHL.Hhv(High, 20);
            var MINL20 = AFHL.Llv(Low, 20);
            var MINL42 = AFHL.Llv(Low, 42);
            var MAXH42 = AFHL.Hhv(High, 42);
            var MINL21 = AFHL.Llv(Low, 21);

           
            var AVGV4 = AFAvg.Ma(Volume, 4);
            var MINL3 = AFHL.Llv(Low, 3);
           
            ///////////////////////
            var AVGC20 = AFAvg.Ma(Close, 20);
            var AVGC50 = AFAvg.Ma(Close, 50);
            var MAXH3 = AFHL.Hhv(High, 3);
             var Insideday = AFPattern.Inside();
            var DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle =
                        AFMisc.WriteIf(TweezerTop, "Tweezer Top",
        AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
        AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
        AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
        AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
        AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

        AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
         AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
        AFMisc.WriteIf(EveningStar1, "Evening Star",
        AFMisc.WriteIf(MorningStar1, "Morning Star",
        AFMisc.WriteIf(Abovethestomach, "Above the stomach",
        AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
        AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


            var pricepattern =
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel =

           AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
           AFMisc.WriteIf(GapUpx, "Gap Up",
           AFMisc.WriteIf(GapDownx, "Gap Down ",
           AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
           AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
           AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
           AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
           AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


            AFMisc.WriteIf(HangingMan1, "Hanging Man",

             AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
           AFMisc.WriteIf(PiercingLine, "Piercing Line ",
            AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
           AFMisc.WriteIf(Doji1, "Doji",
            AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



            AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
            AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
           AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
            AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
            AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
           AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                      AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                      AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                       AFMisc.WriteIf(whitemarubozu, "White marubozu",
                       AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                       AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                       AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                       AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                       AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                       AFMisc.WriteIf(hammer1, "Hammer",
                       AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

           AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
           
           AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
           AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



           AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                       AFMisc.WriteIf(Insideday, "Inside Day", "No pattern")))))))))))))))))))))))))))))))))));





            AFMisc.AddTextColumn(singlecandel, "5min Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle, "5min Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern, "5min Price  Pattern", 5.6f, Color.Black, Color.White);


            AFTimeFrame.TimeFrameRestore();

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            AFTimeFrame.TimeFrameSet(TFInterval.In15Minute ); // switch now to hourly 
            O1 = AFTools.Ref(Open, -1); O2 = AFTools.Ref(Open, -2); O3 = AFTools.Ref(Open, -3); O4 = AFTools.Ref(Open, -4); O5 = AFTools.Ref(Open, -5); O6 = AFTools.Ref(Open, -6); O7 = AFTools.Ref(Open, -7); O8 = AFTools.Ref(Open, -8); O9 = AFTools.Ref(Open, -9);

            H1 = AFTools.Ref(High, -1); H2 = AFTools.Ref(High, -2); H3 = AFTools.Ref(High, -3); H4 = AFTools.Ref(High, -4); H5 = AFTools.Ref(High, -5); H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); H7 = AFTools.Ref(High, -7); H8 = AFTools.Ref(High, -8); H9 = AFTools.Ref(High, -9);

            L1 = AFTools.Ref(Low, -1); L2 = AFTools.Ref(Low, -2); L3 = AFTools.Ref(Low, -3); L4 = AFTools.Ref(Low, -4); L5 = AFTools.Ref(Low, -5); L6 = AFTools.Ref(Low, -6); L7 = AFTools.Ref(Low, -7); L8 = AFTools.Ref(Low, -8); L9 = AFTools.Ref(Low, -9);


            C1 = AFTools.Ref(Close, -1); C2 = AFTools.Ref(Close, -2); C3 = AFTools.Ref(Close, -3); C4 = AFTools.Ref(Close, -4); C5 = AFTools.Ref(Close, -5); C6 = AFTools.Ref(Close, -6); C7 = AFTools.Ref(Close, -7); C8 = AFTools.Ref(Close, -8); C9 = AFTools.Ref(Close, -9);

            /* BODY Colors*/
            WhiteCandle = Close >= Open;
            BlackCandle = Open > Close;
            B1 = O1 - C1;
            B2 = O2 - C2;
            B3 = O3 - C3;
            Avgbody = ((B1 + B2 + B3) / 3);
            XBODY = Avgbody * 3;

            BODY = Open - Close;





            /*Single candle Pattern */
            smallBodyMaximum = 0.0025f;//less than 0.25%
            LargeBodyMinimum = 0.01f;//greater than 1.0%
            smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            mediumBody = !largeBody & !smallBody;
            identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - ( MHT)
            Today is the maximum High in the last 5 days*/
            MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - ( MHY)
            Yesterday is the maximum High in the last 5 days*/
            MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - ( MLT)
            Today is the minimum Low in the last 5 days*/
            MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - ( MLY)
            Yesterday is the minimum Low in the last 5 days*/
            MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /* Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            //   DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            Close1 = (Open - Close) * (0.002f);
            Open1 = (Open * 0.002f);

            LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
            Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));





            PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            GapUpx = AFPattern.GapUp();
            GapDownx = AFPattern.GapDown();
            BigGapUp = Low > 1.01f * H1;
            BigGapDown = High < 0.99f * L1;
            HugeGapUp = Low > 1.02f * H1;
            HugeGapDown = High < 0.98f * L1;
            DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            percdiff = 5; /* peak detection threshold */
            fwdcheck = 5; /* forward validity check */
            mindistance = 10;
            validdiff = percdiff / 400;

            PK = AFPattern.Peak(High, percdiff, 1) == High;
            TR = AFPattern.Trough(Low, percdiff, 1) == Low;


            x = AFAvg.Cum(1);
            XPK1 = AFTools.ValueWhen(PK, x, 1);
            XPK2 = AFTools.ValueWhen(PK, x, 2);
            xTR1 = AFTools.ValueWhen(TR, x, 1);
            xTr2 = AFTools.ValueWhen(TR, x, 2);

            peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            MINO10 = AFHL.Llv(Open, 10);
            AVGH10 = AFAvg.Ma(High, 10);
            AVGL10 = AFAvg.Ma(Low, 10);
            MAXO10 = AFHL.Hhv(Open, 10);
            MINL10 = AFHL.Llv(Low, 10);
            MAXH10 = AFHL.Hhv(High, 10);
            AVGH21 = AFAvg.Ma(High, 21);
            AVGL21 = AFAvg.Ma(Low, 21);
            MINL5 = AFHL.Llv(Low, 5);
            BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

            DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);




     

            //////////////////////////////price 
            MAXC20 = AFHL.Hhv(Close, 20);
            AVGC40 = AFAvg.Ma(Close, 40);
            AVGH5 = AFAvg.Ma(High, 5);
            AVGL5 = AFAvg.Ma(Low, 5);
            AVGH34 = AFAvg.Ma(High, 34);
            AVGL34 = AFAvg.Ma(Low, 43);
            MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            MAXH20 = AFHL.Hhv(High, 20);
            MINL20 = AFHL.Llv(Low, 20);
            MINL42 = AFHL.Llv(Low, 42);
            MAXH42 = AFHL.Hhv(High, 42);
            MINL21 = AFHL.Llv(Low, 21);

            //to be reveiewd 
            AVGV4 = AFAvg.Ma(Volume, 4);
            MINL3 = AFHL.Llv(Low, 3);
          var   oneDayreversal = ((L3 <= MINL21) & (L6 > L5) & (L5 > L4) & (L4 > L3) & (L2 > L3) & (L1 > L2) & (Low > L1));
          
         
            ///////////////////////
            AVGC20 = AFAvg.Ma(Close, 20);
            AVGC50 = AFAvg.Ma(Close, 50);
            MAXH3 = AFHL.Hhv(High, 3);
           Insideday = AFPattern.Inside();
            DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle4 =
                  AFMisc.WriteIf(TweezerTop, "Tweezer Top",
  AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
  AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
  AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
  AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
  AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

  AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
   AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
  AFMisc.WriteIf(EveningStar1, "Evening Star",
  AFMisc.WriteIf(MorningStar1, "Morning Star",
  AFMisc.WriteIf(Abovethestomach, "Above the stomach",
  AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
  AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


            var pricepattern4 =
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel4 =

            AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
            AFMisc.WriteIf(GapUpx, "Gap Up",
            AFMisc.WriteIf(GapDownx, "Gap Down ",
            AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
            AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
            AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
            AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
            AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


             AFMisc.WriteIf(HangingMan1, "Hanging Man",

              AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
            AFMisc.WriteIf(PiercingLine, "Piercing Line ",
             AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
            AFMisc.WriteIf(Doji1, "Doji",
             AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



             AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
             AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
            AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
             AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
             AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
            AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                       AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                       AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                        AFMisc.WriteIf(whitemarubozu, "White marubozu",
                        AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                        AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                        AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                        AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                        AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                        AFMisc.WriteIf(hammer1, "Hammer",
                        AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
             AFMisc.WriteIf(oneDayreversal, "One Day reversal",
            AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
            AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



            AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                        AFMisc.WriteIf(Insideday, "Inside Day", "No pattern"))))))))))))))))))))))))))))))))))));






            AFMisc.AddTextColumn(singlecandel4, "15min Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle4, "15min Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern4, "15min Price  Pattern", 5.6f, Color.Black, Color.White);
            AFTimeFrame.TimeFrameRestore();


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            AFTimeFrame.TimeFrameSet(TFInterval.InHourly ); // switch now to hourly 
            O1 = AFTools.Ref(Open, -1); O2 = AFTools.Ref(Open, -2); O3 = AFTools.Ref(Open, -3); O4 = AFTools.Ref(Open, -4); O5 = AFTools.Ref(Open, -5); O6 = AFTools.Ref(Open, -6); O7 = AFTools.Ref(Open, -7); O8 = AFTools.Ref(Open, -8); O9 = AFTools.Ref(Open, -9);

            H1 = AFTools.Ref(High, -1); H2 = AFTools.Ref(High, -2); H3 = AFTools.Ref(High, -3); H4 = AFTools.Ref(High, -4); H5 = AFTools.Ref(High, -5); H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); H7 = AFTools.Ref(High, -7); H8 = AFTools.Ref(High, -8); H9 = AFTools.Ref(High, -9);

            L1 = AFTools.Ref(Low, -1); L2 = AFTools.Ref(Low, -2); L3 = AFTools.Ref(Low, -3); L4 = AFTools.Ref(Low, -4); L5 = AFTools.Ref(Low, -5); L6 = AFTools.Ref(Low, -6); L7 = AFTools.Ref(Low, -7); L8 = AFTools.Ref(Low, -8); L9 = AFTools.Ref(Low, -9);


            C1 = AFTools.Ref(Close, -1); C2 = AFTools.Ref(Close, -2); C3 = AFTools.Ref(Close, -3); C4 = AFTools.Ref(Close, -4); C5 = AFTools.Ref(Close, -5); C6 = AFTools.Ref(Close, -6); C7 = AFTools.Ref(Close, -7); C8 = AFTools.Ref(Close, -8); C9 = AFTools.Ref(Close, -9);

            /* BODY Colors*/
            WhiteCandle = Close >= Open;
            BlackCandle = Open > Close;
            B1 = O1 - C1;
            B2 = O2 - C2;
            B3 = O3 - C3;
            Avgbody = ((B1 + B2 + B3) / 3);
            XBODY = Avgbody * 3;

            BODY = Open - Close;





            /*Single candle Pattern */
            smallBodyMaximum = 0.0025f;//less than 0.25%
            LargeBodyMinimum = 0.01f;//greater than 1.0%
            smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            mediumBody = !largeBody & !smallBody;
            identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - ( MHT)
            Today is the maximum High in the last 5 days*/
            MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - ( MHY)
            Yesterday is the maximum High in the last 5 days*/
            MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - ( MLT)
            Today is the minimum Low in the last 5 days*/
            MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - ( MLY)
            Yesterday is the minimum Low in the last 5 days*/
            MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /* Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            //   DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            Close1 = (Open - Close) * (0.002f);
            Open1 = (Open * 0.002f);

            LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
            Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));



         


            PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            GapUpx = AFPattern.GapUp();
            GapDownx = AFPattern.GapDown();
            BigGapUp = Low > 1.01f * H1;
            BigGapDown = High < 0.99f * L1;
            HugeGapUp = Low > 1.02f * H1;
            HugeGapDown = High < 0.98f * L1;
            DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            percdiff = 5; /* peak detection threshold */
            fwdcheck = 5; /* forward validity check */
            mindistance = 10;
            validdiff = percdiff / 400;

            PK = AFPattern.Peak(High, percdiff, 1) == High;
            TR = AFPattern.Trough(Low, percdiff, 1) == Low;


            x = AFAvg.Cum(1);
            XPK1 = AFTools.ValueWhen(PK, x, 1);
            XPK2 = AFTools.ValueWhen(PK, x, 2);
            xTR1 = AFTools.ValueWhen(TR, x, 1);
            xTr2 = AFTools.ValueWhen(TR, x, 2);

            peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            MINO10 = AFHL.Llv(Open, 10);
            AVGH10 = AFAvg.Ma(High, 10);
            AVGL10 = AFAvg.Ma(Low, 10);
            MAXO10 = AFHL.Hhv(Open, 10);
            MINL10 = AFHL.Llv(Low, 10);
            MAXH10 = AFHL.Hhv(High, 10);
            AVGH21 = AFAvg.Ma(High, 21);
            AVGL21 = AFAvg.Ma(Low, 21);
            MINL5 = AFHL.Llv(Low, 5);
            BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

            DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);


            //////////////////////////////price 
            MAXC20 = AFHL.Hhv(Close, 20);
            AVGC40 = AFAvg.Ma(Close, 40);
            AVGH5 = AFAvg.Ma(High, 5);
            AVGL5 = AFAvg.Ma(Low, 5);
            AVGH34 = AFAvg.Ma(High, 34);
            AVGL34 = AFAvg.Ma(Low, 43);
            MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            MAXH20 = AFHL.Hhv(High, 20);
            MINL20 = AFHL.Llv(Low, 20);
            MINL42 = AFHL.Llv(Low, 42);
            MAXH42 = AFHL.Hhv(High, 42);
            MINL21 = AFHL.Llv(Low, 21);

           
            //to be reveiewd 
            AVGV4 = AFAvg.Ma(Volume, 4);
            MINL3 = AFHL.Llv(Low, 3);
           
            ///////////////////////
            AVGC20 = AFAvg.Ma(Close, 20);
            AVGC50 = AFAvg.Ma(Close, 50);
            MAXH3 = AFHL.Hhv(High, 3);
           Insideday = AFPattern.Inside();
            DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle5 =
                  AFMisc.WriteIf(TweezerTop, "Tweezer Top",
  AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
  AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
  AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
  AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
  AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

  AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
   AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
  AFMisc.WriteIf(EveningStar1, "Evening Star",
  AFMisc.WriteIf(MorningStar1, "Morning Star",
  AFMisc.WriteIf(Abovethestomach, "Above the stomach",
  AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
  AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


            var pricepattern5 =
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel5 =

            AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
            AFMisc.WriteIf(GapUpx, "Gap Up",
            AFMisc.WriteIf(GapDownx, "Gap Down ",
            AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
            AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
            AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
            AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
            AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


             AFMisc.WriteIf(HangingMan1, "Hanging Man",

              AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
            AFMisc.WriteIf(PiercingLine, "Piercing Line ",
             AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
            AFMisc.WriteIf(Doji1, "Doji",
             AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



             AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
             AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
            AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
             AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
             AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
            AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                       AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                       AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                        AFMisc.WriteIf(whitemarubozu, "White marubozu",
                        AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                        AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                        AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                        AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                        AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                        AFMisc.WriteIf(hammer1, "Hammer",
                        AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
             AFMisc.WriteIf(oneDayreversal, "One Day reversal",
            AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
            AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



            AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                        AFMisc.WriteIf(Insideday, "Inside Day", "No pattern"))))))))))))))))))))))))))))))))))));






            AFMisc.AddTextColumn(singlecandel5, "Hourly Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle5, "Hourly  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern5, "Hourly Price  Pattern", 5.6f, Color.Black, Color.White);
            AFTimeFrame.TimeFrameRestore();


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            AFTimeFrame.TimeFrameSet(TFInterval.InDaily); // switch now to hourly 
            O1 = AFTools.Ref(Open, -1); O2 = AFTools.Ref(Open, -2); O3 = AFTools.Ref(Open, -3); O4 = AFTools.Ref(Open, -4); O5 = AFTools.Ref(Open, -5); O6 = AFTools.Ref(Open, -6); O7 = AFTools.Ref(Open, -7); O8 = AFTools.Ref(Open, -8); O9 = AFTools.Ref(Open, -9);

            H1 = AFTools.Ref(High, -1); H2 = AFTools.Ref(High, -2); H3 = AFTools.Ref(High, -3); H4 = AFTools.Ref(High, -4); H5 = AFTools.Ref(High, -5); H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); H7 = AFTools.Ref(High, -7); H8 = AFTools.Ref(High, -8); H9 = AFTools.Ref(High, -9);

            L1 = AFTools.Ref(Low, -1); L2 = AFTools.Ref(Low, -2); L3 = AFTools.Ref(Low, -3); L4 = AFTools.Ref(Low, -4); L5 = AFTools.Ref(Low, -5); L6 = AFTools.Ref(Low, -6); L7 = AFTools.Ref(Low, -7); L8 = AFTools.Ref(Low, -8); L9 = AFTools.Ref(Low, -9);


            C1 = AFTools.Ref(Close, -1); C2 = AFTools.Ref(Close, -2); C3 = AFTools.Ref(Close, -3); C4 = AFTools.Ref(Close, -4); C5 = AFTools.Ref(Close, -5); C6 = AFTools.Ref(Close, -6); C7 = AFTools.Ref(Close, -7); C8 = AFTools.Ref(Close, -8); C9 = AFTools.Ref(Close, -9);

            /* BODY Colors*/
            WhiteCandle = Close >= Open;
            BlackCandle = Open > Close;
            B1 = O1 - C1;
            B2 = O2 - C2;
            B3 = O3 - C3;
            Avgbody = ((B1 + B2 + B3) / 3);
            XBODY = Avgbody * 3;

            BODY = Open - Close;





            /*Single candle Pattern */
            smallBodyMaximum = 0.0025f;//less than 0.25%
            LargeBodyMinimum = 0.01f;//greater than 1.0%
            smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            mediumBody = !largeBody & !smallBody;
            identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - ( MHT)
            Today is the maximum High in the last 5 days*/
            MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - ( MHY)
            Yesterday is the maximum High in the last 5 days*/
            MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - ( MLT)
            Today is the minimum Low in the last 5 days*/
            MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - ( MLY)
            Yesterday is the minimum Low in the last 5 days*/
            MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /* Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            //   DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            Close1 = (Open - Close) * (0.002f);
            Open1 = (Open * 0.002f);

            LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
            Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));



        


            PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            GapUpx = AFPattern.GapUp();
            GapDownx = AFPattern.GapDown();
            BigGapUp = Low > 1.01f * H1;
            BigGapDown = High < 0.99f * L1;
            HugeGapUp = Low > 1.02f * H1;
            HugeGapDown = High < 0.98f * L1;
            DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            percdiff = 5; /* peak detection threshold */
            fwdcheck = 5; /* forward validity check */
            mindistance = 10;
            validdiff = percdiff / 400;

            PK = AFPattern.Peak(High, percdiff, 1) == High;
            TR = AFPattern.Trough(Low, percdiff, 1) == Low;


            x = AFAvg.Cum(1);
            XPK1 = AFTools.ValueWhen(PK, x, 1);
            XPK2 = AFTools.ValueWhen(PK, x, 2);
            xTR1 = AFTools.ValueWhen(TR, x, 1);
            xTr2 = AFTools.ValueWhen(TR, x, 2);

            peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            MINO10 = AFHL.Llv(Open, 10);
            AVGH10 = AFAvg.Ma(High, 10);
            AVGL10 = AFAvg.Ma(Low, 10);
            MAXO10 = AFHL.Hhv(Open, 10);
            MINL10 = AFHL.Llv(Low, 10);
            MAXH10 = AFHL.Hhv(High, 10);
            AVGH21 = AFAvg.Ma(High, 21);
            AVGL21 = AFAvg.Ma(Low, 21);
            MINL5 = AFHL.Llv(Low, 5);
            BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

            DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);




            //////////////////////////////price 
            MAXC20 = AFHL.Hhv(Close, 20);
            AVGC40 = AFAvg.Ma(Close, 40);
            AVGH5 = AFAvg.Ma(High, 5);
            AVGL5 = AFAvg.Ma(Low, 5);
            AVGH34 = AFAvg.Ma(High, 34);
            AVGL34 = AFAvg.Ma(Low, 43);
            MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            MAXH20 = AFHL.Hhv(High, 20);
            MINL20 = AFHL.Llv(Low, 20);
            MINL42 = AFHL.Llv(Low, 42);
            MAXH42 = AFHL.Hhv(High, 42);
            MINL21 = AFHL.Llv(Low, 21);

          
            ////////////////////////////////////
            //to be reveiewd 
            AVGV4 = AFAvg.Ma(Volume, 4);
            MINL3 = AFHL.Llv(Low, 3);
           
         
            ///////////////////////
            AVGC20 = AFAvg.Ma(Close, 20);
            AVGC50 = AFAvg.Ma(Close, 50);
            MAXH3 = AFHL.Hhv(High, 3);
           Insideday = AFPattern.Inside();
            DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle6=
                  AFMisc.WriteIf(TweezerTop, "Tweezer Top",
  AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
  AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
  AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
  AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
  AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

  AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
   AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
  AFMisc.WriteIf(EveningStar1, "Evening Star",
  AFMisc.WriteIf(MorningStar1, "Morning Star",
  AFMisc.WriteIf(Abovethestomach, "Above the stomach",
  AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
  AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


            var pricepattern6=
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel6 =

            AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
            AFMisc.WriteIf(GapUpx, "Gap Up",
            AFMisc.WriteIf(GapDownx, "Gap Down ",
            AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
            AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
            AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
            AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
            AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


             AFMisc.WriteIf(HangingMan1, "Hanging Man",

              AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
            AFMisc.WriteIf(PiercingLine, "Piercing Line ",
             AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
            AFMisc.WriteIf(Doji1, "Doji",
             AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



             AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
             AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
            AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
             AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
             AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
            AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                       AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                       AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                        AFMisc.WriteIf(whitemarubozu, "White marubozu",
                        AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                        AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                        AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                        AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                        AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                        AFMisc.WriteIf(hammer1, "Hammer",
                        AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
             AFMisc.WriteIf(oneDayreversal, "One Day reversal",
            AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
            AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



            AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                        AFMisc.WriteIf(Insideday, "Inside Day", "No pattern"))))))))))))))))))))))))))))))))))));






            AFMisc.AddTextColumn(singlecandel6, "Daily Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle6, "Daily  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern6, "Daily Price  Pattern", 5.6f, Color.Black, Color.White);
            AFTimeFrame.TimeFrameRestore();


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            
            
            AFTimeFrame.TimeFrameSet(TFInterval.InWeekly ); // switch now to hourly 
             O1 = AFTools.Ref(Open, -1);  O2 = AFTools.Ref(Open, -2);  O3 = AFTools.Ref(Open, -3);  O4 = AFTools.Ref(Open, -4);  O5 = AFTools.Ref(Open, -5);  O6 = AFTools.Ref(Open, -6);  O7 = AFTools.Ref(Open, -7);  O8 = AFTools.Ref(Open, -8);  O9 = AFTools.Ref(Open, -9);

             H1 = AFTools.Ref(High, -1);  H2 = AFTools.Ref(High, -2);  H3 = AFTools.Ref(High, -3);  H4 = AFTools.Ref(High, -4);  H5 = AFTools.Ref(High, -5);  H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6);  H7 = AFTools.Ref(High, -7);  H8 = AFTools.Ref(High, -8);  H9 = AFTools.Ref(High, -9);

             L1 = AFTools.Ref(Low, -1);  L2 = AFTools.Ref(Low, -2);  L3 = AFTools.Ref(Low, -3);  L4 = AFTools.Ref(Low, -4);  L5 = AFTools.Ref(Low, -5);  L6 = AFTools.Ref(Low, -6);  L7 = AFTools.Ref(Low, -7);  L8 = AFTools.Ref(Low, -8);  L9 = AFTools.Ref(Low, -9);


             C1 = AFTools.Ref(Close, -1);  C2 = AFTools.Ref(Close, -2);  C3 = AFTools.Ref(Close, -3);  C4 = AFTools.Ref(Close, -4);  C5 = AFTools.Ref(Close, -5);  C6 = AFTools.Ref(Close, -6);  C7 = AFTools.Ref(Close, -7);  C8 = AFTools.Ref(Close, -8);  C9 = AFTools.Ref(Close, -9);

            /* BODY Colors*/
             WhiteCandle = Close >= Open;
             BlackCandle = Open > Close;
             B1 = O1 - C1;
             B2 = O2 - C2;
             B3 = O3 - C3;
             Avgbody = ((B1 + B2 + B3) / 3);
             XBODY = Avgbody * 3;

             BODY = Open - Close;





            /*Single candle Pattern */
             smallBodyMaximum = 0.0025f;//less than 0.25%
             LargeBodyMinimum = 0.01f;//greater than 1.0%
             smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
             largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
             mediumBody = !largeBody & !smallBody;
             identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
             realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
             smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
             smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
             largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
             largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
             upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

             downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
            AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - ( MHT)
            Today is the maximum High in the last 5 days*/
             MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - ( MHY)
            Yesterday is the maximum High in the last 5 days*/
             MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - ( MLT)
            Today is the minimum Low in the last 5 days*/
             MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - ( MLY)
            Yesterday is the minimum Low in the last 5 days*/
             MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /* Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            //   DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



             ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
             ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
             Close1 = (Open - Close) * (0.002f);
             Open1 = (Open * 0.002f);

             LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
             LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
             Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
             whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
             blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

             marubozuclosingblack = BlackCandle & High > Open & Low == Close;
             marubozuopeningblack = BlackCandle & High == Open & Low < Close;
             marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
             marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
             BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
             WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


             hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
             InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
             HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
             ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
             BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
             BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
             BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
             BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
             BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
             BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));





             PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

             EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
             MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
             MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
             GapUpx = AFPattern.GapUp();
             GapDownx = AFPattern.GapDown();
             BigGapUp = Low > 1.01f * H1;
             BigGapDown = High < 0.99f * L1;
             HugeGapUp = Low > 1.02f * H1;
             HugeGapDown = High < 0.98f * L1;
             DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
             DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

             consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
             consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

             consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
             consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
             Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
             Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
             TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
             tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
             percdiff = 5; /* peak detection threshold */
             fwdcheck = 5; /* forward validity check */
             mindistance = 10;
             validdiff = percdiff / 400;

             PK = AFPattern.Peak(High, percdiff, 1) == High;
             TR = AFPattern.Trough(Low, percdiff, 1) == Low;


             x = AFAvg.Cum(1);
             XPK1 = AFTools.ValueWhen(PK, x, 1);
             XPK2 = AFTools.ValueWhen(PK, x, 2);
             xTR1 = AFTools.ValueWhen(TR, x, 1);
             xTr2 = AFTools.ValueWhen(TR, x, 2);

             peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
             Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

             doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
             doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
             MINO10 = AFHL.Llv(Open, 10);
             AVGH10 = AFAvg.Ma(High, 10);
             AVGL10 = AFAvg.Ma(Low, 10);
             MAXO10 = AFHL.Hhv(Open, 10);
             MINL10 = AFHL.Llv(Low, 10);
             MAXH10 = AFHL.Hhv(High, 10);
             AVGH21 = AFAvg.Ma(High, 21);
             AVGL21 = AFAvg.Ma(Low, 21);
             MINL5 = AFHL.Llv(Low, 5);
             BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
             BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
             BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
             BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

             DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);





            
            //////////////////////////////price 
             MAXC20 = AFHL.Hhv(Close, 20);
             AVGC40 = AFAvg.Ma(Close, 40);
             AVGH5 = AFAvg.Ma(High, 5);
             AVGL5 = AFAvg.Ma(Low, 5);
             AVGH34 = AFAvg.Ma(High, 34);
             AVGL34 = AFAvg.Ma(Low, 43);
             MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
             MAXH20 = AFHL.Hhv(High, 20);
             MINL20 = AFHL.Llv(Low, 20);
             MINL42 = AFHL.Llv(Low, 42);
             MAXH42 = AFHL.Hhv(High, 42);
             MINL21 = AFHL.Llv(Low, 21);

            

            ////////////////////////////////////
            //to be reveiewd 
             AVGV4 = AFAvg.Ma(Volume, 4);
             MINL3 = AFHL.Llv(Low, 3);
         
            ///////////////////////
             AVGC20 = AFAvg.Ma(Close, 20);
             AVGC50 = AFAvg.Ma(Close, 50);
             MAXH3 = AFHL.Hhv(High, 3);
             Insideday = AFPattern.Inside();
             DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


            var doublecandle1 =
                  AFMisc.WriteIf(TweezerTop, "Tweezer Top",
  AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
  AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
  AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
  AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
  AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

  AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
   AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
  AFMisc.WriteIf(EveningStar1, "Evening Star",
  AFMisc.WriteIf(MorningStar1, "Morning Star",
  AFMisc.WriteIf(Abovethestomach, "Above the stomach",
  AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
  AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


             var pricepattern1 =
             AFMisc.WriteIf(doubletop, "Double top ",
             AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var   singlecandel1 =

            AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
            AFMisc.WriteIf(GapUpx, "Gap Up",
            AFMisc.WriteIf(GapDownx, "Gap Down ",
            AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
            AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
            AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
            AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
            AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


             AFMisc.WriteIf(HangingMan1, "Hanging Man",

              AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
            AFMisc.WriteIf(PiercingLine, "Piercing Line ",
             AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
            AFMisc.WriteIf(Doji1, "Doji",
             AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



             AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
             AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
            AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
             AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
             AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
            AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                       AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                       AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                        AFMisc.WriteIf(whitemarubozu, "White marubozu",
                        AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                        AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                        AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                        AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                        AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                        AFMisc.WriteIf(hammer1, "Hammer",
                        AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

            AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
             AFMisc.WriteIf(oneDayreversal, "One Day reversal",
            AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
            AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



            AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                        AFMisc.WriteIf(Insideday, "Inside Day", "No pattern"))))))))))))))))))))))))))))))))))));






            AFMisc.AddTextColumn(singlecandel1, "Weekly Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle1, "Weekly  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern1, "Weekly Price  Pattern", 5.6f, Color.Black, Color.White);
            AFTimeFrame.TimeFrameRestore();


            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            AFTimeFrame.TimeFrameSet(TFInterval.InMonthly ); // switch now to hourly 
            O1 = AFTools.Ref(Open, -1); O2 = AFTools.Ref(Open, -2); O3 = AFTools.Ref(Open, -3); O4 = AFTools.Ref(Open, -4); O5 = AFTools.Ref(Open, -5); O6 = AFTools.Ref(Open, -6); O7 = AFTools.Ref(Open, -7); O8 = AFTools.Ref(Open, -8); O9 = AFTools.Ref(Open, -9);

            H1 = AFTools.Ref(High, -1); H2 = AFTools.Ref(High, -2); H3 = AFTools.Ref(High, -3); H4 = AFTools.Ref(High, -4); H5 = AFTools.Ref(High, -5); H6 = AFTools.Ref(High, -6); H6 = AFTools.Ref(High, -6); H7 = AFTools.Ref(High, -7); H8 = AFTools.Ref(High, -8); H9 = AFTools.Ref(High, -9);

            L1 = AFTools.Ref(Low, -1); L2 = AFTools.Ref(Low, -2); L3 = AFTools.Ref(Low, -3); L4 = AFTools.Ref(Low, -4); L5 = AFTools.Ref(Low, -5); L6 = AFTools.Ref(Low, -6); L7 = AFTools.Ref(Low, -7); L8 = AFTools.Ref(Low, -8); L9 = AFTools.Ref(Low, -9);


            C1 = AFTools.Ref(Close, -1); C2 = AFTools.Ref(Close, -2); C3 = AFTools.Ref(Close, -3); C4 = AFTools.Ref(Close, -4); C5 = AFTools.Ref(Close, -5); C6 = AFTools.Ref(Close, -6); C7 = AFTools.Ref(Close, -7); C8 = AFTools.Ref(Close, -8); C9 = AFTools.Ref(Close, -9);

            /* BODY Colors*/
            WhiteCandle = Close >= Open;
            BlackCandle = Open > Close;
            B1 = O1 - C1;
            B2 = O2 - C2;
            B3 = O3 - C3;
            Avgbody = ((B1 + B2 + B3) / 3);
            XBODY = Avgbody * 3;

            BODY = Open - Close;





            /*Single candle Pattern */
            smallBodyMaximum = 0.0025f;//less than 0.25%
            LargeBodyMinimum = 0.01f;//greater than 1.0%
            smallBody = (Open >= Close * (1 - smallBodyMaximum) & WhiteCandle) | (Close >= Open * (1 - smallBodyMaximum) & BlackCandle);
            largeBody = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) | Close <= Open * (1 - LargeBodyMinimum) & BlackCandle;
            mediumBody = !largeBody & !smallBody;
            identicalBodies = AFMath.Abs(AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) - AFMath.Abs(Open - Close)) < AFMath.Abs(Open - Close) * smallBodyMaximum;
            realBodySize = AFMath.Abs(Open - Close);



            /*Shadows*/
            smallUpperShadow = (WhiteCandle & High <= Close * (1 + smallBodyMaximum)) | (BlackCandle & High <= Open * (1 + smallBodyMaximum));
            smallLowerShadow = (WhiteCandle & Low >= Open * (1 - smallBodyMaximum)) | (BlackCandle & Low >= Close * (1 - smallBodyMaximum));
            largeUpperShadow = (WhiteCandle & High >= Close * (1 + LargeBodyMinimum)) | (BlackCandle & High >= Open * (1 + LargeBodyMinimum));
            largeLowerShadow = (WhiteCandle & Low <= Open * (1 - LargeBodyMinimum)) | (BlackCandle & Low <= Close * (1 - LargeBodyMinimum));

            /*Gaps*/
            upGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Open > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Close > AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Open > AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Close > AFTools.Ref(Close, -1), 1, 0))));

            downGap = AFTools.Iif(AFTools.Ref(BlackCandle, -1) & WhiteCandle & Close < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(BlackCandle, -1) & BlackCandle & Open < AFTools.Ref(Close, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & WhiteCandle & Close < AFTools.Ref(Open, -1), 1,
           AFTools.Iif(AFTools.Ref(WhiteCandle, -1) & BlackCandle & Open < AFTools.Ref(Open, -1), 1, 0))));




            /*Maximum High Today - ( MHT)
            Today is the maximum High in the last 5 days*/
            MHT = AFHL.Hhv(High, 5) == High;

            /*Maximum High Yesterday - ( MHY)
            Yesterday is the maximum High in the last 5 days*/
            MHY = AFHL.Hhv(High, 5) == AFTools.Ref(High, -1);

            /*Minimum Low Today - ( MLT)
            Today is the minimum Low in the last 5 days*/
            MLT = AFHL.Llv(Low, 5) == Low;

            /*Minimum Low Yesterday - ( MLY)
            Yesterday is the minimum Low in the last 5 days*/
            MLY = AFHL.Llv(Low, 5) == AFTools.Ref(Low, -1);

            /* Doji1 definitions*/

            /*Doji1 Today - (DT)*/
            //   DT = AFMath.Abs(Close - Open) <= (Close * smallBodyMaximum) | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            /* Doji1 Yesterday - (DY)*/
            //   DY = AFMath.Abs(AFTools.Ref(Close, -1) - AFTools.Ref(Open, -1)) <= AFTools.Ref(Close, -1) * smallBodyMaximum | AFMath.Abs(AFTools.Ref(Open, -1) - AFTools.Ref(Close, -1)) <= (AFTools.Ref(High, -1) - AFTools.Ref(Low, -1)) * 0.1f;



            ShortWhitecandle = ((Close > Open) & ((High - Low) > (3 * (Close - Open))));
            ShortblackCandle = ((Open > Close) & ((High - Low) > (3 * (Open - Close))));
            Close1 = (Open - Close) * (0.002f);
            Open1 = (Open * 0.002f);

            LongblackCandle = (Close <= Open * (1 - LargeBodyMinimum) & BlackCandle) & (Open > Close) & ((Open - Close) / (.001f + High - Low) > .6f);
            LongwhiteCandle = (Close >= Open * (1 + LargeBodyMinimum) & WhiteCandle) & ((Close > Open) & ((Close - Open) / (.001f + High - Low) > .6f));
            Doji1 = Open == Close | (AFMath.Abs(Open - Close) <= ((High - Low) * 0.1f));

            //almost equal needs to be reviewd and implemented 
            whitemarubozu = ((Close > Open) & AFMisc.AlmostEqual(High, Close, 5) & AFMisc.AlmostEqual(Open, Low, 5));
            blackmarubozu = ((Open > Close) & (High == Open) & (Close == Low));

            marubozuclosingblack = BlackCandle & High > Open & Low == Close;
            marubozuopeningblack = BlackCandle & High == Open & Low < Close;
            marubozuclosingwhite = WhiteCandle & High == Close & Open > Low;
            marubozuopeningwhite = WhiteCandle & High > Close & Open == Low;
            BlackSpinningTop = ((Open > Close) & ((High - Low) > (3 * (Open - Close))) & (((High - Open) / (0.001f + High - Low)) < 0.4f) & (((Close - Low) / (0.001f + High - Low)) < 0.4f));
            WhiteSpinningTop = ((Close > Open) & ((High - Low) > (3 * (Close - Open))) & (((High - Close) / (0.001f + High - Low)) < 0.4f) & (((Open - Low) / (0.001f + High - Low)) < 0.4f));


            hammer1 = (((High - Low) > 3 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) > 0.6f) & ((Open - Low) / (.001f + High - Low) > 0.6f));
            InvertedHammer1 = (((High - Low) > 3 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) > 0.6f) & ((High - Open) / (0.001f + High - Low) > 0.6f));
            HangingMan1 = (((High - Low) > 4 * (Open - Close)) & ((Close - Low) / (.001f + High - Low) >= 0.75f) & ((Open - Low) / (.001f + High - Low) >= 0.75f));
            ShootingStar1 = (((High - Low) > 4 * (Open - Close)) & ((High - Close) / (0.001f + High - Low) >= 0.75f) & ((High - Open) / (0.001f + High - Low) >= 0.75f));
            BearishEngulfing = ((C1 > O1) & (Open > Close) & (Open >= C1) & (O1 >= Close) & ((Open - Close) > (C1 - O1)));
            BullishEngulfing = ((O1 > C1) & (Close > Open) & (Close >= O1) & (C1 >= Open) & ((Close - Open) > (O1 - C1)));
            BearishEveningDojiStar = ((C2 > O2) & ((C2 - O2) / (0.001f + H2 - L2) > 0.6f) & (C2 < O1) & (C1 > O1) & ((H1 - L1) > (3 * (C1 - O1))) & (Open > Close) & (Open < O1));
            BullishMorningDojiStar = ((O2 > C2) & ((O2 - C2) / (0.001f + H2 - L2) > 0.6f) & (C2 > O1) & (O1 > C1) & ((H1 - L1) > (3 * (C1 - O1))) & (Close > Open) & (Open > O1));
            BullishHarami = ((O1 > C1) & (Close > Open) & (Close <= O1) & (C1 <= Open) & ((Close - Open) < (O1 - C1)));
            BearishHarami = ((C1 > O1) & (Open > Close) & (Open <= C1) & (O1 <= Close) & ((Open - Close) < (C1 - O1)));





            PiercingLine = ((C1 < O1) & (((O1 + C1) / 2) < Close) & (Open < Close) & (Open < C1) & (Close < O1) & ((Close - Open) / (0.001f + (High - Low)) > 0.6f));

            EveningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(WhiteCandle, -2) & AFTools.Ref(upGap, -1) & !AFTools.Ref(largeBody, -1) & BlackCandle & !smallBody & (MHT | MHY);
            MorningStar1 = AFTools.Ref(largeBody, -2) & AFTools.Ref(BlackCandle, -2) & AFTools.Ref(downGap, -1) & WhiteCandle & largeBody & Close > AFTools.Ref(Close, -2) & MLY;
            MATCHLOW = AFHL.Llv(Low, 8) == AFHL.Llv(Low, 2) & AFTools.Ref(Close, -1) <= AFTools.Ref(Open, -1) * .99f & AFMath.Abs(Close - AFTools.Ref(Close, -1)) <= Close * .0025f & Open > AFTools.Ref(Close, -1) & Open <= (High - ((High - Low) * .5f));
            GapUpx = AFPattern.GapUp();
            GapDownx = AFPattern.GapDown();
            BigGapUp = Low > 1.01f * H1;
            BigGapDown = High < 0.99f * L1;
            HugeGapUp = Low > 1.02f * H1;
            HugeGapDown = High < 0.98f * L1;
            DoubleGapUp = AFPattern.GapUp() & AFTools.Ref(AFPattern.GapUp(), -1);
            DoubleGapDown = AFPattern.GapDown() & AFTools.Ref(AFPattern.GapDown(), -1);

            consecutave5up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5);
            consecutave10up = (High > H1) & (H1 > H2) & (H2 > H3) & (H3 > H4) & (H4 > H5) & (H5 > H6) & (H6 > H7) & (H7 > H8) & (H8 > H9);

            consecutave5down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5);
            consecutave10down = (Low < L1) & (L1 < L2) & (L2 < L3) & (L3 < L4) & (L4 < L5) & (L5 < L6) & (L6 < L7) & (L7 < L8) & (L8 < L9);
            Abovethestomach = (O1 > C1) & (Close > Open) & Close >= AFStat.Median(C1, 1);
            Belowthestomch = LongwhiteCandle & Open < AFStat.Median(C1, 1) & Close <= AFStat.Median(C1, 1);
            TweezerTop = AFMath.Abs(High - AFTools.Ref(High, -1)) <= High * 0.0025f & Open > Close & (AFTools.Ref(Close, -1) > AFTools.Ref(Open, -1)) & (MHT | MHY);

            /*Tweezer Bottom*/
            tweezerBottom = (AFMath.Abs(Low - AFTools.Ref(Low, -1)) / Low < 0.0025f | AFMath.Abs(Low - AFTools.Ref(Low, -2)) / Low < 0.0025f) & Open < Close & (AFTools.Ref(Open, -1) > AFTools.Ref(Close, -1)) & (MLT | MLY);




            /* Detecting double tops & bottoms (come into view, by Isfandi)*/
            percdiff = 5; /* peak detection threshold */
            fwdcheck = 5; /* forward validity check */
            mindistance = 10;
            validdiff = percdiff / 400;

            PK = AFPattern.Peak(High, percdiff, 1) == High;
            TR = AFPattern.Trough(Low, percdiff, 1) == Low;


            x = AFAvg.Cum(1);
            XPK1 = AFTools.ValueWhen(PK, x, 1);
            XPK2 = AFTools.ValueWhen(PK, x, 2);
            xTR1 = AFTools.ValueWhen(TR, x, 1);
            xTr2 = AFTools.ValueWhen(TR, x, 2);

            peakdiff = AFTools.ValueWhen(PK, High, 1) / AFTools.ValueWhen(PK, High, 2);
            Troughdiff = AFTools.ValueWhen(TR, Low, 1) / AFTools.ValueWhen(TR, Low, 2);

            doubletop = PK & AFMath.Abs(peakdiff - 1) < validdiff & (XPK1 - XPK2) > mindistance & High > AFHL.Hhv(AFTools.Ref(High, fwdcheck), fwdcheck - 1);
            doubleBot = TR & AFMath.Abs(Troughdiff - 1) < validdiff & (xTR1 - xTr2) > mindistance & Low < AFHL.Llv(AFTools.Ref(Low, fwdcheck), fwdcheck - 1);



            ////////////////////////////////////////////////
            MINO10 = AFHL.Llv(Open, 10);
            AVGH10 = AFAvg.Ma(High, 10);
            AVGL10 = AFAvg.Ma(Low, 10);
            MAXO10 = AFHL.Hhv(Open, 10);
            MINL10 = AFHL.Llv(Low, 10);
            MAXH10 = AFHL.Hhv(High, 10);
            AVGH21 = AFAvg.Ma(High, 21);
            AVGL21 = AFAvg.Ma(Low, 21);
            MINL5 = AFHL.Llv(Low, 5);
            BullishBeltHold = (Open = MINO10) & (Open < L1) & (Close - Open) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (Open - Low) <= .01f * (High - Low) & (Close <= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 < C2) & (C2 < C3);
            BearishBeltHold = (Open = MAXO10) & (Open > H1) & (Open - Close) >= .7f * (High - Low) & (High - Low) >= 1.2f * (AVGH10 - AVGL10) & (High - Open) <= .01f * (High - Low) & (Close >= H1 - .5f * (H1 - L1)) & (H1 > L1) & (High > Low) & (C1 > C2) & (C2 < C3);
            BullishBreakaway = C4 < O4 & AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C3 < O3 & H3 < L4 & C2 < C3 & C1 < C2 & AFMath.Abs(Close - Open) > .6f * (High - Low) & Close > Open & Close > H3;
            BearishBreakaway = AFMath.Abs(C4 - O4) > .5f * (H4 - L4) & C4 > O4 & C3 > O3 & L3 > H4 & C2 > C3 & C1 > C2 & Close < Open & Low < H4 & High > L3;

            DojiDragonfly = AFMath.Abs(Open - Close) <= .02f * (High - Low) & (High - Close) <= .3f * (High - Low) & (High - Low) >= (AVGH10 - AVGL10) & (High > Low) & (Low = MINL10);
            //BullishDojiGravestone=abs(O-C)<=.01*(H-L) AND (H-C)>=.95*(H-L) AND (H>L) AND (L<=L1+.3*(H1-L1)) AND (H-L)>=(AVGH10-AVGL10);




          

            //////////////////////////////price 
            MAXC20 = AFHL.Hhv(Close, 20);
            AVGC40 = AFAvg.Ma(Close, 40);
            AVGH5 = AFAvg.Ma(High, 5);
            AVGL5 = AFAvg.Ma(Low, 5);
            AVGH34 = AFAvg.Ma(High, 34);
            AVGL34 = AFAvg.Ma(Low, 43);
            MAXH5 = AFHL.Hhv(High, 5);
            MAXH10 = AFHL.Hhv(High, 10);
            MAXH20 = AFHL.Hhv(High, 20);
            MINL20 = AFHL.Llv(Low, 20);
            MINL42 = AFHL.Llv(Low, 42);
            MAXH42 = AFHL.Hhv(High, 42);
            MINL21 = AFHL.Llv(Low, 21);

            //to be reveiewd 
            AVGV4 = AFAvg.Ma(Volume, 4);
            MINL3 = AFHL.Llv(Low, 3);
           
            ///////////////////////
            AVGC20 = AFAvg.Ma(Close, 20);
            AVGC50 = AFAvg.Ma(Close, 50);
            MAXH3 = AFHL.Hhv(High, 3);
           
            Insideday = AFPattern.Inside();
            DojiGapUp = C3 > O3 & O2 > O3 & C2 > O2 & O1 > O2 & Open > C1 & Open == Close & High > Close & Close > Open;
            //needs to be review
            //DojiGapdown=C3 < O3 AND O2 < O3 AND C2  < O2 AND O1 < O2 AND O <  C1 AND O = C AND H <  C AND C < O;

            /* Add AFInfo.Name in column*/


          var   doublecandle2 =
                AFMisc.WriteIf(TweezerTop, "Tweezer Top",
AFMisc.WriteIf(tweezerBottom, "Tweezer Bottom",
AFMisc.WriteIf(consecutave5down, "Consecutive 5 down",
AFMisc.WriteIf(consecutave10down, "Consecutive 10 down",
AFMisc.WriteIf(consecutave5up, "Consecutive 5 up",
AFMisc.WriteIf(consecutave10up, "Consecutive 10 up",

AFMisc.WriteIf(BullishMorningDojiStar, "Bullish Morning Doji Star ",
 AFMisc.WriteIf(BearishEveningDojiStar, "Bearish Evening Doji Star ",
AFMisc.WriteIf(EveningStar1, "Evening Star",
AFMisc.WriteIf(MorningStar1, "Morning Star",
AFMisc.WriteIf(Abovethestomach, "Above the stomach",
AFMisc.WriteIf(BullishBreakaway, "Bullish Breakaway",
AFMisc.WriteIf(BearishBreakaway, "Bearish Breakaway", "No pattern")))))))))))));


           var  pricepattern2 =
            AFMisc.WriteIf(doubletop, "Double top ",
            AFMisc.WriteIf(doubleBot, "Double bottom", "No pattern"));


            var singlecandel2 =

           AFMisc.WriteIf(MATCHLOW, "MATCH Low ",
           AFMisc.WriteIf(GapUpx, "Gap Up",
           AFMisc.WriteIf(GapDownx, "Gap Down ",
           AFMisc.WriteIf(BigGapUp, "Big Gap Up ",
           AFMisc.WriteIf(HugeGapUp, "Huge Gap Up ",
           AFMisc.WriteIf(HugeGapDown, "Huge Gap Down ",
           AFMisc.WriteIf(DoubleGapUp, "Double Gap Up ",
           AFMisc.WriteIf(DoubleGapDown, "Double Gap Down ",


            AFMisc.WriteIf(HangingMan1, "Hanging Man",

             AFMisc.WriteIf(BullishHarami, "Bullish Harami ",
           AFMisc.WriteIf(PiercingLine, "Piercing Line ",
            AFMisc.WriteIf(BearishHarami, "Bearish Harami ",
           AFMisc.WriteIf(Doji1, "Doji",
            AFMisc.WriteIf(DojiGapUp, "Doji Gap Up",



            AFMisc.WriteIf(BlackSpinningTop, "Black Spinning Top ",
            AFMisc.WriteIf(WhiteSpinningTop, "White Spinning Top ",
           AFMisc.WriteIf(ShootingStar1, "Shooting Star ",
            AFMisc.WriteIf(marubozuclosingwhite, "Marubozu closing white",
            AFMisc.WriteIf(marubozuopeningwhite, "Marubozu opening white",
           AFMisc.WriteIf(marubozuopeningblack, "Marubozu opening black",
                      AFMisc.WriteIf(marubozuclosingblack, "Marubozu closing black",
                      AFMisc.WriteIf(blackmarubozu, "Black marubozu",
                       AFMisc.WriteIf(whitemarubozu, "White marubozu",
                       AFMisc.WriteIf(ShortWhitecandle, "Short White candle",
                       AFMisc.WriteIf(ShortblackCandle, "Short black Candle",
                       AFMisc.WriteIf(LongwhiteCandle, "Long white Candle",
                       AFMisc.WriteIf(LongblackCandle, "Long Black Candle",
                       AFMisc.WriteIf(BearishEngulfing, "Bearish Engulfing",
                       AFMisc.WriteIf(hammer1, "Hammer",
                       AFMisc.WriteIf(InvertedHammer1, "Inverted hammer",

           AFMisc.WriteIf(BullishEngulfing, "Bullish Engulfing ",
            AFMisc.WriteIf(oneDayreversal, "One Day reversal",
           AFMisc.WriteIf(BullishBeltHold, "Bullish Belt Hold",
           AFMisc.WriteIf(BearishBeltHold, "Bearish Belt Hold",



           AFMisc.WriteIf(Belowthestomch, "Below the stomch",

                       AFMisc.WriteIf(Insideday, "Inside Day", "No pattern"))))))))))))))))))))))))))))))))))));






            AFMisc.AddTextColumn(singlecandel2, "Monthly Single  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(doublecandle2, "Monthly  Candle Pattern", 5.6f, Color.Black, Color.White);
            AFMisc.AddTextColumn(pricepattern2, "Monthly Price  Pattern", 5.6f, Color.Black, Color.White);
            AFTimeFrame.TimeFrameRestore();

            AFMisc.SectionBegin("Shubhalabha");
            if (AFTools.ParamToggle("Add Formula", "No,Yes", 0) == 1)
            {
                shubhascanner();
            }
            AFMisc.SectionEnd();
        }
      
        
    }
}
