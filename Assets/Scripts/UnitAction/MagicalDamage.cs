using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalDamage : IVisitor<UnitBase>
{
    private UnitBase unitSource;
    private ElementType element;
    private float amount;

    public MagicalDamage(UnitBase _unitSource, float _amount, ElementType _element)
    {
        unitSource = _unitSource;
        amount = _amount;
        element = _element;
    }

    public void Visit(UnitBase unit)
    {
        float dealt = amount;
        dealt = CalculateElementalRelation(dealt, element, unit);
        dealt -= Mathf.Round(dealt * unit.spellRes);

        if (BattleSceneController.Instance.isTargetAllEnemy)
        {
            dealt *= 0.7f;
        }

        BattleSceneController.Instance.ShowTextDamage((int)dealt, unit.transform.position);
        Debug.Log(unitSource.unitName + " Spell " + unit.unitName + " for " + dealt + " Damage.");

        BattleController.Instance.ApplyDamage(unit, (int)dealt);
    }

    private float CalculateElementalRelation(float _amount, ElementType _spellElementType, UnitBase _target)
    {
        switch (_spellElementType)
        {
            case ElementType.Fire:
                _amount += _amount * (unitSource.fireAtt - _target.fireRes);
                break;
            case ElementType.Water:
                _amount += _amount * (unitSource.waterAtt - _target.waterRes);
                break;
            case ElementType.Lightning:
                _amount += _amount * (unitSource.lightningAtt - _target.lightningRes);
                break;
            case ElementType.Earth:
                _amount += _amount * (unitSource.earthAtt - _target.earthRes);
                break;
            case ElementType.Wind:
                _amount += _amount * (unitSource.windAtt - _target.windRes);
                break;
            case ElementType.Ice:
                _amount += _amount * (unitSource.iceAtt - _target.iceRes);
                break;
            case ElementType.Holy:
                _amount += _amount * (unitSource.lightAtt - _target.lightRes);
                break;
        }
        return _amount;
    }

}