public enum IGameVarType
{
    BOOL = 0,
    INT = 1,
    FLOAT = 2,

    CHARACTER = 10
}

public interface IGameVarWrapper
{
    public IGameVarType GetGameVarType();
}
