using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Ziggurat
{
    public interface IUnitCreationObserver
    {
        void OnCreateUnit(Unit unit);
    }

    public interface IUnitOpponentManager
    {
        Unit GetNextOpponentFor(Unit seekingUnit);
        void OnUnitDied(Unit unit);
    }

    public class AllUnitsManager : MonoBehaviour, IUnitCreationObserver, IUnitOpponentManager
    {
        private List<Unit> _redUnits = new List<Unit>();
        private List<Unit> _greenUnits = new List<Unit>();
        private List<Unit> _blueUnits = new List<Unit>();

        public void OnCreateUnit(Unit unit)
        {
            switch (unit.Model.ArmyType)
            {
                case ArmyType.red:
                    _redUnits.Add(unit);
                    break;
                case ArmyType.green:
                    _greenUnits.Add(unit);
                    break;
                case ArmyType.blue:
                    _blueUnits.Add(unit);
                    break;
            }
            Debug.Log("Unit added:\n" + unit.Model.ToString());
        }

        public Unit GetNextOpponentFor(Unit seekingUnit)
        {
            var unitsForSeek = new List<Unit>();
            switch (seekingUnit.Model.ArmyType)
            {
                case ArmyType.red:
                    unitsForSeek.AddRange(_greenUnits);
                    unitsForSeek.AddRange(_blueUnits);
                    break;
                case ArmyType.green:
                    unitsForSeek.AddRange(_redUnits);
                    unitsForSeek.AddRange(_blueUnits);
                    break;
                case ArmyType.blue:
                    unitsForSeek.AddRange(_greenUnits);
                    unitsForSeek.AddRange(_redUnits);
                    break;
            }
            var opponent = unitsForSeek.OrderBy(unit =>
            {
                var distance = seekingUnit.transform.position - unit.transform.position;
                return distance.magnitude;
            }).FirstOrDefault();
            return opponent;
        }

        public void OnUnitDied(Unit unit)
        {
            switch (unit.Model.ArmyType)
            {
                case ArmyType.red:
                    _redUnits.Remove(unit);
                    break;
                case ArmyType.green:
                    _greenUnits.Remove(unit);
                    break;
                case ArmyType.blue:
                    _blueUnits.Remove(unit);
                    break;
            }
        }
    }
}