using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TC.GPConquest.Common
{
    /*
     * This class is used as template in order to easily transfer
     * informations in the network trhough an array of bytes.
     * This class will be used by the ByteArray utility in order
     * to serialize/deserialize from/to CaptureTransferInfo/byteArray
     * **/
    [Serializable]
    public class CaptureTransferInfo
    {
        public uint uniqueId;
        public double captureTime;
    }

}

