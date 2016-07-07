using Nethereum.ABI;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static OxygenWeb.Services.OxygenDTO;

namespace OxygenWeb.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index(int id)
        {
            var web3 = new Nethereum.Web3.Web3();

            // Get contract reference
            // The Solidity file can be found here: https://gist.github.com/anonymous/47f27037cac99e4a0c3b146fe84ac49a
            // You can use the online Solidity compiler to compile it, get the ABI and deploy it:
            // https://ethereum.github.io/browser-solidity/

            var abi = "[{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"},{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"assetShares\",\"outputs\":[{\"name\":\"shareOwnerId\",\"type\":\"address\"},{\"name\":\"assetValuePerc\",\"type\":\"uint256\"},{\"name\":\"purchasePrice\",\"type\":\"uint256\"},{\"name\":\"offerPrice\",\"type\":\"uint256\"},{\"name\":\"creationDateTime\",\"type\":\"uint256\"},{\"name\":\"purchaseDateTime\",\"type\":\"uint256\"}],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetId\",\"type\":\"uint256\"},{\"name\":\"numberOfShares\",\"type\":\"uint256\"},{\"name\":\"shareAssetValuePerc\",\"type\":\"uint256\"},{\"name\":\"currentOfferPrice\",\"type\":\"uint256\"},{\"name\":\"newOfferPrice\",\"type\":\"uint256\"}],\"name\":\"changeOfferPrice\",\"outputs\":[],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetId\",\"type\":\"uint256\"},{\"name\":\"numberOfShares\",\"type\":\"uint256\"},{\"name\":\"shareAssetValuePerc\",\"type\":\"uint256\"},{\"name\":\"offerPrice\",\"type\":\"uint256\"},{\"name\":\"currentOwnerId\",\"type\":\"address\"}],\"name\":\"purchaseShares\",\"outputs\":[],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetId\",\"type\":\"uint256\"},{\"name\":\"numberOfShares\",\"type\":\"uint256\"},{\"name\":\"shareAssetValuePerc\",\"type\":\"uint256\"}],\"name\":\"invalidateShares\",\"outputs\":[],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetId\",\"type\":\"uint256\"},{\"name\":\"numberOfShares\",\"type\":\"uint256\"},{\"name\":\"shareAssetValuePerc\",\"type\":\"uint256\"}],\"name\":\"createShares\",\"outputs\":[],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetId\",\"type\":\"uint256\"}],\"name\":\"getNumberOfShares\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"assets\",\"outputs\":[{\"name\":\"assetOwnerId\",\"type\":\"address\"},{\"name\":\"assetId\",\"type\":\"uint256\"}],\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"assetOwnerId\",\"type\":\"address\"},{\"name\":\"assetId\",\"type\":\"uint256\"}],\"name\":\"createRealtyAsset\",\"outputs\":[],\"type\":\"function\"}]";
            var contractAddress = "0xfa31bc162833b0d29c8dc5293dd0cdffdc78385e";
            var contract = web3.Eth.GetContract(abi, contractAddress);

            // Retrieve asset

            var assetsFunc = contract.GetFunction<AssetsInput>();
            var input = new AssetsInput { index = id };
            var assetOut = await assetsFunc.CallDeserializingToObjectAsync<AssetsOutput>(input);

            ViewBag.assetOwnerId = assetOut.assetOwnerId;
            ViewBag.assetId = assetOut.assetId;

            // Retrieve number of shares

            var nbSharesFunc = contract.GetFunction("getNumberOfShares");
            var nbShares = await nbSharesFunc.CallAsync<int>(assetOut.assetId);

            var assetSharesFunc = contract.GetFunction<SharesInput>();
            var shares = new List<String>();

            for (int i=0; i<nbShares; i++)
            {
                var shareIn = new SharesInput { id = assetOut.assetId, index = i };
                var shareOut = await assetSharesFunc.CallDeserializingToObjectAsync<SharesOutput>(shareIn);
                shares.Add("Share " + i + " for " + shareOut.assetValuePerc + "%");
            }

            ViewBag.shares = shares;

            return View();
        }

        public ActionResult Create(int id)
        {
            ViewBag.Message = "Create Shares.";

            return View();
        }

        public ActionResult Invalidate(int id)
        {
            ViewBag.Message = "Invalidate Shares.";

            return View();
        }
    }
}