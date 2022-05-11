public class GameContext
{
    private GameVarWrapper _Owner = new NullVarWrapper();
    private GameVarWrapper _Caster = new NullVarWrapper();
    private GameVarWrapper _Target = new NullVarWrapper();

    public GameContext()
    {
        _Owner = new NullVarWrapper();
        _Caster = new NullVarWrapper();
        _Target = new NullVarWrapper();
    }

    public GameContext(GameVarWrapper owner, GameVarWrapper caster, GameVarWrapper target)
    {
        _Owner = owner;
        _Caster = caster;
        _Target = target;
    }

    public GameVarWrapper GetOwner()
    {
        return _Owner;
    }

    public GameVarWrapper GetCaster()
    {
        return _Caster;
    }

    public GameVarWrapper GetTarget()
    {
        return _Target;
    }
}
