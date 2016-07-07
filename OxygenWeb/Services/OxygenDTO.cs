using Nethereum.ABI.FunctionEncoding.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OxygenWeb.Services
{
    public class OxygenDTO
    {
        [Function(Name = "assets")]
        public class AssetsInput
        {
            [Parameter("uint256")]
            public int index { get; set; }
        }

        [Function(Name = "assets")]
        [FunctionOutput]
        public class AssetsOutput
        {
            [Parameter("address", 1)]
            public string assetOwnerId { get; set; }

            [Parameter("uint256", 2)]
            public int assetId { get; set; }
        }

        [Function(Name = "assetShares")]
        public class SharesInput
        {
            [Parameter("uint256", 1)]
            public int id { get; set; }

            [Parameter("uint256", 2)]
            public int index { get; set; }
        }

        [Function(Name = "assetShares")]
        [FunctionOutput]
        public class SharesOutput
        {
            [Parameter("address", 1)]
            public string shareOwnerId { get; set; }

            [Parameter("uint256", 2)]
            public int assetValuePerc { get; set; }

            [Parameter("uint256", 3)]
            public int purchasePrice { get; set; }

            [Parameter("uint256", 4)]
            public int offerPrice { get; set; }

            [Parameter("uint256", 5)]
            public int creationDateTime { get; set; }

            [Parameter("uint256", 6)]
            public int purchaseDateTime { get; set; }
        }
    }
}