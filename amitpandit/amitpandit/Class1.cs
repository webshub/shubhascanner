using System;
using AmiBroker;
using AmiBroker.PlugIn;
using AmiBroker.Utils;

namespace amitpandit
{
    public class Class1 : IndicatorBase
    {
        [ABMethod]
        public ATArray amitpanditFunc1(ATArray array, float period)
        {
            ATArray result = AFAvg.Ma(array, period);
            return result;
        }


         [ABMethod]
        public void abc()
        {
            AFMisc.SectionBegin("Shubhalabha");

//Taking user input 
var Voluserinput=AFTools.Param("Select Volume ",800000,1,8000000);
var Lastcloseuserinput=AFTools.Param("Select Last Close is Greater Than condition ",100,1,10000);
var ATRuserinput=AFTools.Param("Select AFInd.Atr is Greater Than condition ",10,1,10000);
var ATRbaruserinput=AFTools.Param("Select AFInd.Atr Bar configuration ",1,1,10000);
var EMA1shorttermuserinput=AFTools.Param("Select Short AFAvg.Ema value ",5,1,100);
var EMA2longtermuserinput=AFTools.Param("Select long AFAvg.Ema value ",26,1,100);
var BBrangeuserinput=AFTools.Param("BB-range ",15,1,100);
var BBwidthuserinput=AFTools.Param("BB-width ",2,1,100);
var Shorttermuserinput=AFTools.Param("Select Short term duration", 5,1,365);
var Longtermuserinput=AFTools.Param("Select Long term duration", 30,10,365);
var shortchangeuserinput=AFTools.Param("Select % change for Short term" ,3.81f,1,100);
var longchangeuserinput=AFTools.Param("Select % change for one long term", 2.49f,1,100);
var bbup=AFInd.BBandTop(Close, BBrangeuserinput, BBwidthuserinput );
var bbdiff=Close - bbup;


// Buy Sell condition

Buy =( Close >  Lastcloseuserinput & Close < AFAvg.Ema(Close, EMA1shorttermuserinput) &  Volume >Voluserinput & AFInd.Atr(ATRbaruserinput)>ATRuserinput & AFInd.BBandTop( Close, BBrangeuserinput, BBwidthuserinput ) > Close );
Sell = ( Close >  Lastcloseuserinput & Close < AFAvg.Ema(Close, EMA2longtermuserinput)  & Volume >Voluserinput & AFInd.Atr(ATRbaruserinput)>ATRuserinput & AFInd.BBandBot( Close, BBrangeuserinput, BBwidthuserinput ) < Close );


// Buy Sell condition backup to try anything.

//Buy =( Close >  Lastcloseuserinput AND Close < EMA(Close, EMA1shorttermuserinput)  AND   DROC > shortchangeuserinput AND WROC > longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandTop( Close, BBrangeuserinput, BBwidthuserinput ) > Close );
//Sell = ( Close >  Lastcloseuserinput AND Close < EMA(Close, EMA2longtermuserinput)  AND   DROC < shortchangeuserinput AND WROC < longchangeuserinput AND  Volume >Voluserinput AND ATR(ATRbaruserinput)>ATRuserinput AND BBandBot( Close, BBrangeuserinput, BBwidthuserinput ) < Close )

// Comment following two lines if you want to get signals without swing signals.

//Buy=ExRem(Buy,Sell);
//Sell=ExRem(Sell,Buy);

// Enable following to get all data without buy sell.
//Filter = 1;

Filter= Buy | Sell;
//Adding column to report


AFMisc.AddTextColumn( AFInfo.FullName(), "Company AFInfo.Name", 77 ,Color.Green);
AFMisc.AddColumn(Volume,"Last Volume ",1.2f,Color.Green);
AFMisc.AddColumn(Close,"Last Close  ",1.2f,Color.Green);

AFMisc.AddColumn(AFInd.Atr(ATRbaruserinput) ,"Last AFInd.Atr",1.2f,Color.Green);
AFMisc.AddColumn(AFAvg.Ema(Close, EMA1shorttermuserinput),"EMA1 Short term",1.2f, Color.Green);
AFMisc.AddColumn(AFAvg.Ema(Close, EMA2longtermuserinput),"EMA2 Long term",1.2f, Color.Green);
AFMisc.AddColumn(AFInd.BBandTop(Close, BBrangeuserinput, BBwidthuserinput ),"Bollinger band upper  ",1.2f,Color.Green );
AFMisc.AddColumn(bbdiff,"Diff bbup & Close   ",1.2f,Color.Green );

// This prints report with Buy and Sell.

AFMisc.AddColumn( Buy, "Buy", 1.2f,Color.Green  );
AFMisc.AddColumn(Sell, "Sell", 1.2f,Color.Green );

// This marks buy and sell on charts.



AFMisc.SectionEnd();
        }
    }
}
