using NUnit.Framework;
using UnityEngine;

public class RewardWalletTests
{
    [Test]
    public void Add_AyniOdulu_Biriktirir()
    {
        var wallet = new RewardWallet();
        var cash = ScriptableObject.CreateInstance<RewardDefinition>();

        wallet.Add(cash, 100);
        wallet.Add(cash, 50);

        Assert.AreEqual(150, wallet.GetAmount(cash));
    }

    [Test]
    public void Clear_Cuzdani_Bosaltir()
    {
        var wallet = new RewardWallet();
        var cash = ScriptableObject.CreateInstance<RewardDefinition>();

        wallet.Add(cash, 100);
        wallet.Clear();

        Assert.AreEqual(0, wallet.GetAmount(cash));
    }
}
