using UnityEngine;

[CreateAssetMenu(fileName = "AllCharcters", menuName = "DataList/All Characters")]
public class AllCharactersDataList : DataAutoList<CharacterData, AllCharactersDataList>
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void RefreshList()
    {
        if (!(Inst is null))
        {
            Inst.RefreshDataList();
        }
    }
}
