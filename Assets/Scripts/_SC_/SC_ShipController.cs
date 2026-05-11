using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class SC_ShipController : MonoBehaviour
{
    [Required] [SerializeField] protected SC_SpaceShipManager _ShipManager = null;

    void TODO_Move()
    {

    }

    void Shoot()
    {
        _ShipManager.RequestShoot();
    }
}
