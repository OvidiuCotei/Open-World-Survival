using UnityEngine;

namespace OvyKode
{
    public class BluePrint
    {
        public string itemName;
        public string req1;
        public string req2;
        public string req3;
        public int req1Amount;
        public int req2Amount;
        public int req3Amount;
        public int numOfRequirements;
        public int numberOfItemsToProduce;

        public BluePrint(string name, int producedItems, int reqNum, string r1, int r1Num, string r2, int r2Num/*, string r3, int r3Num*/)
        {
            itemName = name;
            numOfRequirements = reqNum;
            numberOfItemsToProduce = producedItems;
            req1 = r1;
            req2 = r2;
            //req3 = r3;
            req1Amount = r1Num;
            req2Amount = r2Num;
            //req3Amount = r3Num;
        }
    }
}