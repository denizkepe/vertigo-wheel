using NUnit.Framework;

public class BombWeightPolicyTests
{
    [Test]
    public void SafeZonaYaklastikca_Gerilim_Artar()
    {
        var policy = new BombWeightPolicy(5, 6f);
        Assert.Greater(policy.GetZoneMultiplier(4), policy.GetZoneMultiplier(1));
    }

    [Test]
    public void ArtArda_Basarisizlik_BombayiAzaltir()
    {
        var policy = new BombWeightPolicy(5, 6f);
        float sifir = policy.GetBombMultiplier(4, 0);
        float uc = policy.GetBombMultiplier(4, 3);
        Assert.Less(uc, sifir);   // 3 basarisizlik sonrasi bomba daha dusuk
    }

    [Test]
    public void Merhamet_Tabanin_Altina_Inmez()
    {
        var policy = new BombWeightPolicy(5, 6f, 0.15f, 0.3f);
        Assert.AreEqual(0.3f, policy.GetMercyMultiplier(100), 0.001f);
    }
}
