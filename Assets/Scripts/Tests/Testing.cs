using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FinalBlocktasy;
using UnityEngine;
using UnityEngine.TestTools;

public class Testing
{
    // A Test behaves as an ordinary method
    [Test]
    public void CurrentHP_tidak_lebih_dari_MaxHp()
    {
        var health = new Health(100);
        var heal = 200;

        health.SetHP(heal);

        Assert.AreEqual(health.maxHp, health.currentHp);
    }

    [Test]
    public void CurrentHP_tidak_kurang_dari_nol()
    {
        var health = new Health(100);
        var damage = -200;

        health.SetHP(damage);

        Assert.AreEqual(0, health.currentHp);
    }

    [Test]
    public void TurnGauge_tidak_lebih_dari_MaxGauge()
    {
        var turnGauge = new TurnGauge();
        var adds = 106;

        turnGauge.SetTurnGauge(adds);

        Assert.AreEqual(turnGauge.maxGauge, turnGauge.currentGauge);
    }

    [Test]
    public void TurnGauge_tidak_kurang_dari_nol()
    {
        var turnGauge = new TurnGauge();
        var subtract = -5;

        turnGauge.SetTurnGauge(subtract);

        Assert.AreEqual(0, turnGauge.currentGauge);
    }

    [Test]
    public void Level_pertambahan_exp_negatif()
    {
        var level = new Level();
        var additionInNegative = -5;

        level.AddXp(additionInNegative);

        Assert.AreEqual(0, level.currentXp);
    }

    [Test]
    public void Level_bertambah()
    {
        var level = new Level();
        var additonXp = 200;

        level.AddXp(additonXp);

        Assert.AreEqual(2, level.level);
    }
}
