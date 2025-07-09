using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Landmark
{
    public Vector2Int Position;
    public abstract ELandmarkType Type { get; }

    public static Landmark CreateNewLandmark(LandmarkData data, Vector2Int position)
    {
        Landmark landmark;

        switch (data.Type)
        {
            case ELandmarkType.SimpleLandmark:
                landmark = new Landmark_Simple();
                break;
            case ELandmarkType.HiveSpawn:
                landmark = new Landmark_HiveSpawn();
                break;
            case ELandmarkType.Deposit:
                landmark = new Landmark_Deposit();
                break;
            default:
                Debug.LogError("Landmark not implemented");
                return null;
        }

        landmark.Position = position;
        landmark.Initialize(data);

        return landmark;
    }

    public abstract void Initialize(LandmarkData data);
    public abstract void Interract(CharacterManager character);
}

public class Landmark_Simple : Landmark
{
    public override ELandmarkType Type => ELandmarkType.SimpleLandmark;

    public override void Initialize(LandmarkData data)
    {
    }

    public override void Interract(CharacterManager character)
    {
        throw new System.NotImplementedException();
    }
}

public class Landmark_HiveSpawn : Landmark
{
    public override ELandmarkType Type => ELandmarkType.HiveSpawn;

    public override void Initialize(LandmarkData data)
    {
    }

    public override void Interract(CharacterManager character)
    {
        character.GetCharaInventory().GetItemList();
    }
}

public class Landmark_Deposit : Landmark
{
    public override ELandmarkType Type => ELandmarkType.Deposit;

    private InventoryItem DropableItem;

    public override void Initialize(LandmarkData data)
    {
        if (data is not LandmarkData_Deposit)
        {
            Debug.LogError("Wrong data");
            return;
        }

        LandmarkData_Deposit landmarkData = data as LandmarkData_Deposit;
        DropableItem = landmarkData.LootableItem;
    }

    public override void Interract(CharacterManager character)
    {
        character.GetCharaInventory().AddItem(DropableItem);
    }
}
