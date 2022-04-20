using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalDamage : IVisitor<UnitBase>
{
    private UnitBase unitSource;
    private float amount;

    public PhysicalDamage(UnitBase _unitSource, float _amount)
    {
        unitSource = _unitSource;
        amount = _amount;
    }

    public void Visit(UnitBase unit)
    {
        const float CReduction = 0.06f;
        float targetDamageReduction = (CReduction * unit.def) / (1 + (CReduction * unit.def));
        amount -= Mathf.Round(amount * targetDamageReduction);

        BattleSceneController.Instance.ShowTextDamage((int)amount, unit.transform.position);
        Debug.Log(unitSource.unitName + " Attacking " + unit.unitName + " for " + amount + " Damage.");

        BattleController.Instance.ApplyDamage(unit, (int)amount);
    }
}
