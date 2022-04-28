using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSpell : IVisitor<UnitBase>
{
    private UnitBase unitSource;
    private ElementType element;
    private int spellLevel;
    private float mod = 1.24f;
    private float modIncrease = 0.48f;

    public BasicSpell(UnitBase _unitSource, ElementType _element, int _spellLevel)
    {
        unitSource = _unitSource;
        element = _element;
        spellLevel = _spellLevel;
    }

    public void Visit(UnitBase unit)
    {
        float dealt = 50 * spellLevel;
        float _mod = mod + (modIncrease * spellLevel);
        MagicalDamage basicSpell = new MagicalDamage(unitSource, dealt + (_mod * unitSource.mag), element);
        unit.Accept(basicSpell);
    }
}
