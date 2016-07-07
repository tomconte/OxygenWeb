contract OxygenContractV2 {
    
    struct Share {
        address shareOwnerId;
        uint assetValuePerc; // *100
        uint purchasePrice; // in cents
        uint offerPrice; // in cents
        uint creationDateTime;
        uint purchaseDateTime;
    }
    
    struct RealtyAsset {
        address assetOwnerId;
        uint assetId;
    }
    
    RealtyAsset[] public assets;
    mapping (uint => Share[]) public assetShares;
    
    function createRealtyAsset(address assetOwnerId, uint assetId) {
        assets.push(RealtyAsset(assetOwnerId, assetId));
    }
    
    function createShares(uint assetId, uint numberOfShares, uint shareAssetValuePerc) {
        // TODO: check the sender is the owner of the asset
        // TODO: check the total asset value perc is not over 100%
        for (uint i = 0; i < numberOfShares; i++) {
            assetShares[assetId].push(Share(msg.sender, shareAssetValuePerc, 0, 0, now, 0));
        }
    }

    function getNumberOfShares(uint assetId) returns(uint) {
        return assetShares[assetId].length;
    }

    function invalidateShares(uint assetId, uint numberOfShares, uint shareAssetValuePerc) {
        // TODO: check the sender is the owner of the asset
        Share[] shares = assetShares[assetId];
        // 1. Count matching shares
        uint[] memory shareIndices = new uint[](numberOfShares);
        uint j = 0;
        for (uint i=0; i<shares.length; i++) {
            if (shares[i].shareOwnerId==msg.sender && shares[i].assetValuePerc==shareAssetValuePerc) {
                shareIndices[j++] = i;
                if (j == numberOfShares) {
                    break;
                }
            }
        }
        // 2. If not enough shares, throw
        if (j < numberOfShares)
            throw;
        // 3. Delete the shares we earmarked
        for (uint k=shareIndices.length-1; k>=0; k--) {
            delete shares[shareIndices[k]];
        }
    }
    
    function changeOfferPrice(uint assetId, uint numberOfShares, uint shareAssetValuePerc, uint currentOfferPrice, uint newOfferPrice)
    {
        Share[] shares = assetShares[assetId];
        // 1. Count matching shares
        uint[] memory shareIndices = new uint[](numberOfShares);
        uint j = 0;
        for (uint i=0; i<shares.length; i++) {
            if (shares[i].shareOwnerId==msg.sender && shares[i].assetValuePerc==shareAssetValuePerc && shares[i].offerPrice == currentOfferPrice) {
                shareIndices[j++] = i;
                if (j == numberOfShares) {
                    break;
                }
            }
        }
        // 2. If not enough shares, throw
        if (j < numberOfShares)
            throw;
        // 3. Change the offer price for the shares we earmarked
        for (uint k=0; k<shareIndices.length; k++) {
            shares[shareIndices[k]].offerPrice = newOfferPrice;
        }
    }
    
    function purchaseShares(uint assetId, uint numberOfShares, uint shareAssetValuePerc, uint offerPrice, address currentOwnerId)
    {
        // Find the matching shares
        Share[] shares = assetShares[assetId];
        uint[] memory shareIndices = new uint[](numberOfShares);
        uint j = 0;
        for (uint i=0; i<shares.length; i++) {
            if (shares[i].shareOwnerId==currentOwnerId && shares[i].assetValuePerc==shareAssetValuePerc && shares[i].offerPrice == offerPrice) {
                shareIndices[j++] = i;
                if (j == numberOfShares) {
                    break;
                }
            }
        }
        // If not enough shares, throw
        if (j < numberOfShares)
            throw;
        // Change share owner
        // Set purchase price
        // Set purchase date
        // Set offer price to 0
        for (uint k=0; k<shareIndices.length; k++) {
            shares[shareIndices[k]].shareOwnerId = msg.sender;
            shares[shareIndices[k]].purchasePrice = offerPrice;
            shares[shareIndices[k]].purchaseDateTime = now;
            shares[shareIndices[k]].offerPrice = 0;
        }
    }
}
