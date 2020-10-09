﻿using System.Collections.Generic;

namespace EBInstPack
{
    class BRRFile
    {
        public int loopPoint = 0;
        public List<byte> data = new List<byte>();
        public string filename = string.Empty;
    }

    class BRRFunctions
    {
        public static int DecodeLoopPoint(byte[] fileData)
        {
            //This function takes in an AddMusicK-format BRR file and returns its loop point's numerical value.
            //Special thanks to Milon in the SMW Central discord for this explanation.
            //A BRR loop point is stored by taking the raw value, dividing it by 16, and multiplying it by 9.

            //For example:
            //In C700, Chrono Trigger's Choir sample shows up with a loop point of 2288.
            //2288 / 16 = 143
            //143 * 9 = 1287
            //1287 in hex is [05 07], which is then swapped as [07 05].
            //(Which is what you see if you open that file in a hex editor!)

            //So to do the inverse, we need to take the first two bytes of a file,
            //reverse them, divide the number by 9, and multiply it by 16.
            var amkLoopHeader = IsolateLoopPoint(fileData);
            return (HexHelpers.ByteArrayToInt(amkLoopHeader) / 9) * 16;
        }

        private static byte[] IsolateLoopPoint(byte[] fileData)
        {
            return new byte[] { fileData[0], fileData[1] };
        }

        public static List<byte> IsolateBRRdata(byte[] fileData)
        {
            var result = new List<byte>();

            for (int i = 2; i < fileData.Length; i++) //starting at 2 here to skip the loop point & return the rest of it
            {
                result.Add(fileData[i]); //Something tells me this could be done in a cleaner way...
            }
            return result;
        }

        public static BRRFile FindDuplicate(List<BRRFile> samples, string name)
        {
            foreach (var brr in samples)
            {
                if (brr.filename == name)
                    return brr;
            }

            return null; //If there's nothing there with that name, what do?
        }
    }
}
